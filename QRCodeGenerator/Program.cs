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

            List<byte> dataCodeWords = new List<byte>();
            for (int i = 0; i < bitString.Length; i += 8) { 
                string byteString = bitString.Substring(i, 8);
                byte codeword = Convert.ToByte(byteString, 2);
                dataCodeWords.Add(codeword);
            }


        }
    }
}
