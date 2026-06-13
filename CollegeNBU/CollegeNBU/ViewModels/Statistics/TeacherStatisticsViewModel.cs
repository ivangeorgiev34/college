namespace CollegeNBU.Web.ViewModels.Statistics;

public class TeacherStatisticsViewModel
{
    public string TeacherName { get; set; } = null!;

    public int CoursesCount { get; set; }

    public decimal AverageGrade { get; set; }

    public int TotalAbsences { get; set; }
}