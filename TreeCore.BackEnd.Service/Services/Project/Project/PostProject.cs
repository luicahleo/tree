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
using TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrder;
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
    public class PostProject : PostObjectService<ProjectDTO, ProjectEntity, ProjectDTOMapper>
    {

        private readonly GetDependencies<ProjectDTO, ProjectEntity> _getDependency;
        private readonly PutDependencies<ProjectEntity> _putDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        private readonly GetDependencies<BusinessProcessDTO, BusinessProcessEntity> _getBP;
        private readonly GetDependencies<ProgramDTO, ProgramEntity> _getProgram;
        private readonly GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> _getStatus;
        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getCurrency;
        private readonly PostWorkOrder _postWorkOrder;

        public PostProject(PostDependencies<ProjectEntity> postDependency,
            GetDependencies<ProjectDTO, ProjectEntity> getDependency,
            PutDependencies<ProjectEntity> putDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            GetDependencies<BusinessProcessDTO, BusinessProcessEntity> getBP,
            GetDependencies<ProgramDTO, ProgramEntity> getProgram,
            GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> getStatus,
            GetDependencies<CurrencyDTO, CurrencyEntity> getCurrency,
            PostWorkOrder postWorkOrder,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ProjectValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getUserDependency = getUserDependency;
            _getBP = getBP;
            _getProgram = getProgram;  
            _getStatus = getStatus; 
            _getCurrency = getCurrency;
            _postWorkOrder = postWorkOrder;
        }

        public async Task<Result<ProjectDTO>> Create(ProjectDTO oProject, WorkOrderDTO oWorkOrder, int clientID, string email, string code = "")
        {
            codesDict["LEVEL1"] = code;
            var project = await ValidateEntity(oProject, clientID, email, code)
                 .Bind(ValidateText)
                 .Bind(SaveItem);

            if (project.Success)
            {
                var WO = await _postWorkOrder.Create(oWorkOrder, (int)project.Value.CoreProjectID, clientID, email, code);

                if (WO.Success)
                {
                    CommitTransaction(project.Value);

                    var projectDTO = _mapper.Map(project.Value).Result;
                    projectDTO.LinkedWorkOrders.ListOrEmpty().Add(WO.Value);
                    return projectDTO;
                }
                else
                {
                    return Result.Failure<ProjectDTO>(ImmutableArray.Create(WO.Errores.ToArray()));
                }
            }
            return _mapper.Map(project.Value).Result;
        }

        public override async Task<Result<ProjectEntity>> ValidateEntity(ProjectDTO dto, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            if (_getDependency.GetItemByCode(dto.Code, client).Result != null)
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
            var oEntity = ProjectEntity.Create(client, dto.Code, dto.Code, dto.Description, dto.Active, oBP, oProgram, oStatus, oBudget, new List<WorkOrderEntity>(), DateTime.Now, user, DateTime.Now, user, null);

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