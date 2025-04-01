using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Applicants table
        public DbSet<Applicant> Applicants { get; set; }

        // Applicant Phone Number table
        public DbSet<ApplicantPhoneNumber> ApplicantPhoneNumbers { get; set; }

        // Applications table
        public DbSet<Application> Applications { get; set; }

        // Designations
        public DbSet<Designation> Designations { get; set; }

        // Geographic Locations
        public DbSet<GeographicLocation> GeographicLocations { get; set; }

        // Geographic Location Types
        public DbSet<GeographicLocationType> GeographicLocationTypes { get; set; }

        // Marital Statuses (Fixed Typo)
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }

        // Officers
        public DbSet<Officer> Officers { get; set; }

        // Official Records
        public DbSet<OfficialRecord> OfficialRecords { get; set; }

        // Phone Number Types
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }

        // Programs
        public DbSet<AssistanceProgram> AssistancePrograms { get; set; }

        // Sexes
        public DbSet<Sex> Sexes { get; set; }

        // Statuses table (Fixed Typo)
        public DbSet<Status> Statuses { get; set; }

    }
}
