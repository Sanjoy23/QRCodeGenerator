namespace QRCodeGenerator.ECC
{
    public static class Galois
    {
        public const int FieldSize = 256;
        private const int Primitive = 0x11d;

        public static readonly byte[] Exp;
        public static readonly byte[] Log;

        static Galois()
        {
            Exp = new byte[512];
            Log = new byte[256];
            byte x = 1;
            for (int i = 0; i < 255; i++)
            {
                Exp[i] = x;
                Log[x] = (byte)i;
                int xInt = x << 1;
                if ((xInt & 0x100) != 0)
                    xInt ^= Primitive;
                x = (byte)(xInt & 0xFF);
            }

            for (int i = 255; i < 512; i++)
            {
                Exp[i] = Exp[i - 255];
            }
        }

        public static byte Add(byte a, byte b)
        {
            return (byte)(a ^ b);
        }
        public static byte Sub(byte a, byte b)
        {
            return Add(a, b);
        }

        public static byte Mul(byte a, byte b)
        {
            if (a == 0 || b == 0)
                return 0;
            int idx = Log[a] + Log[b];
            return Exp[idx];
        }

        public static byte Div(byte a, byte b)
        {
            if (b == 0) throw new DivideByZeroException();
            if(a == 0) return 0;
            int idx = Log[a] - Log[b];
            if (idx < 0) idx += 255;
            return Exp[idx];
        }

        public static byte Pow(byte a, int power)
        {
            if (power == 0) return 1;
            if (a == 0) return 0;
            int idx = (Log[a] * power) % 255;
            return Exp[idx];
        }

        public static void Print()
        {
            foreach(var l in  Log) Console.WriteLine("Log: " + l + " ");
            foreach (var e in Exp) Console.WriteLine("Exp: " + e + " ");
        }
    }
}
