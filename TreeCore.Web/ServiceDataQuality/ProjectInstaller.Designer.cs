
namespace ServiceDataQuality
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServiceDataQualityProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServiceDataQualityInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServiceDataQualityProcessInstaller
            // 
            this.ServiceDataQualityProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.ServiceDataQualityProcessInstaller.Password = null;
            this.ServiceDataQualityProcessInstaller.Username = null;
            this.ServiceDataQualityProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceDataQualityProcessInstaller_AfterInstall);
            // 
            // ServiceDataQualityInstaller
            // 
            this.ServiceDataQualityInstaller.ServiceName = "ServiceDataQuality";
            this.ServiceDataQualityInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.ServiceDataQualityInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceDataQualityInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServiceDataQualityProcessInstaller,
            this.ServiceDataQualityInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ServiceDataQualityProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ServiceDataQualityInstaller;
    }
}