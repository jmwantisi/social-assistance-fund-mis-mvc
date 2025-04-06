using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;

namespace SocialAssistanceFundMisMcv.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(int Pending, int Approved)> GetApplicationStatusCountsAsync()
        {
            var pending = await _context.Applications.CountAsync(a => a.Status != null && a.Status.Name == "Pending" && !a.Removed);
            var approved = await _context.Applications.CountAsync(a => a.Status != null && a.Status.Name == "Approved" && !a.Removed);
            return (pending, approved);
        }

        public async Task<(int Male, int Female)> GetApprovedApplicantsByGenderAsync()
        {
            var approvedApplications = await _context.Applications
                .Include(a => a.Applicant).ThenInclude(ap => ap.Sex)
                .Include(a => a.Status)
                .Where(a => a.Status != null && a.Status.Name == "Approved" && !a.Removed)
                .ToListAsync();

            var male = approvedApplications.Count(a => a.Applicant?.Sex?.Name == "Male");
            var female = approvedApplications.Count(a => a.Applicant?.Sex?.Name == "Female");

            return (male, female);
        }

        public async Task<Dictionary<string, int>> GetApplicationCountsPerProgramAsync()
        {
            return await _context.Applications
                .Include(a => a.Program)
                .Where(a => !a.Removed)
                .GroupBy(a => a.Program!.Name!)
                .Select(g => new { ProgramName = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.ProgramName, g => g.Count);
        }
    }
}
