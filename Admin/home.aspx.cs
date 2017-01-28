using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DashboardData();
        }
    }

    public void DashboardData()
    {
        dashbordManager objdash = new dashbordManager();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            ds = objdash.getDeshboardRecords();
            if (ds.Tables.Count > 0)
            {
                #region Life Time Sales

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblLifeTimeSale.Text = ds.Tables[0].Rows[0]["AllOverSeles"].ToString();
                }
                else { lblLifeTimeSale.Text = "0.00"; }
                #endregion

                #region Todays Sales

                if (ds.Tables[1].Rows.Count > 0)
                {
                    lblTodaysSales.Text = ds.Tables[1].Rows[0]["TodaysOrderSales"].ToString();
                }
                else { lblTodaysSales.Text = "0"; }
                #endregion

                #region Todays Order

                if (ds.Tables[2].Rows.Count > 0)
                {
                    lbltodaysOrder.Text = ds.Tables[2].Rows[0]["TodaysOrderCount"].ToString();

                }
                else { lbltodaysOrder.Text = "0"; }
                #endregion

                #region New Customer

                if (ds.Tables[3].Rows.Count > 0)
                {
                    gvNewCustomer.DataSource = ds.Tables[3];
                    gvNewCustomer.DataBind();
                }
                else
                {
                    gvNewCustomer.DataSource = dt;
                    gvNewCustomer.DataBind();
                }
                #endregion

                #region Recent Order

                if (ds.Tables[4].Rows.Count > 0)
                {
                    gvRecentOrder.DataSource = ds.Tables[4];
                    gvRecentOrder.DataBind();
                }
                else
                {
                    gvRecentOrder.DataSource = dt;
                    gvRecentOrder.DataBind();
                }
                #endregion

                #region Product Performance

                if (ds.Tables[5].Rows.Count > 0)
                {
                    gvProductPerformance.DataSource = ds.Tables[5];
                    gvProductPerformance.DataBind();
                }
                else
                {
                    gvProductPerformance.DataSource = dt;
                    gvProductPerformance.DataBind();
                }
                #endregion

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { ds.Dispose(); objdash = null; }
    }
}