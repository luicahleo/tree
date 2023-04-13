using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ClientesController : GeneralBaseController<Clientes, TreeCoreContext>, IBasica<Clientes>
    {
        public ClientesController()
            : base()
        { }

        public bool RegistroVinculado(long ClienteID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Cliente, long clienteID)
        {
            bool isExiste = false;
            List<Clientes> datos = new List<Clientes>();

            datos = (from c in Context.Clientes where (c.Cliente == Cliente) select c).ToList<Clientes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
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
    }
}