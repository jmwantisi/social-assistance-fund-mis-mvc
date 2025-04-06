using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public interface IDesignationService
    {
        Task<DesignationDTO> CreateDesignationAsync(DesignationDTO designationDto);
        Task<DesignationDTO?> GetDesignationByIdAsync(int id);
        Task<List<DesignationDTO>> GetAllDesignationsAsync();
        Task<DesignationDTO> UpdateDesignationAsync(int id, DesignationDTO updatedDesignationDto);
        Task<bool> DeleteDesignationAsync(int id);
        Task<bool> PermanentlyDeleteDesignationAsync(int id);
    }
    public class DesignationService : IDesignationService
    {
        private readonly ApplicationDbContext _context;

        public DesignationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DesignationDTO> CreateDesignationAsync(DesignationDTO designationDto)
        {
            if (designationDto == null)
                throw new ArgumentNullException(nameof(designationDto));

            var designation = new Designation
            {
                Name = designationDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Designations.Add(designation);
            await _context.SaveChangesAsync();

            return MapToDTO(designation);
        }

        public async Task<DesignationDTO?> GetDesignationByIdAsync(int id)
        {
            var designation = await _context.Designations
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && !d.Removed);

            return designation != null ? MapToDTO(designation) : null;
        }

        public async Task<List<DesignationDTO>> GetAllDesignationsAsync()
        {
            return await _context.Designations
                .Where(d => !d.Removed)
                .Select(d => MapToDTO(d))
                .ToListAsync();
        }

        public async Task<DesignationDTO> UpdateDesignationAsync(int id, DesignationDTO updatedDesignationDto)
        {
            if (updatedDesignationDto == null)
                throw new ArgumentNullException(nameof(updatedDesignationDto));

            var existingDesignation = await _context.Designations.FindAsync(id);
            if (existingDesignation == null)
                throw new KeyNotFoundException("Designation not found.");

            existingDesignation.Name = updatedDesignationDto.Name;
            existingDesignation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDTO(existingDesignation);
        }

        public async Task<bool> DeleteDesignationAsync(int id)
        {
            var designation = await _context.Designations.FindAsync(id);
            if (designation == null)
                throw new KeyNotFoundException("Designation not found.");

            designation.Removed = true;
            designation.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PermanentlyDeleteDesignationAsync(int id)
        {
            var designation = await _context.Designations.FindAsync(id);
            if (designation == null)
                throw new KeyNotFoundException("Designation not found.");

            _context.Designations.Remove(designation);
            await _context.SaveChangesAsync();

            return true;
        }

        private static DesignationDTO MapToDTO(Designation designation) => new DesignationDTO
        {
            Id = designation.Id,
            Name = designation.Name,
            Removed = designation.Removed,
            CreatedAt = designation.CreatedAt,
            UpdatedAt = designation.UpdatedAt
        };
    }
}
