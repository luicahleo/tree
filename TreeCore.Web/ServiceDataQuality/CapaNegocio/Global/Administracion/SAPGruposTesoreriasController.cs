using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class SAPGruposTesoreriasController : GeneralBaseController<SAPGruposTesorerias, TreeCoreContext>, IBasica<SAPGruposTesorerias>
    {
        public SAPGruposTesoreriasController()
            : base()
        { }

        public bool RegistroVinculado(long SAPGrupoTesoreriaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string SAPGrupoTesoreria, long clienteID)
        {
            bool isExiste = false;
            List<SAPGruposTesorerias> datos = new List<SAPGruposTesorerias>();


            datos = (from c in Context.SAPGruposTesorerias where (c.SAPGrupoTesoreria == SAPGrupoTesoreria && c.ClienteID == clienteID) select c).ToList<SAPGruposTesorerias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SAPGrupoTesoreriaID)
        {
            SAPGruposTesorerias dato = new SAPGruposTesorerias();
            SAPGruposTesoreriasController cController = new SAPGruposTesoreriasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SAPGrupoTesoreriaID == " + SAPGrupoTesoreriaID.ToString());

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

        public SAPGruposTesorerias GetDefault(long lClienteID)
        {
            SAPGruposTesorerias oGrupo;

            try
            {
                oGrupo = (from c in Context.SAPGruposTesorerias where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGrupo = null;
            }

            return oGrupo;
        }
        public List<SAPGruposTesorerias> GetSAPGruposTesoreriasByCliente(long clienteID)
        {
            List<SAPGruposTesorerias> datos = new List<SAPGruposTesorerias>();

            datos = (from c in Context.SAPGruposTesorerias where (c.ClienteID == clienteID) orderby c.Descripcion select c).ToList<SAPGruposTesorerias>();

            return datos;
        }

        public SAPGruposTesorerias GetGrupoTesoreriaByNombre(string sNombre)
        {
            List<SAPGruposTesorerias> lista = null;
            SAPGruposTesorerias dato = null;

            try
            {

                lista = (from c in Context.SAPGruposTesorerias where c.SAPGrupoTesoreria == sNombre select c).ToList();
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