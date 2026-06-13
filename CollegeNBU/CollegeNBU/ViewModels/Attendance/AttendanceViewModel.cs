namespace CollegeNBU.Web.ViewModels.Attendance;

public class AttendanceViewModel
{
    public int Id { get; set; }

    public string StudentName { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public bool IsAbsent { get; set; }

    public string TeacherName { get; set; } = null!;
}