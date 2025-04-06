using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Seeders
{
    public static class DefaultSeeder
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
                                // Counties
                    var counties = new List<GeographicLocation>
                    {
                        new() { Name = "Nairobi", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, // ID = 1
                        new() { Name = "Kisumu", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, // ID = 2
                        new() { Name = "Mombasa", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, // ID = 3
                        new() { Name = "Kiambu", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow } // ID = 4
                    };
                    context.GeographicLocations.AddRange(counties);
                    context.SaveChanges();

                                // Sub-Counties
                    var subCounties = new List<GeographicLocation>
                    {
                        new() { Name = "Westlands", GeographicLocationTypeId = 2, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, // Nairobi
                        new() { Name = "Lang'ata", GeographicLocationTypeId = 2, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Kisumu Central", GeographicLocationTypeId = 2, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Nyali", GeographicLocationTypeId = 2, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Thika Town", GeographicLocationTypeId = 2, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };
                    context.GeographicLocations.AddRange(subCounties);
                    context.SaveChanges();

                                // Locations
                    var locations = new List<GeographicLocation>
                    {
                        new() { Name = "Parklands", GeographicLocationTypeId = 3, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Karen", GeographicLocationTypeId = 3, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Milimani", GeographicLocationTypeId = 3, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Mkomani", GeographicLocationTypeId = 3, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Ngoigwa", GeographicLocationTypeId = 3, GeographicLocationParentId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };
                    context.GeographicLocations.AddRange(locations);
                    context.SaveChanges();

                                // Sub-Locations
                    var subLocations = new List<GeographicLocation>
                    {
                        new() { Name = "Parklands North", GeographicLocationTypeId = 4, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Karen South", GeographicLocationTypeId = 4, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Milimani East", GeographicLocationTypeId = 4, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Mkomani North", GeographicLocationTypeId = 4, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Ngoigwa West", GeographicLocationTypeId = 4, GeographicLocationParentId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };
                    context.GeographicLocations.AddRange(subLocations);
                    context.SaveChanges();

                    // Villages
                    var villages = new List<GeographicLocation>
                    {
                        new() { Name = "Kipro Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Karen Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Kisumu Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Mkomani Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Ngoigwa Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };
                    context.GeographicLocations.AddRange(villages);
                    context.SaveChanges();
                }


            if (!context.AssistancePrograms.Any())
            {
                var assistancePrograms = new List<AssistanceProgram>
                {
                    new() { Name = "Orphans and vulnerable children", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Poor elderly persons", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Persons with disability", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Persons in extreme poverty", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Any other", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
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
                    new() { Name = "Pending", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Approved", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.Statuses.AddRange(statuses);
                context.SaveChanges();
            }
        }
    }
}
