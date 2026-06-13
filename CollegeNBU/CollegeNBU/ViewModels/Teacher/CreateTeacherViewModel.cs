namespace CollegeNBU.Web.ViewModels.Teacher;

public class CreateTeacherViewModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? QualificationSubjects { get; set; }

    public int DepartmentId { get; set; }
}