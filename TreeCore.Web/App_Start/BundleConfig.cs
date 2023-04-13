using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace TreeCore
{
    public class BundleConfig
    {

#if SERVICESETTINGS
    public static string Version = System.Configuration.ConfigurationManager.AppSettings["Version"];
#elif TREEAPI
    public static string Version = TreeAPI.Properties.Settings.Default.Version;
#else
        public static string Version = TreeCore.Properties.Settings.Default.Version;

#endif

        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region GENERAL

            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle(Comun.BundleConfigPaths.CONTENT_JS_GENERAL).Include(
                            "~/JS/common.js",
                            "~/JS/dropzone.js",
                            "~/JS/evo-calendar.js",
                            //"~/JS/LayoutResponsive.js",  Comentado el 11/01/2022 Comprobar si da errores y eliminar linea
                            "~/JS/PaginaNoEncontrada.js",
                            "~/Scripts/jquery-3.6.0.min.js",
                            "~/Scripts/jquery-ui-1.12.1.custom/jquery-ui.js",
                            "~/Scripts/bootstrap.min.js",
                            "~/Scripts/js.cookie.min.js",
                            "~/Scripts/jquery-sortable.js",
                            "~/Scripts/bootstrap-toaster.js",
                            "~/Scripts/jquery.inactivity.min.js"
                        ));

            bundles.Add(new ScriptBundle(Comun.BundleConfigPaths.CONTENT_JS_GLOBAL).IncludeDirectory(
                            "~/General/js/", "*.js", false
                        ));

            bundles.Add(new ScriptBundle(Comun.BundleConfigPaths.CONTENT_JS_DEFAULT).Include(
                            "~/JS/Default.js"
                        ));

            
            bundles.Add(new Bundle(Comun.BundleConfigPaths.CONTENT_JS_CHART).Include(
                            "~/Scripts/Chartjs/chart.js",
                            "~/Scripts/Chartjs/luxon.js",
                            "~/Scripts/Chartjs/chartjs-adapter-luxon.js"
                        ));

            bundles.Add(new StyleBundle(Comun.BundleConfigPaths.CONTENT_CSS_SEMAFORO).Include(
                            "~/Scripts/Semaforo/jquery-ui-1.10.0.custom.min.css",
                            "~/Scripts/Semaforo/bootstrap.css"
                        ));
            bundles.Add(new Bundle(Comun.BundleConfigPaths.CONTENT_JS_SEMAFORO).Include(
                            //"~/Scripts/Semaforo/bootstrap.js",
                            "~/Scripts/Semaforo/jquery-ui.js",
                            "~/Scripts/Semaforo/slider.js"
                        ));

            bundles.Add(new Bundle(Comun.BundleConfigPaths.CONTENT_JS_COMPANIES).Include(
                            "~/Componentes/js/Localizaciones.js",
                            "~/Componentes/js/toolbarFiltros.js",
                            "~/Componentes/js/Geoposicion.js",
                            "~/Componentes/js/GridEmplazamientosContactos.js",
                            "~/Componentes/js/FormContactos.js"//,
                            //"~/PaginasComunes/js/Ext.ux.Map.js",
                            //"~/PaginasComunes/js/Ext.ux.GMapPanel.js"
                ));

            bundles.Add(new StyleBundle(Comun.BundleConfigPaths.CONTENT_CSS).IncludeDirectory(
                            "~/CSS/", "*.css", true
                        ));
            #endregion

            


            



            BundleTable.EnableOptimizations = true;
        }
    }
}