using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace UIAService
{
    public partial class UIAS : ServiceBase
    {
        public UIAS()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Service started");
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Service stopped");
        }
    }
}
