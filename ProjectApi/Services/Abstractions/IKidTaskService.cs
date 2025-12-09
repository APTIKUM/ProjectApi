using ProjectApi.DTOs;
using ProjectApi.Models;

namespace ProjectApi.Services.Abstractions
{
    public interface IKidTaskService
    {
        Task<IEnumerable<KidTask>> GetAllKidTasksAsync(); // will be removed
        Task<KidTask?> GetTaskByIdAsync(int taskId);
        Task<KidTask> CreateTaskAsync(string kidId, KidTask task);
        Task<KidTask> UpdateTaskAsync(int taskId, KidTaskUpdateDto task);
        Task<bool> DeleteTaskAsync(int taskId);

        Task<KidTask> ChangeCompletedAsync(int taskId, bool isSetCompleted, DateOnly? date = null);
    }
}
