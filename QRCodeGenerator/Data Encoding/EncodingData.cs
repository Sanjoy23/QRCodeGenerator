namespace QRCodeGenerator.Services
{
    public class EncodingData
    {
        public static string EncodingDataWithMode(string input, QrEncodingMode mode)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException("input can not be null or empty");

            if (mode == QrEncodingMode.Numeric)
            {
                string encodedData = AlphaNumericEncoding.EncodeAlphaNumericData(input);
                return encodedData;
            }
            throw new InvalidOperationException("Invalid operation");
        }
    }
}
