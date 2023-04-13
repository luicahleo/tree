using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.Data;
using TreeCore.Page;

namespace TreeCore.PaginasComunes
{
    public partial class ProyectosUsuarios : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseUserControl currentUC;

        public long ProyectoID
        {
            get { return long.Parse(hdProyectoSeleccionadoID.Value.ToString()); }
            set { hdProyectoSeleccionadoID.SetValue(value); hdProyectoSeleccionadoID.DataBind(); }
        }

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                //             //#region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storeUsuarios, gridPrincipal.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //             //#endregion



                //             #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                //             #endregion



                //hdClienteID.SetValue(ClienteID);
                storeUsuarios.Reload();
                #region EXCEL
                if (Request.QueryString["opcion"] != null)
                {
                    string sOpcion = Request.QueryString["opcion"];

                    if (sOpcion == "EXPORTAR")
                    {
                        try
                        {
                            List<Data.Vw_ProyectosUsuarios> listaDatos;
                            string sOrden = Request.QueryString["orden"];
                            string sDir = Request.QueryString["dir"];
                            string sFiltro = Request.QueryString["filtro"];
                            int iCount = 0;
                            //long sProyecto = long.Parse(hdProyectoSeleccionadoID.Value.ToString());

                            listaDatos = ListaUsuariosProyectos(0, 0, sOrden, sDir, ref iCount, sFiltro, ProyectoID);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridPrincipal.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.strProyectos).ToString(), _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            Response.Write("ERROR: " + ex.Message);
                        }

                        Response.End();
                    }

                    ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
                }
            }
            #endregion
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        #region STORES

        #region STORE USUARIOS PROYECTOS
        protected void storeUsuarios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    List<Vw_ProyectosUsuarios> lista = null;
                    if (e.Parameters["proyectoIDParametro"] != null && e.Parameters["proyectoIDParametro"] != "")
                    {
                        ProyectoID = long.Parse(e.Parameters["proyectoIDParametro"].ToString());
                        lista = ListaUsuariosProyectos(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, ProyectoID);
                    }

                    if (lista != null)
                    {
                        storeUsuarios.DataSource = lista;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Vw_ProyectosUsuarios> ListaUsuariosProyectos(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long proyectoID)
        {
            List<Data.Vw_ProyectosUsuarios> listaDatos;
            UsuariosProyectosController cProyectosUsuarios = new UsuariosProyectosController();

            try
            {
                listaDatos = cProyectosUsuarios.GetItemsWithExtNetFilterList<Data.Vw_ProyectosUsuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ProyectoID==" + proyectoID);

            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion


        #region USUARIOSPROYECTOSVINCULADOS
       
        protected void storeUsuariosLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    //var lista = ListaUsuariosLibres();
                    List<UsuariosProyectosLibres> lista = ListaUsuariosLibres();
                    lista.AddRange(ListaEntidades());
                    lista.AddRange(ListaDepartamentos());

                    if (lista != null)
                    {
                        storeUsuariosLibres.DataSource = lista;

                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private class UsuariosProyectosLibres
        {
            private long _usuariosProyectosLibresID;
            private string _nombre;
            private long? _clienteID;
            private string _tipo;
            public long UsuariosProyectosLibresID
            {
                get => _usuariosProyectosLibresID;
                set
                {
                    _usuariosProyectosLibresID = value;

                }
            }
            public string Nombre
            {
                get => _nombre;
                set
                {
                    _nombre = value;

                }
            }
            public long? ClienteID
            {
                get => _clienteID;
                set
                {
                    _clienteID = value;

                }
            }
            public string Tipo
            {
                get => _tipo;
                set
                {
                    _tipo = value;

                }
            }

            public UsuariosProyectosLibres(long id, string nombre, long? clienteID, string tipo)
            {
                _usuariosProyectosLibresID = id;
                _nombre = nombre;
                _clienteID = clienteID;
                _tipo = tipo;
            }
        }

        private List<UsuariosProyectosLibres> ListaUsuariosLibres()
        {
            List<UsuariosProyectosLibres> lista = new List<UsuariosProyectosLibres>();
            List<Usuarios> listaDatos;
            UsuariosProyectosController cProyectosEstados = new UsuariosProyectosController();
            try
            {
                listaDatos = cProyectosEstados.ProyectosUsuariosNoVinculados(ProyectoID);

                foreach (Usuarios item in listaDatos)
                {
                    lista.Add(new UsuariosProyectosLibres(item.UsuarioID, item.NombreCompleto, item.ClienteID, "Usuarios"));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return lista;
        }

        private List<UsuariosProyectosLibres> ListaEntidades()
        {
            List<UsuariosProyectosLibres> lista = new List<UsuariosProyectosLibres>();
            List<object> listaDatos;
            EntidadesController cEntidades = new EntidadesController();
            //long cliID = long.Parse(hdClienteID.Value.ToString());
            try
            {
                listaDatos = cEntidades.listaEntidadesProyectos(ProyectoID, 14);

                foreach (Entidades item in listaDatos)
                {
                    lista.Add(new UsuariosProyectosLibres(item.EntidadID, item.Nombre, item.ClienteID, "Entidades"));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return lista;
        }

        private List<UsuariosProyectosLibres> ListaDepartamentos()
        {
            List<UsuariosProyectosLibres> lista = new List<UsuariosProyectosLibres>();
            List<Departamentos> listaDatos;
            EntidadesController cEntidades = new EntidadesController();
            DepartamentosController cDepartamentos = new DepartamentosController();
            //long cliID = long.Parse(hdClienteID.Value.ToString());
            try
            {
                listaDatos = cDepartamentos.GetAllByClienteID(14, ProyectoID);

                foreach (Departamentos item in listaDatos)
                {

                    lista.Add(new UsuariosProyectosLibres(item.DepartamentoID, item.Departamento, item.ClienteID, "Departamentos"));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return lista;
        }
        #endregion



        #endregion


        #region DIRECTMETHOD

        #region USUARIOSPROYECTOS
        [DirectMethod]
        public DirectResponse AsignarProyecto(long usuarioID, long? clienteID,string accion)
        {
            DirectResponse direct = new DirectResponse();
            UsuariosProyectosController cUsuariosProyectos = new UsuariosProyectosController();

            try
            {
                if (ProyectoID != 0)
                {
                    long lProyectoID = ProyectoID;

                    if (accion == "eliminar")
                    {
                        UsuariosProyectos oDato = cUsuariosProyectos.GetUsuarioProyectoByIDs(lProyectoID, usuarioID);

                        if (cUsuariosProyectos.DeleteItem(oDato.UsuariosProyectosID))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                    }
                    else
                    {
                        if (!cUsuariosProyectos.controlDuplicidad(lProyectoID, usuarioID))
                        {
                            UsuariosProyectos lDato = cUsuariosProyectos.GetUsuarioProyectoByIDs(lProyectoID, usuarioID);

                            long? cliID = clienteID;
                            Data.UsuariosProyectos oDato = new Data.UsuariosProyectos();
                            oDato.UsuarioID = usuarioID;
                            oDato.ProyectoID = lProyectoID;
                            oDato.ClienteID = cliID;
                            cUsuariosProyectos.AddItem(oDato);
                        }

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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod]
        public DirectResponse AsignarUsuariosEntidades(long entidadID)
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cEntidades = new UsuariosController();
            List<Usuarios> listaDatos;
            try
            {
                if (ProyectoID != 0)
                {
                    long lProyectoID = ProyectoID;
                    listaDatos = cEntidades.UsuariosPorEntidad(entidadID);

                    foreach (Usuarios item in listaDatos)
                    {
                        AsignarProyecto(item.UsuarioID, item.ClienteID, "agregar");
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod]
        public DirectResponse AsignarUsuariosDepartamentos(long departamentoID)
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cEntidades = new UsuariosController();
            List<Usuarios> listaDatos;
            try
            {
                if (ProyectoID != 0)
                {
                    long lProyectoID = ProyectoID;
                    listaDatos = cEntidades.UsuariosPorDepartamento(departamentoID);

                    foreach (Usuarios item in listaDatos)
                    {
                        AsignarProyecto(item.UsuarioID, item.ClienteID, "agregar");
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }
        #endregion

        #endregion

    }
}