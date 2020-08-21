using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace DNAEncoder
{
    public class DNADecoder
    {
        private string _path;
        private string[] _source;
        private List<byte> _buffer = new List<byte>();

        public DNADecoder(string filepath)
        {
            _path = filepath;
        }

        public async Task EncodeToEnd()
        {
            _source = await File.ReadAllLinesAsync(_path);

            var counter = 0;

            Parallel.For(1, _source.Length-1, i =>
            {
                counter++;

                if (counter % 10000 == 1)
                    Console.WriteLine($"processing sequence {counter} of {_source.Length}");

                var sequence = _source[i];

                lock(_buffer)
                {
                    _buffer.AddRange(DecodeSequence(sequence));
                }

                
            });

            Console.WriteLine("saving binary file...");
            await File.WriteAllBytesAsync(_path + ".bin", _buffer.ToArray());
        }

        public byte[] DecodeSequence(string sequence)
        {
            var result = new List<byte>();

            for (var i = 0; i < sequence.Length; i += 4)
            {
                try
                {
                    var resultByte = new bool[8];

                    DecodeCharToByte(sequence[i])
                        .CopyTo(resultByte, 0);

                    DecodeCharToByte(sequence[i + 1])
                        .CopyTo(resultByte, 2);

                    DecodeCharToByte(sequence[i + 2])
                        .CopyTo(resultByte, 4);

                    DecodeCharToByte(sequence[i + 3])
                        .CopyTo(resultByte, 6);

                    result.Add(ConvertToByte(new BitArray(resultByte)));
                }
                catch (Exception ex) { }
            }

            return result.ToArray();
        }

        public bool[] DecodeCharToByte(char c)
        {
            switch (c)
            {
                case 'A':
                    return new bool[] { false, false };
                case 'G':
                    return new bool[] { false, true };
                case 'C':
                    return new bool[] { true, false };
                case 'T':
                    return new bool[] { true, true };
                default:
                    return null;
            }
        }

        public byte ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }
    }
}
