using Newtonsoft.Json.Linq;
using StoreTest.WebAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoreTest
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null)
            {
                string code = Request.QueryString["code"].ToString();

                string responses = GetUserAccessToken(code);

                string[] resAarry = responses.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                string tokenStr = resAarry[0];
                string[] tokenStrAarry = tokenStr.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                string accesstoken = tokenStrAarry[1];

                //var userinfo = GetUserInfoFromGitHub(accesstoken).Result;
                var userinfo = GetUserInfoFromGitHub(accesstoken).GetAwaiter().GetResult();

                if(userinfo!=null && userinfo != "")
                {
                    var jObject = JObject.Parse(userinfo);
                    //var time = jObject["login"].ToString();

                    SQLiteHelper.SaveLoginInfo(jObject["id"].ToString(), jObject["login"].ToString());

                    Session["User"] = jObject;
                    Response.Redirect("MapSearch.aspx");
                }
                else
                {
                    lblError.Text = "登录失败！";
                }
                
            }
        }

        public string GetUserAccessToken(string code)
        {
            string url = "https://github.com/login/oauth/access_token?client_id=your_github_clientid&client_secret=your_github_secret&code=" + code + "&state=0&redirect_uri=http://localhost:9090/home";

            System.Net.HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            if (req == null || req.GetResponse() == null)
                return string.Empty;

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            if (resp == null)
                return string.Empty;

            using (Stream stream = resp.GetResponseStream())
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        //access_token=cc43f01ab7c059ce3b2532ffd97b251cc53b4c0b&scope=&token_type=bearer
        public async Task<string> GetUserInfoFromGitHub(string token)
        {
            // Initialization.  
            //DataTable responseObj = new DataTable();
            string result = "";

            // HTTP GET.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri("https://api.github.com/");

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Foo"));
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
                //client.DefaultRequestHeaders.Add("token_type", "bearer");
                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP GET  
                response = await client.GetAsync("user").ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                    result = response.Content.ReadAsStringAsync().Result;
                    //responseObj = JsonConvert.DeserializeObject<DataTable>(result);  
                }
            }

            return result;
        }
    }
}