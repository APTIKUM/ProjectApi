using ProjectApi.Models;

namespace ProjectApi.Services.Abstractions
{
    public interface IKidService
    {
        Task<IEnumerable<Kid>> GetAllKidsAsync();
        Task<Kid?> GetKidByIdAsync(string id);
        Task<bool> UpdateKidAsync(string id, Kid kid);
        Task<bool> DeleteKidAsync(string id);
    }
}
