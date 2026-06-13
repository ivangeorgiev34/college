namespace CollegeNBU.Web.ViewModels.SemesterProgram;

public class SemesterProgramViewModel
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public List<string> Courses { get; set; } = new();
}