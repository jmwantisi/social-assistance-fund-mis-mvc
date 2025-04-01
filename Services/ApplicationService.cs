using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class ApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application> CreateApplicationAsync(Application application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            application.CreatedAt = DateTime.UtcNow;
            application.UpdatedAt = DateTime.UtcNow;

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return await GetApplicationByIdAsync(application.Id);
        }

        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            return await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Program)
                .Include(a => a.Status)
                .Include(a => a.OfficialRecord).ThenInclude(or => or.Officer)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);
        }

        public async Task<List<Application>> GetAllApplicationsAsync()
        {
            return await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Program)
                .Include(a => a.Status)
                .Include(a => a.OfficialRecord).ThenInclude(or => or.Officer)
                .Where(a => !a.Removed)
                .ToListAsync();
        }

        public async Task<Application?> UpdateApplicationAsync(int id, Application updatedApplication)
        {
            if (updatedApplication == null)
                throw new ArgumentNullException(nameof(updatedApplication));

            var existingApplication = await _context.Applications.FindAsync(id);
            if (existingApplication == null)
                throw new KeyNotFoundException("Application not found.");

            _context.Entry(existingApplication).CurrentValues.SetValues(updatedApplication);
            existingApplication.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetApplicationByIdAsync(id);
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            application.Removed = true;
            application.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PermanentlyDeleteApplicationAsync(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
