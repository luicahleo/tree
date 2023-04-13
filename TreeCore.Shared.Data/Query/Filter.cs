using System.Collections.Generic;

namespace TreeCore.Shared.Data.Query
{
    public class Filter
    {
        public string Column;
        public string Operator { get; }
        public object Value { get; }
        public string Type { get; }
        //public string Linked { get; }
        public List<Filter> Filters;

        public Filter(string column, string @operator, object value)
        {
            Column = column;
            Operator = @operator;
            Value = value.ToString();
        }

        public Filter(string column, string @operator, object value, string type, string linked)
        {
            Column = column;
            Operator = @operator;
            Value = value.ToString();
            Type = type;
            //Linked = linked;
        }

        public Filter(string type, List<Filter> filters)
        {
            Type = type;
            Filters = filters;
        }

        private string GetSentenceSql(int iterator, string identifyCol)
        {
            identifyCol = (string.IsNullOrEmpty(identifyCol)) ? "" : $"{identifyCol}.";

            return $" {identifyCol}{this.Column} {this.Operator} @{this.Column}{iterator} ";
        }

        public static string BuilFilters(List<Filter> filters, string parentType, int countFilter, out int outCountFilter, Dictionary<string, object> inDicParam, out Dictionary<string, object> outDicParam, string identifyCol)
        {
            string sqlFilter = "WHERE 1 = 1 ";

            if (filters.Count == 1 && filters[0] != null && filters[0].Filters != null)
            {
                foreach (Filter filter in filters[0].Filters)
                {
                    if (parentType != Types.AND && parentType != Types.OR)
                    {
                        parentType = Types.AND;
                    }

                    sqlFilter += string.IsNullOrEmpty(sqlFilter) ? "( " : parentType + " ( ";
                    int iterator = countFilter++;

                    if (filter.Filters != null && filter.Filters.Count > 0)
                    {
                        sqlFilter += BuilFilters(filter.Filters, filter.Type, countFilter, out outCountFilter, inDicParam, out outDicParam, identifyCol);
                        outCountFilter = countFilter;
                        outDicParam = inDicParam;
                    }
                    else
                    {
                        sqlFilter += filter.GetSentenceSql(iterator, identifyCol);
                        string nameVar = $"{filter.Column}{iterator}";
                        inDicParam.Add(nameVar, filter.Value);

                    }
                }

                sqlFilter += ")";
            }
            else if (filters.Count > 0 && filters[0] != null && filters[0].Column == null)
            {
                int i = 0;
                int j = 0;

                foreach (Filter filt in filters)
                {
                    j++;
                    foreach (Filter filter in filt.Filters)
                    {
                        if (parentType != Types.AND && parentType != Types.OR)
                        {
                            parentType = Types.AND;
                        }
                        if (i == 0)
                        {
                            sqlFilter += string.IsNullOrEmpty(sqlFilter) ? "( " : parentType + " ( ";
                        }

                        int iterator = countFilter++;

                        if (filter.Filters != null && filter.Filters.Count > 0)
                        {
                            sqlFilter += BuilFilters(filter.Filters, filter.Type, countFilter, out outCountFilter, inDicParam, out outDicParam, identifyCol);
                            outCountFilter = countFilter;
                            outDicParam = inDicParam;
                        }
                        else
                        {
                            sqlFilter += filter.GetSentenceSql(iterator, identifyCol);
                            string nameVar = $"{filter.Column}{iterator}";
                            inDicParam.Add(nameVar, filter.Value);

                        }

                        i = 0;
                    }

                    if (j == filters.Count)
                    {
                        sqlFilter += ")";
                    }
                    else
                    {
                        sqlFilter += ")" + filt.Type + " (";
                        i = 1;
                    }
                }
            }
            else if (filters[0] != null)
            {
                int j = 0;

                foreach (Filter filter in filters)
                {
                    if ((parentType != Types.AND && parentType != Types.OR) || sqlFilter == "WHERE 1 = 1 ")
                    {
                        parentType = Types.AND;
                    }
                    else
                    {
                        parentType = filter.Type;
                    }
                    
                    sqlFilter += string.IsNullOrEmpty(sqlFilter) ? "( " : parentType + " ( ";
                    int iterator = countFilter++;

                    if (filter != null && filter.Filters != null && filter.Filters.Count > 0)
                    {
                        sqlFilter += BuilFilters(filter.Filters, filter.Type, countFilter, out outCountFilter, inDicParam, out outDicParam, identifyCol);
                        outCountFilter = countFilter;
                        outDicParam = inDicParam;
                    }
                    else
                    {
                        sqlFilter += filter.GetSentenceSql(iterator, identifyCol);
                        string nameVar = $"{filter.Column}{iterator}";
                        inDicParam.Add(nameVar, filter.Value);

                    }
                    j++;
                }

                if (j == filters.Count)
                {
                    foreach (var v in filters)
                    {
                        sqlFilter += ")";
                    }
                }
            }
            else
            {
                sqlFilter = " ";
            }

            outCountFilter = countFilter;
            outDicParam = inDicParam;
            return sqlFilter;
        }

        public class Types
        {
            public const string AND = "AND";
            public const string OR = "OR";
        }
    }
}
