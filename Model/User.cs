namespace ReceiptReader.Model;

public class User
{
    public string Name { get; set; }
    string Password { get; set; }
    string Email { get; set; }
    bool IsEmailConfirmed{ get; set; }

    public User(string name, string password, string email)
    {
        this.Name = name;
        this.Password = password;
        this.Email = email;
        IsEmailConfirmed = false;
    }
    public User(string name)
    {
        this.Name = name;
        this.Password = "123";
        this.Email = "admin";
        IsEmailConfirmed = false;
    }
}
