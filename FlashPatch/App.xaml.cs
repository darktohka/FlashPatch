using System.Net;
using System.Windows;

namespace FlashPatch {
    public partial class App : Application {

        private void Application_Startup(object sender, StartupEventArgs e) {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
    }
}
