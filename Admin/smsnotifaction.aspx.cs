using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_smsnotifaction : System.Web.UI.Page
{
    smsnotifactionManager objsms = new smsnotifactionManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            BindSmsTemplate();
            BindSmsTemplateFields();

            //bind sms notifaction
            DataTable dtsmsnotification = new DataTable();
            dtsmsnotification = objsms.SelectSingleRecord();
            if (dtsmsnotification.Rows.Count > 0)
            {
                txtSid.Text = Server.HtmlDecode(dtsmsnotification.Rows[0]["TwilioSid"].ToString());
                txtToken.Text = Server.HtmlDecode(dtsmsnotification.Rows[0]["TwilioAuthToken"].ToString());
            }
        }
    }

    private void BindSmsTemplate()
    {
        string strtabbind = "";
        try
        {
            Array tabname = Enum.GetValues(typeof(smsnotifactionManager.smsTemplate));

            strtabbind += "<ul class='nav nav-tabs'>";
            foreach (smsnotifactionManager.smsTemplate tab in tabname)
            {
                if (((int)tab) == 1)
                {
                    strtabbind += "<li class='active'><a href='#" + tab.ToString() + "' data-toggle='tab'>" + tab.ToString().Replace("_", " ") + "</a></li>";
                }
                else
                {
                    strtabbind += "<li><a href='#" + tab.ToString() + "' data-toggle='tab'>" + tab.ToString().Replace("_", " ") + "</a></li>";
                }
            }
            strtabbind += "</ul>";
            ltrtabbind.Text = strtabbind.ToString();
        }
        //ltrtab.Text = strLanguage.ToString();

        catch (Exception ex) { throw ex; }

    }

    private void BindSmsTemplateFields()
    {
        smsnotifactionManager objemailnot = new smsnotifactionManager();
        string strtabbindtemplate = "";
        try
        {
            Array tabname = Enum.GetValues(typeof(EmailNotificationManager.EmailTemplate));
            strtabbindtemplate += "<div class='tab-content'>";
            foreach (smsnotifactionManager.smsTemplate tab in tabname)
            {
                DataTable dtcontent = new DataTable();
                objemailnot.smstype = Convert.ToInt32(((int)tab).ToString());

                dtcontent = objemailnot.SelectSingleItem();
                if (dtcontent.Rows.Count > 0)
                {
                    if (((int)tab) == 1)
                    {
                        strtabbindtemplate += "<div class='active tab-pane' id=" + tab.ToString() + ">";

                        strtabbindtemplate += "<div class='form-group'>";
                        strtabbindtemplate += "<div class='col-sm-9'>";
                        strtabbindtemplate += "<textarea class='form-control ckeditor' id='txtdescription" + ((int)tab).ToString() + "'  maxlength='500' name='txtdescription" + ((int)tab).ToString() + "' >" + dtcontent.Rows[0]["Descp"].ToString() + "</textarea>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "</div>";
                    }
                    else
                    {
                        strtabbindtemplate += "<div class='tab-pane' id=" + tab.ToString() + ">";

                        strtabbindtemplate += "<div class='form-group'>";
                        strtabbindtemplate += "<div class='col-sm-9'>";
                        strtabbindtemplate += "<textarea class='form-control ckeditor' id='txtdescription" + ((int)tab).ToString() + "'  maxlength='500' name='txtdescription" + ((int)tab).ToString() + "' >" + dtcontent.Rows[0]["Descp"].ToString() + "</textarea>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "</div>";
                    }
                }
            }
            strtabbindtemplate += "</div>";

            ltrtabbinddata.Text = strtabbindtemplate.ToString();
        }
        catch (Exception ex) { throw ex; }

    }

    protected void btnsmsnotification_Click(object sender, EventArgs e)
    {
        smsnotifactionManager objsmsnotification = new smsnotifactionManager();
        try
        {
            Array tabname = Enum.GetValues(typeof(smsnotifactionManager.smsTemplate));
            foreach (smsnotifactionManager.smsTemplate tab in tabname)
            {
                objsmsnotification.smstype = Convert.ToInt32(((int)tab));
                objsmsnotification.TwilioSid = Convert.ToString(txtSid.Text);
                objsmsnotification.TwilioAuthToken = Convert.ToString(txtToken.Text);

                objsmsnotification.Descp = Convert.ToString(Request.Form["txtdescription" + ((int)tab)]);
                objsmsnotification.UpdateItem();
            }
            //Response.Redirect("setup.aspx");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sms notification updated successfully');", true);

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }
}