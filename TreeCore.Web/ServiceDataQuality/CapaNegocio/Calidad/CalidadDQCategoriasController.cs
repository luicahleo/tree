using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DQCategoriasController : GeneralBaseController<DQCategorias, TreeCoreContext>
    {
        public DQCategoriasController()
            : base()
        { }

        public bool RegistroVinculado(long DQCategoriaID)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        public bool RegistroDuplicado(string DQCategoria, long lClienteID)
        {
            bool bExiste = false;
            List<DQCategorias> listaDatos;


            listaDatos = (from c in Context.DQCategorias where c.DQCategoria == DQCategoria && c.ClienteID == lClienteID select c).ToList<DQCategorias>();
         
            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public List<DQCategorias> GetAllActivos(long clienteID)
        {
            List<DQCategorias> lista = null;

            try
            {
                lista = (from c in Context.DQCategorias 
                         where 
                            c.Activa && 
                            c.ClienteID==clienteID
                         orderby c.DQCategoria
                         select c).ToList();
            }
            catch (Exception ex)
            {
                return lista;
                log.Error(ex.Message);
            }

            return lista;
        }

        public long getIDByName (string sName)
        {
            long lCategoriaID;

            try
            {
                lCategoriaID = (from c in Context.DQCategorias where c.DQCategoria == sName select c.DQCategoriaID).First();
            }
            catch (Exception ex)
            {
                lCategoriaID = 0;
                log.Error(ex.Message);
            }

            return lCategoriaID;
        }


    }
}