using System.Collections.Generic;

namespace TreeCore.Shared.DTO.Query
{
    public class QueryDTO
    {
        public List<FilterDTO> Filters { get; set; }
        public List<string> Order { get; set; }
        public string Direction { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

        public QueryDTO() {
            Filters = new List<FilterDTO>();
            Order = new List<string>();
            Direction = "";
            pageSize = -1;
            pageIndex = -1;
        }

        public QueryDTO(List<FilterDTO> oFilters) {
            Filters = oFilters;
            Order = new List<string>();
            Direction = "";
            pageSize = -1;
            pageIndex = -1;
        }

        public QueryDTO(List<FilterDTO> oFilters, List<string> oOrder) {
            Filters = oFilters;
            Order = oOrder;
            Direction = "";
            pageSize = -1;
            pageIndex = -1;
        }

        public QueryDTO(List<FilterDTO> oFilters, List<string> oOrder, string direction) {
            Filters = oFilters;
            Order = oOrder;
            Direction = direction;
            pageSize = -1;
            pageIndex = -1;
        }

        public QueryDTO(List<FilterDTO> oFilters, List<string> oOrder, string direction, int iPageSize, int iPageIndex) {
            Filters = oFilters;
            Order = oOrder;
            Direction = direction;
            pageSize = iPageSize;
            pageIndex = iPageIndex;
        }

        public QueryDTO(List<string> oOrder, string direction, int iPageSize, int iPageIndex) {
            Filters = new List<FilterDTO>();
            Order = oOrder;
            Direction = direction;
            pageSize = iPageSize;
            pageIndex = iPageIndex;
        }

        public QueryDTO(string oOrder, string direction, int iPageSize, int iPageIndex) {
            Filters = new List<FilterDTO>();
            Order = new List<string>() { oOrder };
            Direction = direction;
            pageSize = iPageSize;
            pageIndex = iPageIndex;
        }

        public void AddFilter(FilterDTO filter) { 
            Filters.Add(filter);
        }

    }
}
