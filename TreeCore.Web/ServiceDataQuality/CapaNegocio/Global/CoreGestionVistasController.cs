using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class CoreGestionVistasController : GeneralBaseController<CoreGestionVistas, TreeCoreContext>
    {
        public CoreGestionVistasController()
            : base()
        { }

        public CoreGestionVistas GetDefault(string pagina, long lUsuarioID) {
            CoreGestionVistas oDato;
            try
            {
                oDato = (from c in Context.CoreGestionVistas where c.Pagina == pagina && c.UsuarioID == lUsuarioID && c.Defecto select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }
        public List<CoreGestionVistas> GetVistas(string pagina, long lUsuarioID) {
            List<CoreGestionVistas> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreGestionVistas where c.Pagina == pagina && c.UsuarioID == lUsuarioID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }
    }
}