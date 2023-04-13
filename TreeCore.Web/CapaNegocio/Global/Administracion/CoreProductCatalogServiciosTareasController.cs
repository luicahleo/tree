using System;
using System.Data;
using System.Linq;
using TreeCore.Data;
using Tree.Linq.GenericExtensions;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosTareasController : GeneralBaseController<CoreProductCatalogServiciosTareas, TreeCoreContext>, IGestionBasica<CoreProductCatalogServiciosTareas>
    {
        public CoreProductCatalogServiciosTareasController()
            : base()
        { }

        public InfoResponse Add(CoreProductCatalogServiciosTareas oTarea)
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

        public InfoResponse Update(CoreProductCatalogServiciosTareas oTarea)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oTarea);
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

        public InfoResponse Delete(CoreProductCatalogServiciosTareas oTarea)
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

        public CoreProductCatalogServiciosTareas getTareaByServicioID (long lServicioID)
        {
            CoreProductCatalogServiciosTareas oDato;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServiciosTareas where c.CoreProductCatalogServicioID == lServicioID select c).First();
            }
            catch(Exception ex)
            {
                oDato = null;
            }

            return oDato;
        }
    }
}