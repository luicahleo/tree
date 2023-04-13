using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class ContactosTiposController : GeneralBaseController<ContactosTipos, TreeCoreContext>
    {
        public ContactosTiposController()
            : base()
        { }

        public List<ContactosTipos> GetAllContactosTipos()
        {
            // Local variables
            List<ContactosTipos> lista = null;
            try
            {
                lista = (from c in Context.ContactosTipos select c).ToList<ContactosTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<ContactosTipos> GetAllTipoDatos()
        {
            List<ContactosTipos> lista = new List<ContactosTipos>();
            lista = (from c in Context.ContactosTipos orderby c.ContactoTipo ascending select c).ToList();

            return lista;
        }

        public ContactosTipos GetDefault()
        {
            ContactosTipos oContacto;

            try
            {
                oContacto = (from c in Context.ContactosTipos where c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContacto = null;
            }

            return oContacto;
        }

        public ContactosTipos GetTipobyNombre(string sTipo, long lClienteID)
        {
            ContactosTipos oContacto;

            try
            {
                oContacto = (from c in Context.ContactosTipos where c.ClienteID == lClienteID && c.ContactoTipo == sTipo select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContacto = null;
            }

            return oContacto;
        }

        public ContactosTipos GetDefaultByClienteID(long clienteID)
        {
            ContactosTipos oContacto;

            try
            {
                oContacto = (from c in Context.ContactosTipos where c.Defecto && c.ClienteID==clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContacto = null;
            }

            return oContacto;
        }

        public List<ContactosTipos> GetActivos(long clienteID)
        {
            List<ContactosTipos> listaTipos;

            try
            {
                listaTipos = (from c in Context.ContactosTipos
                              where c.Activo &&
                                    c.ClienteID == clienteID
                              orderby c.ContactoTipo
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        public bool RegistroDuplicado(ContactosTipos ocontactosTipos)
        {
            bool isExiste = false;
            List<ContactosTipos> datos = new List<ContactosTipos>();


            datos = (from c in Context.ContactosTipos where (c.ContactoTipo == ocontactosTipos.ContactoTipo && c.ClienteID == ocontactosTipos.ClienteID && c.ContactoTipoID != ocontactosTipos.ContactoTipoID ) select c).ToList<ContactosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public String getTipoByID (long? lContactoTipoID)
        {
            String sDato;

            try
            {
                sDato = (from c in Context.ContactosTipos where c.ContactoTipoID == lContactoTipoID select c.ContactoTipo).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sDato = null;
            }

            return sDato;
        }

        public List<String> ListContactosTipos()
        {
            return (from c in Context.ContactosTipos select c.ContactoTipo).ToList();
        }

        /*
         Nuevas funciones
         */

        public InfoResponse ModificarActivar(ContactosTipos oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oEntidad.Activo = !oEntidad.Activo;
                oResponse = Update(oEntidad);
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

        public ContactosTipos GetDefault(long ClienteID)
        {
            ContactosTipos cContactosTipos;
            try
            {
                cContactosTipos = (from c
                         in Context.ContactosTipos
                                   where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                cContactosTipos = null;
            }
            return cContactosTipos;
        }

        public InfoResponse SetDefecto(ContactosTipos oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            ContactosTipos oDefault;
            try
            {
                oDefault = GetDefault(Convert.ToInt64(oEntidad.ClienteID));
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.ContactoTipoID != oEntidad.ContactoTipoID)
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

        public InfoResponse Add(ContactosTipos oEntidad)
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

        public InfoResponse Update(ContactosTipos oEntidad)
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

        public InfoResponse Delete(ContactosTipos oEntidad)
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