namespace CollegeNBU.Web.ViewModels.Department;

public class EditDepartmentViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int FacultyId { get; set; }

    public int? HeadTeacherId { get; set; }
}