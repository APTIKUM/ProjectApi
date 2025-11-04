using ProjectApi.Services.Abstractions;

namespace ProjectApi.Services.Implementations
{
    public class GeneratorIdService : IGeneratorIdService
    {
        private readonly Random _random = new();
        private readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public string GenerateKidId()
        {
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
