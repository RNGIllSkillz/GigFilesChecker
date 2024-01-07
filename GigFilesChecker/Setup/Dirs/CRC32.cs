using System;
using System.Security.Cryptography;

namespace GigFilesChecker.Setup.Dirs
{
    public class CRC32 : HashAlgorithm
    {
        private uint _currentHash;
        private static uint[] _crc32Table;

        public CRC32()
        {
            _crc32Table = InitializeCRC32Table();
            _currentHash = 0xFFFFFFFF;
        }

        public override void Initialize()
        {
            _currentHash = 0xFFFFFFFF;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (_crc32Table == null)
            {
                Console.WriteLine("Error! _crc32Table cannot be null.");
                return;
            }
            for (var i = ibStart; i < cbSize; ++i)
            {
                _currentHash = _currentHash >> 8 ^ _crc32Table[(array[i] ^ _currentHash) & 0xFF];
            }
        }

        protected override byte[] HashFinal()
        {
            var hash = ~_currentHash;
            var hashBytes = BitConverter.GetBytes(hash);
            Array.Reverse(hashBytes);
            return hashBytes;
        }

        private static uint[] InitializeCRC32Table()
        {
            var crc32Table = new uint[256];
            for (uint i = 0; i < 256; ++i)
            {
                var current = i;
                for (var j = 0; j < 8; ++j)
                {
                    if ((current & 1) == 1)
                    {
                        current = current >> 1 ^ 0xEDB88320;
                    }
                    else
                    {
                        current >>= 1;
                    }
                }
                crc32Table[i] = current;
            }
            return crc32Table;
        }
    }
}
