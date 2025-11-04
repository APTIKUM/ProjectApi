using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Services.Implementations
{
    public class KidTaskService : IKidTaskService
    {
        private readonly AppDbContext _context;

        public KidTaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KidTask>> GetAllKidTasksAsync() 
            => await _context.KidTasks.ToListAsync();

        public async Task<KidTask?> GetTaskByIdAsync(int taskId)        
            => await _context.KidTasks.FindAsync(taskId);

        public async Task<KidTask> CreateTaskAsync(string kidId, KidTask task)
        {
            var kid = await _context.Kids
                        .Include(k => k.Tasks)
                        .FirstOrDefaultAsync(k => k.Id == kidId)
                        ?? throw new Exception("Ребенок не найден");

            task.KidId = kidId;

            _context.KidTasks.Add(task);

            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<bool> UpdateTaskAsync(int taskId, KidTask task)
        {
            var existingKidRask = await GetTaskByIdAsync(taskId);

            if (existingKidRask == null)
            {
                return false;
            }

            existingKidRask.Title = task.Title;
            existingKidRask.Price = task.Price;
            existingKidRask.Description = task.Description;
            existingKidRask.TimeStart = task.TimeStart;
            existingKidRask.TimeEnd = task.TimeEnd;
            existingKidRask.IsRepetitive = task.IsRepetitive;
            existingKidRask.RepeatDaysJson = task.RepeatDaysJson;
            existingKidRask.IsCompleted = task.IsCompleted;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.KidTasks.FindAsync(taskId);
            if (task == null) return false;

            _context.KidTasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
