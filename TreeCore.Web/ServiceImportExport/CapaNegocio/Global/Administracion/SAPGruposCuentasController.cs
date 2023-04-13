using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class SAPGruposCuentasController : GeneralBaseController<SAPGruposCuentas, TreeCoreContext>, IBasica<SAPGruposCuentas>
    {
        public SAPGruposCuentasController()
            : base()
        { }

        public bool RegistroVinculado(long SAPGrupoCuentaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string SAPGrupoCuenta, long clienteID)
        {
            bool isExiste = false;
            List<SAPGruposCuentas> datos = new List<SAPGruposCuentas>();


            datos = (from c in Context.SAPGruposCuentas where (c.SAPGrupoCuenta == SAPGrupoCuenta && c.ClienteID == clienteID) select c).ToList<SAPGruposCuentas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SAPGrupoCuentaID)
        {
            SAPGruposCuentas dato = new SAPGruposCuentas();
            SAPGruposCuentasController cController = new SAPGruposCuentasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SAPGrupoCuentaID == " + SAPGrupoCuentaID.ToString());

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

        public SAPGruposCuentas GetDefault(long clienteID)
        {
            SAPGruposCuentas oGrupo;

            try
            {
                oGrupo = (from c in Context.SAPGruposCuentas where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGrupo = null;
            }

            return oGrupo;
        }
        public List<SAPGruposCuentas> GetSAPGruposCuentasByCliente(long clienteID)
        {
            List<SAPGruposCuentas> datos = new List<SAPGruposCuentas>();

            datos = (from c in Context.SAPGruposCuentas where (c.ClienteID == clienteID) orderby c.SAPGrupoCuenta select c).ToList<SAPGruposCuentas>();

            return datos;
        }

        public SAPGruposCuentas GetGrupoCuentaByNombre(string sNombre)
        {
            List<SAPGruposCuentas> lista = null;
            SAPGruposCuentas dato = null;

            try
            {

                lista = (from c in Context.SAPGruposCuentas where c.SAPGrupoCuenta == sNombre select c).ToList();
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