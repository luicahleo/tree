using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalAreasOYMController : GeneralBaseController<GlobalAreasOYM, TreeCoreContext>, IBasica<GlobalAreasOYM>
    {
        public GlobalAreasOYMController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalAreaOYMID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string AreaOYM, long clienteID)
        {
            bool isExiste = false;
            List<GlobalAreasOYM> datos = new List<GlobalAreasOYM>();


            datos = (from c in Context.GlobalAreasOYM where (c.AreaOYM == AreaOYM && c.ClienteID == clienteID) select c).ToList<GlobalAreasOYM>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalAreaOYMID)
        {
            GlobalAreasOYM dato = new GlobalAreasOYM();
            GlobalAreasOYMController cController = new GlobalAreasOYMController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalAreaOYMID == " + GlobalAreaOYMID.ToString());

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

        public List<GlobalAreasOYM> GetAreasLibresByLocalidad(long LocalidadID)
        {
            List<long> AreasIdsOfLocalidad = (from c in Context.LocalidadesAreasOYM where c.GlobalLocalidadID == LocalidadID select c.GlobalAreaOYMID).ToList();

            return (from c in Context.GlobalAreasOYM where !AreasIdsOfLocalidad.Contains(c.GlobalAreaOYMID) && c.Activo == true select c).ToList();
        }

        public List<Vw_LocalidadesAreasOYM> GetAreasByLocalidad(long LocalidadID)
        {
            List<long> AreasIdsOfLocalidad = (from c in Context.LocalidadesAreasOYM where c.GlobalLocalidadID == LocalidadID select c.GlobalAreaOYMID).ToList();

            return (from c in Context.Vw_LocalidadesAreasOYM where AreasIdsOfLocalidad.Contains(c.GlobalAreaOYMID) select c).ToList();
        }

        public GlobalAreasOYM GetDefault(long clienteID)
        {
            GlobalAreasOYM oGlobalAreasOYM;
            try
            {
                oGlobalAreasOYM = (from c in Context.GlobalAreasOYM where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGlobalAreasOYM = null;
            }
            return oGlobalAreasOYM;
        }
    }
}