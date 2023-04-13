using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.ServiceDependencies.Services
{
    public class DeleteDependencies<T>: BasicDependence<T>
    where T:class
    {
        public DeleteDependencies(BaseRepository<T> baseRepository):base(baseRepository) { 
        
        }
        public async Task<Result<int>> Delete(T oEntidad)
        {
            return await _repository.Delete(oEntidad);
        }
    }
}
