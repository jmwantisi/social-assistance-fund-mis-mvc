using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Data;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public interface IApplicantService
    {
        Task<Applicant> CreateApplicantAsync(Applicant applicant, List<(string phoneNumber, int phoneNumberTypeId)> phoneNumbers);
        Task<Applicant> GetApplicantByIdAsync(int id);
        Task<List<Applicant>> GetAllApplicantsAsync();
        Task<Applicant> UpdateApplicantAsync(int id, Applicant applicant, List<string> phoneNumbers);
        Task<bool> DeleteApplicantAsync(int id);
        Task<bool> DeletePhoneNumberAsync(int applicantId, int phoneNumberId);
    }

    public class ApplicantService : IApplicantService
    {
        private readonly ApplicationDbContext _context;

        public ApplicantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Applicant> CreateApplicantAsync(Applicant applicant, List<(string phoneNumber, int phoneNumberTypeId)> phoneNumbers)
        {
            if (applicant == null)
                throw new ArgumentNullException(nameof(applicant));

            applicant.CreatedAt = DateTime.UtcNow;
            applicant.UpdatedAt = DateTime.UtcNow;

            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();

            if (phoneNumbers != null && phoneNumbers.Any())
            {
                foreach (var (phoneNumber, phoneNumberTypeId) in phoneNumbers)
                {
                    var newPhoneNumber = new ApplicantPhoneNumber
                    {
                        PhoneNumber = phoneNumber,
                        PhoneNumberTypeId = phoneNumberTypeId,
                        ApplicantId = applicant.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.ApplicantPhoneNumbers.Add(newPhoneNumber);
                }
                await _context.SaveChangesAsync();
            }

            return applicant;
        }


        public async Task<Applicant> GetApplicantByIdAsync(int id)
        {
            return await _context.Applicants
                .Include(a => a.PhoneNumbers.Where(p => !p.Removed))
                .Include(a => a.Sex)
                .Include(a => a.MaritialStatus)
                .Include(a => a.Village)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);
        }

        public async Task<List<Applicant>> GetAllApplicantsAsync()
        {
            return await _context.Applicants
                .Include(a => a.PhoneNumbers.Where(p => !p.Removed))
                .Where(a => !a.Removed)
                .ToListAsync();
        }

        public async Task<Applicant> UpdateApplicantAsync(int id, Applicant updatedApplicant, List<string> phoneNumbers)
        {
            var existingApplicant = await _context.Applicants
                .Include(a => a.PhoneNumbers)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (existingApplicant == null)
                return null;

            existingApplicant.FirstName = updatedApplicant.FirstName;
            existingApplicant.LastName = updatedApplicant.LastName;
            existingApplicant.SexId = updatedApplicant.SexId;
            existingApplicant.Dob = updatedApplicant.Dob;
            existingApplicant.MaritialStatusId = updatedApplicant.MaritialStatusId;
            existingApplicant.VillageId = updatedApplicant.VillageId;
            existingApplicant.IdentityCardNumber = updatedApplicant.IdentityCardNumber;
            existingApplicant.PostalAddress = updatedApplicant.PostalAddress;
            existingApplicant.PhysicalAddress = updatedApplicant.PhysicalAddress;
            existingApplicant.UpdatedAt = DateTime.UtcNow;

            if (phoneNumbers != null && phoneNumbers.Any())
            {
                existingApplicant.PhoneNumbers.Clear();
                foreach (var phoneNumber in phoneNumbers)
                {
                    var newPhoneNumber = new ApplicantPhoneNumber
                    {
                        PhoneNumber = phoneNumber,
                        ApplicantId = id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    existingApplicant.PhoneNumbers.Add(newPhoneNumber);
                }
            }

            await _context.SaveChangesAsync();
            return existingApplicant;
        }

        public async Task<bool> DeleteApplicantAsync(int id)
        {
            var applicant = await _context.Applicants
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (applicant == null)
                return false;

            applicant.Removed = true;
            applicant.UpdatedAt = DateTime.UtcNow;

            foreach (var phoneNumber in applicant.PhoneNumbers)
            {
                phoneNumber.Removed = true;
                phoneNumber.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePhoneNumberAsync(int applicantId, int phoneNumberId)
        {
            var phoneNumberEntity = await _context.ApplicantPhoneNumbers
                .FirstOrDefaultAsync(p => p.Id == phoneNumberId && p.ApplicantId == applicantId && !p.Removed);

            if (phoneNumberEntity == null)
                return false;

            phoneNumberEntity.Removed = true;
            phoneNumberEntity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
