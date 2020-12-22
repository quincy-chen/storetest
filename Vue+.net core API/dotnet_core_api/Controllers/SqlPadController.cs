using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using ApiCore.Dal.IDal;
using ApiCore.Helper;
using ApiCore.Model;
using ApiCore.Model.MyModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace ApiCore.Controllers
{
    [EnableCors("AllowSameDomain")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SqlPadController : ControllerBase
    {
        //public SqlPadController(ISqlPadDal sqlPadDal)
        //{
        //    SqlPadDal = sqlPadDal;
        //}

        //private ISqlPadDal SqlPadDal { get; }
        [HttpPost]
        public ActionResult<object> SaveNearestStore(SearchQueryModel queryModel)
        {
            var searchStoreResultModel = new SearchStoreResultModel();
            string storeStr = queryModel.stores;
            
            List<Store> store = DeserializeJson<List<Store>>(storeStr);

            string result = "";

            if (store.Count > 0)
            {
                var storeOrderBy = store.OrderBy(x => x.distance);
                result = JsonConvert.SerializeObject(storeOrderBy);

                try
                {
                    // 存最近的门店到数据库
                    SQLiteHelper.SaveSearchResule(storeOrderBy.ToList()[0]);
                }
                catch
                {

                }
            }

            searchStoreResultModel.DataRes = result;

            return searchStoreResultModel;
        }
        /// <summary>
        /// 将实体类序列化为JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private string SerializeJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        private T DeserializeJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
        
        private long ConvertToTimeStamp(DateTime time)
        {
            DateTime dateTime = new DateTime(1993, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            return (long)(time.AddHours(-8) - dateTime).TotalMilliseconds;
        }

        private string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        
        [HttpPost]
        public ActionResult<object> GetAuth(LoginQueryModel queryModel)
        {
            var loginResultModel = new LoginResultModel();

            LoginUser loginUser = new LoginUser();

            string responses = GetUserAccessToken(queryModel.code);

            string[] resAarry = responses.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            string tokenStr = resAarry[0];
            string[] tokenStrAarry = tokenStr.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

            string accesstoken = tokenStrAarry[1];

            //var userinfo = GetUserInfoFromGitHub(accesstoken).Result;
            var userinfo = GetUserInfoFromGitHub(accesstoken).GetAwaiter().GetResult();

            if (userinfo != null && userinfo != "")
            {
                var jObject = JObject.Parse(userinfo);

                loginUser.UserName = jObject["login"].ToString();
                loginUser.UserLoginID = jObject["id"].ToString();

                SQLiteHelper.SaveLoginInfo(jObject["id"].ToString(), jObject["login"].ToString());                
            }

            loginResultModel.DataRes = JsonConvert.SerializeObject(loginUser);

            return loginResultModel;
        }
        private string GetUserAccessToken(string code)
        {
            string url = "https://github.com/login/oauth/access_token?client_id=your_client_id&client_secret=your_client_secret&code=" + code + "&state=0&redirect_uri=http://localhost:9090/home";

            System.Net.HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            //Stream newReqStream = req.GetRequestStream();
            //newReqStream.Close();

            //if (req == null || req.GetResponse() == null)
            //    return string.Empty;

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
        private async Task<string> GetUserInfoFromGitHub(string token)
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
        [HttpPost]
        public ActionResult<object> GetHotStoreList(JtDbQueryModel queryModel)
        {
            int pageIndex = queryModel.PageIndex;
            int pageSize = queryModel.PageSize;

            var resModel = new HotStoreResultModel();
            DataSet ds = SQLiteHelper.GetHotStoreByUser();

            List<TopStore> tss = new List<TopStore>();

            for (int i= 0;i < ds.Tables[0].Rows.Count; i++){
                TopStore ts = new TopStore();
                ts.UserLoginID = ds.Tables[0].Rows[i]["UserLoginID"].ToString();
                ts.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                ts.NearestStoreTitle = ds.Tables[0].Rows[i]["NearestStoreTitle"].ToString();
                ts.NearestStoreAddress = ds.Tables[0].Rows[i]["NearestStoreAddress"].ToString();
                ts.StoreSreachedCount = ds.Tables[0].Rows[i]["StoreSreachedCount"].ToString();

                tss.Add(ts);
            }

            var resList = tss
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToList();

            resModel.DataCount = ds.Tables[0].Rows.Count;

            resModel.DataRes = JsonConvert.SerializeObject(tss);

            return resModel;
        }
         
    }
}