using QRCodeGenerator.Services;
using System.Xml;

namespace QRCodeGenerator.Format_and_Version
{
    public static class FormatAndVersionInfo
    {
        public static string GenerateFormatString(ErrorCorrectionLevel ecl, int maskPattern)
        {
            int eclBits = ecl switch
            {
                ErrorCorrectionLevel.L => 0b01,
                ErrorCorrectionLevel.M => 0b00,
                ErrorCorrectionLevel.Q => 0b11,
                ErrorCorrectionLevel.H => 0b10,
                _ => throw new ArgumentException("Invalid Error Correction Level")
            };
            int formatInfo = eclBits << 3 | maskPattern;

            int generator_poly = 0b10100110111;
            int data = formatInfo << 10;

            for (int i = 14; i >= 10; i--)
            {
                if (((data >> i) & 1) == 1)
                    data ^= generator_poly << (i - 10);
            }

            int remainder = data & 0x3FF;
            int formatString = ((formatInfo << 10) | remainder) ^ 0b101010000010010;

            return Convert.ToString(formatString, 2).PadLeft(15, '0');
        }

        public static int[][] SetFormatStirngToQrMatrix(int[][] matrix, string formatString, int version = 1)
        {
            int size = matrix.Length;
            int[] bits = formatString.Select(c => c == '1' ? 1 : 0).ToArray();
            
            // Place format information in top-left area
            int bitIndex = 0;
            for (int col = 0; col <= 8; col++)
            {
                if (col == 6) continue; // Skip timing pattern column
                if (bitIndex < 8)
                {
                    matrix[8][col] = bits[bitIndex++];
                }
            }
            
            // Column 8, rows 7 down to 0 (skipping row 6 which is timing pattern)
            for (int row = 7; row >= 0; row--)
            {
                if (row == 6) continue; 
                if (bitIndex < 15)
                {
                    matrix[row][8] = bits[bitIndex++];
                }
            }

            // Place format information in bottom-right area (duplicate for redundancy)
            int[] rowPositions = { size - 1, size - 2, size - 3, size - 4, size - 5, size - 6, size - 7 };
            for (int i = 0; i < 7; i++)
            {
                matrix[rowPositions[i]][8] = bits[i];
            }
            
            // Dark Module
            int darkModuleCol = version * 4 + 9;
            int bottomRightStartCol = size - 8;
            bitIndex = 7;
            for (int col = bottomRightStartCol; col < size; col++)
            {
                if (col == darkModuleCol) continue;
                if (bitIndex < 15)
                {
                    matrix[8][col] = bits[bitIndex++];
                }
            }
            
            return matrix;
        }

        public static string GenerateVersionInfoString(int version)
        {
            if(version < 7)
                throw new ArgumentException("Version information is only used for version 7 or higher.");

            int generator_poly = 0x1F25;
            int data = version << 12;
            for (int i = 17; i >= 12; i--)
            {
                if (((data >> i) & 1) == 1)
                {
                    data ^= generator_poly << (i - 12);
                }
            }

            int remainder = data & 0xFFF;
            int versionInfo = (version << 12) | remainder;
            string versionString = Convert.ToString(versionInfo, 2).PadLeft(18, '0');

            Console.WriteLine(versionString);
            return versionString;
        }

        public static int[][] SetVersionInfoToQrMatrix(int[][] matrix, string versionInfo)
        {
            int size = matrix.Length;
            int[] bits = versionInfo.Select(c => c == '1' ? 1 : 0).ToArray();
            int bitIdx = 0;
            for (int i = 0; i < 6; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    matrix[size - 11 + j][i] = bits[bitIdx++];
                }
            }

            bitIdx = 0;

            for (int i = 0; i < 6; i++) 
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[i][size - 11 + j] = bits[bitIdx++];
                }
            }

            return matrix;
        }
    }
}
