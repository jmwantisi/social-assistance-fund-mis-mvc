using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Seeders
{
    public static class GeographicLocationSeeder
    {
        public static void SeedGeographicLocations(ApplicationDbContext context)
        {
            if (!context.GeographicLocationTypes.Any())
            {
                var locationTypes = new List<GeographicLocationType>
                {
                    new() { Name = "County", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Sub-County", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Location", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Sub-Location", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Village", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };

                context.GeographicLocationTypes.AddRange(locationTypes);
                context.SaveChanges();
            }

            if (!context.GeographicLocations.Any())
            {
                var counties = new List<GeographicLocation>
                {
                    new() { Name = "County A", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "County B", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.GeographicLocations.AddRange(counties);
                context.SaveChanges();

                var subCounties = new List<GeographicLocation>
                {
                    new() { Name = "Sub-County A1", GeographicLocationTypeId = 2, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Sub-County B1", GeographicLocationTypeId = 2, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.GeographicLocations.AddRange(subCounties);
                context.SaveChanges();

                var locations = new List<GeographicLocation>
                {
                    new() { Name = "Location A1-1", GeographicLocationTypeId = 3, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Location B1-1", GeographicLocationTypeId = 3, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.GeographicLocations.AddRange(locations);
                context.SaveChanges();

                var subLocations = new List<GeographicLocation>
                {
                    new() { Name = "Sub-Location A1-1-1", GeographicLocationTypeId = 4, GeographicLocationParentId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Sub-Location B1-1-1", GeographicLocationTypeId = 4, GeographicLocationParentId = 6, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.GeographicLocations.AddRange(subLocations);
                context.SaveChanges();

                var villages = new List<GeographicLocation>
                {
                    new() { Name = "Village A1-1-1-1", GeographicLocationTypeId = 5, GeographicLocationParentId = 7, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Village B1-1-1-1", GeographicLocationTypeId = 5, GeographicLocationParentId = 8, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.GeographicLocations.AddRange(villages);
                context.SaveChanges();
            }

            if (!context.AssistancePrograms.Any())
            {
                var assistancePrograms = new List<AssistanceProgram>
                {
                    new() { Name = "Food Assistance", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Education Support", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Health Services", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.AssistancePrograms.AddRange(assistancePrograms);
                context.SaveChanges();
            }

            // Designations
            if (!context.Designations.Any())
            {
                var designations = new List<Designation>
                {
                    new() { Name = "Manager", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Field Officer", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.Designations.AddRange(designations);
                context.SaveChanges();
            }

            // Marital Status
            if (!context.MaritalStatuses.Any())
            {
                var maritalStatuses = new List<MaritalStatus>
                {
                    new() { Name = "Single", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Married", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Divorced", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.MaritalStatuses.AddRange(maritalStatuses);
                context.SaveChanges();
            }

            // Officers
            if (!context.Officers.Any())
            {
                // Retrieve the existing designations
                var managerDesignation = context.Designations.FirstOrDefault(d => d.Name == "Manager");
                var supervisorDesignation = context.Designations.FirstOrDefault(d => d.Name == "Field Officer");

                // Create officers and associate with designations
                var officers = new List<Officer>
        {
            new Officer
            {
                FirstName = "John",
                MiddleName = "Doe",
                LastName = "Smith",
                DesignationId = managerDesignation.Id,  // Assign Manager designation
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Officer
            {
                FirstName = "Jane",
                MiddleName = "Anne",
                LastName = "Doe",
                DesignationId = supervisorDesignation.Id,  // Assign Supervisor designation
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

                // Add officers to the context and save changes
                context.Officers.AddRange(officers);
                context.SaveChanges();
            }

            // Phone Number Types
            if (!context.PhoneNumberTypes.Any())
            {
                var phoneNumberTypes = new List<PhoneNumberType>
                {
                    new() { Name = "Mobile", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Landline", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.PhoneNumberTypes.AddRange(phoneNumberTypes);
                context.SaveChanges();
            }

            // Sex
            if (!context.Sexes.Any())
            {
                var sexes = new List<Sex>
                {
                    new() { Name = "Male", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Female", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Other", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.Sexes.AddRange(sexes);
                context.SaveChanges();
            }

            // Status
            if (!context.Statuses.Any())
            {
                var statuses = new List<Status>
                {
                    new() { Name = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Inactive", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.Statuses.AddRange(statuses);
                context.SaveChanges();
            }
        }
    }
}
