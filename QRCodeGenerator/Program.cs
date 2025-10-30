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

            string modeIndicator = ModeAnalyzer.EncodingModeIndicator(QrEncodingMode.Alphanumeric);
            string charCountIndicator = CharacterCountIndicator.GetBits(input, 4, QrEncodingMode.Alphanumeric);
            string encodedData = AlphaNumericEncoding.EncodeAlphaNumericData(input);

            // Correct bit order: Mode + Count + Data
            string completed = string.Concat(modeIndicator, charCountIndicator, encodedData);

            // Lookup total capacity (Version 4, L)
            Dictionary<(int, string), int> CodeWords = ErrorCollectionCodeWords.CodeWordsByVersionAndECLevel();
            int totalCodeWordsByVandECL = CodeWords[(4, "L")];
            int MaxSupportedBits = totalCodeWordsByVandECL * 8;

            // Step 1: Add terminator bits (max 4)
            int remainingBits = MaxSupportedBits - completed.Length;
            string terminator = new string('0', Math.Min(4, remainingBits));
            completed += terminator;

            // Step 2: Pad to full bytes
            int remainder = completed.Length % 8;
            if (remainder != 0)
            {
                completed = completed.PadRight(completed.Length + (8 - remainder), '0');
            }

            // Step 3: Add pad bytes (0xEC, 0x11)
            string[] padBytes = { "11101100", "00010001" };
            int padIndex = 0;

            while (completed.Length < MaxSupportedBits)
            {
                completed += padBytes[padIndex];
                padIndex = 1 - padIndex; // alternate between 0xEC and 0x11
            }

            if (completed.Length > MaxSupportedBits)
            {
                completed = completed.Substring(0, MaxSupportedBits);
            }
        }
    }
}
