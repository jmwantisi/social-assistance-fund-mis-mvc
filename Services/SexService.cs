using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace socialAssistanceFundMIS.Services
{
    public interface ISexService
    {
        Task<SexDTO> CreateSexAsync(SexDTO sexDto);
        Task<SexDTO?> GetSexByIdAsync(int id);
        Task<List<SexDTO>> GetAllSexesAsync();
        Task<SexDTO> UpdateSexAsync(int id, SexDTO updatedSexDto);
        Task<bool> DeleteSexAsync(int id);
        Task<bool> PermanentlyDeleteSexAsync(int id);
    }
    public class SexService : ISexService
    {
        private readonly ApplicationDbContext _context;

        public SexService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a Sex from DTO
        public async Task<SexDTO> CreateSexAsync(SexDTO sexDto)
        {
            if (sexDto == null) throw new ArgumentNullException(nameof(sexDto));

            var sex = new Sex
            {
                Name = sexDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Sexes.Add(sex);
            await _context.SaveChangesAsync();

            return MapToDTO(sex);
        }

        // Get a Sex by ID
        public async Task<SexDTO?> GetSexByIdAsync(int id)
        {
            var sex = await FindSexAsync(id);
            return sex is null ? null : MapToDTO(sex);
        }

        // Get all Sexes
        public async Task<List<SexDTO>> GetAllSexesAsync()
        {
            var sexes = await _context.Sexes
                .Where(s => !s.Removed)
                .ToListAsync();

            return sexes.Select(MapToDTO).ToList();
        }

        // Update a Sex
        public async Task<SexDTO> UpdateSexAsync(int id, SexDTO updatedSexDto)
        {
            if (updatedSexDto == null) throw new ArgumentNullException(nameof(updatedSexDto));

            var existingSex = await FindSexAsync(id);
            existingSex.Name = updatedSexDto.Name;
            existingSex.UpdatedAt = DateTime.UtcNow;

            _context.Sexes.Update(existingSex);
            await _context.SaveChangesAsync();

            return MapToDTO(existingSex);
        }

        // Soft Delete a Sex
        public async Task<bool> DeleteSexAsync(int id)
        {
            var sex = await FindSexAsync(id);

            sex.Removed = true;
            sex.UpdatedAt = DateTime.UtcNow;

            _context.Sexes.Update(sex);
            await _context.SaveChangesAsync();

            return true;
        }

        // Permanently delete a Sex
        public async Task<bool> PermanentlyDeleteSexAsync(int id)
        {
            var sex = await FindSexAsync(id);

            _context.Sexes.Remove(sex);
            await _context.SaveChangesAsync();

            return true;
        }

        // Helper to find a Sex by ID
        private async Task<Sex> FindSexAsync(int id)
        {
            var sex = await _context.Sexes.FindAsync(id);
            if (sex == null) throw new KeyNotFoundException("Sex not found.");
            return sex;
        }

        // Map to DTO for consistency
        private static SexDTO MapToDTO(Sex sex) => new()
        {
            Id = sex.Id,
            Name = sex.Name,
            Removed = sex.Removed,
            CreatedAt = sex.CreatedAt,
            UpdatedAt = sex.UpdatedAt
        };
    }
}
