using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class SAPGruposTesoreriasController : GeneralBaseController<SAPGruposTesorerias, TreeCoreContext>
    {
        public SAPGruposTesoreriasController()
            : base()
        { }

        public bool RegistroVinculado(long SAPGrupoTesoreriaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(SAPGruposTesorerias oDato)
        {
            bool isExiste = false;
            List<SAPGruposTesorerias> datos = new List<SAPGruposTesorerias>();


            datos = (from c in Context.SAPGruposTesorerias where (c.SAPGrupoTesoreria == oDato.SAPGrupoTesoreria && c.ClienteID == oDato.ClienteID && c.SAPGrupoTesoreriaID != oDato.SAPGrupoTesoreriaID ) select c).ToList<SAPGruposTesorerias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SAPGrupoTesoreriaID)
        {
            SAPGruposTesorerias dato = new SAPGruposTesorerias();
            SAPGruposTesoreriasController cController = new SAPGruposTesoreriasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SAPGrupoTesoreriaID == " + SAPGrupoTesoreriaID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        public SAPGruposTesorerias GetDefault(long lClienteID)
        {
            SAPGruposTesorerias oGrupo;

            try
            {
                oGrupo = (from c in Context.SAPGruposTesorerias where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGrupo = null;
            }

            return oGrupo;
        }
        public List<SAPGruposTesorerias> GetSAPGruposTesoreriasByCliente(long clienteID)
        {
            List<SAPGruposTesorerias> datos = new List<SAPGruposTesorerias>();

            datos = (from c in Context.SAPGruposTesorerias where (c.ClienteID == clienteID) orderby c.Descripcion select c).ToList<SAPGruposTesorerias>();

            return datos;
        }

        public SAPGruposTesorerias GetGrupoTesoreriaByNombre(string sNombre)
        {
            List<SAPGruposTesorerias> lista = null;
            SAPGruposTesorerias dato = null;

            try
            {

                lista = (from c in Context.SAPGruposTesorerias where c.SAPGrupoTesoreria == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return dato;
            }

            return dato;
        }

        /*
       Nuevas funciones
       */
        public InfoResponse SetDefecto(SAPGruposTesorerias oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            SAPGruposTesorerias oDefault;
            try
            {
                oDefault = GetDefault(Convert.ToInt64(oEntidad.ClienteID));
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.SAPGrupoTesoreriaID != oEntidad.SAPGrupoTesoreriaID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }
                        oEntidad.Defecto = true;
                        oEntidad.Activo = true;
                        oResponse = Update(oEntidad);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oEntidad
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oEntidad.Defecto = true;
                    oEntidad.Activo = true;
                    oResponse = Update(oEntidad);
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
        public InfoResponse Add(SAPGruposTesorerias oEntidad)
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
                    oResponse = AddEntity(oEntidad);
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

        public InfoResponse Update(SAPGruposTesorerias oEntidad)
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
                    oResponse = UpdateEntity(oEntidad);
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

        public InfoResponse Delete(SAPGruposTesorerias oEntidad)
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