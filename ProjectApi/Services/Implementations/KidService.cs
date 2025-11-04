using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.Helpers;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Services.Implementations
{
    public class KidService : IKidService
    {
        private readonly AppDbContext _context;

        public KidService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kid>> GetAllKidsAsync() 
            => await _context.Kids.ToListAsync();

        public async Task<Kid?> GetKidByIdAsync(string id) 
            => await _context.Kids.FirstOrDefaultAsync(k => k.Id == id);

        public async Task<bool> UpdateKidAsync(string id, Kid kid)
        {
            var existingKid = await _context.Kids.FindAsync(id);
            if (existingKid == null) return false;

            existingKid.GameBalance = kid.GameBalance;
            existingKid.AvatarUrl = kid.AvatarUrl;
            existingKid.Name = kid.Name;


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteKidAsync(string id)
        {
            var kid = await _context.Kids.FindAsync(id);
            if (kid == null) return false;

            _context.Kids.Remove(kid);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Kid> CreateKidAsync(int parentId, Kid kid)
        {
            var parent = await _context.Parents
                .Include(p => p.Kids)
                .FirstOrDefaultAsync(p => p.Id == parentId)
                ?? throw new Exception("Родитель не найден");

            kid.Id = IdGenerator.GenerateKidId();

            while (await _context.Kids.FindAsync(kid.Id) != null)
            {
                kid.Id = IdGenerator.GenerateKidId();
            }

            _context.Kids.Add(kid);
            parent.Kids.Add(kid);

            await _context.SaveChangesAsync();
            return kid;
        }

        public async Task<List<KidTask>> GetKidTasksAsync(string kidId)
        {
            return await _context.KidTasks
                .Where(kt => kt.KidId == kidId)
                .ToListAsync();
        }
    }
}
