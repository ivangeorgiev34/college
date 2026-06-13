namespace CollegeNBU.Web.ViewModels.Attendance;

public class CreateAttendanceViewModel
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public bool IsAbsent { get; set; }
}