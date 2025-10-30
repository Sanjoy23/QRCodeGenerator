
namespace QRCodeGenerator.Services
{
    public enum ErrorCorrectionLevel {
        L, // 7% error
        M, // 15% error
        Q, //25% error
        H //30% error
    }
    public static class ErrorCorrectionLevelHelper
    {
        public static ErrorCorrectionLevel FromString(string input)
        {
            if(string.IsNullOrEmpty(input)) throw new ArgumentNullException("Error correction level cannot be null or empty");

            return input.ToUpper() switch
            {
                "L" => ErrorCorrectionLevel.L,
                "M" => ErrorCorrectionLevel.M,
                "Q" => ErrorCorrectionLevel.Q,
                "H" => ErrorCorrectionLevel.H,
                _ => throw new ArgumentException($"Invalid Error Correction lever: {input}")
            };
        }
    }
}
