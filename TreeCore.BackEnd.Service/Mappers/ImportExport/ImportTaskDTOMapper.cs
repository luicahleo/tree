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
    public class ImportTaskDTOMapper : BaseMapper<ImportTaskDTO, ImportTaskEntity>
    {
        public override Task<ImportTaskDTO> Map(ImportTaskEntity entity)
        {
            ImportTaskDTO dto = new ImportTaskDTO()
            {
                Code = entity.DocumentoCarga,
                Document = new Shared.DTO.ValueObject.FileDTO
                {
                    Document = entity.RutaDocumento
                },
                UploadDate = entity.FechaSubida,
                ImportDate = entity.FechaEstimadaSubida,
                LogFile = entity.RutaLog,
                Processed = entity.Procesado,
                Success = entity.Exito,
                Type = entity.DocumentosCargasPlantillas.DocumentoCargaPlantilla
            };
            return Task.FromResult(dto);
        }
    }
}
