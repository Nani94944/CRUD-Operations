using System.ComponentModel.DataAnnotations;

namespace CRUD_Operations.DTOs.Roles
{
    public class RoleCreateDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}