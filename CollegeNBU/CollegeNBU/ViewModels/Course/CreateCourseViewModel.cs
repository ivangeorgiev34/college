namespace CollegeNBU.Web.ViewModels.Course;

public class CreateCourseViewModel
{
    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int TeacherId { get; set; }
}