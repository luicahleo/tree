using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ClientesController : GeneralBaseController<Clientes, TreeCoreContext>, IBasica<Clientes>, IGestionBasica<Clientes>
    {
        public ClientesController()
            : base()
        { }

        public InfoResponse Add(Clientes oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (!RegistroDuplicado(oEntidad))
                {
                    oResponse = AddEntity(oEntidad);
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.LogRegistroExistente)
                    };
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

        public InfoResponse Update(Clientes oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (!RegistroDuplicado(oEntidad))
                {
                    oResponse = UpdateEntity(oEntidad);
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.LogActualizacionRealizada)
                    };
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

        public InfoResponse Delete(Clientes oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDefecto(oEntidad.ClienteID))
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.jsPorDefecto)
                    };
                }
                else
                {
                    oResponse = DeleteEntity(oEntidad);
                    if (oResponse.Result)
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    }
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

        public InfoResponse AgregarCliente(Clientes cliente)
        {
            InfoResponse infoResponse;

            try
            {
                infoResponse = Add(cliente);
                if (infoResponse.Result)
                {
                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                }
                else
                {
                    DiscardChanges();
                    infoResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.strMensajeGenerico)
                    };
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                infoResponse = new InfoResponse()
                {
                    Result = false,
                    Description = GetGlobalResource(Comun.strMensajeGenerico)
                };
            }

            return infoResponse;
        }

        public InfoResponse ActualizarCliente(Clientes cliente)
        {
            InfoResponse infoResponse;

            try
            {
                infoResponse = Update(cliente);
                if (infoResponse.Result)
                {
                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                }
                else
                {
                    DiscardChanges();
                    infoResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.strMensajeGenerico)
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                infoResponse = new InfoResponse()
                {
                    Result = false,
                    Description = GetGlobalResource(Comun.strMensajeGenerico)
                };
            }

            return infoResponse;
        }

        public bool RegistroVinculado(long ClienteID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(Clientes Cliente)
        {
            bool isExiste = false;
            List<Clientes> datos = new List<Clientes>();

            datos = (from c in Context.Clientes 
                     where 
                        c.Cliente == Cliente.Cliente && 
                        c.ClienteID != Cliente.ClienteID 
                     select c).ToList<Clientes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse AsignarPorDefecto(long clienteID, long hdCliID)
        {
            InfoResponse infoResponse;
            Clientes oDato;

            // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
            oDato = GetDefault(hdCliID);

            // SI HAY Y ES DISTINTO AL SELECCIONADO
            if (oDato != null)
            {
                if (oDato.Defecto)
                {
                    oDato.Defecto = !oDato.Defecto;
                    infoResponse = Update(oDato);
                }

                oDato = GetItem(clienteID);
                oDato.Defecto = true;
                oDato.Activo = true;
                infoResponse = Update(oDato);
            }
            // SI NO HAY ELEMENTO POR DEFECTO
            else
            {
                oDato = GetItem(clienteID);
                oDato.Defecto = true;
                oDato.Activo = true;
                infoResponse = Update(oDato);
            }

            return infoResponse;
        }

        public bool RegistroDefecto(long ClienteID)
        {
            Clientes dato = new Clientes();
            ClientesController cController = new ClientesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ClienteID == " + ClienteID.ToString());

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
        public long GetOperadorByClienteID(long ClienteID)
        {
            long lRes = 0;
            Clientes datos = new Clientes();
            datos = (from c in Context.Clientes where (c.ClienteID == ClienteID) select c).First();
            lRes = datos.OperadorID;
            return lRes;
        }

        public Clientes GetClienteByNombre(string sNombre)
        {

            Clientes datos = new Clientes();
            List<Clientes> lista = null;
            lista = (from c in Context.Clientes where (c.Cliente == sNombre) select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                datos = lista.ElementAt(0);
            }

            return datos;
        }

        public List<Clientes> GetActivos()
        {
            List<Clientes> clientes;
            try
            {
                clientes = (from c in Context.Clientes where c.Activo orderby c.Cliente select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                clientes = null;
            }
            return clientes;
        }

        public long? GetSingleClientID()
        {
            long? cliID = null;

            try
            {
                List<Clientes> lClientes = (from c in Context.Clientes
                             where c.Defecto
                             select c).ToList();

                if(lClientes != null && lClientes.Count == 1)
                {
                    cliID = lClientes[0].ClienteID;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                cliID = null;
            }

            return cliID;
        }

        public Clientes GetDefault(long lClienteID)
        {
            Clientes oClientes;

            try
            {
                oClientes = (from c
                             in Context.Clientes
                             where c.Defecto
                             select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oClientes = null;
            }

            return oClientes;
        }

        public bool RegistroDuplicado(string nombre, long clienteID)
        {
            Clientes cliente = GetItem(clienteID);
            cliente.Cliente = nombre;
            return RegistroDuplicado(cliente);
        }
    }
}