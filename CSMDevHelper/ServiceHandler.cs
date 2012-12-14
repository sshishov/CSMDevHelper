using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace CSMDevHelper
{
    class ServiceHandler
    {
        ServiceController Controller;

        ServiceHandler()
        {
            this.Controller = new ServiceController(@"Mitel Customer Service Manager Server");
        }

        ServiceHandler(string serviceName)
        {
            this.Controller = new ServiceController(serviceName);
        }

        void Bounce()
        {
            if (this.Controller.Status == ServiceControllerStatus.Running)
            {
                this.Controller.Stop();
                this.Controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0,1,0));
                //Diagnostics::Trace.WriteLine("STOPPED");
            }
            if (this.Controller.Status == ServiceControllerStatus.Stopped)
            {
                this.Controller.Start();
                this.Controller.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0,1,0));
                //Diagnostics::Trace.WriteLine("STARTED");
            }
        }
    }
}
