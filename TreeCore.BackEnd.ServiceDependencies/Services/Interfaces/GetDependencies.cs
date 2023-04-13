using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO;
using TreeCore.Shared.DTO.Query;

namespace TreeCore.BackEnd.ServiceDependencies.Services
{
    public class GetDependencies<DTO, T>: BasicDependence<T>
        where T:class
        where DTO:BaseDTO
    {

        private readonly DTO _objectDTO;

        public GetDependencies(BaseRepository<T> baseRepository, DTO oObjeto):base(baseRepository) {
            _objectDTO = oObjeto;
        }

        public async Task<T> GetItemByCode(string code, int Client)
        {
            return await _repository.GetItemByCode(code, Client);
        }

        public async Task<IEnumerable<T>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize, int pageIndex)
        {
            DTO tmp = _objectDTO;
            filters = MapFilter(filters, tmp);

            if (orders != null)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    orders[i] = tmp.map.GetValueOrDefault(orders[i].ToLower());
                }
            }
            
            return await _repository.GetList(Client, filters, orders, direction, pageSize, pageIndex);
        }
        private List<Filter> MapFilter(List<Filter> filters, DTO tmp)
        {
            if (filters != null && filters.Count > 0)
            {
                filters.ForEach(f =>
                { 
                    if (f != null)
                    {
                        if (f.Column != null)
                        {
                            f.Column = tmp.map.GetValueOrDefault(f.Column.ToLower());
                        }
                        else if (f.Filters != null)
                        {
                            f.Filters = MapFilter(f.Filters, tmp);
                        }
                    }
                });
            }

            return filters;
        }
    }
}
