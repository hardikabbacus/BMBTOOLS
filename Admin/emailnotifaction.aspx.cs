using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_emailnotifaction : System.Web.UI.Page
{
    EmailNotificationManager objemailnotification = new EmailNotificationManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //BindEmailnotification
            BindEmailTemplate();
            BindEmailTemplateFields();

            DataTable dtemailnotification = new DataTable();
            objemailnotification.EmailType = Convert.ToInt32((int)EmailNotificationManager.EmailTemplate.Invoice);

            dtemailnotification = objemailnotification.SelectSingleItem();
            if (dtemailnotification.Rows.Count > 0)
            {
                txtfromemail.Text = Server.HtmlDecode(dtemailnotification.Rows[0]["FromEmail"].ToString());
            }

        }
    }

    private void BindEmailTemplate()
    {
        string strtabbind = "";
        try
        {
            Array tabname = Enum.GetValues(typeof(EmailNotificationManager.EmailTemplate));

            strtabbind += "<ul class='nav nav-tabs'>";

            foreach (EmailNotificationManager.EmailTemplate tab in tabname)
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

    private void BindEmailTemplateFields()
    {
        EmailNotificationManager objemailnot = new EmailNotificationManager();
        string strtabbindtemplate = "";
        try
        {
            Array tabname = Enum.GetValues(typeof(EmailNotificationManager.EmailTemplate));
            strtabbindtemplate += "<div class='tab-content'>";
            foreach (EmailNotificationManager.EmailTemplate tab in tabname)
            {
                DataTable dtcontent = new DataTable();
                objemailnot.EmailType = Convert.ToInt32(((int)tab).ToString());

                dtcontent = objemailnot.SelectSingleItem();
                if (dtcontent.Rows.Count > 0)
                {


                    if (((int)tab) == 1)
                    {


                        strtabbindtemplate += "<div class='active tab-pane' id=" + tab.ToString() + ">";
                        strtabbindtemplate += "<div class='form-group'>";
                        strtabbindtemplate += "<label for='inputName' class='col-sm-1 control-label'>Subject :</label>";
                        strtabbindtemplate += "<div class='col-sm-6'>";
                        strtabbindtemplate += "<input type='text' class='form-control' id='txtsubject" + ((int)tab).ToString() + "' maxlength='100' name='txtsubject" + ((int)tab).ToString() + "' value='" + dtcontent.Rows[0]["EmailSubject"].ToString() + "'/>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "<div class='form-group'>";
                        strtabbindtemplate += "<div class='col-sm-9'>";
                        strtabbindtemplate += "<textarea class='form-control ckeditor' id='txtdescription" + ((int)tab).ToString() + "'  maxlength='500' name='txtdescription" + ((int)tab).ToString() + "' >" + dtcontent.Rows[0]["EmailBody"].ToString() + "</textarea>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "</div>";
                    }
                    else
                    {
                        strtabbindtemplate += "<div class='tab-pane' id=" + tab.ToString() + ">";
                        strtabbindtemplate += "<div class='form-group'>";
                        strtabbindtemplate += "<label for='inputName' class='col-sm-1 control-label'>Subject :</label>";
                        strtabbindtemplate += "<div class='col-sm-6'>";
                        strtabbindtemplate += "<input type='text' class='form-control' id='txtsubject" + ((int)tab).ToString() + "' maxlength='100' name='txtsubject" + ((int)tab).ToString() + "' value='" + dtcontent.Rows[0]["EmailSubject"].ToString() + "'/>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "</div>";
                        strtabbindtemplate += "<div class='form-group'>";
                        strtabbindtemplate += "<div class='col-sm-9'>";
                        strtabbindtemplate += "<textarea class='form-control ckeditor' id='txtdescription" + ((int)tab).ToString() + "'  maxlength='500' name='txtdescription" + ((int)tab).ToString() + "' >" + dtcontent.Rows[0]["EmailBody"].ToString() + "</textarea>";
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

    protected void btnconfirmemailnotification_Click(object sender, EventArgs e)
    {
        EmailNotificationManager objemailnotification = new EmailNotificationManager();
        try
        {
            Array tabname = Enum.GetValues(typeof(EmailNotificationManager.EmailTemplate));
            foreach (EmailNotificationManager.EmailTemplate tab in tabname)
            {
                objemailnotification.EmailType = Convert.ToInt32(((int)tab));
                objemailnotification.FromEmail = Convert.ToString(txtfromemail.Text);
                objemailnotification.EmailSubject = Convert.ToString(Request.Form["txtsubject" + ((int)tab)]);
                objemailnotification.EmailBody = Convert.ToString(Request.Form["txtdescription" + ((int)tab)]);
                objemailnotification.UpdateItem();
            }
            //Response.Redirect("setup.aspx");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email notification updated successfully');", true);

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

}