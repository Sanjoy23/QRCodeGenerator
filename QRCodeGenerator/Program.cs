using System;
using System.Collections.Generic;
using QRCodeGenerator.Services;
using QRCodeGenerator.Utilites;

namespace QRCodeGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            string input = "HELLO WORLD";

            string bitString = BitStream.DataToBitStream(input);

            List<byte> codeWords = BitStreamToCodeWords.ToCodeWords(bitString);

            Console.WriteLine(codeWords.Count);
        }
    }
}
