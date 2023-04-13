using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class DQSemaforosController : GeneralBaseController<DQSemaforos, TreeCoreContext>, IGestionBasica<DQSemaforos>
    {
        public DQSemaforosController()
             : base()
        { }

        public InfoResponse Add(DQSemaforos oSemaforo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oSemaforo))
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
                    oResponse = AddEntity(oSemaforo);
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

        public InfoResponse Update(DQSemaforos oSemaforo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oSemaforo))
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
                    oResponse = UpdateEntity(oSemaforo);
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

        public InfoResponse Delete(DQSemaforos oSemaforo)
        {
            InfoResponse oResponse;
            try
            {
                oResponse = DeleteEntity(oSemaforo);
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

        public InfoResponse ModificarActivar(DQSemaforos oSemaforo)
        {
            InfoResponse oResponse;
            try
            {
                oSemaforo.Activo = !oSemaforo.Activo;
                oResponse = Update(oSemaforo);
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

        public bool RegistroDuplicado(DQSemaforos oSemaforo)
        {
            bool isExiste = false;
            List<DQSemaforos> datos;

            datos = (from c in Context.DQSemaforos where (c.DQSemaforo == oSemaforo.DQSemaforo && c.DQSemaforoID != oSemaforo.DQSemaforoID) select c).ToList<DQSemaforos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<DQSemaforos> GetAllSemaforosActivos(bool bActivo)
        {
            List<DQSemaforos> lista = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.DQSemaforos where c.Activo == true select c).ToList();
                }
                else
                {
                    lista = (from c in Context.DQSemaforos select c).ToList();
                }
            }
            catch (Exception ex)
            {
                return lista;
                log.Error(ex.Message);
            }

            return lista;
        }

    }
}