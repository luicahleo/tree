using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class DocumentosCargasPlantillasController : GeneralBaseController<DocumentosCargasPlantillas, TreeCoreContext>
    {
        public DocumentosCargasPlantillasController()
            : base()
        { }


        public List<DocumentosCargasPlantillas> GetAllPlantillasSinAyuda(bool bActiva)
        {

            List<DocumentosCargasPlantillas> lista = null;

            try
            {
                if (bActiva)
                {
                    lista = (from c in Context.DocumentosCargasPlantillas where c.Activo && (!c.DocumentoCargaPlantilla.Contains("HELP")) orderby c.DocumentoCargaPlantilla select c).ToList();
                }
                else {
                    lista = (from c in Context.DocumentosCargasPlantillas where (!c.DocumentoCargaPlantilla.Contains("HELP")) orderby c.DocumentoCargaPlantilla select c).ToList();
                }
                

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }



            return lista;
        }
        public List<DocumentosCargasPlantillas> GetAllPlantillas(bool bActiva)
        {

            List<DocumentosCargasPlantillas> lista = null;

            try
            {
                if (bActiva)
                {
                    lista = (from c in Context.DocumentosCargasPlantillas where c.Activo orderby c.DocumentoCargaPlantilla select c).ToList();
                }
                else {
                    lista = (from c in Context.DocumentosCargasPlantillas orderby c.DocumentoCargaPlantilla select c).ToList();
                }
                

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }



            return lista;
        }

        public DocumentosCargasPlantillas GetPlantillaByID(long lPlantillaID, bool bActiva)
        {

            List<DocumentosCargasPlantillas> lista = null;
            DocumentosCargasPlantillas oDocPlantilla = null;

            try
            {
                if (bActiva)
                {
                    lista = (from c in Context.DocumentosCargasPlantillas where c.DocumentoCargaPlantillaID == lPlantillaID && c.Activo select c).ToList();
                }
                else
                {
                    lista = (from c in Context.DocumentosCargasPlantillas where c.DocumentoCargaPlantillaID == lPlantillaID select c).ToList();
                }

                

                if (lista.Count > 0) {
                    oDocPlantilla = new DocumentosCargasPlantillas();
                    oDocPlantilla = lista[0];
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }



            return oDocPlantilla;
        }

        public List<Vw_DocumentosCargasPlantillas> GetListaByFiltro (string sFiltro)
        {
            List<Vw_DocumentosCargasPlantillas> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_DocumentosCargasPlantillas where c.DocumentoCargaPlantilla.Contains(sFiltro) 
                              || c.Alias.Contains(sFiltro) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        public List<DocumentosCargasPlantillas> getPlantillasByProyectoIDSinAyuda(long lProyectoID)
        {
            List<DocumentosCargasPlantillas> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.DocumentosCargasPlantillas
                              where c.ProyectoTipoID == lProyectoID && c.Activo == true && (!c.DocumentoCargaPlantilla.Contains("HELP"))
                              orderby c.DocumentoCargaPlantilla select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        public List<DocumentosCargasPlantillas> getPlantillasByProyectoID(long lProyectoID)
        {
            List<DocumentosCargasPlantillas> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.DocumentosCargasPlantillas
                              where c.ProyectoTipoID == lProyectoID && c.Activo == true
                              orderby c.DocumentoCargaPlantilla select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        public DocumentosCargasPlantillas getPlantillasByProyectoIDByName(long lProyectoID, string sName)
        {
            DocumentosCargasPlantillas oDato = null;

            try
            {
                oDato = (from c in Context.DocumentosCargasPlantillas
                              where c.ProyectoTipoID == lProyectoID && c.Activo == true
                              && c.DocumentoCargaPlantilla == sName select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return oDato;
        }

        public long getIDByName(string sName)
        {
            long lPlantillaID = 0;

            try
            {
                lPlantillaID = (from c in Context.DocumentosCargasPlantillas
                         where c.DocumentoCargaPlantilla == sName && c.Activo == true
                         select c.DocumentoCargaPlantillaID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return lPlantillaID;
        }

        public bool RegistroDuplicado(string sNombre)
        {
            bool isExiste = false;
            List<DocumentosCargasPlantillas> listaDatos = new List<DocumentosCargasPlantillas>();


            listaDatos = (from c in Context.DocumentosCargasPlantillas
                     where c.DocumentoCargaPlantilla == sNombre 
                     select c).ToList<DocumentosCargasPlantillas>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}