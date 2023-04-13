using Ext.Net;
using System;
using System.Collections.Generic;
using CapaNegocio;
using System.Linq;
using Ext.Net.Utilities;
using System.Data;
using System.Web.UI.WebControls;
using log4net;
using Tree.Linq.GenericExtensions;

namespace TreeCore.PaginasComunes
{
    public partial class MapaEmplazamientosCercanos : TreeCore.Page.BasePageExtNet
    {
        private ILog log = LogManager.GetLogger("");
        private List<long> lFuncionalidades = new List<long>();


        #region Gestión Página (Init/Load)

        protected void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack & !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                long lEmpID = Convert.ToInt64(Request.QueryString["EmplazamientoID"]);
                EmplazamientosController cEmplazamiento = new EmplazamientosController();
                Data.Emplazamientos Emplazamiento = cEmplazamiento.GetItem(lEmpID);

                if (Emplazamiento != null)
                {
                    this.hdLatitudCercanos.SetValue(Emplazamiento.Latitud);
                    this.hdLongitudCercanos.SetValue(Emplazamiento.Longitud);
                }
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!RequestManager.IsAjaxRequest)
            {
                CargarMapa();
            }

        }

        #endregion

        private void CargarMapa()
        {
            this.UCMapas.SetHiddenValues();
            List<Data.Vw_EmplazamientosMapasCercanos> lEmplazamientos = this.UCMapas.ListaEmplazamientos();
            this.UCMapas.SetDataSourceGridMapas(lEmplazamientos);
        }
    }

}