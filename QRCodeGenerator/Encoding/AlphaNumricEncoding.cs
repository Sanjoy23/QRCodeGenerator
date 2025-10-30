namespace QRCodeGenerator.Services
{
    public class AlphaNumericEncoding
    {
        public static string EncodeAlphaNumericData(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or empty.");

            input = input.ToUpperInvariant();

            var map = AlphaNumericTable();
            ValidateInput(input, map);

            var pairs = SplitIntoPairs(input);
            var outputBits = new List<string>();

            foreach (var pair in pairs)
            {
                if (pair.Length == 2)
                {
                    int a = map[pair[0]];
                    int b = map[pair[1]];
                    int value = a * 45 + b;
                    outputBits.Add(Convert.ToString(value, 2).PadLeft(11, '0'));
                }
                else // single leftover char
                {
                    int a = map[pair[0]];
                    outputBits.Add(Convert.ToString(a, 2).PadLeft(6, '0'));
                }
            }

            return string.Join("", outputBits);
        }

        private static Dictionary<char, int> AlphaNumericTable() => new()
        {
            { '0', 0 }, { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 },
            { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 },
            { 'A', 10 }, { 'B', 11 }, { 'C', 12 }, { 'D', 13 }, { 'E', 14 },
            { 'F', 15 }, { 'G', 16 }, { 'H', 17 }, { 'I', 18 }, { 'J', 19 },
            { 'K', 20 }, { 'L', 21 }, { 'M', 22 }, { 'N', 23 }, { 'O', 24 },
            { 'P', 25 }, { 'Q', 26 }, { 'R', 27 }, { 'S', 28 }, { 'T', 29 },
            { 'U', 30 }, { 'V', 31 }, { 'W', 32 }, { 'X', 33 }, { 'Y', 34 },
            { 'Z', 35 }, { ' ', 36 }, { '$', 37 }, { '%', 38 }, { '*', 39 },
            { '+', 40 }, { '-', 41 }, { '.', 42 }, { '/', 43 }, { ':', 44 }
        };

        private static void ValidateInput(string input, Dictionary<char, int> map)
        {
            foreach (char c in input)
            {
                if (!map.ContainsKey(c))
                    throw new InvalidOperationException($"Character '{c}' not allowed in Alphanumeric mode.");
            }
        }

        private static List<string> SplitIntoPairs(string input)
        {
            var result = new List<string>();
            for (int i = 0; i < input.Length; i += 2)
            {
                if (i + 1 < input.Length)
                    result.Add(string.Concat(input[i], input[i + 1]));
                else
                    result.Add(input[i].ToString());
            }
            return result;
        }
    }
}
