using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreFormulasController : GeneralBaseController<CoreFormulas, TreeCoreContext>, IGestionBasica<CoreFormulas>
    {
        public CoreFormulasController()
            : base()
        { }


        public bool RegistroDuplicado(string lNombre)
        {
            bool isExiste = false;
            List<CoreFormulas> datos = new List<CoreFormulas>();


            datos = (from c in Context.CoreFormulas where (c.Nombre == lNombre ) select c).ToList<CoreFormulas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoCodigo(string lCodigo)
        {
            bool isExiste = false;
            List<CoreFormulas> datos = new List<CoreFormulas>();


            datos = (from c in Context.CoreFormulas where (c.Codigo == lCodigo) select c).ToList<CoreFormulas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse Add(CoreFormulas oCatalogo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oCatalogo))
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
                    oResponse = AddEntity(oCatalogo);
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

        public InfoResponse Update(CoreFormulas oCatalogo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oCatalogo))
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
                    oResponse = UpdateEntity(oCatalogo);
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

        public InfoResponse Delete(CoreFormulas oCatalogo)
        {
            InfoResponse oResponse;
            try
            {

                oResponse = DeleteEntity(oCatalogo);

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

        public bool RegistroDuplicado(CoreFormulas oCatalogo)
        {
            bool isExiste = false;
            List<CoreFormulas> datos;

            datos = (from c in Context.CoreFormulas where (c.Codigo == oCatalogo.Codigo && c.ClienteID == oCatalogo.ClienteID && c.CoreFormulaID != oCatalogo.CoreFormulaID) select c).ToList<CoreFormulas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public CoreProductCatalogServiciosTipos GetDefault()
        {
            CoreProductCatalogServiciosTipos oDato;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServiciosTipos where c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

    }
}