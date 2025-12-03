using ProjectApi.DTOs;
using ProjectApi.Models;

namespace ProjectApi.Services.Abstractions
{
    public interface IKidService
    {
        Task<IEnumerable<Kid>> GetAllKidsAsync();
        Task<Kid?> GetKidByIdAsync(string id);
        Task<Kid> CreateKidAsync(int parentId, Kid kid);
        Task<Kid> UpdateKidAsync(string id, KidUpdateDto kid);
        Task<bool> DeleteKidAsync(string id);
        Task<List<KidTask>> GetKidTasksAsync(string kidId);
    }
}
