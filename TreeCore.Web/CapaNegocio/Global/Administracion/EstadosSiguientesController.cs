using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class EstadosSiguientesController : GeneralBaseController<CoreEstadosSiguientes, TreeCoreContext>, IGestionBasica<CoreEstadosSiguientes>
    {
        public EstadosSiguientesController()
               : base()
        { }

        public InfoResponse Add(CoreEstadosSiguientes oSiguiente)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oSiguiente))
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
                    oResponse = AddEntity(oSiguiente);
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

        public InfoResponse Update(CoreEstadosSiguientes oSiguiente)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oSiguiente))
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
                    oResponse = UpdateEntity(oSiguiente);
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

        public InfoResponse Delete(CoreEstadosSiguientes oSiguiente)
        {
            InfoResponse oResponse;

            try
            {
                if (oSiguiente.Defecto)
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
                    oResponse = DeleteEntity(oSiguiente);
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

        public InfoResponse SetDefecto(CoreEstados oEstado, CoreEstadosSiguientes oSiguiente)
        {
            InfoResponse oResponse;
            CoreEstadosSiguientes oDefault;

            try
            {
                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDefault = GetDefault(oEstado.CoreEstadoID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.CoreEstadoPosibleID != oSiguiente.CoreEstadoPosibleID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = !oDefault.Defecto;
                            oResponse = Update(oDefault);
                        }

                        oSiguiente = GetEstadoSiguiente(oEstado.CoreEstadoID, oSiguiente.CoreEstadoPosibleID);
                        oSiguiente.Defecto = true;
                        oResponse = Update(oSiguiente);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oSiguiente
                        };
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    //oSiguiente = GetEstadoSiguiente(oEstado.CoreEstadoID, oSiguiente.CoreEstadoPosibleID);
                    oSiguiente.Defecto = true;
                    oResponse = Update(oSiguiente);
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

        public bool RegistroDuplicado(CoreEstadosSiguientes oSiguiente)
        {
            bool isExiste = false;
            List<CoreEstadosSiguientes> datos;

            datos = (from c in Context.CoreEstadosSiguientes where (c.CoreEstadoID == oSiguiente.CoreEstadoID && c.CoreEstadoPosibleID == oSiguiente.CoreEstadoPosibleID && c.CoreEstadoSiguienteID != oSiguiente.CoreEstadoSiguienteID) select c).ToList<CoreEstadosSiguientes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public CoreEstadosSiguientes GetDefault(long lEstadoID)
        {
            CoreEstadosSiguientes oSiguiente;

            try
            {
                oSiguiente = (from c
                         in Context.CoreEstadosSiguientes
                          where c.Defecto &&
                                 c.CoreEstadoID == lEstadoID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oSiguiente = null;
            }

            return oSiguiente;
        }

        public List<CoreEstadosSiguientes> getEstadosSiguientesByEstadoID(long lEstadoID)
        {
            List<CoreEstadosSiguientes> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        //public bool RegistroDuplicado(long lEstadoID, long lEstadoPosibleID)
        //{
        //    bool isExiste = false;
        //    List<CoreEstadosSiguientes> datos = new List<CoreEstadosSiguientes>();

        //    datos = (from c in Context.CoreEstadosSiguientes where (c.CoreEstadoID == lEstadoID && c.CoreEstadoPosibleID == lEstadoPosibleID) select c).ToList<CoreEstadosSiguientes>();

        //    if (datos.Count > 0)
        //    {
        //        isExiste = true;
        //    }

        //    return isExiste;
        //}

        //public CoreEstadosSiguientes GetDefault(long lEstadoID)
        //{
        //    CoreEstadosSiguientes oDato;

        //    try
        //    {
        //        oDato = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID && c.Defecto select c).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        oDato = null;
        //    }

        //    return oDato;
        //}

        public CoreEstadosSiguientes GetEstadoSiguiente (long lEstadoID, long lEstadoPosibleID)
        {
            CoreEstadosSiguientes oDato;

            try
            {
                oDato = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID && c.CoreEstadoPosibleID == lEstadoPosibleID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public bool RegistroDefecto(long lEstadoID, long lEstadoSiguienteID)
        {
            CoreEstadosSiguientes oDato;
            bool bDefecto = false;

            oDato = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID && c.CoreEstadoSiguienteID == lEstadoSiguienteID select c).First();

            if (oDato.Defecto != false)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }

        public List<CoreEstados> GetByEstadoID(long lEstadoID)
        {
            List<long> listaEstadoSiguienteID;
            List<CoreEstados> lista = new List<CoreEstados>();

            try
            {
                listaEstadoSiguienteID = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID select c.CoreEstadoPosibleID).ToList();

                foreach (long lEstadoSiguienteID in listaEstadoSiguienteID)
                {
                    CoreEstados oDato = (from c in Context.CoreEstados where c.CoreEstadoID == lEstadoSiguienteID select c).First();

                    if (oDato != null)
                    {
                        lista.Add(oDato);
                    }
                }
                
            }
            catch (Exception)
            {
                lista = null;
            }
            return lista;
        }
    }
}