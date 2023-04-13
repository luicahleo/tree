using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.WorkerServices.Imports;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ImportExport
{
    public class PostImportTask : PostObjectService<ImportTaskDTO, ImportTaskEntity, ImportTaskDTOMapper>
    {

        private readonly GetDependencies<ImportTaskDTO, ImportTaskEntity> _getDependency;
        private readonly GetDependencies<ImportTypeDTO, ImportTypeEntity> _getDependencyType;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;

        public PostImportTask(PostDependencies<ImportTaskEntity> postDependency, GetDependencies<ImportTaskDTO, ImportTaskEntity> getDependency, GetDependencies<ImportTypeDTO, ImportTypeEntity> getDependencyType,
            IHttpContextAccessor httpcontextAccessor, GetDependencies<UserDTO, UserEntity> getUserDependency) : base(httpcontextAccessor, postDependency, new ImportTaskValidation())
        {
            _getDependency = getDependency;
            _getDependencyType = getDependencyType;
            _getUserDependency = getUserDependency;
        }

        public override async Task<Result<ImportTaskEntity>> ValidateEntity(ImportTaskDTO task, int Client, string email, string code = "")
        {
            int? userID = null;
            UserEntity user = await _getUserDependency.GetItemByCode(email, Client);
            if (user != null)
            {
                userID = user.UsuarioID;
            }

            ImportTypeEntity? type = await _getDependencyType.GetItemByCode(task.Type, Client);
            if (type == null)
            {
                return Result.Failure<ImportTaskEntity>(_traduccion.ImportType + " " + $"{task.Type}" + " " + _errorTraduccion.NotFound + ".");
            }

            if (task.Document.DocumentData != null)
            {
                string sRuta = TreeCore.DirectoryMapping.GetImportFilesDirectory();
                sRuta = Path.Combine(sRuta, task.Document.Document);
                if (File.Exists(sRuta))
                {
                    File.Delete(sRuta);
                }
                await File.WriteAllBytesAsync(sRuta, task.Document.DocumentData);
            }
            else
            {
                return Result.Failure<ImportTaskEntity>(_traduccion.ImportType + " " + $"{nameof(task.Document.Document)}" + " " + _errorTraduccion.FormatError + ".");
            }

            ImportTaskEntity ImportTaskEntity = ImportTaskEntity.Create(Client, task.Code, task.Document.Document, task.UploadDate, task.ImportDate, type, userID);

            Task<ImportTaskEntity> oTask = _getDependency.GetItemByCode(task.Code, Client);
            if (oTask.Result != null && oTask.Result.DocumentoCargaID > 0)
            {
                return Result.Failure<ImportTaskEntity>(_traduccion.ImportTask + " " + $"{task.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }
            return ImportTaskEntity;
        }

        public async Task<Result<ImportTaskEntity>> DoWorkImportTask(ImportTaskEntity task) {
            DateTime nextTime = task.FechaEstimadaSubida;
            if (nextTime > DateTime.Now && nextTime.Date == DateTime.Today && !task.Procesado)
            {
                await ImportFactory.CreateImportAsync(await _mapper.Map(task));
            }
            return task;
        }
    }
}
