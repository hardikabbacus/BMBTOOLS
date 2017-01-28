using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Admin_add_language : System.Web.UI.Page
{
    languageManager objlanguage = new languageManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add/Modify Language - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Add/Modify Language";
        if (!Page.IsPostBack)
        {
            Page.Title = "Add Language - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add Language";
            if (Request.QueryString["flag"] == "edit")
            {
                Title = "Modify Language - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify Language";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {
                        DataTable dtcontent = new DataTable();
                        objlanguage.languageId = Convert.ToInt32(Request.QueryString["id"]);
                        dtcontent = objlanguage.GetSinglelanguage();
                        if (dtcontent.Rows.Count > 0)
                        {
                            txtlanguadename.Text = Server.HtmlDecode(dtcontent.Rows[0]["languageName"].ToString());
                            txtalign.Text = Convert.ToString(dtcontent.Rows[0]["textAlign"].ToString());
                            //txtsortorder.Text = dtcontent.Rows[0]["sortorder"].ToString();
                            chkvisible.Checked = Convert.ToInt32(dtcontent.Rows[0]["isactive"]) == 1 ? true : false;
                            //hfprevsort.Value = dtcontent.Rows[0]["sortorder"].ToString();

                        }
                    }
                    else
                        Response.Redirect("viewlanguage.aspx");
                }
                else
                    Response.Redirect("viewlanguage.aspx");
            }
            //else
            //{
            //    txtsortorder.Text = Convert.ToString(CommonFunctions.GetLastSortCount("menu", "sortorder"));
            //}
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        lblmsgs.Text = "";
        if (Page.IsValid)
        {
            objlanguage.languageName = Server.HtmlEncode(txtlanguadename.Text);
            objlanguage.textAlign = Convert.ToChar(txtalign.Text.ToUpper());
            objlanguage.isactive = Convert.ToByte(chkvisible.Checked);


            if (Request.QueryString["flag"] == "edit")
            {
                objlanguage.languageId = Convert.ToInt32(Request.QueryString["id"]);
                if (objlanguage.TitleExist())
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "Language Name already exists.";
                    return;
                }

                objlanguage.UpdateItem();
                Response.Redirect("viewlanguage.aspx?flag=edit&key=" + Request.QueryString["key"]);
            }
            else
            {

                if (objlanguage.TitleExist())
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "Language Name already exists.";
                    return;
                }

                objlanguage.InsertItem();
                Response.Redirect("viewlanguage.aspx?flag=add&key=" + Request.QueryString["key"]);
            }
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("viewlanguage.aspx");
    }
}