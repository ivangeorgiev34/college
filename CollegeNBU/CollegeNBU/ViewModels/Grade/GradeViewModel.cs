namespace CollegeNBU.Web.ViewModels.Grade;

public class GradeViewModel
{
    public int Id { get; set; }

    public string StudentName { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public decimal Value { get; set; }

    public string TeacherName { get; set; } = null!;
}