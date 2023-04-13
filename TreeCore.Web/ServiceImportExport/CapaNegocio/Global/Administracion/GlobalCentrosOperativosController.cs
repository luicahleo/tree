using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalCentrosOperativosController : GeneralBaseController<GlobalCentrosOperativos, TreeCoreContext>, IBasica<GlobalCentrosOperativos>
    {
        public GlobalCentrosOperativosController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalCentroOperativoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GlobalCentroOperativo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalCentrosOperativos> datos = new List<GlobalCentrosOperativos>();


            datos = (from c in Context.GlobalCentrosOperativos where (c.CentroOperativo == GlobalCentroOperativo) select c).ToList<GlobalCentrosOperativos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalCentroOperativoID)
        {
            GlobalCentrosOperativos dato = new GlobalCentrosOperativos();
            GlobalCentrosOperativosController cController = new GlobalCentrosOperativosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalCentroOperativoID == " + GlobalCentroOperativoID.ToString());

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

        public List<GlobalCentrosOperativos> GetAllItemsActivos(bool soloActivos)
        {
            List<GlobalCentrosOperativos> lista = null;
            if (soloActivos)
            {
                lista = (from c in Context.GlobalCentrosOperativos where c.Activo == soloActivos select c).ToList<GlobalCentrosOperativos>();
            }
            else
            {
                lista = (from c in Context.GlobalCentrosOperativos select c).ToList<GlobalCentrosOperativos>();
            }

            return lista;
        }
    }
}