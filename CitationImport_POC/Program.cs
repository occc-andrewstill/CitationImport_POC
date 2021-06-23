using System;
using Topshelf;
using System.Configuration;
using CitationImport_POC.BLL;

namespace CitationImport_POC
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ConfigurationManager.AppSettings["Environment"] == "Local")
            {
                TaskManager tm = new TaskManager();
                tm.Run();
            }
            else
            {
                HostFactory.Run(x =>
                {
                    x.Service<ServiceManager>(s =>
                    {
                        s.ConstructUsing(FileProcess => new ServiceManager());
                        s.WhenStarted(FileProcess => FileProcess.Start());
                        s.WhenStopped(FileProcess => FileProcess.Stop());
                    });

                    x.StartAutomatically();

                    x.RunAsLocalSystem();

                    x.EnableServiceRecovery(recoverOption =>
                    {
                        recoverOption.RestartService(1);
                        recoverOption.RestartService(5);
                        recoverOption.TakeNoAction();
                    });

                    x.SetServiceName("OCCC_CitationImport_POC");
                    x.SetDisplayName("OCCC_CitationImport_POC");
                    x.SetDescription("This service imports citation information from internal clerk DB into Odyssey.");
                });
            }
        }
    }
}
