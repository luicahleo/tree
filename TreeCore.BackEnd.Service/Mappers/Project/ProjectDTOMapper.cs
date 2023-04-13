using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.DTO.Sites;


namespace TreeCore.BackEnd.Service.Mappers
{
    public class ProjectDTOMapper : BaseMapper<ProjectDTO, ProjectEntity>
    {
        public override async Task<ProjectDTO> Map(ProjectEntity entity)
        {
            var WoMapper = new WorkOrderDTOMapper();
            var dto = new ProjectDTO {
                Code = entity.Codigo,
                Name = entity.Nombre,
                Description = entity.Descripcion,
                Active = entity.Activo,
                Budget = new Shared.DTO.ValueObject.BudgetDTO {
                    CurrencyCode = entity.Budget.BudgetMoneda.Moneda,
                    Value = entity.Budget.BudgetValor
                },
                BusinessProcessCode = entity.CoreBusinessProcess.Codigo,
                CreationDate = entity.FechaCreacion,
                CreationUserEmail = entity.UsuarioCreador.EMail,
                EndDate = entity.FechaFin,
                LastModificationDate = entity.FechaUltimaModificacion,
                LastModificationUserEmail = entity.UsuarioModificador.EMail,
                ProgramCode = entity.CoreProgram.Codigo,
                ProjectLifeCycleStatusCode = entity.CoreProjectLifeCycleStatus.Codigo,
                LinkedWorkOrders = new List<Shared.DTO.WorkOrders.WorkOrderDTO>()
                //LinkedAssets = JsonConvert.DeserializeObject<List<AssetDTO>>(entity.Activos)
            };
            foreach (var wo in entity.WorkOrders)
            {
                dto.LinkedWorkOrders.Add(await WoMapper.Map(wo));
            }
            return dto;
        }
    }
}
