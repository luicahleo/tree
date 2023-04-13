using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using Ext.Net;
using System.IO;
using Tree.Linq.GenericExtensions;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using TreeCore.Clases;
using System.Globalization;
using Newtonsoft.Json;

namespace CapaNegocio
{
    public class CoreProductCatalogPacksAsignadosController : GeneralBaseController<CoreProductCatalogPacksAsignados, TreeCoreContext>, IGestionBasica<CoreProductCatalogPacksAsignados>
    {
        public CoreProductCatalogPacksAsignadosController()
            : base()
        { }

        public InfoResponse Add(CoreProductCatalogPacksAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {

                oResponse = AddEntity(oEntidad);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Update(CoreProductCatalogPacksAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {

                oResponse = UpdateEntity(oEntidad);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Delete(CoreProductCatalogPacksAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {

                oResponse = DeleteEntity(oEntidad);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse AnadirPacks(CoreProductCatalogPacksAsignados pack, long lCatalogoID, long lPackID, bool bCheck, long usuarioID, bool agregar)
        {
            InfoResponse oResponse = new InfoResponse();
            CoreProductCatalogServiciosAsignadosController cProduct = new CoreProductCatalogServiciosAsignadosController();
            CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();
            CoreProductCatalogServiciosPacksAsignadosController cServPacks = new CoreProductCatalogServiciosPacksAsignadosController();
            bool bExiste = false;
            bool bServExiste = false;
            string sNombrePack = "";


            cCatalogos.SetDataContext(this.Context);
            cProduct.SetDataContext(this.Context);
            cServPacks.SetDataContext(this.Context);

            CoreProductCatalogs oCatalogo = cCatalogos.GetItem(lCatalogoID);
            oCatalogo.FechaUltimaModificacion = DateTime.Now;
            oCatalogo.UsuarioModificadorID = usuarioID;
            cCatalogos.Add(oCatalogo);

            if (!bCheck)
            {
                List<long> listaServicios = cServPacks.getAllServiciosByID(lPackID);
                List<long> listaServAsign = cProduct.GetAllServiciosByCatalogoID(lCatalogoID);
                List<long> listaIDs = cProduct.getItemsFiltradosByCatalogoID(lCatalogoID);
                foreach (long lServicioID in listaServicios)
                {
                    if (listaIDs.Contains(lServicioID))
                    {
                        bExiste = true;
                        sNombrePack = cServPacks.getItemByServicioID(lServicioID).Nombre;
                    }
                    else if (listaServAsign.Contains(lServicioID))
                    {
                        bServExiste = true;
                    }
                }

                if (bExiste && agregar)
                {
                    oResponse.Description = "This service is already assigned to the pack: " + sNombrePack;
                    oResponse.Result = false;
                }
                else if (bServExiste)
                {
                    oResponse.Description = "Some service is already assigned to this catalog";
                    oResponse.Result = false;
                }
                else
                {
                    if (agregar)
                    {
                        oResponse = Add(pack);

                    }
                    else
                    {
                        oResponse = Update(pack);
                    }
                }
            }
            else
            {
                oResponse = Delete(pack);
                
            }

            if (oResponse.Result)
            {
                oResponse = SubmitChanges();

            }
            else
            {
                DiscardChanges();
            }

            return oResponse;
        }
        public CoreProductCatalogPacksAsignados getDatoByServicioID (long lServicioID)
        {
            CoreProductCatalogPacksAsignados oDato = new CoreProductCatalogPacksAsignados();
            long lPackID = 0;

            try
            {
                lPackID = (from c in Context.CoreProductCatalogServiciosPacksAsignados where c.CoreProductCatalogServicioID == lServicioID select c.CoreProductCatalogPackID).First();

                if (lPackID != 0)
                {
                    oDato = (from c in Context.CoreProductCatalogPacksAsignados where c.CoreProductCatalogPackID == lPackID select c).First();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public long getValorByValoresID (long lPackID, long lCatalogoID)
        {
            long lPackAsignID;

            try
            {
                lPackAsignID = (from c in Context.CoreProductCatalogPacksAsignados where c.CoreProductCatalogPackID == lPackID && c.CoreProductCatalogID == lCatalogoID select c.CoreProductCatalogPackAsignadoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lPackAsignID = 0;
            }

            return lPackAsignID;
        }
        public CoreProductCatalogPacksAsignados getValorByValores (long lPackID, long lCatalogoID)
        {
            CoreProductCatalogPacksAsignados lPackAsign;

            try
            {
                lPackAsign = (from c in Context.CoreProductCatalogPacksAsignados where c.CoreProductCatalogPackID == lPackID && c.CoreProductCatalogID == lCatalogoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lPackAsign = null;
            }

            return lPackAsign;
        }

        public List<CoreProductCatalogPacksAsignados> getPackByCatalogoID (long lCatalogoID)
        {
            List<CoreProductCatalogPacksAsignados> listaDato = new List<CoreProductCatalogPacksAsignados>();

            try
            {
                listaDato = (from c in Context.CoreProductCatalogPacksAsignados where c.CoreProductCatalogID == lCatalogoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDato = null;
            }

            return listaDato;
        }

    }

}