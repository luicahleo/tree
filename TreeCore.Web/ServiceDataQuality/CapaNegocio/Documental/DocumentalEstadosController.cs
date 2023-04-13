using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DocumentalEstadosController : GeneralBaseController<DocumentosEstados, TreeCoreContext>
    {
        public DocumentalEstadosController()
            : base()
        { }

        public bool RegistroVinculado(long DocumentoExtensionID)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        public bool RegistroDuplicado(string Nombre, string Codigo, long lClienteID)
        {
            bool bExiste = false;
            List<DocumentosEstados> listaDatos;


            listaDatos = (from c in Context.DocumentosEstados where (c.Nombre == Nombre || c.Codigo == Codigo && c.ClienteID == lClienteID) select c).ToList<DocumentosEstados>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool RegistroDuplicadoNombre(string Nombre, long lClienteID)
        {
            bool bExiste = false;
            List<DocumentosEstados> listaDatos;


            listaDatos = (from c in Context.DocumentosEstados where (c.Nombre == Nombre  && c.ClienteID == lClienteID) select c).ToList<DocumentosEstados>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }
        public bool RegistroDuplicadoCodigo(string Codigo, long lClienteID)
        {
            bool bExiste = false;
            List<DocumentosEstados> listaDatos;


            listaDatos = (from c in Context.DocumentosEstados where (c.Codigo == Codigo && c.ClienteID == lClienteID) select c).ToList<DocumentosEstados>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }
        public DocumentosEstados GetDefault(long clienteID)
        {
            DocumentosEstados docEstado;
            try
            {
                docEstado = (from c in Context.DocumentosEstados where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                docEstado = null;
                log.Error(ex.Message);
            }
            return docEstado;
        }

        public bool RegistroDefecto(long DocumentoEstadoID)
        {
            DocumentosEstados dato = new DocumentosEstados();
            DocumentalEstadosController cController = new DocumentalEstadosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && DocumentoEstadoID == " + DocumentoEstadoID.ToString());

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

    }
}