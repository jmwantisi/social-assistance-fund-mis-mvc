using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;
using SocialAssistanceFundMisMcv.Services;

namespace socialAssistanceFundMIS.Services
{

    public interface IApplicationService
    {
        Task<Application> CreateApplicationAsync(Application application);
        Task<Application?> GetApplicationByIdAsync(int id);
        Task<List<Application>> GetAllApplicationsAsync();
        Task<Application?> UpdateApplicationAsync(int id, Application updatedApplication);
        Task<bool> ApproveApplicationAsync(int id, int statusId);
        Task<bool> DeleteApplicationAsync(int id);
        Task<bool> PermanentlyDeleteApplicationAsync(int id);
    }


    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly OfficialRecordService _officialRecordService;
        private readonly EmailService _emailService;
        private readonly ApplicantService _applicantService;

        public ApplicationService(ApplicationDbContext context, OfficialRecordService officialRecordService, EmailService emailService, ApplicantService applicantService)
        {
            _context = context;
            _officialRecordService = officialRecordService;
            _emailService = emailService;
            _applicantService = applicantService;
        }

        public async Task<Application> CreateApplicationAsync(Application application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            // Load existing Applicant from DB
            var existingApplicant = await _context.Applicants.FindAsync(application?.ApplicantId);
            if (existingApplicant == null)
                throw new Exception("Applicant not found");

            var existingProgram = await _context.AssistancePrograms.FindAsync(application?.ProgramId);
            if (existingProgram == null)
                throw new Exception("Applicant not found");

            application.Applicant = existingApplicant; // Assign existing Applicant
            application.Program = existingProgram; // Assign existing Program
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
                .ThenInclude(village => village.Village)
                .Include(a => a.Program)
                .Include(a => a.Status)
                .Include(a => a.OfficialRecord)
                .ThenInclude(or => or.Officer)
                .ThenInclude(designation => designation.Designation)
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

            existingApplication.ApplicationDate = updatedApplication.ApplicationDate;
            existingApplication.DeclarationDate = updatedApplication.DeclarationDate;
            existingApplication.ProgramId = updatedApplication.ProgramId;
            existingApplication.ApplicantId = updatedApplication.ApplicantId;
            existingApplication.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetApplicationByIdAsync(id);
        }

        public async Task<bool> ApproveApplicationAsync(int id, int statusId)
        {
            // set official record here also
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            var status = await _context.Statuses.FindAsync(statusId);
            if (status == null)
                throw new KeyNotFoundException("Status not found.");

            application.Status = status; // assign the whole entity, not just the ID
            application.UpdatedAt = DateTime.UtcNow;
            var officialRecord = await _officialRecordService.CreateOfficialRecordAsync();

            application.OfficialRecord = officialRecord;

            if(statusId == 2)
            {
                var applicant = await _applicantService.GetApplicantByIdAsync(application.ApplicantId);
                var subject = "Approval For Social Assistance";
                var message = "Dear " + application?.Applicant?.FirstName + ", \nYour application for Social Assistance has been approved!";
                await _emailService.SendEmailAsync(applicant?.Email, subject, message);
            }

            await _context.SaveChangesAsync();
            return true;
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
