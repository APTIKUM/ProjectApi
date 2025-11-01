namespace ProjectApi.Helpers
{
    public static class IdGenerator
    {
        private static readonly Random _random = new();

        public static string GenerateKidId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
