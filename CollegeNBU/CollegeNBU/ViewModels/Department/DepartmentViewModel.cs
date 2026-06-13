namespace CollegeNBU.Web.ViewModels.Department;

public class DepartmentViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string FacultyName { get; set; } = null!;

    public string? HeadTeacherName { get; set; }
}