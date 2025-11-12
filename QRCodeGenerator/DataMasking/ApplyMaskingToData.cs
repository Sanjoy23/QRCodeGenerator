using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeGenerator.DataMasking
{
    public static class ApplyMaskingToData
    {
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
    }
}
