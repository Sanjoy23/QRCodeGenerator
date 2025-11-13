using QRCodeGenerator.DataMasking;
using QRCodeGenerator.ECC;
using QRCodeGenerator.Format_and_Version;
using QRCodeGenerator.Matrix_Placement;
using QRCodeGenerator.Services;
using System.Data;

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
            var bitList = ErrorCorrectionCoding.ConvertToBitList(finalStream);

            var m = QRCodeMatrix.CreateBaseMatrix(4, bitList);
            var locations = QRCodeMatrix.AllignmentLocations;

            var masked_matrix = ApplyMaskingToData.FindBestMaskAndApply(m, 4, locations);

            string formatStirng = FormatAndVersionInfo.GenerateFormatString(ErrorCorrectionLevel.L, ApplyMaskingToData.FinalMaskPattern);
            int[][] placedFormatMatrix = FormatAndVersionInfo.SetFormatStirngToQrMatrix(masked_matrix, formatStirng);
            
            //FormatAndVersionInfo.GenerateVersionInfoString(7);

            int[][] finalQrMatrix = QRCodeMatrix.AddQuiteZone(placedFormatMatrix);
            for (int i = 0; i < finalQrMatrix.Length; i++)
            {
                for (int j = 0; j < finalQrMatrix.Length; j++) Console.Write(finalQrMatrix[i][j]);
                Console.WriteLine();
            }

            Console.WriteLine(finalQrMatrix.Length);


        }
    }
}
