using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.General
{
    public class ProfileDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public string ModuleCode { get; set; }
        [Required]
        public List<string> UserFuntionalities { get; set; }

        public ProfileDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "Perfil_esES".ToLower());
            map.Add(nameof(Description).ToLower(), "Descripcion".ToLower());
            map.Add(nameof(Active).ToLower(), "Activo".ToLower());
            map.Add(nameof(ModuleCode).ToLower(), "CodigoModulo".ToLower());
        }
    }
}
