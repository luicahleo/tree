using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class AlquileresClasesActivosController : GeneralBaseController<AlquileresClasesActivos, TreeCoreContext>, IBasica<AlquileresClasesActivos>
    {
        public AlquileresClasesActivosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string AlquilerClaseActivo, long clienteID)
        {
            bool isExiste = false;
            List<AlquileresClasesActivos> datos;


            datos = (from c 
                     in Context.AlquileresClasesActivos 
                     where (c.AlquilerClaseActivo == AlquilerClaseActivo && 
                            c.ClienteID == clienteID) 
                     select c).ToList<AlquileresClasesActivos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerClaseActivoID)
        {
            AlquileresClasesActivos dato;
            AlquileresClasesActivosController cController = new AlquileresClasesActivosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AlquilerClaseActivoID == " + AlquilerClaseActivoID.ToString());

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

        public AlquileresClasesActivos GetDefault(long clienteID)
        {
            AlquileresClasesActivos alquilerClaseActivo;
            try
            {
                alquilerClaseActivo = (from c
                                       in Context.AlquileresClasesActivos 
                                       where c.Defecto &&
                                             c.ClienteID == clienteID
                                       select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                alquilerClaseActivo = null;
            }
            return alquilerClaseActivo;
        }
        public AlquileresClasesActivos GetActivoByCodigo(string sNombre)
        {
            List<AlquileresClasesActivos> lista = null;
            AlquileresClasesActivos dato = null;

            try
            {

                lista = (from c in Context.AlquileresClasesActivos where c.CodigoClaseActivo == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<AlquileresClasesActivos>();
                return dato;
            }

            return dato;
        }
    }
}