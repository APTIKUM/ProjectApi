using ProjectApi.DTOs;
using ProjectApi.Models;

namespace ProjectApi.Services.Abstractions
{
    public interface IKidService
    {
        Task<IEnumerable<Kid>> GetAllKidsAsync();
        Task<Kid?> GetKidByIdAsync(string id);
        Task<Kid> CreateKidAsync(int parentId);
        Task<Kid> UpdateKidAsync(string id, KidUpdateDto kid);
        Task<bool> DeleteKidAsync(string id);

        Task<Kid> ChangeGameBalance(string id, int profit);

        Task<List<KidTask>> GetKidTasksAsync(string kidId);
    }
}
