using System;
using System.Collections.Generic;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    /// <summary>
    /// Statefull key which will be modified at runtime.
    /// </summary>
    public class KeyState : ICloneable
    {
        /// <summary>
        /// Return a new statefull key for the given extension.
        /// </summary>
        public static KeyState Get(HdrCryptoExtension? extension)
        {
            switch (extension)
            {
                case HdrCryptoExtension.png: return new KeyState(HdrKey.Png());
                case HdrCryptoExtension.ogg: return new KeyState(HdrKey.Ogg());
                case HdrCryptoExtension.jpg: return new KeyState(HdrKey.Jpg());
                case HdrCryptoExtension.bin: return new KeyState(HdrKey.Bin());
                case HdrCryptoExtension.pvi: return new KeyState(HdrKey.Pvi());
                case HdrCryptoExtension.scr: return new KeyState(HdrKey.Scr());
                case HdrCryptoExtension.bmp: return new KeyState(HdrKey.Bmp());
                case HdrCryptoExtension.str: return new KeyState(HdrKey.Str());
                case HdrCryptoExtension.dat: return new KeyState(HdrKey.Dat());
                case HdrCryptoExtension.ptn: return new KeyState(HdrKey.Ptn());
                default: return null;
            }
        }

        public UInt32[] Key { get; set; }
        public UInt32[] Output { get; set; }
        public UInt32[] Hash { get; set; }
        public UInt32[] Init { get; set; }
        public List<byte[]> Header { get; set; }
        public HdrCryptoExtension? CryptoExtension { get; set; }

        public KeyState()
        {
            Key = new UInt32[4];
            Hash = new UInt32[72];
            Init = new UInt32[4];
            Output = new UInt32[4];
            Header = new List<byte[]>();
            CryptoExtension = null;
        }

        public KeyState(HdrKey key) : this()
        {
            Key = ToUInt32(key.Key);
            Hash = ToUInt32(key.Hash);
            Init = ToUInt32(key.Init);
            Header = new List<byte[]>(key.Header);
            CryptoExtension = key.CryptoExtension;
        }

        private UInt32[] ToUInt32(byte[] input)
        {
            int size = input.Length / sizeof(UInt32);
            UInt32[] uInts = new UInt32[size];
            for (var index = 0; index < size; index++)
            {
                uInts[index] = BitConverter.ToUInt32(input, index * sizeof(UInt32));
            }
            return uInts;
        }

        public object Clone()
        {
            return new KeyState
            {
                Key = (UInt32[]) Key.Clone(),
                Output = (UInt32[]) Output.Clone(),
                Init = (UInt32[]) Init.Clone(),
                Hash = (UInt32[]) Hash.Clone(),
                Header = new List<byte[]>(Header),
                CryptoExtension = CryptoExtension
            };
        }
    }
}