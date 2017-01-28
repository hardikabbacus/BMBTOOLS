using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_forgotpassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txtUser.Focus();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnsend_Click(object sender, EventArgs e)
    {
        try
        {
            AdminManager objuser = new AdminManager();
            objuser.userid = txtUser.Text.Trim().ToString();


            if (objuser.UseridExist())
            {
                DataSet dtAdmin = new DataSet();
                string strcontent = string.Empty;
                dtAdmin = objuser.SelectAdminByUserId();
                if (dtAdmin.Tables[0].Rows.Count > 0)
                {
                    String strSubject = string.Empty;
                    strSubject = "Admin Password Renewal Response";
                    String strFrom = "" + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"].ToString() + "<mark@webtechsystem.com>";
                    String strto = dtAdmin.Tables[0].Rows[0]["emailaddress"].ToString();
                    String strMsg1 = string.Empty;
                    String strMsg = string.Empty;

                    strMsg1 = "<b>Dear " + dtAdmin.Tables[0].Rows[0]["firstname"].ToString() + " " + dtAdmin.Tables[0].Rows[0]["lastname"].ToString() + ",</b><br/>";
                    strMsg1 += "<table cellpadding='0' cellspacing='0' width=\"100%\">";
                    strMsg1 += "<tr class='alltrclass'>";
                    strMsg1 += "<td class='tdlabel' colspan=\"2\" style=\"padding:5px 5px 5px 5px;\">This is the response of your Password renewal Request.<br/> Your UserName and Password are below.</td>";
                    strMsg1 += "</tr>";
                    strMsg1 += "<tr class='alltrclass'>";
                    strMsg1 += "<td class='tdlabel' style=\"width:60px;padding:5px 5px 0px 5px;\"><b>Email:</b> </td>";
                    strMsg1 += "<td class='tdcontent' style=\"padding:5px 5px 0px 5px;\">" + dtAdmin.Tables[0].Rows[0]["userid"].ToString() + "</td>";
                    strMsg1 += "</tr>";

                    strMsg1 += "<tr class='alltrclass'>";
                    strMsg1 += "<td class='tdlabel' style=\"width:60px;padding:5px 5px 0px 5px;\"><b>Password:</b> </td>";
                    strMsg1 += "<td class='tdcontent' style=\"padding:5px 5px 0px 5px;\">" + dtAdmin.Tables[0].Rows[0]["password"].ToString() + "</td>";
                    strMsg1 += "</tr>";
                    strMsg1 += "</table>";

                    //strMsg1 += "<br>This is the response of your Password renewal Request. ";
                    //strMsg1 += "<br><br>Your Login Id is : " + dtAdmin.Tables[0].Rows[0]["userid"].ToString();
                    //strMsg1 += "<br>Your Password is : " + dtAdmin.Tables[0].Rows[0]["password"].ToString();
                    //strMsg1 += "<br><br>Thank You.";

                    strMsg = CommonFunctions.GetFileContents(Server.MapPath("../MailTemplate/adminmail.html"));
                    strMsg = strMsg.Replace("##CONTENT##", strMsg1);
                    strMsg = strMsg.Replace("##SITEURL##", AppSettings.SITEURL);
                    strMsg = strMsg.Replace("##IMAGEPATH##", AppSettings.SITEURL + "images/");
                    strMsg = strMsg.Replace("##COMPANYNAME##", System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"]);

                    CommonFunctions.SendMail2(strFrom, strto, "", strMsg, strSubject, "", "", "");
                    ltrmessage.Text = "Password has been sent to your email address.";
                    txtUser.Text = "";
                }
                dtAdmin.Dispose();
            }
            else
            {
                ltrmessage.Text = "The Username you entered is invalid.Please enter valid User Name to retrieve your password. ";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}