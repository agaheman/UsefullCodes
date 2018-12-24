using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IISAdministrationAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<AppPool> listAppPool = new List<AppPool>();
            var responseAppPools = GET2("https://46.4.71.227:55539/#/api");
            listAppPool = JsonConvert.DeserializeObject<List<AppPool>>(responseAppPools);
        }

        // Returns JSON string
        string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }

            //catch (WebException ex)
            //{
            //    WebResponse errorResponse = ex.Response;
            //    using (Stream responseStream = errorResponse.GetResponseStream())
            //    {
            //        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            //        String errorText = reader.ReadToEnd();
            //        // log errorText
            //    }
            //    throw;
            //}
            catch(NullReferenceException errr)
            {
                var erasd = errr;
                throw;
            }
            catch (Exception err)
            {
                var errr = err;
                throw;
            }
        }

        string GET2(string url)
        {
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            var StatusCode = WebResp.StatusCode;
            var Server = WebResp.Server;


            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            return jsonString;
        }
    }
}
