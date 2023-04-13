using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class DocumentosCargasPlantillasTabsController : GeneralBaseController<DocumentosCargasPlantillasTabs, TreeCoreContext>
    {
        public DocumentosCargasPlantillasTabsController()
            : base()
        { }


        public List<DocumentosCargasPlantillasTabs> GetAllPlantillasConfig(bool bActiva)
        {

            List<DocumentosCargasPlantillasTabs> lista = null;

            try
            {
                if (bActiva)
                {
                    lista = (from c in Context.DocumentosCargasPlantillasTabs where c.Activa select c).ToList();
                }
                else
                {
                    lista = (from c in Context.DocumentosCargasPlantillasTabs select c).ToList();
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }



            return lista;
        }

        public List<DocumentosCargasPlantillasTabs> GetAllPlantillasConfigByPlantillaID(long lPlantillaID, bool bActiva)
        {

            List<DocumentosCargasPlantillasTabs> lista = null;

            try
            {
                if (bActiva)
                {
                    lista = (from c in Context.DocumentosCargasPlantillasTabs where c.DocumentoCargaPlantillaID == lPlantillaID && c.Activa select c).ToList();
                }
                else
                {
                    lista = (from c in Context.DocumentosCargasPlantillasTabs where c.DocumentoCargaPlantillaID == lPlantillaID select c).ToList();
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }



            return lista;
        }

        public DocumentosCargasPlantillasTabs GetPlantillaConfigByPlantillaID(long lPlantillaID, bool bActiva)
        {

            List<DocumentosCargasPlantillasTabs> lista = null;

            DocumentosCargasPlantillasTabs Oretrun = null;
            try
            {
                if (bActiva)
                {
                    lista = (from c in Context.DocumentosCargasPlantillasTabs where c.DocumentoCargaPlantillaID == lPlantillaID && c.Activa select c).ToList();
                }
                else
                {
                    lista = (from c in Context.DocumentosCargasPlantillasTabs where c.DocumentoCargaPlantillaID == lPlantillaID  select c).ToList();
                }
                if (lista.Count > 0)
                {
                    Oretrun = lista[0];
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Oretrun = null;
            }



            return Oretrun;
        }

    }
}