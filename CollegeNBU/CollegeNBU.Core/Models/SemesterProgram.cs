namespace CollegeNBU.Core.Models;

public class SemesterProgram
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Semester { get; set; } = null!;

    public ICollection<SemesterProgramCourse> Courses { get; set; }
        = new List<SemesterProgramCourse>();
}