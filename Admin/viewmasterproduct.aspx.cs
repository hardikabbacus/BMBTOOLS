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


public partial class Admin_viewmasterproduct : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(AppSettings.PAGESIZE);
    string id = "";
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";
    int ImportId = 0;
    string error_code = "";
    string error_line = "";
    productManager objproduct = new productManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Master Product List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        gvAdmin.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Pagging"]);
        //txtsearch.Focus();
        if (!IsPostBack)
        {
            #region SET PAGE NUMBER

            if (Request.QueryString["p"] == null) { pageNo = 1; }
            else if (Request.QueryString["p"] == "") { pageNo = 1; }
            else if (Convert.ToInt32(Request.QueryString["p"]) <= 0) { pageNo = 1; }
            else { pageNo = Convert.ToInt32(Request.QueryString["p"]); }
            #endregion

            //BindCategoryPOPUPProduct();
            //BindBrandPOPUPProduct();
            BindCategoryProductSearch();

            if (Request.QueryString["flag"] == "add")
            {
                //lblmsg.Visible = true;
                lblmsgs.Text = "Master product has been added successfully";
            }
            else if (Request.QueryString["flag"] == "edit")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "Master product has been updated successfully";
            }
            else if (Request.QueryString["flag"] == "delete")
            {
                //lblmsg.Visible = true;
                lblmsgs.Text = "Master product has been deleted successfully";
            }

            //----------------------Maintain Search------------------------------------
            if (Request.QueryString["key"] != "")
            {
                txtsearch.Text = Request.QueryString["key"];
            }
            if (Request.QueryString["catid"] != "")
            {
                ddlsesrchCategory.SelectedValue = Request.QueryString["catid"];
            }

            BingpageSize();
            if (CommonFunctions.IsQueryString("pageSize", true))
            {
                //ddlpageSize.SelectedValue = Request.QueryString["pageSize"];
                pageSize = Convert.ToInt32(Request.QueryString["pageSize"]);
            }
            else
            {
                pageSize = AppSettings.PAGESIZE;
                //ddlpageSize.SelectedValue = AppSettings.PAGESIZE.ToString();
            }
            BindProduct();

            if (Session["AdminType"] != null && Convert.ToString(Session["AdminType"]) == "subadmin")
            {
                //imgbtnDelete.Visible = false;
                //addtop.Visible = false;
                //addbottom.Visible = false;
            }

        }
    }

    //bind category dropdown for popup
    public void BindCategoryPOPUPProduct()
    {
        categoryManager objCategory = new categoryManager();
        DataTable dt = new DataTable();
        DataTable dtsub = new DataTable();
        try
        {
            dt = objCategory.GetParentCategory(true);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //ListItem li = new ListItem("--Select Parent Menu--", i.ToString());
                    //ddlParentMenu.Items.Add(li);
                    ListItem li = new ListItem(dt.Rows[i]["categoryName"].ToString(), dt.Rows[i]["categoryId"].ToString());
                    li.Attributes.Add("style", "color:#a7688a;font-weight:bold;");
                    ddlPOPCategoryProduct.Items.Add(li);
                    objCategory.parentid = Convert.ToInt32(dt.Rows[i]["categoryId"].ToString());
                    dtsub = objCategory.GetSubCategory();
                    if (dtsub != null && dtsub.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtsub.Rows.Count; j++)
                        {
                            ListItem lisub = new ListItem(dtsub.Rows[j]["categoryName"].ToString(), dtsub.Rows[j]["categoryId"].ToString());
                            lisub.Attributes.Add("style", "color:#6dace5");
                            ddlPOPCategoryProduct.Items.Add(lisub);

                        }
                    }
                }
            }

        }
        catch (Exception ex) { throw ex; }
        finally { objCategory = null; }
    }

    //bind brand list for popup
    public void BindBrandPOPUPProduct()
    {
        brandManager objbrand = new brandManager();
        DataTable dt = new DataTable();
        try
        {
            dt = objbrand.GetBrandItem();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem lisub = new ListItem(dt.Rows[i]["brandName"].ToString(), dt.Rows[i]["brandId"].ToString());
                    lisub.Attributes.Add("style", "color:#6dace5");
                    ddlPOPBrandProduct.Items.Add(lisub);
                }
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objbrand = null; }
    }

    //bind category dropdown for Search
    public void BindCategoryProductSearch()
    {
        categoryManager objCategory = new categoryManager();
        DataTable dt = new DataTable();
        DataTable dtsub = new DataTable();
        try
        {
            dt = objCategory.GetAllCategorySearchDDL();
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlsesrchCategory.DataSource = dt;
                ddlsesrchCategory.DataTextField = "categoryName";
                ddlsesrchCategory.DataValueField = "categoryId";
                ddlsesrchCategory.DataBind();
                ddlsesrchCategory.Items.Insert(0, new ListItem("Type in Category", "0"));
            }
            else
            {
                ddlsesrchCategory.Items.Insert(0, new ListItem("- no record -", "0"));
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objCategory = null; }
    }


    //Bind master product
    private void BindProduct(string search = "")
    {
        this.Form.DefaultButton = imgbtnSearch.UniqueID;

        productManager objproduct = new productManager();
        DataTable dtadmin = new DataTable();
        try
        {
            if (txtsearch.Text != "")
            {
                objproduct.productName = txtsearch.Text.Trim();
                if (ddlsesrchCategory.SelectedValue != "0")
                {
                    ddlsesrchCategory.SelectedValue = "0";
                }

                //search by brand
                int bradid = Convert.ToInt32(objproduct.GetBrandIdFromBrandName());
                if (bradid != 0)
                {
                    objproduct.barndId = bradid;
                    objproduct.productName = "";
                }
            }
            else
            {
                objproduct.productName = txtsearch.Text.Trim();
            }

            if (ddlsesrchCategory.SelectedValue != "0")
            {
                objproduct.categoryId = Convert.ToInt32(ddlsesrchCategory.SelectedValue);
            }
            else
            {
                objproduct.categoryId = 0;
            }

            if (pageNo == 0) { pageNo = 1; }
            objproduct.pageNo = pageNo;
            objproduct.pageSize = pageSize;
            objproduct.SortExpression = SortExpression;
            querystring = "&key=" + txtsearch.Text + "&catid=" + ddlsesrchCategory.SelectedValue;
            //querystring = "&pageSize=" + ddlpageSize.SelectedValue + "&key=" + txtsearch.Text;
            dtadmin = objproduct.SearchMasterProductItem();
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
            gvAdmin.DataSource = dtadmin;
            gvAdmin.DataBind();
            if (dtadmin.Rows.Count > 0)
            {
                int startRowOnPage = (gvAdmin.PageIndex * gvAdmin.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + gvAdmin.Rows.Count - 1;
                int totalRows = totalrecs;
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "viewmasterproduct.aspx");
            ltrpaggingbottom.Text = strpaging;
            //Ltrup.Text = strpaging;
            LoadDropDownList();
        }
        catch (Exception ex) { throw ex; }
        finally { dtadmin.Dispose(); objproduct = null; }

    }

    //search for user selections
    protected void imgbtnSearch_Click(object sender, EventArgs e)
    {
        //trmsg.Visible = false;
        gvAdmin.PageIndex = 0;
        pageNo = Convert.ToInt32(Request.QueryString["p"]);
        //pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        BindProduct();
    }

    //bind page drop down
    protected void LoadDropDownList()
    {
        //String addstr = string.Empty;
        //ddlpage.ClearSelection();
        //ddlpage.Items.Clear();

        //for (int i = 1; i <= totalpages; i++)
        //{
        //    addstr = Convert.ToSingle(i) + " of " + totalpages;
        //    ddlpage.Items.Add(addstr);
        //    ddlpage.Items[ddlpage.Items.Count - 1].Value = Convert.ToString(i);
        //}
        //if (pageNo > totalpages)
        //{
        //    addstr = Convert.ToSingle(1) + " of " + 1;
        //    ddlpage.Items.Add(addstr);
        //    ddlpage.Items[ddlpage.Items.Count - 1].Value = Convert.ToString(1);
        //    pageNo = 1;
        //}
        //ddlpage.SelectedValue = Convert.ToString(pageNo);
    }

    //handle user page selection
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        //pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        BindProduct("search");
    }
    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        BindProduct("search");
    }
    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            //ddlpageSize.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    //handle grid view page chenging
    protected void gvAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAdmin.PageIndex = e.NewPageIndex;
        BindProduct();
        //ddlpage.SelectedIndex = e.NewPageIndex;
    }

    //handle delete event
    protected void imgbtnDelete_Click(object sender, EventArgs e)
    {
        productManager objproduct = new productManager();
        try
        {
            int con = 0;
            CheckBox chk = new CheckBox();
            string ActImage = string.Empty;
            string ThumbImage = string.Empty;
            DataSet dsadmin = new DataSet();

            for (int i = 0; i < gvAdmin.Rows.Count; i++)
            {
                chk = (CheckBox)(gvAdmin.Rows[i].FindControl("chkDelete"));
                if (chk.Checked == true)
                {
                    con += 1;
                    objproduct.productId = Convert.ToInt32(gvAdmin.DataKeys[gvAdmin.Rows[i].RowIndex].Value.ToString());

                    //DataTable dtimgs = new DataTable();
                    //dtimgs = objproduct.getAllProductImageByProductid();
                    //if (dtimgs.Rows.Count > 0)
                    //{
                    //    for (int im = 0; im < dtimgs.Rows.Count; im++)
                    //    {
                    //        string fullimagepath = string.Empty;
                    //        string thumbimagepath = string.Empty;
                    //        string mediumimagepath = string.Empty;

                    //        fullimagepath = Server.MapPath("../" + AppSettings.PRODUCT_ACTULE_ROOTURL + dtimgs.Rows[im]["imagename"].ToString());
                    //        thumbimagepath = Server.MapPath("../" + AppSettings.PRODUCT_THUMB_ROOTURL + dtimgs.Rows[im]["imagename"].ToString());
                    //        mediumimagepath = Server.MapPath("../" + AppSettings.PRODUCT_MEDIUM_ROOTURL + dtimgs.Rows[im]["imagename"].ToString());

                    //        CommonFunctions.DeleteFile(fullimagepath);
                    //        CommonFunctions.DeleteFile(thumbimagepath);
                    //        CommonFunctions.DeleteFile(mediumimagepath);
                    //    }
                    //}

                    //objproduct.DeleteProductLanguage();
                    //objproduct.DeleteProductCategory();
                    //objproduct.DeleteProductBrand();
                    //objproduct.DeleteProductImage();
                    //objproduct.DeleteMasterProductLink();
                    //objproduct.DeleteProductTag();
                    objproduct.DeleteItem();

                }
            }
            //Response.Redirect("viewmasterproduct.aspx?flag=delete&key=" + txtsearch.Text + "  &pageSize=" + ddlpageSize.SelectedValue + "");
            Response.Redirect("viewmasterproduct.aspx?flag=delete");
        }
        catch (Exception ex) { throw ex; }
        finally { objproduct = null; }
    }

    //handle row data bound
    protected void gvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
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

        if (Session["AdminType"] != null && Convert.ToString(Session["AdminType"]) == "subadmin")
        {
            e.Row.Cells[0].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (Convert.ToInt32(gvAdmin.DataKeys[e.Row.RowIndex].Value) == Convert.ToInt32(Session["AdminId"]))
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chkDelete");
                chk.Enabled = false;
            }
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

        //if (ddlpageSize.SelectedValue != "")
        //{
        //    pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        //}
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
            BindProduct();
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

    protected void lnkStatus_click(object sender, EventArgs e)
    {
        productManager objpro = new productManager();
        GridViewRow row = ((LinkButton)sender).Parent.Parent as GridViewRow;
        objpro.isactive = Convert.ToByte(Convert.ToInt32(((LinkButton)sender).CommandArgument) == 0 ? 1 : 0);

        objpro.productId = Convert.ToInt32(gvAdmin.DataKeys[gvAdmin.Rows[row.RowIndex].RowIndex].Value.ToString());
        objpro.UpdateStatus();
        BindProduct();
        //trmsg.Visible = true;
        //lblmsgs.Text = "Admin Menu details are updated successfully";
    }

    protected void ddlsesrchCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtsearch.Text = "";
        BindProduct();
    }



    #region --- Product csv ---

    protected void btnupload_Click(object sender, EventArgs e)
    {
        GernerateProductCSV();
    }

    private void GernerateProductCSV()
    {

        DataTable dsProduct = new DataTable();
        try
        {
            productManager objproduct = new productManager();
            dsProduct = objproduct.SelectMasterProductDetailForGeneratexls();

            string strtodaydate = Convert.ToDateTime(System.DateTime.Now.Date).ToString("dd/MM/yyyy");
            string strtotime = Convert.ToDateTime(System.DateTime.Now).ToString("hh:mm");
            string strFileName = "MasterProduct-" + strtodaydate.ToString().Replace("/", "-") + "__" + strtotime.Replace(":", "-") + ".xls";

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", strFileName));
            Response.ContentType = "application/ms-excel";

            if (dsProduct.Rows.Count > 0)
            {
                string str = string.Empty;
                foreach (DataColumn dtcol in dsProduct.Columns)
                {
                    Response.Write(str + dtcol.ColumnName);
                    str = "\t";
                }
                Response.Write("\n");
                foreach (DataRow dr in dsProduct.Rows)
                {
                    str = "";
                    string strChange = "";
                    for (int j = 0; j < dsProduct.Columns.Count; j++)
                    {
                        strChange = Server.HtmlDecode(Convert.ToString(dr[j]).Replace("\n", "").Replace("\r", ""));
                        Response.Write(str + strChange);
                        str = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            else
            {
                Response.Write("sku\tbarcode\tproductName\tisMasterProduct\tisVarientProduct\tvarientitem\tmaster_product_parent\tcategoryname\tsubcategoryname\tbrandname\tminimumQuantity\tcost\tprice\tproductDescription\tisActive\tisFeatured\tinventory\tArabicName\tArabicDesc");
                Response.End();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { dsProduct.Dispose(); }
    }

    protected void btnimport_Click(object sender, EventArgs e)
    {
        productManager objproduct = new productManager();
        if (Page.IsValid)
        {

            string extension = Path.GetExtension(file.FileName).ToLower();
            if (extension == ".xls" || extension == ".xlsx")
            {
                string filetype = "xls";
                bool isValidFile = false;

                string fileUploadLocation = string.Empty;
                DirectoryInfo dirInfo = null;
                string uploadedFileName = string.Empty;
                string uploadedFileFullPath = string.Empty;

                string PhysicalPath = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ErrorMasterProductUploadRootPath"].ToString());
                fileUploadLocation = (PhysicalPath);
                dirInfo = new DirectoryInfo(fileUploadLocation);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                uploadedFileName = file.FileName;
                uploadedFileFullPath = (fileUploadLocation + "\\") + uploadedFileName;
                file.PostedFile.SaveAs(uploadedFileFullPath);

                //count line of the file
                var lines = File.ReadAllLines(uploadedFileFullPath);
                //var totalcols = lines[0].Split(',').Length;
                var totalcols = lines[0].Split('\t').Length;
                if (totalcols == 19)
                {
                    importjobManager objImport = new importjobManager();
                    try
                    {
                        // insert file name for job history

                        objImport.importfilename = uploadedFileName;
                        objImport.filestatus = "ForReview";
                        objImport.importType = Convert.ToString(3);
                        objImport.InsertItem();
                        ImportId = objImport.getmaxid();

                        //Dim headers As String() = csv.GetFieldHeaders()
                        isValidFile = newvalidateCSVandTXTFiles_Automation(uploadedFileFullPath, filetype);
                        if (File.Exists(uploadedFileFullPath))
                        {
                            File.Delete(uploadedFileFullPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { objImport = null; }
                    Response.Redirect("viewImportJobDetails.aspx?id=" + ImportId + "&filename=" + uploadedFileName + "&filetp=3");
                }
                else
                {
                    lblError.Text = "Your enter fields is more then require field.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "invokeButtonClick();", true);
                }

            }
            else
            {
                lblError.Text = "Please upload a valid csv file.";
                ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "invokeButtonClick();", true);

            }

            if (hidflag.Value == "1")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "invokeButtonClick();", true);
            }

        }

    }

    public bool newvalidateCSVandTXTFiles_Automation(string filepath, string filetype)
    {
        //int totalproductinfeedcount = 0;
        int totalproductinfeedcount = 1;
        int totalerrorcount = 0;
        int totalsucesscount = 0;
        bool _isErrorsInRecord = false;

        string BoolErrorLine = "";
        string BoolError = "";

        FileStream fs = null;
        StreamWriter sr = null;
        StreamReader _fileStream = default(StreamReader);
        string _fileToRead = filepath;
        string _fileForErrorRecords = "";

        #region Create Datatable To Update Records
        DataTable dtalldatatoInsert = new DataTable();
        DataColumn dc = new DataColumn("sku");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("barcode");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("productName");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("isMasterProduct");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("isVarientProduct");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("varientitem");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("master_product_parent");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("categoryname");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("subcategoryname");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("brandname");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("minimumQuantity");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("cost");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("price");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("productDescription");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("isActive");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("isFeatured");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("inventory");
        dtalldatatoInsert.Columns.Add(dc);



        // static data add to temp table 
        dc = new DataColumn("isStatus");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("ImportFileId");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("FileError");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("FileErrorLineNumber");
        dtalldatatoInsert.Columns.Add(dc);

        dc = new DataColumn("ArabicName");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("ArabicDesc");
        dtalldatatoInsert.Columns.Add(dc);

        #endregion



        _fileForErrorRecords = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ErrorUploadRootPath"].ToString());

        DirectoryInfo dir = new DirectoryInfo(_fileForErrorRecords);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(_fileForErrorRecords);
        }

        _fileStream = File.OpenText(_fileToRead);
        string _readContent = string.Empty;

        //Adjust the top heading row, not include in DataTable
        if (!_fileStream.EndOfStream)
        {
            _readContent = _fileStream.ReadLine();
            string[] _arraytop = null;
            if (filetype.ToLower().Equals("csv"))
            {
                _arraytop = getArrayFromCommaSepratedValues(_readContent);
            }
            else if (filetype.ToLower().Equals("txt"))
            {
                //For TXT
                _arraytop = _readContent.Split(new char[] { ControlChars.Tab });
            }
            else if (filetype.ToLower().Equals("xls") || filetype.ToLower().Equals("xlsx"))
            {
                _arraytop = getArrayFromTabValues(_readContent);
            }

            //if (_arraytop == null || _arraytop.Length < 20 || _arraytop.Length > 21)
            if (_arraytop == null || _arraytop.Length < 19 || _arraytop.Length > 20)
            {
                totalerrorcount += 1;
                _isErrorsInRecord = true;

                // _fileStream.Close();
                // _fileStream.Dispose();

            }
        }


        string Sqlstring = null;

        string productId = null;
        string productName = null;
        string productDescription = null;
        string sku = null;
        string barcode = null;
        string isVarientProduct = null;
        string varientitem = null;
        string master_product_parent = null;
        string categoryname = null;
        string subcategoryname = null;
        string brandname = null;
        string isMasterProduct = null;
        string price = null;
        string cost = null;
        string minimumQuantity = null;
        string inventory = null;
        string isActive = null;
        string isFeatured = null;

        // static data import into temp table 
        string isStatus = "False";
        string ImportFileId = "";
        string FileError = "";
        string FileErrorLineNumber = "";

        string ArabicName = null;
        string ArabicDesc = null;

        //totaltime = 0
        //Dim CurrencySymbol As String

        //CurrencySymbol = currencynew
        // counter for generating xml
        StringBuilder xmlforUpdate = new StringBuilder();

        bool readflag = false;
        string totalRowNumberError = "";

        bool GlobalVarForError = false;


        while (!_fileStream.EndOfStream)
        {
            _isErrorsInRecord = false;


            string errorInRowNumber = "";
            totalproductinfeedcount += 1;
            _readContent = _fileStream.ReadLine();
            //_readContent = _readContent.Trim();
            _readContent = _readContent.Trim().Replace("\"\"", "''");

            //Check for line blank or not            
            if (_readContent.Length > 0)
            {
                readflag = true;
                string[] _array = null;

                if (filetype.ToLower().Equals("csv"))
                {
                    _array = getArrayFromCommaSepratedValues(_readContent);
                }
                else if (filetype.ToLower().Equals("txt"))
                {
                    //For TXT
                    _array = _readContent.Split(new char[] { ControlChars.Tab });
                }
                else if (filetype.ToLower().Equals("xls") || filetype.ToLower().Equals("xls"))
                {
                    _array = getArrayFromTabValues(_readContent);
                }

                error_code = "";
                error_line = "";

                if (_array == null)
                {
                    // ***<<Check>>**
                    totalerrorcount += 1;
                    _isErrorsInRecord = true;
                    GlobalVarForError = true;
                    errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                    //sr.WriteLine(_readContent)
                }
                else
                {
                    //if (_array.Length == 20)
                    if (_array.Length == 19)
                    {
                        ////Number of cols in csv/txt file
                        //if (_array[0] != null)
                        //{
                        //    if (_array[1] != null)
                        //    {
                        //        //if (_array[2] != null)
                        //        //{
                        //        if (_array[3] != null)
                        //        {

                        if (_array[0] != null && _array[0] != "") { sku = _array[0].ToString().Trim(); }
                        else
                        {
                            sku = "";
                            //totalerrorcount += 1;
                            ////_isErrorsInRecord = true;
                            //error_code += "SKU missing. ";
                            //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            //error_line = totalproductinfeedcount.ToString();
                        }
                        if (_array[1] != null && _array[1] != "") { barcode = _array[1].ToString().Trim(); } else { barcode = ""; }
                        if (_array[2] != null && _array[2] != "") { productName = _array[2].ToString().Trim(); }
                        else
                        {
                            productName = "";
                            //totalerrorcount += 1;
                            ////_isErrorsInRecord = true;
                            //error_code += "product name missing. ";
                            //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            //error_line = totalproductinfeedcount.ToString();
                        }
                        if (_array[3] != null && _array[3] != "")
                        {
                            isMasterProduct = _array[3].ToString().Trim();
                            if (isMasterProduct.ToLower() == "true") { }
                            else if (isMasterProduct.ToLower() == "false") { }
                            else { BoolError += "isMasterProduct, "; _isErrorsInRecord = true; BoolErrorLine += totalproductinfeedcount.ToString() + ","; }
                        }
                        else { isMasterProduct = "Fasle"; }
                        if (_array[4] != null && _array[4] != "")
                        {
                            isVarientProduct = _array[4].ToString().Trim();
                            if (isVarientProduct.ToLower() == "true") { }
                            else if (isVarientProduct.ToLower() == "false") { }
                            else { BoolError += "isVarientProduct, "; _isErrorsInRecord = true; BoolErrorLine += totalproductinfeedcount.ToString() + ","; }
                        }
                        else { isVarientProduct = "False"; }
                        if (_array[5] != null && _array[5] != "") { varientitem = _array[5].ToString().Trim(); } else { varientitem = ""; }
                        if (_array[6] != null && _array[6] != "") { master_product_parent = _array[6].ToString().Trim(); } else { master_product_parent = ""; }
                        if (_array[7] != null && _array[7] != "") { categoryname = _array[7].ToString().Trim(); }
                        else
                        {
                            if (_array[8] != null && _array[8] != "")
                            {
                            }
                            else
                            {
                                categoryname = "";
                                //totalerrorcount += 1;
                                ////_isErrorsInRecord = true;
                                //error_code += "Category name missing. ";
                                //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                                //error_line = totalproductinfeedcount.ToString();
                            }
                            //error_line = totalproductinfeedcount.ToString();
                        }

                        if (_array[8] != null && _array[8] != "") { subcategoryname = _array[8].ToString().Trim(); } else { subcategoryname = ""; }
                        if (_array[9] != null && _array[9] != "") { brandname = _array[9].ToString().Trim(); }
                        else
                        {
                            brandname = "";
                            //totalerrorcount += 1;
                            ////_isErrorsInRecord = true;
                            //error_code += "Brand name missing. ";
                            //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            //error_line = totalproductinfeedcount.ToString();
                        }

                        if (_array[10] != null && _array[10] != "") { minimumQuantity = _array[10].ToString().Trim().Replace(",", ""); } else { minimumQuantity = "0"; }
                        if (_array[11] != null && _array[11] != "")
                        {
                            cost = _array[12].ToString().Trim();
                            var p2 = cost.Split('.');
                            if (p2.Count() > 1)
                            {
                                if (p2[1].Length > 2)
                                {
                                    cost = p2[0] + "." + p2[1].Substring(0, 2);
                                }
                                else
                                {
                                    cost = p2[0] + "." + p2[1];
                                }
                            }
                        }
                        else { cost = "0.00"; }
                        if (_array[12] != null && _array[12] != "")
                        {
                            price = _array[12].ToString().Trim();
                            var p1 = price.Split('.');
                            if (p1[1].Length > 2)
                            {
                                price = p1[0] + "." + p1[1].Substring(0, 2);
                            }
                            else
                            {
                                price = p1[0] + "." + p1[1];
                            }
                        }
                        else { price = "0.00"; }
                        if (_array[13] != null && _array[13] != "") { productDescription = _array[13].ToString().Trim(); } else { productDescription = ""; }
                        if (_array[14] != null && _array[14] != "")
                        {
                            isActive = _array[14].ToString().Trim();
                            if (isActive.ToLower() == "true") { }
                            else if (isActive.ToLower() == "false") { }
                            else { BoolError += "isActive, "; _isErrorsInRecord = true; BoolErrorLine += totalproductinfeedcount.ToString() + ","; }
                        }
                        else { isActive = "False"; }
                        if (_array[15] != null && _array[15] != "")
                        {
                            isFeatured = _array[15].ToString().Trim();
                            if (isFeatured.ToLower() == "true") { }
                            else if (isFeatured.ToLower() == "false") { }
                            else { BoolError += "isFeatured, "; _isErrorsInRecord = true; BoolErrorLine += totalproductinfeedcount.ToString() + ","; }
                        }
                        else { isFeatured = "False"; }
                        if (_array[16] != null && _array[16] != "") { inventory = _array[16].ToString().Trim(); }
                        else
                        {
                            inventory = "0";
                        }

                        if (_array[17] != null && _array[17] != "") { ArabicName = _array[17].ToString().Trim(); }
                        else
                        {
                            ArabicName = "";
                        }
                        if (_array[18] != null && _array[18] != "") { ArabicDesc = _array[18].ToString().Trim(); }
                        else
                        {
                            ArabicDesc = "";
                        }

                        if (checkValidDataMasterProduct(productName, sku, categoryname, brandname) == false)
                        {
                            //****************Error Record****************
                            totalerrorcount += 1;
                            //_isErrorsInRecord = true;
                            //GlobalVarForError = false;
                            //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            error_line = totalproductinfeedcount.ToString();

                            //break;  // comment of 07_11_2016

                        }

                        isStatus = "False";
                        ImportFileId = Convert.ToString(ImportId);
                        if (error_code != "") { FileError = error_code; } else { FileError = ""; }
                        if (error_line != "") { FileErrorLineNumber = error_line; } else { FileErrorLineNumber = "0"; }


                        totalsucesscount += 1;

                        DataRow dr = dtalldatatoInsert.NewRow();
                        dr["sku"] = sku;
                        dr["barcode"] = barcode;
                        dr["productName"] = productName;
                        dr["isMasterProduct"] = isMasterProduct;
                        dr["isVarientProduct"] = isVarientProduct;
                        dr["varientitem"] = varientitem;
                        dr["master_product_parent"] = master_product_parent;
                        dr["categoryname"] = categoryname;
                        dr["subcategoryname"] = subcategoryname;
                        dr["brandname"] = brandname;
                        dr["minimumQuantity"] = minimumQuantity;
                        dr["cost"] = cost;
                        dr["price"] = price;
                        dr["productDescription"] = Server.HtmlEncode(productDescription.Replace("SRN", "\r\n").Replace("|", ",").Replace("BRS", "br /"));
                        dr["isActive"] = isActive;
                        dr["isFeatured"] = isFeatured;
                        dr["inventory"] = inventory;

                        //static added value
                        dr["isStatus"] = isStatus;
                        dr["ImportFileId"] = ImportFileId;
                        dr["FileError"] = FileError;
                        dr["FileErrorLineNumber"] = FileErrorLineNumber;

                        dr["ArabicName"] = Server.HtmlEncode(ArabicName);
                        dr["ArabicDesc"] = Server.HtmlEncode(ArabicDesc);

                        if (_isErrorsInRecord == false)
                        {
                            dtalldatatoInsert.Rows.Add(dr);
                        }
                        else
                        {
                            objproduct.BoolError = Server.HtmlEncode(BoolError);
                            objproduct.ImportFileId = Convert.ToInt32(ImportId);
                            objproduct.BoolErrorLine = BoolErrorLine;
                            //objproduct.DeleteTempError();

                            objproduct.InsertTempError();
                            //BoolErrorLine = 
                        }

                    }
                    else
                    {
                        //****************Error Record****************
                        totalerrorcount += 1;
                        _isErrorsInRecord = true;
                        errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                        error_line = totalproductinfeedcount.ToString();
                        //*********************************************
                        //sr.WriteLine(_readContent)
                    }
                }
            }
            totalRowNumberError += errorInRowNumber.ToString();
        }

        /// Beacause we have to minus header index from success count
        //int totalSuccessCount = Convert.ToInt32(totalproductinfeedcount) - Convert.ToInt32(totalerrorcount);
        int totalSuccessCount = Convert.ToInt32(totalproductinfeedcount) - Convert.ToInt32(totalerrorcount) - 1;

        lbltotalsuccesscount.Text = "Total records are: " + totalproductinfeedcount + " <br/>";
        lbltotalsuccesscount.Text = "Total updated records are: " + totalSuccessCount + " <br/>";
        //lblerrorinrow.Text = "Total error records are: " + totalRowNumberError + " <br/>";


        if (GlobalVarForError == true)
        {
            BindProductsFromLiveServer(dtalldatatoInsert);
            lbltotalerrorcount.Text = "<br>Some records are not updated successfully due to invalid data, Following rows are not updated successfully. <br> Line numbers are: <b> " + totalRowNumberError.ToString().TrimEnd(',') + " </b> ";
        }
        else
        {
            BindProductsFromLiveServer(dtalldatatoInsert);
            //lbltotalerrorcount.Text = "Error in line numbers is: <b> " + totalproductinfeedcount + " </b> First solve this error.";
            lbltotalsuccesscount.Text = "Total records are: " + totalproductinfeedcount + " <br/>";
            lbltotalsuccesscount.Text = "Total updated records are: " + totalSuccessCount + " <br/>";
            lblerrorinrow.Text = "Total error records line are: " + totalRowNumberError + " <br/>";
        }
        _fileStream.Close();
        _fileStream.Dispose();

        if (!readflag)
        {
            IOException ioex = new IOException();
            throw ioex;
        }

        hidflag.Value = "1";

        return true;
        //#End Region
    }

    #region Check Validate All Data Master Product
    private bool checkValidDataMasterProduct(string productName, string sku, string categoryname, string brandname)
    {
        bool isInsertRow = false;
        bool checkValidtion = true;
        error_code = "";

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
                checkValidtion = false;
                error_code += "SKU missing. </br>";
                //return isInsertRow;
            }
        }
        else
        {
            isInsertRow = false;
            checkValidtion = false;
            error_code += "SKU missing. </br>";
            //return isInsertRow;
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
            checkValidtion = false;
            error_code += "Product name missing. </br>";
            //return isInsertRow;
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
            checkValidtion = false;
            error_code += "Category name missing. </br>";
            //return isInsertRow;
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
            checkValidtion = false;
            error_code += "Brand name missing. </br>";
            //return isInsertRow;
        }
        #endregion

        return checkValidtion;
    }
    #endregion

    #region "get array from comma separated string"
    private string[] getArrayFromCommaSepratedValues(string value)
    {
        try
        {
            int counter = 0;
            //string[] strArray = new string[20];
            string[] strArray = new string[17];
            int indexOfComma = 0;
            int indexOfQuote = 0;
            string strRestOfString = string.Empty;
            string interMediate = string.Empty;
            strRestOfString = value;
            string part = string.Empty;
            while (!string.IsNullOrEmpty(strRestOfString))
            {
                if (strRestOfString.StartsWith("\""))
                {
                    strRestOfString = strRestOfString.Substring(1);
                    indexOfQuote = strRestOfString.IndexOf("\"");
                    part = "\"" + strRestOfString.Substring(0, indexOfQuote + 1);
                    strArray[counter] = part.Trim('\"');
                    counter = counter + 1;
                    if (indexOfQuote != strRestOfString.Length - 1)
                    {
                        strRestOfString = strRestOfString.Substring(indexOfQuote + 2);
                        strRestOfString = strRestOfString.Trim();
                    }
                    else
                    {
                        strRestOfString = "";
                    }
                }
                else
                {
                    indexOfComma = strRestOfString.IndexOf(",");
                    if (indexOfComma == -1)
                    {
                        part = strRestOfString;
                    }
                    else
                    {
                        part = strRestOfString.Substring(0, indexOfComma);
                    }
                    strArray[counter] = part;
                    counter = counter + 1;
                    if (indexOfComma > -1)
                    {
                        strRestOfString = strRestOfString.Substring(indexOfComma + 1);
                        strRestOfString = strRestOfString.Trim();
                    }
                    else
                    {
                        strRestOfString = "";
                    }
                }
            }
            return strArray;
        }
        catch
        {
            return null;
        }
    }

    private string[] getArrayFromTabValues(string value)
    {
        try
        {
            int counter = 0;
            //string[] strArray = new string[20];
            string[] strArray = new string[19];

            string strRestOfString = string.Empty;
            string interMediate = string.Empty;
            strRestOfString = value;
            string part = string.Empty;
            var list = strRestOfString.Split('\t');

            for (int i = 0; i < list.Length; i++)
            {
                strArray[counter] = list[i];
                counter = counter + 1;
            }
            return strArray;
        }
        catch
        {
            return null;
        }
    }

    #endregion

    public void BindProductsFromLiveServer(DataTable dttblUpdate)
    {
        productManager objproduct = new productManager();

        //DataTable dt = new DataTable("tmp_Updatable");
        DataTable dt = new DataTable("tmp_productImport");

        string str = "";
        try
        {

            dt = dttblUpdate;
            // lbltotalerrorcount.Text = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                #region BULK INSERT

                //objproduct.DeleteTempMasterProductRecords();

                //// Copy the DataTable to SQL Server using SqlBulkCopy
                //objproduct.SqlBulkMasterProductCopyOperation(dt);

                //objproduct.InsertUpdateMasterProductFromTemp();
                ////Response.Write("<br>SQL BULK COPY Insert comepleted<br>");

                objproduct.SqlBulkCopyOperationImportProduct(dt);

                #endregion
            }
            else
            {
                // lbltotalerrorcount.Text += "<tr><td colspan=\"6\"><b style=\"color:red;\">No products available on live server.</b></td></tr>";
            }
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); dt = null; dttblUpdate.Dispose(); dttblUpdate = null; objproduct = null; }
    }

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

    #endregion


}