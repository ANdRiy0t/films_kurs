namespace kino_ku.ViewModels;

public class UserRegistrationViewModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmedPassword { get; set; }
    
    public string PasswordMessage { get; set; }
    public string EmailMessage { get; set; }
    public string UsernameMessage { get; set; }
}