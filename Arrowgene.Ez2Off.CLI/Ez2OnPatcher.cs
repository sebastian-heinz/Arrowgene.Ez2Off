using Arrowgene.Ez2Off.Common;

namespace Arrowgene.Ez2Off.CLI
{
    using System.Collections.Generic;
    using System.IO;


    public class Ez2OnPatcher
    {
        private readonly List<GamePatch> _xTrap;
        private readonly FileInfo _file;
        private readonly List<GamePatch> _encryption;
        
        private List<GamePatch> _xTrapIp1;
        private List<GamePatch> _xTrapIp2;
        private List<GamePatch> _xTrapIp3;
        private List<GamePatch> _loginIp;
        private List<GamePatch> _loginPort;
        private GamePatcher _patcher;

        public Ez2OnPatcher(FileInfo path)
        {
            _file = path;
            _xTrap = new List<GamePatch>();
            _xTrapIp1 = new List<GamePatch>();
            _xTrapIp2 = new List<GamePatch>();
            _xTrapIp3 = new List<GamePatch>();
            _loginIp = new List<GamePatch>();
            _loginPort = new List<GamePatch>();
            _encryption = new List<GamePatch>();
        }

        public void SavePatches(string ip, ushort port, bool removeXTrap, bool disableEncryption, bool osxPatch)
        {
            byte[] file = Utils.ReadFile(_file.FullName);
            if (file == null)
            {
                return;
            }

            _patcher = new GamePatcher(file);

            _xTrap.Add(new GamePatch(0x8628F, 0x52, 0x90));
            _xTrap.Add(new GamePatch(0x86290, 0xFF, 0x90));
            _xTrap.Add(new GamePatch(0x86291, 0x15, 0x90));
            _xTrap.Add(new GamePatch(0x86292, 0x74, 0x90));
            _xTrap.Add(new GamePatch(0x86293, 0xF2, 0x90));
            _xTrap.Add(new GamePatch(0x86294, 0x54, 0x90));
            _xTrap.Add(new GamePatch(0x86295, 0x00, 0x90));

            _xTrap.Add(new GamePatch(0x11B7C8, 0x55, 0xC3));

            _xTrap.Add(new GamePatch(0x11DAF0, 0x0F, 0xE9));
            _xTrap.Add(new GamePatch(0x11DAF1, 0x84, 0xD3));
            _xTrap.Add(new GamePatch(0x11DAF2, 0xD2, 0x01));
            _xTrap.Add(new GamePatch(0x11DAF3, 0x01, 0x00));
            _xTrap.Add(new GamePatch(0x11DAF5, 0x00, 0x90));

            _xTrap.Add(new GamePatch(0x11F9F5, 0x74, 0x90));
            _xTrap.Add(new GamePatch(0x11F9F6, 0x18, 0x90));

            _xTrap.Add(new GamePatch(0x11FE9C, 0x75, 0xEB));

            _xTrap.Add(new GamePatch(0x11FEF9, 0x90, 0xFF));
            _xTrap.Add(new GamePatch(0x11FEFA, 0x5F, 0xFF));
            _xTrap.Add(new GamePatch(0x11FEFB, 0x01, 0xFF));
            _xTrap.Add(new GamePatch(0x11FEFC, 0x00, 0xFF));

            _xTrap.Add(new GamePatch(0x120D60, 0x30, 0xFF));
            _xTrap.Add(new GamePatch(0x120D61, 0x75, 0xFF));
            _xTrap.Add(new GamePatch(0x120D62, 0x00, 0xFF));
            _xTrap.Add(new GamePatch(0x120D63, 0x00, 0xFF));

            _xTrapIp1 = GamePatcher.CreateIpPatch("127.0.0.1", 0x196714);
            _xTrapIp2 = GamePatcher.CreateIpPatch("127.0.0.1", 0x196750);
            _xTrapIp3 = GamePatcher.CreateIpPatch("127.0.0.1", 0x1967B4);
            
            _encryption.Add(new GamePatch(879232, 139, 184));
            _encryption.Add(new GamePatch(879233, 68, 0));
            _encryption.Add(new GamePatch(879234, 36, 0));
            _encryption.Add(new GamePatch(879235, 20, 0));
            _encryption.Add(new GamePatch(879236, 139, 0));
            _encryption.Add(new GamePatch(879237, 76, 195));
            _encryption.Add(new GamePatch(879238, 36, 144));
            _encryption.Add(new GamePatch(879239, 16, 144));
            _encryption.Add(new GamePatch(879240, 139, 144));
            _encryption.Add(new GamePatch(879241, 84, 144));
            _encryption.Add(new GamePatch(879242, 36, 144));
            _encryption.Add(new GamePatch(879243, 12, 144));
            _encryption.Add(new GamePatch(879244, 80, 144));
            _encryption.Add(new GamePatch(879245, 139, 144));
            _encryption.Add(new GamePatch(879246, 68, 144));
            _encryption.Add(new GamePatch(879247, 36, 144));
            _encryption.Add(new GamePatch(879248, 12, 144));
            _encryption.Add(new GamePatch(879249, 81, 144));
            _encryption.Add(new GamePatch(879250, 139, 144));
            _encryption.Add(new GamePatch(879251, 76, 144));
            _encryption.Add(new GamePatch(879252, 36, 144));
            _encryption.Add(new GamePatch(879253, 12, 144));
            _encryption.Add(new GamePatch(879254, 82, 144));
            _encryption.Add(new GamePatch(879255, 80, 144));
            _encryption.Add(new GamePatch(879256, 81, 144));
            _encryption.Add(new GamePatch(879257, 232, 144));
            _encryption.Add(new GamePatch(879258, 194, 144));
            _encryption.Add(new GamePatch(879259, 2, 144));
            _encryption.Add(new GamePatch(879260, 0, 144));
            _encryption.Add(new GamePatch(879261, 0, 144));
            _encryption.Add(new GamePatch(879262, 131, 144));
            _encryption.Add(new GamePatch(879263, 196, 144));
            _encryption.Add(new GamePatch(879264, 20, 144));
            _encryption.Add(new GamePatch(879265, 195, 144));

            SetIp(ip);
            SetPort(port);
            if (removeXTrap)
            {
                RemoveXtrap();
            }
            if (disableEncryption)
            {
                DisableEncryption();
            }
            else
            {
                EnableEncryption();
            }
            if (osxPatch)
            {
                EnableOsxPatch();
            }
            else
            {
                DisableOsxPatch();
            }
            _patcher.Patch();
            byte[] patched = _patcher.GetFile();
            Utils.WriteFile(patched, _file.FullName);
        }

        private void RemoveXtrap()
        {
            if (!_patcher.IsPatched(_xTrap))
            {
                _patcher.AddPatch(_xTrap);
            }

            if (!_patcher.IsPatched(_xTrapIp1))
            {
                _patcher.AddPatch(_xTrapIp1);
            }

            if (!_patcher.IsPatched(_xTrapIp2))
            {
                _patcher.AddPatch(_xTrapIp2);
            }

            if (!_patcher.IsPatched(_xTrapIp3))
            {
                _patcher.AddPatch(_xTrapIp3);
            }
        }

        private void SetIp(string ip)
        {
            _loginIp = GamePatcher.CreateIpPatch(ip, 0x152B88);
            if (!_patcher.IsPatched(_loginIp))
            {
                _patcher.AddPatch(_loginIp);
            }
        }

        private void SetPort(ushort port)
        {
            _loginPort.AddRange(CreatePortPatch(port, 0x44FF5));
            _loginPort.AddRange(CreatePortPatch(port, 0x45036));
            if (!_patcher.IsPatched(_loginPort))
            {
                _patcher.AddPatch(_loginPort);
            }
        }

        private List<GamePatch> CreatePortPatch(ushort port, int offset)
        {
            byte lo = (byte) (port & 0xff);
            byte hi = (byte) ((port >> 8) & 0xff);

            List<GamePatch> patches = new List<GamePatch>();
            patches.Add(new GamePatch(offset, lo));
            patches.Add(new GamePatch(offset + 1, hi));

            return patches;
        }
        
        private void DisableEncryption()
        {
            if (!_patcher.IsPatched(_encryption))
            {
                _patcher.AddPatch(_encryption);
            }
        }

        private void EnableEncryption()
        {
            List<GamePatch> patches = GamePatch.CreateRevertPatches(_encryption);
            if (!_patcher.IsPatched(patches))
            {
                _patcher.AddPatch(patches);
            }
        }

        private void EnableOsxPatch()
        {
            List<GamePatch> patches = CreateOSXPatch(1176);
            if (!_patcher.IsPatched(patches))
            {
                _patcher.AddPatch(patches);
            }
        }

        private void DisableOsxPatch()
        {
            List<GamePatch> patches = CreateRevertOSXPatch();
            if (!_patcher.IsPatched(patches))
            {
                _patcher.AddPatch(patches);
            }
        }

        private List<GamePatch> CreateRevertOSXPatch()
        {
            return new List<GamePatch>
            {
                new GamePatch(41068, 139),
                new GamePatch(41069, 140),
                new GamePatch(41070, 134),
                new GamePatch(41071, 208),
                new GamePatch(41072, 1),
                new GamePatch(41073, 0),
                new GamePatch(41074, 0),
                new GamePatch(41075, 15),
                new GamePatch(41076, 191),
                new GamePatch(41077, 65),
                new GamePatch(41078, 74),
                new GamePatch(34100, 139),
                new GamePatch(34101, 140),
                new GamePatch(34102, 129),
                new GamePatch(34103, 208),
                new GamePatch(34104, 1),
                new GamePatch(34105, 0),
                new GamePatch(34106, 0),
                new GamePatch(34107, 15),
                new GamePatch(34108, 191),
                new GamePatch(34109, 65),
                new GamePatch(34110, 74),
                new GamePatch(40883, 139),
                new GamePatch(40884, 140),
                new GamePatch(40885, 130),
                new GamePatch(40886, 208),
                new GamePatch(40887, 1),
                new GamePatch(40888, 0),
                new GamePatch(40889, 0),
                new GamePatch(40890, 15),
                new GamePatch(40891, 191),
                new GamePatch(40892, 65),
                new GamePatch(40893, 74),
                new GamePatch(40400, 139),
                new GamePatch(40401, 140),
                new GamePatch(40402, 130),
                new GamePatch(40403, 208),
                new GamePatch(40404, 1),
                new GamePatch(40405, 0),
                new GamePatch(40406, 0),
                new GamePatch(40407, 15),
                new GamePatch(40408, 191),
                new GamePatch(40409, 65),
                new GamePatch(40410, 74)
            };
        }

        private List<GamePatch> CreateOSXPatch(int number)
        {
            byte patched = (byte) (number & 0xFF);
            byte patched2 = (byte) ((number >> 8) & 0xFF);
            byte patched3 = (byte) ((number >> 16) & 0xFF);
            byte patched4 = (byte) (number >> 24);
            return new List<GamePatch>
            {
                new GamePatch(41068, 184),
                new GamePatch(41069, patched),
                new GamePatch(41070, patched2),
                new GamePatch(41071, patched3),
                new GamePatch(41072, patched4),
                new GamePatch(41073, 144),
                new GamePatch(41074, 144),
                new GamePatch(41075, 144),
                new GamePatch(41076, 144),
                new GamePatch(41077, 144),
                new GamePatch(41078, 144),
                new GamePatch(34100, 184),
                new GamePatch(34101, patched),
                new GamePatch(34102, patched2),
                new GamePatch(34103, patched3),
                new GamePatch(34104, patched4),
                new GamePatch(34105, 144),
                new GamePatch(34106, 144),
                new GamePatch(34107, 144),
                new GamePatch(34108, 144),
                new GamePatch(34109, 144),
                new GamePatch(34110, 144),
                new GamePatch(40883, 184),
                new GamePatch(40884, patched),
                new GamePatch(40885, patched2),
                new GamePatch(40886, patched3),
                new GamePatch(40887, patched4),
                new GamePatch(40888, 144),
                new GamePatch(40889, 144),
                new GamePatch(40890, 144),
                new GamePatch(40891, 144),
                new GamePatch(40892, 144),
                new GamePatch(40893, 144),
                new GamePatch(40400, 184),
                new GamePatch(40401, patched),
                new GamePatch(40402, patched2),
                new GamePatch(40403, patched3),
                new GamePatch(40404, patched4),
                new GamePatch(40405, 144),
                new GamePatch(40406, 144),
                new GamePatch(40407, 144),
                new GamePatch(40408, 144),
                new GamePatch(40409, 144),
                new GamePatch(40410, 144)
            };
        }
    }
}