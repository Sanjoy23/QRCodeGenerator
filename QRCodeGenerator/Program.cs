using QRCodeGenerator.ECC;
using QRCodeGenerator.Matrix_Placement;
using QRCodeGenerator.Services;

namespace QRCodeGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            string input = "HELLO WORLD";

            string bitString = BitStream.DataToBitStream(input);

            List<byte> codeWords = BitStreamToCodeWords.ToCodeWords(bitString);

            var blockDefs = new List<ErrorCorrectionCoding.BlockDef> { new ErrorCorrectionCoding.BlockDef(1, 80, 20) };
            var dataBlocks = ErrorCorrectionCoding.SplitIntoBlocks(codeWords, blockDefs);
            var eccBlocks = ErrorCorrectionCoding.ComputeEccForBlocks(dataBlocks, 20);
            var finalStream = ErrorCorrectionCoding.InterleaveBlocks(dataBlocks, eccBlocks);

            //Console.WriteLine("Final codeword count: " + finalStream.Count);
            //Console.WriteLine(string.Join(" ", finalStream.Select(b => b.ToString("X2"))));
            var m = QRCodeMatrix.CreateBaseMatrix(4);
            
        }
    }
}
