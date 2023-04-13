using System;
using System.Collections.Generic;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ComponentesController : GeneralBaseController<Componentes, TreeCoreContext>, IBasica<Componentes>
    {
        public ComponentesController()
            : base()
        { }

        public bool RegistroVinculado(long id)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Componente, string Codigo, long clienteID)
        {
            bool isExiste = false;
            List<Componentes> datos;


            datos = (from c 
                     in Context.Componentes 
                     where (c.Codigo == Codigo || c.Componente == Componente) && 
                     c.ClienteID == clienteID
                     select c).ToList<Componentes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicado(string nombre, long clienteID)
        {
            bool isExiste = false;
            List<Componentes> datos;

            datos = (from c
                     in Context.Componentes
                     where (c.Componente == nombre) &&
                     c.ClienteID == clienteID
                     select c).ToList<Componentes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long id)
        {
            Componentes dato;

            dato = (from c 
                    in Context.Componentes
                    where c.Defecto && c.ComponenteID == id
                    select c).First();

            return (dato != null);
        }
        public List<Componentes> GetAllComponentes()
        {
            List<Componentes> lComponentes;

            lComponentes = (from c in Context.Componentes where c.Activo select c).ToList<Componentes>();

            return lComponentes;
        }

        public Componentes GetDefault(long lClienteID)
        {
            Componentes oComponente;

            try
            {
                oComponente = (from c in Context.Componentes where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oComponente = null;
            }

            return oComponente;
        }

        public List<Componentes> GetActivos(long clienteID){
            List<Componentes> listadatos;
            try
            {
                listadatos = (from c in Context.Componentes where c.Activo && c.ClienteID == clienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
    }
}