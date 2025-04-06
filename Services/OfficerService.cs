using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;

namespace SocialAssistanceFundMisMcv.Services
{
    public interface IOfficerService
    {
        Task<IEnumerable<Officer>> GetAllAsync();
        Task<Officer?> GetByIdAsync(int id);
        Task<Officer> CreateAsync(Officer officer);
        Task<Officer> UpdateAsync(int id, Officer officer);
        Task<bool> DeleteAsync(int id);
    }
    public class OfficerService : IOfficerService
    {
        private readonly ApplicationDbContext _context;

        public OfficerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Officer>> GetAllAsync()
        {
            return await _context.Officers
                .Where(o => !o.Removed)
                .Include(o => o.Designation)
                .ToListAsync();
        }

        public async Task<Officer?> GetByIdAsync(int id)
        {
            return await _context.Officers
                .Include(o => o.Designation)
                .FirstOrDefaultAsync(o => o.Id == id && !o.Removed);
        }

        public async Task<Officer> CreateAsync(Officer officer)
        {
            officer.CreatedAt = DateTime.UtcNow;
            officer.UpdatedAt = DateTime.UtcNow;

            _context.Officers.Add(officer);
            await _context.SaveChangesAsync();
            return officer;
        }

        public async Task<Officer> UpdateAsync(int id, Officer updatedOfficer)
        {
            var existingOfficer = await _context.Officers.FindAsync(id);
            if (existingOfficer == null || existingOfficer.Removed)
                throw new KeyNotFoundException("Officer not found.");

            existingOfficer.FirstName = updatedOfficer.FirstName;
            existingOfficer.MiddleName = updatedOfficer.MiddleName;
            existingOfficer.LastName = updatedOfficer.LastName;
            existingOfficer.DesignationId = updatedOfficer.DesignationId;
            existingOfficer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingOfficer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var officer = await _context.Officers.FindAsync(id);
            if (officer == null || officer.Removed)
                throw new KeyNotFoundException("Officer not found.");

            officer.Removed = true;
            officer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
