using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System;
using TreeCore;
using log4net;
using System.Reflection;

namespace CapaNegocio
{
    public sealed class UsuariosController : GeneralBaseController<Usuarios, TreeCoreContext>
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UsuariosController()
            : base()
        {
            
        }

        #region FUNCIONES GENERALES

        public bool isInterno(long usuarioID)
        {
            bool isInterno = (from c in Context.Usuarios where c.UsuarioID == usuarioID select c).First().Interno;

            return isInterno;
        }

        public override bool UpdateItem(Usuarios item)
        {
            if (UsuarioDuplicado(item, false))
            {
                throw new Exception(Resources.Comun.strUsuarioDuplicado.ToString());
            }
            else
            {
                //Obtener el usuario a Actualizat.
                Usuarios dato = GetItem(item.UsuarioID);
                bool AgregarLDAP = false;
                //Si se ha Cambiado el valor del LDAP a true
                //Hsy que agregar el Usuario al ActiveDirectory
                if (item.LDAP == true)
                {
                    AgregarLDAP = (item.LDAP != dato.LDAP);
                }

                if (base.UpdateItem(item))
                {
                    if (AgregarLDAP)
                    {
                        //agregaUsuarioLDAP(item);
                    }
                }
                return true;
            }
        }

        public Usuarios getUsuarioByEmail(string EMail)
        {
            Usuarios user = new Usuarios();

            try
            {
                user = (from c in Context.Usuarios
                        where c.EMail == EMail && c.Activo == true
                        select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                user = null;
            }

            return user;

        }

        public Usuarios getUserByEmail(string EMail)
        {
            Usuarios user = new Usuarios();

            try
            {
                user = (from c in Context.Usuarios
                        where c.EMail == EMail
                        select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                user = null;
            }

            return user;

        }

        public bool checkSystemUser(string nombre, string apellidos, string email, long? clienteID)
        {
            UsuariosController cUsuarios = new UsuariosController();
            bool correcto = true;

            try
            {
                List<Usuarios> lUser = new List<Usuarios>();

                if(clienteID != null)
                {
                    lUser = (from c in Context.Usuarios
                             where c.EMail == email && c.ClienteID == clienteID
                             select c).ToList();
                }
                else
                {
                    lUser = (from c in Context.Usuarios
                             where c.EMail == email
                             select c).ToList();
                }

                if (lUser == null || lUser.Count == 0)
                {
                    Usuarios user = new Usuarios();
                    user.Nombre = nombre;
                    user.Apellidos = apellidos;
                    user.EMail = email;
                    user.Clave = nuevaClave();
                    user.Activo = true;
                    user.Telefono = "";
                    user.FechaCreacion = DateTime.Now;
                    user.ClienteID = clienteID;

                    if (cUsuarios.AddItem(user) == null)
                    {
                        correcto = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                correcto = false;
            }

            return correcto;
        }

        #endregion

        #region GESTION DE ACCESO

        public bool UsuarioDuplicado(Usuarios item, bool nuevo)
        {
            List<Usuarios> dato;
            //Comprueba si el nombre del usuario esta en la BD
            if (nuevo) // destingue si estamos editando o agregando
            {
                dato = (from c in Context.Usuarios where c.EMail == item.EMail select c).ToList();
            }
            else
            {
                dato = (from c in Context.Usuarios where c.EMail == item.EMail && c.UsuarioID != item.UsuarioID select c).ToList();
            }
            // Si devulve un valor Return true sino return false
            if (dato.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Cambiar Clave de Usuario
        /// </summary>
        /// <param name="UsuarioID">ID del Usuario</param>
        /// <param name="Clave">Clave Nueva</param>
        /// <param name="CambiarSiguienteInicio">bool cambiar al Iniciar</param>
        /// <returns>Acción</returns>
        public string UsuariosCambiarClave(long UsuarioID, string Clave, bool CambiarSiguienteInicio)
        {
            string aviso = "";
            DateTime aux = new DateTime();
            string claveNueva = Tree.Utiles.md5.MD5String(Clave);
            try
            {
                Usuarios usr;
                string valor = "";
                ParametrosController cParametros = new ParametrosController();
                valor = cParametros.GetItemValor("CADUCIDAD CLAVE");
                usr = base.GetItem(UsuarioID); // obtenemos el Usuario

                //si la nueva clave está contenida en las Ultimas Claves
                if (((usr.UltimasClaves.Split(Convert.ToChar(",")).Length < 5) && (usr.UltimasClaves.Contains(claveNueva)) || ((usr.UltimasClaves.Split(Convert.ToChar(",")).Length == 5) && (usr.UltimasClaves.Split(Convert.ToChar(",")).Skip(1).Take(4).Contains(claveNueva)))))
                {
                    aviso = Resources.Comun.strUsuariosClaveRepetida.ToString();
                    return aviso;
                }
                //agregar la nueva clave a la cadena.
                usr.Clave = claveNueva;
                usr.UltimasClaves = TreeCore.Util.EncolarClaveAnterior(usr.UltimasClaves, claveNueva);
                usr.CambiarClave = CambiarSiguienteInicio;
                usr.FechaUltimoCambio = DateTime.Now;
                if (valor != "" && valor != null) aux = DateTime.Now.AddDays(Convert.ToInt32(valor));
                if (aux != DateTime.MinValue) usr.FechaCaducidadClave = Convert.ToDateTime(aux);
                base.UpdateItem(usr);
                if (usr.LDAP)
                {

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                aviso = Resources.Comun.strErrorGenerico.ToString() + " " + ex.Message;
            }
            return aviso;
        }

        /// <summary>
        /// CAMBIAR CLAVE DE USUARIO DESDE DEFAULT
        /// </summary>
        /// <param name="UsuarioID">USUARIO ID</param>
        /// <param name="Clave">CLAVE NUEVA</param>
        /// <returns>ACCION</returns>
        internal string UsuariosCambiarClave(long UsuarioID, string Clave)
        {
            string aviso = "";
            string claveNueva = Tree.Utiles.md5.MD5String(Clave);
            try
            {
                Usuarios usr;
                DateTime aux = new DateTime();
                string valor = "";
                ParametrosController cParametros = new ParametrosController();
                valor = cParametros.GetItemValor("CADUCIDAD CLAVE");
                usr = base.GetItem(UsuarioID);

                if ((usr.UltimasClaves == null) || (usr.UltimasClaves == ""))
                {
                    usr.UltimasClaves = usr.Clave;
                }
                if (((usr.UltimasClaves.Split(Convert.ToChar(",")).Length < 5) && (usr.UltimasClaves.Contains(claveNueva)) || ((usr.UltimasClaves.Split(Convert.ToChar(",")).Length == 5) && (usr.UltimasClaves.Split(Convert.ToChar(",")).Skip(1).Take(4).Contains(claveNueva)))))
                {
                    aviso = "ClaveRepetida";
                    return aviso;
                }

                // vamos a actualizar en ldap si es necesario


                usr.Clave = claveNueva;
                usr.UltimasClaves = TreeCore.Util.EncolarClaveAnterior(usr.UltimasClaves, claveNueva);
                usr.FechaUltimoCambio = DateTime.Now;
                if (valor != "" && valor != null) aux = DateTime.Now.AddDays(Convert.ToInt32(valor));
                if (aux != DateTime.MinValue) usr.FechaCaducidadClave = Convert.ToDateTime(aux);
                usr.CambiarClave = false;
                base.UpdateItem(usr);
                aviso = "Cambio";

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                aviso = Resources.Comun.strErrorGenerico.ToString() + " " + ex.Message;
            }

            return aviso;
        }

        /// <summary>
        /// Genera una nueva clave según las condiciones que necesita la aplicación
        /// </summary>
        /// <returns>Nueva clave generada</returns>
        public string nuevaClave()
        {
            string res = "";
            string numeros = "0123456789";
            string minusculas = "abcdefghijklmnopqrtsuvwxyz";
            string mayusculas = minusculas.ToUpper();

            Random ran = new Random();

            //Genera un dígito aleatorio
            res += numeros[ran.Next(0, numeros.Length)];
            //Genera una mayúscula aleatoria
            res += mayusculas[ran.Next(0, mayusculas.Length)];
            //Genera 5 minúsculas aleatorias
            for (int i = 0; i < 5; i++)
            {
                res += minusculas[ran.Next(0, minusculas.Length)];
            }

            char[] c = res.ToCharArray();
            int k = c.Length;

            for (int i = c.Length; i > 1; i--)
            {
                int j = ran.Next(i);
                char tmp = c[j];
                c[j] = c[i - 1];
                c[i - 1] = tmp;
            }

            res = new String(c);
            return res;
        }

        /// <summary>
        /// Genera un nuevo número aleatorio de 6 dígitos
        /// </summary>
        /// <returns>Nuevo código generado</returns>

        public Usuarios UsuariosLogin(string Usuario, string Clave)
        {
            Usuarios dato = new Usuarios();

            try
            {
                if (Usuario == "" || Clave == "")
                {
                    dato = null;
                }
                else
                {
                    dato = (from c in Context.Usuarios where c.EMail == Usuario && c.Clave == Tree.Utiles.md5.MD5String(Clave) select c).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
                log.Error(ex.Message);
            }

            return dato;
        }

        #endregion

        public bool RegistroDuplicado(string EMail)
        {
            bool isExiste = false;
            List<Usuarios> datos = new List<Usuarios>();
            datos = (from c in Context.Usuarios where (c.EMail == EMail) select c).ToList<Usuarios>();
            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<Usuarios> GetAllUsersByClienteID(long clienteID)
        {
            List<Usuarios> listaDatos;
            try
            {
                listaDatos = (from c in Context.Usuarios where c.ClienteID == clienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<Vw_Usuarios> GetUsersActivos(long clienteID)
        {
            List<Vw_Usuarios> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_Usuarios where c.Activo && c.ClienteID == clienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<Vw_Usuarios> GetActivosSupers()
        {
            List<Vw_Usuarios> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_Usuarios where c.Activo && c.ClienteID == null select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public bool isSuper(long usuarioID)
        {
            bool isSuper = false;
            List<Usuarios> datos = new List<Usuarios>();
            datos = (from c in Context.Usuarios where c.UsuarioID == usuarioID && c.Activo && c.ClienteID == null select c).ToList<Usuarios>();
            if (datos.Count > 0)
            {
                isSuper = true;
            }

            return isSuper;
        }

        public List<Vw_ClientesProyectosZonas> GetClientesProyectosZonas(long usuarioID)
        {
            List<Vw_ClientesProyectosZonas> datos = new List<Vw_ClientesProyectosZonas>();
            UsuariosPerfilesController cUsuaiorsPerfiles = new UsuariosPerfilesController();

            try
            {
                datos = (from c in Context.Vw_ClientesProyectosZonas where c.UsuarioID.Equals(usuarioID) && !c.AlquilerTipoContratacionID.HasValue && !c.AlquilerTipoContratoID.HasValue select c).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return datos;
        }

        public bool BloquearUsuario(string email)
        {
            bool bloqueado = false;
            MonitoringModificacionesUsuariosController cModificacionUsuario = new MonitoringModificacionesUsuariosController();

            try
            {
                Usuarios user = getUsuarioByEmail(email);
                if (user != null)
                {
                    user.Activo = false;
                    user.CambiarClave = true;
                    user.FechaUltimoCambio = DateTime.Now;
                    user.FechaDesactivacion = DateTime.Now;

                    if (UpdateItem(user))
                    {
                        MonitoringModificacionesUsuarios mod = new MonitoringModificacionesUsuarios();
                        mod.UsuarioID = user.UsuarioID;
                        mod.UsuarioModificadorID = user.UsuarioID;
                        mod.FechaModificacion = DateTime.Now;
                        mod.ProyectoTipoID = (long)Comun.Modulos.GLOBAL;
                        mod.CambioEfectuado = Resources.Comun.strUsuarioBloqueado;
                        mod.OperacionRealizada = "";
                        cModificacionUsuario.AddItem(mod);
                        bloqueado = true;
                    }
                    else
                    {
                        bloqueado = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bloqueado = false;
            }

            return bloqueado;
        }

        public long GetEntidadIDByUsuario(long usuarioID)
        {
            // Local variables
            List<Usuarios> lista = null;
            Usuarios dato = null;


            lista = (from c in Context.Usuarios where (c.UsuarioID == usuarioID) select c).ToList<Usuarios>();

            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }


            return dato.EntidadID.Value;

        }

        public Vw_UsuariosAPI ModificarMAC(long UsuarioID, string MAC)
        {
            Vw_UsuariosAPI dato = GetItem<Vw_UsuariosAPI>(UsuarioID);
            Usuarios modelo = GetItem(UsuarioID);
            if (string.IsNullOrEmpty(dato.MacDispositivo))
            {

                if (UsuariosByMacByClienteID(MAC, dato.ClienteID).Count > 0)
                {
                    dato.MacDispositivo = null;
                    return dato;
                }
                else
                {
                    modelo.MacDispositivo = MAC;
                    UpdateItem(modelo);
                }
            }
            else if (dato.MacDispositivo != MAC)
            {
                dato.MacDispositivo = null;
                return dato;
            }

            return GetItem<Vw_UsuariosAPI>(dato.UsuarioID);
        }

        public List<Vw_UsuariosAPI> UsuariosByMacByClienteID(string MAC, long? ClienteID)
        {
            if (ClienteID == null)
            {
                return (from c in Context.Vw_UsuariosAPI where c.MacDispositivo == MAC && c.Activo select c).ToList<Vw_UsuariosAPI>();

            }
            else
            {
                return (from c in Context.Vw_UsuariosAPI where c.MacDispositivo == MAC && (c.ClienteID == ClienteID || c.ClienteID == null) && c.Activo select c).ToList<Vw_UsuariosAPI>();
            }
        }

        public Vw_UsuariosAPI UsuariosLoginAPIV2(string Usuario, string Clave, string MAC)
        {
            List<Vw_UsuariosAPI> datos = null;
            Vw_UsuariosAPI dato;
            //Vw_UsuariosAPI aux;
            try
            {
                datos = (from f in Context.Vw_UsuariosAPI where f.EMail.Equals(Usuario) && f.Activo select f).ToList();
                if (datos.Count == 0)
                {
                    dato = null;
                    return dato;
                }
                else
                {
                    try
                    {
                        dato = UsuariosLoginDBAPI(Usuario, Clave);
                        dato = ModificarMAC(dato.UsuarioID, MAC);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        dato = null;
                        return dato;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //e.Success = False
                //throw new Exception(ex.Message);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                throw new Exception(codTit);

            }

            return dato;
        }

        public Vw_UsuariosAPI UsuariosLoginAPIV3(string Usuario, string Clave, string MAC)
        {
            List<Vw_UsuariosAPI> datos = null;
            Vw_UsuariosAPI dato;

            try
            {
                datos = (from f in Context.Vw_UsuariosAPI where f.EMail.Equals(Usuario) && f.Activo select f).ToList();
                if (datos.Count == 0)
                {
                    dato = null;
                    return dato;
                }
                else
                {
                    try
                    {
                        dato = UsuariosLoginDBAPI(Usuario, Clave);
                        dato = ModificarMAC(dato.UsuarioID, MAC);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        dato = null;
                        return dato;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                throw new Exception(codTit);

            }

            return dato;
        }

        public Vw_UsuariosAPI UsuariosLoginDBAPI(string Usuario, string Clave)
        {
            List<Vw_UsuariosAPI> datos;
            datos = base.GetItemsList<Vw_UsuariosAPI>("Email == \"" + Usuario + "\" && Clave == \"" + Tree.Utiles.md5.MD5String(Clave) + "\" && Activo");

            if (datos.Count > 0)
            {
                List<Vw_UsuariosAPI> lista = base.GetItemsList<Vw_UsuariosAPI>("UsuarioID == " + datos[0].UsuarioID.ToString());
                if (lista.Count > 0)
                {
                    return lista[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                datos = base.GetItemsList<Vw_UsuariosAPI>("Email == \"" + Usuario + "\"");
                if (datos.Count > 0)
                {
                    List<Vw_UsuariosAPI> lista = base.GetItemsList<Vw_UsuariosAPI>("UsuarioID == " + datos[0].UsuarioID.ToString());
                    if (lista.Count > 0)
                    {
                        return lista[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Usuarios> UsuariosPorEntidad(long entidadID)
        {
            List<Usuarios> listaUsuarios;

            try
            {

                listaUsuarios = (from c in Context.Usuarios where c.EntidadID == entidadID select c).ToList<Usuarios>();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarios = null;
            }

            return listaUsuarios;
        }
        public List<Usuarios> UsuariosPorDepartamento(long departamentoID)
        {
            List<Usuarios> listaUsuarios;

            try
            {

                listaUsuarios = (from c in Context.Usuarios where c.DepartamentoID == departamentoID select c).ToList<Usuarios>();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarios = null;
            }

            return listaUsuarios;
        }

        public List<Usuarios> getUsuariosCreadorDocumentos(long ClienteID)
        {
            List<Usuarios> listaUsuarios;
            try
            {
                listaUsuarios = (from doc in Context.Documentos
                                 join user in Context.Usuarios on doc.CreadorID equals user.UsuarioID
                                 join docTyp in Context.DocumentTipos on doc.DocumentTipoID equals docTyp.DocumentTipoID
                                 where
                                    docTyp.ClienteID == ClienteID &&
                                    user.Activo
                                 select user).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarios = null;
            }
            return listaUsuarios;
        }

        public List<Usuarios> getUsuariosDocumentosAsignado(long ClienteID)
        {
            List<Usuarios> listaUsuarios;
            try
            {
                listaUsuarios = (from doc in Context.Documentos
                                 join user in Context.Usuarios on doc.UsuarioID equals user.UsuarioID
                                 join docTyp in Context.DocumentTipos on doc.DocumentTipoID equals docTyp.DocumentTipoID
                                 where
                                    docTyp.ClienteID == ClienteID &&
                                    user.Activo
                                 select user).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarios = null;
            }
            return listaUsuarios;
        }

        public List<Usuarios> getUsuariosConInventario(long ClienteID)
        {
            List<Usuarios> listaUsuarios;
            try
            {
                listaUsuarios = (from inv in Context.InventarioElementos
                                 join user in Context.Usuarios on inv.CreadorID equals user.UsuarioID
                                 join empl in Context.Emplazamientos on inv.EmplazamientoID equals empl.EmplazamientoID
                                 where
                                    empl.ClienteID == ClienteID &&
                                    user.Activo
                                 select user).Distinct().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarios = null;
            }
            return listaUsuarios;
        }

        public Vw_UsuariosAPI getUsuarioApi(long usuarioID)
        {
            Vw_UsuariosAPI user;
            try
            {
                user = (from c in Context.Vw_UsuariosAPI
                        where c.UsuarioID == usuarioID
                        select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                user = null;
            }
            return user;
        }

    }
}