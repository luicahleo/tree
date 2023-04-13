using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class TiposDatosController : GeneralBaseController<TiposDatos, TreeCoreContext>, IBasica<TiposDatos>
    {
        public TiposDatosController()
            : base()
        { }

        public bool RegistroVinculado(long TipoDatoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string TipoDato, long clienteID)
        {
            bool isExiste = false;
            List<TiposDatos> datos = new List<TiposDatos>();


            datos = (from c in Context.TiposDatos where (c.TipoDato == TipoDato && c.ClienteID == clienteID) select c).ToList<TiposDatos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long TipoDatoID)
        {
            TiposDatos dato = new TiposDatos();
            TiposDatosController cController = new TiposDatosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && TipoDatoID == " + TipoDatoID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        public List<TiposDatos> GetAllTipoDatos()
        {
            List<TiposDatos> lista = new List<TiposDatos>();
            lista = (from c in Context.TiposDatos orderby c.TipoDato ascending select c).ToList();

            return lista;

        }

        public List<TiposDatos> GetAllTipoDatosActivos()
        {
            List<TiposDatos> lista = new List<TiposDatos>();
            lista = (from c in Context.TiposDatos where c.Activo == true orderby c.TipoDato ascending select c).ToList();

            return lista;

        }

        public List<TiposDatos> GetListasTiposDatos()
        {
            List<TiposDatos> lista = new List<TiposDatos>();
            lista = (from c in Context.TiposDatos where c.Activo == true && (c.Codigo == Comun.TIPODATO_CODIGO_LISTA || c.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE) orderby c.TipoDato ascending select c).ToList();

            return lista;

        }

        public List<TiposDatos> GetAllTiposDatosNoListas()
        {
            List<TiposDatos> lista = new List<TiposDatos>();
            lista = (from c in Context.TiposDatos where c.Activo == true && (c.Codigo != Comun.TIPODATO_CODIGO_LISTA && c.Codigo != Comun.TIPODATO_CODIGO_LISTA_MULTIPLE) orderby c.TipoDato ascending select c).ToList();

            return lista;

        }

        public List<TiposDatos> GetActivos(long clienteID) {
            List<TiposDatos> lista;
            try
            {
                lista = (from c in Context.TiposDatos where c.ClienteID == clienteID && c.Activo orderby c.TipoDato select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public TiposDatos GetTipoDatosByNombre(string sNombre)
        {
            // Local variables
            List<TiposDatos> lista = null;
            TiposDatos tipoDato = null;

            try
            {
                lista = (from c in Context.TiposDatos where c.TipoDato == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    tipoDato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoDato = null;
            }

            return tipoDato;
        }

        public List<TiposDatos> GetTipoDatosByComponenteID(long lID)
        {
            // Local variables
            List<TiposDatos> lista = null;

            try
            {
                lista = (from c in Context.TiposDatos where c.ComponenteID == lID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public TiposDatos GetDefault(long clienteID) {
            TiposDatos oTiposDatos;
            try
            {
                oTiposDatos = (from c in Context.TiposDatos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTiposDatos =null;
            }
            return oTiposDatos;
        }
    }
}