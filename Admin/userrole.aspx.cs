using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_userrole : System.Web.UI.Page
{

    public int menucount = 0;
    public int submenucount = 0;
    public int turncount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Title = "User Role - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "User Role";

            DataTable dtSeleectedMenu = new DataTable();
            BindMenuEditMode(dtSeleectedMenu);
            BindUserType();

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
            hdTotalMenu.Value = menucount.ToString();
        }
    }

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
                    strMenus.Append(" <div class='label_check'><input type=\"hidden\" name=\"hdMenuId" + menucount.ToString() + "\" id=\"hdMenuId" + menucount.ToString() + "\" value=\"" + dt.Rows[i]["idmenu"].ToString() + "\" /></div>");
                    if (cnt > 0)
                    {
                        strMenus.Append("<div class='label_check'><input type=\"checkbox\" idmenu=\"" + dt.Rows[i]["idmenu"].ToString() + "\" checked=\"checked\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dt.Rows[i]["title"].ToString() + "\" title=\"" + dt.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dt.Rows[i]["title"].ToString() + "</label></div>");
                    }
                    else
                    {
                        strMenus.Append("<div class='label_check'><input type=\"checkbox\" idmenu=\"" + dt.Rows[i]["idmenu"].ToString() + "\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dt.Rows[i]["title"].ToString() + "\" title=\"" + dt.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dt.Rows[i]["title"].ToString() + "</label></div>");
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
                    strSubMenus.Append("<div class='label_check'><input type=\"hidden\" name=\"hdMenuId" + menucount.ToString() + "\" id=\"hdMenuId" + menucount.ToString() + "\" value=\"" + dtSubMenu.Rows[i]["idmenu"].ToString() + "\" /></div>");
                    if (cnt > 0)
                    {
                        strSubMenus.Append("<div class='label_check'><input type=\"checkbox\" idmenu=\"" + dtSubMenu.Rows[i]["idmenu"].ToString() + "\" checked=\"checked\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" title=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dtSubMenu.Rows[i]["title"].ToString() + "</label></div>");
                    }
                    else
                    {
                        strSubMenus.Append("<div class='label_check'><input type=\"checkbox\" idmenu=\"" + dtSubMenu.Rows[i]["idmenu"].ToString() + "\" id=\"chkPermission" + menucount.ToString() + "\" name=\"chkPermission" + menucount.ToString() + "\" alt=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" title=\"" + dtSubMenu.Rows[i]["title"].ToString() + "\" /><label for=\"chkPermission" + menucount.ToString() + "\">" + dtSubMenu.Rows[i]["title"].ToString() + "</label></div>");
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
            //objUser.adminid = adminid;
            objUser.admintypeid = adminid;
            dtcat1 = objUser.selectAdminParentMenus();
            string strMenu = "";
            var Parent = (from p in dtcat1.AsEnumerable() select p);
            if (Parent != null)
            {
                //strMenu += "<ul>";
                //strMenu += "<li class='treeview active'><a href='home.aspx' title='Home'><i class='fa fa-edit'></i><span>Home</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a></li>";
                strMenu += " <li><a href='home.aspx'><i class='fa fa-home'></i><span>Home</span></a></li>";
                foreach (var row in Parent)
                {
                    string strsubmenu = BindSubCat(row.Field<int>("idmenu"), adminid);
                    if (strsubmenu == "<ul class='treeview-menu'></ul>")
                    {
                        // if hr 03_10_2016
                        if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Product"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-cubes'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MasterProduct"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-cubes'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Category"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-asterisk'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Brand"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-bookmark'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SetUp"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-asterisk'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Customer"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-user'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Inventory"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-tasks'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Images"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-camera'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ManageUser"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-user'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ManageUser"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-user'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Importjobs"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-upload'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CMS"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-list'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else
                        {
                            //strMenu += "<li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-edit'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-edit'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                    }
                    else
                    {
                        // if hr 03_10_2016
                        if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Product"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-cubes'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MasterProduct"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-cubes'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Category"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-asterisk'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Brand"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-bookmark'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SetUp"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-asterisk'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Customer"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-user'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Inventory"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-tasks'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Images"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-camera'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ManageUser"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-user'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ManageUser"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-user'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Importjobs"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-upload'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else if (row.Field<string>("title").ToString() == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CMS"]))
                        {
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-list'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
                        }
                        else
                        {
                            //strMenu += "<li class=\"link\" idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-edit'></i><span>" + row.Field<string>("title").ToString() + "</span><span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span></a>";
                            strMenu += " <li idmenu=\"" + row.Field<int>("idmenu").ToString() + "\"><a href='" + row.Field<string>("pageurl").ToString() + "'><i class='fa fa-edit'></i><span>" + row.Field<string>("title").ToString() + "</span></a></li>";
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
            strmenu += "<ul class=\"treeview-menu\">";
            if (dtcat2.Rows.Count > 0)
            {

                for (int j = 0; j < dtcat2.Rows.Count; j++)
                {
                    string strsubmenu = BindSubCat(Convert.ToInt32(dtcat2.Rows[j]["idmenu"]), adminid);
                    if (strsubmenu == "<ul class=\"treeview-menu\"></ul>")
                    {
                        strmenu += "<li class='active' subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"" + dtcat2.Rows[j]["pageurl"].ToString() + "\"><i class='fa fa-circle-o'></i>" + dtcat2.Rows[j]["title"].ToString() + "</a>";
                    }
                    else
                    {
                        strmenu += "<li  class=\"\" subcatid=\"" + dtcat2.Rows[j]["idmenu"].ToString() + "\"><a href=\"javascript:void(0);\"><i class='fa fa-circle-o'></i>" + dtcat2.Rows[j]["title"].ToString() + "</a>";
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

    #endregion

    protected void btnsavechange_Click(object sender, EventArgs e)
    {
        AdminManager objadmin = new AdminManager();
        string queryflg = string.Empty;
        int InsertedUserId = 0;

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
            //    strQ += " INSERT INTO adminrights (idmenu,adminid) VALUES (" + Convert.ToInt32(Request.Form["hdMenuId" + (i + 1).ToString()]) + "," + Convert.ToInt32(InsertedUserId) + ") ";
        }
        objadmin.InsertUserMenu(strQ);
        GenerateAdminMenu(InsertedUserId);
        //generatemenuforadmin(InsertedUserId);
    }
}