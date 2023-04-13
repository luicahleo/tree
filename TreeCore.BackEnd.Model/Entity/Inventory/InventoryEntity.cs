using System;


namespace TreeCore.BackEnd.Model.Entity.Inventory
{
    public class InventoryEntity : BaseEntity
    {
        public readonly int? InventarioElementoID;
        public readonly string Codigo;
       



        public InventoryEntity(int? id, string Codigo)
        {
            this.InventarioElementoID = id;
            this.Codigo = Codigo;
            
        }

        protected InventoryEntity()
        {
            
        }

        public static InventoryEntity UpdateId(InventoryEntity siteEntity, int id) =>
           new InventoryEntity(id, siteEntity.Codigo);


    }
}
