using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.Helpers;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Services.Implementations
{
    public class ParentService : IParentService
    {
        private readonly AppDbContext _context;

        public ParentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Parent>> GetAllParentsAsync()
            => await _context.Parents.ToListAsync();

        public async Task<Parent?> GetParentByIdAsync(int id)
            => await _context.Parents.FindAsync(id);

        public async Task<Parent> CreateParentAsync(Parent parent)
        {
            parent.RegistrationDate = DateTime.UtcNow;

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return parent;
        }

        public async Task<bool> UpdateParentAsync(int id, Parent parent)
        {
            var existingParent = await GetParentByIdAsync(id);

            if (existingParent == null)
            {
                return false;
            }

            existingParent.Email = parent.Email;
            existingParent.Password = parent.Password;
            existingParent.Name = parent.Name;
            existingParent.AvatarUrl = parent.AvatarUrl;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteParentAsync(int id)
        {
            var parent = await GetParentByIdAsync(id);

            if (parent == null)
            {
                return false;
            }

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Parent?> LoginAsync(string email, string password)
        {
            return await _context.Parents
                .FirstOrDefaultAsync(p => p.Email == email && p.Password == password);
        }

        public async Task<List<Kid>> GetParentKidsAsync(int parentId)
        {
            return await _context.Kids
                .Where(k => k.Parents.Any(p => p.Id == parentId))
                .ToListAsync();
        }

        

        public async Task<bool> AddKidToParentAsync(int parentId, string kidId)
        {
            var parent = await _context.Parents
                .Include(p => p.Kids)
                .FirstOrDefaultAsync(p => p.Id == parentId);

            if (parent == null) return false;

            var kid = await _context.Kids.FindAsync(kidId);

            if (kid == null) return false;

            if (!parent.Kids.Any(k => k.Id == kidId))
            {
                parent.Kids.Add(kid);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveKidFromParentAsync(int parentId, string kidId)
        {
            var parent = await _context.Parents
                .Include(p => p.Kids)
                .FirstOrDefaultAsync(p => p.Id == parentId);

            if (parent == null) return false;

            var kid = parent.Kids.FirstOrDefault(k => k.Id == kidId);

            if (kid == null) return false;

            parent.Kids.Remove(kid);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
