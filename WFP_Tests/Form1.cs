using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFP_Tests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static System.Data.Sql.SqlDataSourceEnumerator sqlInstance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
        System.Data.DataTable dt = sqlInstance.GetDataSources();

        private static string DisplayData(System.Data.DataTable table)
        {
            string output = "";
            foreach (System.Data.DataRow row in table.Rows)
            {
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    output += col.ColumnName + "\t" + row[col] + Environment.NewLine;
                }
                output += "============================";
                
            }
            return output;
        }


          
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(DisplayData(dt));

            //DataClasses1DataContext dc = new DataClasses1DataContext();
            //System.Data.Linq.Table<Person> personTb = dc.GetTable<Person>();



        }


    }


}
