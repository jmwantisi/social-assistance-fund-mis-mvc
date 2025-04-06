namespace SocialAssistanceFundMisMcv.ViewModels
{
    public class ReportViewModel
    {
        public int[] StatusData { get; set; } = new int[2]; // Pending, Approved
        public int[] GenderData { get; set; } = new int[2]; // Male, Female
        public List<string> ProgramLabels { get; set; } = new();
        public List<int> ProgramData { get; set; } = new();
    }

}
