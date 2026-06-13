namespace CollegeNBU.Web.ViewModels.SemesterProgram;

public class CreateSemesterProgramViewModel
{
    public int DepartmentId { get; set; }

    public List<int> CourseIds { get; set; } = new();
}