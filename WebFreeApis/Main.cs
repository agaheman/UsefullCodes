using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WebFreeApis
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        IsearchResult isearchResult;
        HttpClient httpClient;
        HttpRequestMessage requestMessage;
        string responseAsString;

        static Uri IsearchApiUri = new Uri( "https://search.bahesab.ir/Isearch/");
        static Uri ReferrerUri = new Uri("https://www.bahesab.ir/map/geographic/");

        private void button1_Click(object sender, EventArgs e)
        {

            string requestJson = "string_o: {\"a\":\"رشت\"}";
            SearchCall( IsearchApiUri, requestJson );



           
            //textBox1.Text = GetCityList( "رشت" ).Result.v;
            Debugger.Break();
        }
        private async void SearchCall(Uri uri,String stringJson)
        {
            httpClient = new HttpClient();

            requestMessage = new HttpRequestMessage( HttpMethod.Post, uri );
            requestMessage.Content = new StringContent( stringJson, Encoding.UTF8, "application/json" );

            requestMessage.Headers.Add( "Referer", "https://www.bahesab.ir/map/geographic/" );
            requestMessage.Headers.Add( "Content-Type", "application/json; charset=utf-8" );

            HttpResponseMessage response = await httpClient.SendAsync( requestMessage );

            responseAsString = await response.Content.ReadAsStringAsync();

            isearchResult = JsonConvert.DeserializeObject<IsearchResult>( responseAsString );
        }

        public class IsearchResult
        {
            public string v { get; set; }
            public int c { get; set; }
        }

    }
}
