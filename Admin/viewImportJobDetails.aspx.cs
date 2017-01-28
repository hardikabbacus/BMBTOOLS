using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

public partial class Admin_viewImportJobDetails : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(AppSettings.PAGESIZE);
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";
    int ImportId = 0;
    string errorMsg = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Import Job List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        gvImportjob.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Pagging"]);
        //txtsearch.Focus();
        if (!IsPostBack)
        {
            #region SET PAGE NUMBER

            if (Request.QueryString["p"] == null) { pageNo = 1; }
            else if (Request.QueryString["p"] == "") { pageNo = 1; }
            else if (Convert.ToInt32(Request.QueryString["p"]) <= 0) { pageNo = 1; }
            else { pageNo = Convert.ToInt32(Request.QueryString["p"]); }
            #endregion

            if (Request.QueryString["flag"] == "add")
            {
                //trmsg.Visible = true;
                lblmsg.Visible = true;
                lblmsgs.Text = "Import Job has been added successfully";
            }
            else if (Request.QueryString["flag"] == "edit")
            {
                // trmsg.Visible = true;
                lblmsg.Visible = true;
                lblmsgs.Text = "Import Job has been updated successfully";
            }
            else if (Request.QueryString["flag"] == "delete")
            {
                //trmsg.Visible = true;
                lblmsg.Visible = true;
                lblmsgs.Text = "Import Job has been deleted successfully";
            }

            BingpageSize();
            if (CommonFunctions.IsQueryString("pageSize", true))
            {
                ddlpageSize.SelectedValue = Request.QueryString["pageSize"];
                pageSize = Convert.ToInt32(Request.QueryString["pageSize"]);
            }
            else
            {
                pageSize = AppSettings.PAGESIZE;
                ddlpageSize.SelectedValue = AppSettings.PAGESIZE.ToString();
            }

            if (Request.QueryString["filename"] != "" && Request.QueryString["filename"] != null)
            {
                lblfilename.Text = Convert.ToString(Request.QueryString["filename"]);
            }

            BindContents();
            getErrorList();

        }
    }

    public void getErrorList()
    {
        productManager objpro = new productManager();
        DataTable dt = new DataTable();
        objpro.ImportFileId = Convert.ToInt32(Request.QueryString["id"]);
        dt = objpro.GetAllTempErrorByImportFileId();
        if (dt.Rows.Count > 0)
        {
            string strError = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strError += "<b>Line: </b> " + dt.Rows[i]["BoolErrorLine"] + " <b>Column: </b> " + Server.HtmlDecode(dt.Rows[i]["BoolError"].ToString()) + "</br>";
            }
            ltrunread.Text = "Unreadable Data Found : ";
            //lblerrorlist.Text = "Unreadable Data Found : <b>Line: </b> " + dt.Rows[0]["BoolErrorLine"] + " <b>Column: </b> " + Server.HtmlDecode(dt.Rows[0]["BoolError"].ToString());
            lblerrorlist.Text = strError;
        }

    }

    //Biniding content
    protected void BindContents(string search = "")
    {
        importjobManager objimportjob = new importjobManager();
        DataTable dtcontents = new DataTable();
        try
        {
            objimportjob.id = Convert.ToInt32(Request.QueryString["id"]);
            objimportjob.productname = "";

            //objlanguage.languagename = Server.HtmlEncode(txtsearch.Text.Trim());
            if (pageNo == 0) { pageNo = 1; }
            objimportjob.pageNo = pageNo;
            objimportjob.pageSize = pageSize;
            objimportjob.SortExpression = SortExpression;
            querystring = "&id=" + Request.QueryString["id"] + "&filename=" + Request.QueryString["filename"] + "&filetp=" + Request.QueryString["filetp"] + "&pageSize=" + ddlpageSize.SelectedValue;
            //if (Convert.ToInt32(Request.QueryString["iptp"]) == 1)
            //{
            //    dtcontents = objimportjob.SearchImportJobs();
            //}
            //else
            //{
            dtcontents = objimportjob.SearchImportJobs();
            //}
            totalrecs = objimportjob.TotalRecord;

            if (pageNo == 1)
            {
                objimportjob.pageNo = 1;
                pageNo = 1;
            }
            else if (pageNo == 0)
            {
                objimportjob.pageNo = 1;
                pageNo = 1;
            }
            else
            {
                objimportjob.pageNo = (pageNo - 1) * pageSize;
            }
            objimportjob.pageSize = pageSize;
            totalpages = totalrecs / pageSize;
            if ((totalrecs % pageSize) > 0 && (totalrecs > pageSize)) { totalpages += 1; }
            gvImportjob.DataSource = dtcontents;
            gvImportjob.DataBind();
            if (dtcontents.Rows.Count > 0)
            {
                lblinsertfilecount.Text = "Proccessed <b>" + dtcontents.Rows[0]["Proccessed"] + "</b> rows of which <b>" + dtcontents.Rows[0]["NoErrorLine"] + "</b> passed validation and <b>" + dtcontents.Rows[0]["ErrorLine"] + "</b> failed.";
                int startRowOnPage = (gvImportjob.PageIndex * gvImportjob.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + gvImportjob.Rows.Count - 1;
                int totalRows = totalrecs;
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPaging(totalpages, pageNo, querystring, "ViewImportJobDetails.aspx");
            ltrpaggingbottom.Text = strpaging;
            //LoadDropDownList();
        }
        catch (Exception ex) { throw ex; }
    }

    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        BindContents("search");
    }
    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            ddlpageSize.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    //handle row data bound
    protected void gvImportjob_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //int productid = Convert.ToInt32(gvImportjob.DataKeys[e.Row.RowIndex].Value);


            Label LblAction = new Label();
            LblAction = (Label)e.Row.FindControl("lblAction");

            HiddenField hidsku = new HiddenField();
            hidsku = (HiddenField)e.Row.FindControl("hidsku");

            Label lblstatus = new Label();
            lblstatus = (Label)e.Row.FindControl("lblstatus");

            HiddenField hiderrno = new HiddenField();
            hiderrno = (HiddenField)e.Row.FindControl("hiderrno");

            //hidsku

            productManager objproduct = new productManager();
            objproduct.sku = hidsku.Value;
            int cont = objproduct.GetProdutctidCount();
            if (cont > 0)
            {
                LblAction.Text = "Update";
            }
            else
            {
                LblAction.Text = "Insert";
            }

            Label lblErrorType = new Label();
            lblErrorType = (Label)e.Row.FindControl("lblErrorType");
            CheckBox chkDelete = (e.Row.FindControl("chkDelete") as CheckBox);
            //if (lblErrorType.Text == "1")
            if (lblErrorType.Text != "0")
            {
                lblstatus.Text = hiderrno.Value;
                chkDelete.Enabled = false;
                chkDelete.Visible = false;
            }
            else
            {
                lblstatus.Text = "Not Started";
            }



            HiddenField hiddescp = new HiddenField();
            hiddescp = (HiddenField)e.Row.FindControl("hiddescp");

            Label lblDescription = new Label();
            lblDescription = (Label)e.Row.FindControl("lblDescription");

            if (hiderrno.Value != "0")
            {
                lblDescription.Text = hiddescp.Value;
                lblDescription.Style.Add("color", "red");
            }
            else
            {
                lblDescription.Text = "Passed Validation";
                lblDescription.Style.Add("color", "green");
            }

            HiddenField hidstatus = new HiddenField();
            hidstatus = (HiddenField)e.Row.FindControl("hidstatus");
            if (hidstatus.Value == "1")
            {
                lblstatus.Text = "Completed";
            }


            //if (hiderrno.Value == "0")
            //{
            //    //lblDescription.Text = "Check Validation";
            //    lblDescription.Text = hiddescp.Value;
            //}
            //else if (hiderrno.Value == "1")
            //{
            //    lblDescription.Text = hiddescp.Value;
            //    lblDescription.Style.Add("color", "red");
            //}
            //else
            //{
            //    lblDescription.Text = "Pass Validation";
            //    lblDescription.Style.Add("color", "green");
            //}

        }

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

    private string SortExpression
    {
        get
        {
            if (ViewState["SortExpression"] == null) { ViewState["SortExpression"] = String.Empty; }
            return ViewState["SortExpression"].ToString();
        }
        set { ViewState["SortExpression"] = value; }
    }

    protected void gvImportjob_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (ddlpageSize.SelectedValue != "")
        {
            pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        }

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
            BindContents();
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
    private int GetSortColumnIndex()
    {
        // Iterate through the Columns collection to determine the index of the column being sorted.
        foreach (DataControlField field in gvImportjob.Columns)
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
                    return gvImportjob.Columns.IndexOf(field);
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

        if (Convert.ToString(gvImportjob.SortDirection) == sortdirec)
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
    protected void imgbtnApproveJob_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvImportjob.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string lblsku = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    string Id = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;

                    productManager objprodct = new productManager();
                    DataTable dtpro = new DataTable();
                    try
                    {
                        objprodct.sku = Convert.ToString(lblsku);
                        // get the data from temp table
                        objprodct.id = Convert.ToInt32(Id);
                        dtpro = objprodct.GetSingleImportProductValue();

                        string productId = null;
                        string productName = null;
                        string sku = null;
                        string isVarientProduct = null;
                        string categoryname = null;
                        string brandname = null;
                        string price = null;
                        string minimumQuantity = null;
                        string inventory = null;


                        //errorMsg = string.Empty;
                        bool checkValid = false;

                        if (dtpro.Rows.Count > 0)
                        {
                            productName = Convert.ToString(dtpro.Rows[0]["productname"]);
                            sku = Convert.ToString(dtpro.Rows[0]["sku"]);
                            isVarientProduct = Convert.ToString(dtpro.Rows[0]["isvarientproduct"]);
                            categoryname = Convert.ToString(dtpro.Rows[0]["categoryname"]);
                            brandname = Convert.ToString(dtpro.Rows[0]["brandname"]);
                            price = Convert.ToString(dtpro.Rows[0]["price"]);
                            minimumQuantity = Convert.ToString(dtpro.Rows[0]["minimumquantity"]);
                            inventory = Convert.ToString(dtpro.Rows[0]["inventory"]);

                            //checkValidDataProduct
                            if (Convert.ToByte(dtpro.Rows[0]["ismasterproduct"]) == Convert.ToByte(true))
                            {
                                checkValid = checkValidDataMasterProduct(productName, sku, categoryname, brandname);
                            }
                            else
                            {
                                if (isVarientProduct.ToLower() == "false") // single product
                                {
                                    checkValid = checkValidDataProduct(productName, sku, categoryname, brandname, price, minimumQuantity, inventory);
                                }
                                else
                                {
                                    checkValid = checkValidDataSinglsProduct(productName, sku, price, minimumQuantity, inventory);
                                }
                            }

                            if (checkValid == false)
                            {
                                objprodct.FileError = errorMsg;
                                objprodct.FileErrorLineNumber = Convert.ToInt32(1);
                                objprodct.updateSingleImportProduct();
                            }
                            else
                            {
                                objprodct.productName = Server.HtmlEncode(Convert.ToString(dtpro.Rows[0]["productname"]));
                                objprodct.productDescription = Server.HtmlEncode(Convert.ToString(dtpro.Rows[0]["productdescription"]));
                                objprodct.sku = Convert.ToString(dtpro.Rows[0]["sku"]);
                                objprodct.barcode = Convert.ToString(dtpro.Rows[0]["barcode"]);
                                objprodct.isVarientProduct = Convert.ToByte(dtpro.Rows[0]["isvarientproduct"]);
                                objprodct.varientItem = Convert.ToString(dtpro.Rows[0]["varientItem"]);
                                if (Request.QueryString["filetp"] == "3")
                                {
                                    objprodct.isMasterProduct = Convert.ToByte(1);
                                }
                                else { objprodct.isMasterProduct = Convert.ToByte(dtpro.Rows[0]["ismasterproduct"]); }
                                objprodct.price = Convert.ToDecimal(dtpro.Rows[0]["price"]);
                                objprodct.cost = Convert.ToDecimal(dtpro.Rows[0]["cost"]);
                                objprodct.minimumQuantity = Convert.ToInt32(dtpro.Rows[0]["minimumquantity"]);
                                objprodct.inventory = Convert.ToInt32(dtpro.Rows[0]["inventory"]);
                                objprodct.isactive = Convert.ToByte(dtpro.Rows[0]["isactive"]);
                                objprodct.isFeatured = Convert.ToByte(dtpro.Rows[0]["isfeatured"]);


                                int count = objprodct.GetProdutctidCount();
                                if (count > 0)
                                {
                                    objprodct.productId = Convert.ToInt32(count);
                                    objprodct.UpdateItem();
                                    objprodct.isStatus = Convert.ToByte(1);
                                    objprodct.UpdateTempTableStatus();
                                }
                                else
                                {
                                    objprodct.InsertItem();
                                    objprodct.isStatus = Convert.ToByte(1);
                                    objprodct.UpdateTempTableStatus();
                                }

                                objprodct.productId = Convert.ToInt32(count);

                                #region product language

                                objprodct.DeleteProductLanguage();

                                objprodct.languageId = Convert.ToInt32(1);
                                objprodct.InsertProductLanguageItem();

                                objprodct.languageId = Convert.ToInt32(2);
                                objprodct.productName = Convert.ToString(dtpro.Rows[0]["ArabicName"]);
                                objprodct.productDescription = Convert.ToString(dtpro.Rows[0]["ArabicDesc"]);
                                objprodct.InsertProductLanguageItem();

                                #endregion

                                // add master product in product master table
                                int master_proid = 0;
                                if (Convert.ToString(dtpro.Rows[0]["master_product_parent"]) != "")
                                {
                                    objprodct.masterProductName = Convert.ToString(dtpro.Rows[0]["master_product_parent"]);
                                    master_proid = objprodct.getMasterProductid();
                                }

                                // add brand in brand product table
                                int brand_id = 0;
                                if (Convert.ToString(dtpro.Rows[0]["brandname"]) != "")
                                {
                                    objprodct.brandname = Convert.ToString(dtpro.Rows[0]["brandname"]);
                                    brand_id = objprodct.getBrandId();
                                }

                                // add category in product category table
                                int cat_id = 0;
                                int sub_catid = 0;
                                objprodct.CategoryName = Convert.ToString(dtpro.Rows[0]["categoryname"]);       // category name
                                objprodct.subcategoryname = Convert.ToString(dtpro.Rows[0]["subcategoryname"]);    // sub category name  

                                if (Convert.ToString(dtpro.Rows[0]["categoryname"]) != "")
                                {
                                    cat_id = Convert.ToInt32(objprodct.getCategoryId());
                                }
                                if (cat_id > 0)
                                {
                                    if (Convert.ToString(dtpro.Rows[0]["subcategoryname"]) != "")
                                    {
                                        objprodct.parentid = Convert.ToInt32(cat_id);
                                        sub_catid = Convert.ToInt32(objprodct.GetCategoryidByParentID());
                                    }
                                }
                                else
                                {
                                    if (Convert.ToString(dtpro.Rows[0]["subcategoryname"]) != "")
                                    {
                                        objprodct.CategoryName = Convert.ToString(dtpro.Rows[0]["subcategoryname"]);
                                        cat_id = Convert.ToInt32(objprodct.getCategoryId());
                                    }
                                }

                                if (objprodct.productId > 0)
                                {


                                    //product language
                                    objprodct.languageId = 1;
                                    //objprodct.UpdateProductLanguage();

                                    //product
                                    objprodct.DeleteProductCategory();

                                    //if (sub_catid > 0) { objprodct.categoryId = sub_catid; } else { objprodct.categoryId = cat_id; }
                                    //objprodct.InsertProductCategroyItem();

                                    if (sub_catid > 0) { objprodct.categoryId = sub_catid; objprodct.InsertProductCategroyItem(); }
                                    objprodct.categoryId = cat_id; objprodct.InsertProductCategroyItem();

                                    // brand
                                    if (brand_id > 0) { objprodct.DeleteProductBrand(); objprodct.barndId = brand_id; objprodct.InsertProductBrandItem(); }

                                    //master product
                                    if (master_proid > 0) { objprodct.DeleteMasterProductLink(); objprodct.masterProductId = master_proid; objprodct.InsertMasterProductLinkItem(); }
                                }
                                else
                                {
                                    objprodct.DeleteProductCategory();
                                    //objprodct.DeleteProductBrand();

                                    int product_id = objprodct.getmaxid();
                                    objprodct.productId = product_id;

                                    //product language
                                    objprodct.languageId = 1;  // productname,productdescription from upside to here
                                    objprodct.InsertProductLanguageItem();


                                    //product
                                    //if (sub_catid > 0) { objprodct.categoryId = sub_catid; } else { objprodct.categoryId = cat_id; }
                                    //objprodct.InsertProductCategroyItem();
                                    if (sub_catid > 0) { objprodct.categoryId = sub_catid; objprodct.InsertProductCategroyItem(); }
                                    objprodct.categoryId = cat_id; objprodct.InsertProductCategroyItem();

                                    // brand
                                    if (brand_id > 0) { objprodct.barndId = brand_id; objprodct.InsertProductBrandItem(); }

                                    // master product
                                    if (master_proid > 0) { objprodct.masterProductId = master_proid; objprodct.InsertMasterProductLinkItem(); }
                                }

                                // update validation message in tempproduct
                                objprodct.FileError = Server.HtmlEncode(errorMsg);
                                objprodct.FileErrorLineNumber = Convert.ToInt32(0);
                                objprodct.updateSingleImportProduct();

                            }
                        }

                        //gvAdmin.EditIndex = -1;
                        //BindContents();
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    finally { objprodct = null; dtpro = null; }
                }
            }
        }
        BindContents();
        lblmsg.Visible = true;
        lblmsgs.Text = "Approved job successfully";
    }
    protected void imgbtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("importJobs.aspx");
    }
    protected void imgbtnCancelJob_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow row in gvImportjob.Rows)
        //{
        //    if (row.RowType == DataControlRowType.DataRow)
        //    {
        //        bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
        //        if (isChecked)
        //        {
        //            string Id = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
        //            productManager objprodct = new productManager();
        //            try
        //            {
        //                objprodct.id = Convert.ToInt32(Id);
        //                objprodct.deleteFromTempTable();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //            finally { objprodct = null; }

        //        }
        //    }
        //}
        //BindContents();
        //lblmsg.Visible = true;
        //lblmsgs.Text = "Cancel job successfully";

        // changes here 24_11_2015
        importjobManager objimpjob = new importjobManager();
        try
        {
            if (Request.QueryString["id"] != null)
            {
                objimpjob.id = Convert.ToInt32(Request.QueryString["id"]);
                objimpjob.deleteImportJobTable();
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
        finally { objimpjob = null; }

        Response.Redirect("importjobs.aspx?filename=" + Request.QueryString["filename"]);

    }

    #region Check Validate All Data Product
    private bool checkValidDataProduct(string productName, string sku, string categoryname, string brandname, string price, string minimumQuantity, string inventory)
    {
        bool isInsertRow = false;
        errorMsg = "";

        #region sku
        if (CommonFunctions.IsValidValue(sku, false, false, true))
        {
            if (checkLength_DataType(sku, "string", 50, true).Length == 0)
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                errorMsg += "SKU missing. ";
                return isInsertRow;
            }
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region productName
        if (CommonFunctions.IsValidValue(productName, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            errorMsg += "Product name missing. ";
            return isInsertRow;
        }
        #endregion

        #region category
        if (CommonFunctions.IsValidValue(categoryname, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            errorMsg += "Category name missing. ";
            return isInsertRow;
        }
        #endregion

        #region Brand
        if (CommonFunctions.IsValidValue(brandname, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            errorMsg += "Brand name missing. ";
            return isInsertRow;
        }
        #endregion

        #region inventory
        if (inventory != "")
        {
            if (CommonFunctions.IsValidValue(inventory, true, false, true))
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                errorMsg += "Inventory missing. ";
                return isInsertRow;
            }
        }
        #endregion

        #region price
        if (price != "")
        {
            if (CommonFunctions.IsValidValue(price, true, true, true))
            {
                if (checkLength_DataType(price, "decimal", 0, true).Length == 0)
                {
                    isInsertRow = true;
                }
                else
                {
                    isInsertRow = false;
                    errorMsg += "Price missing. ";
                    return isInsertRow;
                }
            }
            else
            {
                isInsertRow = false;
                return isInsertRow;
            }
        }
        #endregion

        #region minimumQuantity
        if (minimumQuantity != "")
        {
            if (CommonFunctions.IsValidValue(minimumQuantity, true, false, true))
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                errorMsg += "Minimum Quentity missing. ";
                return isInsertRow;
            }
        }
        #endregion

        #region
        if (minimumQuantity != "" && inventory != "")
        {
            if (Convert.ToInt32(minimumQuantity) > Convert.ToInt32(inventory))
            {
                isInsertRow = false;
                errorMsg += "Iventory must be gerter then minimumQuentity. ";
                return isInsertRow;
            }
        }
        #endregion

        return isInsertRow;
    }
    #endregion

    #region Check Validate All Data Master Product
    private bool checkValidDataMasterProduct(string productName, string sku, string categoryname, string brandname)
    {
        bool isInsertRow = false;
        errorMsg = "";

        #region sku
        if (CommonFunctions.IsValidValue(sku, false, false, true))
        {
            if (checkLength_DataType(sku, "string", 50, true).Length == 0)
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                errorMsg = "SKU missing. ";
                return isInsertRow;
            }
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region productName
        if (CommonFunctions.IsValidValue(productName, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            errorMsg += "Product name missing. ";
            return isInsertRow;
        }
        #endregion

        #region category
        if (CommonFunctions.IsValidValue(categoryname, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            errorMsg += "Category name missing. ";
            return isInsertRow;
        }
        #endregion

        #region Brand
        if (CommonFunctions.IsValidValue(brandname, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            errorMsg += "Brand name missing. ";
            return isInsertRow;
        }
        #endregion

        return isInsertRow;
    }
    #endregion

    #region Check Validate All Data Product
    private bool checkValidDataSinglsProduct(string productName, string sku, string price, string minimumQuantity, string inventory)
    {
        bool isInsertRow = false;
        errorMsg = "";

        #region sku
        if (CommonFunctions.IsValidValue(sku, false, false, true))
        {
            if (checkLength_DataType(sku, "string", 50, true).Length == 0)
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                errorMsg += "SKU missing.";
                //return isInsertRow;
            }
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region productName
        if (CommonFunctions.IsValidValue(productName, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            errorMsg += "Product name missing.";
            //return isInsertRow;
        }
        #endregion

        #region inventory
        if (inventory != "")
        {
            if (CommonFunctions.IsValidValue(inventory, true, false, true))
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                errorMsg += "Inventory missing.";
                //return isInsertRow;
            }
        }
        #endregion

        #region price
        if (price != "")
        {
            if (CommonFunctions.IsValidValue(price, true, true, true))
            {
                if (checkLength_DataType(price, "decimal", 0, true).Length == 0)
                {
                    isInsertRow = true;
                }
                else
                {
                    isInsertRow = false;
                    errorMsg += "Price missing.";
                    //return isInsertRow;
                }
            }
            else
            {
                isInsertRow = false;
                return isInsertRow;
            }
        }
        #endregion

        #region minimumQuantity
        if (minimumQuantity != "")
        {
            if (CommonFunctions.IsValidValue(minimumQuantity, true, false, true))
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                errorMsg += "Minimum Quentity missing.";
                //return isInsertRow;
            }
        }
        #endregion

        #region
        if (minimumQuantity != "" && inventory != "")
        {
            if (Convert.ToInt32(minimumQuantity) > Convert.ToInt32(inventory))
            {
                isInsertRow = false;
                errorMsg += "Inventory must be gereter then minimumQuentity. ";
                return isInsertRow;
            }
        }
        #endregion

        return isInsertRow;
    }
    #endregion

    #region "Check Length and Data Type"

    /// <summary>
    /// This function checks for the DataType and Length of the fields value
    /// </summary>
    /// <param name="var">The parameter whose value has to be checked</param>
    /// <param name="dataType">DataType contains the datatype string to chcked against </param>
    /// <param name="maxLen">Parameter var maximum length should be less than or equal to maxLen</param>
    /// <param name="required">Indicates optional or compulsory field</param>
    /// <returns>string</returns>

    private string checkLength_DataType(string var, string dataType, int maxLen, bool required)
    {
        if (dataType == "string")
        {
            if (required)
            {
                if (var.Trim().Length <= 0)
                {
                    return "The value of this column can not be empty";
                }
            }

            if (var.Length > maxLen)
            {
                return "Maximium length of this field should be <=" + maxLen.ToString();
            }
        }
        else if (dataType == "int")
        {
            if (CommonFunctions.IsNumericValue(var))
            {

                if (required)
                {
                    if (var.Trim().Length <= 0)
                    {
                        return "The value of this column can not be empty";
                    }
                }

                try
                {
                    if (var.Trim().Length > 0)
                    {
                        int test = Convert.ToInt32(var);
                    }
                }
                catch
                {
                    return " DataType mismatch. Integer type is required";
                }
            }
        }
        else if (dataType == "decimal")
        {
            if (required)
            {
                if (var.Trim().Length <= 0)
                {
                    return "The value of this column can not be empty";
                }
            }
            try
            {
                if (var.Trim().Length > 0)
                {
                    decimal test = Convert.ToDecimal(var);
                }
            }
            catch
            {
                return " DataType mismatch. Decimal/Fractional type is required";
            }
        }
        return string.Empty;
    }

    #endregion

}