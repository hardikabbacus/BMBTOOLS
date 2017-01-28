using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_add_usertype : System.Web.UI.Page
{
    usertypeManager objusertype = new usertypeManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add/Modify User Type - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Add/Modify User Type";
        if (!Page.IsPostBack)
        {

            Page.Title = "Add User Type - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add User Type";
            if (Request.QueryString["flag"] == "edit")
            {
                Title = "Modify User Type - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify User Type";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {
                        DataTable dtcontent = new DataTable();
                        objusertype.adminTypeId = Convert.ToInt32(Request.QueryString["id"]);
                        dtcontent = objusertype.SelectSingleItemById();
                        if (dtcontent.Rows.Count > 0)
                        {
                            txtadmintype.Text = Server.HtmlDecode(dtcontent.Rows[0]["typeName"].ToString());
                            txtsortorder.Text = dtcontent.Rows[0]["sortorder"].ToString();
                            chkvisible.Checked = Convert.ToInt32(dtcontent.Rows[0]["isactive"]) == 1 ? true : false;
                            hfprevsort.Value = dtcontent.Rows[0]["sortorder"].ToString();

                        }
                    }
                    else
                        Response.Redirect("viewusertype.aspx");
                }
                else
                    Response.Redirect("viewusertype.aspx");
            }
            else
            {
                txtsortorder.Text = Convert.ToString(CommonFunctions.GetLastSortCount("adminType", "sortorder"));
            }
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        lblmsgs.Text = "";
        if (Page.IsValid)
        {
            objusertype.typeName = Server.HtmlEncode(txtadmintype.Text);
            objusertype.sortorder = Convert.ToInt32(txtsortorder.Text);
            objusertype.isactive = Convert.ToByte(chkvisible.Checked);

            if (Request.QueryString["flag"] == "edit")
            {
                objusertype.adminTypeId = Convert.ToInt32(Request.QueryString["id"]);
                if (objusertype.TitleExist())
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "User type already exists.";
                    return;
                }

                objusertype.UpdateItem(Convert.ToInt32(hfprevsort.Value), Convert.ToInt32(txtsortorder.Text));
                Response.Redirect("viewusertype.aspx?flag=edit&key=" + Request.QueryString["key"]);
            }
            else
            {
                objusertype.adminTypeId = 0;
                if (objusertype.TitleExist())
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "User Type already exists.";
                    return;
                }

                objusertype.InsertItem();
                Response.Redirect("viewusertype.aspx?flag=add&key=" + Request.QueryString["key"]);
            }
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("viewusertype.aspx");
    }
}