using System.Text;
using System.Text.RegularExpressions;

namespace QRCodeGenerator.Services
{
    public class ModeAnalyzer
    {
        private static readonly Regex NumericRegex = new(@"^[0-9]+$");
        private static readonly Regex AlphanumericRegex = new(@"^[0-9A-Z \$%\*\+\-\.\/\:]+$");

        public static QrEncodingMode DetermineMode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return QrEncodingMode.Invalid;
            }
            if (NumericRegex.IsMatch(input))
            {
                return QrEncodingMode.Numeric;
            }
            if (AlphanumericRegex.IsMatch(input))
            {
                return QrEncodingMode.Alphanumeric;
            }
            if (IsIso8859_1(input))
            {
                return QrEncodingMode.Byte;
            }
            if (IsKanji(input))
            {
                return QrEncodingMode.Kanji;
            }
            return QrEncodingMode.Invalid;
        }

        private static bool IsIso8859_1(string input)
        {
            try
            {
                var encoded = Encoding.GetEncoding("ISO-8859-1", new EncoderExceptionFallback(), new DecoderExceptionFallback())
                    .GetBytes(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsKanji(string input)
        {
            foreach (var c in input)
            {
                int code = c;
                if (!((code >= 0x4E00 && code <= 0x9FAF) || (code >= 0x3040 && code <= 0x30FF)))
                    return false;
            }
            return true;
        }
        public static string EncodingModeIndicator(QrEncodingMode mode)
        {
            switch (mode)
            {
                case QrEncodingMode.Numeric:
                    return "0001";
                case QrEncodingMode.Alphanumeric:
                    return "0010";
                case QrEncodingMode.Byte:
                    return "0100";
                case QrEncodingMode.Kanji:
                    return "1000";
                default: return "0000";
            }
        }

    }

    public enum QrEncodingMode
    {
        Numeric,
        Alphanumeric,
        Byte,
        Kanji,
        Invalid
    }

}
