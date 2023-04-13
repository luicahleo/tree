using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public abstract class BasicValidation<DTO, Entity>
    {
        public abstract Result<Entity> ValidateEntity(Entity oEntidad, ErrorTranslations _traduccion);
        public abstract Result<DTO> ValidateDTO(DTO oEntidad, ErrorTranslations _traduccion);
    }
}
