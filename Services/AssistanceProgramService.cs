using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public interface IAssistanceProgramService
    {
        Task<AssistanceProgramDTO> CreateAssistanceProgramAsync(AssistanceProgramDTO dto);
        Task<AssistanceProgramDTO?> GetAssistanceProgramByIdAsync(int id);
        Task<List<AssistanceProgramDTO>> GetAllAssistanceProgramsAsync();
        Task<AssistanceProgramDTO> UpdateAssistanceProgramAsync(int id, AssistanceProgramDTO dto);
        Task<bool> DeleteAssistanceProgramAsync(int id);
        Task<bool> PermanentlyDeleteAssistanceProgramAsync(int id);
    }

    public class AssistanceProgramService : IAssistanceProgramService
    {
        private readonly ApplicationDbContext _context;

        public AssistanceProgramService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AssistanceProgramDTO> CreateAssistanceProgramAsync(AssistanceProgramDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var program = new AssistanceProgram
            {
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AssistancePrograms.Add(program);
            await _context.SaveChangesAsync();

            return MapToDTO(program);
        }

        public async Task<AssistanceProgramDTO?> GetAssistanceProgramByIdAsync(int id)
        {
            var program = await _context.AssistancePrograms
                .Where(ap => ap.Id == id && !ap.Removed)
                .FirstOrDefaultAsync();

            return program == null ? null : MapToDTO(program);
        }

        public async Task<List<AssistanceProgramDTO>> GetAllAssistanceProgramsAsync()
        {
            return await _context.AssistancePrograms
                .Where(ap => !ap.Removed)
                .Select(ap => MapToDTO(ap))
                .ToListAsync();
        }

        public async Task<AssistanceProgramDTO> UpdateAssistanceProgramAsync(int id, AssistanceProgramDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var program = await _context.AssistancePrograms.FindAsync(id);
            if (program == null) throw new KeyNotFoundException("AssistanceProgram not found.");

            program.Name = dto.Name;
            program.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDTO(program);
        }

        public async Task<bool> DeleteAssistanceProgramAsync(int id)
        {
            var program = await _context.AssistancePrograms.FindAsync(id);
            if (program == null) throw new KeyNotFoundException("AssistanceProgram not found.");

            program.Removed = true;
            program.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PermanentlyDeleteAssistanceProgramAsync(int id)
        {
            var program = await _context.AssistancePrograms.FindAsync(id);
            if (program == null) throw new KeyNotFoundException("AssistanceProgram not found.");

            _context.AssistancePrograms.Remove(program);
            await _context.SaveChangesAsync();
            return true;
        }

        private static AssistanceProgramDTO MapToDTO(AssistanceProgram program)
        {
            return new AssistanceProgramDTO
            {
                Id = program.Id,
                Name = program.Name,
                Removed = program.Removed,
                CreatedAt = program.CreatedAt,
                UpdatedAt = program.UpdatedAt
            };
        }
    }
}
