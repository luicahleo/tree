using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Project
{

    public class PutProject : PutObjectService<ProjectDTO, ProjectEntity, ProjectDTOMapper>
    {
        private readonly GetDependencies<ProjectDTO, ProjectEntity> _getDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        private readonly GetDependencies<BusinessProcessDTO, BusinessProcessEntity> _getBP;
        private readonly GetDependencies<ProgramDTO, ProgramEntity> _getProgram;
        private readonly GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> _getStatus;
        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getCurrency;

        public PutProject(PutDependencies<ProjectEntity> putDependency,
            GetDependencies<ProjectDTO, ProjectEntity> getDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            GetDependencies<BusinessProcessDTO, BusinessProcessEntity> getBP,
            GetDependencies<ProgramDTO, ProgramEntity> getProgram,
            GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> getStatus,
            GetDependencies<CurrencyDTO, CurrencyEntity> getCurrency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ProjectValidation())
        {
            _getDependency = getDependency;
            _getUserDependency = getUserDependency;
            _getBP = getBP;
            _getProgram = getProgram;
            _getStatus = getStatus;
            _getCurrency = getCurrency;
        }
        public override async Task<Result<ProjectEntity>> ValidateEntity(ProjectDTO dto, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            var ori = await _getDependency.GetItemByCode(code, client);
            if (ori == null)
            {
                lErrors.Add(Error.Create(_traduccion.Project + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            if (dto.Code != code && _getDependency.GetItemByCode(dto.Code, client).Result != null)
            {
                lErrors.Add(Error.Create(_traduccion.Project + " " + $"{dto.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            var user = await _getUserDependency.GetItemByCode(email, client);
            var oBP = await _getBP.GetItemByCode(dto.BusinessProcessCode, client);
            if (oBP == null)
            {
                lErrors.Add(Error.Create(_traduccion.BusinessProcess + " " + $"{dto.BusinessProcessCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            var oProgram = await _getProgram.GetItemByCode(dto.ProgramCode, client);
            if (oProgram == null)
            {
                lErrors.Add(Error.Create(_traduccion.Product + " " + $"{dto.ProgramCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            var oStatus = await _getStatus.GetItemByCode(dto.ProjectLifeCycleStatusCode, client);
            if (oStatus == null)
            {
                lErrors.Add(Error.Create(_traduccion.ProjectLifeCycleStatus + " " + $"{dto.ProjectLifeCycleStatusCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            var oCurrency = await _getCurrency.GetItemByCode(dto.Budget.CurrencyCode, client);
            if (oCurrency == null)
            {
                lErrors.Add(Error.Create(_traduccion.Currency + " " + $"{dto.Budget.CurrencyCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            var oBudget = new Budget(oCurrency, dto.Budget.Value);
            //var Assets = JsonConvert.SerializeObject(dto.LinkedAssets);
            var oEntity = ProjectEntity.Create(client, dto.Code, dto.Code, dto.Description, dto.Active, oBP, oProgram, oStatus, oBudget, new List<WorkOrderEntity>(), DateTime.Now, user, DateTime.Now, user, null);
            oEntity = ProjectEntity.UpdateId(oEntity, ori.ClienteID);
            if (lErrors.Count > 0)
            {
                return Result.Failure<ProjectEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return oEntity;
            }
        }
    }
}
