using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.ValueObject
{
    public class Address : BaseValueObject
    {
        public string Direccion1 { get; set; }
        public string Direccion2 { get; set; }
        
        public string CodigoPostal { get; set; }
        
        public string Locality { get; set; }
        
        public string Sublocality { get; set; }
       
        public string Pais { get; set; }

        public Address(string Direccion1, string Direccion2, string CodigoPostal, string Locality, string Sublocality, string Pais)
        {
            this.Direccion1 = Direccion1;
            this.Direccion2 = Direccion2;
            this.CodigoPostal = CodigoPostal;
            this.Locality = Locality;
            this.Sublocality = Sublocality;
            this.Pais = Pais;
        }

        protected Address() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Direccion1;
            yield return Direccion2;
            yield return CodigoPostal;
            yield return Locality;
            yield return Sublocality;
            yield return Pais;
        }
    }
}
