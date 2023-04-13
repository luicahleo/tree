using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreServiciosFrecuenciasController : GeneralBaseController<CoreServiciosFrecuencias, TreeCoreContext>
    {
        public CoreServiciosFrecuenciasController()
            : base()
        { }

        //public List<CoreFrecuencias> getFrecuenciasActivas()
        //{
        //    List<CoreFrecuencias> listaFrecuencias;

        //    try
        //    {
        //        listaFrecuencias = (from c in Context.CoreFrecuencias where c.Activo select c).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaFrecuencias = null;
        //    }

        //    return listaFrecuencias;
        //}

        //public string getCodigoByID(long? lID)
        //{
        //    string sCodigo;

        //    try
        //    {
        //        sCodigo = (from c in Context.CoreFrecuencias where c.CoreFrecuenciaID == lID select c.Codigo).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        sCodigo = null;
        //    }

        //    return sCodigo;
        //}

        public bool RegistroDuplicado(string lNombre)
        {
            bool isExiste = false;
            List<CoreServiciosFrecuencias> datos = new List<CoreServiciosFrecuencias>();


            datos = (from c in Context.CoreServiciosFrecuencias where (c.Nombre == lNombre) select c).ToList<CoreServiciosFrecuencias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}