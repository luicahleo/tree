using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class SAPCuentasAsociadasController : GeneralBaseController<SAPCuentasAsociadas, TreeCoreContext>, IBasica<SAPCuentasAsociadas>
    {
        public SAPCuentasAsociadasController()
            : base()
        { }

        public bool RegistroVinculado(long SAPCuentaAsociadaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string SAPCuentaAsociada, long clienteID)
        {
            bool isExiste = false;
            List<SAPCuentasAsociadas> datos = new List<SAPCuentasAsociadas>();


            datos = (from c in Context.SAPCuentasAsociadas where (c.SAPCuentaAsociada == SAPCuentaAsociada && c.ClienteID == clienteID) select c).ToList<SAPCuentasAsociadas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SAPCuentaAsociadaID)
        {
            SAPCuentasAsociadas dato = new SAPCuentasAsociadas();
            SAPCuentasAsociadasController cController = new SAPCuentasAsociadasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SAPCuentaAsociadaID == " + SAPCuentaAsociadaID.ToString());

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

        public SAPCuentasAsociadas GetDefault(long clienteID)
        {
            SAPCuentasAsociadas cuentaAsociada;
            try
            {
                cuentaAsociada = (from c 
                                  in Context.SAPCuentasAsociadas 
                                  where c.Defecto &&
                                        c.ClienteID == clienteID
                                  select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                cuentaAsociada = null;
            }
            return cuentaAsociada;
        }
        public List<SAPCuentasAsociadas> GetSAPCuentasAsociadasByCliente(long clienteID)
        {
            List<SAPCuentasAsociadas> datos = new List<SAPCuentasAsociadas>();

            datos = (from c in Context.SAPCuentasAsociadas where (c.ClienteID == clienteID) orderby c.Descripcion select c).ToList<SAPCuentasAsociadas>();

            return datos;
        }

        public SAPCuentasAsociadas GetCuentaAsociadaByNombre(string sNombre)
        {
            List<SAPCuentasAsociadas> lista = null;
            SAPCuentasAsociadas dato = null;

            try
            {

                lista = (from c in Context.SAPCuentasAsociadas where c.SAPCuentaAsociada == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return dato;
            }

            return dato;
        }

    }
}