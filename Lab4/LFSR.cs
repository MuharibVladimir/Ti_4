using System.IO;

namespace Lab4
{
    public class LFSR
    {
        private ulong _lfsr;

        private const int BufferLength = 2048;
        private const int NumberOfBitsInByte = 8;
        
        public LFSR(ulong registerInitialValue)
        {
            _lfsr = registerInitialValue;
        }

        // x^24+x^4+x^3+x+1
        private ulong GetRegisterBit()
        {
            var resultBit = _lfsr >> 31;
            
            var bit23 = (_lfsr >> 23) & 1;
            var bit3 = (_lfsr >> 3) & 1;
            var bit2 = (_lfsr >> 2) & 1;
            var bit0 = (_lfsr >> 0) & 1;

            var xor = bit0 ^ bit2 ^ bit3 ^ bit23;

            _lfsr = (_lfsr << 1) | xor;

            return resultBit;
        }

        public void Processing(string srcFile, string dstFile)
        {
            if (File.Exists(dstFile))
                File.Delete(dstFile);
            
            var binaryReader = new BinaryReader(File.Open(srcFile, FileMode.Open));
            var binaryWriter = new BinaryWriter(File.Open(dstFile, FileMode.Create));

            try
            {
                var buffer = new byte[BufferLength];
                int numberOfBytesRead;
                do
                {
                    numberOfBytesRead = binaryReader.Read(buffer, 0, BufferLength);
                    for (var i = 0; i < BufferLength; ++i)
                    {
                        for (var j = 0; j < NumberOfBitsInByte; ++j)
                        {
                            buffer[i] = (byte) (buffer[i] ^ (GetRegisterBit() << 7 - j));
                        }
                    }

                    binaryWriter.Write(buffer, 0, numberOfBytesRead);

                } while (numberOfBytesRead > 0);
            }
            finally
            {
                binaryReader.Close();
                binaryWriter.Close();
            }
        }
    }
}