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
                    LastName = model.LastName,
                    SexId = model.SexId,
                    Dob = model.Dob,
                    MaritialStatusId = model.MaritalStatusId,
                    IdentityCardNumber = model.IdentityCardNumber,
                    PhysicalAddress = model.PhysicalAddress,
                    VillageId = model.VillageId
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


        public IActionResult Edit(int id)
        {
            var applicant = _context.Applicants.Find(id);
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
                PhysicalAddress = applicant.PhysicalAddress,
                Sexes = _context.Sexes.ToList(),
                MaritalStatuses = _context.MaritalStatuses.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ApplicantViewModel model)
        {
            if (ModelState.IsValid)
            {
                var applicant = _context.Applicants.Find(model.Id);
                if (applicant == null) return NotFound();

                applicant.FirstName = model.FirstName;
                applicant.LastName = model.LastName;
                applicant.SexId = model.SexId;
                applicant.Dob = model.Dob;
                applicant.MaritialStatusId = model.MaritalStatusId;
                applicant.IdentityCardNumber = model.IdentityCardNumber;
                applicant.PhysicalAddress = model.PhysicalAddress;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
