using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmpresasProveedorasController : GeneralBaseController<EmpresasProveedoras, TreeCoreContext>, IBasica<EmpresasProveedoras>
    {
        public EmpresasProveedorasController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public List<EmpresasProveedoras> GetAllEmpresasProveedoras()
        {
            // Local variables
            List<EmpresasProveedoras> lista = null;
            try
            {
                lista = (from c in Context.EmpresasProveedoras select c).ToList<EmpresasProveedoras>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public bool RegistroDuplicado(string EmpresaProveedora, long clienteID)
        {
            bool isExiste = false;
            List<EmpresasProveedoras> datos = new List<EmpresasProveedoras>();


            datos = (from c in Context.EmpresasProveedoras where (c.EmpresaProveedora == EmpresaProveedora && c.ClienteID == clienteID) select c).ToList<EmpresasProveedoras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoCIF(string CIF, long clienteID)
        {
            bool isExiste = false;
            List<EmpresasProveedoras> datos = new List<EmpresasProveedoras>();


            datos = (from c in Context.EmpresasProveedoras where (c.CIF == CIF && c.ClienteID == clienteID) select c).ToList<EmpresasProveedoras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicado_NombreCIF(string pEmpresaProveedora, string pCIF, long pClienteID, bool isInsert)
        {
            bool exite = false;
            List<EmpresasProveedoras> EmpresasProveedoras = new List<EmpresasProveedoras>();

            //comprobar si el Nombre de Empresa está repetido en el Cliente
            EmpresasProveedoras = (from c in Context.EmpresasProveedoras where c.EmpresaProveedora == pEmpresaProveedora && c.ClienteID == pClienteID select c).ToList();

            if (EmpresasProveedoras.Count == 0)
            { //comprobar si el CIF está repetido en el Cliente
                EmpresasProveedoras = (from c in Context.EmpresasProveedoras where c.CIF == pCIF && c.ClienteID == pClienteID select c).ToList();
            }

            if (isInsert == true && EmpresasProveedoras.Count > 0) //Insert
            {
                exite = true;
            }
            else if (isInsert == false && EmpresasProveedoras.Count > 1) //Update
            {
                exite = true;
            }

            return exite;
        }

        public bool RegistroDuplicadoDetalle(string EmpresaProveedora, long clienteID)
        {
            bool isExiste = false;
            List<EmpresasProveedoras> datos = new List<EmpresasProveedoras>();


            datos = (from c in Context.EmpresasProveedoras where (c.EmpresaProveedora == EmpresaProveedora && c.ClienteID == clienteID) select c).ToList<EmpresasProveedoras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EmpresaProveedoraID)
        {
            EmpresasProveedoras dato = new EmpresasProveedoras();
            EmpresasProveedorasController cController = new EmpresasProveedorasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmpresaProveedoraID == " + EmpresaProveedoraID.ToString());

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

        public Vw_EmpresasProveedoras GetItemOfVw(long empresaProveedoraID)
        {
            return (from c in Context.Vw_EmpresasProveedoras where c.EmpresaProveedoraID == empresaProveedoraID select c).First();
        }

        public bool eliminaEmpresaProveedora(long empresaProveedoraID)
        {
            bool existe = true;
            EmpresasProveedoras dato;
            dato = (from c in Context.EmpresasProveedoras where c.EmpresaProveedoraID == empresaProveedoraID select c).First();
            try
            {
                Context.EmpresasProveedoras.DeleteOnSubmit(dato);
                Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }
            return existe;
        }
    }
}