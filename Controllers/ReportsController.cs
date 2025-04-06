using Microsoft.AspNetCore.Mvc;
using SocialAssistanceFundMisMcv.Services;
using SocialAssistanceFundMisMcv.ViewModels;

namespace SocialAssistanceFundMisMcv.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
            var (pending, approved) = await _reportService.GetApplicationStatusCountsAsync();
            var (male, female) = await _reportService.GetApprovedApplicantsByGenderAsync();
            var perProgram = await _reportService.GetApplicationCountsPerProgramAsync();

            var viewModel = new ReportViewModel
            {
                StatusData = new[] { pending, approved },
                GenderData = new[] { male, female },
                ProgramLabels = perProgram.Keys.ToList(),
                ProgramData = perProgram.Values.ToList()
            };

            return View(viewModel);
        }
    }
}
