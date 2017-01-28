using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admin_setup : System.Web.UI.Page
{
    SetupManager objsetup = new SetupManager();
    ServicesManager objservice = new ServicesManager();
    EmailNotificationManager objemailnotification = new EmailNotificationManager();
    smsnotifactionManager objsms = new smsnotifactionManager();
    StoreManager objstore = new StoreManager();

    public int menucount = 0;
    public int submenucount = 0;
    public int turncount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Setup - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        if (!Page.IsPostBack)
        {
            #region  Bind Genral information
            genralbindData();
            #endregion

            #region Bind Store Information
            Bindstoreinfo();
            #endregion

            #region Bind Services
            BindServices();
            #endregion

            #region BindEmailnotification

            BindEmailTemplate();
            BindEmailTemplateFields();
            DataTable dtemailnotification = new DataTable();
            objemailnotification.EmailType = Convert.ToInt32((int)EmailNotificationManager.EmailTemplate.Invoice);
            dtemailnotification = objemailnotification.SelectSingleItem();
            if (dtemailnotification.Rows.Count > 0)
            {
                txtfromemail.Text = Server.HtmlDecode(dtemailnotification.Rows[0]["FromEmail"].ToString());
            }

            #endregion

            #region bind sms notifaction
            BindSmsTemplate();
            BindSmsTemplateFields();

            DataTable dtsmsnotification = new DataTable();
            dtsmsnotification = objsms.SelectSingleRecord();
            if (dtsmsnotification.Rows.Count > 0)
            {
                txtSid.Text = Server.HtmlDecode(dtsmsnotification.Rows[0]["TwilioSid"].ToString());
                txtToken.Text = Server.HtmlDecode(dtsmsnotification.Rows[0]["TwilioAuthToken"].ToString());
                if (Convert.ToByte(dtsmsnotification.Rows[0]["IsActive"]) == 1) { rbtenable.Checked = true; }
                else { rbtdisabled.Checked = true; }

            }
            #endregion

            #region user role
            DataTable dtSeleectedMenu = new DataTable();
            BindMenuEditMode(dtSeleectedMenu);
            BindUserType();
            if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
            {
                if (RegExp.IsNumericValue(Request.QueryString["id"]))
                {
                    ////check is sub admin is viewing his own account
                    //if (Session["AdminType"] != null && Convert.ToString(Session["AdminType"]) == "subadmin")
                    //{
                    //    if (Convert.ToInt32(Session["adminId"]) != Convert.ToInt32(Request.QueryString["id"]))
                    //    {
                    //        Response.Redirect("adminuser.aspx");
                    //    }
                    //}
                    divmenu.Visible = true;

                    hdtab.Value = "userrole";
                    BindSingleUser();
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
            hdTotalMenu.Value = menucount.ToString();
            #endregion
        }
    }

    #region General
    protected string UploadImage()
    {
        string actualfolder = string.Empty;
        //string smallfolder = string.Empty;


        actualfolder = Server.MapPath("../" + AppSettings.LOGO_ACTULE_ROOTURL);
        //smallfolder = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ALBUM_SMALL_ROOTURL"].ToString());

        DirectoryInfo actDir = new DirectoryInfo(actualfolder);
        //DirectoryInfo smallDir = new DirectoryInfo(smallfolder);


        //check if Directory exist if not create it
        if (!actDir.Exists) { Directory.CreateDirectory(actualfolder); }
        string filename = string.Empty;
        string fullimagepath = string.Empty;

        filename = "Logo" + Path.GetExtension(Path.GetFileName(logoimg.PostedFile.FileName));

        fullimagepath = actualfolder + filename;

        //delete old files if Exists
        CommonFunctions.DeleteFile(fullimagepath);
        //save original image
        logoimg.PostedFile.SaveAs(fullimagepath);

        //generate thumb
        //CommonFunctions.Thmbimages(fullimagepath, smallfolder, filename, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ALBUM_SMALL_WIDTH"].ToString()), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ALBUM_SMALL_HEIGHT"].ToString()), 0);
        //CommonFunctions.Thmbimages(fullimagepath, thumbfolder, filename, Convert.ToInt32(AppSettings.CATEGORY_THUMB_WIDTH), Convert.ToInt32(AppSettings.CATEGORY_THUMB_HEIGHT), 0);
        //CommonFunctions.Thmbimages(fullimagepath, thumbrectfolder, filename, Convert.ToInt32(AppSettings.CATEGORY_THUMBRECT_WIDTH), Convert.ToInt32(AppSettings.CATEGORY_THUMB_HEIGHT), 0);
        //CommonFunctions.Thmbimages(fullimagepath, midiumfolder, filename, Convert.ToInt32(AppSettings.CATEGORY_MEDIUM_WIDTH), Convert.ToInt32(AppSettings.CATEGORY_MEDIUM_HEIGHT), 0);

        return filename;


    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                objsetup.companyname = Convert.ToString(txtcomanyname.Text);
                objsetup.streetAddress = Convert.ToString(txtstreetaddress.Text);
                objsetup.city = Convert.ToString(txtcity.Text);
                objsetup.country = Convert.ToString(txtcountry.Text);
                objsetup.telephone = Convert.ToString(txttelephone.Text);
                objsetup.fax = Convert.ToString(txtfax.Text);
                objsetup.supportEmail = Convert.ToString(txtsupportemail.Text);
                objsetup.aboutCompany = Server.HtmlEncode(txtaboutcompany.Text);
                if (logoimg.HasFile)
                {
                    objsetup.imagepath = UploadImage();
                }
                else
                {
                    objsetup.imagepath = "";
                }
                objsetup.companyid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["companyid"]);
                objsetup.UpdateItem();

                hdtab.Value = "General";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "<script> alert('Company Information updated successfully'); </script>", true);
                lblmsg.Visible = true;
                lblmsgs.Text = "Company information updated successfully";
                genralbindData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { objsetup = null; }
        }
    }

    public void genralbindData()
    {
        //Bind Genral Information
        DataTable dtcontent = new DataTable();
        objsetup.companyid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["companyid"]);
        dtcontent = objsetup.SelectSingleItem();
        if (dtcontent.Rows.Count > 0)
        {
            txtcomanyname.Text = Server.HtmlDecode(dtcontent.Rows[0]["companyname"].ToString());
            txtstreetaddress.Text = Server.HtmlDecode(dtcontent.Rows[0]["streetAddress"].ToString());
            txtcity.Text = Server.HtmlDecode(dtcontent.Rows[0]["city"].ToString());
            txtcountry.Text = Server.HtmlDecode(dtcontent.Rows[0]["country"].ToString());
            txttelephone.Text = Server.HtmlDecode(dtcontent.Rows[0]["telephone"].ToString());
            txtfax.Text = Server.HtmlDecode(dtcontent.Rows[0]["fax"].ToString());
            txtsupportemail.Text = Server.HtmlDecode(dtcontent.Rows[0]["supportEmail"].ToString());
            txtaboutcompany.Text = Server.HtmlDecode(dtcontent.Rows[0]["aboutCompany"].ToString());

            string imgname = Server.HtmlDecode(dtcontent.Rows[0]["logo"].ToString());

            if (imgname != "")
            {
                // string strPath = Server.MapPath("../resources/product/thumb/") + imgname.ToString();
                imgMenu.Src = "../" + AppSettings.LOGO_ACTULE_ROOTURL + imgname.ToString();
            }
            else
            {
                imgMenu.Src = "../images/noimage.png";
            }
        }

    }
    #endregion

    #region Services

    protected void btnupdateservices_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                objservice.facebookUrl = Convert.ToString(txtfacebook.Text);
                objservice.twitterUrl = Convert.ToString(txttwitter.Text);
                objservice.adrollAdvId = Convert.ToString(txtadvid.Text);
                objservice.adrollAdvPixId = Convert.ToString(txtpixid.Text);
                objservice.googleAnalytics = Convert.ToString(txtgoogleanalytics.Text);
                objservice.googleMap = Convert.ToString(txtgooglemaps.Text);

                objservice.serviceid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["serviceid"]);
                objservice.UpdateItem();
                hdtab.Value = "Services";

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "<script>alert('Services Information updated successfully');</script>", false);
                lblmsg.Visible = true;
                lblmsgs.Text = "Services information updated successfully";
                BindServices();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { objservice = null; }
        }
    }

    void BindServices()
    {
        DataTable dtservicecontent = new DataTable();
        objservice.serviceid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["serviceid"]);
        dtservicecontent = objservice.SelectSingleItem();
        if (dtservicecontent.Rows.Count > 0)
        {
            txtfacebook.Text = Server.HtmlDecode(dtservicecontent.Rows[0]["facebookUrl"].ToString());
            txttwitter.Text = Server.HtmlDecode(dtservicecontent.Rows[0]["twitterUrl"].ToString());
            txtadvid.Text = Server.HtmlDecode(dtservicecontent.Rows[0]["adrollAdvId"].ToString());
            txtpixid.Text = Server.HtmlDecode(dtservicecontent.Rows[0]["adrollAdvPixId"].ToString());
            txtgoogleanalytics.Text = Server.HtmlDecode(dtservicecontent.Rows[0]["googleAnalytics"].ToString());
            txtgooglemaps.Text = Server.HtmlDecode(dtservicecontent.Rows[0]["googleMap"].ToString());

        }
    }

    #endregion

    #region Email notifaction

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
                    }
                    else
                    {
                        strtabbindtemplate += "<div class='tab-pane' id=" + tab.ToString() + ">";
                    }
                    //strtabbindtemplate += "<div class='active tab-pane' id=" + tab.ToString() + ">";
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
            strtabbindtemplate += "</div>";

            ltrtabbinddata.Text = strtabbindtemplate.ToString();
        }
        catch (Exception ex) { throw ex; }
        finally { objemailnot = null; }

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
            BindEmailTemplateFields();
            hdtab.Value = "Email";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script>alert('Email Notification updated successfully');</script>", false);
            lblmsg.Visible = true;
            lblmsgs.Text = "Email notification updated successfully";

        }
        catch (Exception ex) { throw ex; }
        finally { objemailnotification = null; }
    }

    #endregion

    #region  SMS notifaction

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

                if (rbtenable.Checked == true) { objsmsnotification.IsActive = Convert.ToByte(true); }
                else { objsmsnotification.IsActive = Convert.ToByte(false); }

                objsmsnotification.Descp = Convert.ToString(Request.Form["txtsmsdescription" + ((int)tab)]);
                objsmsnotification.UpdateItem();
            }
            BindSmsTemplateFields();
            hdtab.Value = "smsnotification";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script> alert('Sms notification updated successfully'); </script>", false);
            lblmsg.Visible = true;
            lblmsgs.Text = "Sms notification updated successfully";

        }
        catch (Exception ex) { throw ex; }
        finally { }
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
            ltrsmsTab.Text = strtabbind.ToString();
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
                    }
                    else
                    {
                        strtabbindtemplate += "<div class='tab-pane' id=" + tab.ToString() + ">";
                    }
                    //strtabbindtemplate += "<div class='active tab-pane' id=" + tab.ToString() + ">";
                    strtabbindtemplate += "<div class='form-group'>";
                    strtabbindtemplate += "<div class='col-sm-12'>";
                    strtabbindtemplate += "<textarea class='form-control' id='txtsmsdescription" + ((int)tab).ToString() + "'  maxlength='160' name='txtsmsdescription" + ((int)tab).ToString() + "' >" + dtcontent.Rows[0]["Descp"].ToString() + "</textarea>Note : Characters Limit : 160";
                    strtabbindtemplate += "</div>";
                    strtabbindtemplate += "</div>";
                    strtabbindtemplate += "</div>";

                }
            }
            strtabbindtemplate += "</div>";
            ltrSmsTabbind.Text = strtabbindtemplate.ToString();
        }
        catch (Exception ex) { throw ex; }

    }

    #endregion

    #region USER TYPE

    public void BindUserType()
    {
        usertypeManager objuser = new usertypeManager();

        DataTable dt = new DataTable();
        try
        {
            dt = objuser.GetAllUserType();
            UserTypelist.DataSource = dt;
            UserTypelist.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
    }

    #region --- menu checkbox bind --

    public void BindSingleUser()
    {
        DataSet dsadmin = new DataSet();
        AdminManager objadmin = new AdminManager();
        try
        {
            objadmin.adminid = Convert.ToInt32(Request.QueryString["id"]);
            dsadmin = objadmin.SelectSingleItem();

            if (dsadmin.Tables[0].Rows.Count > 0)
            {
                //hdadminid.Value = (Request.QueryString["id"]).ToString();
                //txtusername.Text = Server.HtmlDecode(dsadmin.Tables[0].Rows[0]["userid"].ToString());
                //txtfname.Text = dsadmin.Tables[0].Rows[0]["firstname"].ToString();
                //txtlname.Text = dsadmin.Tables[0].Rows[0]["lastname"].ToString();
                //txtemail.Text = dsadmin.Tables[0].Rows[0]["emailaddress"].ToString();
            }
            DataTable dtSeleectedMenu = dsadmin.Tables[1];
            BindMenuEditMode(dtSeleectedMenu);

        }
        catch (Exception ex) { throw ex; }
        finally { objadmin = null; dsadmin.Clear(); }
    }

    public void BindMenuEditMode(DataTable dtSelectedMenus)
    {
        MenuManager objMenu = new MenuManager();
        DataTable dt = new DataTable();
        try
        {
            menucount = 0;
            dt = objMenu.GetParentMenu(true);
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder strMenus = new StringBuilder();
                strMenus.Clear();
                strMenus.Append("<table>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    menucount++;
                    submenucount = 0;
                    //strMenus.Append("<tr>");
                    if (i % 2 == 0)
                    { strMenus.Append("<tr>"); }
                    else { strMenus.Append("<tr>"); }
                    strMenus.Append("<td>");
                    int cnt = 0;
                    for (int j = 0; j < dtSelectedMenus.Rows.Count; j++)
                    {
                        if (dtSelectedMenus.Rows[j]["SelectedAdminMenuId"].ToString() == dt.Rows[i]["idmenu"].ToString())
                        {
                            cnt++;
                        }
                    }
                    strMenus.Append("<input type=\"hidden\" name=\"hdMenuId" + menucount.ToString() + "\" id=\"hdMenuId" + menucount.ToString() + "\" value=\"" + dt.Rows[i]["idmenu"].ToString() + "\" />");
                    if (cnt > 0)
                    {
                        strMenus.Append("<div class='label_check'><input type=\"checkbox\" class=\"clsChk\" idmenu=\"" + dt.Rows[i]["idmenu"].ToString() + "\" checked=\"checked\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dt.Rows[i]["title"].ToString() + "\" title=\"" + dt.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dt.Rows[i]["title"].ToString() + "</label></div>");
                    }
                    else
                    {
                        strMenus.Append("<div class='label_check'><input type=\"checkbox\" class=\"clsChk\" idmenu=\"" + dt.Rows[i]["idmenu"].ToString() + "\"  id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dt.Rows[i]["title"].ToString() + "\" title=\"" + dt.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dt.Rows[i]["title"].ToString() + "</label></div>");
                    }
                    strMenus.Append(BindSubMenusEditMode(Convert.ToInt32(dt.Rows[i]["idmenu"].ToString()), dtSelectedMenus));
                    strMenus.Append("</td>");
                    //strMenus.Append("<tr>");
                }
                strMenus.Append("</table>");
                ltrMenus.Text = strMenus.ToString();
                //trPermission.Visible = true;
            }
            else
            { //trPermission.Visible = false; 
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objMenu = null; }
    }

    public string BindSubMenusEditMode(int parentId, DataTable dtSelectedMenus)
    {
        MenuManager objMenu = new MenuManager();
        DataTable dtSubMenu = new DataTable();
        StringBuilder strSubMenus = new StringBuilder();
        // submenucount++;
        try
        {
            objMenu.parentid = parentId;
            dtSubMenu = objMenu.GetSubMenu();
            if (dtSubMenu != null && dtSubMenu.Rows.Count > 0)
            {
                turncount++;
                int dynamicmargin = 22 * turncount;
                string style = "margin-left:" + dynamicmargin + "px";
                strSubMenus.Clear();
                strSubMenus.Append("<table style=\"margin-left:50px;\">");
                for (int i = 0; i < dtSubMenu.Rows.Count; i++)
                {
                    int cnt = 0;
                    if (i % 2 == 0)
                    { strSubMenus.Append("<tr>"); }
                    else { strSubMenus.Append("<tr>"); }
                    strSubMenus.Append("<tr>");
                    strSubMenus.Append("<td>");
                    for (int j = 0; j < dtSelectedMenus.Rows.Count; j++)
                    {
                        if (dtSelectedMenus.Rows[j]["SelectedAdminMenuId"].ToString() == dtSubMenu.Rows[i]["idmenu"].ToString())
                        {
                            cnt++;
                        }
                    }
                    menucount++;


                    //strSubMenus.Append("<ul>");
                    //strSubMenus.Append("<li>");
                    strSubMenus.Append("<input type=\"hidden\" name=\"hdMenuId" + menucount.ToString() + "\" id=\"hdMenuId" + menucount.ToString() + "\" value=\"" + dtSubMenu.Rows[i]["idmenu"].ToString() + "\" />");
                    if (cnt > 0)
                    {
                        strSubMenus.Append("<div class='label_check'><input type=\"checkbox\" class=\"clsChk\" idmenu=\"" + dtSubMenu.Rows[i]["idmenu"].ToString() + "\" checked=\"checked\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" title=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dtSubMenu.Rows[i]["title"].ToString() + "</label></div>");
                    }
                    else
                    {
                        strSubMenus.Append("<div class='label_check'><input type=\"checkbox\" class=\"clsChk\" idmenu=\"" + dtSubMenu.Rows[i]["idmenu"].ToString() + "\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" title=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dtSubMenu.Rows[i]["title"].ToString() + "</label></div>");
                    }
                    strSubMenus.Append(BindSubMenusEditMode(Convert.ToInt32(dtSubMenu.Rows[i]["idmenu"].ToString()), dtSelectedMenus));
                    strSubMenus.Append("</td>");
                    strSubMenus.Append("</tr>");
                }
                strSubMenus.Append("</table>");
            }
            else
            {
                // trPermission.Visible = false;
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objMenu = null; }
        return strSubMenus.ToString();
    }

    public static void GenerateAdminMenu(int adminid)
    {
        AdminManager objUser = new AdminManager();
        try
        {
            DataTable dtcat1 = new DataTable();
            objUser.adminid = adminid;
            objUser.admintypeid = adminid;
            dtcat1 = objUser.selectAdminParentMenus();
            string strMenu = "";
            var Parent = (from p in dtcat1.AsEnumerable() select p);
            if (Parent != null)
            {
                //strMenu += "<ul>";
                //strMenu += "<li class='treeview active'><a href='home.aspx' title='Home'><i class='fa fa-edit'></i><span>Dashbord</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a></li>";
                strMenu += " <li id='menu0'><a id='0' href='home.aspx'><i class='fa fa-home'></i><span>Dashboard</span></a></li>";
                foreach (var row in Parent)
                {
                    string strsubmenu = BindSubCat(row.Field<int>("idmenu"), adminid);
                    if (strsubmenu == "<ul class='treeview-menu' style='display: none;'></ul>")
                    {
                        //if hr 03_10_2016
                        if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Catalogs"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\"  idmenu=\"" + row.Field<int>("idmenu").ToString() + "\" class='treeview'><a  id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-gears'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Images"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\"  idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"  class='treeview'><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-cubes'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SetUp"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\" class='treeview'><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-asterisk'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Orders"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-shopping-bag'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Customer"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-user'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Inventory"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-tasks'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Payment"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-credit-card'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CMS"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-list'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmartCatelogs"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-list'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        else
                        {
                            strMenu += "<li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-edit'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-edit'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                    }
                    else
                    {
                        //if hr 03_10_2016
                        if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Catalogs"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\"  idmenu=\"" + row.Field<int>("idmenu").ToString() + "\" class='treeview'><a  id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-gears'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Images"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\"  idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"  class='treeview'><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-cubes'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SetUp"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\" class='treeview'><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-asterisk'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Orders"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-shopping-bag'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Customer"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-user'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Inventory"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-tasks'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Payment"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-credit-card'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CMS"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-list'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmartCatelogs"]))
                        {
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-list'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else
                        {
                            strMenu += "<li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-edit'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                            strMenu += " <li  id=\"menu" + row.Field<int>("idmenu").ToString() + "\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a id=\"" + row.Field<int>("idmenu").ToString() + "\" href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-edit'></i><span>" + row.Field<string>("title").ToString() + "</span></a>";
                        }
                        strMenu += strsubmenu;
                    }

                    strMenu += "</li>";
                }
                //strMenu += "<li><a href='logout.ashx'><i class='fa fa-sign-out'></i><span>Logout</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a></li>";
                strMenu += " <li><a href='logout.ashx'><i class='fa fa-sign-out'></i><span>LogOut</span></a></li>";
                //strMenu += "</ul>";
            }


            string actualfolder = HttpContext.Current.Server.MapPath("../" + AppSettings.ADMIN_TYPE_MENU_ROOTURL.ToString());
            DirectoryInfo actDir = new DirectoryInfo(actualfolder);
            if (!actDir.Exists) { Directory.CreateDirectory(actualfolder); }

            StreamWriter fp;
            DirectoryInfo di;
            fp = File.CreateText(actualfolder + adminid.ToString() + ".html");
            fp.WriteLine(strMenu);
            fp.Close();
        }
        catch (Exception ex) { throw ex; }
        finally { objUser = null; }
    }

    public static string BindSubCat(int parentid, int adminid)
    {
        string strmenu = "";
        DataTable dtcat2 = new DataTable();
        AdminManager objUser = new AdminManager();
        StringBuilder strCategoryUrlUpdateQuery = new StringBuilder();
        strCategoryUrlUpdateQuery.Clear();
        try
        {
            objUser.adminid = adminid;
            objUser.parentid = parentid;
            dtcat2 = objUser.selectAdminSubMenus();
            //dtcat2 = objUser.selectAdminParentMenus();
            strmenu += "<ul class='treeview-menu' style='display: none;'>";
            if (dtcat2.Rows.Count > 0)
            {

                for (int j = 0; j < dtcat2.Rows.Count; j++)
                {
                    string strsubmenu = BindSubCat(Convert.ToInt32(dtcat2.Rows[j]["idmenu"]), adminid);

                    if (strsubmenu == "<ul class=\"treeview-menu\" style=\"display: none;\"></ul>")
                    {
                        strmenu += "<li class='active' subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-circle-o'></i>" + dtcat2.Rows[j]["title"].ToString() + "</a>";
                    }
                    else
                    {
                        if (dtcat2.Rows[j]["title"].ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Images"]))
                        {
                            strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-camera'></i><span>" + dtcat2.Rows[j]["title"].ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                        }
                        else if (dtcat2.Rows[j]["title"].ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Product"]))
                        {
                            strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-cubes'></i><span>" + dtcat2.Rows[j]["title"].ToString() + "</span></a>";
                        }
                        else if (dtcat2.Rows[j]["title"].ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Category"]))
                        {
                            strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-asterisk'></i><span>" + dtcat2.Rows[j]["title"].ToString() + "</span></a>";
                        }
                        else if (dtcat2.Rows[j]["title"].ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Brand"]))
                        {
                            strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-bookmark'></i><span>" + dtcat2.Rows[j]["title"].ToString() + "</span></a>";
                        }
                        else if (dtcat2.Rows[j]["title"].ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Configuration"]))
                        {
                            strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-plus-square'></i><span>" + dtcat2.Rows[j]["title"].ToString() + "</span></a>";
                        }
                        else if (dtcat2.Rows[j]["title"].ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ManageUser"]))
                        {
                            strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-users'></i><span>" + dtcat2.Rows[j]["title"].ToString() + "</span></a>";
                        }
                        else if (dtcat2.Rows[j]["title"].ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Importjobs"]))
                        {
                            strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-upload'></i><span>" + dtcat2.Rows[j]["title"].ToString() + "</span></a>";
                        }
                        else
                        {
                            strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-circle-o'></i><span>" + dtcat2.Rows[j]["title"].ToString() + "</span></a>";
                        }
                    }

                    if (strsubmenu == "<ul class='treeview-menu' style='display: none;'></ul>") { }
                    else
                    {
                        strmenu += strsubmenu;
                    }
                    strmenu += "</li>";
                }

            }
            strmenu += "</ul>";
        }
        catch (Exception ex) { throw ex; }
        //finally { dtcat2 = null; objUser = null; }
        return strmenu;
    }

    #endregion

    protected void btnsavechange_Click(object sender, EventArgs e)
    {
        AdminManager objadmin = new AdminManager();
        string queryflg = string.Empty;
        int InsertedUserId = 0;

        //if (Convert.ToInt32(Request.QueryString["id"]) == 0)
        //{
        //    InsertedUserId = 1;
        //}
        //else
        //{
        //InsertedUserId = Convert.ToInt32(Request.QueryString["id"]);
        //}

        InsertedUserId = Convert.ToInt32(Request.QueryString["id"]);

        //END INSERT OR UPDATE ADMIN FIRST 
        string strQ = "delete from adminrights where adminid=" + InsertedUserId;
        for (int i = 0; i < Convert.ToInt32(hdTotalMenu.Value); i++)
        {
            if (Request.Form["chkPermission" + (i + 1).ToString()] != null)
            {
                if (Request.Form["chkPermission" + (i + 1).ToString()].ToString().ToLower() == "on")
                {
                    strQ += " INSERT INTO adminrights (idmenu,adminid) VALUES (" + Convert.ToInt32(Request.Form["hdMenuId" + (i + 1).ToString()]) + "," + Convert.ToInt32(InsertedUserId) + ") ";
                }
            }
            // strQ += " INSERT INTO adminrights (idmenu,adminid) VALUES (" + Convert.ToInt32(Request.Form["hdMenuId" + (i + 1).ToString()]) + "," + Convert.ToInt32(InsertedUserId) + ") ";
        }
        objadmin.InsertUserMenu(strQ);
        GenerateAdminMenu(InsertedUserId);
        //generatemenuforadmin(InsertedUserId);
        //BindSingleUser();
        lblmsg.Visible = true;
        lblmsgs.Text = "User role updated successfully";
    }

    #endregion

    #region StoreInformation
    protected void btnsoresetting_Click(object sender, EventArgs e)
    {
        objstore.product_listing = txtproductlisting.Text;
        if (rbtwithimg.Checked == true)
        {
            objstore.category = 1;
        }
        else if (rbtwithoutimg.Checked == true)
        {
            objstore.category = 2;
        }
        else if (rbtcommanimg.Checked == true)
        {
            objstore.category = 3;
        }
        if (rbtpricerange.Checked == true)
        {
            objstore.filter = 1;
        }
        else if (rbtcategory.Checked == true)
        {
            objstore.filter = 2;
        }
        string sortingoption = string.Empty;
        if (chknewpopular.Checked == true)
        {
            sortingoption += "1,";
        }
        if (chkpricelowhigh.Checked == true)
        {
            sortingoption += "2,";
        }
        if (chkpricehighlow.Checked == true)
        {
            sortingoption += "3,";
        }
        if (chkoldestfirst.Checked == true)
        {
            sortingoption += "4";
        }
        objstore.sorting = sortingoption.TrimEnd(',');
        if (imgnotfound.HasFile)
        {
            objstore.imgnotfound = UploadImageNotfound();
        }
        else
        {
            objstore.imgnotfound = "";
        }
        objstore.storeid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["storeid"]);
        objstore.UpdateItem();
        hdtab.Value = "storesetting";
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "<script> alert('Company Information updated successfully'); </script>", true);
        lblmsg.Visible = true;
        lblmsgs.Text = "Store information updated successfully";
        Bindstoreinfo();
    }

    public void Bindstoreinfo()
    {
        #region "-----------Bind Information--------------"
        DataTable dtstorecontent = new DataTable();
        objstore.storeid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["storeid"]);
        dtstorecontent = objstore.SelectSingleItem();
        if (dtstorecontent.Rows.Count > 0)
        {
            txtproductlisting.Text = Server.HtmlDecode(dtstorecontent.Rows[0]["product_listing"].ToString());
            string category = Server.HtmlDecode(dtstorecontent.Rows[0]["category"].ToString());
            string sorting = Server.HtmlDecode(dtstorecontent.Rows[0]["sorting"].ToString());
            string filter = Server.HtmlDecode(dtstorecontent.Rows[0]["filter"].ToString());
            string noimgfound = Server.HtmlDecode(dtstorecontent.Rows[0]["imgnotfound"].ToString());

            if (Convert.ToInt32(category) == 1)
            {
                rbtwithimg.Checked = true;
            }
            else if (Convert.ToInt32(category) == 2)
            {
                rbtwithoutimg.Checked = true;
            }
            else
            {
                rbtcommanimg.Checked = true;
            }
            if (sorting != "")
            {
                string[] sortingpart = sorting.Split(',');
                for (int sort = 0; sort < sortingpart.Length; sort++)
                {
                    if (Convert.ToInt32(sortingpart[sort]) == 1)
                    {
                        chknewpopular.Checked = true;
                    }
                    else if (Convert.ToInt32(sortingpart[sort]) == 2)
                    {
                        chkpricelowhigh.Checked = true;
                    }
                    else if (Convert.ToInt32(sortingpart[sort]) == 3)
                    {
                        chkpricehighlow.Checked = true;
                    }
                    else if (Convert.ToInt32(sortingpart[sort]) == 4)
                    {
                        chkoldestfirst.Checked = true;
                    }
                }
            }
            if (Convert.ToInt32(filter) == 1)
            {
                rbtpricerange.Checked = true;
            }
            else if (Convert.ToInt32(filter) == 2)
            {
                rbtcategory.Checked = true;
            }

            string imgname = Server.HtmlDecode(dtstorecontent.Rows[0]["imgnotfound"].ToString());

            if (imgname != "")
            {
                notfoundimg.Src = "../" + AppSettings.NOIMG_ACTULE_ROOTURL + imgname.ToString();
            }
            else
            {
                notfoundimg.Src = "../images/noimage.png";
            }
        }
        #endregion
    }

    protected string UploadImageNotfound()
    {
        string actualfolder = string.Empty;
        actualfolder = Server.MapPath("../" + AppSettings.NOIMG_ACTULE_ROOTURL);
        DirectoryInfo actDir = new DirectoryInfo(actualfolder);
        //check if Directory exist if not create it
        if (!actDir.Exists) { Directory.CreateDirectory(actualfolder); }
        string filename = string.Empty;
        string fullimagepath = string.Empty;
        filename = "noimage" + Path.GetExtension(Path.GetFileName(imgnotfound.PostedFile.FileName));
        fullimagepath = actualfolder + filename;
        //delete old files if Exists
        CommonFunctions.DeleteFile(fullimagepath);
        imgnotfound.PostedFile.SaveAs(fullimagepath);
        return filename;
    }
    #endregion

}
