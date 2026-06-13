namespace CollegeNBU.Web.ViewModels.Rector;

public class RectorCourseStatsViewModel
{
    public string CourseName { get; set; } = null!;

    public decimal AverageGrade { get; set; }

    public int StudentsCount { get; set; }

    public int AbsencesCount { get; set; }
}