using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public interface IMaritalStatusService
    {
        Task<MaritalStatusDTO> CreateMaritalStatusAsync(MaritalStatusDTO maritalStatusDto);
        Task<MaritalStatusDTO?> GetMaritalStatusByIdAsync(int id);
        Task<List<MaritalStatusDTO>> GetAllMaritalStatusesAsync();
        Task<MaritalStatusDTO> UpdateMaritalStatusAsync(int id, MaritalStatusDTO updatedMaritalStatusDto);
        Task<bool> DeleteMaritalStatusAsync(int id);
        Task<bool> PermanentlyDeleteMaritalStatusAsync(int id);
    }
    public class MaritalStatusService : IMaritalStatusService
    {
        private readonly ApplicationDbContext _context;

        public MaritalStatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static MaritalStatusDTO MapToDTO(MaritalStatus maritalStatus) => new()
        {
            Id = maritalStatus.Id,
            Name = maritalStatus.Name,
            Removed = maritalStatus.Removed,
            CreatedAt = maritalStatus.CreatedAt,
            UpdatedAt = maritalStatus.UpdatedAt
        };

        public async Task<MaritalStatusDTO> CreateMaritalStatusAsync(MaritalStatusDTO maritalStatusDto)
        {
            if (maritalStatusDto == null)
                throw new ArgumentNullException(nameof(maritalStatusDto));

            var maritalStatus = new MaritalStatus
            {
                Name = maritalStatusDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.MaritalStatuses.Add(maritalStatus);
            await _context.SaveChangesAsync();

            return MapToDTO(maritalStatus);
        }

        public async Task<MaritalStatusDTO?> GetMaritalStatusByIdAsync(int id)
        {
            var maritalStatus = await _context.MaritalStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(ms => ms.Id == id && !ms.Removed);

            return maritalStatus == null ? null : MapToDTO(maritalStatus);
        }

        public async Task<List<MaritalStatusDTO>> GetAllMaritalStatusesAsync()
        {
            var maritalStatuses = await _context.MaritalStatuses
                .AsNoTracking()
                .Where(ms => !ms.Removed)
                .ToListAsync();

            return maritalStatuses.Select(MapToDTO).ToList();
        }

        public async Task<MaritalStatusDTO> UpdateMaritalStatusAsync(int id, MaritalStatusDTO updatedMaritalStatusDto)
        {
            if (updatedMaritalStatusDto == null)
                throw new ArgumentNullException(nameof(updatedMaritalStatusDto));

            var existingMaritalStatus = await _context.MaritalStatuses.FindAsync(id);
            if (existingMaritalStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            existingMaritalStatus.Name = updatedMaritalStatusDto.Name;
            existingMaritalStatus.UpdatedAt = DateTime.UtcNow;

            _context.MaritalStatuses.Update(existingMaritalStatus);
            await _context.SaveChangesAsync();

            return MapToDTO(existingMaritalStatus);
        }

        public async Task<bool> DeleteMaritalStatusAsync(int id)
        {
            var maritalStatus = await _context.MaritalStatuses.FindAsync(id);
            if (maritalStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            maritalStatus.Removed = true;
            maritalStatus.UpdatedAt = DateTime.UtcNow;

            _context.MaritalStatuses.Update(maritalStatus);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PermanentlyDeleteMaritalStatusAsync(int id)
        {
            var maritalStatus = await _context.MaritalStatuses.FindAsync(id);
            if (maritalStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            _context.MaritalStatuses.Remove(maritalStatus);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
