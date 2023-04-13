using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.Auth
{
    public class LoginDTO
    {
        [Required]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
