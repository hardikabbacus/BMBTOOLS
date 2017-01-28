using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Admin_vieworder : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ViewOrderPageSize"]);
    string id = "";
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";
    int startmonth = 0;
    int endmonth = 0;
    int firstyear = 0;
    int lastyear = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Order List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        gvAdmin.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ViewOrderPageSize"]);
        //txtsearch.Focus();
        if (!IsPostBack)
        {
            #region SET PAGE NUMBER

            if (Request.QueryString["p"] == null) { pageNo = 1; }
            else if (Request.QueryString["p"] == "") { pageNo = 1; }
            else if (Convert.ToInt32(Request.QueryString["p"]) <= 0) { pageNo = 1; }
            else { pageNo = Convert.ToInt32(Request.QueryString["p"]); }
            #endregion

            BingpageSize();

            BindOrder();
            BindStatus();
            BindStatusForChnageStatus();
            getYear();
            
            Session["msg"] = "";
            Session["msgOrd"] = "";

            if (CommonFunctions.IsQueryString("pageSize", true))
            {
                ddlpageSize.SelectedValue = Request.QueryString["pageSize"];
                pageSize = Convert.ToInt32(Request.QueryString["pageSize"]);
            }
            else
            {
                pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ViewOrderPageSize"]);
                ddlpageSize.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["ViewOrderPageSize"].ToString();
            }
            Session["cust_name"] = "";
        }
    }

    //get year
    public void getYear()
    {
        int currentYear = DateTime.Today.Year;
        int PerviousYear = DateTime.Today.Year - 1;
        currYear.InnerText = currentYear.ToString();
        PervYear.InnerText = PerviousYear.ToString();
        //for (int i = 1; i >= 0; i--)
        //{
        //    // Now just add an entry that's the current year minus the counter
        //    //chkyear.Items.Add((currentYear - i).ToString());
        //}
    }

    //// get month
    //public void getMonth()
    //{
    //    var currentMonth = DateTime.Today.Month;
    //    for (int i = 1; i <= 12; i++)
    //    {
    //        chkMonth.Items.Add(i.ToString());
    //    }
    //}

    // bind status
    public void BindStatus()
    {
        orderManager objord = new orderManager();
        DataTable dtord = new DataTable();
        try
        {
            dtord = objord.GetAllStatus();
            if (dtord.Rows.Count > 0)
            {
                ddlstatus.DataSource = dtord;
                ddlstatus.DataTextField = "orderstatus";
                ddlstatus.DataValueField = "id";
                ddlstatus.DataBind();
                ddlstatus.Items.Insert(0, new ListItem("-Select Status-", "0"));
            }
            else
            {
                ddlstatus.Items.Insert(0, new ListItem("-Select Status-", "0"));
            }
        }
        catch (Exception r)
        {
            throw r;
        }
        finally { dtord.Dispose(); objord = null; }
    }

    // get status for change status dropdown list
    public void BindStatusForChnageStatus()
    {
        orderManager objord = new orderManager();
        DataTable dtord = new DataTable();
        try
        {
            dtord = objord.GetAllStatus();
            if (dtord.Rows.Count > 0)
            {
                ddlchangestatus.DataSource = dtord;
                ddlchangestatus.DataTextField = "orderstatus";
                ddlchangestatus.DataValueField = "id";
                ddlchangestatus.DataBind();
                ddlchangestatus.Items.Insert(0, new ListItem("Change Status to", "0"));
            }
            else
            {
                ddlchangestatus.Items.Insert(0, new ListItem("Change Status to", "0"));
            }
        }
        catch (Exception r)
        {
            throw r;
        }
        finally { dtord.Dispose(); objord = null; }
    }

    //Bind order
    private void BindOrder(string search = "")
    {
        this.Form.DefaultButton = imgbtnSearch.UniqueID;
        orderManager objorder = new orderManager();
        DataTable dtorder = new DataTable();
        try
        {
            if (txtsearch.Text != "")
            {
                objorder.orderid = Convert.ToInt32(txtsearch.Text.Trim());
            }
            else if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            }
            else { objorder.orderid = 0; }

            if (txtcontact.Text != "") { objorder.contactName = txtcontact.Text.Trim(); } else { objorder.contactName = ""; }

            if (ddlstatus.SelectedValue != "")
            {
                objorder.statusid = Convert.ToInt32(ddlstatus.SelectedValue);
            }
            else
            {
                objorder.statusid = Convert.ToInt32(0);
            }

            //year filter
            if (chk2015.Checked == true && chk2016.Checked == true) { objorder.firstyear = Convert.ToInt32(PervYear.InnerText); objorder.lastyear = Convert.ToInt32(currYear.InnerText); }
            else if (chk2015.Checked == true) { objorder.firstyear = Convert.ToInt32(PervYear.InnerText); }
            else if (chk2016.Checked == true) { objorder.firstyear = Convert.ToInt32(currYear.InnerText); }
            else { objorder.firstyear = 0; objorder.lastyear = 0; }



            // month filter
            string strMonth = string.Empty;

            if (chk1.Checked == true) { strMonth += 1 + ","; }
            if (chk2.Checked == true) { strMonth += 2 + ","; }
            if (chk3.Checked == true) { strMonth += 3 + ","; }
            if (chk4.Checked == true) { strMonth += 4 + ","; }
            if (chk5.Checked == true) { strMonth += 5 + ","; }
            if (chk6.Checked == true) { strMonth += 6 + ","; }
            if (chk7.Checked == true) { strMonth += 7 + ","; }
            if (chk8.Checked == true) { strMonth += 8 + ","; }
            if (chk9.Checked == true) { strMonth += 9 + ","; }
            if (chk10.Checked == true) { strMonth += 10 + ","; }
            if (chk11.Checked == true) { strMonth += 11 + ","; }
            if (chk12.Checked == true) { strMonth += 12 + ","; }

            if (strMonth != "")
            {
                strMonth = strMonth.TrimEnd(',');
                var strTemp = strMonth.Split(',');
                for (int i = 0; i < strTemp.Count(); i++)
                {
                    if (strTemp.Count() >= 2)
                    {
                        endmonth = Convert.ToInt32(strTemp[i]);
                    }

                    if (i == 0)
                    {
                        startmonth = Convert.ToInt32(strTemp[i]);
                    }

                }
            }
            if (startmonth != 0) { objorder.startmonth = startmonth; } else { objorder.startmonth = startmonth; }
            if (endmonth != 0) { objorder.endmonth = endmonth; } else { objorder.endmonth = endmonth; }


            if (pageNo == 0) { pageNo = 1; }
            objorder.pageNo = pageNo;
            objorder.pageSize = pageSize;
            objorder.SortExpression = SortExpression;
            querystring = "&pageSize=" + ddlpageSize.SelectedValue + "&key=" + txtsearch.Text;
            dtorder = objorder.SearchItem();
            totalrecs = objorder.TotalRecord;
            if (pageNo == 1)
            {
                objorder.pageNo = 1;
                pageNo = 1;
            }
            else if (pageNo == 0)
            {
                objorder.pageNo = 1;
                pageNo = 1;
            }
            else
            {
                objorder.pageNo = (pageNo - 1) * pageSize;
            }
            objorder.pageSize = pageSize;
            totalpages = totalrecs / pageSize;
            if ((totalrecs % pageSize) > 0 && (totalrecs > pageSize)) { totalpages += 1; }
            gvAdmin.DataSource = dtorder;
            gvAdmin.DataBind();
            if (dtorder.Rows.Count > 0)
            {
                int startRowOnPage = (gvAdmin.PageIndex * gvAdmin.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + gvAdmin.Rows.Count - 1;
                int totalRows = totalrecs;
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "vieworder.aspx");
            ltrpaggingbottom.Text = strpaging;
            //Ltrup.Text = strpaging;
            //LoadDropDownList();
        }
        catch (Exception ex) { throw ex; }
        finally { dtorder.Dispose(); objorder = null; }

    }

    //search for user selections
    protected void imgbtnSearch_Click(object sender, EventArgs e)
    {
        //trmsg.Visible = false;
        gvAdmin.PageIndex = 0;
        pageNo = Convert.ToInt32(Request.QueryString["p"]);
        pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        BindOrder();
    }

    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        BindOrder();
    }

    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            ddlpageSize.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    //handle grid view page chenging
    protected void gvAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAdmin.PageIndex = e.NewPageIndex;
        BindOrder();
        //ddlpage.SelectedIndex = e.NewPageIndex;
    }

    //handle row data bound
    protected void gvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Call the GetSortColumnIndex helper method to determine the index of the column being sorted.
            int sortColumnIndex = GetSortColumnIndex();
            if (sortColumnIndex != -1)
            {
                //Call the AddSortImage helper method to add a sort direction image to the appropriate column header.
                AddSortImage(sortColumnIndex, e.Row);
            }
        }
    }

    private string SortDirection
    {
        get
        {
            if (ViewState["SortDirection"] == null) { ViewState["SortDirection"] = String.Empty; }
            return ViewState["SortDirection"].ToString();//hfSortdirection.Value.ToString(); 
        }
        set
        {
            ViewState["SortDirection"] = value;
            //hfSortdirection.Value = value; 
        }
    }

    private string SortExpression
    {
        get
        {
            if (ViewState["SortExpression"] == null) { ViewState["SortExpression"] = String.Empty; }
            return ViewState["SortExpression"].ToString();
        }
        set { ViewState["SortExpression"] = value; }
    }

    protected void gvAdmin_Sorting(object sender, GridViewSortEventArgs e)
    {

        if (ddlpageSize.SelectedValue != "")
        {
            pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        }
        //if (ddlpage.SelectedValue != "")
        //{
        //    pageNo = Convert.ToInt32(ddlpage.SelectedValue);
        //}
        if (e.SortExpression.ToString() != string.Empty)
        {

            if (SortDirection.ToLower() == "asc")
            {
                SortDirection = "desc";
            }
            else
            {
                SortDirection = "asc";
            }

            SortExpression = e.SortExpression + " " + SortDirection;
            BindOrder();
        }
    }
    private int GetSortColumnIndex()
    {
        // Iterate through the Columns collection to determine the index of the column being sorted.
        foreach (DataControlField field in gvAdmin.Columns)
        {
            if (field.SortExpression != "" && field.SortExpression != null)
            {
                string sortexp = "";
                if (SortExpression.Contains("asc"))
                {
                    sortexp = SortExpression.Replace("asc", "");
                }
                else if (SortExpression.Contains("desc"))
                {
                    sortexp = SortExpression.Replace("desc", "");
                }

                if (field.SortExpression.ToString().ToLower() == sortexp.ToLower().Trim())
                {
                    return gvAdmin.Columns.IndexOf(field);
                }
            }
        }
        return -1;
    }

    // This is a helper method used to add a sort direction image to the header of the column being sorted.
    public void AddSortImage(int columnIndex, GridViewRow row)
    {
        // Create the sorting image based on the sort direction.
        Image sortImage = new Image();
        string sortdirec = "";

        if (SortDirection == "asc") { sortdirec = "Ascending"; }
        else if (SortExpression.Contains("desc")) { sortdirec = "Decending"; }

        if (Convert.ToString(gvAdmin.SortDirection) == sortdirec)
        {
            sortImage.ImageUrl = "img/table/sort_asc.png";
        }
        else
        {
            sortImage.ImageUrl = "img/table/sort_desc.png";
        }

        // Add the image to the appropriate header cell.
        row.Cells[columnIndex].Controls.Add(sortImage);
    }


    // update multipal orders order status
    protected void btnApplyStatus_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            orderManager objorder = new orderManager();
            try
            {
                int con = 0;
                CheckBox chk = new CheckBox();
                for (int i = 0; i < gvAdmin.Rows.Count; i++)
                {
                    chk = (CheckBox)(gvAdmin.Rows[i].FindControl("chkDelete"));
                    if (chk.Checked == true)
                    {
                        con += 1;
                        objorder.orderid = Convert.ToInt32(gvAdmin.DataKeys[gvAdmin.Rows[i].RowIndex].Value.ToString());
                        objorder.orderstatus = Convert.ToInt32(ddlchangestatus.SelectedValue);
                        objorder.UpdateOrderStatusByOrderId();

                    }
                }
                BindOrder();

                lblmsg.Visible = true;
                lblmsgs.Text = "Order status changed successfully";
            }
            catch (Exception ex) { throw ex; }
            finally { objorder = null; }
        }
    }

    #region   ---- year checkbox checked check ----

    protected void chk2015_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk2016_CheckedChanged(object sender, EventArgs e) { BindOrder(); }

    #endregion

    #region   -----  month checkbox checked check -----

    protected void chk1_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk2_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk3_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk4_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk5_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk6_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk7_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk8_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk9_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk10_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk11_CheckedChanged(object sender, EventArgs e) { BindOrder(); }
    protected void chk12_CheckedChanged(object sender, EventArgs e) { BindOrder(); }

    #endregion


}