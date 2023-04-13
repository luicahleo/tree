using System;
using System.ServiceProcess;
using TreeCore;

namespace ServiceDataQuality
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
            DirectoryMapping.ChangeLogFileName("InfoAp", DirectoryMapping.GetServiceDataQualityLog4NetDirectoryINFO());
            DirectoryMapping.ChangeLogFileName("ErrorAp", DirectoryMapping.GetServiceDataQualityLog4NetDirectoryERROR());


#if SERVICE_AS_PROGRAM
            var myService = new ServiceDataQuality();
            if (Environment.UserInteractive)
            {
                System.Threading.Thread STAThread = new System.Threading.Thread(() => {

                    Console.WriteLine("Starting service...");
                    //myService.ServiceSettingsfunc();
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
                new ServiceDataQuality()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
