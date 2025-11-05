namespace QRCodeGenerator.Services
{
    public static class QRVersionHandler
    {
        public static readonly Dictionary<int, Dictionary<(ErrorCorrectionLevel, QrEncodingMode), int>> VersionCapacities = new()
        {
            {
                4, new()
                {
                    // L level
                    {(ErrorCorrectionLevel.L, QrEncodingMode.Numeric), 187},
                    {(ErrorCorrectionLevel.L, QrEncodingMode.Alphanumeric), 114},
                    {(ErrorCorrectionLevel.L, QrEncodingMode.Byte), 78},
                    {(ErrorCorrectionLevel.L, QrEncodingMode.Kanji), 48},

                    // M level
                    {(ErrorCorrectionLevel.M, QrEncodingMode.Numeric), 149},
                    {(ErrorCorrectionLevel.M, QrEncodingMode.Alphanumeric), 90},
                    {(ErrorCorrectionLevel.M, QrEncodingMode.Byte), 62},
                    {(ErrorCorrectionLevel.M, QrEncodingMode.Kanji), 38},

                    // Q level
                    {(ErrorCorrectionLevel.Q, QrEncodingMode.Numeric), 111},
                    {(ErrorCorrectionLevel.Q, QrEncodingMode.Alphanumeric), 67},
                    {(ErrorCorrectionLevel.Q, QrEncodingMode.Byte), 46},
                    {(ErrorCorrectionLevel.Q, QrEncodingMode.Kanji), 28},

                    // H level
                    {(ErrorCorrectionLevel.H, QrEncodingMode.Numeric), 82},
                    {(ErrorCorrectionLevel.H, QrEncodingMode.Alphanumeric), 50},
                    {(ErrorCorrectionLevel.H, QrEncodingMode.Byte), 34},
                    {(ErrorCorrectionLevel.H, QrEncodingMode.Kanji), 21}
                }
            }

        };

        public static int DetermineSmallestVersion(string input, ErrorCorrectionLevel errorLevel, QrEncodingMode mode)
        {
            int length = input.Length;
            foreach (var version in VersionCapacities.Keys.OrderBy(v => v))
            {
                if(VersionCapacities[version].TryGetValue((errorLevel, mode), out int value))
                {
                    if (length <= value)
                    {
                        return version;
                    }
                }
            }
            throw new InvalidOperationException("Input too large for QR Version 4.");
        }
    }
}
