using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_404 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string querystring = Request.Url.AbsoluteUri;
        string[] strvalue = querystring.Split('.');
        customerManager objcust = new customerManager();
        int compneyid = 0;
        for (int i = 0; i < strvalue.Length; i++)
        {
            objcust.companyName = Convert.ToString(strvalue[i].Replace("http://", ""));
            compneyid = objcust.GetCompanyIdFromCompanyName();
            if (compneyid > 0)
            {
                Response.Write("Company ID = " + compneyid);
                Response.End();
                break;
            }
            Response.Write(querystring.Replace("http://", ""));
            Response.End();

            //else
            //{
            //    Response.Write("testing 404.");
            //    Response.End();
            //}
        }
    }
}