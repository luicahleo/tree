using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreEstadosGlobalesController : GeneralBaseController<CoreEstadosGlobales, TreeCoreContext>, IGestionBasica<CoreEstadosGlobales>
    {
        public CoreEstadosGlobalesController()
            : base()
        { }

        public InfoResponse Add(CoreEstadosGlobales oGlobal)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oGlobal))
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
                    oResponse = AddEntity(oGlobal);
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

        public InfoResponse Update(CoreEstadosGlobales oGlobal)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oGlobal))
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
                    oResponse = UpdateEntity(oGlobal);
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

        public InfoResponse Delete(CoreEstadosGlobales oGlobal)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oGlobal);
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

        public bool RegistroDuplicado(CoreEstadosGlobales oGlobal)
        {
            bool isExiste = false;
            List<CoreEstadosGlobales> datos;

            datos = (from c in Context.CoreEstadosGlobales where (c.CoreEstadoID == oGlobal.CoreEstadoID && c.ObjetoEstadoID == oGlobal.ObjetoEstadoID && c.CoreObjetoNegocioTipoID == oGlobal.CoreObjetoNegocioTipoID) select c).ToList<CoreEstadosGlobales>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<CoreEstadosGlobales> getCoreEstadosGlobales(long lEstadoID)
        {
            List<CoreEstadosGlobales> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.CoreEstadosGlobales select c).ToList();
                listaDatos = (from EstGlo in Context.CoreEstadosGlobales
                              join Est in Context.Vw_CoreEstados on EstGlo.CoreEstadoID equals Est.CoreEstadoID
                              where EstGlo.CoreEstadoID == lEstadoID
                              select EstGlo).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        //public bool RegistroDuplicado(long lEstadoID, long? lObjetoEstadoID, long? lObjetoID)
        //{
        //    bool isExiste = false;
        //    List<CoreEstadosGlobales> datos = new List<CoreEstadosGlobales>();

        //    datos = (from c in Context.CoreEstadosGlobales
        //             where c.CoreEstadoID == lEstadoID && c.ObjetoEstadoID == lObjetoEstadoID
        //             && c.CoreObjetoNegocioTipoID == lObjetoID
        //             select c).ToList<CoreEstadosGlobales>();

        //    if (datos.Count > 0)
        //    {
        //        isExiste = true;
        //    }

        //    return isExiste;
        //}

        public List<Object> GetByEstadoID(long lEstadoID)
        {
            List<Object> listaEstados = new List<Object>();
            string sNombre = "";
            string sTipo = "";
            Object oValor;
            List<CoreEstadosGlobales> lista = new List<CoreEstadosGlobales>();

            try
            {
                lista = (from c in Context.CoreEstadosGlobales where c.CoreEstadoID == lEstadoID select c).ToList();

                foreach (CoreEstadosGlobales oDato in lista)
                {
                    /*if (oDato.TipoEstado == Comun.MODULOGLOBAL)
                    {
                        sNombre = (from c in Context.EstadosGlobales where c.EstadoGlobalID == oDato.CoreEstadoGlobalID select c.EstadoGlobal).First();
                        sTipo = oDato.TipoEstado;

                        oValor = new
                        {
                            Codigo = sNombre,
                            TipoEstado = sTipo,
                        };

                        listaEstados.Add(oValor);
                    }
                    else if (oDato.TipoEstado == Comun.MODULOINVENTARIO)
                    {
                        sNombre = (from c in Context.InventarioElementosAtributosEstados where c.InventarioElementoAtributoEstadoID == oDato.InventarioElementoAtributoEstadoID select c.Codigo).First();
                        sTipo = oDato.TipoEstado;

                        oValor = new
                        {
                            Codigo = sNombre,
                            TipoEstado = sTipo,
                        };

                        listaEstados.Add(oValor);
                    }
                    else if (oDato.TipoEstado == Comun.MODULODOCUMENTAL)
                    {
                        sNombre = (from c in Context.DocumentEstados where c.DocumentEstadoID == oDato.DocumentoEstadoID select c.DocumentEstado).First();
                        sTipo = oDato.TipoEstado;

                        oValor = new
                        {
                            Codigo = sNombre,
                            TipoEstado = sTipo,
                        };

                        listaEstados.Add(oValor);
                    }

                    */
                }

            }
            catch (Exception)
			{
                listaEstados = null;
            }
            return listaEstados;
        }

        public CoreEstadosGlobales getGlobalByEstadoID(long lEstadoID)
        {
            CoreEstadosGlobales oDato;

            try 
            { 
                oDato = (from c in Context.CoreEstadosGlobales where c.CoreEstadoID == lEstadoID select c).First();
            }
            catch (Exception)
            {
                oDato = null;
            }

            return oDato;
        }

        public List<long?> getObjetosByEstadoID(long lEstadoID)
        {
            List<long?> listaIDs;

            try
            {
                listaIDs = (from c in Context.CoreEstadosGlobales where c.CoreEstadoID == lEstadoID select c.ObjetoEstadoID).ToList();
            }
            catch (Exception)
            {
                listaIDs = null;
            }

            return listaIDs;
        }
    }
}