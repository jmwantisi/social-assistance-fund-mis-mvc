using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Services;
using SocialAssistanceFundMisMcv.Services;
using SocialAssistanceFundMisMcv.ViewModels;

namespace SocialAssistanceFundMisMcv.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly LookupService _lookupService;
        private readonly GeographicLocationService _geographicLocationService;
        private readonly ApplicantService _applicantService;

        public ApplicantsController(ApplicationDbContext context, LookupService lookupService, GeographicLocationService geographicLocationService, ApplicantService applicantService)
        {
            _context = context;
            _lookupService = lookupService;
            _geographicLocationService = geographicLocationService;
            _applicantService = applicantService;
        }

        public async Task<IActionResult> Index()
        {
            // Step 1: Fetch applicants first (without async calls in Select)
            var applicants = await _context.Applicants
                .Include(a => a.Sex)
                .Include(a => a.MaritialStatus)
                .Include(a => a.PhoneNumbers) // Ensure phone numbers are loaded
                .Where(a => a.Removed != true)
                .Select(a => new ApplicantViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    MiddleName = a.MiddleName,
                    LastName = a.LastName,
                    SexName = a.Sex != null ? a.Sex.Name : "Not Specified",
                    MaritalStatusName = a.MaritialStatus != null ? a.MaritialStatus.Name : "Not Specified",
                    Dob = a.Dob,
                    IdentityCardNumber = a.IdentityCardNumber,
                    PhysicalAddress = a.PhysicalAddress,
                    PostalAddress = a.PostalAddress,
                    VillageId = a.VillageId, // Store VillageId to fetch later
                    PhoneNumbersListString = a.PhoneNumbers != null && a.PhoneNumbers.Any()
                        ? string.Join(", ", a.PhoneNumbers.Select(p => p.PhoneNumber + "(" + p!.PhoneNumberType!.Name + ")"))
                        : "No Phone Number"
                })
                .ToListAsync(); // Fetch all data first before calling async functions

            // Step 2: Fetch village locations separately in a loop
            foreach (var applicant in applicants)
            {
                if (applicant.VillageId.HasValue)
                {
                    applicant.Location = await _geographicLocationService.GetVillageHierarchyByIdAsync(applicant.VillageId);
                }
            }

            return View(applicants);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ApplicantViewModel
            {
                Sexes = await _lookupService.GetSexesAsync(),
                MaritalStatuses = await _lookupService.GetMaritalStatusesAsync(),
                Villages = await _geographicLocationService.GetVillagesWithHierarchyAsync(),
                PhoneNumberTypes = await _lookupService.GetPhoneNumberTypesAsync() ?? new List<PhoneNumberType>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicantViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var applicant = new Applicant
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    SexId = model.SexId,
                    Dob = model.Dob,
                    MaritialStatusId = model.MaritalStatusId,
                    IdentityCardNumber = model.IdentityCardNumber,
                    PhysicalAddress = model.PhysicalAddress,
                    PostalAddress = model.PostalAddress,
                    VillageId = model.VillageId,
                    Email = model.Email
                };

                var phoneNumbers = model.PhoneNumbers
                    .Where(p => !string.IsNullOrWhiteSpace(p.PhoneNumber))
                    .Select(p => (p.PhoneNumber, p.PhoneNumberTypeId))
                    .ToList();

                await _applicantService.CreateApplicantAsync(applicant, phoneNumbers);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var applicant = await _context.Applicants
                .Include(a => a.PhoneNumbers) // Ensure phone numbers are loaded
                .FirstOrDefaultAsync(a => a.Id == id);

            if (applicant == null) return NotFound();

            var model = new ApplicantViewModel
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                SexId = applicant.SexId,
                Dob = applicant.Dob,
                MaritalStatusId = applicant.MaritialStatusId,
                IdentityCardNumber = applicant.IdentityCardNumber,
                PostalAddress = applicant.PostalAddress,
                PhysicalAddress = applicant.PhysicalAddress,
                VillageId = applicant.VillageId,
                Sexes = await _lookupService.GetSexesAsync(),
                MaritalStatuses = await _lookupService.GetMaritalStatusesAsync(),
                Villages = await _geographicLocationService.GetVillagesWithHierarchyAsync(),
                PhoneNumberTypes = await _lookupService.GetPhoneNumberTypesAsync() ?? new List<PhoneNumberType>(),
                PhoneNumbers = applicant.PhoneNumbers?.Select(pn => new ApplicantPhoneNumber
                {
                    Id = pn.Id,
                    PhoneNumber = pn.PhoneNumber,
                    PhoneNumberTypeId = pn.PhoneNumberTypeId
                }).ToList() ?? new List<ApplicantPhoneNumber>(),
                Email = applicant.Email
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ApplicantViewModel model)
        {
            var applicant = await _context.Applicants
                .Include(a => a.PhoneNumbers) // Ensure phone numbers are loaded
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (applicant == null) return NotFound();

            // Update applicant details
            applicant.FirstName = model.FirstName;
            applicant.MiddleName = model.MiddleName;
            applicant.LastName = model.LastName;
            applicant.SexId = model.SexId;
            applicant.Dob = model.Dob;
            applicant.MaritialStatusId = model.MaritalStatusId;
            applicant.IdentityCardNumber = model.IdentityCardNumber;
            applicant.PhysicalAddress = model.PhysicalAddress;
            applicant.PostalAddress = model.PostalAddress;
            applicant.VillageId = model.VillageId;
            applicant.Email = model.Email;

            // Handle phone numbers
            var existingPhoneNumbers = applicant.PhoneNumbers.ToList();

            // Remove phone numbers that are not in the updated list
            foreach (var existingPhone in existingPhoneNumbers)
            {
                if (!model.PhoneNumbers.Any(p => p.Id == existingPhone.Id))
                {
                    _context.ApplicantPhoneNumbers.Remove(existingPhone);
                }
            }

            // Add or update phone numbers
            foreach (var phone in model.PhoneNumbers)
            {
                var existingPhone = existingPhoneNumbers.FirstOrDefault(p => p.Id == phone.Id);
                if (existingPhone != null)
                {
                    // Update existing phone number
                    existingPhone.PhoneNumber = phone.PhoneNumber;
                    existingPhone.PhoneNumberTypeId = phone.PhoneNumberTypeId;
                }
                else
                {
                    // Add new phone number
                    applicant.PhoneNumbers.Add(new ApplicantPhoneNumber
                    {
                        PhoneNumber = phone.PhoneNumber,
                        PhoneNumberTypeId = phone.PhoneNumberTypeId,
                        ApplicantId = applicant.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _applicantService.DeleteApplicantAsync(id);

            if (!result)
            {
                TempData["Error"] = "Failed to delete applicant. The applicant may not exist.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Applicant deleted successfully.";
            return RedirectToAction("Index");
        }

    }
}
