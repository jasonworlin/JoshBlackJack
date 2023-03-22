namespace common.Domain;
public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public int Balance { get; set; }

    public User()
    {
        Name = string.Empty;
        Email = string.Empty;
        HashedPassword = string.Empty;
    }
}