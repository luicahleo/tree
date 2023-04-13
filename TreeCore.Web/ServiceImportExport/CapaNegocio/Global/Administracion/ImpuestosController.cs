using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ImpuestosController : GeneralBaseController<Impuestos, TreeCoreContext>
    {
        public ImpuestosController()
            : base()
        { }
                
        public bool RegistroDuplicado(string Impuesto, long PaisID, long clienteID)
        {
            bool isExiste = false;
            List<Impuestos> datos;

            datos = (from c 
                     in Context.Impuestos 
                     where (c.Impuesto == Impuesto || 
                     (c.Impuesto == Impuesto && c.PaisID == PaisID) ) &&
                     c.ClienteID == clienteID
                     select c).ToList();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public Impuestos GetDefault(long clienteID)
        {
            Impuestos impuesto;
            try
            {
                impuesto = (from c
                           in Context.Impuestos
                            where c.Defecto &&
                                 c.ClienteID == clienteID
                            select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                impuesto = null;
            }
            return impuesto;
        }


        public Impuestos GetImpuestosByNombre(string sNombre)
        {
            Impuestos dato = null;
            List<Impuestos> lista = null;

            try
            {

                lista = (from c in Context.Impuestos where c.Impuesto == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }

       

    }
}