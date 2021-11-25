using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    /// <summary>
    /// Support for reading, writing, decrypting and encrytping .dat and .tro files.
    /// </summary>
    public class HdrFormat
    {
        public const string Hdr = "HDR";
        public const string ActionRead = "Read";
        public const string ActionWrite = "Write";
        public const string ActionEncrypt = "Encrypt";
        public const string ActionDecrypt = "Decrypt";
        public const string ActionExtractFolder = "ExtractFolder";
        public const string ActionPackFolder = "PackFolder";

        private const int IndexBlockSize = 268;
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

        private static readonly IBufferProvider BufferProvider = new StreamBuffer();
        private static readonly ILogger _logger = LogProvider.Logger(typeof(HdrFormat));

        private static readonly List<string> IgnoreFiles = new List<string>()
        {
            ".ds_store",
            "hdr.report"
        };

        public HdrFormat()
        {
        }

        public event EventHandler<HdrProgressEventArgs> ProgressChanged;

        /// <summary>
        /// Creates a <see cref="HdrArchive"/> from a folder.
        /// For extracting an archive use the <see cref="Extract"/> method.
        /// </summary>
        public HdrArchive Read(string sourcePath)
        {
            byte[] hdrFile = Utils.ReadFile(sourcePath);
            IBuffer buffer = BufferProvider.Provide(hdrFile);
            buffer.SetPositionStart();
            HdrHeader header = ReadHeader(buffer);
            if (header == null)
            {
                _logger.Error($"Can not read header, this is not a hdr file or the file is broken. ({sourcePath})");
                return null;
            }

            List<HdrFile> files = new List<HdrFile>();
            int folderIndexStart = header.IndexOffset;
            int totalFiles = 0;
            int currentFile = 0;
            for (int i = 0; i < header.FolderCount; i++)
            {
                buffer.Position = folderIndexStart + i * IndexBlockSize;
                HdrIndex folderIndex = ReadIndex(buffer);
                buffer.Position = folderIndex.Offset;
                for (int j = 0; j < folderIndex.Length; j++)
                {
                    HdrIndex fileIndex = ReadIndex(buffer);
                    int offset = fileIndex.Offset;
                    int lenght = fileIndex.Length;
                    string fileExtension = "";
                    if (Path.HasExtension(fileIndex.Name))
                    {
                        fileExtension = Path.GetExtension(fileIndex.Name);
                    }

                    HdrFile file = new HdrFile();
                    file.FileExtension = fileExtension;
                    file.FileNameRaw = fileIndex.NameRaw;
                    file.HdrDirectoryPath = folderIndex.Name;
                    file.HdrFullPath = folderIndex.Name + fileIndex.Name;
                    file.Data = buffer.GetBytes(offset, lenght);
                    file.Offset = offset;
                    file.Length = lenght;
                    file.CryptoExtension = HdrFile.GetCryptoExtension(fileExtension);
                    files.Add(file);
                    OnProgressChanged(ActionRead, folderIndex.Name, totalFiles, currentFile++);
                }

                totalFiles += folderIndex.Length;
            }

            return new HdrArchive(files, header);
        }

        /// <summary>
        /// Write a <see cref="HdrArchive"/> to a folder.
        /// For creating an archive from an existing folder use the <see cref="Pack"/> method.
        /// </summary>
        public void Write(HdrArchive archive, string destinationPath)
        {
            Dictionary<string, List<HdrFile>> folderDictionary = new Dictionary<string, List<HdrFile>>();
            List<string> orderedKeys = new List<string>();
            foreach (HdrFile file in archive.Files)
            {
                if (archive.Header.ArchiveType == HdrArchiveType.Tro)
                {
                    string[] folderNameParts = file.HdrDirectoryPath.Split('\\');
                    string folderName = "";
                    for (int i = 0; i < folderNameParts.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(folderNameParts[i]))
                        {
                            folderName += folderNameParts[i] + '\\';
                            if (!folderDictionary.ContainsKey(folderName))
                            {
                                folderDictionary.Add(folderName, new List<HdrFile>());
                                orderedKeys.Add(folderName);
                            }
                        }
                    }
                }

                if (folderDictionary.ContainsKey(file.HdrDirectoryPath))
                {
                    folderDictionary[file.HdrDirectoryPath].Add(file);
                }
                else
                {
                    folderDictionary.Add(file.HdrDirectoryPath, new List<HdrFile>() {file});
                    orderedKeys.Add(file.HdrDirectoryPath);
                }
            }

            orderedKeys.Sort((s1, s2) => string.Compare(s1, s2, StringComparison.InvariantCultureIgnoreCase));
            IBuffer buffer = BufferProvider.Provide();
            int totalFiles = archive.Files.Count;
            int currentFile = 0;
            int folderIndexStart = archive.Header.IndexOffset;
            int fileIndexStart = folderIndexStart + folderDictionary.Count * IndexBlockSize;
            int contentStart = fileIndexStart + IndexBlockSize * archive.Files.Count;
            int currentFolderIndex = 0;
            int currentFileIndex = 0;
            int currentContentLength = 0;
            foreach (string key in orderedKeys)
            {
                HdrIndex folderIndex = new HdrIndex();
                folderIndex.Name = key;
                folderIndex.Length = folderDictionary[key].Count;
                folderIndex.Position = folderIndexStart + currentFolderIndex;
                if (folderIndex.Length > 0)
                {
                    folderIndex.Offset = fileIndexStart + currentFileIndex;
                }
                else
                {
                    folderIndex.Offset = 0;
                }

                WriteIndex(buffer, folderIndex);
                foreach (HdrFile file in folderDictionary[key])
                {
                    HdrIndex fileIndex = new HdrIndex();
                    fileIndex.NameRaw = file.FileNameRaw;
                    fileIndex.Length = file.Data.Length;
                    fileIndex.Position = fileIndexStart + currentFileIndex;
                    fileIndex.Offset = contentStart + currentContentLength;
                    WriteIndex(buffer, fileIndex);
                    buffer.WriteBytes(file.Data, 0, fileIndex.Offset, fileIndex.Length);
                    currentFileIndex += IndexBlockSize;
                    currentContentLength += file.Data.Length;
                    OnProgressChanged(ActionWrite, folderIndex.Name, totalFiles, currentFile++);
                }

                currentFolderIndex += IndexBlockSize;
            }

            HdrHeader header = new HdrHeader();
            header.ContentOffset = contentStart;
            header.Created = archive.Header.Created;
            header.FolderCount = folderDictionary.Count;
            header.Format = Hdr;
            header.Unknown0 = archive.Header.Unknown0;
            header.IndexOffset = folderIndexStart;
            buffer.Position = 0;
            WriteHeader(buffer, header);
            Utils.WriteFile(buffer.GetAllBytes(), destinationPath);
        }

        public void DecryptArchive(string sourcePath, string destinationPath)
        {
            HdrArchive archive = Read(sourcePath);
            byte[] hdrFile = Utils.ReadFile(sourcePath);
            IBuffer buffer = BufferProvider.Provide(hdrFile);
            int totalFiles = archive.Files.Count;
            int current = 0;
            foreach (HdrFile file in archive.Files)
            {
                KeyState keyState = KeyState.Get(file.CryptoExtension);
                if (keyState != null)
                {
                    file.Encrypted = HdrCrypto.Instance.IsEncrypted(file.Data, keyState);
                    byte[] dst;
                    if (file.Encrypted == false)
                    {
                        dst = file.Data;
                    }
                    else
                    {
                        OnProgressChanged(ActionDecrypt, file.FileName, totalFiles, current++);
                        dst = HdrCrypto.Instance.Decrypt(file.Data, keyState);
                    }

                    buffer.Position = file.Offset;
                    buffer.WriteBytes(dst);
                }
            }

            Utils.WriteFile(buffer.GetAllBytes(), destinationPath);
        }

        /// <summary>
        /// Extracts a chunk of data from the source.
        /// </summary>
        public void ExtractOffset(string source, string destination, int offset, int length)
        {
            // TODO - implement correctly
            HdrArchive archive = Read(source);
            if (archive == null)
            {
                return;
            }

            foreach (HdrFile hdrFile in archive.Files)
            {
                if (hdrFile.Offset == offset)
                {
                    Console.WriteLine(hdrFile.HdrFullPath);
                }
            }
        }

        /// <summary>
        /// Extracts a single file from the archive at the given offset.
        /// </summary>
        public void ExtractAtOffset(string source, string destination, int offset)
        {
            // TODO - implement correctly
            HdrArchive archive = Read(source);
            if (archive == null)
            {
                return;
            }

            foreach (HdrFile hdrFile in archive.Files)
            {
                if (hdrFile.Offset == offset)
                {
                    Console.WriteLine(hdrFile.HdrFullPath);
                }
            }
        }

        /// <summary>
        /// Decrypt a single file.
        /// </summary>
        /// <param name="source">Path to the encrypted source.</param>
        /// <param name="destination">Path to save decrypted file.</param>
        public void DecryptFile(string source, string destination)
        {
            if (!Path.HasExtension(source))
            {
                throw new Exception("Unknown file extensions");
            }

            string fileExtension = Path.GetExtension(source);
            HdrCryptoExtension? extension = HdrFile.GetCryptoExtension(fileExtension);
            KeyState keyState = KeyState.Get(extension);

            if (keyState == null)
            {
                throw new Exception("No key for extension");
            }

            byte[] src = Utils.ReadFile(source);
            bool? encrypted = HdrCrypto.Instance.IsEncrypted(src, keyState);
            byte[] dst;
            if (encrypted == false)
            {
                dst = src;
            }
            else
            {
                dst = HdrCrypto.Instance.Decrypt(src, keyState);
            }

            Utils.WriteFile(dst, destination);
        }

        /// <summary>
        /// Decrypts the <see cref="HdrFile"/> if the extension is supported.
        /// </summary>
        public void DecryptHdrFile(HdrFile file)
        {
            KeyState keyState = KeyState.Get(file.CryptoExtension);
            if (keyState != null)
            {
                file.Encrypted = HdrCrypto.Instance.IsEncrypted(file.Data, keyState);
                byte[] dst;
                if (file.Encrypted == false)
                {
                    dst = file.Data;
                }
                else
                {
                    dst = HdrCrypto.Instance.Decrypt(file.Data, keyState);
                }

                file.Data = dst;
            }
        }

        /// <summary>
        /// Decrypts all supported <see cref="HdrFile"/>s inside a <see cref="HdrArchive"/>.
        /// </summary>
        public void DecryptHdrArchive(HdrArchive archive)
        {
            int totalFiles = archive.Files.Count;
            int current = 0;
            foreach (HdrFile file in archive.Files)
            {
                DecryptHdrFile(file);
                if (file.Encrypted == false)
                {
                    archive.Report.NoEncryption.Add(file.FileNameRaw);
                }

                OnProgressChanged(ActionDecrypt, file.FileName, totalFiles, current++);
            }
        }

        public void EncryptFile(string source, string destination)
        {
            if (!Path.HasExtension(source))
            {
                throw new Exception("Unknown file extensions");
            }

            string fileExtension = Path.GetExtension(source);
            HdrCryptoExtension? extension = HdrFile.GetCryptoExtension(fileExtension);
            KeyState keyState = KeyState.Get(extension);
            if (keyState == null)
            {
                throw new Exception("No key for extension");
            }

            byte[] src = Utils.ReadFile(source);
            byte[] dst = HdrCrypto.Instance.Encrypt(src, keyState);
            Utils.WriteFile(dst, destination);
        }

        public void EncryptHdrFile(HdrFile file)
        {
            KeyState keyState = KeyState.Get(file.CryptoExtension);
            if (keyState != null)
            {
                byte[] dst = HdrCrypto.Instance.Encrypt(file.Data, keyState);
                file.Data = dst;
            }
        }

        /// <summary>
        /// Encrypts all supported <see cref="HdrFile"/>s inside a <see cref="HdrArchive"/>.
        /// </summary>
        public void EncryptHdrArchive(HdrArchive archive)
        {
            int totalFiles = archive.Files.Count;
            int current = 0;
            foreach (HdrFile file in archive.Files)
            {
                EncryptHdrFile(file);
                OnProgressChanged(ActionEncrypt, file.FileName, totalFiles, current++);
            }
        }

        /// <summary>
        /// Extract a .hdr file to a folder.
        /// </summary>
        public void Extract(string source, string destination, bool decrypt)
        {
            HdrArchive archive = Read(source);
            if (archive == null)
            {
                return;
            }

            if (decrypt)
            {
                DecryptHdrArchive(archive);
            }

            int totalFiles = archive.Files.Count;
            int current = 0;
            foreach (HdrFile file in archive.Files)
            {
                string directory = HdrToOsPath(file.HdrDirectoryPath);
                string directoryPath = Path.Combine(destination, directory);
                Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, file.FileName);
                Utils.WriteFile(file.Data, filePath);
                OnProgressChanged(ActionWrite, file.FileName, totalFiles, current++);
            }

            string reportPath = Path.Combine(destination, "hdr.report");
            Utils.WriteFile(archive.Report.Data, reportPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="encrypt"></param>
        public void PackFolder(string source, string destination, bool encrypt, bool recursive)
        {
            if (!Directory.Exists(source))
            {
                throw new Exception(string.Format("'{0}' does not exist or is not a directory", source));
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(source);

            List<DirectoryInfo> directories;
            if (directoryInfo.Name.EndsWith(".tro", StringComparison.OrdinalIgnoreCase)
                || directoryInfo.Name.EndsWith(".dat", StringComparison.OrdinalIgnoreCase))
            {
                directories = new List<DirectoryInfo>();
                directories.Add(directoryInfo);
            }
            else
            {
                directories = Utils.GetFolders(directoryInfo, new[] {".tro", ".dat"}, recursive);
            }

            int totalFolders = directories.Count;
            int current = 0;
            foreach (DirectoryInfo directory in directories)
            {
                string pathDifference = Utils.PathDifference(directory.Parent, directoryInfo, true);
                string destinationFolder = Path.Combine(destination, pathDifference);
                string destinationFile = Path.Combine(destinationFolder, directory.Name);
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                Pack(directory.FullName, destinationFile, encrypt);
                OnProgressChanged(ActionPackFolder, destinationFile, totalFolders, current++);
            }
        }

        /// <summary>
        /// Extract all supported types found inside a folder.
        /// </summary>
        /// <param name="source">Source folder</param>
        /// <param name="destination">Destination folder</param>
        /// <param name="decrypt">Decrypt the output.</param>
        /// <param name="recursive">Search folders inside folders.</param>
        public void ExtractFolder(string source, string destination, bool decrypt, bool recursive)
        {
            if (!Directory.Exists(source))
            {
                throw new Exception(string.Format("'{0}' does not exist or is not a directory", source));
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(source);
            List<FileInfo> files = Utils.GetFiles(directoryInfo, new[] {".tro", ".dat"}, recursive);
            int totalFolders = files.Count;
            int current = 0;
            foreach (FileInfo file in files)
            {
                string pathDifference = Utils.PathDifference(directoryInfo, file.Directory, true);
                string destinationDirectoryPath = Path.Combine(destination, pathDifference, file.Name);
                Directory.CreateDirectory(destinationDirectoryPath);
                Extract(file.FullName, destinationDirectoryPath, decrypt);
                OnProgressChanged(ActionExtractFolder, destinationDirectoryPath, totalFolders, current++);
            }
        }


        /// <summary>
        /// Create a .hdr file from a folder.
        /// </summary>
        public void Pack(string source, string destination, bool encrypt)
        {
            if (!Path.HasExtension(destination))
            {
                throw new Exception(String.Format("Destination '{0}' has no .ext, please use '.tro' or '.dat'",
                    destination));
            }

            string ext = Path.GetExtension(destination).ToLower();
            HdrHeader header;
            if (ext == ".tro")
            {
                header = HdrHeader.Tro();
            }
            else if (ext == ".dat")
            {
                header = HdrHeader.Dat();
            }
            else
            {
                throw new Exception(
                    String.Format("Destination has invalid extension of '{0}', please use '.tro' or '.dat'", ext));
            }

            List<HdrFile> hdrFiles = ReadDirectory(source);
            HdrArchive archive = new HdrArchive(hdrFiles, header);
            if (encrypt)
            {
                EncryptHdrArchive(archive);
            }

            Write(archive, destination);
        }

        private HdrHeader ReadHeader(IBuffer buffer)
        {
            HdrHeader header = new HdrHeader();
            string first = buffer.ReadCString();
            if (first == Hdr)
            {
                header.Format = first;
                header.Created = null;
            }
            else if (DateTime.TryParseExact(first, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var created))
            {
                header.Created = created;
                header.Format = buffer.ReadCString();
            }
            else
            {
                return null;
            }

            header.Unknown0 = buffer.ReadInt32();
            header.ContentOffset = buffer.ReadInt32();
            header.FolderCount = buffer.ReadInt32();
            header.IndexOffset = buffer.ReadInt32();
            return header;
        }

        private void WriteHeader(IBuffer buffer, HdrHeader header)
        {
            if (header.Created != null)
            {
                buffer.WriteCString(string.Format("{0:" + DateFormat + "}", header.Created));
            }

            buffer.WriteCString(header.Format);
            buffer.WriteInt32(header.Unknown0);
            buffer.WriteInt32(header.ContentOffset);
            buffer.WriteInt32(header.FolderCount);
            buffer.WriteInt32(header.IndexOffset);
        }

        private HdrIndex ReadIndex(IBuffer data)
        {
            HdrIndex index = new HdrIndex();
            index.Position = data.Position;
            index.NameRaw = data.ReadBytesZeroTerminated();
            data.Position = index.Position + 260;
            index.Length = data.ReadInt32();
            index.Offset = data.ReadInt32();
            return index;
        }

        private void WriteIndex(IBuffer buffer, HdrIndex index)
        {
            buffer.Position = index.Position;
            buffer.WriteBytes(index.NameRaw);
            buffer.Position = index.Position + 260;
            buffer.WriteInt32(index.Length);
            buffer.WriteInt32(index.Offset);
        }

        private List<HdrFile> ReadDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new Exception(string.Format("'{0}' is not a directory"));
            }

            List<HdrFile> hdrFiles = new List<HdrFile>();
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            ReadDirectory(directoryInfo, directoryInfo, hdrFiles);
            return hdrFiles;
        }

        private int ReadDirectory(DirectoryInfo rootDirectoryInfo, DirectoryInfo directoryInfo, List<HdrFile> hdrFiles)
        {
            int count = 0;
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (!IgnoreFiles.Contains(fileInfo.Name.ToLowerInvariant()))
                {
                    string directoryPath = fileInfo.DirectoryName.Replace(rootDirectoryInfo.FullName, "");
                    directoryPath = OsToHdrPath(directoryPath);
                    string fullPath = directoryPath + fileInfo.Name;
                    HdrFile hdrFile = new HdrFile();
                    hdrFile.Data = Utils.ReadFile(fileInfo.FullName);
                    hdrFile.FileName = fileInfo.Name;
                    hdrFile.FileExtension = fileInfo.Extension;
                    hdrFile.HdrDirectoryPath = directoryPath;
                    hdrFile.HdrFullPath = fullPath;
                    hdrFiles.Add(hdrFile);
                }
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                count += ReadDirectory(rootDirectoryInfo, subDirectoryInfo, hdrFiles);
            }

            return ++count;
        }

        private string OsToHdrPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace('/', '\\');
                if (path[0] == '\\')
                {
                    path = path.Substring(1);
                }

                if (path[path.Length - 1] != '\\')
                {
                    path = path + '\\';
                }
            }

            return path;
        }

        private string HdrToOsPath(string path)
        {
            string result = "";
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace('\\', '/');
                string[] parts = path.Split('/');

                foreach (string part in parts)
                {
                    result = Path.Combine(result, part);
                }
            }

            return result;
        }

        private void OnProgressChanged(string action, string message, int total, int current)
        {
            EventHandler<HdrProgressEventArgs> progressChanged = ProgressChanged;
            if (progressChanged != null)
            {
                HdrProgressEventArgs hdrProgressEventArgs = new HdrProgressEventArgs(action, message, total, current);
                progressChanged(this, hdrProgressEventArgs);
            }
        }
    }
}