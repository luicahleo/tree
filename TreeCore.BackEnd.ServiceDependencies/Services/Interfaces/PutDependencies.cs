using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository;
using TreeCore.Shared.ROP;
using TreeCore.Shared.DTO;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.ServiceDependencies.Services
{
    public class PutDependencies<T>: BasicDependence<T>
    where T:class
    {

        public PutDependencies(BaseRepository<T> baseRepository):base(baseRepository) {
        }

        public async Task<Result<T>> Update(T oEntidad) =>
            await _repository.UpdateSingle(oEntidad);
    }
}
