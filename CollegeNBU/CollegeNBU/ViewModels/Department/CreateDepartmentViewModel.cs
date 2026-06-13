namespace CollegeNBU.Web.ViewModels.Department;

public class CreateDepartmentViewModel
{
    public string Name { get; set; } = null!;

    public int FacultyId { get; set; }

    public int? HeadTeacherId { get; set; }
}