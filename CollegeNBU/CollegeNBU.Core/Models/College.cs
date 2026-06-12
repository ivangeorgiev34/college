namespace CollegeNBU.Core.Models;

public class College
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public ICollection<Faculty> Faculties { get; set; }
        = new List<Faculty>();
}