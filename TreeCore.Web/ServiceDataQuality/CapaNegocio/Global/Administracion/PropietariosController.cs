using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class PropietariosController : GeneralBaseController<Propietarios, TreeCoreContext>, IBasica<Propietarios>
    {
        public PropietariosController()
            : base()
        { }

        public bool RegistroVinculado(long PropietarioID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string DNIPropietario, long clienteID)
        {
            bool isExiste = false;
            List<Propietarios> datos;

            datos = (from c 
                     in Context.Propietarios 
                     where (c.DNIPropietario == DNIPropietario && 
                            c.ClienteID == clienteID)
                     select c).ToList<Propietarios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long PropietarioID)
        {
            Propietarios dato;
            PropietariosController cController = new PropietariosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && PropietarioID == " + PropietarioID.ToString());

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

        public Propietarios GetPropietarioByDNI(string dni)
        {

            Propietarios datos = null;
            List<Propietarios> lista;
            try
            {
                lista = (from c in Context.Propietarios where c.DNIPropietario == dni select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    datos = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = null;
            }
            return datos;
        }

        public Vw_PropietariosBase GetViewPropietariosById(long propietarioID)
        {
            Vw_PropietariosBase propietario;
            try
            {
                propietario = (from c 
                               in Context.Vw_PropietariosBase 
                               where c.PropietarioID == propietarioID 
                               select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                propietario = null;
            }
            return propietario;
        }

        public Propietarios GetDefault(long ClienteID)
        {
            Propietarios propietario;
            try
            {
                propietario = (from c 
                               in Context.Propietarios 
                               where c.Defecto && 
                                     c.ClienteID == ClienteID 
                               select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                propietario = null;
            }
            return propietario;
        }
        public bool eliminaPropietario(long propietarioID)
        {
            bool existe = true;
            Propietarios dato;
            dato = (from c in Context.Propietarios where c.PropietarioID == propietarioID select c).First();
            try
            {
                Context.Propietarios.DeleteOnSubmit(dato);
                Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }
            return existe;
        }
    }
}