using ProjectApi.Models;

namespace ProjectApi.Services.Abstractions
{
    public interface IKidService
    {
        Task<IEnumerable<Kid>> GetAllKidsAsync(); // will be removed
        Task<Kid?> GetKidByIdAsync(string id);
        Task<Kid> CreateKidAsync(int parentId, Kid kid);
        Task<bool> UpdateKidAsync(string id, Kid kid);
        Task<bool> DeleteKidAsync(string id);
        Task<List<KidTask>> GetKidTasksAsync(string kidId);
    }
}
