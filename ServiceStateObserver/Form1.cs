using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgahClassLibrary;


namespace ServiceStateObserver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        private void Btn_StartObserver_Click(object sender, EventArgs e)
        {
            var serviceManagement = new ServiceManagement(txt_ServiceName.Text,"هی وای من. سرویس فلان استاپ شد.");

            this.Text = serviceManagement.ServiceControllerStatus.ToString();
        }
    }
}
