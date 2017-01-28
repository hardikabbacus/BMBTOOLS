using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Default : System.Web.UI.Page
{
    AdminManager objlogin = new AdminManager();
    String PreviousPageName = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminId"] != null)
        {
            if (Session["adminId"] != null)
            {
                if (Convert.ToInt32(Session["adminId"]) > 0 && Convert.ToString(Session["adminId"]) != "")
                {
                    if (Request.UrlReferrer != null)
                    {
                        Response.Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        Response.Redirect("home.aspx");
                    }
                }
            }
        }
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["msg"] == "logout")
            {
                ltrmessage.Text = "You have successfully logged Out.";
            }
            else
            {
                if (Session["adminId"] != null && (Convert.ToInt32(Session["adminId"]) == 0 || Convert.ToString(Session["adminId"]) == ""))
                {
                    ltrmessage.Text = "Your login sesion has been expired.<br/> Please login again.";
                }
            }
            txtemail.Focus();
        }
    }
    protected void btnlogin_Click(object sender, EventArgs e)
    {
        //if (txtcaptchacode.Text == HttpContext.Current.Session("captchacode")) {
        bool Authenticated = false;
        //Authenticated = AuthenticationMethod(Server.HtmlDecode(CommonFunctions.encryptString(txtUser.Text)), Server.HtmlDecode(CommonFunctions.encryptString(txtPass.Text)));
        Authenticated = AuthenticationMethod(Server.HtmlDecode(txtemail.Text), Server.HtmlDecode(txtpassword.Text));
        if ((Authenticated == true))
        {
            string strurl = prevurl.Value;
            string[] strvalue = null;
            strvalue = strurl.Split((Convert.ToChar("/")));
            string strname = null;
            strname = strvalue[strvalue.Length - 1];
            if (!string.IsNullOrEmpty(strname) & strname != "home.aspx" & strname != "forgotpassword.aspx")
            {
                //distroy captcha session
                Response.Redirect(strname);
            }
            else
            {
                //distroy captcha session
                Response.Redirect("home.aspx");
            }
        }
        else
        {
            ltrmessage.Text = "Invalid Username or Password";
            SetFocus(txtemail);
        }

    }

    private bool AuthenticationMethod(string UserName, string Password)
    {
        bool boolReturnValue = false;
        objlogin.userid = UserName;
        objlogin.password = Password;

        DataSet ds = new DataSet();
        ds = objlogin.CheckAuthentication();
        int i = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            do
            {

                if (UserName == ds.Tables[0].Rows[i]["userid"].ToString() && Password == ds.Tables[0].Rows[i]["password"].ToString() && ds.Tables[0].Rows[i]["admintypeid"].ToString() == System.Configuration.ConfigurationManager.AppSettings["SuperAdminTypeId"])
                {
                    Session["UserType"] = ds.Tables[0].Rows[i]["typeName"];
                    Session["AdminId"] = ds.Tables[0].Rows[i]["adminid"];
                    Session["UserTypeId"] = ds.Tables[0].Rows[i]["admintypeid"];
                    Session["AdminName"] = ds.Tables[0].Rows[i]["firstname"] + " " + ds.Tables[0].Rows[i]["lastname"];
                    //Session["AdminName"] = ds.Tables[0].Rows[i]["firstname"];

                    objlogin.adminid = Convert.ToInt32(ds.Tables[0].Rows[i]["adminid"]);
                    DataTable dtpages = objlogin.getAdminAccesPages();
                    List<string> pagelist = new List<string>();
                    foreach (DataRow row in dtpages.Rows)
                    {
                        pagelist.Add(Convert.ToString(row["pageurl"]));
                    }
                    pagelist.Add("home.aspx");
                    Session["adminaccespage"] = pagelist;

                    boolReturnValue = true;
                }

                if (UserName == ds.Tables[0].Rows[i]["userid"].ToString() && Password == ds.Tables[0].Rows[i]["password"].ToString() && ds.Tables[0].Rows[i]["admintypeid"].ToString() == System.Configuration.ConfigurationManager.AppSettings["AdminTypeId"])
                {
                    Session["UserType"] = ds.Tables[0].Rows[i]["typeName"];
                    Session["AdminId"] = ds.Tables[0].Rows[i]["adminid"];
                    Session["UserTypeId"] = ds.Tables[0].Rows[i]["admintypeid"];
                    Session["AdminName"] = ds.Tables[0].Rows[i]["firstname"] + " " + ds.Tables[0].Rows[i]["lastname"];

                    objlogin.adminid = Convert.ToInt32(ds.Tables[0].Rows[i]["adminid"]);
                    DataTable dtpages = objlogin.getAdminAccesPages();
                    List<string> pagelist = new List<string>();
                    foreach (DataRow row in dtpages.Rows)
                    {
                        pagelist.Add(Convert.ToString(row["pageurl"]));
                    }
                    pagelist.Add("home.aspx");
                    Session["adminaccespage"] = pagelist;

                    boolReturnValue = true;
                }
                ds.Clear();
                i += 1;

            } while (i < Convert.ToInt32(ds.Tables[0].Rows.Count));
        }
        return boolReturnValue;
    }

}