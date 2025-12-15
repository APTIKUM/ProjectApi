using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.DTOs;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Services.Implementations
{
    public class ParentService : IParentService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public ParentService(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<Parent>> GetAllParentsAsync()
            => await _context.Parents.ToListAsync();

        public async Task<Parent?> GetParentByIdAsync(int id)
            => await _context.Parents.FindAsync(id);

        public async Task<Parent> RegisterParentAsync(ParentRegisterDto parent)
        {
            var newParent = new Parent { Email = parent.Email, Password = _passwordService.HashPassword(parent.Password) };
            _context.Parents.Add(newParent);
            await _context.SaveChangesAsync();

            return newParent;
        }

        public async Task<bool> ChangePassword(int id,string currentPassword, string newPassword)
        {
            var existingParent = await GetParentByIdAsync(id);

            if (existingParent == null
                || !_passwordService.VerifyPassword(currentPassword, existingParent.Password))
            {
                return false;
            }
            existingParent.Password = _passwordService.HashPassword(newPassword);


            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Parent> UpdateParentAsync(int id, ParentUpdateDto updateParentDto)
        {
            var existingParent = await GetParentByIdAsync(id);

            if (existingParent == null)
            {
                return null;
            }
            
            existingParent.Name = updateParentDto.Name ?? existingParent.Name;
            existingParent.AvatarUrl = updateParentDto.AvatarUrl ?? existingParent.AvatarUrl;

            await _context.SaveChangesAsync();

            return existingParent;
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
            var parent = await _context.Parents.FirstOrDefaultAsync(p => p.Email == email);

            if (parent == null 
                || !_passwordService.VerifyPassword(password, parent.Password))
            {
                return null;
            }

            return parent;
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
