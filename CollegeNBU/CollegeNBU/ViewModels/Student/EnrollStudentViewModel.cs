namespace CollegeNBU.Web.ViewModels.Student;

public class EnrollStudentViewModel
{
    public int StudentId { get; set; }

    public List<int> SelectedCourseIds { get; set; } = new();
}