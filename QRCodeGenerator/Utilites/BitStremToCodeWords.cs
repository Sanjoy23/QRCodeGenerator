public static class BitStreamToCodeWords
{
    public static List<byte> ToCodeWords(string bitStream)
    {
        List<byte> dataCodeWords = new List<byte>();
        for (int i = 0; i < bitStream.Length; i += 8)
        {
            string byteString = bitStream.Substring(i, 8);
            byte codeword = Convert.ToByte(byteString, 2);
            dataCodeWords.Add(codeword);
        }
        return dataCodeWords;
    }
}