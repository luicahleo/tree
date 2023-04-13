using System.Collections.Generic;
using System;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class AlquileresTiposPropietariosController : GeneralBaseController<AlquileresTiposPropietarios, TreeCoreContext>, IBasica<AlquileresTiposPropietarios>
    {
        public AlquileresTiposPropietariosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string AlquilerTipoPropietario, long clienteID)
        {
            bool isExiste = false;
            List<AlquileresTiposPropietarios> datos;


            datos = (from c 
                     in Context.AlquileresTiposPropietarios 
                     where (c.TipoPropietario == AlquilerTipoPropietario && c.ClienteID == clienteID) 
                     select c).ToList<AlquileresTiposPropietarios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerTipoPropietarioID)
        {
            AlquileresTiposPropietarios dato;
            AlquileresTiposPropietariosController cController = new AlquileresTiposPropietariosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AlquilerTipoPropietarioID == " + AlquilerTipoPropietarioID.ToString());

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
        public AlquileresTiposPropietarios GetDefault()
        {
            AlquileresTiposPropietarios alquilerTipoPropietario;
            try
            {
                alquilerTipoPropietario = (from c in Context.AlquileresTiposPropietarios where c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                alquilerTipoPropietario = null;
            }
            return alquilerTipoPropietario;
        }

        public long GetTipoByNombreAll(string Nombre)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.AlquileresTiposPropietarios where c.TipoPropietario.Equals(Nombre) select c.AlquilerTipoPropietarioID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;

            }
            return tipoID;
        }
    }
}