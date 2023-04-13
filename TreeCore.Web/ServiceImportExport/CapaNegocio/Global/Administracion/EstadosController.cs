using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EstadosController : GeneralBaseController<CoreEstados, TreeCoreContext>
    {
        public EstadosController()
               : base()
        { }

        public bool RegistroDuplicado(string sCodigo, string sNombre, long lTipologiaID)
        {
            bool isExiste = false;
            List<CoreEstados> datos = new List<CoreEstados>();

            //datos = (from c in Context.CoreEstados where (c.Codigo == sCodigo && c.Nombre == sNombre && c.CoreTipologiaID == lTipologiaID) select c).ToList<CoreEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        //public List<Vw_CoreEstados> GetCoreEstadosFromProyectoTipo(List<long> listaTipologias)
        //{
        //    List<Vw_CoreEstados> listaDatos;

        //    try
        //    {
        //        listaDatos = (from CoreEstados in Context.Vw_CoreEstados 
        //                      where listaTipologias.Contains(CoreEstados.CoreTipologiaID) select CoreEstados).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaDatos = null;
        //    }

        //    return listaDatos;
        //}

        //public CoreEstados GetDefault(long lTipologiaID)
        //{
        //    CoreEstados oEstado;

        //    try
        //    {
        //        oEstado = (from c in Context.CoreEstados where c.CoreTipologiaID == lTipologiaID && c.Defecto select c).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        oEstado = null;
        //    }

        //    return oEstado;
        //}

        //public List<CoreEstados> GetCoreEstadosFromTipologia (long lTipologiaID)
        //{
        //    List<CoreEstados> lista;

        //    try
        //    {
        //        lista = (from CoreEstados in Context.CoreEstados
        //                      join CoreTipologias in Context.Vw_CoreTipologias on CoreEstados.CoreTipologiaID equals CoreTipologias.CoreTipologiaID
        //                      where CoreEstados.CoreTipologiaID == lTipologiaID
        //                      select CoreEstados).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        lista = null;
        //    }

        //    return lista;
        //}

        //public List<Vw_CoreEstados> GetVistaCoreEstadosFromTipologia(long lTipologiaID)
        //{
        //    List<Vw_CoreEstados> lista;

        //    try
        //    {
        //        lista = (from CoreEstados in Context.Vw_CoreEstados
        //                 join CoreTipologias in Context.Vw_CoreTipologias on CoreEstados.CoreTipologiaID equals CoreTipologias.CoreTipologiaID
        //                 where CoreEstados.CoreTipologiaID == lTipologiaID
        //                 select CoreEstados).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        lista = null;
        //    }

        //    return lista;
        //}

        //public bool ExisteEstadoByTipologia(long lTipologiaID, string sCodigo)
        //{
        //    bool bTiene = false;
        //    List<CoreEstados> listaDatos = new List<CoreEstados>();

        //    listaDatos = (from c in Context.CoreEstados where c.CoreTipologiaID == lTipologiaID && c.Codigo == sCodigo select c).ToList();

        //    if (listaDatos.Count > 0)
        //    {
        //        bTiene = true;
        //    }

        //    return bTiene;
        //}

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

        //public List<Vw_CoreEstados> getEstadosSiguientesLibres(long lEstadoID, long lTipologiaID)
        //{
        //    List<Vw_CoreEstados> listaDatos;
        //    List<long> listaIDs;

        //    try
        //    {
        //        listaIDs = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID select c.CoreEstadoPosibleID).ToList();
        //        listaDatos = (from c in Context.Vw_CoreEstados where c.CoreTipologiaID == lTipologiaID && c.CoreEstadoID != lEstadoID && !listaIDs.Contains(c.CoreEstadoID) select c).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaDatos = null;
        //    }

        //    return listaDatos;
        //}

        //public List<CoreEstados> getAllEstados(long lEstadoID, long lTipologiaID)
        //{
        //    List<CoreEstados> listaDatos;

        //    try
        //    {
        //        listaDatos = (from c in Context.CoreEstados where c.CoreEstadoID != lEstadoID && c.CoreTipologiaID == lTipologiaID select c).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaDatos = null;
        //    }

        //    return listaDatos;
        //}

        //public Vw_CoreEstados getVistaEstado(long lEstadoID, long lTipologiaID)
        //{
        //    Vw_CoreEstados oDato;

        //    try
        //    {
        //        oDato = (from c in Context.Vw_CoreEstados where c.CoreEstadoID == lEstadoID && c.CoreTipologiaID == lTipologiaID select c).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        oDato = null;
        //    }

        //    return oDato;
        //}

        //public long? GetEstadoIDByTipologia(string sCodigo, long lTipologia)
        //{
        //    long? lDatos = null;

        //    try
        //    {
        //        lDatos = (from c in Context.CoreEstados where c.Codigo == sCodigo && c.CoreTipologiaID == lTipologia select c.CoreEstadoID).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        lDatos = null;
        //    }

        //    return lDatos;
        //}
    }
}