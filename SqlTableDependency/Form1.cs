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
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        static string _connectionString = "Data Source=.;Initial Catalog=Agah.TestDB;User ID=Agah;Password=Openit14";

        private static ModelToTableMapper<Person> _mapper;

        private SqlTableDependency<Person> _tableDependency;

        private void WatchTable()
        {
            _mapper = new ModelToTableMapper<Person>();

            _mapper.AddMapping(p => p.Id, "Id");
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
                          "Entity.Id: " + e.Entity.Id + Environment.NewLine +
                          "Entity.FirstName: " + e.Entity.FirstName + Environment.NewLine +
                          "Entity.LastName: " + e.Entity.LastName + Environment.NewLine +
                          "Sender: " + e.Sender + Environment.NewLine;

            Notification noty = new Notification();
            noty.DisplayNotify(new System.Drawing.Icon(System.IO.Path.GetFullPath(@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Setup\setup.ico")), " :تغییر در سرور" + e.Server, " :تغییر در دیتابیس" + e.Database, text);
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
