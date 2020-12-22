using StoreTest.WebAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoreTest
{
    public partial class HotSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["User"] == null)
            {
                Response.Redirect("Home.aspx");
            }

            DataSet ds = SQLiteHelper.GetHotStoreByUser();

            if (ds != null)
            {
                rpList.DataSource = ds;
                rpList.DataBind();
            }
        }
    }
}