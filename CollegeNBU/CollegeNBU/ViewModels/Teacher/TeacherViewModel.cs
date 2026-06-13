namespace CollegeNBU.Web.ViewModels.Teacher;

public class TeacherViewModel
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public string? QualificationSubjects { get; set; }
}