using ProjectApi.Models;

namespace ProjectApi.Services.Abstractions
{
    public interface IKidTaskService
    {
        Task<IEnumerable<KidTask>> GetAllKidTasksAsync(); // will be removed
        Task<KidTask?> GetTaskByIdAsync(int taskId);
        Task<KidTask> CreateTaskAsync(string kidId, KidTask task);
        Task<bool> UpdateTaskAsync(int taskId, KidTask task);
        Task<bool> DeleteTaskAsync(int taskId);
    }
}
