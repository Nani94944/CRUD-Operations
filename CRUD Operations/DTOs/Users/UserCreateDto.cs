using System.ComponentModel.DataAnnotations;

namespace CRUD_Operations.DTOs.Users
{
    public class UserCreateDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength ( 100 , MinimumLength = 6 )]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}