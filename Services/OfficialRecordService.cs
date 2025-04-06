using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;
using SocialAssistanceFundMisMcv.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace socialAssistanceFundMIS.Services
{
    public interface IOfficialRecordService
    {
        Task<OfficialRecord> CreateOfficialRecordAsync();
        Task<OfficialRecordDTO?> GetOfficialRecordByIdAsync(int id);
        Task<List<OfficialRecordDTO>> GetAllOfficialRecordsAsync();
        Task<OfficialRecordDTO> UpdateOfficialRecordAsync(int id, OfficialRecordDTO dto);
        Task<bool> DeleteOfficialRecordAsync(int id);
        Task<bool> PermanentlyDeleteOfficialRecordAsync(int id);
    }
    public class OfficialRecordService : IOfficialRecordService
    {
        private readonly ApplicationDbContext _context;

        private readonly OfficerService _officerService;

        public OfficialRecordService(ApplicationDbContext context, OfficerService officerService)
        {
            _context = context;
            _officerService = officerService;
        }

        // Create an OfficialRecord from DTO
        // OfficialRecord will come in as user who logged in
        // Authentication no a requirement for this assignement
        public async Task<OfficialRecord> CreateOfficialRecordAsync()
        {
            var officialRecord = new OfficialRecord();
            var existingOfficer = await _officerService.GetByIdAsync(1);
            if (existingOfficer == null)
                throw new Exception("Officer not found");

            officialRecord.Officer = existingOfficer;
            officialRecord.OfficiationDate = DateTime.UtcNow;
            officialRecord.CreatedAt = DateTime.UtcNow;
            officialRecord.UpdatedAt = DateTime.UtcNow;

            _context.OfficialRecords.Add(officialRecord);
            await _context.SaveChangesAsync();

            return officialRecord;
        }

        // Get an OfficialRecord by ID
        public async Task<OfficialRecordDTO?> GetOfficialRecordByIdAsync(int id)
        {
            var record = await _context.OfficialRecords
                .AsNoTracking()
                .Include(o => o.Officer)
                .FirstOrDefaultAsync(o => o.Id == id && !o.Removed);

            return record == null ? null : MapToDTO(record);
        }

        // Get all OfficialRecords
        public async Task<List<OfficialRecordDTO>> GetAllOfficialRecordsAsync()
        {
            var records = await _context.OfficialRecords
                .AsNoTracking()
                .Include(o => o.Officer)
                .Where(o => !o.Removed)
                .ToListAsync();

            return records.Select(MapToDTO).ToList();
        }

        // Update an OfficialRecord
        public async Task<OfficialRecordDTO> UpdateOfficialRecordAsync(int id, OfficialRecordDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var record = await _context.OfficialRecords.FindAsync(id);
            if (record == null) throw new KeyNotFoundException("OfficialRecord not found.");

            record.OfficerId = dto.OfficerId;
            record.OfficiationDate = dto.OfficiationDate;
            record.UpdatedAt = DateTime.UtcNow;

            _context.OfficialRecords.Update(record);
            await _context.SaveChangesAsync();

            return MapToDTO(record);
        }

        // Soft Delete an OfficialRecord
        public async Task<bool> DeleteOfficialRecordAsync(int id)
        {
            var record = await _context.OfficialRecords.FindAsync(id);
            if (record == null) throw new KeyNotFoundException("OfficialRecord not found.");

            record.Removed = true;
            record.UpdatedAt = DateTime.UtcNow;

            _context.OfficialRecords.Update(record);
            await _context.SaveChangesAsync();
            return true;
        }

        // Permanently delete an OfficialRecord
        public async Task<bool> PermanentlyDeleteOfficialRecordAsync(int id)
        {
            var record = await _context.OfficialRecords.FindAsync(id);
            if (record == null) throw new KeyNotFoundException("OfficialRecord not found.");

            _context.OfficialRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        // Utility: Map entity to DTO
        private static OfficialRecordDTO MapToDTO(OfficialRecord record) => new OfficialRecordDTO
        {
            Id = record.Id,
            OfficerId = record.OfficerId,
            OfficiationDate = record.OfficiationDate,
            Removed = record.Removed,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt
        };
    }
}
