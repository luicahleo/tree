using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class EntidadesTiposController : GeneralBaseController<EntidadesTipos, TreeCoreContext>
    {
        public EntidadesTiposController()
            : base()
        { }


        public List<EntidadesTipos> GetAllEntidadesTipos()
        {
            // Local variables
            List<EntidadesTipos> lista = null;
            try
            {
                lista = (from c in Context.EntidadesTipos select c).ToList<EntidadesTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }




        public bool RegistroDuplicado(EntidadesTipos oEntidad)
        {
            bool isExiste = false;
            List<EntidadesTipos> datos = new List<EntidadesTipos>();


            datos = (from c in Context.EntidadesTipos where (c.EntidadTipo == oEntidad.EntidadTipo && c.ClienteID == oEntidad.ClienteID && c.EntidadTipoID != oEntidad.EntidadTipoID) select c).ToList<EntidadesTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<EntidadesTipos> GetEntidadesTiposByCliente(long clienteID)
        {
            List<EntidadesTipos> datos = new List<EntidadesTipos>();

            datos = (from c in Context.EntidadesTipos where (c.ClienteID == clienteID) select c).ToList<EntidadesTipos>();

            return datos;
        }

        public EntidadesTipos RegistroDefecto(long clienteID)
        {
            EntidadesTipos dato = new EntidadesTipos();
            EntidadesTiposController cController = new EntidadesTiposController();
           
            dato = cController.GetItem("Defecto == true && ClienteID == " + clienteID);
            return dato;
        }

          


        public long GetTipoByNombreAll(string Nombre)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.EntidadesTipos where c.EntidadTipo.Equals(Nombre) select c.EntidadTipoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;

            }
            return tipoID;
        }

        /*
         Nuevas funciones
         */

        public InfoResponse Add(EntidadesTipos oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
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
                    oResponse =  AddEntity(oEntidad);
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

        public InfoResponse Update(EntidadesTipos oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
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
                    oResponse =  UpdateEntity(oEntidad);
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

        public InfoResponse Delete(EntidadesTipos oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (oEntidad.Defecto)
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.jsPorDefecto,
                        Data = null
                    };
                }
                else
                {
                    oResponse = DeleteEntity(oEntidad);
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


    }
}