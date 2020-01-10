using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("Sql");

            //app.UseHangfireDashboard();
            //app.UseHangfireServer();
        }
    }
}
