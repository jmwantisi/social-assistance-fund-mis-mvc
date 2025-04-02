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
        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationService applicationService, LookupService lookupService, ApplicantService applicantService, ApplicationDbContext context)
        {
            _applicationService = applicationService;
            _lookupService = lookupService;
            _applicantService = applicantService;
            _context = context;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var application = new Application
                {
                    ApplicationDate = model.ApplicationDate,
                    Applicant = new Applicant { Id = model.ApplicantId },
                    Program = new AssistanceProgram { Id = model.SelectedProgramId },
                };

                await _applicationService.CreateApplicationAsync(application);
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns if validation fails
            model.Programs = await _lookupService.GetProgramsAsync();
            model.Applicants = await _applicantService.GetAllApplicantsAsync();
         
            return View(model);
        
        }

        // GET: Application/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _applicationService.UpdateApplicationAsync(id, application);
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        // GET: Application/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: Application/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _applicationService.DeleteApplicationAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
