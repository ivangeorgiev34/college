namespace CollegeNBU.Core.Models;

public class SemesterProgramCourse
{
    public int SemesterProgramId { get; set; }

    public SemesterProgram SemesterProgram { get; set; } = null!;

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;
}