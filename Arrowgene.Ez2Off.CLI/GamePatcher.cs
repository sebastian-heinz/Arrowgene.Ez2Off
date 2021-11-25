using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowgene.Ez2Off.CLI
{
    public class GamePatcher
    {
        private byte[] _file;
        private List<GamePatch> _patches;
        private bool _patched;

        public static List<GamePatch> CreateIpPatch(string ip, int offset)
        {
            byte[] ipBytes = Encoding.UTF8.GetBytes(ip);
            if (ipBytes.Length > 15)
            {
                throw new Exception("IP is to long max 15byte");
            }

            List<GamePatch> patches = new List<GamePatch>();
            byte[] data = new byte[15];
            Array.Copy(ipBytes, data, ipBytes.Length);
            for (int i = 0; i < data.Length; i++)
            {
                GamePatch patch = new GamePatch(offset + i, data[i]);
                patches.Add(patch);
            }

            return patches;
        }

        public GamePatcher(byte[] file)
        {
            _file = file;
            _patched = false;
            _patches = new List<GamePatch>();
        }

        public byte[] GetFile()
        {
            int length = _file.Length;
            byte[] file = new byte[length];
            Buffer.BlockCopy(_file, 0, file, 0, length);
            return file;
        }

        public bool IsPatched(List<GamePatch> patches)
        {
            foreach (GamePatch patch in patches)
            {
                if (!IsPatched(patch))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsPatched(GamePatch patch)
        {
            if (patch.Offset > _file.Length)
            {
                throw new Exception(String.Format("A patch offset ({0}) is larger than the file size ({1}).", patch.Offset, _file.Length));
            }

            if (_file[patch.Offset] != patch.Patched)
            {
                return false;
            }

            return true;
        }

        public void AddPatch(List<GamePatch> patches)
        {
            foreach (GamePatch patch in patches)
            {
                AddPatch(patch);
            }
        }

        public void AddPatch(GamePatch patch)
        {
            if (_patched)
            {
                throw new Exception("Can only add patches to unpatched file.");
            }

            GamePatch clone = new GamePatch(patch);
            if (clone.Offset > _file.Length)
            {
                throw new Exception(String.Format("A patch offset ({0}) is larger than the file size ({1}).", clone.Offset, _file.Length));
            }

            if (clone.OriginalDefined && _file[clone.Offset] != clone.Original)
            {
                throw new Exception(String.Format("Patch Original ({0}) doesn't match file ({1}) value.", clone.Original, _file[clone.Offset]));
            }

            _patches.Add(clone);
        }

        public void Patch()
        {
            if (!_patched)
            {
                Patch(false);
                _patched = true;
            }
        }

        public void Revert()
        {
            if (_patched)
            {
                Patch(true);
                _patched = false;
            }
        }

        private void Patch(bool restore)
        {
            foreach (GamePatch patch in _patches)
            {
                if (restore)
                {
                    RevertPatch(patch);
                }
                else
                {
                    ApplyPatch(patch);
                }
            }
        }

        private void ApplyPatch(GamePatch patch)
        {
            if (patch.OriginalDefined)
            {
                if (_file[patch.Offset] == patch.Original)
                {
                    _file[patch.Offset] = patch.Patched;
                }
                else
                {
                    throw new Exception("ApplyPatch: Original doesn't match");
                }
            }
            else
            {
                patch.Original = _file[patch.Offset];
                _file[patch.Offset] = patch.Patched;
            }
        }

        private void RevertPatch(GamePatch patch)
        {
            if (_file[patch.Offset] == patch.Patched)
            {
                _file[patch.Offset] = patch.Original;
            }
            else
            {
                throw new Exception("RevertPatch: Original doesn't match");
            }
        }
    }
}