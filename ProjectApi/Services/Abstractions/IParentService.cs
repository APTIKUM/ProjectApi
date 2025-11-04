using ProjectApi.Models;
namespace ProjectApi.Services.Abstractions
{
    public interface IParentService
    {
        Task<IEnumerable<Parent>> GetAllParentsAsync(); // will be removed
        Task<Parent?> GetParentByIdAsync(int id);
        Task<Parent> CreateParentAsync(Parent parent);
        Task<bool> UpdateParentAsync(int id, Parent parent);
        Task<bool> DeleteParentAsync(int id);
        Task<Parent?> LoginAsync(string email, string password);

        // для детей
        Task<List<Kid>> GetParentKidsAsync(int parentId);
        Task<bool> AddKidToParentAsync(int parentId, string kidId);
        Task<bool> RemoveKidFromParentAsync(int parentId, string kidId);

    }
}
