namespace CollegeNBU.Web.ViewModels.Course;

public class EditCourseViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int TeacherId { get; set; }

    public string Description { get; set; } = null!;
}