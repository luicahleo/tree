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
    public class ContactosGlobalesEmplazamientosController : GeneralBaseController<ContactosGlobalesEmplazamientos, TreeCoreContext>, IGestionBasica<ContactosGlobalesEmplazamientos>
    {
        public ContactosGlobalesEmplazamientosController()
            : base()
        { }

        public InfoResponse Add(ContactosGlobalesEmplazamientos oContacto)
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

        public InfoResponse AddRelacion(long lEmplazamientoID, long lContactoGlobalID)
        {
            List<ContactosGlobalesEmplazamientos> listaDatos;
            ContactosGlobalesEmplazamientos oDato = new ContactosGlobalesEmplazamientos();
            bool bAdd = false;
            InfoResponse oResponse = new InfoResponse();

            try
            {
                listaDatos = (from c in Context.ContactosGlobalesEmplazamientos where c.EmplazamientoID == lEmplazamientoID && c.ContactoGlobalID == lContactoGlobalID select c).ToList();
                if (listaDatos != null && listaDatos.Count > 0)
                {
                    bAdd = true;
                }
                else
                {
                    oDato.EmplazamientoID = lEmplazamientoID;
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

        public InfoResponse Update(ContactosGlobalesEmplazamientos oContacto)
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

        public InfoResponse Delete(ContactosGlobalesEmplazamientos oContacto)
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

        public bool RegistroDuplicado(ContactosGlobalesEmplazamientos oContacto)
        {
            bool isExiste = false;
            List<ContactosGlobalesEmplazamientos> datos;

            datos = (from c in Context.ContactosGlobalesEmplazamientos where (c.ContactoGlobalID != oContacto.ContactoGlobalID) select c).ToList<ContactosGlobalesEmplazamientos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<Vw_ContactosGlobalesEmplazamientos> GetContactosByEmplazamientoID()
        {
            List<Vw_ContactosGlobalesEmplazamientos> listaContactos;

            try
            {
                listaContactos = (from c in Context.Vw_ContactosGlobalesEmplazamientos where c.Activo == true orderby c.EmplazamientoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        public ContactosGlobalesEmplazamientos GetContactosByID(long lEmplazamientoID, long lContactoGlobalID)
        {
            ContactosGlobalesEmplazamientos oDato;

            try
            {
                oDato = (from c in Context.ContactosGlobalesEmplazamientos where c.ContactoGlobalID == lContactoGlobalID && c.EmplazamientoID == lEmplazamientoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;

        }

        public Vw_ContactosGlobalesEmplazamientos GetVistaByID(long lContactoGlobalEmplazamientoID)
        {
            Vw_ContactosGlobalesEmplazamientos oDato;

            try
            {
                oDato = (from c in Context.Vw_ContactosGlobalesEmplazamientos where c.ContactoGlobalEmplazamientoID == lContactoGlobalEmplazamientoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;

        }

        public List<Vw_ContactosGlobalesEmplazamientos> GetListaContactosByID(long lEmplazamientoID)
        {
            List<Vw_ContactosGlobalesEmplazamientos> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ContactosGlobalesEmplazamientos where c.EmplazamientoID == lEmplazamientoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;

        }
    }
}