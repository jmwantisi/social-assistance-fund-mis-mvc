using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public interface IPhoneNumberTypeService
    {
        Task<PhoneNumberTypeDTO> CreatePhoneNumberTypeAsync(PhoneNumberTypeDTO dto);
        Task<PhoneNumberTypeDTO?> GetPhoneNumberTypeByIdAsync(int id);
        Task<List<PhoneNumberTypeDTO>> GetAllPhoneNumberTypesAsync();
        Task<PhoneNumberTypeDTO> UpdatePhoneNumberTypeAsync(int id, PhoneNumberTypeDTO dto);
        Task<bool> DeletePhoneNumberTypeAsync(int id);
        Task<bool> PermanentlyDeletePhoneNumberTypeAsync(int id);
    }
    public class PhoneNumberTypeService : IPhoneNumberTypeService
    {
        private readonly ApplicationDbContext _context;

        public PhoneNumberTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PhoneNumberTypeDTO> CreatePhoneNumberTypeAsync(PhoneNumberTypeDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var phoneNumberType = new PhoneNumberType
            {
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PhoneNumberTypes.Add(phoneNumberType);
            await _context.SaveChangesAsync();

            return MapToDTO(phoneNumberType);
        }

        public async Task<PhoneNumberTypeDTO?> GetPhoneNumberTypeByIdAsync(int id)
        {
            var phoneNumberType = await _context.PhoneNumberTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(pnt => pnt.Id == id && !pnt.Removed);

            return phoneNumberType != null ? MapToDTO(phoneNumberType) : null;
        }

        public async Task<List<PhoneNumberTypeDTO>> GetAllPhoneNumberTypesAsync()
        {
            var phoneNumberTypes = await _context.PhoneNumberTypes
                .AsNoTracking()
                .Where(pnt => !pnt.Removed)
                .ToListAsync();

            return phoneNumberTypes.Select(MapToDTO).ToList();
        }

        public async Task<PhoneNumberTypeDTO> UpdatePhoneNumberTypeAsync(int id, PhoneNumberTypeDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var phoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);
            if (phoneNumberType == null) throw new KeyNotFoundException("Phone Number Type not found.");

            phoneNumberType.Name = dto.Name;
            phoneNumberType.UpdatedAt = DateTime.UtcNow;

            _context.PhoneNumberTypes.Update(phoneNumberType);
            await _context.SaveChangesAsync();

            return MapToDTO(phoneNumberType);
        }

        public async Task<bool> DeletePhoneNumberTypeAsync(int id)
        {
            var phoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);
            if (phoneNumberType == null) throw new KeyNotFoundException("Phone Number Type not found.");

            phoneNumberType.Removed = true;
            phoneNumberType.UpdatedAt = DateTime.UtcNow;

            _context.PhoneNumberTypes.Update(phoneNumberType);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PermanentlyDeletePhoneNumberTypeAsync(int id)
        {
            var phoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);
            if (phoneNumberType == null) throw new KeyNotFoundException("Phone Number Type not found.");

            _context.PhoneNumberTypes.Remove(phoneNumberType);
            await _context.SaveChangesAsync();

            return true;
        }

        private static PhoneNumberTypeDTO MapToDTO(PhoneNumberType entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Removed = entity.Removed,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
