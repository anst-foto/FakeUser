namespace FakeUser.Model;

public record User
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}
