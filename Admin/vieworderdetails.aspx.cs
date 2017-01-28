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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;



public partial class Admin_vieworderdetails : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(AppSettings.PAGESIZE);
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Single Order - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        gvAdmin.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Pagging"]);
        GVOrder.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Pagging"]);
        ltrheading.Text = "View Order";

        //this.Form.DefaultButton = btnsearch.UniqueID;
        if (!IsPostBack)
        {
            BindProduct();

            if (Request.QueryString["orderid"] != null)
            {
                lblOrderNo.Text = "Order no : " + Request.QueryString["orderid"].ToString();
                GetOrderDetails();
                getOrderstatus();
                if (Request.QueryString["customer"] != null)
                {
                    getCustomerAndCompanyDetails();
                }

                if (Session["msgOrd"].ToString() != "")
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = Session["msgOrd"].ToString();
                }
            }
        }
    }

    // get customer and company details
    public void getCustomerAndCompanyDetails()
    {
        customerManager objcustomer = new customerManager();
        DataTable dtcustomer = new DataTable();
        objcustomer.customerId = Convert.ToInt32(Request.QueryString["customer"]);
        try
        {
            dtcustomer = objcustomer.GetCustomerDetailsForOrderView();
            if (dtcustomer.Rows.Count > 0)
            {
                string StrContent = string.Empty;
                StrContent += "<ul>";
                StrContent += "<li><label>Company Name : </label><p>" + dtcustomer.Rows[0]["companyName"].ToString() + "</p></li>";
                StrContent += "<li><label>Shipping Address : </label><p>" + dtcustomer.Rows[0]["streetAddress"].ToString() + "</p></li>";
                StrContent += "<li><label>Contact Person : </label><p>" + dtcustomer.Rows[0]["contactName"].ToString() + "</p></li>";
                StrContent += "<li><label>Contact Phone : </label><p>" + dtcustomer.Rows[0]["mobile"].ToString() + "</p></li>";
                StrContent += "</ul>";

                ltrContact.Text = StrContent;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objcustomer = null; dtcustomer = null; }


    }

    public void getOrderstatus()
    {
        orderManager objorder = new orderManager();
        DataTable dtorder = new DataTable();
        int chkstatus = 0;
        try
        {
            if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            }
            chkstatus = objorder.GetOrderStatus();
            if (chkstatus != 0)
            {
                if (chkstatus == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OrderRecieved"]))
                {
                    //lnkorderreceived.ForeColor = System.Drawing.Color.Green;
                    lnkorderreceived.Enabled = false;
                    lnkrecive.Attributes.Add("class", "active");
                }
                else if (chkstatus == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PreparingOrder"]))
                {
                    //lnkpreparingorder.ForeColor = System.Drawing.Color.Green; 
                    lnkorderreceived.ForeColor = System.Drawing.Color.Black;
                    lnkorderreceived.Enabled = false; lnkpreparingorder.Enabled = false;
                    lnkpreorder.Attributes.Add("class", "active");
                }
                else if (chkstatus == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Shipped"]))
                {
                    //lnkshipped.ForeColor = System.Drawing.Color.Green; 
                    lnkorderreceived.ForeColor = System.Drawing.Color.Black; lnkpreparingorder.ForeColor = System.Drawing.Color.Black;
                    lnkorderreceived.Enabled = false; lnkpreparingorder.Enabled = false; lnkshipped.Enabled = false;
                    lnkpreorder.Attributes.Add("class", "active"); lnkship.Attributes.Add("class", "active");
                }
                else if (chkstatus == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Delivered"]))
                {
                    //lnkdelivered.ForeColor = System.Drawing.Color.Green; 
                    lnkshipped.ForeColor = System.Drawing.Color.Black; lnkorderreceived.ForeColor = System.Drawing.Color.Black; lnkpreparingorder.ForeColor = System.Drawing.Color.Black;
                    lnkorderreceived.Enabled = false; lnkpreparingorder.Enabled = false; lnkshipped.Enabled = false; lnkdelivered.Enabled = false;
                    lnkpreorder.Attributes.Add("class", "active"); lnkship.Attributes.Add("class", "active"); lnkdelivery.Attributes.Add("class", "active");

                    btnCancelorder.Visible = false;
                    btnSendOrderMail.Visible = false;
                    btnOrderpdf.Visible = false;
                    btnMakepaid.Visible = false;
                    btnSaveChanges.Visible = false;
                    btnRemoveItem.Visible = false;
                    btnEditOrder.Visible = false;
                    lnkEdit.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally { dtorder.Dispose(); objorder = null; }
    }

    // bind single order
    private void GetOrderDetails()
    {
        orderManager objorder = new orderManager();
        DataTable dtorder = new DataTable();
        try
        {
            pageSize = 100;
            if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            }
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
                lblTotalPayAmmount.Text = Convert.ToString(dtorder.Rows[0]["TotalNetPrice"]) + " SAR";
                hidPayType.Value = dtorder.Rows[0]["payType"].ToString();
                if (dtorder.Rows[0]["payType"].ToString() == System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"]) { lblDelivertType.Text = "Credit"; }
                else { lblDelivertType.Text = "Cash on delivery"; }
                int startRowOnPage = (gvAdmin.PageIndex * gvAdmin.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + gvAdmin.Rows.Count - 1;
                int totalRows = totalrecs;
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "vieworderdetails.aspx");
            ltrpaggingbottom.Text = strpaging;

        }
        catch (Exception ex) { throw ex; }
        finally { dtorder.Dispose(); objorder = null; }
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

            HiddenField hidOrderStatus = new HiddenField();
            hidOrderStatus = (HiddenField)e.Row.FindControl("hidOrderStatus");
            ImageButton imgDelete = (e.Row.FindControl("imgDelete") as ImageButton);
            CheckBox chkDelete = (e.Row.FindControl("chkDelete") as CheckBox);
            if (hidOrderStatus.Value == System.Configuration.ConfigurationManager.AppSettings["Delivered"])
            { imgDelete.Enabled = false; chkDelete.Visible = false; }

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
        System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
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

    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        CheckBox chk = (sender as CheckBox);
        if (chk.ID == "chkHeader")
        {
            foreach (GridViewRow row in gvAdmin.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                }
            }
        }
        CheckBox chkAll = (gvAdmin.HeaderRow.FindControl("chkHeader") as CheckBox);
        chkAll.Checked = true;
        foreach (GridViewRow row in gvAdmin.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                for (int i = 1; i < row.Cells.Count; i++)
                {

                    if (row.Cells[i].Controls.OfType<TextBox>().ToList().Count > 0)
                    {
                        count++;
                        row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                        row.Cells[i].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                    }

                    if (!isChecked)
                    {
                        chkAll.Checked = false;
                    }
                }
            }
        }
        if (count > 0)
        {
            btnSaveChanges.Visible = true;
            btnRemoveItem.Visible = true;
        }
        else
        {
            btnSaveChanges.Visible = false;
            btnRemoveItem.Visible = false;
        }
    }

    protected void OnCheckedChangedInner(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvAdmin.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                for (int i = 1; i < row.Cells.Count; i++)
                {

                    if (row.Cells[i].Controls.OfType<TextBox>().ToList().Count > 0)
                    {

                        row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                        row.Cells[i].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                    }

                }
            }
        }
    }

    #region -- Remove Item ------

    protected void btnRemoveItem_Click(object sender, EventArgs e)
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

        lblmsg.Visible = true;
        lblmsgs.Text = "Item has been removed successfully.";
    }

    #endregion

    #region ----- Edit Order -------

    protected void btnEditOrder_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkAll = (gvAdmin.HeaderRow.FindControl("chkHeader") as CheckBox);
            if (chkAll.Checked == true)
            { chkAll.Checked = false; }
            else { chkAll.Checked = true; }

            foreach (GridViewRow row in gvAdmin.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chkAll.Checked;
                }
            }
            int count = 0;
            foreach (GridViewRow row in gvAdmin.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Controls.OfType<TextBox>().ToList().Count > 0)
                        {
                            count++;
                            row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                            row.Cells[i].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;

                            btnSaveChanges.Visible = true;
                            btnRemoveItem.Visible = true;
                        }


                        if (!isChecked)
                        {
                            chkAll.Checked = false;
                        }
                    }
                }
            }

            if (count > 0)
            {
                btnSaveChanges.Visible = true;
                btnRemoveItem.Visible = true;
            }
            else
            {
                btnSaveChanges.Visible = false;
                btnRemoveItem.Visible = false;
            }
        }
        catch (Exception ex)
        {

            throw;
        }


    }

    #endregion

    #region --- Cancel Order ---

    protected void btnCancelorder_Click(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();
        try
        {
            if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
                objorder.orderstatus = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CancelOrder"]);
                objorder.UpdateOrderStatusByOrderId();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objorder = null; }

        lblmsg.Visible = true;
        lblmsgs.Text = "Order has been canceled successfully..";

    }

    #endregion

    #region  ---- Save Change ----

    protected void btnSaveChanges_Click(object sender, EventArgs e)
    {
        Session["msgOrd"] = "";
        decimal netPayment = Convert.ToDecimal(0.0);
        bool ordFlag = false;
        int Discount = 0;

        customerManager objcustomer = new customerManager();
        DataTable dtcustomer = new DataTable();
        objcustomer.customerId = Convert.ToInt32(Request.QueryString["customer"]);
        try
        {
            //customer_id = objcustomer.GetCustomerId();
            dtcustomer = objcustomer.GetdDiscountCredit();
            if (dtcustomer.Rows.Count > 0)
            {
                //customer_id = Convert.ToInt32(dtcustomer.Rows[0]["customerid"]);
                Discount = Convert.ToInt32(dtcustomer.Rows[0]["globleDiscountRate"].ToString().Replace("%", ""));
            }

        }
        catch (Exception c)
        {
            throw c;
        }
        finally { objcustomer = null; }

        foreach (GridViewRow row in gvAdmin.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    orderManager objorder = new orderManager();
                    try
                    {
                        int productid = Convert.ToInt32(row.Cells[0].Controls.OfType<Label>().FirstOrDefault().Text);
                        int Qty = Convert.ToInt32(row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Text);
                        decimal price = Convert.ToDecimal(row.Cells[5].Controls.OfType<Label>().FirstOrDefault().Text);
                        //int Discount = Convert.ToInt32(row.Cells[6].Controls.OfType<Label>().FirstOrDefault().Text);
                        string ProductName = Convert.ToString(row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text);
                        decimal Cost = Convert.ToDecimal(row.Cells[9].Controls.OfType<Label>().FirstOrDefault().Text);

                        objorder.productid = Convert.ToInt32(productid);
                        int Inventory = objorder.GetInventoryByProductid();

                        if (Inventory >= Convert.ToInt32(Qty))
                        {
                            objorder.price = Convert.ToDecimal(price);
                            objorder.qty = Convert.ToInt32(Qty);
                            objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
                            objorder.productName = Server.HtmlEncode(Convert.ToString(ProductName));
                            objorder.costPrice = Convert.ToDecimal(Cost);
                            objorder.globleDiscountRate = Convert.ToInt32(Discount);

                            if (Convert.ToInt32(Discount) > 0)
                            {
                                objorder.finalPrice = Convert.ToDecimal(Convert.ToDecimal(price) - ((Convert.ToDecimal(price) * Convert.ToInt32(Discount) / 100)));
                                objorder.netprice = Convert.ToDecimal((Convert.ToDecimal(price) * Convert.ToInt32(Qty)) - ((Convert.ToDecimal(price) * Convert.ToInt32(Qty)) * Convert.ToInt32(Discount) / 100));
                            }
                            else
                            {
                                objorder.finalPrice = Convert.ToDecimal(price);
                                objorder.netprice = Convert.ToDecimal(Convert.ToDecimal(price) * Convert.ToInt32(Qty));
                            }
                            //objorder.netprice = Convert.ToDecimal(Convert.ToDecimal(price) * Convert.ToInt32(Qty));

                            objorder.UpdateOrderDetail();
                            if (Convert.ToInt32(Discount) > 0)
                            {
                                netPayment = Convert.ToDecimal(Convert.ToDecimal(netPayment) + (Convert.ToDecimal(objorder.netprice) - (Convert.ToDecimal(objorder.netprice) * Convert.ToInt32(Discount) / 100)));
                            }
                            else
                            {
                                netPayment = Convert.ToDecimal(Convert.ToDecimal(netPayment) + Convert.ToDecimal(objorder.netprice));
                            }
                            ordFlag = true;
                        }
                        else
                        {
                            Session["msgOrd"] = "Please enter quentity less than " + Inventory + " for this product : " + ProductName;
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
            try
            {
                objordertotal.totalammount = Convert.ToDecimal(netPayment);
                objordertotal.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
                objordertotal.UpdateOrder();

                hidorderid.Value = Convert.ToString(Request.QueryString["orderid"]);
                Session["OrderId"] = Convert.ToString(Request.QueryString["orderid"]);

                objordertotal.orderstatus = Convert.ToInt32(1);
                objordertotal.UpdateOrderStatusByOrderId();
                //GetOrderDetails();
                //Response.Redirect("add_order.aspx?orderid=" + order_id);
            }
            catch (Exception o)
            {
                throw o;
            }
            finally { objordertotal = null; }
        }

        GetOrderDetails(); // bind data

        lblmsg.Visible = true;
        lblmsgs.Text = "Order has been updated successfully.";

        // old save 
        if (Request.QueryString["orderid"] != null)
        {
            orderManager objorder = new orderManager();
            try
            {
                objorder.orderid = Convert.ToInt32(Session["OrderId"]);
                int Ammount = objorder.getTotalAmountFromOrderId();
                if (Ammount != 0)
                {
                    Response.Redirect("vieworderdetails.aspx?orderid=" + Request.QueryString["orderid"] + "&customer=" + Request.QueryString["customer"]);
                }
                else { Response.Write("<script>alert('There are no items avaliable in order');</script>"); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { objorder = null; }
        }
        //end old save
    }

    #endregion

    #region  ---- Remove single record ------

    protected void imgDelete_Click(object sender, ImageClickEventArgs e)
    {
        orderManager objprodelete = new orderManager();
        ImageButton lnkRemove = (ImageButton)sender;
        Session["msgOrd"] = "";
        try
        {
            // delete item from tblorderdetail table
            objprodelete.orderdetailid = Convert.ToInt32(lnkRemove.CommandArgument);
            objprodelete.DeleteOrderDetails();

            // update order total ammount in tblorder
            objprodelete.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            decimal TotalAmmount = objprodelete.GetSumOfItemPriceUsingOrderId();
            objprodelete.totalammount = Convert.ToDecimal(TotalAmmount);
            objprodelete.UpdateOrder();

            decimal total = objprodelete.GetTotalAmmountFromOrderId();
            if (Convert.ToString(total) == "0.00")
            {
                objprodelete.orderstatus = Convert.ToInt32(0);
                objprodelete.UpdateOrderStatusByOrderId();
            }

            btnSaveChanges.Visible = false;
            btnRemoveItem.Visible = false;

        }
        catch (Exception o)
        {
            throw o;
        }
        finally { objprodelete = null; }

        GetOrderDetails();
        lblmsg.Visible = true;
        lblmsgs.Text = "Item has been removed successfully.";
    }

    #endregion

    #region --- Bind PopUp Product -----

    //Bind product
    private void BindProduct(string search = "")
    {
        //this.Form.DefaultButton = imgbtnSearch.UniqueID;

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
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "add_order.aspx");
            ltrpaggingbottom.Text = strpaging;

        }
        catch (Exception ex) { throw ex; }
        finally { dtadmin.Dispose(); objproduct = null; }
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

    private int GetSortColumnIndexGVO()
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

    // This is a helper method used to add a sort direction image to the header of the column being sorted.
    public void AddSortImageGVO(int columnIndex, GridViewRow row)
    {
        // Create the sorting image based on the sort direction.
        System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
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

    #endregion

    #region --- Search ----

    //search for user selections
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        GVOrder.PageIndex = 0;
        BindProduct();
    }

    #endregion

    #region   ---- Add item Pop Up -----

    protected void btnAddProOrder_Click(object sender, EventArgs e)
    {
        int order_id = 0;
        bool ordFlag = false;
        int customer_id = 0;
        string GlobleDiscount = "";
        decimal netPayment = Convert.ToDecimal(0.0);

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
                        customerManager objcustomer = new customerManager();
                        DataTable dtcustomer = new DataTable();
                        objorder.customerid = Convert.ToInt32(Request.QueryString["customer"]);
                        dtcustomer = objcustomer.GetDiscountAndCreditfromCustomerid();

                        if (dtcustomer.Rows.Count > 0)
                        {
                            //customer_id = Convert.ToInt32(dtcustomer.Rows[0]["customerid"]);
                            GlobleDiscount = dtcustomer.Rows[0]["globleDiscountRate"].ToString();
                        }


                        if (ordFlag == false && Request.QueryString["orderid"] == null && Convert.ToInt32(Request.QueryString["orderid"]) == 0)
                        {

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

                            //if (Convert.ToInt32(dtord.Rows[0]["qty"]) > Convert.ToInt32(Convert.ToInt32(TxtQty)))
                            //{
                            //    objorder.qty = Convert.ToInt32(Convert.ToInt32(TxtQty) - Convert.ToInt32(dtord.Rows[0]["qty"]));
                            //    objorder.netprice = Convert.ToDecimal(Convert.ToDecimal(lblPrice) * Convert.ToInt32(Convert.ToInt32(TxtQty) - Convert.ToInt32(dtord.Rows[0]["qty"])));
                            //    objorder.UpdateOrderDetail();
                            //}
                            //else
                            //{
                            //    objorder.qty = Convert.ToInt32(Convert.ToInt32(TxtQty) + Convert.ToInt32(dtord.Rows[0]["qty"]));
                            //    objorder.netprice = Convert.ToDecimal(Convert.ToDecimal(lblPrice) * Convert.ToInt32(Convert.ToInt32(TxtQty) + Convert.ToInt32(dtord.Rows[0]["qty"])));
                            //    objorder.UpdateOrderDetail();
                            //}

                        }
                        else
                        {
                            objorder.InsertOrderDetail();
                        }

                        // insert into order details table
                        //objorder.InsertOrderDetail();
                        if (Convert.ToInt32(GlobleDiscount.Replace("%", "")) > 0)
                        {
                            netPayment = Convert.ToDecimal(Convert.ToDecimal(netPayment) + (Convert.ToDecimal(objorder.netprice) - (Convert.ToDecimal(objorder.netprice) * Convert.ToInt32(GlobleDiscount.Replace("%", "")) / 100)));
                        }
                        else
                        {
                            netPayment = Convert.ToDecimal(Convert.ToDecimal(netPayment) + Convert.ToDecimal(objorder.netprice));
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
            try
            {
                objordertotal.orderid = order_id;

                decimal TotalAmmount = objordertotal.GetSumOfItemPriceUsingOrderId();
                if (TotalAmmount != 0)
                {
                    netPayment = TotalAmmount;
                }

                objordertotal.totalammount = Convert.ToDecimal(netPayment);
                objordertotal.UpdateOrder();

                hidorderid.Value = Convert.ToString(order_id);
                Session["OrderId"] = Convert.ToString(order_id);

                //GetOrderDetails();
                Response.Redirect("vieworderdetails.aspx?orderid=" + order_id + "&customer=" + Request.QueryString["customer"]);
            }
            catch (Exception o)
            {
                throw o;
            }
            finally { objordertotal = null; }
        }

        myModal.Style.Add("display", "none");
        GetOrderDetails();
    }

    #endregion

    protected void lnkorderreceived_Click(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();
        try
        {
            if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            }
            objorder.UpdateStatus(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OrderRecieved"]));
            lnkorderreceived.ForeColor = System.Drawing.Color.Green;

            SendMail(Convert.ToInt32(Request.QueryString["orderid"]), Convert.ToInt32(Request.QueryString["customer"]), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsOrderReceivedId"]));
            Response.Redirect("vieworderdetails.aspx?orderid=" + Request.QueryString["orderid"] + "&customer=" + Request.QueryString["customer"]);
        }
        catch (Exception ex)
        {
            throw;
        }
        finally { objorder = null; }
    }

    protected void lnkpreparingorder_Click(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();
        try
        {
            if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            }
            objorder.UpdateStatus(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PreparingOrder"]));
            lnkorderreceived.Enabled = false;
            lnkpreparingorder.ForeColor = System.Drawing.Color.Green; lnkorderreceived.ForeColor = System.Drawing.Color.Black;

            // preparing order
            // SendMail(Convert.ToInt32(Request.QueryString["orderid"]), Convert.ToInt32(Request.QueryString["customer"]), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsOrderReceivedId"]));
            Response.Redirect("vieworderdetails.aspx?orderid=" + Request.QueryString["orderid"] + "&customer=" + Request.QueryString["customer"]);
        }
        catch (Exception ex)
        {
            throw;
        }
        finally { objorder = null; }
    }

    protected void lnkshipped_Click(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();
        try
        {
            if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            }
            objorder.UpdateStatus(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Shipped"]));
            lnkpreparingorder.Enabled = false; lnkorderreceived.Enabled = false;
            lnkshipped.ForeColor = System.Drawing.Color.Green; lnkorderreceived.ForeColor = System.Drawing.Color.Black; lnkpreparingorder.ForeColor = System.Drawing.Color.Black;

            // shipped order
            SendMail(Convert.ToInt32(Request.QueryString["orderid"]), Convert.ToInt32(Request.QueryString["customer"]), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsOrderShippedId"]));

            Response.Redirect("vieworderdetails.aspx?orderid=" + Request.QueryString["orderid"] + "&customer=" + Request.QueryString["customer"]);
        }
        catch (Exception ex)
        {
            throw;
        }
        finally { objorder = null; }
    }

    protected void lnkdelivered_Click(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();
        try
        {
            if (Request.QueryString["orderid"] != null)
            {
                objorder.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            }
            objorder.UpdateStatus(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Delivered"]));
            lnkshipped.Enabled = false; lnkorderreceived.Enabled = false;
            lnkdelivered.ForeColor = System.Drawing.Color.Green; lnkshipped.ForeColor = System.Drawing.Color.Black; lnkorderreceived.ForeColor = System.Drawing.Color.Black; lnkpreparingorder.ForeColor = System.Drawing.Color.Black;

            // delivered order
            //SendMail(Convert.ToInt32(Request.QueryString["orderid"]), Convert.ToInt32(Request.QueryString["customer"]), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsOrderReceivedId"]));

            Response.Redirect("vieworderdetails.aspx?orderid=" + Request.QueryString["orderid"] + "&customer=" + Request.QueryString["customer"]);

        }
        catch (Exception ex)
        {
            throw;
        }
        finally { objorder = null; }
    }

    private void SendMail(int orderid, int customer, int status)
    {
        orderManager objorder = new orderManager();
        DataTable getcustomer = new DataTable();
        getcustomer = objorder.GetCustomerDetail(customer);
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
                        strMsg1 += "<td>" + Convert.ToDecimal(Convert.ToDecimal(getOrderDetail.Rows[i]["Price"]) * Convert.ToInt32(getOrderDetail.Rows[i]["Qty"])) + "</td>";
                        strMsg1 += "</tr>";
                        Subtotal += Convert.ToDecimal(Convert.ToDecimal(getOrderDetail.Rows[i]["Price"]) * Convert.ToDecimal(getOrderDetail.Rows[i]["Qty"]));
                    }
                    TotalDiscount = Subtotal * Convert.ToDecimal(Discount[0]) / 100;

                    strMsg1 += "<table style=\"width:250px;font-family:Arial, Helvetica, sans-serif; font-size:12px; padding:10px; float:right; background:#f1f1f1;\">";
                    strMsg1 += "<tr style=\"border-bottom:1px solid #ccc; display:inline-block; width:100%;height: 30px;margin-bottom: 5px; \">";
                    strMsg1 += "<td style=\"width:130px;\">Subtotal</td>";
                    strMsg1 += "<td>" + getOrderDetail.Rows[0]["TotalNetPrice"] + "SR" + "</td>";
                    //strMsg1 += "<td>" + Subtotal + "SR" + "</td>";
                    strMsg1 += "</tr>";

                    strMsg1 += "<tr style=\"border-bottom:1px solid #ccc;display:inline-block;width:100%;height: 30px;margin-bottom: 5px; \">";
                    strMsg1 += "<td style=\"width: 130px;\">Total Discount</td>";
                    strMsg1 += "<td>" + TotalDiscount + "SR" + "</td>";
                    //strMsg1 += "<td>0.00 SR" + "</td>";
                    strMsg1 += "</tr>";

                    strMsg1 += "<tr style=\"display:inline-block;width:100%;height: 30px;margin-bottom: 5px;\">";
                    strMsg1 += "<td style=\"width: 130px; font-size:16px;\">Grand Total</td>";

                    //strMsg1 += "<td style=\"font-size:16px;\">" + Convert.ToDecimal(Subtotal - TotalDiscount) + "SR" + "</td>";
                    strMsg1 += "<td style=\"font-size:16px;\">" + Convert.ToDecimal(Subtotal) + "SR" + "</td>";
                    strMsg1 += "</tr>";
                    strMsg1 += "</table>";


                    strMsg = GetData.Rows[0]["EmailBody"].ToString();
                    strMsg = strMsg.Replace("##CONTENT##", strMsg1);
                    strMsg = strMsg.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
                    strMsg = strMsg.Replace("##COMPANYNAME##", System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"]);
                    string strMsgall = CommonFunctions.GetFileContents(Server.MapPath("../MailTemplate/bmb-Mail.html"));
                    strMsgall = strMsgall.Replace("##COMPANYNAME##", System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"]);
                    strMsgall = strMsgall.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
                    strMsgall = strMsgall.Replace("##CONTENT##", strMsg);

                    if (strto != null)
                    {
                        CommonFunctions.SendMail2(strFrom, strto, "", strMsgall, strSubject, "", "", "");
                        //CommonFunctions.SendMail2(strFrom, "hardik@webtechsystem.com", "", strMsgall, strSubject, "", "", "");
                    }

                }
            }
        }
    }



    protected void btnSendOrderMail_Click(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();
        try
        {
            DataTable getcustomer = new DataTable();
            getcustomer = objorder.GetCustomerDetail(Convert.ToInt32(Request.QueryString["customer"]));
            if (getcustomer.Rows.Count > 0)
            {
                string strto = string.Empty;
                string[] Discount = getcustomer.Rows[0]["Discount"].ToString().Split('%');

                strto = getcustomer.Rows[0]["Email"].ToString();


                DataTable GetData = new DataTable();
                GetData = objorder.GetSubject(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsInvoicePDFId"]));
                if (GetData.Rows.Count > 0)
                {
                    string strSubject = string.Empty;
                    string strFrom = string.Empty;

                    strSubject = GetData.Rows[0]["EmailSubject"].ToString();
                    strFrom = GetData.Rows[0]["FromEmail"].ToString();

                    DataTable getOrderDetail = new DataTable();
                    //getOrderDetail = objorder.GetOrderDetail(Convert.ToInt32(Request.QueryString["orderid"]));
                    getOrderDetail = objorder.GetOrderDetailForPdf(Convert.ToInt32(Request.QueryString["orderid"]));
                    if (getOrderDetail.Rows.Count > 0)
                    {
                        //string Str = getstring(Discount[0], GetData.Rows[0]["EmailBody"].ToString(), getOrderDetail);
                        string Str = getstring(Discount[0], getOrderDetail);
                        if (Str != "")
                        {
                            StringReader sr = new StringReader(Str);

                            Document pdfDoc = new Document(PageSize.A4, 0f, 0f, 0f, 0f);

                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                pdfDoc.Open();
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                                pdfDoc.Close();
                                byte[] bytes = memoryStream.ToArray();
                                memoryStream.Close();

                                if (strto != null)
                                {
                                    CommonFunctions.SendMailWithAttachment2(strFrom, strto, "", "", strSubject, "", "", "", bytes);
                                    //CommonFunctions.SendMailWithAttachment2(strFrom, "hardik@webtechsystem.com", "", "", strSubject, "", "", "", bytes);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally { objorder = null; }
    }

    protected void btnOrderpdf_Click(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();
        try
        {
            DataTable getcustomer = new DataTable();
            getcustomer = objorder.GetCustomerDetail(Convert.ToInt32(Request.QueryString["customer"]));
            if (getcustomer.Rows.Count > 0)
            {
                string strto = string.Empty;
                string[] Discount = getcustomer.Rows[0]["Discount"].ToString().Split('%');
                strto = getcustomer.Rows[0]["Email"].ToString();

                DataTable GetData = new DataTable();
                GetData = objorder.GetSubject(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsInvoicePDFId"]));
                if (GetData.Rows.Count > 0)
                {
                    string strSubject = string.Empty;
                    string strFrom = string.Empty;

                    strSubject = GetData.Rows[0]["EmailSubject"].ToString();
                    strFrom = GetData.Rows[0]["FromEmail"].ToString();

                    DataTable getOrderDetail = new DataTable();
                    //getOrderDetail = objorder.GetOrderDetail(Convert.ToInt32(Request.QueryString["orderid"]));
                    getOrderDetail = objorder.GetOrderDetailForPdf(Convert.ToInt32(Request.QueryString["orderid"]));
                    if (getOrderDetail.Rows.Count > 0)
                    {

                        //string Str = getstring(Discount[0], GetData.Rows[0]["EmailBody"].ToString(), getOrderDetail);
                        string Str = getstring(Discount[0], getOrderDetail);
                        StringBuilder customermailstrin = new StringBuilder();
                        if (Str != "")
                        {
                            customermailstrin.Append(Str);

                            StringReader sr = new StringReader(Str);
                            Document pdfDoc = new Document(PageSize.A4, 0f, 0f, 0f, 0f);

                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                pdfDoc.Open();
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                                pdfDoc.Close();
                                byte[] bytes = memoryStream.ToArray();

                                Response.Clear();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + Convert.ToInt32(Request.QueryString["orderid"]) + ".pdf");
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.BinaryWrite(bytes);
                                Response.End();

                                // excal format only
                                //Response.Clear();
                                //Response.Buffer = true;
                                //Response.AddHeader("content-disposition", "attachment;filename=ReceiptExport.xls");
                                //Response.Charset = "";
                                //Response.ContentType = "application/vnd.ms-excel";
                                //string style = @"<style> .textmode { } </style>";
                                //Response.Write(style);
                                //Response.Output.Write(Str.ToString());
                                //Response.Flush();
                                //Response.End();


                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally { objorder = null; }
    }

    private string getstring(string Discount, DataTable getOrderDetail)
    {
        orderManager objorder = new orderManager();

        string strMsg1 = string.Empty;
        string strMsg = string.Empty;
        string strMsgall = string.Empty;

        decimal TotalDiscount = 0;
        decimal Subtotal = 0;

        strMsg1 += "<table cellpadding='0' cellspacing='0'>";
        strMsg1 += "<tr>";
        strMsg1 += "<td><table width='100%' cellpadding='0' cellspacing='0'>";
        strMsg1 += "<tr>";
        strMsg1 += "<td style='padding:0;width:50px'><img style='width: 50px;' src='##SITEURL##images/top.jpg' /></td>";
        strMsg1 += "<td style='background:#F7F8F8; text-align:right;'><table style='font-family: Arial, Helvetica, sans-serif; font-size: 14px;float:right;padding-right:0;width:100%' cellpadding='0' cellspacing='0'>";
        strMsg1 += "<tbody>";
        strMsg1 += "<tr>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td style='font-weight: bold;width: 200px; text-align: left;color:#020000'>Order Number:</td>";
        strMsg1 += "<td style='text-align: right;  width: 200px;color:#727071'>" + getOrderDetail.Rows[0]["orderid"] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td style='font-weight: bold;width: 200px; text-align: left;color:#020000'>Date:</td>";
        DateTime dtime = new DateTime();
        dtime = Convert.ToDateTime(getOrderDetail.Rows[0]["orderdate"].ToString());
        strMsg1 += "<td style='text-align: right; width:200px;color:#727071'>" + dtime.ToShortDateString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "</tr>";
        strMsg1 += "</tbody>";
        strMsg1 += "</table></td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td colspan='2' style='padding:0;background:#F7F8F8; text-align:left;'><img style='width:154px;' src='##SITEURL##images/main_logo.png' /></td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td></td>";
        strMsg1 += "<td style='border-bottom:1px solid #cbcdce;background:#f7f8f8'>&nbsp;</td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table></td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td><table cellpadding='0' cellspacing='0'>";
        strMsg1 += "<tr>";
        strMsg1 += "<td><img style='width: 50px; height:800px;' src='##SITEURL##images/mid.jpg' /></td>";
        strMsg1 += "<td valign='top' style='background:#f7f8f8;padding-right:10px;padding-left:10px'><table style='width: 730px;' cellpadding='0' cellspacing='0' border='0'>";
        strMsg1 += "<tr>";
        strMsg1 += "<td><table style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; color: #000;' cellpadding='0' cellspacing='0' border='0' width='100%'>";
        strMsg1 += "<tr>";
        strMsg1 += "<td style='width: 250px; padding: 5px;'><table  width='100%'>";
        strMsg1 += "<tr>";
        strMsg1 += "<td style='width: 80px; font-weight: bold; font-size: 12px; text-align: left;'>Company Name:</td>";
        strMsg1 += "<td style='font-size: 12px; text-align: right;'>" + getOrderDetail.Rows[0]["companyname"] + "</td>";
        strMsg1 += "</tr>";

        string address = string.Empty;
        if (getOrderDetail.Rows[0]["streetaddress"].ToString() != "")
        { address = getOrderDetail.Rows[0]["streetaddress"].ToString(); }
        if (getOrderDetail.Rows[0]["city"].ToString() != "")
        { address += "," + getOrderDetail.Rows[0]["city"].ToString(); }
        if (getOrderDetail.Rows[0]["country"].ToString() != "") { address += "," + getOrderDetail.Rows[0]["country"].ToString(); }

        strMsg1 += "<tr>";
        strMsg1 += "<td style='width: 80px; font-weight: bold; font-size: 12px; text-align: left; vertical-align:top'>Address:</td>";
        strMsg1 += "<td style='font-size: 12px; text-align: right;'>" + address + "</td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table></td>";
        strMsg1 += "<td style='width: 250px; padding: 10px; font-size: 12px;'><table width='100%'>";
        strMsg1 += "<tr>";
        strMsg1 += "<td style='width: 120px; font-weight: bold; font-size: 12px; text-align: left;'>Contact Person:</td>";
        strMsg1 += "<td style='text-align: right; font-size: 12px;'>" + getOrderDetail.Rows[0]["contactname"] + "</td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td style='width: 120px; font-weight: bold; font-size: 12px; text-align: left;'>Mobile:</td>";
        strMsg1 += "<td style='text-align: right; font-size: 12px;'>" + getOrderDetail.Rows[0]["mobile"] + "</td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td style='width: 120px; font-weight: bold; font-size: 12px; text-align: left;'>Email:</td>";
        strMsg1 += "<td style='text-align: right; font-size: 12px;'>" + getOrderDetail.Rows[0]["email"] + "</td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table></td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table></td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr style=''>";
        strMsg1 += "<td ><div style='background:#fff;'><table cellpadding='0' cellspacing='0' border='0' style='width: 730px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; padding: 0;border:1px solid #ccc'>";
        strMsg1 += "<tr style='height: 30px;'>";
        strMsg1 += "<td style='border-top: 1px solid #ccc;border-left: 1px solid #ccc;border-bottom: 1px solid #ccc;border-right: 1px solid #ccc; width:100px; padding: 5px;'>SKU</td>";
        strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:180px;'>Product Name</td>";
        strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:50px;'>Qty</td>";
        strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:70px;'>Unit Price</td>";
        strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px; width:50px;'>Discount</td>";
        strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:65px;'>Final Price</td>";
        strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:65px;'>Total</td>";
        strMsg1 += "</tr>";
        decimal subtotal = 0;
        for (int i = 0; i < getOrderDetail.Rows.Count; i++)
        {
            int cnt = Convert.ToInt32(getOrderDetail.Rows.Count);
            if (i == 0)
            {
                strMsg1 += "<tr style='height: 30px;'>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; width:100px;  border-left: 1px solid #ccc;  padding: 5px;text-align:left;'>&nbsp;&nbsp;" + getOrderDetail.Rows[i]["sku"] + "</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; padding: 5px;width:180px;'>" + Server.HtmlDecode(getOrderDetail.Rows[i]["Productname"].ToString()) + "</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; width:50px;  padding: 5px;'>" + getOrderDetail.Rows[i]["qty"] + "PCS</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; padding: 5px;text-align:right;width:70px;'>" + getOrderDetail.Rows[i]["price"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["discount"] + "%&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["finalprice"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='font-weight: bold; border-right: 1px solid #ccc;  padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["netprice"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "</tr>";
            }
            if ((cnt - 1) == i)
            {
                strMsg1 += "<tr style='height: 30px;'>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; width:100px; border-left: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:left;'>&nbsp;&nbsp;" + getOrderDetail.Rows[i]["sku"] + "</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;width:180px;'>" + Server.HtmlDecode(getOrderDetail.Rows[i]["Productname"].ToString()) + "</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; border-bottom: 1px solid #ccc;width:50px; padding: 5px;'>" + getOrderDetail.Rows[i]["qty"] + "PCS</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:right;width:70px;'>" + getOrderDetail.Rows[i]["price"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["discount"] + "%&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["finalprice"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='font-weight: bold; border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["netprice"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "</tr>";
            }
            else
            {
                strMsg1 += "<tr style='height: 30px;'>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; width:100px; border-left: 1px solid #ccc;  padding: 5px;text-align:left;'>&nbsp;&nbsp;" + getOrderDetail.Rows[i]["sku"] + "</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc;  padding: 5px;width:180px;'>" + Server.HtmlDecode(getOrderDetail.Rows[i]["Productname"].ToString()) + "</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; width:50px; padding: 5px;'>" + getOrderDetail.Rows[i]["qty"] + "PCS</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc;  padding: 5px;text-align:right;width:70px;'>" + getOrderDetail.Rows[i]["price"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc;  padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["discount"] + "%&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='border-right: 1px solid #ccc; 1px solid #ccc; padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["finalprice"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "<td style='font-weight: bold; border-right: 1px solid #ccc;  padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["netprice"] + "&nbsp;&nbsp;</td>";
                strMsg1 += "</tr>";
            }
            subtotal = Convert.ToDecimal(Convert.ToDecimal(subtotal) + Convert.ToDecimal(Convert.ToDecimal(getOrderDetail.Rows[i]["price"]) * Convert.ToInt32(getOrderDetail.Rows[i]["qty"])));
        }
        decimal totaldiscount = 0;
        totaldiscount = Convert.ToDecimal(Convert.ToDecimal(subtotal) - Convert.ToDecimal(getOrderDetail.Rows[0]["totalammount"]));
        strMsg1 += "<tr>";
        strMsg1 += "<td colspan='4' style='padding: 5px; height: 50px; vertical-align: top; text-align: left;border-bottom: 1px solid #ccc;border-left: 1px solid #ccc;'>&nbsp;&nbsp;Customer Signature</td>";
        strMsg1 += "<td colspan='3' style='padding: 10px; border: 1px solid #ccc;padding-top:0'><table width='100%' cellpadding='0' cellspacing='0'>";
        strMsg1 += "<tr style='margin: 5px; height: 25px;'>";
        strMsg1 += "<td style='margin-right: 20px; width: 250px; text-align: left; font-size: 14px;'>Subtotal:</td>";
        strMsg1 += "<td style='text-align: right;width:300px; font-size: 14px;'>" + subtotal + "SAR</td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr style='margin: 5px; height: 25px;'>";
        strMsg1 += "<td style='margin-right: 20px; width: 250px; text-align: left; font-size: 14px;'>Total Discouns:</td>";
        strMsg1 += "<td style='text-align: right;width:300px; font-size: 14px;'>" + totaldiscount + "SAR</td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr style='margin: 2px; height: 30px;'>";
        strMsg1 += "<td style='margin-right: 20px; width: 250px; text-align: left; font-size: 14px; font-weight: bold;border-top: 1px solid #ccc;padding-top:10px'>Grand Total:</td>";
        strMsg1 += "<td style='text-align: right; width:300px; font-size: 14px; font-weight: bold;border-top: 1px solid #ccc;padding-top:10px'>" + getOrderDetail.Rows[0]["totalammount"] + "SAR</td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table>";
        strMsg1 += "</td>";
        strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td colspan='8'>&nbsp;</td>";
        //strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td colspan='4' style='background:#F7F8F8;border-top: 1px solid #ccc;' ></td>";
        strMsg1 += "<td colspan='3' style='padding: 10px; border-left: 1px solid rgb(204, 204, 204);border-right: 1px solid rgb(204, 204, 204);border-bottom: 1px solid rgb(204, 204, 204); height:50px;'><div>";
        strMsg1 += "<label style='margin-right: 20px; float: left; width: 120px; text-align: left; font-size: 14px;'>Payment Method:</label>";

        if (getOrderDetail.Rows[0]["payType"].ToString() == System.Configuration.ConfigurationManager.AppSettings["PayTypeCase"])
        {
            strMsg1 += "<span style='float: right; font-size: 14px; font-weight: bold;'>Cash</span> </div></td>";
        }
        if (getOrderDetail.Rows[0]["payType"].ToString() == System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"])
        {
            strMsg1 += "<span style='float: right; font-size: 14px; font-weight: bold;'>Credit</span> </div></td>";
        }
        strMsg1 += "</tr>";
        strMsg1 += "</table></div></td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table></td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table></td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td style='background:#f7f8f8; text-align:left;'><img style='width:200px;' src='##SITEURL##images/blk.jpg' /></td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td style='background:#F7F8F8'>";
        strMsg1 += "<table width='100%'>";

        //strMsg1 += "<tr>";
        //strMsg1 += "<td colspan='5'>";
        //strMsg1 += "<div style='width:100px;height:100px;border-radius:10px;background:white;'>";
        //strMsg1 += "</div>";
        //strMsg1 += "</td>";
        //strMsg1 += "</tr>";

        strMsg1 += "<tr>";
        strMsg1 += "<td style='width:50px'><img src='##SITEURL##images/botom.jpg' /></td>";
        strMsg1 += "<td style='width:661px'><table cellpadding='0' style='float:right;font-family: Arial, Helvetica, sans-serif; font-size: 11px;width:450px'>";
        strMsg1 += "<tr>";
        strMsg1 += "<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>";
        strMsg1 += "<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>";
        strMsg1 += "<th style='padding-right:30px;color:#f05222;text-align:left'>Showroom</th>";
        strMsg1 += "<th style='padding-right:30px;color:#f05222;text-align:left'>Contact Numbers</th>";
        strMsg1 += "<th style='padding-right:30px;color:#f05222;text-align:left'>Email</th>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'>Prince Sulaiman Street</td>";
        strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'><span style='color:#f05222'>T</span> +966 11 270 9251</td>";
        strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'><a style='color:#818284;text-decoration:none' href='mailto:support@bmbtools.com'>support@bmbtools.com</a></td>";
        strMsg1 += "</tr>";
        strMsg1 += "<tr>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'>Al Faisalya District, Riyadh</td>";
        strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'><span style='color:#f05222'>F</span> +966 11 270 6276</td>";
        strMsg1 += "<td></td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table></td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table></td>";
        strMsg1 += "</tr>";
        strMsg1 += "</table>";

        strMsgall = CommonFunctions.GetFileContents(Server.MapPath("../MailTemplate/Newbmb-invoice.html"));
        strMsgall = strMsgall.Replace("##CONTENT##", strMsg1);
        strMsgall = strMsgall.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
        strMsgall = strMsgall.Replace("##COMPANY##", System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"]);

        return strMsgall;
    }




    protected void btnMakepaid_Click(object sender, EventArgs e)
    {
        paymentManager objpayment = new paymentManager();
        try
        {
            objpayment.orderid = Convert.ToInt32(Request.QueryString["orderid"]);
            objpayment.customerid = Convert.ToInt32(Request.QueryString["customer"]);
            objpayment.paystatus = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaymentPaid"]);
            objpayment.paynotes = "";
            string[] totalAmmount = lblTotalPayAmmount.Text.ToString().Split(' ');
            objpayment.payammount = Convert.ToDecimal(totalAmmount[0]);

            int orderStatus = objpayment.GetPaymentAmountStatusFromOrderid();
            if (orderStatus > 0)
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "This order has been already paid.";
            }
            else
            {
                objpayment.InsertPayment();
                Response.Redirect("viewpayment.aspx");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objpayment = null; }
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

                if (hidPayType.Value != System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"].ToString())
                {
                    //objcust.customerId = Convert.ToInt32(Request.QueryString["customer"]);
                    //decimal CreditLimits = objcust.GetCustomerCreditFromID();
                    if (Convert.ToDecimal(CreditLimits) > Convert.ToDecimal(lblTotalPayAmmount.Text.Replace("SAR", "").Trim()))
                    {
                        //string credits = creditLimits.ToString().Replace("SAR", "").Replace(".00", "");
                        objcust.creditLimit = Convert.ToString(Convert.ToInt32(CreditLimits) - Convert.ToInt32(TotalAmmount));
                        if (GlobleDiscount.Replace("%", "") == "0") { objcust.globleDiscountRate = "0"; }
                        else
                        {
                            objcust.globleDiscountRate = Convert.ToString(Convert.ToInt32(GlobleDiscount.Replace("%", "")) - Convert.ToInt32(5)) + "%";
                        }
                        objcust.UpdateCustomerCreditLimits();

                        //order paytype update
                        objorder.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCredit"]);
                        objorder.updatePayType();

                        Response.Redirect("vieworderdetails.aspx?orderid=" + Request.QueryString["orderid"] + "&customer=" + Request.QueryString["customer"]);
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsgs.Text = "Your credit limits is lower than total payment.";
                    }
                }
                // if cash on delivery
                if (hidPayType.Value != System.Configuration.ConfigurationManager.AppSettings["PayTypeCase"].ToString())
                {
                    objcust.creditLimit = Convert.ToString(Convert.ToInt32(CreditLimits) + Convert.ToInt32(TotalAmmount));
                    objcust.globleDiscountRate = Convert.ToString(Convert.ToInt32(GlobleDiscount.Replace("%", "")) + Convert.ToInt32(5)) + "%";
                    objcust.UpdateCustomerCreditLimits();

                    //order paytype update
                    objorder.payType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PayTypeCase"]);
                    objorder.updatePayType();

                    Response.Redirect("vieworderdetails.aspx?orderid=" + Request.QueryString["orderid"] + "&customer=" + Request.QueryString["customer"]);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally { objorder = null; objcust = null; dtcustomer.Dispose(); }


    }
}