namespace QRCodeGenerator.Matrix_Placement
{
    public static class QRCodeMatrix
    {
        public enum ModuleType
        {
            Empty = 0,
            Data = 1,
            Pattern = 2,
            Format = 3,
            Version = 4,
            Timing = 5
        }
        public static List<(int row, int col)> AllignmentLocations = new();
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
        public static int[][] CreateBaseMatrix(int version, List<byte> message)
        {
            int size = getSize(version);
            int[][] matrix = new int[size][];
            for (int i = 0; i < size; i++)
            {
                matrix[i] = new int[size];
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
                    Console.Write(matrix[i][j]);

                }
                Console.WriteLine();
            }

            for (int i = 0; i < AllignmentLocations.Count; i++)
            {
                Console.WriteLine(AllignmentLocations[i]);
            }
            return matrix;

        }

        public static void AddFinderPattern(int[][] matrix, int row, int col)
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
                        matrix[r][c] = (int)ModuleType.Pattern;
                    }
                    // Inner 3x3 square is black
                    else if (i >= 2 && i <= 4 && j >= 2 && j <= 4)
                    {
                        matrix[r][c] = (int)ModuleType.Pattern;
                    }
                    else
                    {
                        matrix[r][c] = (int)ModuleType.Empty;
                    }
                }
            }
        }

        public static void PlaceAllignmentPattern(int[][] matrix, int version)
        {
            int[] locations = getAllignmentPatternLocations(version);
            int size = matrix[0].Length;

            foreach (int row in locations)
            {
                foreach (int col in locations)
                {
                    if (!checkOverlapsPattern(row, col, size))
                    {
                        AllignmentLocations?.Add((row, col));
                        SetEachAllignmentPattern(matrix, row, col);
                    }
                }
            }
        }

        public static void SetEachAllignmentPattern(int[][] matrix, int c_row, int c_col)
        {
            matrix[c_row][c_col] = (int)ModuleType.Pattern;
            for (int i = c_row - 2; i < c_row + 3; i++)
            {
                for (int j = c_col - 2; j < c_col + 3; j++)
                {
                    if (i == c_row - 2 || j == c_col - 2 || j == c_col + 2 || i == c_row + 2)
                    {
                        matrix[i][j] = (int)ModuleType.Pattern; ;
                    }
                    else if (i == c_row && j == c_col) matrix[i][j] = (int)ModuleType.Pattern;
                    else matrix[i][j] = (int)ModuleType.Empty;
                }
            }
        }

        public static void AddTimingPattern(int[][] matrix, int size)
        {
            for (int i = 8; i < size - 8; i++)
            {
                matrix[6][i] = matrix[6][i - 1] == (int)ModuleType.Pattern ? 0 : (int)ModuleType.Pattern; ;
            }

            for (int i = 8; i < size - 8; i++)
            {
                matrix[i][6] = matrix[i - 1][6] == (int)ModuleType.Pattern ? 0 : (int)ModuleType.Pattern;
            }
        }

        public static void AddDarkModule(int[][] matrix, int version)
        {
            int row = 8;
            int col = version * 4 + 9;
            matrix[row][col] = (int)ModuleType.Pattern;
            //Console.WriteLine(row + " " + col);
        }

        public static void AddReverseFormatInfoArea(int[][] matrix, int size)
        {
            for (int i = 0; i < 9; i++)
            {
                matrix[8][i] = (int)ModuleType.Pattern;
                matrix[8][size - i - 1] = (int)ModuleType.Pattern;
            }

            for (int i = 0; i < 9; i++)
            {
                matrix[i][8] = (int)ModuleType.Pattern;
                matrix[size - i - 1][8] = (int)ModuleType.Pattern;
            }

        }

        public static void AddReverseVersionInfoArea(int[][] matrix, int size, int version)
        {
            if (version < 7) throw new InvalidOperationException("Applicable only for version 7 to 40");

        }
        public static void AddDataBits(int[][] matrix, List<byte> dataBits, int version, int size)
        {

            int bitIndex = 0;
            int direction = -1; // -1 means moving up, +1 means down

            for (int col = size - 1; col > 0; col -= 2)
            {
                if (col == 7) col--;

                for (int row = (direction == -1 ? size - 1 : 0);
                         (direction == -1 ? row >= 0 : row < size);
                         row += direction)
                {
                    for (int left = col; left > col - 2; left--)
                    {
                        if (left < 0) continue;
                        if (CheckValidDataPlace(size, row, left, version))
                        {
                            matrix[row][left] = dataBits[bitIndex++];
                        }
                        if (bitIndex > dataBits.Count) return;
                    }
                }
                direction *= -1;
            }
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

        public static bool CheckValidDataPlace(int size, int row, int col, int version)
        {
            if ((row < 9 && col < 9) || (row < 9 && col > size - 9) || (col < 9 && row > size - 9)) return false;
            if (row == 6) return false;
            if (col == 6) return false;

            if (size > 21)
            {
                foreach (var loc in AllignmentLocations)
                {
                    int x = loc.row;
                    int y = loc.col;
                    if ((x - 2 <= row && row <= x + 2) && (y - 2 <= col && col <= y + 2))
                        return false;
                }
            }

            if (row == version * 4 + 9 && col == 8)
                return false;
            return true;
        }
    }


}
