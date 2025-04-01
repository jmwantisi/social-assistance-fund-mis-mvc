using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class OfficialRecordService
    {
        private readonly ApplicationDbContext _context;

        public OfficialRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create an OfficialRecord from DTO
        public async Task<OfficialRecordDTO> CreateOfficialRecordAsync(OfficialRecordDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var officialRecord = new OfficialRecord
            {
                OfficerId = dto.OfficerId,
                OfficiationDate = dto.OfficiationDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.OfficialRecords.Add(officialRecord);
            await _context.SaveChangesAsync();

            return MapToDTO(officialRecord);
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
