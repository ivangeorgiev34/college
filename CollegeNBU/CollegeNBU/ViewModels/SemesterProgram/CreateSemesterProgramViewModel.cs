using System.ComponentModel.DataAnnotations;

namespace CollegeNBU.Web.ViewModels.SemesterProgram;

public class CreateSemesterProgramViewModel
{
    [Required(ErrorMessage = "The Semester Program name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "The Semester Program semester is required.")]
    public int Semester { get; set; }

    public int DepartmentId { get; set; }

    public List<int> CourseIds { get; set; } = new();
}