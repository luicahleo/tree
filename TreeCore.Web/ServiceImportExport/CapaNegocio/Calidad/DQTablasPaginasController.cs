using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DQTablasPaginasController : GeneralBaseController<DQTablasPaginas, TreeCoreContext>
    {
        public DQTablasPaginasController()
            : base()
        { }

        public bool RegistroVinculado(long DQGroupID)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        public bool RegistroDuplicado(string Alias)
        {
            bool bExiste = false;
            List<DQTablasPaginas> listaDatos;


            listaDatos = (from c in Context.DQTablasPaginas where (c.Alias == Alias) select c).ToList<DQTablasPaginas>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }


        public bool RegistroDuplicadoTablas(long TablaID)
        {
            bool bExiste = false;
            List<DQTablasPaginas> listaDatos;


            listaDatos = (from c in Context.DQTablasPaginas where (c.TablaModeloDatosID == TablaID) select c).ToList<DQTablasPaginas>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public List<Vw_DQTablasPaginas> getTablasActivas()
        {
            List<Vw_DQTablasPaginas> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_DQTablasPaginas 
                              where c.Activo 
                              select c).ToList<Vw_DQTablasPaginas>();  
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        public Vw_DQTablasPaginas GetVwItem(long id)
        {
            Vw_DQTablasPaginas item;
            try
            {
                item = (from c in Context.Vw_DQTablasPaginas 
                        where c.DQTablaPaginaID == id
                        select c).First();
            }
            catch (Exception ex)
            {
                item = null;
                log.Error(ex.Message);
            }
            return item;
        }

        public string getClaveTabla (long lTablaID)
        {
            string sClave;

            try
            {
                sClave = (from c in Context.Vw_DQTablasPaginas where c.DQTablaPaginaID == lTablaID select c.ClaveRecurso).First();
            }
            catch (Exception ex)
            {
                sClave = "";
                log.Error(ex.Message);
            }

            return sClave;
        }

        public long getIDByTabla (string sNombre)
        {
            long lTablaID;

            try
            {
                lTablaID = (from c in Context.Vw_DQTablasPaginas where c.ClaveRecurso == sNombre select c.DQTablaPaginaID).First();
            }
            catch (Exception ex)
            {
                lTablaID = 0;
                log.Error(ex.Message);
            }

            return lTablaID;
        }


        public long GetDQTablaIDByTablaModeloDatoID(long TablaModeloDatoID)
        {
            long lTablaID;

            try
            {
                lTablaID = (from c in Context.DQTablasPaginas where c.TablaModeloDatosID == TablaModeloDatoID select c.DQTablaPaginaID).First();
            }
            catch (Exception ex)
            {
                lTablaID = 0;
                log.Error(ex.Message);
            }

            return lTablaID;

        }

    }
}