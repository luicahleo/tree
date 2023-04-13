using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository;

namespace TreeCore.BackEnd.ServiceDependencies.Services
{
    public abstract class BasicDependence<T>
        where T:class
    {
        protected readonly BaseRepository<T> _repository;
        public BasicDependence(BaseRepository<T> baseRepository)
        {
            _repository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        }
        public async Task CommitTransaction()
        {
            await _repository.CommitTransaction();
        }
    }
}
