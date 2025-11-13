namespace QRCodeGenerator.DataMasking
{
    public static class EvaluationsConditions
    {
        public static int EvaluationsConditoins1(int[][] matrix, int size)
        {
            int TotalHori = 0;
            int TotalVert = 0;
            //int countRow = 0;
            for (int i = 0; i < size; i++)
            {
                int cnt = 1;
                for (int j = 1; j < size; j++)
                {
                    if (matrix[i][j] == matrix[i][j - 1])
                    {
                        cnt++;
                    }
                    else
                    {
                        if (cnt >= 5)
                        {
                            TotalHori += 3 + (cnt - 5);
                        }
                        cnt = 1;
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                int cnt = 1;
                for (int j = 1; j < size; j++)
                {
                    if (matrix[j][i] == matrix[j - 1][i])
                    {
                        cnt++;
                    }
                    else
                    {
                        if (cnt >= 5)
                        {
                            TotalVert += 3 + (cnt - 5);
                        }
                        cnt = 1;
                    }
                }
            }
            //Console.WriteLine($"Hori: {TotalHori}, Vert: {TotalVert}");
            return TotalVert + TotalHori;
        }

        public static int EvaluationsConditoins2(int[][] matrix, int size)
        {
            int penalty = 0;
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < size - 1; j++)
                {
                    if (matrix[i][j] == matrix[i][j + 1] && matrix[i][j] == matrix[i + 1][j] && matrix[i][j] == matrix[i + 1][j + 1])
                        penalty += 3;
                }
            }
            return penalty;
        }

        public static int EvaluationsConditoins3(int[][] matrix, int size)
        {
            int penalty = 0;
            int[] pattern1 = new int[7] { 1, 0, 1, 1, 1, 0, 1 };
            int[] pattern2 = new int[7] { 0, 1, 0, 0, 0, 1, 0 };

            for (int i = 0; i < size; i++)
            {
                bool match1 = true, match2 = true; ;
                for (int j = 0; j < size - 7; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        if (matrix[i][j + k] != pattern1[k]) match1 = false;
                        if (matrix[i][j + k] != pattern2[k]) match2 = false;
                    }

                    if (match1 || match2)
                    {
                        // Check 4 white modules before or after
                        bool beforeWhite = (j >= 4) &&
                            matrix[i][j - 1] == 0 &&
                            matrix[i][j - 2] == 0 &&
                            matrix[i][j - 3] == 0 &&
                            matrix[i][j - 4] == 0;

                        bool afterWhite = (j + 7 <= size - 4) &&
                            matrix[i][j + 7] == 0 &&
                            matrix[i][j + 8] == 0 &&
                            matrix[i][j + 9] == 0 &&
                            matrix[i][j + 10] == 0;

                        if (beforeWhite || afterWhite)
                            penalty += 40;
                    }
                }


            }

            for (int j = 0; j < size; j++)
            {
                for (int i = 0; i <= size - 7; i++)
                {
                    bool match1 = true, match2 = true;
                    for (int k = 0; k < 7; k++)
                    {
                        if (matrix[i + k][j] != pattern1[k]) match1 = false;
                        if (matrix[i + k][j] != pattern2[k]) match2 = false;
                    }

                    if (match1 || match2)
                    {
                        bool beforeWhite = (i >= 4) &&
                            matrix[i - 1][j] == 0 &&
                            matrix[i - 2][j] == 0 &&
                            matrix[i - 3][j] == 0 &&
                            matrix[i - 4][j] == 0;

                        bool afterWhite = (i + 7 <= size - 4) &&
                            matrix[i + 7][j] == 0 &&
                            matrix[i + 8][j] == 0 &&
                            matrix[i + 9][j] == 0 &&
                            matrix[i + 10][j] == 0;

                        if (beforeWhite || afterWhite)
                            penalty += 40;
                    }
                }

            }
            return penalty;
        }

        public static int EvaluationsConditoins4(int[][] matrix)
        {
            int penalty = 0;
            int totalModules = matrix.Length * matrix.Length;
            int blackModules = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == 1)
                        blackModules++;
                }
            }
            double percentage = ((penalty * 1.0) / totalModules) * 100;
            double preMultiple = Math.Floor(percentage / 5) * 5;
            double nextMultiple = Math.Ceiling(percentage / 5) * 5;
            int preDeviation = ((int)Math.Abs(preMultiple - 50)) / 5;
            int nextDeviation = ((int)Math.Abs(nextMultiple - 50)) / 5;
            return Math.Min(preDeviation, nextDeviation);
        }

    }
}
