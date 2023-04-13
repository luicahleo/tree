using TreeCore.BackEnd.Model.Entity;

namespace TreeCore.BackEnd.Model.Entity.Companies
{
    public class CompanyAddressEntity : BaseEntity
    {
        public int? EntidadDireccionID;
        public string Codigo;
        public string EntidadDireccion;
        public CompanyEntity? CoreCompany;
        public bool Defecto;
        public string DireccionJSON;


        public CompanyAddressEntity(int? EntidadDireccionID, CompanyEntity CoreCompany, string Codigo, string Nombre, bool Defecto, string DireccionJSON)
        {
            this.EntidadDireccionID = EntidadDireccionID;
            this.Codigo = Codigo;
            this.CoreCompany = CoreCompany;
            this.EntidadDireccion = Nombre;
            this.Defecto = Defecto;
            this.DireccionJSON = DireccionJSON;

        }

        protected CompanyAddressEntity() { }

        public static CompanyAddressEntity UpdateId(CompanyAddressEntity companyAddress, int id) =>
            new CompanyAddressEntity(id, companyAddress.CoreCompany, companyAddress.Codigo, companyAddress.EntidadDireccion, companyAddress.Defecto, companyAddress.DireccionJSON);
    }
}
