using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoreTest
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                var jObject = Session["User"] as JObject;

                if (jObject != null && jObject["login"] != null)
                {
                    lblUser.Text = jObject["login"].ToString();
                }
            }
        }

        protected void lbLogout_Click(object sender, EventArgs e)
        {
            Session["User"] = null;
            Session.Clear();
            Response.Redirect("Home.aspx");
        }
    }
}