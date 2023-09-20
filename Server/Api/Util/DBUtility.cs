namespace Api.Util
{
    public static class DBUtility
    {
        public static async Task<byte[]> ReadBytesFromFileAsync(string path)
        {
            string fullPath = Environment.CurrentDirectory + path;
            return await File.ReadAllBytesAsync(fullPath);
        }
    }
}
