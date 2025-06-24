using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [LocalizedRequired]
    public string Username { get; set; }

    [LocalizedRequired]
    [EmailAddress ( ErrorMessage = "InvalidEmail" )]
    public string Email { get; set; }

    [LocalizedRequired]
    [StringLength ( 100 , MinimumLength = 6 , ErrorMessage = "PasswordLength" )]
    public string Password { get; set; }

    [LocalizedRequired]
    public int RoleId { get; set; }
}