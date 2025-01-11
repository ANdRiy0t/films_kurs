using Microsoft.AspNetCore.Identity;

namespace kino_ku.Api.Entities;

public class User : BaseEntity
{
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