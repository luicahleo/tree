using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.Query;

namespace TreeCore.Shared.DTO.Config
{
    public class ViewDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Page { get; set; }
        [Required]
        public string Icon { get; set; }
        [Required]
        public bool Default { get; set; }
        [Required]
        public List<ColumnDTO> Columns{ get; set; }
        [Required]
        public List<FilterDTO> Filters{ get; set; }

        public ViewDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "nombre");
            map.Add(nameof(Page).ToLower(), "pagina");
            map.Add("UsuarioID".ToLower(), "usuarioid");
            map.Add(nameof(Default).ToLower(), "defecto");
            map.Add(nameof(Icon).ToLower(), "icono");
        }
    }
}
