using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EntidadesController : GeneralBaseController<Entidades, TreeCoreContext>
    {
        public EntidadesController()
            : base()
        { }


        public List<Entidades> GetAllEntidades()
        {
            // Local variables
            List<Entidades> lista = null;
            try
            {
                lista = (from c in Context.Entidades where c.Activo select c).ToList<Entidades>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        

        public List<Entidades> GetAllEntidadesByClienteID(long CliID)
        {
            // Local variables
            List<Entidades> lista = null;
            try
            {
                lista = (from c in Context.Entidades where c.Activo && c.ClienteID == CliID select c).ToList<Entidades>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<Entidades> GetEmpresasProveedorasYOperadoresByClienteID(long CliID)
        {
            // Local variables
            List<Entidades> lista = null;
            try
            {
                lista = (from c in Context.Entidades where c.Activo && c.ClienteID == CliID && (c.EmpresaProveedoraID !=null || c.OperadorID != null) select c).ToList<Entidades>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<Entidades> GetEmpresasProveedorasYOperadores()
        {
            // Local variables
            List<Entidades> lista = null;
            try
            {
                lista = (from c in Context.Entidades where c.Activo && (c.EmpresaProveedoraID !=null || c.OperadorID != null) select c).ToList<Entidades>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<Vw_Entidades> GetEntidadPorTipo(string tipoEntidad, long clienteID)
        {
            List<Vw_Entidades> lEntidades = null;
            try
            {
                switch (tipoEntidad)
                {
                    case "Owners":
                        lEntidades = (from c in Context.Vw_Entidades where c.PropietarioID != 0 && c.ClienteID == clienteID select c).ToList();
                        break;
                    case "Suppliers":
                        lEntidades = (from c in Context.Vw_Entidades where c.ProveedorID != 0 && c.ClienteID == clienteID select c).ToList();
                        break;
                    case "Companies":
                        lEntidades = (from c in Context.Vw_Entidades where c.EmpresaProveedoraID != 0 && c.ClienteID == clienteID select c).ToList();
                        break;
                    case "Operators":
                        lEntidades = (from c in Context.Vw_Entidades where c.OperadorID != 0 && c.ClienteID == clienteID select c).ToList();
                        break;
                    default:
                        lEntidades = (from c in Context.Vw_Entidades where c.ClienteID == clienteID select c).ToList();
                        break;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lEntidades = null;
            }


            return lEntidades;
        }

        public bool RegistroDuplicado(string nombre, long clienteID)
        {
            bool isExiste = false;
            List<Entidades> datos = new List<Entidades>();


            datos = (from c in Context.Entidades where (c.Nombre == nombre && c.ClienteID == clienteID) select c).ToList<Entidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool ComprobarEntidadCliente(long clienteID)
        {
            bool isExiste = false;
            List<Entidades> datos = new List<Entidades>();


            datos = (from c in Context.Entidades where (c.EntidadCliente && c.ClienteID == clienteID) select c).ToList<Entidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public Entidades getDefecto(long clienteID)
        {
            Entidades dato = new Entidades();


            dato = (from c in Context.Entidades where (c.EntidadCliente && c.ClienteID == clienteID) select c).First();

            

            return dato;
        }


        public long? GetOperadorIDyNombreEntidad(long lClienteID, string nombre)
        {
            List<Entidades> listaDatos;
            long? ID = 0;
            listaDatos = (from c in Context.Entidades where c.Nombre == nombre && c.Activo && c.ClienteID == lClienteID select c).ToList();

            if (listaDatos.Count == 1)
            {
                if (listaDatos.FirstOrDefault().OperadorID != null)
                {
                    return listaDatos.FirstOrDefault().OperadorID;
                }

            }

            return ID;
        }

        public long? GetOperadorIDyCodigoEntidad(long lClienteID, string codigo)
        {
            List<Entidades> listaDatos;
            long? ID = 0;
            try
            {
                listaDatos = (from c in Context.Entidades where c.Codigo == codigo && c.Activo && c.ClienteID == lClienteID select c).ToList();

                if (listaDatos.Count == 1)
                {
                    if (listaDatos.FirstOrDefault().OperadorID != null)
                    {
                        return listaDatos.FirstOrDefault().OperadorID;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ID = 0;
            }

            return ID;
        }

        public Operadores GetActivoOperador(string codigo, long clienteID)
        {
            Operadores operador;
            try
            {
                operador = (from ent in Context.Entidades
                            join oper in Context.Operadores on ent.OperadorID equals oper.OperadorID
                            where
                                ent.Activo &&
                                ent.Codigo == codigo &&
                                ent.ClienteID == clienteID
                            select oper).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                operador = null;
            }

            return operador;
        }

        public Entidades GetEntidadByCodigo(string codigo, long clienteID)
        {
            Entidades Entidad;
            try
            {
                Entidad = (from ent in Context.Entidades
                           where
                               ent.Activo &&
                               ent.Codigo == codigo &&
                               ent.ClienteID == clienteID
                           select ent).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Entidad = null;
            }

            return Entidad;
        }

        public long GetEntidadIDByNombre(string nombre, long clienteID)
        {
            long EntidadID = 0;
            try
            {
                EntidadID = (from ent in Context.Entidades
                             where
                                 ent.Activo &&
                                 ent.Nombre == nombre &&
                                 ent.ClienteID == clienteID
                             select ent.EntidadID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                EntidadID = 0;
            }

            return EntidadID;
        }

        public bool eliminaEntidad(long entidadID)
        {
            bool existe = true;
            Entidades dato;
            dato = (from c in Context.Entidades where c.EntidadID == entidadID select c).First();
            try
            {
                Context.Entidades.DeleteOnSubmit(dato);
                Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }
            return existe;
        }

        public List<Entidades> GetActivos(long clienteID)
        {
            List<Entidades> entidades;
            try
            {
                entidades = (from c in Context.Entidades
                             where
                                 c.ClienteID == clienteID &&
                                 c.Activo
                             select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                entidades = new List<Entidades>();
            }
            return entidades;
        }

        public List<Entidades> GetAllOperadores(long clienteID)
        {
            List<Entidades> entidades;
            try
            {
                entidades = (from c in Context.Entidades
                             where
                                 c.ClienteID == clienteID &&
                                 c.Activo &&
                                 c.OperadorID.HasValue
                             select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                entidades = new List<Entidades>();
            }
            return entidades;
        }

        public List<Entidades> GetAllEmpresasProveedorasLibres(long proyectoID)
        {
            List<long> tipos;
            tipos = (from c in Context.ProyectosEmpresasProveedoras where c.ProyectoID == proyectoID select c.EmpresaProveedoraID).ToList<long>();
            return (from c in Context.Entidades where !(tipos.Contains((long)c.EmpresaProveedoraID)) && c.EmpresaProveedoraID != null select c).ToList<Entidades>();
        }

        public List<object> listaEntidadesProyectos(long proyectoID, long clienteID)
        {
            List<object> listaEntidades;
            List<object> listaAuxiliar= new List<object>();
            int listaUsuarios;
            int listaProyectoUsuarios;
            try
            {
                List<long> tipos;
                tipos = (from c in Context.ProyectosEmpresasProveedoras where c.ProyectoID == proyectoID select c.EmpresaProveedoraID).ToList<long>();
                listaEntidades = (from c in Context.Entidades where tipos.Contains((long)c.EmpresaProveedoraID) && c.EmpresaProveedoraID != null && c.ClienteID==clienteID select c).ToList<object>();
                

                foreach (Entidades item in listaEntidades)
                {
                    listaUsuarios = (from c in Context.Usuarios where c.EntidadID == item.EntidadID && c.ClienteID== clienteID select c).Count();
                    listaProyectoUsuarios = (from c in Context.Vw_ProyectosUsuarios where c.EntidadID == item.EntidadID && c.ProyectoID==proyectoID && c.ClienteID== clienteID select c).Count();
                    if (listaProyectoUsuarios >= listaUsuarios)
                    {
                        listaAuxiliar.Add(item);
                        //listaEntidades.Remove(item);
                    }
                }
                foreach (object item in listaAuxiliar)
                {
                    listaEntidades.Remove(item);
                }
                listaEntidades.Add(getDefecto(clienteID));

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaEntidades = null;
            }

            return listaEntidades;
        }
        
        public string getCodigoByID (long? lID)
        {
            string sCodigo;

            try
            {
                sCodigo = (from c in Context.Entidades where c.EntidadID == lID select c.Codigo).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sCodigo = null;
            }

            return sCodigo;
        }

        public List<Entidades> getEntidadesOperadores(long clienteID)
        {
            List<Entidades> listaDatos;
            try
            {
                listaDatos = (from c in Context.Entidades where (c.ClienteID == clienteID && c.Activo) orderby c.Nombre select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public long? getOperadorCliente(long lClienteID)
        {
            long? lEntidadID = null;
            try
            {
                lEntidadID = (from c in Context.Entidades where (c.ClienteID == lClienteID && c.EntidadCliente == true) select c.EntidadID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lEntidadID = null;
            }
            return lEntidadID;
        }

    }
}