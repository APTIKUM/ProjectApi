using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.DTOs;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Services.Implementations
{
    public class KidTaskService : IKidTaskService
    {
        private readonly AppDbContext _context;

        private readonly IKidService _kidService;

        public KidTaskService(AppDbContext context, IKidService kidService)
        {
            _kidService = kidService;
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

        public async Task<KidTask> UpdateTaskAsync(int taskId, KidTaskUpdateDto task)
        {
            var existingKidTask = await GetTaskByIdAsync(taskId);

            if (existingKidTask == null)
            {
                return null;
            }

            existingKidTask.Title = task.Title ?? existingKidTask.Title;
            existingKidTask.Price = task.Price ?? existingKidTask.Price;
            existingKidTask.Description = task.Description ?? existingKidTask.Description;
            existingKidTask.TimeStart = task.TimeStart ?? existingKidTask.TimeStart;
            existingKidTask.TimeEnd = task.TimeEnd ?? existingKidTask.TimeEnd;
            existingKidTask.IsRepetitive = task.IsRepetitive ?? existingKidTask.IsRepetitive;
            existingKidTask.RepeatDaysJson = task.RepeatDaysJson ?? existingKidTask.RepeatDaysJson;
            existingKidTask.CompletedDatesJson = task.CompletedDatesJson ?? existingKidTask.CompletedDatesJson;
            //existingKidTask.IsCompleted = task.IsCompleted ?? existingKidTask.IsCompleted;

            await _context.SaveChangesAsync();

            return existingKidTask;
        }

        public async Task<KidTask> ChangeCompletedAsync(int taskId, bool isSetCompleted, DateOnly? date = null)
        {
            var kidTask = await GetTaskByIdAsync(taskId);

            if (kidTask == null)
            {
                return null;
            }

            if (kidTask.IsRepetitive)
            {
                if (date == null)
                {
                    return null;
                }

                var dateOnly = (DateOnly)date;

                var containsDate = kidTask.CompletedDates.Contains(dateOnly);

                if (isSetCompleted == containsDate) return kidTask;

                var completedDates = kidTask.CompletedDates;

                if (isSetCompleted)
                {
                    completedDates.Add(dateOnly);
                }    
                else
                {
                    completedDates.Remove(dateOnly);
                }

                kidTask.CompletedDates = completedDates;

            }
            else
            {
                if (kidTask.IsCompleted == isSetCompleted)
                {
                    return kidTask;
                }

                kidTask.IsCompleted = isSetCompleted;
            }

            await _kidService.ChangeGameBalance(kidTask.KidId, isSetCompleted ? kidTask.Price : -kidTask.Price);
            
            await _context.SaveChangesAsync();
            return kidTask;
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
