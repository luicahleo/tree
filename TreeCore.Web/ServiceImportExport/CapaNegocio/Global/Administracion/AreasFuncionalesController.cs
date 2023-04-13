using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class AreasFuncionalesController : GeneralBaseController<AreasFuncionales, TreeCoreContext>, IBasica<AreasFuncionales>
    {
        public AreasFuncionalesController()
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

        public AreasFuncionales GetDefault(long clienteID)
        {
            AreasFuncionales areaFuncional;
            try
            {
                areaFuncional = (from c in Context.AreasFuncionales where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                areaFuncional = null;
            }
            return areaFuncional;
        }

        public List<AreasFuncionales> GetActivos(long clienteID) {
            List<AreasFuncionales> listadatos;
            try
            {
                listadatos = (from c in Context.AreasFuncionales where c.Activo && c.ClienteID == clienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        public AreasFuncionales GetAreaFuncionalByNombre(String nombre)
        {
            List<TreeCore.Data.AreasFuncionales> lista = new List<AreasFuncionales>();
            AreasFuncionales dato = null;
            try
            {
                lista = (from c in Context.AreasFuncionales where (c.AreaFuncional == nombre) select c).ToList();
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