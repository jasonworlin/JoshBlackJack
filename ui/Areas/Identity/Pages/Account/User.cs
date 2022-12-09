using System.ComponentModel.DataAnnotations;

public class User
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    /*public User(string email, string password)
    {
        Email = email;
        Password = password;
    }*/
}