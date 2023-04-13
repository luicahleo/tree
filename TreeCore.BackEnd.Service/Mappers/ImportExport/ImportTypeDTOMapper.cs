using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class ImportTypeDTOMapper : BaseMapper<ImportTypeDTO, ImportTypeEntity>
    {
        public override Task<ImportTypeDTO> Map(ImportTypeEntity entity)
        {
            ImportTypeDTO dto = new ImportTypeDTO()
            {
                Code = entity.DocumentoCargaPlantilla,
                Document = entity.RutaDocumento
            };
            return Task.FromResult(dto);
        }
    }
}
