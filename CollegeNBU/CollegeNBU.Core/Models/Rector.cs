namespace CollegeNBU.Core.Models;

public class Rector
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }
}