using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class CountryEntity : BaseEntity
    {
        public readonly int? PaisID;
        public readonly string Pais;
        public readonly string PaisCod;
        public readonly bool Activo;
        public readonly bool Defecto;


        public CountryEntity(int? paisID, string pais, string paisCod,bool activo, bool defecto)
        {
            PaisID = paisID;
            Pais = pais ?? throw new ArgumentNullException(nameof(pais)); ;
            PaisCod = paisCod ?? throw new ArgumentNullException(nameof(paisCod)); ;
            Activo = activo;
            Defecto = defecto;
        }

        protected CountryEntity() { }

        public static CountryEntity UpdateId(CountryEntity Country, int id) =>
            new CountryEntity(id, Country.Pais, Country.PaisCod,Country.Activo,Country.Defecto);
    }
}
