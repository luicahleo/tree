using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Page;

namespace TreeCore.Componentes
{
    public partial class ReajustePrecios : BaseUserControl
    {


        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);



        protected void Page_Load(object sender, EventArgs e)
        {
            

            hdClienteID = (Hidden)X.GetCmp("hdCliID");
        }


        #region STORES

        protected void storeInflaciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                //Recupera los datos y los establece
                var ls = ListaInflaciones();
                if (ls != null)
                {
                    storeInflaciones.DataSource = ls;
                }
            }
        }

        private List<Data.Inflaciones> ListaInflaciones()
        {
            List<Data.Inflaciones> listadatos;
            try
            {
                InflacionesController mControl = new InflacionesController();
                long lCliID = long.Parse(hdClienteID.Value.ToString());

                //listadatos = mControl.GetActivos(lCliID);
                listadatos = mControl.GetAllInflaciones();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        #endregion


        public long Tipo
        {
            get { return long.Parse(RGTipo.CheckedItems[0].InputValue.ToString()); }
        }

        public string Inflacion
        {
            get { return cmbInflaciones.Value.ToString(); }
            set { this.cmbInflaciones.Value = value; }
        }
        public double CantidadFija
        {
            get { return float.Parse(txtCantidad.Value.ToString()); }
            set { this.txtCantidad.Value = value; }
        }
        public double PorcentajeFijo
        {
            get { return double.Parse(txtPorcentaje.Value.ToString()); }
            set { this.txtPorcentaje.Value = value; }
        }
        public int Periodicidad
        {
            get { return int.Parse(txtCadencia.Value.ToString()); }
            set { this.txtCadencia.Value = value; }
        }
        public DateTime FechaInicio
        {
            get { return DateTime.Parse(txtFechaInicioRevision.Value.ToString()); }
            set { this.txtFechaInicioRevision.Value = value; }
        }
        public DateTime FechaProxima
        {
            get
            {
                return DateTime.Parse(this.txtFechaProxima.Value.ToString());

            }
            set { this.txtFechaProxima.Value = value; }
        }
        public DateTime FechaFin
        {
            get { return DateTime.Parse(txtFechaFinRevision.Value.ToString()); }
            set { this.txtFechaFinRevision.Value = value; }
        }
        public int HdRadio
        {
            get { return int.Parse(hdRadio.Value.ToString()); }
            set { this.hdRadio.Value = value; }
        }
        public string HdProximaFecha
        {
            get { return hdProximaFecha.Value.ToString(); }
            set { this.hdProximaFecha.Value = value; }
        }
        public string HdUltimaFecha
        {
            get { return hdUltimaFecha.Value.ToString(); }
            set { this.hdUltimaFecha.Value = value; }
        }
        public int HdCadencia
        {
            get { return (int)hdCadencia.Value; }
            set { this.hdCadencia.Value = value; }
        }
        public double? HdValor
        {
            get { return (double?)hdValor.Value; }
            set { this.hdValor.Value = value; }
        }
        public long? HdInflacion
        {
            get { return (long?)hdInflacion.Value; }
            set { this.hdInflacion.Value = value; }
        }

        public bool ControlFechaFin
        {
            get { return this.chkControlFechaFin.Checked; }
        }

        public int CheckTipo
        {
            set
            {
                switch (value)
                {
                    case 1:
                        RBSinIncremento.Checked = true;
                        break;
                    case 2:
                        RBCPI.Checked = true;
                        break;
                    case 3:
                        RBCantidaFija.Checked = true;
                        break;
                    case 4:
                        RBPorcentajeFijo.Checked = true;
                        break;
                }
            }
        }

        public bool CheckFechaFin
        {
            set
            {
                if (value)
                {
                    chkControlFechaFin.Checked = true;
                }
                else
                {
                    chkControlFechaFin.Checked = false;
                }
            }
        }

    }
}