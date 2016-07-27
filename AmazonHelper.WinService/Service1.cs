using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AmazonHelper.WinService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Console.WriteLine("Service started....");
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10000; // 60 seconds
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            using (var textWriter = new StreamWriter(@"D:\\1.txt", true))
            {
                textWriter.WriteLine(string.Format("New line appended: {0}", DateTime.Now));
            }
        }

        protected override void OnStop()
        {
            Console.WriteLine("Service stopped....");
        }
    }
}
