using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GlobalDireccionesLugaresRetirosAccesosController : GeneralBaseController<GlobalDireccionesLugaresRetirosAccesos, TreeCoreContext>, IBasica<GlobalDireccionesLugaresRetirosAccesos>
    {
        public GlobalDireccionesLugaresRetirosAccesosController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalDireccionLugarRetiroAccesoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GlobalDireccionLugarRetiroAcceso, long clienteID)
        {
            bool isExiste = false;
            List<GlobalDireccionesLugaresRetirosAccesos> datos;


            datos = (from c 
                     in Context.GlobalDireccionesLugaresRetirosAccesos 
                     where c.Nombre == GlobalDireccionLugarRetiroAcceso && 
                            c.ClienteID == clienteID
                     select c).ToList<GlobalDireccionesLugaresRetirosAccesos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalDireccionLugarRetiroAccesoID)
        {
            GlobalDireccionesLugaresRetirosAccesos dato;
            bool defecto = false;
            try
            {
                dato = (from c in Context.GlobalDireccionesLugaresRetirosAccesos
                        where c.Defecto &&
                                c.GlobalDireccionLugarRetiroAccesoID == GlobalDireccionLugarRetiroAccesoID
                        select c).First();

                if (dato != null)
                {
                    defecto = true;
                }
                else
                {
                    defecto = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                defecto = false;
            }
            return defecto;
        }

        public GlobalDireccionesLugaresRetirosAccesos GetDefault(long ClienteID)
        {
            GlobalDireccionesLugaresRetirosAccesos direccionLugarRetiroAcceso;
            try
            {
                direccionLugarRetiroAcceso = (from c
                         in Context.GlobalDireccionesLugaresRetirosAccesos
                         where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direccionLugarRetiroAcceso = null;
            }
            return direccionLugarRetiroAcceso;
        }
    }
}