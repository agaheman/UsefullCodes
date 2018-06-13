using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Threading;
using AgahClassLibrary;

namespace AgahClassLibrary
{
    public class ServiceManagement:ServiceBase
    {
        private Thread m_thread = null;
        private AutoResetEvent m_done = new AutoResetEvent(false);

        public ServiceManagement(string serviceName,string onStopMessage)
        {
            if (string.IsNullOrEmpty(serviceName))
                throw new ArgumentException("Its not givven.", nameof(serviceName));

            if (!DoesServiceExist(serviceName))
                throw new ArgumentException("Service does not exist.", nameof(serviceName));

            _serviceController = new ServiceController(serviceName);
            _onStopMessage = onStopMessage;
        }


        private readonly string _onStopMessage;
        private readonly ServiceController _serviceController;

        public ServiceControllerStatus ServiceControllerStatus => _serviceController.Status;

        private static bool DoesServiceExist(string serviceName, string machineName)
        {
            ServiceController[] services = ServiceController.GetServices(machineName);
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);
            return service != null;
        }
        private static bool DoesServiceExist(string serviceName)
        {
            var services = ServiceController.GetServices();
            
            return services.Any(s => s.ServiceName == serviceName);
        }
        protected override void OnStop()
        {
            var noty = new Notification();
            noty.DisplayNotify(_onStopMessage, "TipTitle", "TipText");
        }

        protected override void OnStart(string[] args)
        {
            var noty = new Notification();
            noty.DisplayNotify("Started Successfully.", "TipTitle", "TipText");
            base.OnStart(args);
        }

        public void StartService()
        {
            _serviceController.Start();
        }
    }
}
