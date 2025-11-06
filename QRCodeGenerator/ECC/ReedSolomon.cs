using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeGenerator.ECC
{
    public static class ReedSolomon
    {
        public static byte[] BuildGeneratorPolynomial(int degree)
        {
            byte[] g = new byte[] { 1 };
            for(int i = 0; i < degree; i++)
            {
                g = MultiplyPolynomials(g, new byte[] { 1, Galois.Exp[i] });
            }
            //Console.WriteLine(g.Length);
            return g;
        }

        private static byte[] MultiplyPolynomials(byte[] a, byte[] b)
        {
            byte[] result = new byte[a.Length + b.Length - 1];
            for(int i = 0; i < a.Length; i++)
            {
                for(int j = 0; j < b.Length; j++)
                {
                    byte prod = Galois.Mul(a[i], b[j]);
                    result[i + j] = Galois.Add(result[i + j], prod);
                }
            }
            return result;
        }

        public static byte[] EncodeBlock(byte[] dataBlock, int degree)
        {
            if (degree <= 0) return Array.Empty<byte>();
            byte[] generator = BuildGeneratorPolynomial(degree);
            byte[] message = new byte[dataBlock.Length + degree];
            Array.Copy(dataBlock, 0, message, 0, dataBlock.Length);
            for (int i = 0; i < degree; i++)
            {
                message[dataBlock.Length + i] = 0; 
            }

            for (int i = 0; i < dataBlock.Length; i++)
            {
                byte coef = message[i];
                if (coef != 0)
                {
                    for (int j = 0; j < generator.Length; j++)
                    {
                        byte product = Galois.Mul(coef, generator[j]);
                        message[i + j] = Galois.Sub(message[i + j], product);
                    }
                }
            }

            byte[] ecc = new byte[degree];
            Array.Copy(message, dataBlock.Length, ecc, 0, degree);
            return ecc;

        }
    }
}
