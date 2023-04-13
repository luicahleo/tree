using System;


namespace TreeCore.BackEnd.Model.Entity.Sites
{
    public class SiteEntity : BaseEntity
    {
        public readonly int? EmplazamientoID;
        public readonly string Codigo;
       



        public SiteEntity(int? EmplazamientoID, string Codigo)
        {
            this.EmplazamientoID = EmplazamientoID;
            this.Codigo = Codigo;
            
        }

        protected SiteEntity()
        {
            
        }

        public static SiteEntity UpdateId(SiteEntity siteEntity, int id) =>
           new SiteEntity(id, siteEntity.Codigo);


    }
}
