using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;

namespace SocialAssistanceFundMisMcv.Services
{
    public class LookupService
    {
        private readonly ApplicationDbContext _context;

        public LookupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssistanceProgram>> GetProgramsAsync()
        {
            return await _context.AssistancePrograms.ToListAsync();
        }

        public async Task<IEnumerable<Sex>> GetSexesAsync()
        {
            return await _context.Sexes.ToListAsync();
        }

        public async Task<IEnumerable<MaritalStatus>> GetMaritalStatusesAsync()
        {
            return await _context.MaritalStatuses.ToListAsync();
        }

        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<IEnumerable<PhoneNumberType>> GetPhoneNumberTypesAsync()
        {
            return await _context.PhoneNumberTypes.ToListAsync();
        }
    }
}
