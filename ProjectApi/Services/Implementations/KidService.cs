using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Services.Implementations
{
    public class KidService : IKidService
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new();

        public KidService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kid>> GetAllKidsAsync()
        {
            return await _context.Kids
                .Include(k => k.Parents)
                .ToListAsync();
        }

        public async Task<Kid?> GetKidByIdAsync(string id)
        {
            return await _context.Kids
                .Include(k => k.Parents)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        // перенесено в ParentServices потому что я так решил
        //public async Task<Kid> CreateKidAsync(Kid kid)
        //{
        //    kid.Id = GenerateKidId();

        //    while (await _context.Kids.FindAsync(kid.Id) != null)
        //    {
        //        kid.Id = GenerateKidId();
        //    }
            
        //    _context.Kids.Add(kid);
        //    await _context.SaveChangesAsync();

        //    return kid;
        //} 

        public async Task<bool> UpdateKidAsync(string id, Kid kid)
        {
            var existingKid = await _context.Kids.FindAsync(id);
            if (existingKid == null) return false;

            existingKid.GameBalance = kid.GameBalance;
            existingKid.AvatarUrl = kid.AvatarUrl;

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
    }
}
