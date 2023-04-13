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
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosAtributosConfiguracionesController : GeneralBaseController<CoreProductCatalogServiciosAtributosConfiguraciones, TreeCoreContext>, IGestionBasica<CoreProductCatalogServiciosAtributosConfiguraciones>
    {
        public CoreProductCatalogServiciosAtributosConfiguracionesController()
            : base()
        { }

        public InfoResponse Add(CoreProductCatalogServiciosAtributosConfiguraciones oAtributo)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = AddEntity(oAtributo);
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

        public InfoResponse Update(CoreProductCatalogServiciosAtributosConfiguraciones oAtributo)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oAtributo);
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

        public InfoResponse Delete(CoreProductCatalogServiciosAtributosConfiguraciones oAtributo)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oAtributo);
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

        public InfoResponse AddAtributo(CoreAtributosConfiguraciones oAtrConfig, CoreProductCatalogServiciosTareas oTask)
        {
            CoreAtributosConfiguracionesController cController = new CoreAtributosConfiguracionesController();
            cController.SetDataContext(this.Context);
            InfoResponse oResponse;

            try
            {
                oResponse = cController.Add(oAtrConfig);

                if (oResponse.Result)
                {
                    CoreProductCatalogServiciosAtributosConfiguraciones oConfig = new CoreProductCatalogServiciosAtributosConfiguraciones
                    {
                        CoreProductCatalogServicioTareaID = oTask.CoreProductCatalogServicioTareaID,
                        CoreAtributosConfiguraciones = oAtrConfig,
                        Orden = 1
                    };

                    oResponse = Add(oConfig);
                }
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

        public List<CoreProductCatalogServiciosAtributosConfiguraciones> getListaByTareaID (long lTareaID)
        {
            List<CoreProductCatalogServiciosAtributosConfiguraciones> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreProductCatalogServiciosAtributosConfiguraciones where c.CoreProductCatalogServicioTareaID == lTareaID select c).ToList();
            }
            catch(Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        public CoreProductCatalogServiciosAtributosConfiguraciones getDataByAtributoID (long lAtributoID)
        {
            CoreProductCatalogServiciosAtributosConfiguraciones oDato;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServiciosAtributosConfiguraciones where c.CoreAtributoConfiguracionID == lAtributoID select c).First();
            }
            catch (Exception)
            {
                oDato = null;
            }

            return oDato;
        }

        public void ActualizarOrdenAtributo(long atrID, long NewOrden)
        {
            CoreProductCatalogServiciosAtributosConfiguraciones oAtrEjecutor;

            try
            {
                oAtrEjecutor = (from c in Context.CoreProductCatalogServiciosAtributosConfiguraciones where c.CoreAtributoConfiguracionID == atrID select c).First();

                if (oAtrEjecutor != null)
                {
                    oAtrEjecutor.Orden = (int)NewOrden;
                    Context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}