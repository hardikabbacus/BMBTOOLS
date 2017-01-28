using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_services : System.Web.UI.Page
{
    ServicesManager objservice = new ServicesManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Bind Services

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
    }

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
                //   Response.Redirect("setup.aspx?flagservice=edit");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Services Information updated successfully')", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { objservice = null; }
        }
    }

}