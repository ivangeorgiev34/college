namespace CollegeNBU.Web.ViewModels.Faculty;

public class FacultyViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CollegeId { get; set; }

    public string CollegeName { get; set; } = null!;
}