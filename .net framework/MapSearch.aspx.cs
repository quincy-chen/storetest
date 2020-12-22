using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

namespace StoreTest
{
    public partial class MapSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Home.aspx");
            }
            else
            {
                var jObject = Session["User"] as JObject;

                if (jObject != null && jObject["id"] != null)
                {
                    hdUser.Value = jObject["id"].ToString();
                }
            }
        }
    }
}