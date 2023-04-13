using CapaNegocio;
using System.Collections.Generic;

namespace TreeCore.Data
{
    public class Accesos
    {

        List<string> _PaginasPermitidas;
        public List<string> PaginasPermitidas
        {
            get { return _PaginasPermitidas; }
            set { _PaginasPermitidas = value; }
        }

        public Accesos(Usuarios user)
        {
            _PaginasPermitidas = new List<string>();
            _PaginasPermitidas.Add("default.aspx");
            _PaginasPermitidas.Add("inicio.aspx");
            _PaginasPermitidas.Add("thirdinicio.aspx");
            _PaginasPermitidas.Add("contactinicio.aspx");
            _PaginasPermitidas.Add("settingsinicio.aspx");
            _PaginasPermitidas.Add("contractsinicio.aspx");
            _PaginasPermitidas.Add("monitoringinicio.aspx");
            _PaginasPermitidas.Add("filesinicio.aspx");
            _PaginasPermitidas.Add("exportimportinicio.aspx");
            _PaginasPermitidas.Add("woinicio.aspx");
            _PaginasPermitidas.Add("sitesinicio.aspx");
            _PaginasPermitidas.Add("dqinicio.aspx");

            _PaginasPermitidas.Add("inflaciones.aspx");
            _PaginasPermitidas.Add("mapas.aspx");
            _PaginasPermitidas.Add("diagramaestados.aspx");
            _PaginasPermitidas.Add("inventarioadmin.aspx");



            ModulosController mControl = new ModulosController();
            List<string> lPaginasPermitidas = new List<string>();

            if (user != null && user.ClienteID == null && user.EMail.Equals(Comun.TREE_SUPER_USER))
            {
                lPaginasPermitidas = mControl.getModulosSuperUser();
            }
            else
            {
                lPaginasPermitidas = mControl.getModulos(user.UsuarioID);
            }

            if (lPaginasPermitidas != null && lPaginasPermitidas.Count > 0)
            {
                _PaginasPermitidas.AddRange(lPaginasPermitidas);
            }

            #region GLOBAL

            #region ADMINISTRACION

            if (_PaginasPermitidas.Contains("licencias.aspx"))
            {
                PaginasPermitidas.Add("licenciastipos.aspx");
            }

            if (_PaginasPermitidas.Contains("proyectos.aspx"))
            {
                PaginasPermitidas.Add("proyectosgestion.aspx");
                PaginasPermitidas.Add("proyectosusuarios.aspx");
            }


            if (_PaginasPermitidas.Contains("monedas.aspx"))
            {
                PaginasPermitidas.Add("monedasglobales.aspx");
            }


            if (_PaginasPermitidas.Contains("inflaciones.aspx"))
            {
                PaginasPermitidas.Add("globalsapinterfazindices.aspx");
            }

            PaginasPermitidas.Add("configinicialform.aspx");

            if (_PaginasPermitidas.Contains("emplazamientoscontenedor.aspx"))
            {
                PaginasPermitidas.Add("emplazamientosgridprincipal.aspx");
                PaginasPermitidas.Add("emplazamientosgridlocalizaciones.aspx");
                PaginasPermitidas.Add("emplazamientosgridinventario.aspx");
                PaginasPermitidas.Add("emplazamientosgridatributosdinamicos.aspx");
                PaginasPermitidas.Add("emplazamientosmapas.aspx");
                PaginasPermitidas.Add("servicesettings.aspx");
                PaginasPermitidas.Add("emplazamientosgriddocumentos.aspx");
                PaginasPermitidas.Add("emplazamientosgridcontactos.aspx");
            }

            if (_PaginasPermitidas.Contains("productcatalogservicioscontenedor.aspx"))
            {
                PaginasPermitidas.Add("productcatalog.aspx");
                PaginasPermitidas.Add("productcatalogpacks.aspx");
                PaginasPermitidas.Add("productcatalogservicios.aspx");
                PaginasPermitidas.Add("productcatalogprecios.aspx");
                PaginasPermitidas.Add("formulas.aspx");
            }

            if (_PaginasPermitidas.Contains("estadoscontenedor.aspx"))
            {
                PaginasPermitidas.Add("estados.aspx");
            }

            if (_PaginasPermitidas.Contains("globalsettingscontenedor.aspx"))
            {
                PaginasPermitidas.Add("loginsettings.aspx");
                PaginasPermitidas.Add("servicesettings.aspx");
            }

            #endregion

            #endregion

            #region CALIDAD

            if (_PaginasPermitidas.Contains("calidadkpi.aspx"))
            {
                PaginasPermitidas.Add("calidadkpiresults.aspx");
                PaginasPermitidas.Add("calidadkpireport.aspx");
            }

            #endregion

            #region INVENTARIO

            PaginasPermitidas.Add("invinicio.aspx");

            if (_PaginasPermitidas.Contains("inventariocategoriascontenedor.aspx"))
            {
                PaginasPermitidas.Add("inventariocategoriasvinculaciones.aspx");
                PaginasPermitidas.Add("inventariocategorias.aspx");
                PaginasPermitidas.Add("inventariocategoriadiagrama.aspx");
                PaginasPermitidas.Add("inventariocategoriasatributos.aspx");
            }

            if (_PaginasPermitidas.Contains("inventariogestioncontenedor.aspx"))
            {
                PaginasPermitidas.Add("inventariocategoryview.aspx");
                PaginasPermitidas.Add("inventariocategoryviewvistacategoria.aspx");
                PaginasPermitidas.Add("inventariogestion_content.aspx");
            }

            #endregion

            #region IMPORT/EXPORT

            PaginasPermitidas.Add("dataupload.aspx");

            #endregion

            #region WORK ORDERS

            PaginasPermitidas.Add("workorderscontenedor.aspx");
            PaginasPermitidas.Add("workorderstickets.aspx");
            PaginasPermitidas.Add("workorderscalendar.aspx");

            #endregion

            #region WORKFLOWS

            PaginasPermitidas.Add("workflows.aspx");

            #endregion

        }

    }
}
