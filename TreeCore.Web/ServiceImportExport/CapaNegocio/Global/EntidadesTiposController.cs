using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EntidadesTiposController : GeneralBaseController<EntidadesTipos, TreeCoreContext>
    {
        public EntidadesTiposController()
            : base()
        { }


        public List<EntidadesTipos> GetAllEntidadesTipos()
        {
            // Local variables
            List<EntidadesTipos> lista = null;
            try
            {
                lista = (from c in Context.EntidadesTipos select c).ToList<EntidadesTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }




        public bool RegistroDuplicado(string entidadTipo, long clienteID)
        {
            bool isExiste = false;
            List<EntidadesTipos> datos = new List<EntidadesTipos>();


            datos = (from c in Context.EntidadesTipos where (c.EntidadTipo == entidadTipo && c.ClienteID == clienteID) select c).ToList<EntidadesTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<EntidadesTipos> GetEntidadesTiposByCliente(long clienteID)
        {
            List<EntidadesTipos> datos = new List<EntidadesTipos>();

            datos = (from c in Context.EntidadesTipos where (c.ClienteID == clienteID) select c).ToList<EntidadesTipos>();

            return datos;
        }

        public EntidadesTipos RegistroDefecto(long clienteID)
        {
            EntidadesTipos dato = new EntidadesTipos();
            EntidadesTiposController cController = new EntidadesTiposController();
           
            dato = cController.GetItem("Defecto == true && ClienteID == " + clienteID);
            return dato;
        }

          


        public long GetTipoByNombreAll(string Nombre)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.EntidadesTipos where c.EntidadTipo.Equals(Nombre) select c.EntidadTipoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;

            }
            return tipoID;
        }




    }
}