using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TreeCore.APIClient;
using TreeCore.BackEnd.WorkerServices.Imports;
using TreeCore.Shared.DTO.Auth;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.Query;

namespace TreeCore.BackEnd.WorkerServices
{
    public class WorkerService : BackgroundService
    {
        private BaseAPIClient<ImportTaskDTO> _APIClient;

        public WorkerService()
        {
            
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                await GenerarWorks();
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

            }
        }

        private async Task GenerarWorks()
        {
            FilterDTO filter = new FilterDTO
            {
                Field = nameof(ImportTaskDTO.Processed),
                Operator = nameof(Operators.eq),
                Value = "False"
            };
            QueryDTO oQueryDTO = new QueryDTO(new List<FilterDTO>() { filter });
            try
            {
                var cToken = new BaseAPIClient<TokenDTO>();
                var oToken = cToken.Login("treeservices@atrebo.com", "Atrebo.2022").Result;
                _APIClient = new BaseAPIClient<ImportTaskDTO>(oToken.Value.AccessToken);

                var listaImportaciones = await _APIClient.GetList(oQueryDTO);
                foreach (var oImportacion in listaImportaciones.Value)
                {
                    if (oImportacion.ImportDate.Date == DateTime.Today)
                    {
                        await ImportFactory.CreateImportAsync(oImportacion);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            await Task.CompletedTask;
        }
    }
}
