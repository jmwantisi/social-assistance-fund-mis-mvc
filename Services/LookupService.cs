using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;

namespace SocialAssistanceFundMisMcv.Services
{
    public interface ILookupService
    {
        Task<IEnumerable<AssistanceProgram>> GetProgramsAsync();
        Task<AssistanceProgram?> GetProgramByIdAsync(int id);
        Task<IEnumerable<Sex>> GetSexesAsync();
        Task<Sex?> GetSexByIdAsync(int id);
        Task<IEnumerable<MaritalStatus>> GetMaritalStatusesAsync();
        Task<MaritalStatus?> GetMaritalStatusByIdAsync(int id);
        Task<IEnumerable<Status>> GetStatusesAsync();
        Task<Status?> GetStatusByIdAsync(int id);
        Task<IEnumerable<PhoneNumberType>> GetPhoneNumberTypesAsync();
        Task<PhoneNumberType?> GetPhoneNumberTypeByIdAsync(int id);
    }
    public class LookupService : ILookupService
    {
        private readonly ApplicationDbContext _context;

        public LookupService(ApplicationDbContext context)
        {
            _context = context;
        }

        // AssistancePrograms
        public async Task<IEnumerable<AssistanceProgram>> GetProgramsAsync()
        {
            return await _context.AssistancePrograms.ToListAsync();
        }

        public async Task<AssistanceProgram?> GetProgramByIdAsync(int id)
        {
            return await _context.AssistancePrograms.FindAsync(id);
        }

        // Sexes
        public async Task<IEnumerable<Sex>> GetSexesAsync()
        {
            return await _context.Sexes.ToListAsync();
        }

        public async Task<Sex?> GetSexByIdAsync(int id)
        {
            return await _context.Sexes.FindAsync(id);
        }

        // MaritalStatuses
        public async Task<IEnumerable<MaritalStatus>> GetMaritalStatusesAsync()
        {
            return await _context.MaritalStatuses.ToListAsync();
        }

        public async Task<MaritalStatus?> GetMaritalStatusByIdAsync(int id)
        {
            return await _context.MaritalStatuses.FindAsync(id);
        }

        // Statuses
        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<Status?> GetStatusByIdAsync(int id)
        {
            return await _context.Statuses.FindAsync(id);
        }

        // PhoneNumberTypes
        public async Task<IEnumerable<PhoneNumberType>> GetPhoneNumberTypesAsync()
        {
            return await _context.PhoneNumberTypes.ToListAsync();
        }

        public async Task<PhoneNumberType?> GetPhoneNumberTypeByIdAsync(int id)
        {
            return await _context.PhoneNumberTypes.FindAsync(id);
        }
    }
}
