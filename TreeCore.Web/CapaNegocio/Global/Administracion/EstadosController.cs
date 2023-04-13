using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class EstadosController : GeneralBaseController<CoreEstados, TreeCoreContext>, IGestionBasica<CoreEstados>
    {
        public EstadosController()
               : base()
        { }

        public InfoResponse Add(CoreEstados oEstado)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oEstado))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = AddEntity(oEstado);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Update(CoreEstados oEstado)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oEstado))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = UpdateEntity(oEstado);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Delete(CoreEstados oEstado)
        {
            InfoResponse oResponse;

            try
            {
                if (oEstado.Defecto)
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.jsPorDefecto,
                        Data = null
                    };
                }
                else
                {
                    oResponse = DeleteEntity(oEstado);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse ModificarActivar(CoreEstados oEstado)
        {
            InfoResponse oResponse;

            try
            {
                oEstado.Activo = !oEstado.Activo;
                oResponse = Update(oEstado);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse SetDefecto(CoreEstados oEstado)
        {
            InfoResponse oResponse;
            CoreEstados oDefault;

            try
            {
                oDefault = GetDefault(oEstado.CoreWorkFlowID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.CoreEstadoID != oEstado.CoreEstadoID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }

                        oEstado.Defecto = true;
                        oEstado.Activo = true;
                        oResponse = Update(oEstado);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oEstado
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oEstado.Defecto = true;
                    oEstado.Activo = true;
                    oResponse = Update(oEstado);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public bool RegistroDuplicado(CoreEstados oEstado)
        {
            bool isExiste = false;
            List<CoreEstados> datos;

            datos = (from c in Context.CoreEstados where ((c.Codigo == oEstado.Codigo || c.Nombre == oEstado.Nombre) && c.CoreWorkFlowID == oEstado.CoreWorkFlowID && c.CoreEstadoID != oEstado.CoreEstadoID) select c).ToList<CoreEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoForm(string sCodigo, string sNombre, long lWorkflowID)
        {
            bool isExiste = false;
            List<CoreEstados> datos;

            datos = (from c in Context.CoreEstados where ((c.Codigo == sCodigo || c.Nombre == sNombre) && c.CoreWorkFlowID == lWorkflowID) select c).ToList<CoreEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public CoreEstados GetDefault(long? lWorkflowID)
        {
            CoreEstados oEstado;

            try
            {
                oEstado = (from c
                         in Context.CoreEstados
                          where c.Defecto &&
                                 c.CoreWorkFlowID == lWorkflowID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }

        public List<Vw_CoreEstados> GetCoreEstadosFromWorkflowID(long? lWorkflowID)
        {
            List<Vw_CoreEstados> listaDatos = new List<Vw_CoreEstados>();
            List<CoreEstados> listaEstados;

            try
            {
                listaEstados = (from CoreEstados in Context.CoreEstados
                              where CoreEstados.CoreWorkFlowID == lWorkflowID
                              orderby CoreEstados.Porcentaje
                              select CoreEstados).ToList();

                foreach (CoreEstados oDato in listaEstados)
                {
                    Vw_CoreEstados oEstado = (from c in Context.Vw_CoreEstados where c.CoreEstadoID == oDato.CoreEstadoID select c).First();

                    if (oEstado != null)
                    {
                        listaDatos.Add(oEstado);
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

        public List<CoreEstados> GetCoreEstadosFromWorkflow(long lWorkflowID)
        {
            List<CoreEstados> lista;

            try
            {
                lista = (from CoreEstados in Context.CoreEstados
                         join CoreWorkflows in Context.CoreWorkflows on CoreEstados.CoreWorkFlowID equals CoreWorkflows.CoreWorkFlowID
                         where CoreEstados.CoreWorkFlowID == lWorkflowID
                         select CoreEstados).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<CoreEstados> GetVistaCoreEstadosFromWorkflow(long lWorkflowID)
        {
            List<CoreEstados> lista;

            try
            {
                lista = (from CoreEstados in Context.CoreEstados
                         join CoreWorkflows in Context.CoreWorkflows on CoreEstados.CoreWorkFlowID equals CoreWorkflows.CoreWorkFlowID
                         where CoreEstados.CoreWorkFlowID == lWorkflowID
                         select CoreEstados).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

        return lista;
        }

        public bool ExisteEstadoByWorkflow(long lWorkflowID, string sCodigo, string sNombre)
        {
            bool bTiene = false;
            List<CoreEstados> listaDatos = new List<CoreEstados>();

            listaDatos = (from c in Context.CoreEstados where c.CoreWorkFlowID == lWorkflowID && c.Codigo == sCodigo && c.Nombre == sNombre select c).ToList();

         if (listaDatos.Count > 0)
            {
                bTiene = true;
            }

            return bTiene;
        }

        public string getNombreEstado(long lEstadoID)
        {
            string sNombre;

            try
            {
                sNombre = (from c in Context.CoreEstados where c.CoreEstadoID == lEstadoID select c.Nombre).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sNombre = null;
            }

            return sNombre;
        }

        public List<CoreEstadosSiguientes> getEstadosSiguientes (long lEstadoID)
        {
            List<CoreEstadosSiguientes> listaDatos;

            try
            {
                listaDatos = (from EstSig in Context.CoreEstadosSiguientes
                            join Est in Context.Vw_CoreEstados on EstSig.CoreEstadoPosibleID equals Est.CoreEstadoID
                            where EstSig.CoreEstadoID == lEstadoID 
                            select EstSig).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<CoreEstados> getSiguientesEstadosByUsuario(long lEstadoID, long lUsuarioID)
        {
            List<CoreEstados> listaDatos;

            try
            {
                listaDatos = (from est in Context.CoreEstados
                              join estSig in Context.CoreEstadosSiguientes on est.CoreEstadoID equals estSig.CoreEstadoPosibleID
                              join estRol in Context.CoreEstadosRoles on est.CoreEstadoID equals estRol.CoreEstadoID
                              join usuRol in Context.UsuariosRoles on estRol.RolID equals usuRol.RolID
                              where estSig.CoreEstadoID == lEstadoID && usuRol.UsuarioID == lUsuarioID
                              select est).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<Vw_CoreEstados> getEstadosSiguientesLibres(long lEstadoID, long lWorkflowID)
        {
            List<CoreEstados> listaDatos;
            List<Vw_CoreEstados> listaEstados = new List<Vw_CoreEstados>();
            List<long> listaIDs;

            try
            {
                listaIDs = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID select c.CoreEstadoPosibleID).ToList();
                listaDatos = (from c in Context.CoreEstados where c.CoreWorkFlowID == lWorkflowID && c.Activo && c.CoreEstadoID != lEstadoID && !listaIDs.Contains(c.CoreEstadoID) orderby c.Nombre select c).ToList();

                foreach (CoreEstados oDato in listaDatos)
                {
                    Vw_CoreEstados oEstado = (from c in Context.Vw_CoreEstados where c.CoreEstadoID == oDato.CoreEstadoID select c).First();

                    if (oEstado != null)
                    {
                        listaEstados.Add(oEstado);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaEstados = null;
            }

            return listaEstados;
        }

        public List<CoreEstados> getAllEstados(long lEstadoID, long lWorkflowID)
        {
            List<CoreEstados> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreEstados where c.CoreEstadoID != lEstadoID && c.CoreWorkFlowID == lWorkflowID orderby c.Nombre select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

          return listaDatos;
        }

        public Vw_CoreEstados getVistaEstado(long lEstadoID, long lWorkflowID)
        {
            Vw_CoreEstados oDato;
            CoreEstados oTabla;

            try
            {
                oTabla = (from c in Context.CoreEstados where c.CoreEstadoID == lEstadoID && c.CoreWorkFlowID == lWorkflowID select c).First();
                oDato = (from c in Context.Vw_CoreEstados where c.CoreEstadoID == oTabla.CoreEstadoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

         return oDato;
        }

        public long? GetEstadoIDByWorkflow(string sCodigo, long lWorkflowID)
        {
            long? lDatos = null;

            try
            {
                lDatos = (from c in Context.CoreEstados where c.Codigo == sCodigo && c.CoreWorkFlowID == lWorkflowID select c.CoreEstadoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lDatos = null;
            }

            return lDatos;
        }

    }
}