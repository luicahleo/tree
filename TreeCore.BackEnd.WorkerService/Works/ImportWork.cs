using System;
using System.Threading;
using System.Threading.Tasks;
using TreeCore.BackEnd.WorkerServices.Imports;

namespace TreeCore.BackEnd.WorkerServices.Works
{
    public class ImportWork : IBaseWork
    {
        private readonly IBaseImport _import;
        private readonly DateTime _fechaEjecucion;
        private readonly string _file;
        private Timer _timer = null!;
        private readonly string _key;

        public ImportWork(IBaseImport import, DateTime fechaEjecucion, string file, string key)
        {
            _key = key;
            _import = import;
            _fechaEjecucion = fechaEjecucion;
            _file = file;
        }

        public Task RunWork()
        {
            var time = TimeSpan.FromHours((_fechaEjecucion - DateTime.Now).TotalHours);
            _timer = new Timer(new TimerCallback(DoWork), null, time ,TimeSpan.Zero);

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            await _import.LoadFile(_file);
            WorksRepository.RemoveWork(_key);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}