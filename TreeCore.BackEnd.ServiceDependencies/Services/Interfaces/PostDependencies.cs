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
    public class PostDependencies<T>: BasicDependence<T>
    where T:class
    {

        public PostDependencies(BaseRepository<T> baseRepository):base(baseRepository) {
        }

        public async Task<Result<T>> Create(T oEntidad) =>
            await _repository.InsertSingle(oEntidad);
    }
}
