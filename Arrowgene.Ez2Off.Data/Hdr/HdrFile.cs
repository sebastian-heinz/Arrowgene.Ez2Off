using Arrowgene.Ez2Off.Common;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    public class HdrFile
    {
        public static HdrCryptoExtension? GetCryptoExtension(string extension)
        {
            if (!string.IsNullOrEmpty(extension))
            {
                switch (extension.ToLower())
                {
                    case ".png": return HdrCryptoExtension.png;
                    case ".ogg": return HdrCryptoExtension.ogg;
                    case ".jpg": return HdrCryptoExtension.jpg;
                    case ".bin": return HdrCryptoExtension.bin;
                    case ".pvi": return HdrCryptoExtension.pvi;
                    case ".scr": return HdrCryptoExtension.scr;
                    case ".bmp": return HdrCryptoExtension.bmp;
                    case ".str": return HdrCryptoExtension.str;
                    case ".dat": return HdrCryptoExtension.dat;
                    case ".ptn": return HdrCryptoExtension.ptn;
                }
            }

            return null;
        }

        public HdrFile()
        {
        }

        public byte[] Data { get; set; }
        public string FileExtension { get; set; }
        public byte[] FileNameRaw { get; set; }
        public string HdrDirectoryPath { get; set; }
        public string HdrFullPath { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public HdrCryptoExtension? CryptoExtension { get; set; }
        public bool? Encrypted { get; set; }

        public string FileName
        {
            get => Utils.KoreanEncoding.GetString(FileNameRaw);
            set => FileNameRaw = Utils.KoreanEncoding.GetBytes(value);
        }
    }
}