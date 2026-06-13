namespace CollegeNBU.Web.ViewModels.Student;

public class StudentViewModel
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;
}