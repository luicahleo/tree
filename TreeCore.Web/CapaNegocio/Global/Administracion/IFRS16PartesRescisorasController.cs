using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class IFRS16PartesRescisorasController : GeneralBaseController<IFRS16PartesRescisoras, TreeCoreContext>, IBasica<IFRS16PartesRescisoras>
    {
        public IFRS16PartesRescisorasController()
            : base()
        { }

        public bool RegistroVinculado(long IFRS16ParteRescisoraID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string parteRecisora, long clienteID)
        {
            bool isExiste = false;
            List<IFRS16PartesRescisoras> datos;


            datos = (from c
                     in Context.IFRS16PartesRescisoras
                     where (c.ParteRescisora == parteRecisora &&
                            c.ClienteID == clienteID)
                     select c).ToList<IFRS16PartesRescisoras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long IFRS16ParteRescisoraID)
        {
            IFRS16PartesRescisoras dato;
            bool defecto = false;

            dato = (from c
                    in Context.IFRS16PartesRescisoras
                    where c.Defecto &&
                            c.IFRS16ParteRescisoraID == IFRS16ParteRescisoraID
                    select c).First();

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

        public IFRS16PartesRescisoras GetDefault(long? ClienteID)
        {
            IFRS16PartesRescisoras parteRecibida;
            try
            {
                if (ClienteID.HasValue)
                {
                    parteRecibida = (from c
                                     in Context.IFRS16PartesRescisoras
                                     where c.Defecto &&
                                            c.ClienteID == ClienteID
                                     select c).First();
                }
                else
                {
                    parteRecibida = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                parteRecibida = null;
            }
            return parteRecibida;
        }

        public IFRS16PartesRescisoras GetParteRescisoraByNombre(string sNombre)
        {
            List<IFRS16PartesRescisoras> lista = null;
            IFRS16PartesRescisoras dato = null;

            try
            {

                lista = (from c in Context.IFRS16PartesRescisoras where c.ParteRescisora == sNombre select c).ToList();
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