using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class ContactosGlobalesController : GeneralBaseController<ContactosGlobales, TreeCoreContext>, IGestionBasica<ContactosGlobales>
    {
        public ContactosGlobalesController()
            : base()
        { }

        public InfoResponse Add(ContactosGlobales oContacto)
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

        public InfoResponse Update(ContactosGlobales oContacto)
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

        public InfoResponse Delete(ContactosGlobales oContacto)
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

        public InfoResponse ModificarActivar(ContactosGlobales oContacto)
        {
            InfoResponse oResponse;

            try
            {
                oContacto.Activo = !oContacto.Activo;
                oResponse = Update(oContacto);
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

        public bool RegistroDuplicado(ContactosGlobales oContacto)
        {
            bool isExiste = false;
            List<ContactosGlobales> datos;

            datos = (from c in Context.ContactosGlobales where ((c.Telefono == oContacto.Telefono || c.Email == oContacto.Email) && c.ContactoGlobalID != oContacto.ContactoGlobalID) select c).ToList<ContactosGlobales>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse AddContactoEmplazamiento(ContactosGlobales oContacto, long lEmplazamientoID)
        {
            ContactosGlobalesEmplazamientosController cEmplazamientos = new ContactosGlobalesEmplazamientosController();
            cEmplazamientos.SetDataContext(this.Context);
            InfoResponse oResponse;

            try
            {
                oResponse = Add(oContacto);

                if (oResponse.Result)
                {
                    ContactosGlobalesEmplazamientos oDato = new ContactosGlobalesEmplazamientos();
                    oDato.ContactoGlobalID = oContacto.ContactoGlobalID;
                    oDato.EmplazamientoID = lEmplazamientoID;
                    cEmplazamientos.Add(oDato);
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

        public InfoResponse AddContactoEntidad(ContactosGlobales oContacto, long lEntidadID)
        {
            ContactosGlobalesEntidadesController cEntidades = new ContactosGlobalesEntidadesController();
            cEntidades.SetDataContext(this.Context);
            InfoResponse oResponse;

            try
            {
                oResponse = Add(oContacto);

                if (oResponse.Result)
                {
                    ContactosGlobalesEntidades oDato = new ContactosGlobalesEntidades();
                    oDato.ContactoGlobalID = oContacto.ContactoGlobalID;
                    oDato.ContactoGlobalEntidadID = lEntidadID;
                    cEntidades.Add(oDato);
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

        public bool ContactoDuplicadoID(string sTelefono, string sEmail, long sContacto)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where ((c.Telefono == sTelefono || c.Email == sEmail) && c.ContactoGlobalID != sContacto)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool DuplicadoID(string sTelefono, string sEmail)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Telefono == sTelefono || c.Email == sEmail)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool ContactoDuplicadoEmail(string sEmail, long sContacto)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Email == sEmail && c.ContactoGlobalID != sContacto)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool DuplicadoEmail(string sEmail)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where c.Email == sEmail select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public ContactosGlobales getContactoByID(long lContactoID)
        {
            ContactosGlobales oContacto;

            try
            {
                oContacto = (from c in Context.ContactosGlobales where c.ContactoGlobalID == lContactoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContacto = null;
            }

            return oContacto;
        }

        public bool tieneRegistrosAsociados(long lContactoGlobalID)
        {
            bool bTiene = false;
            int iRegistros = 0;

            iRegistros = (from c in Context.ContactosGlobalesEmplazamientos where c.ContactoGlobalID == lContactoGlobalID select c).Count();

            if (iRegistros > 0)
            {
                bTiene = true;
            }

            return bTiene;
        }

        public List<ContactosGlobales> getContactosByID(long lContactoID)
        {
            List<ContactosGlobales> listaContactos;

            try
            {
                listaContactos = (from c in Context.ContactosGlobales where c.ContactoGlobalID == lContactoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        public ContactosGlobales GetContactoByTelefonoEmail(string sTelefono, string sEmail)
        {

            List<ContactosGlobales> listaDatos;
            ContactosGlobales dato = null;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Telefono == sTelefono ||
                                 c.Email == sEmail)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                dato = listaDatos[0];
            }

            return dato;
        }

        public ContactosGlobales GetContactoByEmail(string sEmail, long lClienteID)
        {

            List<ContactosGlobales> listaDatos;
            ContactosGlobales dato = null;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Email == sEmail && c.ClienteID == lClienteID)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                dato = listaDatos[0];
            }

            return dato;
        }
    }
}