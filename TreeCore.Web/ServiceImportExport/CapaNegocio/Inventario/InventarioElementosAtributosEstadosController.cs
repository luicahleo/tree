using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InventarioElementosAtributosEstadosController : GeneralBaseController<InventarioElementosAtributosEstados, TreeCoreContext>
    {
        public InventarioElementosAtributosEstadosController()
            : base()
        { }

        public bool RegistroVinculado(long InventarioElementoAtributoEstadoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string InventarioElementoAtributoEstado, string sCodigo, long clienteID)
        {
            bool isExiste = false;
            List<InventarioElementosAtributosEstados> datos = new List<InventarioElementosAtributosEstados>();


            datos = (from c in Context.InventarioElementosAtributosEstados where 
                     ((c.Nombre == InventarioElementoAtributoEstado || c.Codigo == sCodigo) && c.ClienteID == clienteID) 
                     select c).ToList<InventarioElementosAtributosEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoCodigo(string Codigo, long clienteID)
        {
            bool isExiste = false;
            List<InventarioElementosAtributosEstados> datos = new List<InventarioElementosAtributosEstados>();


            datos = (from c in Context.InventarioElementosAtributosEstados where (c.Codigo == Codigo && c.ClienteID == clienteID) select c).ToList<InventarioElementosAtributosEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long InventarioElementoAtributoEstadoID)
        {
            InventarioElementosAtributosEstados dato = new InventarioElementosAtributosEstados();
            InventarioElementosAtributosEstadosController cController = new InventarioElementosAtributosEstadosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && InventarioElementoAtributoEstadoID == " + InventarioElementoAtributoEstadoID.ToString());

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

        public InventarioElementosAtributosEstados GetDefault(long lClienteID)
        {
            List<InventarioElementosAtributosEstados> listaDatos;

            listaDatos = (from c in Context.InventarioElementosAtributosEstados where c.Defecto && c.Activo && c.ClienteID == lClienteID select c).ToList();
            
            if (listaDatos.Count == 1)
            {
                return listaDatos.FirstOrDefault();
            }

            return null;
        }

        public long GetIDDefault(long lClienteID)
        {
            List<InventarioElementosAtributosEstados> listaDatos;
            long ID = 0;
            listaDatos = (from c in Context.InventarioElementosAtributosEstados where c.Defecto && c.Activo && c.ClienteID == lClienteID select c).ToList();

            if (listaDatos.Count == 1)
            {
                return listaDatos.FirstOrDefault().InventarioElementoAtributoEstadoID;
            }

            return ID;
        }

        public long GetEstadoIDByNombre(long lClienteID,string nombre)
        {
            List<InventarioElementosAtributosEstados> listaDatos;
            long ID = 0;
            listaDatos = (from c in Context.InventarioElementosAtributosEstados where c.Nombre ==nombre &&c.Activo && c.ClienteID == lClienteID select c).ToList();

            if (listaDatos.Count == 1)
            {
                return listaDatos.FirstOrDefault().InventarioElementoAtributoEstadoID;
            }

            return ID;
        }

        public InventarioElementosAtributosEstados GetEstadoByNombre(long lClienteID,string nombre)
        {
            List<InventarioElementosAtributosEstados> listaDatos;
            InventarioElementosAtributosEstados ID; 
            try
            {
                ID = (from c in Context.InventarioElementosAtributosEstados where c.Nombre == nombre && c.Activo && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                ID = null;
                log.Error(ex.Message);
            }
            return ID;
        }

        public long GetEstadoIDByCodigo(long lClienteID,string sCodigo)
        {
            List<InventarioElementosAtributosEstados> listaDatos;
            long ID = 0;
            listaDatos = (from c in Context.InventarioElementosAtributosEstados where c.Codigo == sCodigo && c.Activo && c.ClienteID == lClienteID select c).ToList();

            if (listaDatos.Count == 1)
            {
                return listaDatos.FirstOrDefault().InventarioElementoAtributoEstadoID;
            }

            return ID;
        }


        public List<InventarioElementosAtributosEstados> GetActivos(long clienteID) {
            List<InventarioElementosAtributosEstados> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioElementosAtributosEstados where c.Activo && c.ClienteID == clienteID  orderby c.Nombre select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        //public List<InventarioElementosAtributosEstados> GetActivosLibres (long lEstadoID)
        //{
        //    List<InventarioElementosAtributosEstados> listaDatos;
        //    List<long?> listaIDs;

        //    try
        //    {
        //        listaIDs = (from c in Context.CoreEstadosGlobales where c.CoreEstadoID == lEstadoID select c.InventarioElementoAtributoEstadoID).ToList();
        //        listaDatos = (from c in Context.InventarioElementosAtributosEstados where c.Activo && !listaIDs.Contains(c.InventarioElementoAtributoEstadoID) select c).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaDatos = null;
        //    }

        //    return listaDatos;
        //}
    }
}