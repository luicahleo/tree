using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TreeCore.BackEnd.WorkerServices.Works;
using TreeCore.Shared.DTO.ImportExport;
using System.Collections.Generic;

namespace TreeCore.BackEnd.WorkerServices.Imports
{
    public static class ImportFactory
    {
        public static async Task<ImportWork> CreateImportAsync(ImportTaskDTO import)
        {
            IBaseImport baseImport = null;
            DateTime nextTime = import.ImportDate;

            IEnumerable < MethodInfo > listMethods = typeof(ImportFactory).GetMethods().Where(x => x.Name.Replace(" ", "").ToLower() == import.Type.ToLower());
            if (listMethods.Count() > 0)
            {
                baseImport = (IBaseImport)typeof(ImportFactory).GetMethods().Where(x => x.Name.Replace(" ", "").ToLower() == import.Type.ToLower()).Select(x => x).FirstOrDefault().Invoke(null, new object[] { import });
                if (baseImport == null)
                {
                    return null;
                }
                string key = $"Carga{import.Document.Document}";
                ImportWork work = new ImportWork(baseImport, nextTime, import.Document.Document, key);
                await WorksRepository.AddWork(key, work);
                return work;
            }
            else
            {
                return null;
            }
        }

        //case "REGIONAL_DISTRIBUTION":
        //    break;
        //case "ENTITIES":
        public static IBaseImport entities(ImportTaskDTO import) => new CompanyImport(import);
        //case "SITES":
        //    break;
        //case "CONTACTS":
        //    break;
        //case "DOCUMENTS":
        //    break;
        //case "INVENTORY":
        //    break;
        //case "ALTA_INSTALACION":
        //    break;
        //case "ELEMENTS RELATIONSHIPS":
        //    break;
        //case "FORM SECTIONS":
        //    break;
        //case "USERS":
        //    break;
        //case "PRODUCT CATALOG SERVICES":
        public static IBaseImport productcatalogservices(ImportTaskDTO import) => new ProductImport(import);
        //case "PRODUCT CATALOG":
        //    break;

        public static IBaseImport contracts(ImportTaskDTO import) => new ContractsImport(import);
    }

}
