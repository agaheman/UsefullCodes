using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace IISAdministration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public class IISApp
        {
            public string AppName {get; set;}
            public string AppPath { get; set; }
            public string PhysicalPath { get; set; }
            public bool ServiceAutoStartEnabled { get; set; }
            public bool PreloadEnabled { get; set; }
        }
        public class IISSite
        {
            public string SiteName { get; set; }
            public string LogFileDirectory { get; set; }

            public List<IISApp> AppList { get; set; }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerManager manager = new ServerManager();
            foreach (var pool in manager.ApplicationPools)
            {
                textBox1.Text += pool.Name + "\t"+ pool.ManagedRuntimeVersion +"\t" + pool.WorkerProcesses.Methods.Count.ToString() + Environment.NewLine;
            }
            textBox1.Text += "___________________________";
            textBox1.Text += Environment.NewLine;

            //var Sites = new List<IISSite>();

            //foreach (var site in manager.Sites)
            //{
            //    var tempSite = new IISSite
            //    {
            //        SiteName = site.Name,
            //        LogFileDirectory = site.LogFile.Directory
            //    };

            //    foreach (var app in site.Applications)
            //    {
            //        var tempApp = new IISApp
            //        {
            //            AppPath = app.Path,
            //            PreloadEnabled = (bool)app.Attributes["preloadEnabled"].Value,
            //            ServiceAutoStartEnabled = (bool)app.Attributes["serviceAutoStartEnabled"].Value
            //        };

            //        foreach (var vd in app.VirtualDirectories)
            //        {
            //            tempApp.AppName = vd.ToString();
            //            tempApp.PhysicalPath = vd.PhysicalPath;
            //        }
            //        tempSite.AppList.Add(tempApp);
            //    }
            //    Sites.Add(tempSite);
            //}

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            ServerManager manager2 = new ServerManager();
            IEnumerable<Request> requests = manager2.ApplicationPools
                               .Where(applicationPool => applicationPool.Name == "TransportInsurance")
                               .SelectMany(pool => pool.WorkerProcesses)
                               .SelectMany(workerProcesses => workerProcesses.GetRequests(int.Parse(textBox2.Text)).ToList());

            dataGridView1.DataSource = requests;
        }
        //ZipFile.CreateFromDirectory(startPath, zipDestinationPath + @"\LogFiles");

    }
}
