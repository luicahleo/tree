using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class ContactosGlobalesEntidadesController : GeneralBaseController<ContactosGlobalesEntidades, TreeCoreContext>, IGestionBasica<ContactosGlobalesEntidades>
    {
        public ContactosGlobalesEntidadesController()
            : base()
        { }

        public InfoResponse Add(ContactosGlobalesEntidades oContacto)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oContacto))
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
                    oResponse = AddEntity(oContacto);
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

        public InfoResponse AddRelacion(long lEntidadID, long lContactoGlobalID)
        {
            List<ContactosGlobalesEntidades> listaDatos;
            ContactosGlobalesEntidades oDato = new ContactosGlobalesEntidades();
            bool bAdd = false;
            InfoResponse oResponse = new InfoResponse();

            try
            {
                listaDatos = (from c in Context.ContactosGlobalesEntidades where c.EntidadID == lEntidadID && c.ContactoGlobalID == lContactoGlobalID select c).ToList();
                if (listaDatos != null && listaDatos.Count > 0)
                {
                    bAdd = true;
                }
                else
                {
                    oDato.EntidadID = lEntidadID;
                    oDato.ContactoGlobalID = lContactoGlobalID;
                    oResponse = this.Add(oDato);
                    bAdd = (oDato != null);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bAdd = false;
            }

            return oResponse;

        }

        public InfoResponse Update(ContactosGlobalesEntidades oContacto)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oContacto))
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
                    oResponse = UpdateEntity(oContacto);
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

        public InfoResponse Delete(ContactosGlobalesEntidades oContacto)
        {
            InfoResponse oResponse;
            try
            {
                oResponse = DeleteEntity(oContacto);
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

        public bool RegistroDuplicado(ContactosGlobalesEntidades oContacto)
        {
            bool isExiste = false;
            List<ContactosGlobalesEntidades> datos;

            datos = (from c in Context.ContactosGlobalesEntidades where c.ContactoGlobalEntidadID != oContacto.ContactoGlobalEntidadID select c).ToList<ContactosGlobalesEntidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<Vw_ContactosGlobalesEntidades> GetContactosGlobalesByEntidad(long entidadID, long clienteID)
        {
            List<Vw_ContactosGlobalesEntidades> datos = new List<Vw_ContactosGlobalesEntidades>();

            datos = (from c in Context.Vw_ContactosGlobalesEntidades where (c.EntidadID == entidadID) && (c.ClienteID == clienteID) select c).ToList<Vw_ContactosGlobalesEntidades>();

            return datos;
        }

        public ContactosGlobalesEntidades GetContactoGlobalEntidadByIDs(long lEntidadID, long lContactoGlobalID)
        {
            ContactosGlobalesEntidades oDato;

            try
            {
                oDato = (from c in Context.ContactosGlobalesEntidades where c.ContactoGlobalID == lContactoGlobalID && c.EntidadID == lEntidadID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;

        }

        public ContactosGlobalesEntidades GetContactosByID(long lEntidadID, long lContactoGlobalID)
        {
            ContactosGlobalesEntidades oDato;

            try
            {
                oDato = (from c in Context.ContactosGlobalesEntidades where c.ContactoGlobalID == lContactoGlobalID && c.EntidadID == lEntidadID select c).First();
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