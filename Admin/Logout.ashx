<%@ WebHandler Language="C#" Class="Logout" %>
using System;
using System.Web;
using System.Web.SessionState;

public class Logout : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Session["adminId"] = null;
        context.Session["AdminName"] = null;
        if (Convert.ToString(context.Session["superadmin"]) == "superadmin")
        {
            context.Session["superadmin"] = null;
            context.Response.Redirect("default.aspx?ref=superadmin&msg=logout");
        }
        else
        {
            context.Response.Redirect("default.aspx");
        }

        //context.Response.Redirect("Default.aspx");

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}