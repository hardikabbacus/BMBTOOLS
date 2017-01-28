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

public partial class Admin_viewpayment : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ManagePaymentPageSize"]);
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
        Page.Title = "Payment List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        gvAdmin.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ManagePaymentPageSize"]);
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

            getYear();
            BindPayment();

            if (CommonFunctions.IsQueryString("pageSize", true))
            {
                ddlpageSize.SelectedValue = Request.QueryString["pageSize"];
                pageSize = Convert.ToInt32(Request.QueryString["pageSize"]);
            }
            else
            {
                pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ManagePaymentPageSize"]);
                ddlpageSize.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["ManagePaymentPageSize"].ToString();
            }

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

    //Bind order
    private void BindPayment(string search = "")
    {
        this.Form.DefaultButton = imgbtnSearch.UniqueID;

        paymentManager objPay = new paymentManager();
        DataTable dtorder = new DataTable();
        try
        {
            if (txtsearch.Text != "")
            {
                objPay.commonsearch = txtsearch.Text.Trim();
            }
            //else if (Request.QueryString["orderid"] != null)
            //{
            //    objPay.commonsearch = Convert.ToInt32(Request.QueryString["orderid"]);
            //}
            else { objPay.commonsearch = ""; }

            //year filter
            if (chk2015.Checked == true && chk2016.Checked == true) { objPay.firstyear = Convert.ToInt32(PervYear.InnerText); objPay.lastyear = Convert.ToInt32(currYear.InnerText); }
            else if (chk2015.Checked == true) { objPay.firstyear = Convert.ToInt32(PervYear.InnerText); }
            else if (chk2016.Checked == true) { objPay.firstyear = Convert.ToInt32(currYear.InnerText); }
            else { objPay.firstyear = 0; objPay.lastyear = 0; }


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
            if (startmonth != 0) { objPay.startmonth = startmonth; } else { objPay.startmonth = startmonth; }
            if (endmonth != 0) { objPay.endmonth = endmonth; } else { objPay.endmonth = endmonth; }


            if (pageNo == 0) { pageNo = 1; }
            objPay.pageNo = pageNo;
            objPay.pageSize = pageSize;
            objPay.SortExpression = SortExpression;
            querystring = "&pageSize=" + ddlpageSize.SelectedValue + "&key=" + txtsearch.Text;
            dtorder = objPay.SearchItem();
            totalrecs = objPay.TotalRecord;
            if (pageNo == 1)
            {
                objPay.pageNo = 1;
                pageNo = 1;
            }
            else if (pageNo == 0)
            {
                objPay.pageNo = 1;
                pageNo = 1;
            }
            else
            {
                objPay.pageNo = (pageNo - 1) * pageSize;
            }
            objPay.pageSize = pageSize;
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
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "viewpayment.aspx");
            ltrpaggingbottom.Text = strpaging;
            //Ltrup.Text = strpaging;
            //LoadDropDownList();
        }
        catch (Exception ex) { throw ex; }
        finally { dtorder.Dispose(); objPay = null; }

    }

    //search for user selections
    protected void imgbtnSearch_Click(object sender, EventArgs e)
    {
        //trmsg.Visible = false;
        gvAdmin.PageIndex = 0;
        pageNo = Convert.ToInt32(Request.QueryString["p"]);
        pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        BindPayment();
    }

    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        BindPayment();
    }

    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            ddlpageSize.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    ////handle grid view page chenging
    //protected void gvAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvAdmin.PageIndex = e.NewPageIndex;
    //    BindPayment();
    //    //ddlpage.SelectedIndex = e.NewPageIndex;
    //}

    //handle grid view page chenging
    protected void gvAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAdmin.PageIndex = e.NewPageIndex;
        BindPayment();
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
            BindPayment();
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


    protected void btnpay_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            paymentManager objpay = new paymentManager();
            objpay.orderid = Convert.ToInt32(txtPopOrderno.Text.Trim());
            objpay.payammount = Convert.ToDecimal(txtPopAmmount.Text.Trim());
            objpay.paynotes = Convert.ToString(txtPopNots.Text.Trim());
            objpay.paystatus = Convert.ToInt32(1);
            if (hidCustid.Value != "0")
            {
                objpay.customerid = Convert.ToInt32(hidCustid.Value);
            }
            else
            {
                objpay.contactName = Convert.ToString(txtPOPCustomer.Text);
                int Custid = objpay.GetCustomerIdByCustomerName();
                objpay.customerid = Convert.ToInt32(Custid);
            }
            objpay.InsertPayment();
            BindPayment();
        }
        else
        {

        }
    }

    #region  ------------   YEAR   --------------

    protected void chk2015_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk2016_CheckedChanged(object sender, EventArgs e) { BindPayment(); }

    #endregion

    #region  --------------   MONTH   ------------

    protected void chk1_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk2_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk3_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk4_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk5_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk6_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk7_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk8_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk9_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk10_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk11_CheckedChanged(object sender, EventArgs e) { BindPayment(); }
    protected void chk12_CheckedChanged(object sender, EventArgs e) { BindPayment(); }

    #endregion


    protected void imgbtnDelete_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            paymentManager objpay = new paymentManager();
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
                        objpay.paymentid = Convert.ToInt32(gvAdmin.DataKeys[gvAdmin.Rows[i].RowIndex].Value.ToString());
                        objpay.DeletePaymentRecord();
                    }
                }
                BindPayment();

                lblmsg.Visible = true;
                lblmsgs.Text = "Payment details deleted successfully";
            }
            catch (Exception ex) { throw ex; }
            finally { objpay = null; }
        }
    }

    protected void btnpayupdate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            paymentManager objpay = new paymentManager();
            objpay.paymentid = Convert.ToInt32(hidPaymentId.Value);
            objpay.orderid = Convert.ToInt32(txtPopOrderno.Text.Trim());
            objpay.payammount = Convert.ToDecimal(txtPopAmmount.Text.Trim());
            objpay.paynotes = Convert.ToString(txtPopNots.Text.Trim());
            objpay.paystatus = Convert.ToInt32(1);

            if (hidCustid.Value != "0")
            {
                objpay.customerid = Convert.ToInt32(hidCustid.Value);
            }
            else
            {
                objpay.contactName = Convert.ToString(txtPOPCustomer.Text);
                int Custid = objpay.GetCustomerIdByCustomerName();
                objpay.customerid = Convert.ToInt32(Custid);
            }
            objpay.UpdatePayment();
            BindPayment();

        }
    }
}