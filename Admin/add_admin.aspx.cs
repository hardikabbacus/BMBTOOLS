using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;

public partial class Admin_add_admin : System.Web.UI.Page
{
    public int menucount = 0;
    public int submenucount = 0;
    public int turncount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //lblemailerrmsg.Visible = false;
        //lblusererrmsg.Visible = false;
        txtfname.Focus();
        if (!Page.IsPostBack)
        {
            Page.Title = "Add User - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add User";
            bindadmintype();
            if (Request.QueryString["flag"] == "edit")
            {
                //bindadmintype();
                Title = "Modify User - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify User";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {
                        //check is sub admin is viewing his own account
                        if (Session["AdminType"] != null && Convert.ToString(Session["AdminType"]) == "subadmin")
                        {
                            if (Convert.ToInt32(Session["adminId"]) != Convert.ToInt32(Request.QueryString["id"]))
                            {
                                Response.Redirect("adminuser.aspx");
                            }
                        }

                        BindSingleUser();
                    }
                    else
                    {
                        Response.Redirect("home.aspx");
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
            else
            {
                DataTable dtSeleectedMenu = new DataTable();
                BindMenuEditMode(dtSeleectedMenu);
            }
            //savemenu.Visible = true;
            hdTotalMenu.Value = menucount.ToString();
        }
    }


    #region N Level Menu

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
                hdadminid.Value = (Request.QueryString["id"]).ToString();
                txtusername.Text = Server.HtmlDecode(dsadmin.Tables[0].Rows[0]["userid"].ToString());
                txtfname.Text = dsadmin.Tables[0].Rows[0]["firstname"].ToString();
                txtlname.Text = dsadmin.Tables[0].Rows[0]["lastname"].ToString();
                txtemail.Text = dsadmin.Tables[0].Rows[0]["emailaddress"].ToString();
                txtmobile.Text = dsadmin.Tables[0].Rows[0]["mobile"].ToString();
                bindadmintype();
                ddladmintype.SelectedValue = dsadmin.Tables[0].Rows[0]["admintypeid"].ToString(); //admintypeid
                if (Convert.ToString(dsadmin.Tables[0].Rows[0]["password"]) != "")
                {
                    reqtxtpassword.ValidationGroup = "";
                    reqtxtpassword.Enabled = false;
                    hfpass.Value = dsadmin.Tables[0].Rows[0]["password"].ToString();
                    txtpassword.Text = dsadmin.Tables[0].Rows[0]["password"].ToString();
                }
                //   txtpass.Attributes.Add("value", Convert.ToString(dtadmin.Rows[0]["password"]));
                // txtconfirmpass.Attributes.Add("value", Convert.ToString(dtadmin.Rows[0]["password"]));

                if (Convert.ToInt32(dsadmin.Tables[0].Rows[0]["isactive"]) != 0) { rbtActive.Checked = true; }
                else { rbtInactive.Checked = true; }


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
                    { strMenus.Append("<tr  style=\"background:#efefef;\">"); }
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
                        strMenus.Append("<label><div class='label_check'><input type=\"checkbox\" idmenu=\"" + dt.Rows[i]["idmenu"].ToString() + "\" checked=\"checked\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dt.Rows[i]["title"].ToString() + "\" title=\"" + dt.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dt.Rows[i]["title"].ToString() + "</label></div></label>");
                    }
                    else
                    {
                        strMenus.Append("<label><div class='label_check'><input type=\"checkbox\" idmenu=\"" + dt.Rows[i]["idmenu"].ToString() + "\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dt.Rows[i]["title"].ToString() + "\" title=\"" + dt.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dt.Rows[i]["title"].ToString() + "</label></div></label>");
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
                strSubMenus.Append("<table>");
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
                        strSubMenus.Append("<label style=\"" + style + "\"><div class='label_check'><input type=\"checkbox\" idmenu=\"" + dtSubMenu.Rows[i]["idmenu"].ToString() + "\" checked=\"checked\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" title=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dtSubMenu.Rows[i]["title"].ToString() + "</label></div></label>");
                    }
                    else
                    {
                        strSubMenus.Append("<label style=\"" + style + "\"><div class='label_check'><input type=\"checkbox\" idmenu=\"" + dtSubMenu.Rows[i]["idmenu"].ToString() + "\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" title=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dtSubMenu.Rows[i]["title"].ToString() + "</label></div></label>");
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

    #endregion

    //bind sub menu
    protected void BindSubMenus(CheckBoxList chksubmenulist, int idparent)
    {
        AdminManager objuser = new AdminManager();
        try
        {
            DataSet dtmenu = new DataSet();
            chksubmenulist.Items.Clear();
            chksubmenulist.ClearSelection();
            objuser.parentid = idparent;
            dtmenu = objuser.selectSubMenus();
            if (dtmenu.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dtmenu.Tables[0].Rows.Count; i++)
                {
                    chksubmenulist.Items.Add(dtmenu.Tables[0].Rows[i]["title"].ToString());
                    chksubmenulist.Items[i].Value = dtmenu.Tables[0].Rows[i]["idmenu"].ToString();

                    if (Request.QueryString["flag"] == "edit")
                        objuser.adminid = Convert.ToInt32(Request.QueryString["id"]);

                    objuser.idmenu = Convert.ToInt32(dtmenu.Tables[0].Rows[i]["idmenu"]);
                    if (objuser.getAdminmenu() > 0)
                    {
                        chksubmenulist.Items[i].Selected = true;
                    }
                }
            }
            dtmenu.Dispose();

        }
        catch (Exception ex) { throw ex; }
        finally { objuser = null; }
    }

    //validate user input
    protected Boolean validateInput()
    {
        AdminManager objadmin = new AdminManager();
        Boolean flag = true;
        objadmin.emailaddress = txtemail.Text.Trim();

        if (Request.QueryString["flag"] == "edit")
            objadmin.adminid = Convert.ToInt32(Request.QueryString["id"]);
        else
            objadmin.adminid = 0;

        if (flag == true)
        {
            if (objadmin.EmailIdExist())
            {
                lblmsg.Visible = true;

                lblmsgs.Text = "Although you indicated you're a new user, an account already exists for Email <b>" + txtemail.Text + "</b>";
                flag = false;
            }
            else
            {
                objadmin.userid = txtusername.Text.Trim();
                if (objadmin.UseridExist())
                {
                    //lblmsgs.Text = "Although you indicated you're a new user, an account already exists for username <b>" + txtuname.Text + "</b>";
                    flag = false;
                }
            }


        }

        return flag;
    }

    public void bindadmintype()
    {
        AdminManager objadmintype = new AdminManager();
        DataTable dttype = new DataTable();
        try
        {
            dttype = objadmintype.GetAllAdminType();
            ddladmintype.Items.Clear();
            if (dttype.Rows.Count > 0)
            {
                for (int i = 0; i < dttype.Rows.Count; i++)
                {
                    ListItem lisub = new ListItem(dttype.Rows[i]["typeName"].ToString(), dttype.Rows[i]["adminTypeId"].ToString());
                    ddladmintype.Items.Add(lisub);
                }
                ddladmintype.Items.Insert(0, new ListItem("--Select User Type --", "0"));
            }
            else
            {
                ddladmintype.Items.Insert(0, new ListItem("--No Type Available--", "0"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objadmintype = null; }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        AdminManager objadmin = new AdminManager();
        //START INSERT OR UPDATE ADMIN FIRST 
        string queryflg = string.Empty;
        int InsertedUserId = 0;

        if (Page.IsValid)
        {
            if (validateInput())
            {
                string imgName = string.Empty;
                objadmin.userid = txtusername.Text.Trim();
                if (txtpassword.Text != "")
                {
                    objadmin.password = txtpassword.Text.Trim();
                }
                else
                {
                    objadmin.password = hfpass.Value;
                }
                objadmin.firstname = Convert.ToString(txtfname.Text.Trim());
                objadmin.lastname = Convert.ToString(txtlname.Text.Trim());
                objadmin.emailaddress = Convert.ToString(txtemail.Text.Trim());
                objadmin.admintypeid = Convert.ToInt32(ddladmintype.SelectedValue);
                objadmin.mobile = Convert.ToString(txtmobile.Text);
                if (Convert.ToInt32(Request.QueryString["id"]) == 1)
                {
                    objadmin.isactive = 1;
                }
                else
                {
                    if (rbtActive.Checked == true)
                    {
                        objadmin.isactive = Convert.ToByte(1);
                    }
                    else
                    {
                        objadmin.isactive = Convert.ToByte(0);
                    }

                }

                if (Request.QueryString["flag"] == "edit")
                {
                    objadmin.adminid = Convert.ToInt32(Request.QueryString["id"]);

                    objadmin.UpdateItem();
                    InsertedUserId = Convert.ToInt32(Request.QueryString["id"]);
                    queryflg = "edit";
                }
                else
                {

                    InsertedUserId = objadmin.InsertItem();
                    queryflg = "add";

                    //send mail to user
                    SendMail(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsNewUserId"]));
                }

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

                    //    strQ += " INSERT INTO adminrights (idmenu,adminid) VALUES (" + Convert.ToInt32(Request.Form["hdMenuId" + (i + 1).ToString()]) + "," + Convert.ToInt32(InsertedUserId) + ") ";

                }
                objadmin.InsertUserMenu(strQ);
                //GenerateAdminMenu(InsertedUserId);
                generatemenuforadmin(InsertedUserId);

                Response.Redirect("viewadmin.aspx?flag=" + queryflg + "&key=" + Request.QueryString["key"]);
            }
        }
    }

    //generate menu file

    public void generatemenuforadmin(int adminid)
    {
        AdminManager objUser = new AdminManager();

        try
        {
            DataTable dtcat1 = new DataTable();
            objUser.adminid = adminid;
            dtcat1 = objUser.selectAdminParentMenus();
            string strMenu = "";
            strMenu += " <li><a href='home.aspx'><i class='fa fa-edit'></i><span>Home</span></a></li>";
            if (dtcat1.Rows.Count > 0)
            {
                for (int j = 0; j < dtcat1.Rows.Count; j++)
                {
                    strMenu += " <li><a href='" + dtcat1.Rows[j]["pageurl"] + "'><i class='fa fa-edit'></i><span>" + dtcat1.Rows[j]["title"] + "</span></a></li>";
                }
            }
            strMenu += " <li><a href='logout.ashx'><i class='fa fa-edit'></i><span>Logout</span></a></li>";

            string actualfolder = HttpContext.Current.Server.MapPath("../" + AppSettings.ADMIN_MENU_ROOTURL.ToString());
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

    public static void GenerateAdminMenu(int adminid)
    {
        AdminManager objUser = new AdminManager();
        try
        {
            DataTable dtcat1 = new DataTable();
            objUser.adminid = adminid;
            dtcat1 = objUser.selectAdminParentMenus();
            string strMenu = "";
            var Parent = (from p in dtcat1.AsEnumerable() select p);
            if (Parent != null)
            {
                strMenu += "<ul>";
                strMenu += "<li><a href='home.aspx' title='Home'><img src='images/icon6.png' style='vertical-align: top;width:20px;' alt=''>Home</a></li>";
                foreach (var row in Parent)
                {
                    string strsubmenu = BindSubCat(row.Field<int>("idmenu"), adminid);
                    if (strsubmenu == "<ul class='treeview-menu'></ul>")
                    {
                        strMenu += "<li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-circle-o'></i>" + row.Field<string>("title").ToString() + "</a>";
                        //strMenu += "<li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'>" + row.Field<string>("title").ToString() + "</a>";
                    }
                    else
                    {
                        strMenu += "<li class=\"link\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href=\"javascript:void(0);\"><i class='fa fa-circle-o'></i>" + row.Field<string>("title").ToString() + "</a>";
                        //strMenu += "<li class=\"link\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href=\"javascript:void(0);\">" + row.Field<string>("title").ToString() + "</a>";
                        strMenu += strsubmenu;
                    }

                    strMenu += "</li>";
                }
                strMenu += "<li><a href='logout.ashx'><img style='vertical-align: top;width:20px;' src='images/icon8.png' alt=''>Logout</a></li>";
                strMenu += "</ul>";
            }


            string actualfolder = HttpContext.Current.Server.MapPath("../" + AppSettings.ADMIN_MENU_ROOTURL.ToString());
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
            strmenu += "<ul class=\"nav main\">";
            if (dtcat2.Rows.Count > 0)
            {

                for (int j = 0; j < dtcat2.Rows.Count; j++)
                {
                    string strsubmenu = BindSubCat(Convert.ToInt32(dtcat2.Rows[j]["idmenu"]), adminid);
                    if (strsubmenu == "<ul class=\"nav main\"></ul>")
                    {
                        strmenu += "<li subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\">" + dtcat2.Rows[j]["title"].ToString() + "</a>";
                    }
                    else
                    {
                        strmenu += "<li  class=\"link\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"javascript:void(0);\">" + dtcat2.Rows[j]["title"].ToString() + "</a>";
                    }
                    strmenu += strsubmenu;
                    strmenu += "</li>";
                }

            }
            strmenu += "</ul>";
        }
        catch (Exception ex) { throw ex; }
        //finally { dtcat2 = null; objUser = null; }
        return strmenu;
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("viewadmin.aspx");
    }

    #region

    private void SendMail(int status)
    {
        orderManager objorder = new orderManager();

        DataTable GetData = new DataTable();
        GetData = objorder.GetSubject(status);
        if (GetData.Rows.Count > 0)
        {
            String strSubject = string.Empty;
            string strFrom = string.Empty;
            string strTo = string.Empty;
            string customername = string.Empty;
            string username = string.Empty;
            string password = string.Empty;

            strSubject = GetData.Rows[0]["EmailSubject"].ToString();
            strFrom = GetData.Rows[0]["FromEmail"].ToString();
            strTo = txtemail.Text.Trim();

            customername = txtfname.Text.Trim() + " " + txtlname.Text.Trim();
            username = txtusername.Text.Trim();
            password = txtpassword.Text.Trim();


            String strMsg = string.Empty;

            strMsg = GetData.Rows[0]["EmailBody"].ToString();

            strMsg = strMsg.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
            //string strMsgall = CommonFunctions.GetFileContents(Server.MapPath("../MailTemplate/NewUser.html"));
            strMsg = strMsg.Replace("##COMPANY##", System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"]);
            strMsg = strMsg.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
            strMsg = strMsg.Replace("##CustomerName##", customername);
            strMsg = strMsg.Replace("##UserName##", username);
            strMsg = strMsg.Replace("##Password##", password);

            if (strTo != null)
            {
                CommonFunctions.SendMail2(strFrom, strTo, "", strMsg, strSubject, "", "", "");
                //CommonFunctions.SendMail2(strFrom, "hardik@webtechsystem.com", "", strMsgall, strSubject, "", "", "");
            }


        }

    }

    #endregion

}