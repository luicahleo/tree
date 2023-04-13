using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using TreeCore.Page;
using System.Reflection;
using System.Data.SqlClient;
using TreeCore.Clases;

namespace TreeCore.Componentes
{
    public partial class FormContactos : BaseUserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region DATOS
        public long? ContactoTipoID
        {
            get
            {
                if (cmbTipoContacto.SelectedItem.Value != null && cmbTipoContacto.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbTipoContacto.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbTipoContacto.SetValue(value);
                this.hdTipoContacto.SetValue(value);
                this.cmbTipoContacto.ShowTrigger(0);
            }
        }


        public long ClienteID
        {
            get
            {
                return long.Parse(this.hdClienteID.Value.ToString());
            }
            set
            {
                this.hdClienteID.SetValue(value);
            }
        }
        #endregion

        #region GESTION PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region STORES

        #region TIPOS CONTACTOS

        protected void storeTiposContactos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.ContactosTipos> listaContactos = ListaContactos();

                    if (listaContactos != null)
                    {
                        storeTiposContactos.DataSource = listaContactos;
                        storeTiposContactos.DataBind();

                        PageProxy proxy = (PageProxy)storeTiposContactos.Proxy[0];
                        proxy.Total = listaContactos.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.ContactosTipos> ListaContactos()
        {
            List<Data.ContactosTipos> listaContactos;
            ContactosTiposController cContactos = new ContactosTiposController();

            long lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

            try
            {
                listaContactos = cContactos.GetActivos(lCliID);
                Data.ContactosTipos contactoDefecto = cContactos.GetDefaultByClienteID(lCliID);
                if (contactoDefecto != null && cmbTipoContacto.Value.ToString() == "")
                {
                    cmbTipoContacto.Value = contactoDefecto.ContactoTipoID;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar()
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesController cContactos = new ContactosGlobalesController();

            MunicipiosController cMunicipios = new MunicipiosController();
            cMunicipios.SetDataContext(cContactos.Context);

            Data.ContactosGlobales oContacto;
            InfoResponse oResponse;
            long lCliID;
            long lEmpID;
            long lMunicipioID = 0;
            long? lPaisID;

            direct.Success = true;
            direct.Result = "";

            hdEditando.SetValue("");

            try
            {

                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());
                lEmpID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoSeleccionado")).Value.ToString());

                if (hdContactoGlobalID.Value != null && hdContactoGlobalID.Value.ToString() != "")
                {
                    oContacto = cContactos.getContactoByID(Convert.ToInt32(hdContactoGlobalID.Value));

                    if (oContacto != null)
                    {
                        if ((oContacto.Telefono != txtTelefono.Text) || (oContacto.Email != txtEmail.Text))
                        {
                            if (geoPunto.Municipio != "")
                            {
                                lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                                if (lPaisID != 0)
                                {
                                    lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                                }
                            }

                            #region DATOS PRINCIPALES

                            oContacto.Nombre = txtNombre.Text;
                            oContacto.Apellidos = txtApellidos.Text;
                            oContacto.Telefono = txtTelefono.Text;
                            oContacto.Telefono2 = txtTelefono2.Text;
                            oContacto.Email = txtEmail.Text;
                            oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                            #endregion

                            #region DATOS LOCALIZACIONES

                            oContacto.CP = geoPunto.CodigoPostal;
                            oContacto.Direccion = geoPunto.Direccion;
                            oContacto.Comentarios = txaComentarios.Text;
                            oContacto.MunicipioID = lMunicipioID;

                            #endregion

                            oResponse = cContactos.Update(oContacto);

                            if (oResponse.Result)
                            {
                                oResponse = cContactos.SubmitChanges();
                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cContactos.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                            else
                            {
                                cContactos.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        else
                        {
                            if (geoPunto.Municipio != "")
                            {
                                lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                                if (lPaisID != 0)
                                {
                                    lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                                }
                            }

                            #region DATOS PRINCIPALES

                            oContacto.Nombre = txtNombre.Text;
                            oContacto.Apellidos = txtApellidos.Text;
                            oContacto.Telefono = txtTelefono.Text;
                            oContacto.Telefono2 = txtTelefono2.Text;
                            oContacto.Email = txtEmail.Text;
                            oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                            #endregion

                            #region DATOS LOCALIZACIONES

                            oContacto.CP = geoPunto.CodigoPostal;
                            oContacto.Direccion = geoPunto.Direccion;
                            oContacto.Comentarios = txaComentarios.Text;
                            oContacto.MunicipioID = lMunicipioID;

                            #endregion

                            oResponse = cContactos.Update(oContacto);

                            if (oResponse.Result)
                            {
                                oResponse = cContactos.SubmitChanges();
                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cContactos.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                            else
                            {
                                cContactos.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                    }
                }
                else
                {
                    if (geoPunto.Municipio != "")
                    {
                        lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                        if (lPaisID != 0)
                        {
                            lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                        }
                    }

                    oContacto = new Data.ContactosGlobales();

                    oContacto.ClienteID = lCliID;
                    oContacto.Activo = true;

                    #region DATOS PRINCIPALES

                    oContacto.Nombre = txtNombre.Text;
                    oContacto.Apellidos = txtApellidos.Text;
                    oContacto.Telefono = txtTelefono.Text;
                    oContacto.Telefono2 = txtTelefono2.Text;
                    oContacto.Email = txtEmail.Text;
                    oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                    #endregion

                    #region DATOS LOCALIZACIONES

                    oContacto.CP = geoPunto.CodigoPostal;
                    oContacto.Direccion = geoPunto.Direccion;
                    oContacto.Comentarios = txaComentarios.Text;
                    oContacto.MunicipioID = lMunicipioID;

                    #endregion

                    oResponse = cContactos.AddContactoEmplazamiento(oContacto, lEmpID);

                    if (oResponse.Result)
                    {
                        oResponse = cContactos.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            hdContactoGlobalID.Value = oContacto.ContactoGlobalID.ToString();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cContactos.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cContactos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }


            return direct;
        }

        [DirectMethod()]
        public DirectResponse AgregarEditarContacto()
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesController cContactos = new ContactosGlobalesController();

            MunicipiosController cMunicipios = new MunicipiosController();
            cMunicipios.SetDataContext(cContactos.Context);

            Data.ContactosGlobales oContacto;
            InfoResponse oResponse;
            long lCliID;
            long lMunicipioID = 0;
            long? lPaisID;

            direct.Success = true;
            direct.Result = "";

            hdEditando.SetValue("");

            try
            {
                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                if (hdContactoGlobalID.Value != null && hdContactoGlobalID.Value.ToString() != "")
                {
                    oContacto = cContactos.getContactoByID(Convert.ToInt32(hdContactoGlobalID.Value));

                    if (oContacto != null)
                    {
                        if ((oContacto.Telefono != txtTelefono.Text) || (oContacto.Email != txtEmail.Text))
                        {
                            if (geoPunto.Municipio != "")
                            {
                                lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                                if (lPaisID != 0)
                                {
                                    lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                                }
                            }

                            #region DATOS PRINCIPALES

                            oContacto.Nombre = txtNombre.Text;
                            oContacto.Apellidos = txtApellidos.Text;
                            oContacto.Telefono = txtTelefono.Text;
                            oContacto.Telefono2 = txtTelefono2.Text;
                            oContacto.Email = txtEmail.Text;
                            oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                            #endregion

                            #region DATOS LOCALIZACIONES

                            oContacto.CP = geoPunto.CodigoPostal;
                            oContacto.Direccion = geoPunto.Direccion;
                            oContacto.Comentarios = txaComentarios.Text;
                            oContacto.MunicipioID = lMunicipioID;

                            #endregion

                            oResponse = cContactos.Update(oContacto);

                            if (oResponse.Result)
                            {
                                oResponse = cContactos.SubmitChanges();
                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cContactos.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                            else
                            {
                                cContactos.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        else
                        {
                            if (geoPunto.Municipio != "")
                            {
                                lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                                if (lPaisID != 0)
                                {
                                    lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                                }
                            }

                            #region DATOS PRINCIPALES

                            oContacto.Nombre = txtNombre.Text;
                            oContacto.Apellidos = txtApellidos.Text;
                            oContacto.Telefono = txtTelefono.Text;
                            oContacto.Telefono2 = txtTelefono2.Text;
                            oContacto.Email = txtEmail.Text;
                            oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                            #endregion

                            #region DATOS LOCALIZACIONES

                            oContacto.CP = geoPunto.CodigoPostal;
                            oContacto.Direccion = geoPunto.Direccion;
                            oContacto.Comentarios = txaComentarios.Text;
                            oContacto.MunicipioID = lMunicipioID;

                            #endregion

                            oResponse = cContactos.Update(oContacto);

                            if (oResponse.Result)
                            {
                                oResponse = cContactos.SubmitChanges();
                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cContactos.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                            else
                            {
                                cContactos.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                    }
                }
                else
                {
                    if (geoPunto.Municipio != null)
                    {
                        lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                        if (lPaisID != 0)
                        {
                            lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                        }
                    }

                    oContacto = new Data.ContactosGlobales();

                    oContacto.ClienteID = lCliID;
                    oContacto.Activo = true;

                    #region DATOS PRINCIPALES

                    oContacto.Nombre = txtNombre.Text;
                    oContacto.Apellidos = txtApellidos.Text;
                    oContacto.Telefono = txtTelefono.Text;
                    oContacto.Telefono2 = txtTelefono2.Text;
                    oContacto.Email = txtEmail.Text;
                    oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                    #endregion

                    #region DATOS LOCALIZACIONES

                    oContacto.CP = geoPunto.CodigoPostal;
                    oContacto.Direccion = geoPunto.Direccion;
                    oContacto.Comentarios = txaComentarios.Text;
                    oContacto.MunicipioID = lMunicipioID;

                    #endregion

                    oResponse = cContactos.Add(oContacto);

                    if (oResponse.Result)
                    {
                        oResponse = cContactos.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            hdContactoGlobalID.Value = oContacto.ContactoGlobalID.ToString();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cContactos.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cContactos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse AgregarEditarContactoEntidad()
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesController cContactos = new ContactosGlobalesController();

            MunicipiosController cMunicipios = new MunicipiosController();
            cMunicipios.SetDataContext(cContactos.Context);

            Data.ContactosGlobales oContacto;
            InfoResponse oResponse;
            long lCliID;
            long lEntID;
            long lMunicipioID = 0;
            long? lPaisID;

            direct.Success = true;
            direct.Result = "";

            hdEditando.SetValue("");

            try
            {

                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());
                lEntID = long.Parse(((Hidden)X.GetCmp("hdEntidadID")).Value.ToString());

                if (hdContactoGlobalID.Value != null && hdContactoGlobalID.Value.ToString() != "")
                {
                    oContacto = cContactos.getContactoByID(Convert.ToInt32(hdContactoGlobalID.Value));

                    if (oContacto != null)
                    {
                        if ((oContacto.Telefono != txtTelefono.Text) || (oContacto.Email != txtEmail.Text))
                        {
                            if (geoPunto.Municipio != "")
                            {
                                lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                                if (lPaisID != 0)
                                {
                                    lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                                }
                            }

                            #region DATOS PRINCIPALES

                            oContacto.Nombre = txtNombre.Text;
                            oContacto.Apellidos = txtApellidos.Text;
                            oContacto.Telefono = txtTelefono.Text;
                            oContacto.Telefono2 = txtTelefono2.Text;
                            oContacto.Email = txtEmail.Text;
                            oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                            #endregion

                            #region DATOS LOCALIZACIONES

                            oContacto.CP = geoPunto.CodigoPostal;
                            oContacto.Direccion = geoPunto.Direccion;
                            oContacto.Comentarios = txaComentarios.Text;
                            oContacto.MunicipioID = lMunicipioID;

                            #endregion

                            oResponse = cContactos.Update(oContacto);

                            if (oResponse.Result)
                            {
                                oResponse = cContactos.SubmitChanges();
                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cContactos.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                            else
                            {
                                cContactos.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        else
                        {
                            if (geoPunto.Municipio != "")
                            {
                                lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                                if (lPaisID != 0)
                                {
                                    lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                                }
                            }

                            #region DATOS PRINCIPALES

                            oContacto.Nombre = txtNombre.Text;
                            oContacto.Apellidos = txtApellidos.Text;
                            oContacto.Telefono = txtTelefono.Text;
                            oContacto.Telefono2 = txtTelefono2.Text;
                            oContacto.Email = txtEmail.Text;
                            oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                            #endregion

                            #region DATOS LOCALIZACIONES

                            oContacto.CP = geoPunto.CodigoPostal;
                            oContacto.Direccion = geoPunto.Direccion;
                            oContacto.Comentarios = txaComentarios.Text;
                            oContacto.MunicipioID = lMunicipioID;

                            #endregion

                            oResponse = cContactos.Update(oContacto);

                            if (oResponse.Result)
                            {
                                oResponse = cContactos.SubmitChanges();
                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cContactos.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                            else
                            {
                                cContactos.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                    }
                }
                else
                {
                    oContacto = new Data.ContactosGlobales();

                    if (geoPunto.Municipio != "")
                    {
                        lPaisID = cMunicipios.getPaisByMunicipio(geoPunto.Municipio.Split(',')[0]);

                        if (lPaisID != 0)
                        {
                            lMunicipioID = cMunicipios.GetMunicipioByNombre(geoPunto.Municipio.Split(',')[0]);
                        }
                    }

                    oContacto.ClienteID = lCliID;
                    oContacto.Activo = true;

                    #region DATOS PRINCIPALES

                    oContacto.Nombre = txtNombre.Text;
                    oContacto.Apellidos = txtApellidos.Text;
                    oContacto.Telefono = txtTelefono.Text;
                    oContacto.Telefono2 = txtTelefono2.Text;
                    oContacto.Email = txtEmail.Text;
                    oContacto.ContactoTipoID = Convert.ToInt64(cmbTipoContacto.SelectedItem.Value);

                    #endregion

                    #region DATOS LOCALIZACIONES

                    oContacto.CP = geoPunto.CodigoPostal;
                    oContacto.Direccion = geoPunto.Direccion;
                    oContacto.Comentarios = txaComentarios.Text;
                    oContacto.MunicipioID = lMunicipioID;

                    #endregion

                    oResponse = cContactos.AddContactoEntidad(oContacto, lEntID);

                    if (oResponse.Result)
                    {
                        oResponse = cContactos.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            hdContactoGlobalID.Value = oContacto.ContactoGlobalID.ToString();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cContactos.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cContactos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }


            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar(long lEmplazamientoID, long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesEmplazamientosController cEmplazamientos = new ContactosGlobalesEmplazamientosController();
            PaisesController cPaises = new PaisesController();
            MunicipiosController cMunicipios = new MunicipiosController();
            Data.Paises oPais;
            string sMunicipio = "";
            string sProvincia = "";

            try
            {
                Data.ContactosGlobalesEmplazamientos oDato = cEmplazamientos.GetContactosByID(lEmplazamientoID, lContactoGlobalID);
                hdEmplazamientoID.Value = oDato.EmplazamientoID.ToString();
                hdContactoGlobalID.Value = oDato.ContactoGlobalID.ToString();

                Data.ContactosGlobales oContacto = new Data.ContactosGlobales();

                if (oContacto != null)
                {
                    #region DATOS PRINCIPALES

                    txtNombre.Text = oContacto.Nombre;
                    txtApellidos.Text = oContacto.Apellidos;
                    txtTelefono.Text = oContacto.Telefono;
                    txtTelefono2.Text = oContacto.Telefono2;
                    txtEmail.Text = oContacto.Email;

                    if (oContacto.ContactoTipoID != null)
                    {
                        cmbTipoContacto.SetValue(oContacto.ContactoTipoID);
                    }

                    #endregion

                    #region DATOS LOCALIZACIONES

                    if (oContacto.Direccion != "" || oContacto.CP != ""
                        || oContacto.MunicipioID != null || oContacto.MunicipioID != 0)
                    {
                        geoPunto.Direccion = oContacto.Direccion;
                        geoPunto.CodigoPostal = oContacto.CP;

                        oPais = cPaises.GetPaisByMunicipioID((long)oContacto.MunicipioID);
                        sMunicipio = cMunicipios.GetMunicipioByID(oContacto.MunicipioID);
                        sProvincia = cMunicipios.getNombreProvinciaByMunicipioID(sMunicipio);
                        geoPunto.Municipio = sMunicipio + ", " + sProvincia + " (" + oPais.PaisCod + ")";
                    }

                    #endregion

                    txaComentarios.Text = oContacto.Comentarios;

                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }
        [DirectMethod()]
        public DirectResponse MostrarEditarContactoEntidad(long lEntidadID, long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesEntidadesController cEntidades = new ContactosGlobalesEntidadesController();
            PaisesController cPaises = new PaisesController();
            MunicipiosController cMunicipios = new MunicipiosController();
            Data.Paises oPais;
            string sMunicipio = "";
            string sProvincia = "";

            try
            {
                Data.ContactosGlobalesEntidades oDato = cEntidades.GetContactosByID(lEntidadID, lContactoGlobalID);
                hdEntidadID.Value = oDato.EntidadID.ToString();
                hdContactoGlobalID.Value = oDato.ContactoGlobalID.ToString();

                Data.ContactosGlobales oContacto = new Data.ContactosGlobales();

                if (oContacto != null)
                {
                    #region DATOS PRINCIPALES

                    txtNombre.Text = oContacto.Nombre;
                    txtApellidos.Text = oContacto.Apellidos;
                    txtTelefono.Text = oContacto.Telefono;
                    txtTelefono2.Text = oContacto.Telefono2;
                    txtEmail.Text = oContacto.Email;

                    if (oContacto.ContactoTipoID != null)
                    {
                        cmbTipoContacto.SetValue(oContacto.ContactoTipoID);
                    }

                    #endregion

                    #region DATOS LOCALIZACIONES

                    if (oContacto.Direccion != "" || oContacto.CP != ""
                        || oContacto.MunicipioID != null || oContacto.MunicipioID != 0)
                    {
                        geoPunto.Direccion = oContacto.Direccion;
                        geoPunto.CodigoPostal = oContacto.CP;

                        oPais = cPaises.GetPaisByMunicipioID((long)oContacto.MunicipioID);
                        sMunicipio = cMunicipios.GetMunicipioByID(oContacto.MunicipioID);
                        sProvincia = cMunicipios.getNombreProvinciaByMunicipioID(sMunicipio);
                        geoPunto.Municipio = sMunicipio + ", " + sProvincia + " (" + oPais.PaisCod + ")";
                    }

                    #endregion

                    txaComentarios.Text = oContacto.Comentarios;

                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarContacto(long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesController cContactos = new ContactosGlobalesController();
            ContactosTiposController cTipos = new ContactosTiposController();
            PaisesController cPaises = new PaisesController();
            MunicipiosController cMunicipios = new MunicipiosController();
            Data.Paises oPais;
            string sMunicipio = "";
            string sProvincia = "";
            hdEditando.SetValue("Editar");

            try
            {
                Data.ContactosGlobales oDato = cContactos.getContactoByID(lContactoGlobalID);
                hdContactoGlobalID.Value = oDato.ContactoGlobalID.ToString();

                if (oDato != null)
                {
                    #region DATOS PRINCIPALES

                    txtNombre.Text = oDato.Nombre;
                    txtApellidos.Text = oDato.Apellidos;
                    txtTelefono.Text = oDato.Telefono;
                    txtTelefono2.Text = oDato.Telefono2;
                    txtEmail.Text = oDato.Email;

                    if (oDato.ContactoTipoID != null)
                    {
                        cmbTipoContacto.SetValue(oDato.ContactoTipoID);
                    }

                    #endregion

                    #region DATOS LOCALIZACIONES

                    txaComentarios.Text = oDato.Comentarios;

                    if (oDato.Direccion != "" || oDato.CP != ""
                        || oDato.MunicipioID != null || oDato.MunicipioID != 0)
                    {
                        geoPunto.Direccion = oDato.Direccion;
                        geoPunto.CodigoPostal = oDato.CP;

                        oPais = cPaises.GetPaisByMunicipioID((long)oDato.MunicipioID);
                        sMunicipio = cMunicipios.GetMunicipioByID(oDato.MunicipioID);
                        sProvincia = cMunicipios.getNombreProvinciaByMunicipioID(sMunicipio);
                        geoPunto.Municipio = sMunicipio + ", " + sProvincia + " (" + oPais.PaisCod + ")";
                    }

                    //btnGuardarAgregarEditarContacto.Enable();
                    //string javaScript = "addlistenerValidacion();";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "addlistenerValidacion", javaScript, true);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ActivarContacto(long lSeleccionado)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesController cContactos = new ContactosGlobalesController();

            hdEditando.SetValue("");
            InfoResponse oResponse;

            try
            {
                Data.ContactosGlobales oDato = cContactos.GetItem(lSeleccionado);
                oResponse = cContactos.ModificarActivar(oDato);

                if (oResponse.Result)
                {
                    oResponse = cContactos.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                    }
                    else
                    {
                        cContactos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cContactos.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar(long lSeleccionado)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesController cContactos = new ContactosGlobalesController();

            hdEditando.SetValue("");
            InfoResponse oResponse;

            try
            {
                Data.ContactosGlobales oDato = cContactos.GetItem(lSeleccionado);
                oResponse = cContactos.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cContactos.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                    else
                    {
                        cContactos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cContactos.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ComprobarContactoExiste()
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesController cContactos = new ContactosGlobalesController();

            try
            {
                if (hdContactoGlobalID.Value != null && hdContactoGlobalID.Value.ToString() != "")
                {
                    string sEmail = cContactos.getContactoByID(long.Parse(hdContactoGlobalID.Value.ToString())).Email;

                    if (sEmail == txtEmail.Text)
                    {
                        if (cContactos.DuplicadoID(txtTelefono.Text, txtEmail.Text))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsTelefonoExiste);
                        }
                    }
                    else
                    {
                        if (cContactos.DuplicadoEmail(txtEmail.Text))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsEmailExiste);
                        }
                    }
                }
                else
                {
                    if (!cContactos.DuplicadoEmail(txtEmail.Text))
                    {
                        if (cContactos.DuplicadoID(txtTelefono.Text, txtEmail.Text))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsTelefonoExiste);
                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsEmailExiste);
                    }
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = "";
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #region LOAD PREFIJOS
        [DirectMethod]
        public string LoadPrefijos()
        {
            DirectResponse direct = new DirectResponse();
            List<Ext.Net.MenuItem> items = new List<Ext.Net.MenuItem>();

            #region Controllers
            PaisesController cPaises = new PaisesController();
            #endregion

            try
            {

                long clienteID = ClienteID;
                items = cPaises.GetMenuItemsPrefijos(clienteID);
                items.ForEach(i =>
                {
                    Icon icono = (Icon)Enum.Parse(typeof(Icon), i.Icon.ToString());
                });
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return ComponentLoader.ToConfig(items);
        }
        #endregion

        #endregion

        #region FUNCTIONS

        #endregion

    }
}
