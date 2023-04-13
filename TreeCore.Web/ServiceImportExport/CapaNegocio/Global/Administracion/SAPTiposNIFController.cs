using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class SAPTiposNIFController : GeneralBaseController<SAPTiposNIF, TreeCoreContext>, IBasica<SAPTiposNIF>
    {
        public SAPTiposNIFController()
            : base()
        { }

        public bool RegistroVinculado(long SAPTipoNIFID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string codigo, long clienteID)
        {
            bool isExiste = false;
            List<SAPTiposNIF> datos = new List<SAPTiposNIF>();


            datos = (from c in Context.SAPTiposNIF where (c.Codigo == codigo && c.ClienteID == clienteID) select c).ToList<SAPTiposNIF>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SAPTipoNIFID)
        {
            SAPTiposNIF dato = new SAPTiposNIF();
            SAPTiposNIFController cController = new SAPTiposNIFController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SAPTipoNIFID == " + SAPTipoNIFID.ToString());

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
        public SAPTiposNIF GetDefault(long clienteID) {
            SAPTiposNIF oSAPTiposNIF;
            try
            {
                oSAPTiposNIF = (from c 
                                in Context.SAPTiposNIF 
                                where c.Defecto && 
                                        c.ClienteID == clienteID 
                                select c).First();
            }
            catch (Exception)
            {
                oSAPTiposNIF = null;
                throw;
            }
            return oSAPTiposNIF;
        }

        public List<SAPTiposNIF> GetSAPTiposNIFByCliente(long clienteID)
        {
            List<SAPTiposNIF> datos = new List<SAPTiposNIF>();

            datos = (from c in Context.SAPTiposNIF where (c.ClienteID == clienteID) orderby c.Descripcion select c).ToList<SAPTiposNIF>();

            return datos;
        }
        public SAPTiposNIF GetTipoNifByCodigo(string sCodigo)
        {
            List<SAPTiposNIF> lista = null;
            SAPTiposNIF dato = null;

            try
            {

                lista = (from c in Context.SAPTiposNIF where c.Codigo == sCodigo select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return dato;
            }

            return dato;
        }
    }
}