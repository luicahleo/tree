using CapaNegocio;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Data.SqlClient;
using TreeCore.Page;
using System.Reflection;
using System.Globalization;
using System.Threading;


namespace TreeCore.Componentes
{

    public partial class toolbarFiltros : System.Web.UI.UserControl
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool _MostrarComboFecha;
        private bool _MostrarBusqueda;
        private bool _MostrarEstatico;
        private bool _MostrarBotones;
        private bool _MostrarComboMisFiltros;
        private bool _MostrarComboProyectos;
        private bool _OcultarComboClientes;
        private bool _QuitarBotonesFiltros;
        private bool _QuitarFiltros;
        private string _DatosCmbEstatico;
        private string _Stores;
        private string _Grid;

        public string FechaDefecto
        {
            get { return FechaDefecto; }
            set { cmbFechasRango.SelectedItem.Value = value; }
        }

        public long ClienteID
        {
            get
            {
                if (cmbClientes.SelectedItem.Value == null || cmbClientes.SelectedItem.Value == "")
                {


                    //if ((string)hdClienteID.Value == "0)
                    //{
                    //}
                    cmbClientes.SelectedItem.Value = "0";
                   

                    return long.Parse(cmbClientes.SelectedItem.Value.ToString());

                }
                else
                {

                    return long.Parse(cmbClientes.SelectedItem.Value);
                }
            }
            set { this.cmbClientes.Value = value; this.hdClienteID.Value = value; }
        }

        public string DatosCmbEstatico
        {
            get { return _DatosCmbEstatico; }
            set { this._DatosCmbEstatico = value; }
        }

        public string FiltrarFecha
        {
            get
            {
                string Fechainicio = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (cmbFechasRango.SelectedItem.Value == "Dia")
                {
                    Fechainicio = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                if (cmbFechasRango.SelectedItem.Value == "Semana")
                {
                    Fechainicio = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                if (cmbFechasRango.SelectedItem.Value == "Mes")
                {
                    Fechainicio = DateTime.Today.AddMonths(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                if (cmbFechasRango.SelectedItem.Value == "Trimestre")
                {
                    Fechainicio = DateTime.Today.AddMonths(-3).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                if (cmbFechasRango.SelectedItem.Value == "Año")
                {
                    Fechainicio = DateTime.Today.AddYears(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                return Fechainicio;

            }
            set
            {
                this.cmbFechasRango.Value = value;
            }
        }

        public string FechaFiltrada(string fechaFiltrada)
        {
            string Fechainicio = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (fechaFiltrada == "Dia")
            {
                Fechainicio = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            if (fechaFiltrada == "Semana")
            {
                Fechainicio = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            if (fechaFiltrada == "Mes")
            {
                Fechainicio = DateTime.Today.AddMonths(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            if (fechaFiltrada == "Trimestre")
            {
                Fechainicio = DateTime.Today.AddMonths(-3).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            if (fechaFiltrada == "Año")
            {
                Fechainicio = DateTime.Today.AddYears(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return Fechainicio;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            hdStoresID.Value = Stores;
            hdGrid.Value = Grid;

            if (MostrarBusqueda)
            {
                this.txtSearch.Hidden = false;
            }
            if (MostrarEstatico)
            {
                this.cmbEstatico.Hidden = false;
            }
            if (MostrarComboFecha)
            {
                this.cmbFechasRango.Hidden = false;
            }
            if (MostrarBotones)
            {
                this.btnGestionarColumnas.Hidden = false;
                this.btnFiltroNegativo.Hidden = false;


            }
            else
            {
                this.btnGestionarColumnas.Hidden = true;
                this.btnFiltroNegativo.Hidden = true;
            }

            if (MostrarComboMisFiltros)
            {
                this.cmbMisFiltros.Hidden = false;
            }
            if (OcultarComboClientes)
            {
                this.cmbClientes.Hidden = true;
            }
            if(this.hdClienteID.Value != null)
            {
                if (this.hdClienteID.Value.ToString() == "0")
                {
                    this.cmbClientes.Hidden = false;
                }
            }

            if (QuitarBotonesFiltros)
            {
                this.wrapBotonesYcmbFiltros.Hidden = true;
            }

            if (QuitarFiltros)
            {
                this.btnQuitarFiltros.Hidden = true;
            }
          

            if (this.btnQuitarFiltros.Hidden == true
                && this.btnGestionarColumnas.Hidden == true
                && this.btnFiltroNegativo.Hidden == true
                && this.cmbMisFiltros.Hidden == true
                && this.cmbClientes.Hidden ==true 
                && this.cmbFechasRango.Hidden == true
                && this.cmbEstatico.Hidden == true)
            {
                this.wrapBotonesYcmbFiltros.Hidden = true;
            }
            else if (this.cmbClientes.Hidden == false)
            {
                this.wrapBotonesYcmbFiltros.Hidden = false;
            }
        }

        #region STORES

        #region CLIENTES

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest && OcultarComboClientes != true)
            {
                try
                {
                    List<Data.Clientes> listaClientes = ListaClientes();

                    if (listaClientes != null)
                    {
                        storeClientes.DataSource = listaClientes;
                    }

                    //if (!hdClienteID.Value.ToString().Equals("0"))
                    //{
                    //	cmbClientes.Hidden = true;
                    //	cmbClientes.SelectedItem.Value = hdClienteID.Value.ToString();
                    //}
                    else
                    {
                        cmbClientes.Hidden = false;

                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);

                }
            }
        }

        #endregion

        #endregion

        public bool OcultarComboClientes
        {
            get { return _OcultarComboClientes; }
            set { this._OcultarComboClientes = value; }
        }

        public bool MostrarComboFecha
        {
            get { return _MostrarComboFecha; }
            set { this._MostrarComboFecha = value; }
        }

        public bool MostrarBusqueda
        {
            get { return _MostrarBusqueda; }
            set { this._MostrarBusqueda = value; }
        }

        public bool MostrarEstatico
        {
            get { return _MostrarEstatico; }
            set { this._MostrarEstatico = value; }
        }

        public bool QuitarFiltros
        {
            get { return _QuitarFiltros; }
            set { this._QuitarFiltros = value; }
        }

        public bool QuitarBotonesFiltros
        {
            get { return _QuitarBotonesFiltros; }
            set { this._QuitarBotonesFiltros = value; }
        }

        public bool MostrarBotones
        {
            get { return _MostrarBotones; }
            set { this._MostrarBotones = value; }
        }

        public bool MostrarComboMisFiltros
        {
            get { return _MostrarComboMisFiltros; }
            set { this._MostrarComboMisFiltros = value; }
        }

        public bool MostrarComboProyectos
        {
            get { return _MostrarComboProyectos; }
            set { this._MostrarComboProyectos = value; }
        }

        public string Stores
        {
            get { return _Stores; }
            set { this._Stores = value; }
        }

        public string Grid
        {
            get { return _Grid; }
            set { this._Grid = value; }
        }

        #region CMB ESTATICO

        public string CmbEstatico
        {
            get { return cmbEstatico.SelectedItem.Value; }
        }

        [DirectMethod()]
        public DirectResponse CargarCmbEstatico()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                if (DatosCmbEstatico != null && DatosCmbEstatico != "")
                {
                    string[] datos = DatosCmbEstatico.Split(';');
                    foreach (string dato in datos)
                    {
                        string[] datoSeparado = dato.Split(',');

                        string displayText = GetGlobalResourceObject("Comun", datoSeparado[0]).ToString();
                        string value = datoSeparado[1];

                        cmbEstatico.AddItem(displayText, value);
                    }

                    cmbEstatico.Select(0);
                }
            }
            catch
            {
                direct.Success = false;
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion
    }

}