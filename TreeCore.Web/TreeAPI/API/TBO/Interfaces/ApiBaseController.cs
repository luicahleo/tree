using CapaNegocio;
using System;
using System.Globalization;
using System.Web;
using System.Web.Http;
using TreeCore.Data;

namespace TreeAPI.API.TBO.Interfaces
{
    public class ApiBaseController : ApiController
    {
        


        #region PROPPERTIES
        public Usuarios user;
        public Paises pais;
        public RegionesPaises regionPais;
        public Provincias provincia;
        public Municipios municipio;
        #endregion

        #region LANGUAGE
        public static string GetLanguage()
        {
            IdiomasController cIdiomas = new IdiomasController();
            return cIdiomas.GetCodigoDefecto();
        }

        public static string GetGlobalResource(string tag, string CodeLanguage)
        {
            string str;
            try
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(CodeLanguage);
                str = HttpContext.GetGlobalResourceObject("TBO", tag, cultureInfo).ToString();
            }
            catch (Exception ex)
            {
                str = string.Empty;
            }

            return str;
        }
        #endregion

        protected void SetUsuario(string emailUser)
        {
            try
            {
                UsuariosController cUsuarios = new UsuariosController();
                user = cUsuarios.getUsuarioByEmail(emailUser);
            }
            catch (Exception ex)
            {
                user = null;
            }
        }

        protected bool ComprobarLocalizacion(long clienteID, string sRegion, string sPais, string sRegionPais, string sProvincia, string sMunicipalidad)
        {
            bool esCorrecto = false;

            RegionesController cRegiones = new RegionesController();
            PaisesController cPaises = new PaisesController();
            RegionesPaisesController cRegionesPaises = new RegionesPaisesController();
            ProvinciasController cProvincias = new ProvinciasController();
            MunicipiosController cMunicipios = new MunicipiosController();

            Regiones region = cRegiones.GetRegionActivaByNombre(sRegion);
            if (region != null)
            {
                pais = cPaises.GetPaisByNombreRegion(sPais, region.RegionID, clienteID);
                if (pais != null)
                {
                    regionPais = cRegionesPaises.GetByNombre(pais.PaisID, sRegionPais);
                    if (regionPais != null)
                    {
                        provincia = cProvincias.GetActivaByNombre(regionPais.RegionPaisID, sProvincia);
                        if (provincia != null)
                        {
                            municipio = cMunicipios.GetActivoByNombre(provincia.ProvinciaID, sMunicipalidad);
                            if (municipio != null)
                            {
                                esCorrecto = true;
                            }
                        }
                    }
                }
            }
            
            return esCorrecto;
        }

        public string concatLocalizacion(string Region, string Pais, string RegionPais, string provincia, string Municipalidad)
        {
            string result = string.Join(" -> ", new { Region, Pais, RegionPais, provincia, Municipalidad });
                
            return result;
        }

        #region LOG
        public string GetMessajeLog(string nameClass, string nameMethod, string message)
        {
            return "Exception - " + nameClass + ".asmx.cs - " + nameMethod + " : " + message;
        }
        #endregion

        protected bool DateIsValid(string date)
        {
            bool dateIsValid = false;
            DateTime d;
            dateIsValid = DateTime.TryParseExact(date, Comun.FORMATO_FECHA, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

            return dateIsValid;
        }

    }
}