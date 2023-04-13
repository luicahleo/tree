using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.PaginasComunes
{
	public partial class SeguimientoChecklist : System.Web.UI.Page
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
                    NombreDato = "Longitud del cable entrada",
                    Dato = "32",
                    TipoDato = "cm",
                    bDone = true,
                },


                    new
                {
                    Id = 1,
                    NombreDato = "Separacion Paneles",
                    Dato = "22",
                    TipoDato = "cm",
                    bDone = false,
                },


                    new
                {
                    Id = 1,
                    NombreDato = "Paso distancia",
                    Dato = "42",
                    TipoDato = "cm",
                    bDone = false,
                },

                    new
                {
                    Id = 1,
                    NombreDato = "Paso distancia",
                    Dato = "42",
                    TipoDato = "cm",
                    bDone = true,
                },

                    new
                {
                    Id = 1,
                    NombreDato = "Paso distancia",
                    Dato = "42",
                    TipoDato = "cm",
                    bDone = false,
                },

                    new
                {
                    Id = 1,
                    NombreDato = "Paso distancia",
                    Dato = "42",
                    TipoDato = "cm",
                    bDone = true,
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
                    NombreTask = "Txtareatask del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = true,
                },


                new
                {
                    NombreTask = "Checkbox Task del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = false,
                },


                new
                {
                    NombreTask = "Txtareatask del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = true,
                },


                new
                {
                    NombreTask = "Checkbox Task del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = false,
                },

                new
                {
                    NombreTask = "Txtareatask del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = true,
                },


                new
                {
                    NombreTask = "Checkbox Task del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = false,
                },




                new
                {
                    NombreTask = "Txtareatask del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = true,
                },


                new
                {
                    NombreTask = "Checkbox Task del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = false,
                },

                new
                {
                    NombreTask = "Txtareatask del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = true,
                },


                new
                {
                    NombreTask = "Checkbox Task del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = false,
                },

                new
                {
                    NombreTask = "Txtareatask del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = true,
                },


                new
                {
                    NombreTask = "Checkbox Task del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = false,
                },

                new
                {
                    NombreTask = "Txtareatask del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = true,
                },


                new
                {
                    NombreTask = "Checkbox Task del cable entrada",
                    DateTask = DateTime.Now.ToString(),
                    bDone = false,
                },



            };

                return goods;
            }
        }

    }
}