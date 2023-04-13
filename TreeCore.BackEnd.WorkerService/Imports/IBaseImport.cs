using NLog;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.APIClient;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Auth;
using TreeCore.Shared.DTO.ImportExport;

namespace TreeCore.BackEnd.WorkerServices.Imports
{
    public abstract class IBaseImport
    {
        protected readonly ImportTaskDTO _task;
        protected readonly Hashtable listaErrores;
        protected readonly EscritorLogs logCarga;
        protected readonly Logger log;
        protected readonly GeneralTranslations _traduccion;
        protected readonly ErrorTranslations _errorTraduccion;
        protected bool _correcto;

        protected IBaseImport(ImportTaskDTO task) {
            _task = task;
            log = LogManager.GetCurrentClassLogger();
            listaErrores = new Hashtable();
            logCarga = new EscritorLogs(task.Code);
            _traduccion = new GeneralTranslations(System.Globalization.CultureInfo.CurrentCulture);
            _errorTraduccion = new ErrorTranslations(System.Globalization.CultureInfo.CurrentCulture);
        }

        public virtual async Task LoadFile(string sRuta)
        {
            var cToken = new BaseAPIClient<TokenDTO>();
            var oToken = cToken.Login("treeservices@atrebo.com", "Atrebo.2022").Result;
            var cImportTask = new BaseAPIClient<ImportTaskDTO>(oToken.Value.AccessToken);

            _task.Processed = true;
            _task.Success = _correcto;
            _task.LogFile = logCarga.NombreFichero;
            await cImportTask.UpdateEntity(_task.Code, _task);
        }

        public void AddError(string Key, string Error) {
            List<string> lErrors;
            if (listaErrores[Key] != null)
            {
                lErrors = (List<string>)listaErrores[Key];
            }
            else
            {
                lErrors = new List<string>();
            }
            lErrors.Add(Error);
            listaErrores[Key] = lErrors;
        }

    }
}
