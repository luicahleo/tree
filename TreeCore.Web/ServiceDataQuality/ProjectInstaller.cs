using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;

namespace ServiceDataQuality
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private string GetConfigurationValue(string key)
        {
            Assembly service = Assembly.GetAssembly(typeof(ServiceDataQuality));
            Configuration config = ConfigurationManager.OpenExeConfiguration(service.Location);
            if (config.AppSettings.Settings[key] != null)
            {
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                throw new System.IndexOutOfRangeException
                    ("Settings collection does not contain the requested key: " + key);
            }
        }
        public ProjectInstaller()
        {
            InitializeComponent();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            this.ServiceDataQualityInstaller.ServiceName = GetConfigurationValue("ServiceName") + "_(BD-" + version + ")";
            this.ServiceDataQualityInstaller.DisplayName = GetConfigurationValue("ServiceName") + "_(BD-" + version + ")";

        }

        private void ServiceDataQualityProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void ServiceDataQualityInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
