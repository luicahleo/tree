using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class DepartamentosController : GeneralBaseController<Departamentos, TreeCoreContext>, IBasica<Departamentos>
    {
        public DepartamentosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Departamento, long clienteID)
        {
            bool isExiste = false;
            List<Departamentos> datos = new List<Departamentos>();


            datos = (from c in Context.Departamentos where (c.Departamento == Departamento && c.ClienteID == clienteID) select c).ToList<Departamentos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long DepartamentoID)
        {
            Departamentos dato = new Departamentos();
            DepartamentosController cController = new DepartamentosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && DepartamentoID == " + DepartamentoID.ToString());

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

        public long GetDepartamentoID(string sNombre)
        {
            long lDatos = new long();

            try
            {
                lDatos = (from c in Context.Departamentos where c.Departamento == sNombre select c.DepartamentoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }

            return lDatos;
        }

        public Departamentos GetDefault(long clienteId)
        {
            Departamentos departamento;
            try
            {
                departamento = (from c
                                in Context.Departamentos
                                where c.Defecto &&
                                    c.ClienteID == clienteId
                                select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                departamento = null;
            }
            return departamento;
        }

        public List<Departamentos> GetAll()
        {
            List<Departamentos> listaDepartamentos;

            try
            {
                listaDepartamentos = (from c in Context.Departamentos select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDepartamentos = null;
            }

            return listaDepartamentos;
        }
        public List<Departamentos> GetAllByClienteID(long clienteID, long proyectoID)
        {
            List<Departamentos> listaDatos = new List<Departamentos>();
            try
            {
                DataTable result;
                #region CADENA CONEXIÓN
#if SERVICESETTINGS
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string connectionString = TreeAPI.Properties.Settings.Default.Conexion;
#else
                string connectionString = TreeCore.Properties.Settings.Default.Conexion;
#endif
                #endregion
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_DepartamentosByProyectoID_Get", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@ProyectoID", proyectoID);

                    var da = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(ds);
                    result = ds.Tables[0];
                }
                Departamentos d;
                foreach (DataRow row in result.Rows)
                {
                    d = new Departamentos();
                    d.DepartamentoID = (long)row[0];
                    d.Departamento = (string)row[1];
                    listaDatos.Add(d);
                }

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