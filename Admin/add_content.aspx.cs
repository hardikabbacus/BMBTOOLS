using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_add_content : System.Web.UI.Page
{
    ContentManager objcontent = new ContentManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add/Modify CMS - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Add/Modify CMS";
        if (!Page.IsPostBack)
        {

            Page.Title = "Add CMS - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add CMS";
            if (Request.QueryString["flag"] == "edit")
            {
                Title = "Modify CMS - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify CMS";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {
                        DataTable dtcontent = new DataTable();
                        objcontent.Id = Convert.ToInt32(Request.QueryString["id"]);
                        dtcontent = objcontent.SelectSingleItemById();
                        if (dtcontent.Rows.Count > 0)
                        {
                            txtcontentname.Text = Server.HtmlDecode(dtcontent.Rows[0]["ContentTitle"].ToString());
                            txtdesc.Text = Server.HtmlDecode(dtcontent.Rows[0]["ContentDesc"].ToString());
                            txtmetatitle.Text = Server.HtmlDecode(dtcontent.Rows[0]["MetaTitle"].ToString());
                            txtmetakey.Text = Server.HtmlDecode(dtcontent.Rows[0]["MetaKeyword"].ToString());
                            txtmetadesc.Text = Server.HtmlDecode(dtcontent.Rows[0]["MetaDescription"].ToString());
                            txtsortorder.Text = dtcontent.Rows[0]["DisplayRank"].ToString();
                            chkvisible.Checked = Convert.ToInt32(dtcontent.Rows[0]["DisplayStatus"]) == 1 ? true : false;
                            hfprevsort.Value = dtcontent.Rows[0]["DisplayRank"].ToString();

                        }
                    }
                    else
                        Response.Redirect("viewcontent.aspx");
                }
                else
                    Response.Redirect("viewcontent.aspx");
            }
            else
            {
                txtsortorder.Text = Convert.ToString(CommonFunctions.GetLastSortCount("tblContentCms", "DisplayRank"));
            }
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        lblmsgs.Text = "";
        if (Page.IsValid)
        {
            objcontent.ContentTitle = Server.HtmlEncode(txtcontentname.Text);
            objcontent.ContentDesc = Server.HtmlEncode(txtdesc.Text);
            objcontent.MetaTitle = Server.HtmlEncode(txtmetatitle.Text);
            objcontent.MetaKeyword = Server.HtmlEncode(txtmetakey.Text);
            objcontent.MetaDescription = Server.HtmlEncode(txtmetadesc.Text);
            objcontent.DisplayRank = Convert.ToInt32(txtsortorder.Text);
            objcontent.DisplayStatus = Convert.ToBoolean(chkvisible.Checked);

            if (Request.QueryString["flag"] == "edit")
            {
                objcontent.Id = Convert.ToInt32(Request.QueryString["id"]);
                if (objcontent.TitleExist())
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "Content name already exists.";
                    return;
                }

                objcontent.UpdateItem(Convert.ToInt32(hfprevsort.Value), Convert.ToInt32(txtsortorder.Text));
                Response.Redirect("viewcontent.aspx?flag=edit&key=" + Request.QueryString["key"]);
            }
            else
            {
                objcontent.Id = 0;
                if (objcontent.TitleExist())
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "Content name already exists.";
                    return;
                }

                objcontent.InsertItem();
                Response.Redirect("viewcontent.aspx?flag=add&key=" + Request.QueryString["key"]);
            }
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("viewcontent.aspx");
    }
}