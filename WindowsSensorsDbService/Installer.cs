using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace WindowsSensorsDbService
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public Installer()
        {
            InitializeComponent();

            // Initialize ServiceProcessInstaller
            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;

            // Initialize ServiceInstaller
            serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = "WindowsSensorsDbService";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // Add installers to collection
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
