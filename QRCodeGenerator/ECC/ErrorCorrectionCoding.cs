namespace QRCodeGenerator.ECC
{
    public static class ErrorCorrectionCoding
    {
        public record BlockDef(int count, int DataCodewordsPerBlock, int EcCodewordsPerBlock);

        public static List<byte[]> SplitIntoBlocks(List<byte> dataCodewords, IEnumerable<BlockDef>blockDefs)
        {
            var defs = blockDefs.ToList();
            var blocks = new List<byte[]>();
            int offset = 0;
            foreach(var d in defs)
            {
                for(int b = 0; b < d.count; b++)
                {
                    if(offset + d.DataCodewordsPerBlock > dataCodewords.Count)
                    {
                        throw new InvalidOperationException("Not enough data bytes");
                    }
                    var block = new byte[d.DataCodewordsPerBlock];
                    dataCodewords.CopyTo(offset, block, 0, d.DataCodewordsPerBlock);
                    blocks.Add(block);
                    offset += d.DataCodewordsPerBlock;
                }
            }
            if(offset != dataCodewords.Count)
            {
                throw new InvalidOperationException("Data codewords count doesn't match expected total from block definitions");
            }

            return blocks;
        }

        public static List<byte[]> ComputeEccForBlocks(List<byte[]> dataBlocks, int degree)
        {
            var eccBlocks = new List<byte[]>();
            foreach (var block in dataBlocks)
            {
                var ecc = ReedSolomon.EncodeBlock(block, degree);
                eccBlocks.Add(ecc);
            }
            return eccBlocks;
        }

        public static List<byte> InterleaveBlocks(List<byte[]> dataBlocks, List<byte[]> eccBlocks)
        {
            var result = new List<byte>();

            // 1. Find max data block length
            int maxDataLen = dataBlocks.Max(b => b.Length);

            // 2. Interleave data bytes
            for (int i = 0; i < maxDataLen; i++)
            {
                foreach (var block in dataBlocks)
                {
                    if (i < block.Length) result.Add(block[i]);
                }
            }

            // 3. Interleave ECC bytes (all ECC blocks have same length)
            int ecLen = eccBlocks[0].Length;
            for (int i = 0; i < ecLen; i++)
            {
                foreach (var e in eccBlocks)
                {
                    result.Add(e[i]);
                }
            }

            return result;
        }

        public static List<byte> ConvertToBitList(List<byte> codewords)
        {
            List<byte> bits = new List<byte>(codewords.Count * 8);
            foreach (byte b in codewords)
            {
                for (int i = 7; i >= 0; i--) // MSB first
                    bits.Add((byte)((b >> i) & 1));
            }
            return bits;
        }
    }
}
