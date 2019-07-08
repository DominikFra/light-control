using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Formatting;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace LightService
{
    public partial class LightService : ServiceBase
    {
        private const string eventLogSource = "LightService";
        public LightService()
        {
            InitializeComponent();
        }

        public void Start()
        {
            this.OnStart(new string[0]);
        }
        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("OnStart");
            var config = new HttpSelfHostConfiguration("http://localhost:8080");
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("OnStop");
        }

        protected override void OnContinue()
        {
            base.OnContinue();
            eventLog1.WriteEntry("OnContinue");
        }
    }
}
