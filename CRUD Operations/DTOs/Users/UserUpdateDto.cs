using System.ComponentModel.DataAnnotations;

namespace CRUD_Operations.DTOs.Users
{
    public class UserUpdateDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}