using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using System.IO;
using TreeCore;

namespace CapaNegocio
{
    public class OperadoresController : GeneralBaseController<Operadores, TreeCoreContext>, IBasica<Operadores>
    {
        public OperadoresController()
            : base()
        { }

        public bool RegistroVinculado(long OperadorID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveOperador(string operador, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.Operadores
                          where c.Operador == operador &&
                                c.ClienteID == clienteID &&
                                c.Activo
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }
            return existe;
        }

        public Operadores GetActivoOperador(string operador, long clienteID)
        {
            Operadores result;

            try
            {
                result = (from c in Context.Operadores
                          where c.Operador == operador &&
                                c.ClienteID == clienteID &&
                                c.Activo
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }
            return result;
        }

        public bool RegistroDuplicado(string Operador, long clienteID)
        {
            bool isExiste = false;
            List<Operadores> datos = new List<Operadores>();


            datos = (from c in Context.Operadores where (c.Operador == Operador && c.ClienteID == clienteID) select c).ToList<Operadores>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long OperadorID)
        {
            Operadores dato = new Operadores();
            OperadoresController cController = new OperadoresController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && OperadorID == " + OperadorID.ToString());

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

        public List<Operadores> GetOperadoresActivos(long ClienteID)
        {
            List<Operadores> lista = null;

            try
            {
                lista = (from c in Context.Operadores where c.Activo && c.ClienteID == ClienteID orderby c.Operador select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<Operadores>();
            }

            return lista;
        }

        public List<string> GetOperadoresActivosNombre(long ClienteID)
        {
            List<string> lista = null;

            try
            {
                lista = (from c in Context.Entidades where c.Activo && c.ClienteID == ClienteID && c.OperadorID.HasValue orderby c.Nombre select c.Nombre).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<string>();
            }

            return lista;
        }

        public long GetOperadorByNombre(string Nombre)
        {

            long operadorID = 0;
            try
            {

                operadorID = (from c in Context.Operadores where c.Operador.Equals(Nombre) select c.OperadorID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                operadorID = -1;

            }
            return operadorID;
        }

        public bool HasOperadorCodigoConexion(string codigoEntidad, string codigoConexion)
        {
            bool existe;
            try
            {
                existe = (from ent in Context.Entidades
                          join op in Context.Operadores on ent.OperadorID equals op.OperadorID
                          where ent.Codigo.Equals(codigoEntidad) &&
                                codigoConexion.Equals(op.CodigoConexion)
                          select op).Count() > 0;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }
            return existe;
        }

        public bool EsClienteOperador(string codigoEntidad, long clienteID)
        {
            bool EsClienteOperador = false;

            try
            {
                EsClienteOperador = (from ope in Context.Operadores
                                     join c in Context.Clientes on ope.OperadorID equals c.OperadorID
                                     join ent in Context.Entidades on ope.OperadorID equals ent.OperadorID
                                     where c.ClienteID == clienteID &&
                                             ent.Codigo == codigoEntidad
                                     select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                EsClienteOperador = false;
            }

            return EsClienteOperador;
        }

        public List<Vw_EntidadesOperadores> getEntidadesOperadores(long clienteID)
        {
            List<Vw_EntidadesOperadores> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_EntidadesOperadores where c.ClienteID == clienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
              

        public string getEntidadesOperadoresConIcono(long clienteID)
        {
            DocumentosController cDocumentos = new DocumentosController();
            List<Vw_EntidadesOperadores> listaDatos;
            string operadoresIcono = "";
            bool tieneIcono = false;

            try
            {
                listaDatos = (from c in Context.Vw_EntidadesOperadores where c.ClienteID == clienteID select c).ToList();

                foreach (Vw_EntidadesOperadores operador in listaDatos)
                {
                    string tempPath = TreeCore.DirectoryMapping.GetIconoOperadorTempDirectory();
                    tempPath = Path.Combine(tempPath, cDocumentos.getFileNameIconoOperador((long) operador.OperadorID));

                    if (!File.Exists(tempPath))
                    {
                        string originalPath = TreeCore.DirectoryMapping.GetIconoOperadorDirectory();
                        originalPath = Path.Combine(originalPath, cDocumentos.getFileNameIconoOperador((long)operador.OperadorID));

                        if (File.Exists(originalPath))
                        {
                            File.Copy(originalPath, tempPath);
                            tieneIcono = true;
                        }
                        else
                        {
                            tieneIcono = false;
                            //path = "/ima/mapicons/ico-noOperator-map.svg";
                        }
                    }
                    else
                    {
                        tieneIcono = true;
                    }

                    if(tieneIcono)
                    {
                        if(operadoresIcono == "")
                        {
                            operadoresIcono = operador.OperadorID.ToString();
                        }
                        else
                        {
                            operadoresIcono += ", " + operador.OperadorID;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return operadoresIcono;
        }

        public bool eliminaOperador(long operadorID)
        {
            bool existe = true;
            Operadores dato;
            dato = (from c in Context.Operadores where c.OperadorID == operadorID select c).First();
            try
            {
                Context.Operadores.DeleteOnSubmit(dato);
                Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }
            return existe;
        }

        public Vw_EntidadesOperadores getOperadorUsuario(long lUsuarioID)
        {
            Vw_EntidadesOperadores oDato = null;
            try
            {
                UsuariosController cUsuarios = new UsuariosController();
                Usuarios user = cUsuarios.GetItem(lUsuarioID);
             /*   if (user.Entidades != null && user.Entidades.OperadorID != null)
                {
                    oDato = (from c in Context.Vw_EntidadesOperadores where c.EntidadID == user.EntidadID select c).First();
                }
                else
                {
                    oDato = null;
                }
             */
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public Vw_EntidadesOperadores getOperadorCliente(long lClienteID)
        {
            Vw_EntidadesOperadores oDato;
            try
            {
                ClientesController cClientes = new ClientesController();
                Clientes cli = cClientes.GetItem(lClienteID);
                oDato = (from c in Context.Vw_EntidadesOperadores where c.OperadorID == cli.OperadorID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public List<Operadores> GetAllOperadores(long clienteID) {
            List<Operadores> ops;

            try
            {
                ops = (from c in Context.Operadores 
                       where c.ClienteID == clienteID 
                       select c).ToList();
            }
            catch(Exception ex)
            {
                ops = null;
                log.Error(ex.Message);
            }
            return ops;
        }
    }
}