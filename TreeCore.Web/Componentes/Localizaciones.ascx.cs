using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Page;

namespace TreeCore.Componentes
{
    public partial class Localizaciones : BaseUserControl
    {
        private bool _Requerido;
        private bool _PaisDefecto;
        private bool _ActRegion;
        private bool _LayoutHorizontal;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public long? RegionID
        {
            get
            {
                if (cmbRegionPaises.SelectedItem.Value != null && cmbRegionPaises.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbRegionPaises.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbRegionPaises.SetValue(value);
                this.hdRegionPaises.SetValue(value);
                this.cmbRegionPaises.ShowTrigger(0);
            }
        }
        public long? PaisID
        {
            get
            {
                if (cmbPais.SelectedItem.Value != null && cmbPais.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbPais.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbPais.SetValue(value);
                this.hdPais.SetValue(value);
                this.cmbPais.ShowTrigger(0);
            }
        }
        public long? RegionPaisID
        {
            get
            {
                if (cmbRegion.SelectedItem.Value != null && cmbRegion.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbRegion.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbRegion.SetValue(value);
                this.hdRegion.SetValue(value);
                this.cmbRegion.ShowTrigger(0);
            }
        }
        public long? ProvinciaID
        {
            get
            {
                if (cmbProvincia.SelectedItem.Value != null && cmbProvincia.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbProvincia.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbProvincia.SetValue(value);
                this.hdProvincia.SetValue(value);
                this.cmbProvincia.ShowTrigger(0);
            }
        }
        public long? MunicipioID
        {
            get
            {
                if (cmbMunicipio.SelectedItem.Value != null && cmbMunicipio.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbMunicipio.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbMunicipio.SetValue(value);
                this.hdMunicipio.SetValue(value);
                this.cmbMunicipio.ShowTrigger(0);
            }
        }

        public bool Requerido
        {
            get { return _Requerido; }
            set { this._Requerido = value; }
        }

        public bool PaisDefecto
        {
            get { return _PaisDefecto; }
            set { this._PaisDefecto = value; }
        }

        public bool ActRegion
        {
            get { return _ActRegion; }
            set { this._ActRegion = value; }
        }

        public bool LayoutHorizontal
        {
            get { return _LayoutHorizontal; }
            set { this._LayoutHorizontal = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            

            hdClienteID = (Hidden)X.GetCmp("hdCliID");

            if (Requerido)
            {
                this.cmbPais.AllowBlank = false;
                this.cmbRegion.AllowBlank = false;
                this.cmbProvincia.AllowBlank = false;
                this.cmbMunicipio.AllowBlank = false;
                if (ActRegion)
                {
                    this.cmbRegionPaises.AllowBlank = false;
                }
            }
            if (PaisDefecto)
            {
                this.cmbPais.AllowBlank = false;
            }
            if (!ActRegion)
            {
                this.cmbRegionPaises.Hidden = true;
                this.cmbRegionPaises.AllowBlank = true;
            }
            //if (LayoutHorizontal)
            //{
            //    containerHorizontal.Items.AddRange(containerVertical.Items);
            //    if (containerVertical.Items != null && containerVertical.Items.Count != 0)
            //    {
            //        containerVertical.Items.Clear();
            //    }
            //}

        }


        #region STORES

        #region REGION

        protected void storeRegionPaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    if (ActRegion)
                    {
                        List<Data.Regiones> listaRegiones = ListaRegionPaises();

                        if (listaRegiones != null)
                        {
                            this.storeRegionPaises.DataSource = listaRegiones;
                            this.storeRegionPaises.DataBind();
                        }
                    }
                    else
                    {
                        storePaises.Reload();
                    }

                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.Regiones> ListaRegionPaises()
        {
            List<Data.Regiones> listaDatos;
            RegionesController cRegiones = new RegionesController();

            try
            {
                listaDatos = cRegiones.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region PAISES

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Paises> listaPaises = ListaPaises();

                    if (listaPaises != null)
                    {
                        this.storePaises.DataSource = listaPaises;
                        this.storePaises.DataBind();
                    }
                    string pais;
                    if (hdPais.Value != null)
                    {
                        pais = hdPais.Value.ToString();
                    }
                    else
                    {
                        pais = "";
                    }

                    if (PaisDefecto && pais.Equals(""))
                    {
                        PaisesController cPais = new PaisesController();
                        if (hdClienteID.Value != null && hdClienteID.Value.ToString() != "0")
                        {
                            Data.Paises oPais = cPais.GetDefault(long.Parse(hdClienteID.Value.ToString()));
                            this.PaisID = oPais.PaisID;
                        }

                    }

                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.Paises> ListaPaises()
        {
            List<Data.Paises> listaDatos = new List<Data.Paises>();
            PaisesController cPaises = new PaisesController();

            try
            {
                if (!ActRegion)
                {
                    if (hdClienteID.Value != null)
                    {
                        listaDatos = cPaises.GetActivos(long.Parse(hdClienteID.Value.ToString()));

                    }
                }
                else
                {
                    if (this.hdRegionPaises.Value != null && this.hdRegionPaises.Value.ToString() != "")
                    {
                        listaDatos = cPaises.GetPaisesRegion(Convert.ToInt64(this.hdRegionPaises.Value));
                        //hdRegionPaises.SetValue("");
                    }
                    else if (this.cmbRegionPaises.Value != null && this.cmbRegionPaises.Value.ToString() != "")
                    {
                        listaDatos = cPaises.GetPaisesRegion(Convert.ToInt64(this.cmbRegionPaises.Value));

                    }
                    else
                    {
                        listaDatos = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region REGIONES PAISES

        protected void storeRegiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.RegionesPaises> listaRegiones = ListaRegiones();

                    if (listaRegiones != null)
                    {
                        storeRegiones.DataSource = listaRegiones;
                        this.storeRegiones.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.RegionesPaises> ListaRegiones()
        {
            List<Data.RegionesPaises> listaDatos;
            RegionesPaisesController cRegiones = new RegionesPaisesController();

            try
            {
                if (this.cmbPais.Value != null && this.cmbPais.Value.ToString() != "")
                {
                    listaDatos = cRegiones.getAllRegionPaisByPaisID(Convert.ToInt64(this.cmbPais.Value));

                }
                else if (this.hdPais.Value != null && this.hdPais.Value.ToString() != "")
                {
                    listaDatos = cRegiones.getAllRegionPaisByPaisID(Convert.ToInt64(this.hdPais.Value));
                    //hdPais.SetValue("");
                }
                else
                {
                    listaDatos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region PROVINCIAS

        protected void storeProvincias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Provincias> listaProvincias = ListaProvincias();

                    if (listaProvincias != null)
                    {
                        storeProvincias.DataSource = listaProvincias;
                        this.storeProvincias.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.Provincias> ListaProvincias()
        {
            List<Data.Provincias> listaDatos;
            ProvinciasController cProvincias = new ProvinciasController();

            try
            {

                if (this.cmbRegion.Value != null && this.cmbRegion.Value.ToString() != "")
                {
                    listaDatos = cProvincias.getAllProvinciasByRegionPaisID(Convert.ToInt64(this.cmbRegion.Value));
                }
                else if (this.hdRegion.Value != null && this.hdRegion.Value.ToString() != "")
                {
                    listaDatos = cProvincias.getAllProvinciasByRegionPaisID(Convert.ToInt64(this.hdRegion.Value));
                    //hdRegion.SetValue("");
                }
                else
                {
                    listaDatos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region MUNICIPIO

        protected void storeMunicipios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Municipios> listaMunicipios = ListaMunicipios();

                    if (listaMunicipios != null)
                    {
                        storeMunicipios.DataSource = listaMunicipios;
                        storeMunicipios.DataBind();
                    }

                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.Municipios> ListaMunicipios()
        {
            List<Data.Municipios> listaDatos;
            MunicipiosController cMunicipios = new MunicipiosController();

            try
            {
                if (this.cmbProvincia.Value != null && this.cmbProvincia.Value.ToString() != "")
                {
                    listaDatos = cMunicipios.getAllMunicipiosByProvID(Convert.ToInt64(this.cmbProvincia.Value));
                }
                else if (this.hdProvincia.Value != null && this.hdProvincia.Value.ToString() != "")
                {
                    listaDatos = cMunicipios.getAllMunicipiosByProvID(Convert.ToInt64(this.hdProvincia.Value));
                    //hdProvincia.SetValue("");
                }
                else
                {
                    listaDatos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECT METHODS

        [DirectMethod]
        public void LimpiarCombos(bool Update)
        {
            cmbPais.ClearValue();
            cmbRegion.ClearValue();
            cmbProvincia.ClearValue();
            cmbMunicipio.ClearValue();
            cmbRegionPaises.ClearValue();

            if (LayoutHorizontal && Update)
            {
                //containerHorizontal.Items.AddRange(containerVertical.Items);
                //if (containerVertical.Items != null && containerVertical.Items.Count != 0)
                //{
                //    containerVertical.Items.Clear();
                //}
                //PanelCombos.UpdateContent();
            }
        }

        [DirectMethod]
        public void RecargarCombos()
        {
            storeRegionPaises.Reload();
            storePaises.Reload();
            storeRegiones.Reload();
            storeProvincias.Reload();
            storeMunicipios.Reload();
        }

        #endregion


    }
}