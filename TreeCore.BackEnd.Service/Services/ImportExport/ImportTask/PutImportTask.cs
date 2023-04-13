using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.WorkerServices.Imports;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ImportExport
{

    public class PutImportTask : PutObjectService<ImportTaskDTO, ImportTaskEntity, ImportTaskDTOMapper>
    {
        private readonly GetDependencies<ImportTaskDTO, ImportTaskEntity> _getDependency;
        private readonly GetDependencies<ImportTypeDTO, ImportTypeEntity> _getDependencyType;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;


        public PutImportTask(PutDependencies<ImportTaskEntity> putDependency,
            GetDependencies<ImportTaskDTO, ImportTaskEntity> getDependency,
            GetDependencies<ImportTypeDTO, ImportTypeEntity> getDependencyType,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ImportTaskValidation())
        {
            _getDependency = getDependency;
            _getDependencyType = getDependencyType;
            _getUserDependency = getUserDependency;
        }

        public override async Task<Result<ImportTaskEntity>> ValidateEntity(ImportTaskDTO task, int client, string code, string email)
        {
            Task<IEnumerable<ImportTaskEntity>> listImport;
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            int? userID = null;
            UserEntity user = await _getUserDependency.GetItemByCode(email, client);
            if (user != null)
            {
                userID = user.UsuarioID;
            }

            ImportTaskEntity? cur = await _getDependency.GetItemByCode(code, client);
            ImportTypeEntity? type = await _getDependencyType.GetItemByCode(task.Type, client);
            if (type == null)
            {
                return Result.Failure<ImportTaskEntity>(_traduccion.ImportType + " " + $"{task.Type}" + " " + _errorTraduccion.NotFound + ".");
            }

            ImportTaskEntity importFinal = new ImportTaskEntity(cur.DocumentoCargaID, client, task.Code, task.Document.Document, task.UploadDate, task.ImportDate, task.Processed, true, task.Success, type, task.LogFile, userID);

            filter = new Filter(nameof(ImportTaskDTO.Code), Operators.eq, task.Code);
            listFilters.Add(filter);

            listImport = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listImport.Result != null && listImport.Result.ListOrEmpty().Count > 0 &&
                listImport.Result.ListOrEmpty()[0].DocumentoCargaID != importFinal.DocumentoCargaID)
            {
                return Result.Failure<ImportTaskEntity>(_traduccion.ImportTask + " " + $"{task.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            return importFinal;
        }
        public async Task<Result<ImportTaskEntity>> DoWorkImportTask(ImportTaskEntity task)
        {
            DateTime nextTime = task.FechaEstimadaSubida;
            if (nextTime > DateTime.Now && nextTime.Date == DateTime.Today && !task.Procesado)
            {
                await ImportFactory.CreateImportAsync(await _mapper.Map(task));
            }
            return task;
        }
    }
}

