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
using DNTPersianUtils.Core;
using System.IO;

namespace CodeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Btn_Tuple_Click(object sender, EventArgs e)
        {
            var fullName = GetFullName(textBox1.Text);
            MessageBox.Show("FirstName: "+fullName.Item1+" LastName: "+fullName.Item2);
        }

        #region Tuple

        public static Tuple<string, string> GetFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new NullReferenceException("name is empty.");

            var nameParts = fullName.Split(' ');

            if (nameParts.Length != 2)
                throw new FormatException("name must contain 'space'");

            return Tuple.Create(nameParts[0], nameParts[1]);
        }

        public static void PrintSelectedTuple()
        {
            var list = new List<Tuple<string, int>>
                          {
                              new Tuple<string, int>("A", 1),
                              new Tuple<string, int>("B", 2),
                              new Tuple<string, int>("C", 3)
                          };

            var item = list.Where(x => x.Item2 == 2).SingleOrDefault();
            if (item != null)
                Console.WriteLine("Selected Item1: {0}, Item2: {1}",
                    item.Item1, item.Item2);
        }

        public static void PrintTuples()
        {
            var tuple1 = new Tuple<int>(12);
            Console.WriteLine("tuple1 contains: item1:{0}", tuple1.Item1);

            var tuple2 = Tuple.Create("Item1", 12);
            Console.WriteLine("tuple2 contains: item1:{0}, item2:{1}",
                tuple2.Item1, tuple2.Item2);

            var tuple3 = Tuple.Create(new DateTime(2010, 5, 6), "Item2", 20);
            Console.WriteLine("tuple3 contains: item1:{0}, item2:{1}, item3:{2}",
                tuple3.Item1, tuple3.Item2, tuple3.Item3);
        }

        public static void Tuple8()
        {
            var tup =
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int>>
                    (1, 2, 3, 4, 5, 6, 7, new Tuple<int, int>(8, 9));

            Console.WriteLine("tup.Rest Item1: {0}, Item2: {1}",
                    tup.Rest.Item1, tup.Rest.Item2);
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //this.Text= System.Enum.IsDefined(typeof(ActivityTypeEnum),Convert.ToInt32(textBox1.Text)) ? ((ActivityTypeEnum) Convert.ToInt32(textBox1.Text)).ToString().Replace("___", ")").Replace("__", "(").Replace("_", " ") : "نا مشخص";


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Btn_SqlClass_Click(object sender, EventArgs e)
        {
           // var newSqlClass = new SqlClass("AgahTestDB");


        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text= new StringManipulator().StringManipulatorBy(textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            textBox1.Text = dt.ToShortPersianDateTimeString();
            textBox2.Text = textBox1.Text.ToGregorianDateTime().ToString();
            MessageBox.Show(
                $@"GetPersianWeekDayName    {dt.GetPersianWeekDayName()}{Environment.NewLine}
                    GetPersianMonth {dt.GetPersianMonth()}{Environment.NewLine}
                    ToShortPersianDateString    {dt.ToShortPersianDateString()}{Environment.NewLine}
                    NormalizePersianText    {"'سلام عزیزم'.".NormalizePersianText(PersianNormalizers.ConvertEnglishQuotes)}{Environment.NewLine}
                    '1397/01/00' IsValidPersianDateTime  {"1397/01/00".IsValidPersianDateTime()}{Environment.NewLine}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var drives = DriveInfo.GetDrives().Where(x => x.IsReady).ToList();

            foreach (var drive in drives)
            {
                if (drive.Name == (Path.GetPathRoot(Environment.SystemDirectory)))
                {
                    this.Text  = Convert.ToChar(Path.GetPathRoot(Environment.SystemDirectory).Substring(0, 1)).ToString();
                }
                listBox1.Items.Add(drive.Name);
                textBox2.Text = Path.GetPathRoot(Environment.SystemDirectory);
            }
            }
        }
}
