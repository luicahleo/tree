using Newtonsoft.Json;
using System.Collections.Generic;
using TreeCore.Page;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Query;

namespace TreeCore.Clases
{
    public class QueryWeb
    {
        public static QueryDTO ParseFilterDTO(string sFiltro, string sOrder, string sDir, List<Filter> listafDTO, int pageSize = -1, int pageIndex = -1)
        {
            List<string> listOrders = new List<string>();
            List<FilterDTO> listFilters = new List<FilterDTO>();

            if (listafDTO != null)
            {
                foreach (var oFiltro in listafDTO)
                {
                    listFilters.Add(new FilterDTO
                    {
                        Field = oFiltro.Column,
                        Value = oFiltro.Value.ToString(),
                        Operator = oFiltro.Operator,
                        Type = oFiltro.Type
                    });
                }
            }
            else
            {
                listFilters = new List<FilterDTO>();
            }

            if (sFiltro != "" && sFiltro != null)
            {
                foreach (var oFiltro in JsonConvert.DeserializeObject<List<FilterExtNet>>(sFiltro))
                {
                    listFilters.Add(new FilterDTO
                    {
                        Field = oFiltro.property,
                        Value = oFiltro.value,
                        Operator = oFiltro.@operator,
                        Type = Filter.Types.AND
                    });
                }
            }

            if (sOrder != "")
            {
                listOrders.Add(sOrder);
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            QueryDTO query = new QueryDTO(listFilters, listOrders, sDir, pageSize, pageIndex);

            return query;
        }
    }
}