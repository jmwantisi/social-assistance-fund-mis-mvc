using Microsoft.AspNetCore.Mvc;
using socialAssistanceFundMIS.Services;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Data;
using SocialAssistanceFundMisMcv.ViewModels;
using SocialAssistanceFundMisMcv.Services;
using Microsoft.EntityFrameworkCore;

namespace SocialAssistanceFundMisMcv.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationService _applicationService;
        private readonly LookupService _lookupService; // Service for fetching lookups
        private readonly ApplicantService _applicantService;
        private readonly GeographicLocationService _geographicLocationService;
        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationService applicationService, LookupService lookupService, ApplicantService applicantService, ApplicationDbContext context, GeographicLocationService geographicLocationService)
        {
            _applicationService = applicationService;
            _lookupService = lookupService;
            _applicantService = applicantService;
            _context = context;
            _geographicLocationService = geographicLocationService;
        }

        // GET: Application
        public async Task<IActionResult> Index()
        {
            var applications = await _applicationService.GetAllApplicationsAsync();
            return View(applications);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: Application/Create
        // GET: Application/Create
        public async Task<IActionResult> Create()
        {
            var model = new ApplicationViewModel
            {
                Programs = await _lookupService.GetProgramsAsync(),
                Applicants = await _context.Applicants.Select(a => new Applicant()
                {
                    Id = a.Id,
                    FirstName = a.FirstName + " " + (string.IsNullOrEmpty(a.MiddleName) ? "" : a.MiddleName + " ") + a.LastName
                })
                .ToListAsync()
            };

            return View(model);
        }

        // POST: Application/Create
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Programs = await _lookupService.GetProgramsAsync();
                model.Applicants = await _context.Applicants.Select(a => new Applicant()
                {
                    Id = a.Id,
                    FirstName = a.FirstName + " " + (string.IsNullOrEmpty(a.MiddleName) ? "" : a.MiddleName + " ") + a.LastName
                }).ToListAsync();

                return View(model);
            }

            var application = new Application
            {
                ProgramId = model.SelectedProgramId,
                ApplicantId = model.ApplicantId,
                DeclarationDate = model.DeclarationDate,
                ApplicationDate = model.ApplicationDate
            };

            await _applicationService.CreateApplicationAsync(application);

            return RedirectToAction("Index");
        }


        // GET: Application/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var model = new ApplicationViewModel
            {
                Id = application.Id, // Add Id to distinguish Edit vs Create
                SelectedProgramId = application.ProgramId,
                Programs = await _lookupService.GetProgramsAsync(),
                ApplicantId = application.ApplicantId,
                Applicants = await _context.Applicants.Select(a => new Applicant()
                {
                    Id = a.Id,
                    FirstName = a.FirstName + " " + (string.IsNullOrEmpty(a.MiddleName) ? "" : a.MiddleName + " ") + a.LastName
                }).ToListAsync()
            };

            return View("Create", model); // Use "Create" view for Edit as well
        }


        // POST: Application/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Programs = await _lookupService.GetProgramsAsync();
                model.Applicants = await _context.Applicants.Select(a => new Applicant()
                {
                    Id = a.Id,
                    FirstName = a.FirstName + " " + (string.IsNullOrEmpty(a.MiddleName) ? "" : a.MiddleName + " ") + a.LastName
                }).ToListAsync();

                return View("Create", model);
            }

            var application = new Application
            {
                ProgramId = model.SelectedProgramId,
                ApplicantId = model.ApplicantId,
                DeclarationDate = model.DeclarationDate,
                ApplicationDate = model.ApplicationDate
            };

            application.ProgramId = model.SelectedProgramId;
            application.ApplicantId = model.ApplicantId;

            await _applicationService.UpdateApplicationAsync(model.Id, application);

            return RedirectToAction("Index");
        }

        // Applications/View/4
        public async Task<IActionResult> View(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var model = new ApplicationViewModel
            {
                Id = application.Id,
                SelectedProgramId = application.ProgramId,
                ApplicantId = application.ApplicantId,
                Programs = await _lookupService.GetProgramsAsync(),
                Applicants = await _context.Applicants.Select(a => new Applicant()
                {
                    Id = a.Id,
                    FirstName = a.FirstName + " " + (string.IsNullOrEmpty(a.MiddleName) ? "" : a.MiddleName + " ") + a.LastName
                }).ToListAsync(),
            };

            ViewData["IsReadOnly"] = true; // Pass read-only mode
            return View("Create", model);  // Reuse the Create view
        }

        //


        // GET: Application/Delete/5
        [HttpPost]
        public async Task<IActionResult> Approve(int id, int statusId)
        {

            var result = await _applicationService.ApproveApplicationAsync(id, statusId);

            if (!result)
            {
                TempData["Error"] = "Failed to approve application. The application may not exist.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Application approved successfully.";
            return RedirectToAction("Index");

        }

        // GET: Application/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _applicationService.DeleteApplicationAsync(id);

            if (!result)
            {
                TempData["Error"] = "Failed to delete application. The application may not exist.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Application deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
