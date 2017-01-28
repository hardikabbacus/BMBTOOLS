using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;

public partial class Admin_add_order : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(AppSettings.PAGESIZE);
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add Order - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        GVOrder.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Pagging"]);
        gvAdmin.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Pagging"]);
        ltrheading.Text = "Add Order";

        this.Form.DefaultButton = btnsearch.UniqueID;

        if (!IsPostBack)
        {
            BindProduct();

            if (Session["cust_name"] != null)
            {
                txtCustomer.Text = Session["cust_name"].ToString();
            }

            //if (Session["OrderId"] != null)
            //{
            //    GetOrderDetails();
            //}
            if (lblTotalPayAmmount.Text == "" || lblTotalPayAmmount.Text == "0.00 SAR")
            {
                lnkEdit.Enabled = false;
            }

            if (Request.QueryString["orderid"] != null)
            {
                GetOrderDetails();
                if (lblTotalPayAmmount.Text == "" || lblTotalPayAmmount.Text == "0.00 SAR")
                {
                    lnkEdit.Enabled = false;
                }
                else { lnkEdit.Enabled = true; }
                hidCustomerId.Value = Request.QueryString["customer"].ToString();
                if (Session["msg"].ToString() != "")
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = Session["msg"].ToString();
                }
            }
            if (Request.QueryString["chkid"] != null)
            {
                if (Request.QueryString["chkid"] == Convert.ToString(1)) { rbtCOD.Checked = true; lblDelivertType.Text = "Cash on delivery"; }
                if (Request.QueryString["chkid"] == Convert.ToString(2))
                {
                    string creditLimits = "";

                    customerManager objcust = new customerManager();
                    if (Session["cust_name"].ToString() != "")
                    {
                        objcust.contactName = Session["cust_name"].ToString();
                        objcust.companyName = Session["cust_name"].ToString();
                        creditLimits = objcust.GetCustomerCreditFromName();
                        if (lblTotalPayAmmount.Text != "")
                        {
                            if (Convert.ToDecimal(creditLimits.Trim()) < Convert.ToDecimal(lblTotalPayAmmount.Text.Replace("SAR", "").Trim()))
                            { rbtCOD.Checked = true; lblDelivertType.Text = "Cash on delivery"; }
                            else { rbtCredit.Checked = true; lblDelivertType.Text = "Credit"; }
                        }
                    }
                }
            }

            if (lblTotalPayAmmount.Text != "")
            { rbtCOD.Enabled = true; rbtCredit.Enabled = true; }

        }
    }

    //Bind product
    private void BindProduct(string search = "")
    {
        //this.Form.DefaultButton = imgbtnSearch.UniqueID;
        pageSize = Convert.ToInt32(2000);
        productManager objproduct = new productManager();
        DataSet dtadmin = new DataSet();
        try
        {

            if (txtSearchProduct.Text != "")
            {
                objproduct.productName = Server.HtmlEncode(txtSearchProduct.Text.Trim());
            }
            else
            {
                objproduct.productName = Server.HtmlEncode(txtSearchProduct.Text.Trim());
            }

            if (pageNo == 0) { pageNo = 1; }
            objproduct.pageNo = pageNo;
            objproduct.pageSize = pageSize;
            objproduct.SortExpression = SortExpression;

            dtadmin = objproduct.SearchOrder();
            totalrecs = objproduct.TotalRecord;
            if (pageNo == 1)
            {
                objproduct.pageNo = 1;
                pageNo = 1;
            }
            else if (pageNo == 0)
            {
                objproduct.pageNo = 1;
                pageNo = 1;
            }
            else
            {
                objproduct.pageNo = (pageNo - 1) * pageSize;
            }
            objproduct.pageSize = pageSize;
            totalpages = totalrecs / pageSize;
            if ((totalrecs % pageSize) > 0 && (totalrecs > pageSize)) { totalpages += 1; }
            GVOrder.DataSource = dtadmin;
            GVOrder.DataBind();
            if (dtadmin.Tables[0].Rows.Count > 0)
            {
                int startRowOnPage = (GVOrder.PageIndex * GVOrder.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + GVOrder.Rows.Count - 1;
                int totalRows = totalrecs;
                //ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "add_order.aspx");
            //ltrpaggingbottom.Text = strpaging;

        }
        catch (Exception ex) { throw ex; }
        finally { dtadmin.Dispose(); objproduct = null; }
    }

    // bind order
    private void GetOrderDetails()
    {
        orderManager objorder = new orderManager();
        DataTable dtorder = new DataTable();
        try
        {

            if (rbtCOD.Checked == true) { lblDelivertType.Text = "Cash on Delivery"; }
            if (rbtCredit.Checked == true) { lblDelivertType.Text = "Credit"; }

            //if (hidorderid.Value != "0")
            if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            }
            //else if (Session["OrderId"] != null)
            //{
            //    objorder.orderid = Convert.ToInt32(Session["OrderId"]);
            //}
            else
            {
                objorder.orderid = Convert.ToInt32(0);
            }

            if (pageNo == 0) { pageNo = 1; }
            objorder.pageNo = pageNo;
            objorder.pageSize = pageSize;
            objorder.SortExpression = SortExpression;

            dtorder = objorder.SearchOrderetaild();
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
                if (rbtCredit.Checked == true)
                {
                    decimal CreditLimit = 0;

                    CreditLimit = objorder.Getcreditlimitamountbycustomer(Convert.ToString(dtorder.Rows[0]["orderid"]));
                    if (CreditLimit != 0)
                    {
                        //lbltotal.Visible = true;
                        //lblcreaditamount.Visible = true;

                        // lbltotal.Text=Convert.ToString(dtorder.Rows[0]["TotalNetPrice"]);
                        //lblcreaditamount.Text=CreditLimit.ToString("0.00");

                        lblTotalPayAmmount.Text = Convert.ToString(Convert.ToDecimal(dtorder.Rows[0]["TotalNetPrice"]) - CreditLimit) + " SAR";
                    }
                }
                else
                {
                    //lbltotal.Visible = false;
                    //lblcreaditamount.Visible = false;
                    lblTotalPayAmmount.Text = Convert.ToString(dtorder.Rows[0]["TotalNetPrice"]) + " SAR";
                }
                int startRowOnPage = (gvAdmin.PageIndex * gvAdmin.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + gvAdmin.Rows.Count - 1;
                int totalRows = totalrecs;
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "add_order.aspx");
            ltrpaggingbottom.Text = strpaging;

        }
        catch (Exception ex) { throw ex; }
        finally { dtorder.Dispose(); objorder = null; }
    }

    //search for user selections
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        GVOrder.PageIndex = 0;
        BindProduct();
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

    protected void GVOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
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
            BindProduct();
        }
    }

    protected void gvAdmin_Sorting(object sender, GridViewSortEventArgs e)
    {
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
            GetOrderDetails();
        }
    }

    //handle row data bound
    protected void GVOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdmenuimage = new HiddenField();
            hdmenuimage = (HiddenField)e.Row.FindControl("hdmenuimage");

            HtmlImage imgMenu = new HtmlImage();
            imgMenu = (HtmlImage)e.Row.FindControl("imgMenu");

            HtmlAnchor imgAnc = new HtmlAnchor();
            imgAnc = (HtmlAnchor)e.Row.FindControl("ancImage");

            if (hdmenuimage.Value != "")
            {
                string strPath = Server.MapPath("../resources/product/thumb/") + hdmenuimage.Value.ToString();
                if (System.IO.File.Exists(strPath))
                {
                    imgMenu.Src = "../resources/product/thumb/" + hdmenuimage.Value.ToString();
                    imgAnc.HRef = "../resources/product/thumb/" + hdmenuimage.Value.ToString();
                }
                else
                {
                    imgMenu.Src = "../images/noimage.png";
                    imgAnc.HRef = "../images/noimage.png";
                }
            }
            else
            {
                imgMenu.Src = "../images/noimage.png";
                imgAnc.HRef = "../images/noimage.png";
            }
        }

    }

    //handle row data bound
    protected void gvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdmenuimageod = new HiddenField();
            hdmenuimageod = (HiddenField)e.Row.FindControl("hdmenuimageod");

            HtmlImage imgMenuod = new HtmlImage();
            imgMenuod = (HtmlImage)e.Row.FindControl("imgMenuod");

            HtmlAnchor imgAncod = new HtmlAnchor();
            imgAncod = (HtmlAnchor)e.Row.FindControl("ancImageod");

            if (hdmenuimageod.Value != "")
            {
                string strPath = Server.MapPath("../resources/product/thumb/") + hdmenuimageod.Value.ToString();
                if (System.IO.File.Exists(strPath))
                {
                    imgMenuod.Src = "../resources/product/thumb/" + hdmenuimageod.Value.ToString();
                    imgAncod.HRef = "../resources/product/thumb/" + hdmenuimageod.Value.ToString();
                }
                else
                {
                    imgMenuod.Src = "../images/noimage.png";
                    imgAncod.HRef = "../images/noimage.png";
                }
            }
            else
            {
                imgMenuod.Src = "../images/noimage.png";
                imgAncod.HRef = "../images/noimage.png";
            }
        }

    }

    private int GetSortColumnIndex()
    {
        // Iterate through the Columns collection to determine the index of the column being sorted.
        foreach (DataControlField field in GVOrder.Columns)
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
                    return GVOrder.Columns.IndexOf(field);
                }
            }
        }


        return -1;
    }

    private int GetSortColumnIndexOD()
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

        //product
        if (Convert.ToString(GVOrder.SortDirection) == sortdirec)
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

    public void AddSortImageOD(int columnIndex, GridViewRow row)
    {
        // Create the sorting image based on the sort direction.
        Image sortImage = new Image();
        string sortdirec = "";

        if (SortDirection == "asc") { sortdirec = "Ascending"; }
        else if (SortExpression.Contains("desc")) { sortdirec = "Decending"; }

        //product
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


    protected void btnAddProOrder_Click(object sender, EventArgs e)
    {
        Session["msg"] = "";
        string strCustomerName = txtCustomer.Text.Trim();
        int customer_id = 0;
        string creditLimits = "";
        string reducePercent = "";
        string GlobleDiscount = "";
        int order_id = 0;
        bool ordFlag = false;

        //decimal discount = Convert.ToDecimal(0.0);
        decimal netPayment = Convert.ToDecimal(0.0);

        customerManager objcustomer = new customerManager();
        DataTable dtcustomer = new DataTable();
        objcustomer.contactName = Convert.ToString(strCustomerName);
        objcustomer.companyName = Convert.ToString(strCustomerName);
        try
        {
            //customer_id = objcustomer.GetCustomerId();
            dtcustomer = objcustomer.GetCustomerIdAndDiscountCredit();
            if (dtcustomer.Rows.Count > 0)
            {
                customer_id = Convert.ToInt32(dtcustomer.Rows[0]["customerid"]);
                GlobleDiscount = dtcustomer.Rows[0]["globleDiscountRate"].ToString();
                creditLimits = dtcustomer.Rows[0]["creditLimit"].ToString();
                reducePercent = dtcustomer.Rows[0]["reducePercent"].ToString();
            }

        }
        catch (Exception c)
        {
            throw c;
        }
        finally { objcustomer = null; }

        foreach (GridViewRow row in GVOrder.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string lblProdutId = row.Cells[0].Controls.OfType<Label>().FirstOrDefault().Text;
                    string lblPrice = row.Cells[4].Controls.OfType<Label>().FirstOrDefault().Text;
                    string TxtQty = row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    string lblproName = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;
                    string lblCost = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;

                    orderManager objorder = new orderManager();
                    try
                    {
                        objorder.productid = Convert.ToInt32(lblProdutId);
                        int Inventory = objorder.GetInventoryByProductid();

                        if (Inventory >= Convert.ToInt32(TxtQty))
                        {

                            objorder.customerid = Convert.ToInt32(customer_id);
                            //objorder.orderstatus = Convert.ToInt32(1);

                            if (ordFlag == false && Request.QueryString["orderid"] == null && Convert.ToInt32(Request.QueryString["orderid"]) == 0)
                            {
                                if (rbtCOD.Checked == true) { objorder.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCase"]); }
                                if (rbtCredit.Checked == true) { objorder.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"]); }
                                objorder.contactName = Convert.ToString(strCustomerName);
                                order_id = objorder.InsertOrder(); // insert into order table
                                ordFlag = true;
                            }
                            else
                            {
                                if (Request.QueryString["orderid"] != null && Convert.ToInt32(Request.QueryString["orderid"]) != 0)
                                {
                                    order_id = Convert.ToInt32(Request.QueryString["orderid"]);
                                    ordFlag = true;
                                }
                            }

                            objorder.productid = Convert.ToInt32(lblProdutId);
                            objorder.productName = Server.HtmlEncode(Convert.ToString(lblproName));
                            objorder.globleDiscountRate = Convert.ToInt32(GlobleDiscount.Replace("%", ""));
                            objorder.costPrice = Convert.ToDecimal(lblCost);

                            objorder.price = Convert.ToDecimal(lblPrice);
                            objorder.qty = Convert.ToInt32(TxtQty);
                            objorder.orderid = Convert.ToInt32(order_id);
                            if (Convert.ToInt32(GlobleDiscount.Replace("%", "")) > 0)
                            {
                                objorder.finalPrice = Convert.ToDecimal(Convert.ToDecimal(lblPrice) - ((Convert.ToDecimal(lblPrice) * Convert.ToInt32(GlobleDiscount.Replace("%", "")) / 100)));
                                objorder.netprice = Convert.ToDecimal((Convert.ToDecimal(lblPrice) * Convert.ToInt32(TxtQty)) - ((Convert.ToDecimal(lblPrice) * Convert.ToInt32(TxtQty)) * Convert.ToInt32(GlobleDiscount.Replace("%", "")) / 100));
                            }
                            else
                            {
                                objorder.finalPrice = Convert.ToDecimal(lblPrice);
                                objorder.netprice = Convert.ToDecimal(Convert.ToDecimal(lblPrice) * Convert.ToInt32(TxtQty));
                            }

                            DataTable dtord = new DataTable();
                            dtord = objorder.GetOrderDetailByproIdordId();
                            if (dtord.Rows.Count > 0)
                            {
                                objorder.UpdateOrderDetail();
                            }
                            else
                            {
                                objorder.InsertOrderDetail();
                            }



                            if (Convert.ToInt32(GlobleDiscount.Replace("%", "")) > 0)
                            {
                                netPayment = Convert.ToDecimal(Convert.ToDecimal(netPayment) + (Convert.ToDecimal(objorder.netprice) - (Convert.ToDecimal(objorder.netprice) * Convert.ToInt32(GlobleDiscount.Replace("%", "")) / 100)));
                            }
                            else
                            {
                                netPayment = Convert.ToDecimal(Convert.ToDecimal(netPayment) + Convert.ToDecimal(objorder.netprice));
                            }
                        }
                        else
                        {
                            Session["msg"] = "Please enter quentity less than " + Inventory + " for this product : " + lblproName;
                            lblerror.Visible = true;
                            lblerrors.Text = "Please enter quentity less than " + Inventory + " for this product : " + lblproName;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { objorder = null; }
                }
            }
        }

        // add into order 
        if (ordFlag == true)
        {
            orderManager objordertotal = new orderManager();
            customerManager objcust = new customerManager();
            try
            {
                objordertotal.orderid = order_id;
                decimal TotalAmmount = objordertotal.GetSumOfItemPriceUsingOrderId();
                if (TotalAmmount != 0)
                {
                    netPayment = TotalAmmount;
                }

                objordertotal.totalammount = Convert.ToDecimal(netPayment);

                if (rbtCOD.Checked == true) { objordertotal.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCase"]); }
                if (rbtCredit.Checked == true)
                {
                    objordertotal.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"]);
                }

                objordertotal.contactName = Convert.ToString(strCustomerName);

                objordertotal.UpdateOrder();

                hidorderid.Value = Convert.ToString(order_id);
                Session["OrderId"] = Convert.ToString(order_id);
                Session["cust_name"] = Convert.ToString(txtCustomer.Text.Trim());

                GetOrderDetails();
                rbtCOD.Enabled = true;
                rbtCredit.Enabled = true;
                Response.Redirect("add_order.aspx?orderid=" + order_id + "&chkid=" + objordertotal.payType + "&customer=" + customer_id);
            }
            catch (Exception o)
            {
                throw o;
            }
            finally { objordertotal = null; objcust = null; }
        }
    }

    protected void btnConfirmOrder_Click(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();

        //if (Session["OrderId"] != null)
        //{
        if (Request.QueryString["orderid"] != null)
        {
            objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            int Ammount = objorder.getTotalAmountFromOrderId();
            if (Ammount != 0)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
                objorder.orderstatus = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OrderRecieved"]);
                if (rbtCOD.Checked == true) { objorder.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCase"]); }
                if (rbtCredit.Checked == true)
                {
                    objorder.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"]);
                }

                objorder.UpdateOrderStatusByOrderId();

                string strCustomerName = txtCustomer.Text.Trim();
                int customer_id;
                string creditLimits = "";
                string reducePercent = "";
                string GlobleDiscount = "";
                customerManager objcustomer = new customerManager();
                DataTable dtcustomer = new DataTable();
                objcustomer.contactName = Convert.ToString(strCustomerName);
                objcustomer.companyName = Convert.ToString(strCustomerName);
                try
                {
                    customer_id = objcustomer.GetCustomerId();
                    if (rbtCredit.Checked == true)
                    {
                        dtcustomer = objcustomer.GetCustomerIdAndDiscountCredit();
                        if (dtcustomer.Rows.Count > 0)
                        {
                            customer_id = Convert.ToInt32(dtcustomer.Rows[0]["customerid"]);
                            GlobleDiscount = dtcustomer.Rows[0]["globleDiscountRate"].ToString();
                            creditLimits = dtcustomer.Rows[0]["creditLimit"].ToString();
                            reducePercent = dtcustomer.Rows[0]["reducePercent"].ToString();
                        }

                        decimal TotalAmmount = objorder.GetSumOfItemPriceUsingOrderId();
                        string credits = creditLimits.ToString().Replace("SAR", "").Replace(".00", "");
                        objcustomer.creditLimit = Convert.ToString(Convert.ToInt32(credits) - Convert.ToInt32(TotalAmmount));
                        objcustomer.customerId = Convert.ToInt32(customer_id);
                        if (GlobleDiscount.Replace("%", "") == "0") { objcustomer.globleDiscountRate = "0"; }
                        else
                        {
                            objcustomer.globleDiscountRate = Convert.ToString(Convert.ToInt32(GlobleDiscount.Replace("%", "")) - Convert.ToInt32(5)) + "%";
                        }
                        objcustomer.UpdateCustomerCreditLimits();
                    }
                }
                catch (Exception c)
                {
                    throw c;
                }
                finally { objcustomer = null; }

                SendMail(Convert.ToInt32(Request.QueryString["orderid"]), customer_id, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsOrderReceivedId"]));

                Session["cust_name"] = "";

                //Response.Redirect("vieworder.aspx?orderid=" + Request.QueryString["orderid"]);
                Response.Redirect("vieworder.aspx");
            }
            else
            {
                //Response.Write("<script>alert('There are no items avaliable in order');</script>");
                lblerrMsg.Text = "There are no items avaliable in order.";
            }
        }

    }

    protected void imgbtnDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvAdmin.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string lblorderdetailid = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    string lblnetprice = row.Cells[7].Controls.OfType<Label>().FirstOrDefault().Text;

                    orderManager objorder = new orderManager();

                    try
                    {
                        objorder.orderdetailid = Convert.ToInt32(lblorderdetailid);
                        objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);

                        decimal TotalAmmount = objorder.GetTotalAmmountFromOrderId();
                        TotalAmmount = Convert.ToDecimal(TotalAmmount - Convert.ToDecimal(lblnetprice));

                        objorder.totalammount = TotalAmmount;

                        objorder.UpdateOrder();
                        objorder.DeleteOrderDetails();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { objorder = null; }

                }
            }
        }
        GetOrderDetails(); // bind data
    }

    protected void rbtCOD_CheckedChanged(object sender, EventArgs e)
    {
        GetOrderDetails();
        lblDelivertType.Text = "Cash on Delivery";
    }

    protected void rbtCredit_CheckedChanged(object sender, EventArgs e)
    {
        GetOrderDetails();
        lblDelivertType.Text = "Credit";
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        customerManager objcust = new customerManager();
        orderManager objorder = new orderManager();
        DataTable dtcustomer = new DataTable();
        try
        {
            if (Request.QueryString["customer"] != null)
            {
                objcust.customerId = Convert.ToInt32(Request.QueryString["customer"]);
                decimal CreditLimits = objcust.GetCustomerCreditFromID();
                string reducePercent = "";
                string GlobleDiscount = "";

                dtcustomer = objcust.GetCustomerIdAndDiscountCreditByID();
                if (dtcustomer.Rows.Count > 0)
                {
                    GlobleDiscount = dtcustomer.Rows[0]["globleDiscountRate"].ToString();
                    reducePercent = dtcustomer.Rows[0]["reducePercent"].ToString();
                }
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
                decimal TotalAmmount = objorder.GetSumOfItemPriceUsingOrderId();

                if (Request.QueryString["chkid"].ToString() != System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"].ToString())
                {

                    if (Convert.ToDecimal(CreditLimits) > Convert.ToDecimal(lblTotalPayAmmount.Text.Replace("SAR", "").Trim()))
                    {
                        //string credits = creditLimits.ToString().Replace("SAR", "").Replace(".00", "");
                        objcust.creditLimit = Convert.ToString(Convert.ToInt32(CreditLimits) - Convert.ToInt32(TotalAmmount));
                        if (GlobleDiscount.Replace("%", "") == "0")
                        { objcust.globleDiscountRate = "0"; }
                        else
                        {
                            objcust.globleDiscountRate = Convert.ToString(Convert.ToInt32(GlobleDiscount.Replace("%", "")) - Convert.ToInt32(5)) + "%";
                        }
                        objcust.UpdateCustomerCreditLimits();

                        //order paytype update
                        objorder.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"]);
                        objorder.updatePayType();

                        Response.Redirect("add_order.aspx?orderid=" + Request.QueryString["orderid"] + "&chkid" + Request.QueryString["chkid"] + "&customer=" + Request.QueryString["customer"]);
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsgs.Text = "Your credit limits is lower than total payment.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "invokeButtonClick();", true);
                    }
                }
                // if cash on delivery
                if (Request.QueryString["chkid"].ToString() != System.Configuration.ConfigurationManager.AppSettings["PayTypeCase"].ToString())
                {
                    objcust.creditLimit = Convert.ToString(Convert.ToInt32(CreditLimits) + Convert.ToInt32(TotalAmmount));
                    objcust.globleDiscountRate = Convert.ToString(Convert.ToInt32(GlobleDiscount.Replace("%", "")) + Convert.ToInt32(5)) + "%";
                    objcust.UpdateCustomerCreditLimits();

                    //order paytype update
                    objorder.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCase"]);
                    objorder.updatePayType();

                    Response.Redirect("add_order.aspx?orderid=" + Request.QueryString["orderid"] + "&chkid" + Request.QueryString["chkid"] + "&customer=" + Request.QueryString["customer"]);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally { objorder = null; objcust = null; dtcustomer.Dispose(); }
    }


    private void SendMail(int orderid, int customer, int status)
    {
        orderManager objorder = new orderManager();
        DataTable getcustomer = new DataTable();
        getcustomer = objorder.GetCustomerDetail(customer);
        try
        {
            if (getcustomer.Rows.Count > 0)
            {
                string strto = string.Empty;
                string[] Discount = getcustomer.Rows[0]["Discount"].ToString().Split('%');

                strto = getcustomer.Rows[0]["Email"].ToString();


                DataTable GetData = new DataTable();
                GetData = objorder.GetSubject(status);

                if (GetData.Rows.Count > 0)
                {
                    String strSubject = string.Empty;
                    string strFrom = string.Empty;
                    strSubject = GetData.Rows[0]["EmailSubject"].ToString();
                    strFrom = GetData.Rows[0]["FromEmail"].ToString();

                    String strMsg1 = string.Empty;
                    String strMsg = string.Empty;

                    DataTable getOrderDetail = new DataTable();
                    getOrderDetail = objorder.GetOrderDetail(orderid);
                    if (getOrderDetail.Rows.Count > 0)
                    {
                        decimal TotalDiscount = 0;
                        decimal Subtotal = 0;

                        strMsg1 += "<tr>";
                        strMsg1 += "<td style=\"background:#fefefe;border-bottom: 1px solid #ccc;\">";
                        strMsg1 += "<table>";
                        strMsg1 += "<tr>";
                        strMsg1 += "<td colspan=\"2\" style=\"text-align:justify;padding-bottom:5px;float:left;line-height:20px;width:400px; padding:10px 20px;\">";
                        strMsg1 += "<h1 style=\" font-size:16px; color:#231f20;\">Order Detail</h1>";
                        strMsg1 += "</td>";
                        strMsg1 += "<td colspan=\"2\" style=\"text-align:justify;padding-bottom:5px;float:left;line-height:20px;width:200px; padding:10px 20px;\">";
                        strMsg1 += "<h1 style=\" font-size:12px; color:#231f20;\">Order Number: " + getOrderDetail.Rows[0]["orderid"] + "</h1>";
                        strMsg1 += "<h2 style=\" font-size:12px; color:#231f20;\">Date:" + getOrderDetail.Rows[0]["OrderDate"] + "</h2>";
                        strMsg1 += "</td>";
                        strMsg1 += "</tr>";
                        strMsg1 += "</table>";
                        strMsg1 += "</td>";
                        strMsg1 += "</tr>";

                        strMsg1 += "<tr>";
                        strMsg1 += "<td>";
                        strMsg1 += "<table style=\"width:100%;font-family:Arial, Helvetica, sans-serif; font-size:12px; padding:10px;\">";
                        strMsg1 += "<tr style=\"text-align:left;height: 30px;\">";
                        strMsg1 += "<th>Product Name</th>";
                        strMsg1 += "<th>Unit Price</th>";
                        strMsg1 += "<th>Qty</th>";
                        strMsg1 += "<th>Subtotal</th>";
                        strMsg1 += "</tr>";

                        for (int i = 0; i < getOrderDetail.Rows.Count; i++)
                        {
                            strMsg1 += "<tr style=\"height:30px;\">";
                            strMsg1 += "<td>" + Server.HtmlDecode(getOrderDetail.Rows[i]["ProductName"].ToString()) + "</td>";
                            strMsg1 += "<td style=\"width:130px;\">" + getOrderDetail.Rows[i]["Price"] + "<span style=\"text-decoration:line-through; float:right; width:50px; color:#888;\">" + getOrderDetail.Rows[i]["Cost"] + "</span></td>";
                            strMsg1 += "<td>" + getOrderDetail.Rows[i]["Qty"] + "</td>";
                            //strMsg1 += "<td>" + getOrderDetail.Rows[i]["SubTotal"] + "</td>";
                            strMsg1 += "<td>" + Convert.ToString(Convert.ToInt32(getOrderDetail.Rows[i]["Price"]) * Convert.ToInt32(getOrderDetail.Rows[i]["Qty"])) + "</td>";
                            strMsg1 += "</tr>";
                            Subtotal += Convert.ToDecimal(Convert.ToDecimal(getOrderDetail.Rows[i]["Price"]) * Convert.ToDecimal(getOrderDetail.Rows[i]["Qty"]));
                        }
                        TotalDiscount = Subtotal * Convert.ToDecimal(Discount[0]) / 100;

                        strMsg1 += "<table style=\"width:250px;font-family:Arial, Helvetica, sans-serif; font-size:12px; padding:10px; float:right; background:#f1f1f1;\">";
                        strMsg1 += "<tr style=\"border-bottom:1px solid #ccc; display:inline-block; width:100%;height: 30px;margin-bottom: 5px; \">";
                        strMsg1 += "<td style=\"width:130px;\">Subtotal</td>";
                        //strMsg1 += "<td>" + getOrderDetail.Rows[0]["TotalNetPrice"] + "SR" + "</td>";
                        strMsg1 += "<td>" + Subtotal + "SR" + "</td>";
                        strMsg1 += "</tr>";

                        strMsg1 += "<tr style=\"border-bottom:1px solid #ccc;display:inline-block;width:100%;height: 30px;margin-bottom: 5px; \">";
                        strMsg1 += "<td style=\"width: 130px;\">Total Discount</td>";
                        strMsg1 += "<td>" + TotalDiscount + "SR" + "</td>";
                        strMsg1 += "</tr>";

                        strMsg1 += "<tr style=\"display:inline-block;width:100%;height: 30px;margin-bottom: 5px;\">";
                        strMsg1 += "<td style=\"width: 130px; font-size:16px;\">Grand Total</td>";
                        //strMsg1 += "<td style=\"font-size:16px;\">" + Convert.ToString(Convert.ToInt32(getOrderDetail.Rows[0]["TotalNetPrice"]) - 100 + "SR") + "</td>";
                        strMsg1 += "<td style=\"font-size:16px;\">" + Convert.ToDecimal(Subtotal - TotalDiscount) + "SR" + "</td>";
                        strMsg1 += "</tr>";
                        strMsg1 += "</table>";


                        strMsg = GetData.Rows[0]["EmailBody"].ToString();
                        strMsg = strMsg.Replace("##CONTENT##", strMsg1);
                        strMsg = strMsg.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
                        string strMsgall = CommonFunctions.GetFileContents(Server.MapPath("../MailTemplate/bmb-Mail.html"));
                        strMsgall = strMsgall.Replace("##COMPANYNAME##", System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"]);
                        strMsgall = strMsgall.Replace("##CONTENT##", strMsg);

                        if (strto != null)
                        {
                            //CommonFunctions.SendMail2(strFrom, strto, "", strMsgall, strSubject, "", "", "");
                        }

                    }
                }

            }
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    protected void btndiscard_Click(object sender, EventArgs e)
    {
        Response.Redirect("vieworder.aspx");
    }
}