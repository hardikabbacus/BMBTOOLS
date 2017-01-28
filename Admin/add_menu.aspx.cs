using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_add_menu : System.Web.UI.Page
{
    MenuManager objMenu = new MenuManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add/Modify Admin Menu - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Add/Modify Admin Menu";
        if (!Page.IsPostBack)
        {
            BindParentMenu();
            Page.Title = "Add Admin Menu - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add Admin Menu";
            if (Request.QueryString["flag"] == "edit")
            {
                Title = "Modify Admin Menu - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify Admin Menu";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {
                        DataTable dtcontent = new DataTable();
                        objMenu.idmenu = Convert.ToInt32(Request.QueryString["id"]);
                        dtcontent = objMenu.SelectSingleItemByMenuId();
                        if (dtcontent.Rows.Count > 0)
                        {
                            txtmenuname.Text = Server.HtmlDecode(dtcontent.Rows[0]["title"].ToString());
                            ddlParentMenu.SelectedValue = Convert.ToString(dtcontent.Rows[0]["parentid"].ToString());
                            txtPageName.Text = Server.HtmlDecode(dtcontent.Rows[0]["pageurl"].ToString());
                            txtImageName.Text = Server.HtmlDecode(dtcontent.Rows[0]["imagepath"].ToString());
                            txtsortorder.Text = dtcontent.Rows[0]["sortorder"].ToString();
                            chkvisible.Checked = Convert.ToInt32(dtcontent.Rows[0]["isactive"]) == 1 ? true : false;
                            hfprevsort.Value = dtcontent.Rows[0]["sortorder"].ToString();

                            //hdpageurl.Value = Server.HtmlDecode(dtcontent.Rows[0]["pageurl"].ToString());
                            //chkseparate.Checked = Convert.ToInt32(dtcontent.Rows[0]["isseparate"]) == 1 ? true : false;
                            //editorcontentsdesc1.Text = Server.HtmlDecode(dtcontent.Rows[0]["menuDesc"].ToString());

                        }
                    }
                    else
                        Response.Redirect("menu.aspx");
                }
                else
                    Response.Redirect("menu.aspx");
            }
            else
            {
                txtsortorder.Text = Convert.ToString(CommonFunctions.GetLastSortCount("menu", "sortorder"));
            }
        }
    }

    public void BindParentMenu()
    {
        DataTable dt = new DataTable();
        DataTable dtsub = new DataTable();
        try
        {
            dt = objMenu.GetParentMenu(true);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    //ListItem li = new ListItem("--Select Parent Menu--", i.ToString());
                    //ddlParentMenu.Items.Add(li);


                    ListItem li = new ListItem(dt.Rows[i]["title"].ToString(), dt.Rows[i]["idmenu"].ToString());
                    li.Attributes.Add("style", "color:#a7688a;font-weight:bold;");
                    ddlParentMenu.Items.Add(li);
                    objMenu.parentid = Convert.ToInt32(dt.Rows[i]["idmenu"].ToString());
                    dtsub = objMenu.GetSubMenu();
                    if (dtsub != null && dtsub.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtsub.Rows.Count; j++)
                        {
                            ListItem lisub = new ListItem("--" + dtsub.Rows[j]["title"].ToString(), dtsub.Rows[j]["idmenu"].ToString());
                            lisub.Attributes.Add("style", "color:#6dace5");
                            ddlParentMenu.Items.Add(lisub);

                        }
                    }
                }
                ddlParentMenu.Items.Insert(0, new ListItem("--Select Parent Menu--", "0"));
            }
            else
            {
                ddlParentMenu.Items.Insert(0, new ListItem("--No Menu Available--", "0"));
            }

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        lblmsgs.Text = "";
        if (Page.IsValid)
        {
            objMenu.title = Server.HtmlEncode(txtmenuname.Text);
            objMenu.sortorder = Convert.ToInt32(txtsortorder.Text);
            objMenu.ismenu = Convert.ToByte(true);
            objMenu.isactive = Convert.ToByte(chkvisible.Checked);
            objMenu.parentid = Convert.ToInt32(ddlParentMenu.SelectedValue);
            objMenu.imagepath = Server.HtmlEncode(txtImageName.Text);
            objMenu.isseparate = Convert.ToByte(0);
            objMenu.pageurl = Server.HtmlEncode(txtPageName.Text);
            objMenu.menudesc = "";

            //objMenu.isseparate = Convert.ToByte(chkseparate.Checked);
            //if (chkseparate.Checked == true)
            //{
            //    objMenu.pageurl = Server.HtmlEncode(txtPageName.Text);
            //}
            //else
            //{
            //    objMenu.pageurl = "";
            //}
            //objMenu.menudesc = Server.HtmlEncode(editorcontentsdesc1.Text);


            if (Request.QueryString["flag"] == "edit")
            {
                objMenu.idmenu = Convert.ToInt32(Request.QueryString["id"]);
                if (objMenu.TitleExist())
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "Menu Name already exists.";
                    return;
                }

                objMenu.UpdateItem(Convert.ToInt32(hfprevsort.Value), Convert.ToInt32(txtsortorder.Text));
                Response.Redirect("menu.aspx?flag=edit&key=" + Request.QueryString["key"]);
            }
            else
            {
                objMenu.idmenu = 0;
                if (objMenu.TitleExist())
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "Menu Name already exists.";
                    return;
                }

                objMenu.InsertItem();
                Response.Redirect("menu.aspx?flag=add&key=" + Request.QueryString["key"]);
            }
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("menu.aspx");
    }
}