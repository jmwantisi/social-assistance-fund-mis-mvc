using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace socialAssistanceFundMIS.Services
{
    public interface IGeographicLocationService
    {
        Task<List<GeographicLocation>> GetVillagesWithHierarchyAsync();
        Task<string> GetVillageHierarchyByIdAsync(int? villageId);
        Task<GeographicLocation> CreateGeographicLocationAsync(GeographicLocation location);
        Task<GeographicLocation?> GetGeographicLocationByIdAsync(int id);
        Task<List<GeographicLocation>> GetAllGeographicLocationsAsync();
        Task<GeographicLocation> UpdateGeographicLocationAsync(int id, GeographicLocation location);
        Task DeleteGeographicLocationAsync(int id);
        Task PermanentlyDeleteGeographicLocationAsync(int id);
    }
    public class GeographicLocationService : IGeographicLocationService
    {
        private readonly ApplicationDbContext _context;

        public GeographicLocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GeographicLocation>> GetVillagesWithHierarchyAsync()
        {
            var villages = await _context.GeographicLocations
                .Where(gl => gl.GeographicLocationType.Name == "Village")
                .Include(gl => gl.GeographicLocationParent) // Sub-Location
                    .ThenInclude(subLoc => subLoc.GeographicLocationParent) // Location
                        .ThenInclude(loc => loc.GeographicLocationParent) // Sub-County
                            .ThenInclude(subCounty => subCounty.GeographicLocationParent) // County
                .AsNoTracking() // Prevents EF from tracking changes
                .ToListAsync();

            // ParentLocations are coming out blank
            var data = villages.Select(village => new GeographicLocation
            {
                Id = village.Id,
                Name = string.Join(", ", new[] { village.Name, village.GeographicLocationParent?.Name,
                                         village.GeographicLocationParent?.GeographicLocationParent?.Name,
                                         village.GeographicLocationParent?.GeographicLocationParent?.GeographicLocationParent?.Name,
                                         village.GeographicLocationParent?.GeographicLocationParent?.GeographicLocationParent?.GeographicLocationParent?.Name }
                                    .Where(name => !string.IsNullOrEmpty(name))),
                GeographicLocationTypeId = village.GeographicLocationTypeId,
                GeographicLocationParentId = village.GeographicLocationParentId,
                CreatedAt = village.CreatedAt,
                UpdatedAt = village.UpdatedAt,
                Removed = village.Removed
            }).ToList();

            return data;
        }

        public async Task<string> GetVillageHierarchyByIdAsync(int? villageId)
        {
            if (villageId == null)
            {
                return "Village ID is required";
            }

            var village = await _context.GeographicLocations
                .Where(gl => gl.Id == villageId && gl.GeographicLocationType.Name == "Village")
                .Include(gl => gl.GeographicLocationParent) // Sub-Location
                    .ThenInclude(subLoc => subLoc.GeographicLocationParent) // Location
                        .ThenInclude(loc => loc.GeographicLocationParent) // Sub-County
                            .ThenInclude(subCounty => subCounty.GeographicLocationParent) // County
                .AsNoTracking() // Prevents EF from tracking changes
                .FirstOrDefaultAsync();

            if (village == null)
            {
                return "Village not found";
            }

            return FormatLocationHierarchy(village); // Return formatted hierarchy as string
        }

        // Helper function to format hierarchy properly
        private string FormatLocationHierarchy(GeographicLocation village)
        {
            var names = new List<string>();

            if (!string.IsNullOrEmpty(village.Name)) names.Add(village.Name);
            if (village.GeographicLocationParent != null)
            {
                if (!string.IsNullOrEmpty(village.GeographicLocationParent.Name))
                    names.Add(village.GeographicLocationParent.Name);

                if (village.GeographicLocationParent.GeographicLocationParent != null)
                {
                    if (!string.IsNullOrEmpty(village.GeographicLocationParent.GeographicLocationParent.Name))
                        names.Add(village.GeographicLocationParent.GeographicLocationParent.Name);

                    if (village.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent != null)
                    {
                        if (!string.IsNullOrEmpty(village.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent.Name))
                            names.Add(village.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent.Name);

                        if (village.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent != null)
                        {
                            if (!string.IsNullOrEmpty(village.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent.Name))
                                names.Add(village.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent.GeographicLocationParent.Name);
                        }
                    }
                }
            }

            return string.Join(", ", names); // Return formatted hierarchy as a string
        }




        public async Task<GeographicLocation> CreateGeographicLocationAsync(GeographicLocation location)
        {
            if (location == null) throw new ArgumentNullException(nameof(location));

            _context.GeographicLocations.Add(location);
            await _context.SaveChangesAsync();

            return location;
        }

        public async Task<GeographicLocation?> GetGeographicLocationByIdAsync(int id)
        {
            return await _context.GeographicLocations
                .AsNoTracking()
                .Include(gl => gl.GeographicLocationParent)
                .ThenInclude(subLoc => subLoc.GeographicLocationParent)
                .ThenInclude(loc => loc.GeographicLocationParent)
                .ThenInclude(subCounty => subCounty.GeographicLocationParent)
                .FirstOrDefaultAsync(gl => gl.Id == id && !gl.Removed);
        }

        public async Task<List<GeographicLocation>> GetAllGeographicLocationsAsync()
        {
            return await _context.GeographicLocations
                .AsNoTracking()
                .Where(gl => !gl.Removed)
                .Include(gl => gl.GeographicLocationParent)
                .ThenInclude(subLoc => subLoc.GeographicLocationParent)
                .ThenInclude(loc => loc.GeographicLocationParent)
                .ThenInclude(subCounty => subCounty.GeographicLocationParent)
                .ToListAsync();
        }

        public async Task<GeographicLocation> UpdateGeographicLocationAsync(int id, GeographicLocation location)
        {
            if (location == null) throw new ArgumentNullException(nameof(location));

            var entity = await _context.GeographicLocations.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("Geographic Location not found.");

            entity.Name = location.Name;
            entity.GeographicLocationTypeId = location.GeographicLocationTypeId;
            entity.GeographicLocationParentId = location.GeographicLocationParentId;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.GeographicLocations.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteGeographicLocationAsync(int id)
        {
            var entity = await _context.GeographicLocations.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("Geographic Location not found.");

            entity.Removed = true;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.GeographicLocations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task PermanentlyDeleteGeographicLocationAsync(int id)
        {
            var entity = await _context.GeographicLocations.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("Geographic Location not found.");

            _context.GeographicLocations.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
