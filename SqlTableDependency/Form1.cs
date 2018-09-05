using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//-------------------------------
using AgahClassLibrary;
using TableDependency;
using TableDependency.EventArgs;
using TableDependency.SqlClient;

namespace SqlTableDependency
{
    public partial class FormSqlTableDependency : Form
    {
        public FormSqlTableDependency()
        {
            InitializeComponent();
        }

        private class Person
        {
            public int PersonId { get; set; }
            public int NationalNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        static string _connectionString = "Data Source=.;Initial Catalog=AgahTestDB;User ID=sa;Password=Sonylive1";
        //SqlTableDependency 
        //https://www.nuget.org/packages/SqlTableDependency/
        private static ModelToTableMapper<Person> _mapper;

        private SqlTableDependency<Person> _tableDependency;

        private void WatchTable()
        {
            _mapper = new ModelToTableMapper<Person>();

            _mapper.AddMapping(p => p.PersonId, "PersonId");
            _mapper.AddMapping(p => p.NationalNumber, "NationalNumber");
            _mapper.AddMapping(p => p.FirstName, "FirstName");
            _mapper.AddMapping(p => p.LastName, "LastName");

            if (DoesUserHavePermission())
            {
                _tableDependency = new SqlTableDependency<Person>(_connectionString, "Persons", _mapper);


                _tableDependency.OnChanged += OnNotificationReceived;
                _tableDependency.Start();
            }
            

        }

        private void OnNotificationReceived(object sender, RecordChangedEventArgs<Person> e)
        {
            string text = "Database: " + e.Database + Environment.NewLine +
                          "Server: " + e.Server + Environment.NewLine +
                          "ChangeType: " + e.ChangeType + Environment.NewLine +
                          "Entity.PersonId: " + e.Entity.PersonId + Environment.NewLine +
                          "Entity.NationalNumber: " + e.Entity.NationalNumber + Environment.NewLine +
                          "Entity.FirstName: " + e.Entity.FirstName + Environment.NewLine +
                          "Entity.LastName: " + e.Entity.LastName + Environment.NewLine +
                          "Sender: " + e.Sender + Environment.NewLine;

            Notification noty = new Notification();
            noty.DisplayNotify(new System.Drawing.Icon(System.IO.Path.GetFullPath(@"C:\Program Files (x86)\Cisco\Cisco AnyConnect Secure Mobility Client\res\attention.ico")), " :تغییر در سرور" + e.Server, " :تغییر در دیتابیس" + e.Database, text);
        }

        private void FormSqlTableDependency_Load(object sender, EventArgs e)
        {
            WatchTable();
        }

        private void FormSqlTableDependency_FormClosing(object sender, FormClosingEventArgs e)
        {
            _tableDependency.Stop();
        }

        private bool DoesUserHavePermission()
        {
            try
            {
                SqlClientPermission clientPermission = new SqlClientPermission(PermissionState.Unrestricted);

                // will throw an error if user does not have permissions
                clientPermission.Demand();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
