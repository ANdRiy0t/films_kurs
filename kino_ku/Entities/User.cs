namespace kino_ku.Entities;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public TypeUser TypeUser { get; set; }
}

public enum TypeUser
{
    User,
    Admin
}