using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using TreeCore.Page;
using System.Reflection;

namespace TreeCore.Componentes
{
    public partial class Geoposicion : BaseUserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool _Localizacion;
        private bool _Contactos;
        private bool _Entidad;
        private string _NombreMapa = "";

        #region VARIABLES
        public string Latitud
        {
            get
            {
                if (txtLatitud.Text != null)
                {
                    return txtLatitud.Text;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.txtLatitud.SetValue(value);
            }
        }

        public string Longitud
        {
            get
            {
                if (txtLongitud.Text != null)
                {
                    return txtLongitud.Text;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.txtLongitud.SetValue(value);
            }
        }

        public string Direccion
        {
            get
            {
                if (txtDireccion.Text != null)
                {
                    return txtDireccion.Text;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.txtDireccion.SetValue(value);
            }
        }

        public string Barrio
        {
            get
            {
                if (txtBarrio.Text != null)
                {
                    return txtBarrio.Text;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.txtBarrio.SetValue(value);
            }
        }

        public string CodigoPostal
        {
            get
            {
                if (txtCodigoPostal.Text != null)
                {
                    return txtCodigoPostal.Text;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.txtCodigoPostal.SetValue(value);
            }
        }

        public bool Localizacion
        {
            get { return _Localizacion; }
            set { this._Localizacion = value; }
        }

        public bool Contactos
        {
            get { return _Contactos; }
            set { this._Contactos = value; }
        }

        public bool Entidad
        {
            get { return _Entidad; }
            set { this._Entidad = value; }
        }

        public string NombreMapa
        {
            get { return _NombreMapa; }
            set { this._NombreMapa = value; }
        }

        public string Municipio
        {
            get
            {
                if (cmbMunicipioProvincia.SelectedItem.Text != null && cmbMunicipioProvincia.SelectedItem.Text != "")
                {
                    return this.cmbMunicipioProvincia.SelectedItem.Text;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbMunicipioProvincia.SetText(value);
                this.cmbMunicipioProvincia.ShowTrigger(0);
            }
        }

        #endregion

        #region GESTION PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {
            

            this.txtLatitud.AllowBlank = false;
            this.txtLongitud.AllowBlank = false;

            hdClienteID = (Hidden)X.GetCmp("hdCliID");
            PaisesController cPais = new PaisesController();
            if (hdClienteID.Value != null && hdClienteID.Value.ToString() != "")
            {
                Data.Paises oDato = cPais.GetDefault(long.Parse(hdClienteID.Value.ToString()));

                if (oDato != null)
                {
                    this.hdLatitudPadre.SetValue(oDato.Latitud);
                    this.hdLongitudPadre.SetValue(oDato.Longitud);
                    this.hdPaisCodPadre.SetValue(oDato.PaisCod);
                    this.hdZoom.SetValue(oDato.Zoom);
                }
            }
            if (Localizacion)
            {
                txtDireccion.Hidden = true;
                txtLatitud.Hidden = true;
                txtLongitud.Hidden = true;
                toolPosicionar.Hidden = true;
                this.hdMapa.SetValue(null);

                if (NombreMapa != "")
                {
                    this.hdNombreMapa.SetValue(NombreMapa);
                }
            }
            else if (Contactos)
            {
                txtBarrio.Hidden = true;
                txtLatitud.Hidden = true;
                txtLongitud.Hidden = true;
                this.hdMapa.SetValue(null);

                if (NombreMapa != "")
                {
                    this.hdNombreMapa.SetValue(NombreMapa);
                }
            }
            else if (Entidad)
            {
                txtBarrio.Hidden = true;
                txtLatitud.Hidden = true;
                txtLongitud.Hidden = true;
                this.hdMapa.SetValue(null);
                toolPosicionar.Hidden = true;
            }
            else
            {
                toolPosicionar.Disabled = false;
                txtLatitud.Hidden = false;
                txtLongitud.Hidden = false;
                this.hdMapa.SetValue(null);

                if (NombreMapa != "")
                {
                    this.hdNombreMapa.SetValue(NombreMapa);
                }
            }
        }

        #endregion

        #region STORES

        #region CORE MUNICIPIOS

        protected void storeCoreMunicipios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_CoreMunicipios> listaMunicipios = ListaMunicipios();

                    if (listaMunicipios != null)
                    {
                        this.storeCoreMunicipios.DataSource = listaMunicipios;
                        this.storeCoreMunicipios.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_CoreMunicipios> ListaMunicipios()
        {
            List<Data.Vw_CoreMunicipios> listaMunicipios;
            MunicipiosController cMunicipios = new MunicipiosController();

            try
            {
                listaMunicipios = cMunicipios.getMunicipiosByClienteID(long.Parse(hdClienteID.Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMunicipios = null;
            }

            return listaMunicipios;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod]
        public void LimpiarDireccion()
        {
            txtLatitud.SetText("");
            txtLongitud.SetText("");
        }

        [DirectMethod()]
        public DirectResponse BusquedaMunicipio(string sMunicipio)
        {
            DirectResponse direct = new DirectResponse();
            MunicipiosController cMunicipios = new MunicipiosController();
            PaisesController cPaises = new PaisesController();
            Data.Paises oPais = null;
            Data.Municipios oMunicipio;
            string sProvincia = "";
            string sPaisCod = "";
            bool bExiste;
            string sResult;

            try
            {
                direct.Success = true;
                direct.Result = "";

                oMunicipio = cMunicipios.GetByNombre(sMunicipio);

                if (oMunicipio != null)
                {
                    oPais = cPaises.GetPaisByMunicipioID(oMunicipio.MunicipioID);
                    sProvincia = cMunicipios.getNombreProvinciaByMunicipioID(sMunicipio);
                }

                if (oPais != null)
                {
                    sPaisCod = oPais.PaisCod;
                }

                if (oMunicipio != null)
                {
                    bExiste = true;
                }
                else
                {
                    bExiste = false;
                }

                sResult = bExiste.ToString() + ',' + sProvincia + ',' + sPaisCod;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = "";
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = sResult;

            return direct;
        }

        #endregion
    }
}