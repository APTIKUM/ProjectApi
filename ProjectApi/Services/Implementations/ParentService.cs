using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
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
    }
}
