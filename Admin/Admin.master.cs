using System;
using System.IO;
using System.Text;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;

public partial class Admin_Admin : System.Web.UI.MasterPage
{
    AdminManager objLogin = new AdminManager();

    protected StringBuilder strmenu = new StringBuilder();
    string cookiedvalue = "";
    string cookieid = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["AdminId"] == null || Convert.ToInt32(Session["AdminId"]) == 0 || Convert.ToString(Session["AdminId"]) == "")
        {
            Response.Redirect("default.aspx");
        }
        if (Request.Cookies["menunid"] != null)
        {
            cookieid = Convert.ToString(Request.Cookies["menunid"].Value);
            cookiedvalue = Convert.ToString(Request.Cookies["menuname"].Value);
        }
        //BindLeftMenu();

        //---------------Welcome--------------------

        //if (Session["superadmin"] == "superadmin")
        //{
        //    ltrprofile.Text = Convert.ToString(Session["AdminName"]) + " - " + "Super Admin";
        //}
        //else
        //{
        //    ltrprofile.Text = Convert.ToString(Session["AdminName"]) + " - " + "Admin";
        //}

        ltrprofile.Text = Convert.ToString(Session["AdminName"]) + " - " + Convert.ToString(Session["UserType"]);

        objLogin.adminid = Convert.ToInt32(Session["adminId"]);
        string strName = string.Empty;
        strName = objLogin.getName();

        //ltrwelcome.Text = "<li>Hello " + strName + "</li>";
        //ltrtime.Text = DateTime.Now.ToLongDateString();
        strmenu.Append(CommonFunctions.GetFileContents(Server.MapPath("../resources/AdminTypeMenu/") + Convert.ToString(Session["UserTypeId"]) + ".html"));
    }

}
