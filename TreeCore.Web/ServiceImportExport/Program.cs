using System.ServiceProcess;
using System.Windows.Input;
using System;
using TreeCore;

namespace ServiceImportExport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            //Log4Net configure
            log4net.Config.XmlConfigurator.Configure();
            DirectoryMapping.ChangeLogFileName("InfoAp", DirectoryMapping.GetServiceImportExportLog4NetDirectoryINFO());
            DirectoryMapping.ChangeLogFileName("ErrorAp", DirectoryMapping.GetServiceImportExportLog4NetDirectoryERROR());


#if SERVICE_AS_PROGRAM
            var myService = new ServiceImportExport();
            if (Environment.UserInteractive)
            {
                System.Threading.Thread STAThread = new System.Threading.Thread(() => {

                    Console.WriteLine("Starting service...");
                   // myService.ServiceSettingsfunc();
                    myService.Start();
                    Console.WriteLine("Service is running.");

                    bool execute = true;
                    while (execute)
                    {
                        //if (KeyBoard.IsKeyDown(Key.PrintScreen))
                        //{
                        //    myService.CallEvent();
                        //}
                        //else
                        //if (Keyboard.IsKeyDown(Key.Escape))
                        //{
                        //    execute = false;
                        //}

                        // respirar
                        System.Threading.Thread.Sleep(10);
                    }
                    Console.WriteLine("Stopping service...");
                    myService.Stop();
                    Console.WriteLine("Service stopped.");
                });

                STAThread.SetApartmentState(System.Threading.ApartmentState.STA);

                STAThread.Start();

                STAThread.Join();
            }
            else
#endif
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new ServiceImportExport()
                };
                ServiceBase.Run(ServicesToRun);
            }
                
        }
    }
}
