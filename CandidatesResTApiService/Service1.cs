using InterLoinkClass.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CandidatesResTApiService
{
    public partial class Service1 : ServiceBase
    {
        Thread listen;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listen = new Thread(new ThreadStart(ProcessRequest));
            listen.Start();
        }

        protected override void OnStop()
        {
            listen.Abort();
        }

        private void ProcessRequest()
        {

            while (true)
            {
                try
                {
                    RequestProcessor proc = new RequestProcessor();
                    proc.ProcessTraffic();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
