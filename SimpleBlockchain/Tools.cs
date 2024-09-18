namespace SimpleBlockchain
{
    public static class Tools
    {
        public static long TimestampNow()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }
}
