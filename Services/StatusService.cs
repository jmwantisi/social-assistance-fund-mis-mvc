using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace SocialAssistanceFundMisMcv.Services
{
    public interface IStatusService
    {
        Task<StatusDTO> CreateStatusAsync(StatusDTO statusDTO);
        Task<StatusDTO?> GetStatusByIdAsync(int id);
        Task<List<StatusDTO>> GetAllStatusesAsync();
        Task<StatusDTO> UpdateStatusAsync(int id, StatusDTO updatedStatusDTO);
        Task DeleteStatusAsync(int id);
        Task PermanentlyDeleteStatusAsync(int id);
    }
    public class StatusService
    {
        private readonly ApplicationDbContext _context;

        public StatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a Status
        public async Task<StatusDTO> CreateStatusAsync(StatusDTO statusDTO)
        {
            if (statusDTO == null) throw new ArgumentNullException(nameof(statusDTO));

            var status = new Status
            {
                Name = statusDTO.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            return MapToDTO(status);
        }

        // Get a Status by ID
        public async Task<StatusDTO?> GetStatusByIdAsync(int id)
        {
            var status = await FindStatusAsync(id);
            return status is null ? null : MapToDTO(status);
        }

        // Get all Statuses
        public async Task<List<StatusDTO>> GetAllStatusesAsync()
        {
            var statuses = await _context.Statuses
                .Where(s => !s.Removed)
                .ToListAsync();

            return statuses.Select(MapToDTO).ToList();
        }

        // Update a Status
        public async Task<StatusDTO> UpdateStatusAsync(int id, StatusDTO updatedStatusDTO)
        {
            if (updatedStatusDTO == null) throw new ArgumentNullException(nameof(updatedStatusDTO));

            var existingStatus = await FindStatusAsync(id);
            existingStatus.Name = updatedStatusDTO.Name;
            existingStatus.UpdatedAt = DateTime.UtcNow;

            _context.Statuses.Update(existingStatus);
            await _context.SaveChangesAsync();

            return MapToDTO(existingStatus);
        }

        // Soft Delete a Status
        public async Task DeleteStatusAsync(int id)
        {
            var status = await FindStatusAsync(id);

            status.Removed = true;
            status.UpdatedAt = DateTime.UtcNow;

            _context.Statuses.Update(status);
            await _context.SaveChangesAsync();
        }

        // Permanently delete a Status
        public async Task PermanentlyDeleteStatusAsync(int id)
        {
            var status = await FindStatusAsync(id);

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
        }

        // Helper to find a Status by ID
        private async Task<Status> FindStatusAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status == null) throw new KeyNotFoundException("Status not found.");
            return status;
        }

        // Map to DTO for consistency
        private static StatusDTO MapToDTO(Status status) => new()
        {
            Id = status.Id,
            Name = status.Name
        };
    }
}
