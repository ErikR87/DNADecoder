using System;

namespace DNAEncoder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("loading data...");
            var decoder = new DNADecoder(@"C:\temp\encoded_withprimers.txt");
            decoder.EncodeToEnd().Wait();
            Console.WriteLine("done.");
            Console.ReadKey();
        }
    }
}
