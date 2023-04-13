using CapaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;


namespace TreeCore
{
    public static class DirectoryMapping
    {
        private static string _rootDirectory = "";
        private static string _rootImaDirectory = "";
        private static string _rootTempDirectory = "";
        private static string _relativeTempDirectory = "";

        static DirectoryMapping()
        {
#if SERVICESETTINGS
#elif TREEAPI
#else
            _rootDirectory = HttpContext.Current.Server.MapPath("~/");
            _rootImaDirectory = HttpContext.Current.Server.MapPath("~/ima/");
            _rootTempDirectory = HttpContext.Current.Server.MapPath("~/_temp/");
#endif

            _relativeTempDirectory = "_temp/";
        }


        #region Log4Net
        private static string GetLog4NetDirectory()
        {
#if SERVICESETTINGS
            string dir = System.Configuration.ConfigurationManager.AppSettings["RutaRaiz"];
#elif TREEAPI
            string dir = TreeAPI.Properties.Settings.Default.RutaRaiz;
#else
            string dir = TreeCore.Properties.Settings.Default.RutaRaiz;
#endif


            if (!Path.IsPathRooted(dir))
            {

#if SERVICESETTINGS
                dir = Path.Combine(_rootDirectory, System.Configuration.ConfigurationManager.AppSettings["RutaRaiz"]);
#elif TREEAPI
                dir = Path.Combine(_rootDirectory, TreeAPI.Properties.Settings.Default.RutaRaiz);
#else
                dir = Path.Combine(_rootDirectory, TreeCore.Properties.Settings.Default.RutaRaiz);
#endif

            }

            dir = Path.Combine(dir, "Logs");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        #region Web
        private static string GetWebLog4NetDirectory()
        {
            string dir = GetLog4NetDirectory();
            dir = Path.Combine(dir, "TreeCore");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetWebLog4NetDirectoryINFO()
        {
            string dir = GetWebLog4NetDirectory();
            dir = Path.Combine(dir, "INFO");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetWebLog4NetDirectoryERROR()
        {
            string dir = GetWebLog4NetDirectory();
            dir = Path.Combine(dir, "ERROR");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        #endregion

        #region API
        private static string GetAPILog4NetDirectory()
        {
            string dir = GetLog4NetDirectory();
            dir = Path.Combine(dir, "TreeAPI");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetAPILog4NetDirectoryINFO()
        {
            string dir = GetAPILog4NetDirectory();
            dir = Path.Combine(dir, "INFO");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetAPILog4NetDirectoryERROR()
        {
            string dir = GetAPILog4NetDirectory();
            dir = Path.Combine(dir, "ERROR");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        #endregion

        #region Services

        #region Import Export
        private static string GetServiceImportExportLog4NetDirectory()
        {
            string dir = GetLog4NetDirectory();
            dir = Path.Combine(dir, "ServiceImportExport");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetServiceImportExportLog4NetDirectoryINFO()
        {
            string dir = GetServiceImportExportLog4NetDirectory();
            dir = Path.Combine(dir, "INFO");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetServiceImportExportLog4NetDirectoryERROR()
        {
            string dir = GetServiceImportExportLog4NetDirectory();
            dir = Path.Combine(dir, "ERROR");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        #endregion

        #region Data Quality
        private static string GetDataQualityLog4NetDirectory()
        {
            string dir = GetLog4NetDirectory();
            dir = Path.Combine(dir, "ServiceDataQuality");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetServiceDataQualityLog4NetDirectoryINFO()
        {
            string dir = GetDataQualityLog4NetDirectory();
            dir = Path.Combine(dir, "INFO");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetServiceDataQualityLog4NetDirectoryERROR()
        {
            string dir = GetDataQualityLog4NetDirectory();
            dir = Path.Combine(dir, "ERROR");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        #endregion

        #endregion

        public static string GetLogDirectory(string sDir)
        {

            if (sDir.Length > 0)
            {
                if (sDir[sDir.Length - 1] != '/' &&
                   sDir[sDir.Length - 1] != '\\')
                    sDir += '/';
            }

            if (!Directory.Exists(sDir))
                Directory.CreateDirectory(sDir);

            return sDir;
        }
        public static bool ChangeLogFileName(string appenderName, string newFilename)
        {
            var rootRepository = log4net.LogManager.GetRepository();
            foreach (var appender in rootRepository.GetAppenders())
            {
                if (appender.Name.Equals(appenderName) && appender is log4net.Appender.FileAppender)
                {
                    var fileAppender = appender as log4net.Appender.FileAppender;
                    fileAppender.File = newFilename + "\\";
                    fileAppender.ActivateOptions();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region DATA
        private static string GetDataDirectory()
        {
#if SERVICESETTINGS
            string dir = System.Configuration.ConfigurationManager.AppSettings["RutaRaiz"];
#elif TREEAPI
            string dir = TreeAPI.Properties.Settings.Default.RutaRaiz;
#else
            string dir = TreeCore.Properties.Settings.Default.RutaRaiz;
#endif


            if (!Path.IsPathRooted(dir))
            {

#if SERVICESETTINGS
                dir = Path.Combine(_rootDirectory, System.Configuration.ConfigurationManager.AppSettings["RutaRaiz"]);
#elif TREEAPI
                dir = Path.Combine(_rootDirectory, TreeAPI.Properties.Settings.Default.RutaRaiz);
#else
                dir = Path.Combine(_rootDirectory, TreeCore.Properties.Settings.Default.RutaRaiz);
#endif

            }

            dir = Path.Combine(dir, "Data");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetDocumentDirectory()
        {
            string dir = GetDataDirectory();
            dir = Path.Combine(dir, "Documents");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetIconoOperadorDirectory()
        {
            string dir = GetDataDirectory();
            dir = Path.Combine(dir, "icoOperators");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetImagenUsuarioDirectory()
        {
            string dir = GetDataDirectory();
            dir = Path.Combine(dir, "UserImage");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetImagenClienteDirectory()
        {
            string dir = GetDataDirectory();
            dir = Path.Combine(dir, "ClientImage");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetSettingMigrationDirectory()
        {
            string dir = GetDataDirectory();
            dir = Path.Combine(dir, "SettingMigration");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        #region ImportExport

        public static string GetImportExportDirectory()
        {
            string dir = GetDataDirectory();
            dir = Path.Combine(dir, "ImportExport");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetImportDirectory()
        {
            string dir = GetImportExportDirectory();
            dir = Path.Combine(dir, "Import");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetExportDirectory()
        {
            string dir = GetImportExportDirectory();
            dir = Path.Combine(dir, "Export");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetImportFilesDirectory()
        {
            string dir = GetImportDirectory();
            dir = Path.Combine(dir, "Files");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetImportLogDirectory()
        {
            string dir = GetImportDirectory();
            dir = Path.Combine(dir, "Log");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
        public static string GetImportDocumentsDirectory()
        {
            string dir = GetImportDirectory();
            dir = Path.Combine(dir, "DocumentosCarga");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        #endregion

        #region Templates
        public static void CopyTemplates()
        {
            string sourcePath = System.AppDomain.CurrentDomain.BaseDirectory;
            sourcePath = Path.Combine(sourcePath, "ima//mailtemplates");

            string targetPath = GetTemplatesDirectory();

            string fileName = "";
            string destFile = "";

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }

        }
        public static string GetTemplatesDirectory()
        {
            string dir = GetDataDirectory();
            dir = Path.Combine(dir, "Templates");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        #endregion

        #endregion

        #region Temp

        public static string GetTempDirectory(string DocumentoTipo)
        {
            string dir = _rootTempDirectory;
            dir = Path.Combine(_rootTempDirectory, DocumentoTipo);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }
        public static string GetTemplatesTempDirectory()
        {
            string dir = _rootTempDirectory;
            dir = Path.Combine(_rootTempDirectory, "Templates\\");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }
        public static string GetIconoOperadorTempDirectory()
        {
            string dir = _rootTempDirectory;
            dir = Path.Combine(_rootTempDirectory, "icoOperadores");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }
        public static string GetImagenUsuarioTempDirectory()
        {
            string dir = _rootTempDirectory;
            dir = Path.Combine(_rootTempDirectory, "imgUsuarios");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }
        public static string GetImagenClienteTempDirectory()
        {
            string dir = _rootTempDirectory;
            dir = Path.Combine(_rootTempDirectory, "imgCliente");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }
        public static string GetFileTemplatesTempDirectoryRelative(string file)
        {
            string fileName = Path.GetFileName(file);
            string dir = "../" + _relativeTempDirectory;
            dir = "../../" + _relativeTempDirectory + "Templates/" + fileName;

            return dir;
        }
        public static string GetIconoOperadorTempDirectoryRelative()
        {
            string dir = _relativeTempDirectory;
            dir = Path.Combine(_relativeTempDirectory, "icoOperadores");
            return dir;
        }
        public static string GetImagenUsuarioTempDirectoryRelative()
        {
            string dir = _relativeTempDirectory;
            dir = Path.Combine(_relativeTempDirectory, "imgUsuarios");
            return dir;
        }
        public static string GetImagenClienteTempDirectoryRelative()
        {
            string dir = _relativeTempDirectory;
            dir = Path.Combine(_relativeTempDirectory, "imgCliente");
            return dir;
        }
        public static string GetFileTempDirectoryRelative(string DocumentoTipo, string file)
        {
            string fileName = Path.GetFileName(file);
            string dir = "../" + _relativeTempDirectory;
            dir = "../../" + _relativeTempDirectory + DocumentoTipo + "/" + fileName;

            return dir;
        }

        #endregion

    }

}