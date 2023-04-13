using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.PaginasComunes
{
    public partial class SeguimientoPresupuestos : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }


        protected void Page_Load(object sender, EventArgs e)
        {
        }



        public static List<object> DataTask
        {
            get
            {
                List<object> goods = new List<object>
            {
                new
                {
                    Id = 1,
                    NombreDato = "Pendiente aprobar presupuesto",
                    CreadoPor = "Creado Por Super Admin",
                    CreadoDate = DateTime.Now.ToString(),
                    bDone = true,
                },                new
                {
                    Id = 1,
                    NombreDato = "Pendiente aprobar presupuesto",
                    CreadoPor = "Creado Por Super Admin",
                    CreadoDate = DateTime.Now.ToString(),
                    bDone = true,
                },
                    new
                {
                    Id = 1,
                    NombreDato = "Pendiente aprobar presupuesto",
                    CreadoPor = "Creado Por Super Admin",
                    CreadoDate = DateTime.Now.ToString(),
                    bDone = true,
                },        new
                {
                    Id = 1,
                    NombreDato = "Pendiente aprobar presupuesto",
                    CreadoPor = "Creado Por Super Admin",
                    CreadoDate = DateTime.Now.ToString(),
                    bDone = true,
                },        new
                {
                    Id = 1,
                    NombreDato = "Pendiente aprobar presupuesto",
                    CreadoPor = "Creado Por Super Admin",
                    CreadoDate = DateTime.Now.ToString(),
                    bDone = true,
                },




            };

                return goods;
            }
        }



        public static List<object> DataTask2
        {
            get
            {
                List<object> goods = new List<object>
            {
                new
                {
                    Id = 1,
                    NombreTracking = "Longitud del cable entrada",
                    ByUser = "Super Admin",
                    Date = DateTime.Now.ToString(),
                },


                 new
                {
                    Id = 2,
                    NombreTracking = "Longitud 2 entrada",
                    ByUser = "Super Admin",
                    Date = DateTime.Now.ToString(),

                },


                 new
                {
                    Id = 3,
                    NombreTracking = "Marcado del cable entrada",
                    ByUser = "Super Admin",
                    Date = DateTime.Now.ToString(),

                },


                new
                {
                    Id = 4,
                    NombreTracking = "Prueba del cable entrada",
                    ByUser = "Super Admin",
                    Date = DateTime.Now.ToString(),

                },





            };

                return goods;
            }
        }




        public static List<object> DataGridP1
        {
            get
            {
                List<object> goods = new List<object>
            {
                new
                {
                    ID = 1,
                    NombreCategoria = "Infra y Equipo",
                    NombreItemTarea = "Concrete sidewalk Repair(5cm concrete)",
                    CosteItemUnidad = "9€",
                    CosteItemTipoUnidad = "m²",
                    NumItems = 2,
                    CosteTotalFila = 218.222,
                },


                new
                {
                    ID = 2,
                    NombreCategoria = "Infra y Equipo",
                    NombreItemTarea = "WOOD sidewalk Repair(5cm WOOD)",
                    CosteItemUnidad = "12€",
                    CosteItemTipoUnidad = "m²",
                    NumItems = 3,
                    CosteTotalFila = 4622.3 ,
                },



                                new
                {
                    ID = 3,
                    NombreCategoria = "Infra y Equipo",
                    NombreItemTarea = "WOOD sidewalk Repair(5cm WOOD)",
                    CosteItemUnidad = "12€",
                    CosteItemTipoUnidad = "m²",
                    NumItems = 3,
                    CosteTotalFila = 4622.3 ,
                },

                                                new
                {
                    ID = 4,
                    NombreCategoria = "Infra y Equipo",
                    NombreItemTarea = "WOOD sidewalk Repair(5cm WOOD)",
                    CosteItemUnidad = "12€",
                    CosteItemTipoUnidad = "m²",
                    NumItems = 3,
                    CosteTotalFila = 4622.3 ,
                },

                                                                new
                {
                    ID = 5,
                    NombreCategoria = "Infra y Equipo",
                    NombreItemTarea = "WOOD sidewalk Repair(5cm WOOD)",
                    CosteItemUnidad = "12€",
                    CosteItemTipoUnidad = "m²",
                    NumItems = 3,
                    CosteTotalFila = 4622.3 ,
                },

                                                                                new
                {
                    ID = 6,
                    NombreCategoria = "Infra y Equipo",
                    NombreItemTarea = "WOOD sidewalk Repair(5cm WOOD)",
                    CosteItemUnidad = "12€",
                    CosteItemTipoUnidad = "m²",
                    NumItems = 3,
                    CosteTotalFila = 4622.3 ,
                },



            };

                return goods;
            }
        }

    }
}