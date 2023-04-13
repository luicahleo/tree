using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class DispositivosTiposController : GeneralBaseController<DispositivosTipos, TreeCoreContext>, IBasica<DispositivosTipos>
    {
        public DispositivosTiposController()
            : base()
        { }

        public bool RegistroVinculado(long DispositivoTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string DispositivoTipo, long clienteID)
        {
            bool isExiste = false;
            List<DispositivosTipos> datos;

            datos = (from c 
                     in Context.DispositivosTipos 
                     where (c.DispositivoTipo == DispositivoTipo && 
                            c.ClienteID == clienteID) 
                     select c).ToList<DispositivosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long DispositivoTipoID)
        {
            DispositivosTipos dato = new DispositivosTipos();
            DispositivosTiposController cController = new DispositivosTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && DispositivoTipoID == " + DispositivoTipoID.ToString());

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

        public bool RegistroPadre(long DispositivoTipoID, long clienteID)
        {
            bool isPadre = false;
            List<DispositivosTipos> datos = new List<DispositivosTipos>();

            DispositivosTipos dispositivos = new DispositivosTipos();
            dispositivos = GetItem(DispositivoTipoID);

            datos = (from c in Context.DispositivosTipos where (c.DispositivoTipoPadre == dispositivos.DispositivoTipo && c.ClienteID == clienteID) select c).ToList<DispositivosTipos>();

            if (datos.Count > 0)
            {
                isPadre = true;
            }

            return isPadre;
        }

        public List<DispositivosTipos> GetAllTipoDatos()
        {
            List<DispositivosTipos> lista = new List<DispositivosTipos>();
            lista = (from c in Context.DispositivosTipos orderby c.DispositivoTipo ascending select c).ToList();

            return lista;
        }

        public DispositivosTipos GetDefault()
        {
            DispositivosTipos oDispositivo;

            try
            {
                oDispositivo = (from c in Context.DispositivosTipos where c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDispositivo = null;
            }

            return oDispositivo;
        }
    }
}