using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmpresasProveedorasProyectosTiposController : GeneralBaseController<EmpresasProveedorasProyectosTipos, TreeCoreContext>, IBasica<EmpresasProveedorasProyectosTipos>
    {
        public EmpresasProveedorasProyectosTiposController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Codigo, long clienteID)
        {
            bool isExiste = false;
            List<AreasFuncionales> datos = new List<AreasFuncionales>();


            datos = (from c in Context.AreasFuncionales where (c.Codigo == Codigo && c.ClienteID == clienteID) select c).ToList<AreasFuncionales>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AreaFuncionalID)
        {
            AreasFuncionales dato = new AreasFuncionales();
            AreasFuncionalesController cController = new AreasFuncionalesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AreaFuncionalID == " + AreaFuncionalID.ToString());

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

        public bool RegistroDuplicadoDetalle(long pEmpresaProveedoraID, long pProyectoTipoID)
        {
            bool exite = false;

            List<EmpresasProveedorasProyectosTipos> EmpresasProveedorasProyectosTipos = new List<EmpresasProveedorasProyectosTipos>();

            EmpresasProveedorasProyectosTipos = (from c in Context.EmpresasProveedorasProyectosTipos where c.EmpresaProveedoraID == pEmpresaProveedoraID && c.ProyectoTipoID == pProyectoTipoID select c).ToList();

            if (EmpresasProveedorasProyectosTipos.Count > 0)
            {
                exite = true;
            }

            return exite;
        }
        public List<EmpresasProveedorasProyectosTipos> getByEmpresaProveedora(long pEmpresaProveedoraID)
        {
            List<EmpresasProveedorasProyectosTipos> listaDato = new List<EmpresasProveedorasProyectosTipos>();
            listaDato=(from c in Context.EmpresasProveedorasProyectosTipos where c.EmpresaProveedoraID == pEmpresaProveedoraID select c).ToList();
            return listaDato;
        }


    }
}