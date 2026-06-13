namespace CollegeNBU.Web.ViewModels.Statistics;

public class CourseStatisticsViewModel
{
    public string CourseName { get; set; } = null!;

    public decimal AverageGrade { get; set; }

    public int TotalGrades { get; set; }
}