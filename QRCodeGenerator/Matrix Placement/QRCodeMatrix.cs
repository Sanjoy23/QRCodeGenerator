namespace QRCodeGenerator.Matrix_Placement
{
    public static class QRCodeMatrix
    {
        public enum ModuleType
        {
            Empty = 0,
            Data = 1,
            Function = 2,
            Format = 3,
            Version = 4,
            Timing = 5
        }
        private static readonly Dictionary<int, int[]> AllignmentPatternLocations = new()
        {
            { 2,  new[] { 6, 18 } },
            { 3,  new[] { 6, 22 } },
            { 4,  new[] { 6, 26 } },
            { 5,  new[] { 6, 30 } },
            { 6,  new[] { 6, 34 } },
            { 7,  new[] { 6, 22, 38 } },
            { 8,  new[] { 6, 24, 42 } },
            { 9,  new[] { 6, 26, 46 } },
            { 10, new[] { 6, 28, 50 } },
            { 11, new[] { 6, 30, 54 } },
            { 12, new[] { 6, 32, 58 } },
            { 13, new[] { 6, 34, 62 } },
            { 14, new[] { 6, 26, 46, 66 } },
            { 15, new[] { 6, 26, 48, 70 } },
            { 16, new[] { 6, 26, 50, 74 } },
            { 17, new[] { 6, 30, 54, 78 } },
            { 18, new[] { 6, 30, 56, 82 } },
            { 19, new[] { 6, 30, 58, 86 } },
            { 20, new[] { 6, 34, 62, 90 } },
            { 21, new[] { 6, 28, 50, 72, 94 } },
            { 22, new[] { 6, 26, 50, 74, 98 } },
            { 23, new[] { 6, 30, 54, 78, 102 } },
            { 24, new[] { 6, 28, 54, 80, 106 } },
            { 25, new[] { 6, 32, 58, 84, 110 } },
            { 26, new[] { 6, 30, 58, 86, 114 } },
            { 27, new[] { 6, 34, 62, 90, 118 } },
            { 28, new[] { 6, 26, 50, 74, 98, 122 } },
            { 29, new[] { 6, 30, 54, 78, 102, 126 } },
            { 30, new[] { 6, 26, 52, 78, 104, 130 } },
            { 31, new[] { 6, 30, 56, 82, 108, 134 } },
            { 32, new[] { 6, 34, 60, 86, 112, 138 } },
            { 33, new[] { 6, 30, 58, 86, 114, 142 } },
            { 34, new[] { 6, 34, 62, 90, 118, 146 } },
            { 35, new[] { 6, 30, 54, 78, 102, 126, 150 } },
            { 36, new[] { 6, 24, 50, 76, 102, 128, 154 } },
            { 37, new[] { 6, 28, 54, 80, 106, 132, 158 } },
            { 38, new[] { 6, 32, 58, 84, 110, 136, 162 } },
            { 39, new[] { 6, 26, 54, 82, 110, 138, 166 } },
            { 40, new[] { 6, 30, 58, 86, 114, 142, 170 } },
        };
        public static bool[][] CreateBaseMatrix(int version, List<byte> message)
        {
            int size = getSize(version);
            bool[][] matrix = new bool[size][];
            for (int i = 0; i < size; i++)
            {
                matrix[i] = new bool[size];
            }
            AddFinderPattern(matrix, 0, 0);
            AddFinderPattern(matrix, 0, size - 7);
            AddFinderPattern(matrix, size - 7, 0);
            PlaceAllignmentPattern(matrix, version);
            AddTimingPattern(matrix, size);
            AddDarkModule(matrix, version);
            AddReverseFormatInfoArea(matrix, size);
            AddDataBits(matrix, message, version, size);

            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i][j]) Console.Write("*");
                    else if(!matrix[i][j]) Console.Write("#");
                    else Console.Write(matrix[i][j]);
                   
                }
                Console.WriteLine();
            }
            return matrix;

        }

        public static void AddFinderPattern(bool[][] matrix, int row, int col)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    int r = row + i;
                    int c = col + j;

                    // Outer 1-pixel border is black
                    if (i == 0 || i == 6 || j == 0 || j == 6)
                    {
                        matrix[r][c] = true;
                    }
                    // Inner 3x3 square is black
                    else if (i >= 2 && i <= 4 && j >= 2 && j <= 4)
                    {
                        matrix[r][c] = true;
                    }
                    else
                    {
                        matrix[r][c] = false;
                    }
                }
            }
        }

        public static void PlaceAllignmentPattern(bool[][] matrix, int version)
        {
            int[] locations = getAllignmentPatternLocations(version);
            int size = matrix[0].Length;

            foreach (int row in locations)
            {
                foreach(int col in locations)
                {
                    if (!checkOverlapsPattern(row, col, size))
                    {
                        SetEachAllignmentPattern(matrix, row, col);
                    }
                }
            }
        }

        public static void SetEachAllignmentPattern(bool[][] matrix, int c_row, int c_col)
        {
            matrix[c_row][c_col] = true;
            for(int i = c_row - 2; i < c_row + 3; i++)
            {
                for(int j = c_col - 2; j < c_col + 3; j++)
                {
                    if (i == c_row - 2 || j == c_col - 2 || j == c_col + 2 || i == c_row + 2)
                    {
                        matrix[i][j] = true;
                    }
                    else if (i == c_row && j == c_col) matrix[i][j] = true;
                    else matrix[i][j] = false;
                }
            }
        }

        public static void AddTimingPattern(bool[][] matrix, int size)
        {
            for(int i = 8; i < size - 8; i++)
            {
                matrix[6][i] = !matrix[6][i - 1] ;
            }

            for(int i = 8; i < size - 8; i++)
            {
                matrix[i][6] = !matrix[i][6];
            }
        }

        public static void AddDarkModule(bool[][] matrix, int version)
        {
            int row = 8;
            int col = version * 4 + 9;
            matrix[row][col] = true;
            //Console.WriteLine(row + " " + col);
        }

        public static void AddReverseFormatInfoArea(bool[][] matrix, int size)
        {
            for(int i = 0; i < 9; i++)
            {
                matrix[8][i] = true;
                matrix[8][size - i - 1] = true;
            }

            for (int i = 0; i < 9; i++)
            {
                matrix[i][8] = true;
                matrix[size - i - 1][8] = true;
            }

        }

        public static void AddReverseVersionInfoArea(bool[][] matrix, int size, int version)
        {
            if (version < 7) throw new InvalidOperationException("Applicable only for version 7 to 40");

        }
        public static void AddDataBits(bool[][] matrix, List<byte> dataBits, int version, int size)
        {
            int direction = -1;
            int dataIndex = 0;
            
        }

        public static int getSize(int version)
        {
            return (version - 1) * 4 + 21;
        }

        public static int[] getAllignmentPatternLocations(int version)
        {
            if (AllignmentPatternLocations.TryGetValue(version, out var locations)) return locations;
            
            return Array.Empty<int>();
        }

        public static bool checkOverlapsPattern(int row, int col, int size)
        {
            return ((row < 8 && col < 8) || (row < 8 && col > size - 9) || (row > size - 9 && col < 8));
        }
    }


}
