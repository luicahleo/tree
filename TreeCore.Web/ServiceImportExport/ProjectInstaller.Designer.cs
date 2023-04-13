
namespace ServiceImportExport
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServiceImportExportProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServiceImportExportInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServiceImportExportProcessInstaller
            // 
            this.ServiceImportExportProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.ServiceImportExportProcessInstaller.Password = null;
            this.ServiceImportExportProcessInstaller.Username = null;
            this.ServiceImportExportProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceImportExportProcessInstaller_AfterInstall);
            // 
            // ServiceImportExportInstaller
            // 
            this.ServiceImportExportInstaller.ServiceName = "ServiceImportExport";
            this.ServiceImportExportInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.ServiceImportExportInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceImportExportInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServiceImportExportProcessInstaller,
            this.ServiceImportExportInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ServiceImportExportProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ServiceImportExportInstaller;
    }
}