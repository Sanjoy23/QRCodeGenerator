using QRCodeGenerator.Matrix_Placement;

namespace QRCodeGenerator.DataMasking
{
    public static class ApplyMaskingToData
    {
        public static int FinalMaskPattern;
        public static bool Mask_Function(int mask_pattern, int row, int col)
        {
            if (mask_pattern == 0) return (row + col) % 2 == 0;
            else if (mask_pattern == 1) return row % 2 == 0;
            else if (mask_pattern == 2) return col % 3 == 0;
            else if (mask_pattern == 3) return (row + col) % 3 == 0;
            else if (mask_pattern == 4) return (row / 2 + col / 3) % 2 == 0;
            else if (mask_pattern == 5) return ((row * col) % 2) + ((row * col) % 3) == 0;
            else if (mask_pattern == 6) return (((row * col) % 2) + ((row * col) % 3)) % 2 == 0;
            else if (mask_pattern == 7) return (((row + col) % 2) + ((row * col) % 3)) % 2 == 0;
            else return false;
        }

        public static int[][] FindBestMaskAndApply(int[][] matrix, int version, List<(int row, int col)> AllignmentLocations)
        {
            int minScore = int.MaxValue;
            int bestMask = 0;

            for (int i = 0; i < 8; i++)
            {
                int[][] maskedMatrix = ApplyMask(matrix, version, i, AllignmentLocations);
                int totalScore = EvaluationsConditions.EvaluationsConditoins1(maskedMatrix, maskedMatrix.Length)
                            + EvaluationsConditions.EvaluationsConditoins2(maskedMatrix, maskedMatrix.Length)
                            + EvaluationsConditions.EvaluationsConditoins3(maskedMatrix, maskedMatrix.Length)
                            + EvaluationsConditions.EvaluationsConditoins4(maskedMatrix);
                if (totalScore < minScore)
                {
                    minScore = totalScore;
                    bestMask = i;
                    FinalMaskPattern = i;
                }
            }

            return ApplyMask(matrix, version, bestMask, AllignmentLocations);
        }

        public static int[][] ApplyMask(int[][] matrix, int version, int maskPattern, List<(int row, int col)> AllignmentLocations)
        {
            int size = matrix.Length;
            int[][] CopyMatrix = new int[size][];
            for (int i = 0; i < size; i++)
            {
                CopyMatrix[i] = new int[size];
                Array.Copy(matrix[i], CopyMatrix[i], size);
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (QRCodeMatrix.IsValidDataModule(size, i, j, version))
                    {
                        //Console.WriteLine($"row: {i} col: {j}");
                        if (Mask_Function(maskPattern, i, j))
                        {
                            CopyMatrix[i][j] = 1 - CopyMatrix[i][j];
                        }
                    }
                }
            }
            return CopyMatrix;
        }
    }
}
