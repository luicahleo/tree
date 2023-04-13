using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.General
{
    public class RolDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public bool Active { get; set; }
        [Required] public List<string> Profiles { get; set; }
        [JsonIgnore] public ProfileDTO ProfileDTO { get; }

        public RolDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "Codigo".ToLower());
            map.Add(nameof(Name).ToLower(), "Nombre".ToLower());
            map.Add(nameof(Description).ToLower(), "Descripcion".ToLower());
            map.Add(nameof(Active).ToLower(), "Activo".ToLower());
        }
    }
}
