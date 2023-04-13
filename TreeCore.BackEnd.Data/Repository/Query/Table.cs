namespace TreeCore.BackEnd.Data.Repository.Query
{
    public class Table
    {
        public readonly string Name;
        public readonly string Code;
        public readonly string ID;

        public Table(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public Table(string name, string code, string Id)
        {
            Name = name;
            Code = code;
            ID = Id;
        }
    }

    
}
