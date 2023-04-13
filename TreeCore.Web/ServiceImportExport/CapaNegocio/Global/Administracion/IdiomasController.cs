using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class  IdiomasController : GeneralBaseController<Idiomas, TreeCoreContext>
    {
        public IdiomasController()
            : base()
        { }

        public bool RegistroVinculado(long IdiomaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string Idioma, string sCodigo, long clienteID, long? lID)
        {
            bool isExiste = false;
            List<Idiomas> datos = new List<Idiomas>();


            datos = (from c in Context.Idiomas where ((c.Idioma == Idioma || c.CodigoIdioma == sCodigo) && c.ClienteID == clienteID) select c).ToList<Idiomas>();

            if (datos.Count > 0 && lID != datos[0].IdiomaID)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long IdiomaID)
        {
             Idiomas dato = new Idiomas();
             IdiomasController cController = new IdiomasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && IdiomaID == " + IdiomaID.ToString());

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

        public Idiomas GetIdiomaByID(long idiomaID)
        {
            Idiomas datos = new Idiomas();
            datos = (from c in Context.Idiomas where (c.IdiomaID == idiomaID) select c).First();
            return datos;
        }

        public List<Idiomas> GetAllIdiomas()
        {
            List<Idiomas> datos = new List<Idiomas>();
            IdiomasController cIdiomas = new IdiomasController();

            try
            {
                datos = cIdiomas.GetItemsList("Activo == true");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = null;
            }

            return datos;
        }

        public string GetCadenaIdioma()
        {
            int countIdioma = 0;
            countIdioma = (from c in Context.Idiomas where (c.Activo == true) select c).Count();

            List<Idiomas> datos = new List<Idiomas>();
            IdiomasController cIdiomas = new IdiomasController();
            string cadena = "";

            datos = cIdiomas.GetAllIdiomas();

            foreach (Idiomas detalle in datos)
            {
                if (detalle.Idioma.Equals("Español"))
                {
                    cadena += "btnIdiomaEspañol";
                }

                cadena += "btnIdioma" + detalle.Idioma + '|';
            }

            return cadena;
        }

        public bool isExisteIdioma(string idioma)
        {
            bool isExiste = false;
            List<Idiomas> listaIdiomas = new List<Idiomas>();

            listaIdiomas = (from c in Context.Idiomas where c.Idioma == idioma select c).ToList<Idiomas>();

            if (listaIdiomas.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public Idiomas GetDefault(long lClienteID)
        {
            Idiomas oIdioma;

            try
            {
                oIdioma = (from c in Context.Idiomas where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oIdioma = null;
            }

            return oIdioma;
        }

        public string GetCodigoDefecto()
        {
            string codigo = string.Empty;

            try
            {
                codigo = (from c 
                          in Context.Idiomas 
                          where c.Defecto 
                          select c.CodigoIdioma).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                codigo = null;
            }

            return codigo;
        }
    }
}