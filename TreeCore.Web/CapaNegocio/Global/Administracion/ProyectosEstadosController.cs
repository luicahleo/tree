using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ProyectosEstadosController : GeneralBaseController<ProyectosEstados, TreeCoreContext>, IBasica<ProyectosEstados>
    {
        public ProyectosEstadosController()
            : base()
        { }

        public bool RegistroDefecto(long id)
        {
            ProyectosEstados dato = new ProyectosEstados();
            ProyectosEstadosController cController = new ProyectosEstadosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ProyectoEstadoID == " + id.ToString());

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

        public bool RegistroDuplicado(string nombre, long clienteID)
        {
            throw new NotImplementedException();
        }
        public bool RegistroVinculado(long id)
        {
            throw new NotImplementedException();
        }

        public bool RegistroDuplicadoProyecto(string nombre)
        {
            bool isExiste = false;
            List<ProyectosEstados> datos;

            datos = (from c in Context.ProyectosEstados where (c.ProyectoEstado == nombre) select c).ToList<ProyectosEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoCodigo(string codigo)
        {
            bool isExiste = false;
            List<ProyectosEstados> datos;

            datos = (from c in Context.ProyectosEstados where (c.Codigo == codigo) select c).ToList<ProyectosEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }



        public ProyectosEstados GetDefault()
        {
            ProyectosEstados proyectosEstados;
            try
            {
                proyectosEstados = (from c
                         in Context.ProyectosEstados
                                    where c.Defecto
                                    select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectosEstados = null;
            }
            return proyectosEstados;
        }

        public List<ProyectosEstados> GetActivos()
        {
            List<ProyectosEstados> listaDatos = null;
            try
            {
                listaDatos = (from c
                         in Context.ProyectosEstados
                                    where c.Activo
                                    select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaDatos;
        }

    }
}