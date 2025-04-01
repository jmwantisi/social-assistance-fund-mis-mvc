using Microsoft.AspNetCore.Mvc;
using socialAssistanceFundMIS.Data;
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

        public ApplicantsController(ApplicationDbContext context, LookupService lookupService, GeographicLocationService geographicLocationService)
        {
            _context = context;
            _lookupService = lookupService;
            _geographicLocationService = geographicLocationService;
        }

        public IActionResult Index()
        {
            var applicants = _context.Applicants.Select(a => new ApplicantViewModel
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                IdentityCardNumber = a.IdentityCardNumber,
                PhysicalAddress = a.PhysicalAddress
            }).ToList();
            return View(applicants);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ApplicantViewModel
            {
                Sexes = await _lookupService.GetSexesAsync(),
                MaritalStatuses = await _lookupService.GetMaritalStatusesAsync(),
                Counties = await _geographicLocationService.GetVillagesWithHierarchyAsync()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ApplicantViewModel model)
        {
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
                    PhysicalAddress = model.PhysicalAddress
                };
                _context.Applicants.Add(applicant);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
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
