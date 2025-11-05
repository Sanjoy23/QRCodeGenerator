namespace QRCodeGenerator.Services
{
    public class CharacterCountIndicator
    {
        public static string GetBits(string input, int version, QrEncodingMode mode)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException("input cannot be null or empty");

            int count = input.Length;
            int bitLength = GetBitLength(version, mode);

            string binary = Convert.ToString(count, 2);

            if (binary.Length > bitLength)
            {
                throw new InvalidOperationException($"Character count ({count}) too large for {bitLength}-bit field.");
            }

            return binary.PadLeft(bitLength, '0');

        }


        public static int GetBitLength(int version, QrEncodingMode mode)
        {
            if (version >= 1 && version <= 9)
            {
                switch (mode)
                {
                    case QrEncodingMode.Numeric:
                        return 10;
                    case QrEncodingMode.Alphanumeric:
                        return 9;
                    case QrEncodingMode.Byte:
                        return 8;
                    case QrEncodingMode.Kanji:
                        return 8;
                    default: return 0;
                }
            }
            return 0;
        }
    }
}
