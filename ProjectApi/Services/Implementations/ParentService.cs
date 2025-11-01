using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;
using ProjectApi.Helpers;

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

        public async Task<IEnumerable<Kid>> GetParentKidsAsync(int parentId)
        {
            var parent = await _context.Parents
                .Include(p => p.Kids)
                .FirstOrDefaultAsync(p => p.Id == parentId);

            return parent?.Kids ?? new List<Kid>();
        }

        public async Task<bool> AddKidToParentAsync(int parentId, string kidId)
        {
            var parent = await _context.Parents
                .Include(p => p.Kids)
                .FirstOrDefaultAsync(p => p.Id == parentId);

            var kid = await _context.Kids.FindAsync(kidId);

            if (parent == null || kid == null) return false;

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

        public async Task<Kid> CreateKidForParentAsync(int parentId, Kid kid)
        {
            var parent = await _context.Parents
                .FirstOrDefaultAsync(p => p.Id == parentId);

            if (parent == null)
                throw new Exception("Родитель не найден");

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
    }
}
