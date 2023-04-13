using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreEstadosTareasController : GeneralBaseController<CoreEstadosTareas, TreeCoreContext>, IGestionBasica<CoreEstadosTareas>
    {
        public CoreEstadosTareasController()
            : base()
        { }

        public InfoResponse Add(CoreEstadosTareas oTarea)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = AddEntity(oTarea);
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

        public InfoResponse Update(CoreEstadosTareas oTarea)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oTarea))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = UpdateEntity(oTarea);
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

        public InfoResponse Delete(CoreEstadosTareas oTarea)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oTarea);
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

        public InfoResponse AddEstadoTarea(long lEstadoID, long lWorkflowID, long lAccionID, string sInfo, bool bCheck, string sDescripcion)
        {
            InfoResponse oResponse = new InfoResponse();
            long lInformacionID = 0;
            long lWorkflowInformacionID = 0;

            CoreTiposInformacionesAccionesController cTiposAcciones = new CoreTiposInformacionesAccionesController();
            CoreWorkflowsInformacionesController cWorkflowInfo = new CoreWorkflowsInformacionesController();
            CoreCustomFieldsController cCustoms = new CoreCustomFieldsController();
            CoreTiposInformacionesController cTiposController = new CoreTiposInformacionesController();
            CoreAtributosConfiguracionesController cAtributosController = new CoreAtributosConfiguracionesController();
            cTiposAcciones.SetDataContext(this.Context);
            cWorkflowInfo.SetDataContext(this.Context);
            cTiposController.SetDataContext(this.Context);
            cCustoms.SetDataContext(this.Context);
            cAtributosController.SetDataContext(this.Context);

            try
            {
                CoreAtributosConfiguraciones oAtr = cAtributosController.GetAtributoByCodigo(sInfo);

                if (oAtr == null)
                {
                    CoreTiposInformaciones oTipo = cTiposController.GetInformacion(sInfo);

                    if (oTipo != null)
                    {
                        lInformacionID = oTipo.CoreTipoInformacionID;
                        CoreWorkflowsInformaciones oWorkInfo = cWorkflowInfo.getObjetoByID(lInformacionID, lWorkflowID);

                        if (oWorkInfo != null)
                        {
                            lWorkflowInformacionID = oWorkInfo.CoreWorkflowInformacionID;
                        }
                    }
                }
                else
                {
                    // CustomField esta asociado a CoreTipoInformacionID = 1
                    lInformacionID = 1;
                    CoreCustomFields oCustom = cCustoms.getCustomByAtributo(oAtr.CoreAtributoConfiguracionID);
                    
                    if (oCustom != null)
                    {
                        CoreWorkflowsInformaciones oWorkInfo = cWorkflowInfo.getObjetoByIDs(lInformacionID, lWorkflowID, oCustom.CoreCustomFieldID);

                        if (oWorkInfo != null)
                        {
                            lWorkflowInformacionID = oWorkInfo.CoreWorkflowInformacionID;
                        }
                    }
                }

                if (lInformacionID != 0 && lAccionID != 0 && lWorkflowInformacionID != 0)
                {
                    CoreTiposInformacionesAcciones oTipoAccion = cTiposAcciones.getIDByParametros(lAccionID, lInformacionID);

                    if (oTipoAccion != null)
                    {
                        CoreEstadosTareas oDato = new CoreEstadosTareas()
                        {
                            CoreEstadoID = lEstadoID,
                            CoreWorkflowInformacionID = lWorkflowInformacionID,
                            Obligatorio = bCheck,
                            CoreTipoInformacionAccionID = oTipoAccion.CoreTipoInformacionAccionID,
                            Descripcion = sDescripcion
                        };

                        oResponse = Add(oDato);
                    }
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

        public List<CoreEstadosTareas> GetByEstado(long estadoID)
        {
            List<CoreEstadosTareas> estadosTareas;

            try
            {
                estadosTareas = (from c in Context.CoreEstadosTareas
                                 where c.CoreEstadoID == estadoID
                                 select c).ToList();
            }
            catch (Exception ex)
            {
                estadosTareas = null;
                log.Error(ex.Message);
            }

            return estadosTareas;
        }

        public List<long> GetIDsByEstado(long estadoID)
        {
            List<long> estadosTareas;

            try
            {
                estadosTareas = (from c in Context.CoreEstadosTareas
                                 where c.CoreEstadoID == estadoID
                                 select c.CoreTipoInformacionAccionID).ToList();
            }
            catch (Exception ex)
            {
                estadosTareas = null;
                log.Error(ex.Message);
            }

            return estadosTareas;
        }

        public bool RegistroDuplicado(CoreEstadosTareas oTarea)
        {
            bool isExiste = false;
            List<CoreEstadosTareas> datos;

            datos = (from c in Context.CoreEstadosTareas where (c.CoreWorkflowInformacionID == oTarea.CoreWorkflowInformacionID && c.CoreTipoInformacionAccionID != oTarea.CoreTipoInformacionAccionID) select c).ToList<CoreEstadosTareas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}