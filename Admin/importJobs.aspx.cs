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
public partial class Admin_importJobs : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ImportJobsPageSize"]);
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";
    int ImportId = 0;
    string error_code = "";
    string error_line = "";

    productManager objproduct = new productManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Import Job List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        gvImportjob.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ImportJobsPageSize"]);
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
            else if (Request.QueryString["filename"] != null)
            {
                //trmsg.Visible = true;
                lblmsg.Visible = true;
                lblmsgs.Text = "Import Job has been canceled successfully";
            }

            if (CommonFunctions.IsQueryString("pageSize", true))
            {
                ddlpageSize.SelectedValue = Request.QueryString["pageSize"];
                pageSize = Convert.ToInt32(Request.QueryString["pageSize"]);
            }
            else
            {
                pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ImportJobsPageSize"]);
                ddlpageSize.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["ImportJobsPageSize"].ToString();
            }

            BindContents();
        }
    }

    //Biniding content
    protected void BindContents(string search = "")
    {
        importjobManager objimportjob = new importjobManager();
        DataTable dtcontents = new DataTable();
        try
        {
            objimportjob.importfilename = Server.HtmlEncode(txtsearch.Text.Trim());
            if (ddlsearch.SelectedValue != "0")
            {
                objimportjob.importType = ddlsearch.SelectedValue.ToString();
            }
            else
            {
                objimportjob.importType = ddlsearch.SelectedValue.ToString();
            }
            if (pageNo == 0) { pageNo = 1; }
            objimportjob.pageNo = pageNo;
            objimportjob.pageSize = pageSize;
            objimportjob.SortExpression = SortExpression;
            querystring = "&pageSize=" + ddlpageSize.SelectedValue + "&jobtype=" + ddlsearch.SelectedValue + "&key=" + txtsearch.Text;
            dtcontents = objimportjob.SearchItem();
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
                int startRowOnPage = (gvImportjob.PageIndex * gvImportjob.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + gvImportjob.Rows.Count - 1;
                int totalRows = totalrecs;
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPaging(totalpages, pageNo, querystring, "importjobs.aspx");
            ltrpaggingbottom.Text = strpaging;
            //LoadDropDownList();
        }
        catch (Exception ex) { throw ex; }
    }

    //handle row data bound
    protected void gvImportjob_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int productid = Convert.ToInt32(gvImportjob.DataKeys[e.Row.RowIndex].Value);
            Label impfilename = new Label();
            impfilename = (Label)e.Row.FindControl("lblfilename");

            Label impfiletype = new Label();
            impfiletype = (Label)e.Row.FindControl("lblimpfile");

            HtmlAnchor imgAnc = new HtmlAnchor();
            imgAnc = (HtmlAnchor)e.Row.FindControl("BtnRedirect");

            Label lblimptyp = new Label();
            lblimptyp = (Label)e.Row.FindControl("lblImportType");

            if (impfiletype.Text == "1")
            {
                imgAnc.HRef = "viewImportJobDetails.aspx?id=" + productid + "&filename=" + impfilename.Text + "&filetp=" + impfiletype.Text;
                lblimptyp.Text = "Product";
            }
            if (impfiletype.Text == "2")
            {
                imgAnc.HRef = "ViewImportInventoryJobs.aspx?id=" + productid + "&filename=" + impfilename.Text + "&filetp=" + impfiletype.Text;
                lblimptyp.Text = "Inventory";
            }
            if (impfiletype.Text == "3")
            {
                imgAnc.HRef = "viewImportJobDetails.aspx?id=" + productid + "&filename=" + impfilename.Text + "&filetp=" + impfiletype.Text;
                lblimptyp.Text = "Master Product";
            }

            //// complete / cancel / review 
            Label lblAvailData = new Label();
            lblAvailData = (Label)e.Row.FindControl("lblAvailData");
            Label lblCompleteIncomplete = new Label();
            lblCompleteIncomplete = (Label)e.Row.FindControl("lblCompleteIncomplete");
            Label lblfilestatus = new Label();
            lblfilestatus = (Label)e.Row.FindControl("lblfilestatus");

            if (lblAvailData.Text == "0")
            {
                lblfilestatus.Text = "Cancel";
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                //e.Row.ForeColor = System.Drawing.Color.Red;
            }
            else if (lblCompleteIncomplete.Text == "0")
            {
                lblfilestatus.Text = "Complete";
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                //e.Row.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblfilestatus.Text = "For Review";
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Orange;
                //e.Row.ForeColor = System.Drawing.Color.Orange;
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

    #region ------------ product and inventory import ----------------------------

    protected void btnimport_Click(object sender, EventArgs e)
    {
        productManager objproduct = new productManager();
        if (Page.IsValid)
        {
            int cntID = 0;
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (extension == ".xls" || extension == ".xlsx")
            {
                string filetype = "xls";
                bool isValidFile = false;

                string fileUploadLocation = string.Empty;
                DirectoryInfo dirInfo = null;
                string uploadedFileName = string.Empty;
                string uploadedFileFullPath = string.Empty;
                int typeid = 0;

                string PhysicalPath = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ImportProduct"].ToString());
                fileUploadLocation = (PhysicalPath);
                dirInfo = new DirectoryInfo(fileUploadLocation);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                uploadedFileName = file.FileName;
                uploadedFileFullPath = (fileUploadLocation + "\\") + uploadedFileName;
                file.PostedFile.SaveAs(uploadedFileFullPath);

                importjobManager objImport = new importjobManager();

                //count line of the file
                var lines = File.ReadAllLines(uploadedFileFullPath);
                var totalcols = lines[0].Split('\t').Length;

                if (totalcols == 2 && ddlselecttype.SelectedValue == "2")
                {
                    try
                    {
                        objImport.importfilename = uploadedFileName;
                        objImport.filestatus = "ForReview";
                        objImport.importType = Convert.ToString(ddlselecttype.SelectedValue);

                        //// check file name already exist 
                        //cntID = objImport.isExistFileName();
                        //if (cntID == 0)
                        //{
                        objImport.InsertItem();
                        ImportId = objImport.getmaxid();
                        //}

                        //if (cntID == 0)
                        //{
                        isValidFile = newvalidateCSVandTXTFiles_Inventory(uploadedFileFullPath, filetype);

                        if (File.Exists(uploadedFileFullPath))
                        {
                            File.Delete(uploadedFileFullPath);
                        }
                        lblmsg.Visible = true;
                        typeid = 2;

                        Response.Redirect("ViewImportInventoryJobs.aspx?id=" + ImportId + "&filename=" + uploadedFileName + "&filetp=2");
                        //}
                        //else
                        //{
                        //    lblmsg.Visible = true;
                        //    lblmsgs.Text = "Filename is already exist.";
                        //}
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    finally { objImport = null; }
                }
                else if (totalcols == 19 && ddlselecttype.SelectedValue == "1")
                {
                    try
                    {
                        objImport.importfilename = uploadedFileName;
                        objImport.filestatus = "ForReview";
                        objImport.importType = Convert.ToString(ddlselecttype.SelectedValue);

                        //// check file name already exist
                        //cntID = objImport.isExistFileName();
                        //if (cntID == 0)
                        //{
                        objImport.InsertItem();
                        ImportId = objImport.getmaxid();
                        //}

                        //if (cntID == 0)
                        //{
                        isValidFile = newvalidateCSVandTXTFiles_Automation(uploadedFileFullPath, filetype);

                        if (File.Exists(uploadedFileFullPath))
                        {
                            File.Delete(uploadedFileFullPath);
                        }

                        lblmsg.Visible = false;
                        typeid = 1;

                        Response.Redirect("viewImportJobDetails.aspx?id=" + ImportId + "&filename=" + uploadedFileName + "&filetp=1");
                        //}
                        //else
                        //{
                        //    lblmsg.Visible = true;
                        //    lblmsgs.Text = "Filename is already exist.";
                        //}

                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    finally { objImport = null; }
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsgs.Text = "Please select valid type.";
                }

                //if (typeid == 1)
                //{ Response.Redirect("viewImportJobDetails.aspx?id=" + ImportId + "&filename=" + uploadedFileName + "&filetp=1"); }
                //else
                //{ Response.Redirect("ViewImportInventoryJobs.aspx?id=" + ImportId + "&filename=" + uploadedFileName + "&filetp=2"); }



            }
            else
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "Please upload a valid csv file.";
            }

            BindContents();
        }

    }

    #region --------------------- Product Import ---------------------------

    public bool newvalidateCSVandTXTFiles_Automation(string filepath, string filetype)
    {
        int totalproductinfeedcount = 1;
        int totalerrorcount = 0;

        bool _isErrorsInRecord = false;


        FileStream fs = null;
        StreamWriter sr = null;
        StreamReader _fileStream = default(StreamReader);
        string _fileToRead = filepath;
        string _fileForErrorRecords = "";

        string BoolErrorLine = "";
        string BoolError = "";

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

            if (_arraytop == null || _arraytop.Length < 19 || _arraytop.Length > 20)
            {
                totalerrorcount += 1;
                _isErrorsInRecord = true;

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

            BoolError = "";
            BoolErrorLine = "";

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
                else if (filetype.ToLower().Equals("xls") || filetype.ToLower().Equals("xlsx"))
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
                        //Number of cols in csv/txt file
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
                            else { BoolError += "isMasterProduct, "; _isErrorsInRecord = true; BoolErrorLine = totalproductinfeedcount.ToString(); }
                        }
                        else { isMasterProduct = "Fasle"; }
                        if (_array[4] != null && _array[4] != "")
                        {
                            isVarientProduct = _array[4].ToString().Trim();
                            if (isVarientProduct.ToLower() == "true") { }
                            else if (isVarientProduct.ToLower() == "false") { }
                            else { BoolError += "isVarientProduct, "; _isErrorsInRecord = true; BoolErrorLine = totalproductinfeedcount.ToString(); }
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

                        if (_array[10] != null && _array[10] != "") { minimumQuantity = _array[10].ToString().Trim().Replace(",", ""); }
                        else
                        {
                            minimumQuantity = "0";
                            //totalerrorcount += 1;
                            ////_isErrorsInRecord = true;
                            //error_code += "Minimum Quentity missing. ";
                            //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            //error_line = totalproductinfeedcount.ToString();
                        }
                        if (_array[11] != null && _array[11] != "")
                        {
                            cost = _array[11].ToString().Trim().Replace(",", "");

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
                            if (p1.Count() > 1)
                            {
                                if (p1[1].Length > 2)
                                {
                                    price = p1[0] + "." + p1[1].Substring(0, 2);
                                }
                                else
                                {
                                    price = p1[0] + "." + p1[1];
                                }
                            }
                            //price = price.Substring(price.IndexOf('.') + 1);
                        }
                        else
                        {
                            price = "0.00";
                            //totalerrorcount += 1;
                            ////_isErrorsInRecord = true;
                            //error_code += "Price missing. ";
                            //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            //error_line = totalproductinfeedcount.ToString();
                        }
                        if (_array[13] != null && _array[13] != "") { productDescription = _array[13].ToString().Trim(); } else { productDescription = ""; }
                        if (_array[14] != null && _array[14] != "")
                        {
                            isActive = _array[14].ToString().Trim();
                            if (isActive.ToLower() == "true") { }
                            else if (isActive.ToLower() == "false") { }
                            else { BoolError += "isActive, "; _isErrorsInRecord = true; BoolErrorLine = totalproductinfeedcount.ToString(); }
                        }
                        else { isActive = "False"; }
                        if (_array[15] != null && _array[15] != "")
                        {
                            isFeatured = _array[15].ToString().Trim();
                            if (isFeatured.ToLower() == "true") { }
                            else if (isFeatured.ToLower() == "false") { }
                            else { BoolError += "isFeatured, "; _isErrorsInRecord = true; BoolErrorLine = totalproductinfeedcount.ToString(); }
                        }
                        else { isFeatured = "False"; }
                        if (_array[16] != null && _array[16] != "") { inventory = _array[16].ToString().Trim(); }
                        else
                        {
                            inventory = "0";
                            //totalerrorcount += 1;
                            ////_isErrorsInRecord = true;
                            //error_code += "Inventory missing. ";
                            //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            //error_line = totalproductinfeedcount.ToString();
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

                        if (isMasterProduct.ToLower() == "false")
                        {
                            if (isVarientProduct.ToLower() == "false")
                            {
                                if (checkValidDataProduct(productName, sku, categoryname, brandname, price, minimumQuantity, inventory) == false)
                                {
                                    error_line = totalproductinfeedcount.ToString();
                                }
                            }
                            else
                            {
                                if (checkValidDataSinglsProduct(productName, sku, price, minimumQuantity, inventory, master_product_parent) == false)
                                {
                                    error_line = totalproductinfeedcount.ToString();
                                }
                            }
                            //checkValidDataProduct(productName, sku, categoryname, brandname, price, minimumQuantity, inventory);
                            //error_line = totalproductinfeedcount.ToString();
                        }
                        else
                        {
                            if (checkValidDataMasterProduct(productName, sku, categoryname, brandname) == false)
                            {
                                error_line = totalproductinfeedcount.ToString();
                            }
                        }


                        isStatus = "False";
                        ImportFileId = Convert.ToString(ImportId);
                        if (error_code != "") { FileError = error_code; } else { FileError = ""; }
                        if (error_line != "") { FileErrorLineNumber = error_line; } else { FileErrorLineNumber = "0"; }


                        // insert into the datarow

                        //if (checkValidDatatableData(productId, productName, productDescription, sku, barcode, isVarientProduct, isMasterProduct, price, cost, minimumQuantity, inventory, isActive, isFeatured) == false)
                        //{
                        //    //****************Error Record****************
                        //    totalerrorcount += 1;
                        //    //_isErrorsInRecord = true;
                        //    //GlobalVarForError = false;
                        //    errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                        //}

                        DataRow dr = dtalldatatoInsert.NewRow();
                        //dr["productId"] = productId;
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

                        //dtalldatatoInsert.Rows.Add(dr);

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



        // insert data into temp_productImport table
        BindProductsFromLiveServer(dtalldatatoInsert);

        _fileStream.Close();
        _fileStream.Dispose();

        if (!readflag)
        {
            IOException ioex = new IOException();
            throw ioex;
        }

        return true;

    }


    // get array from comma sepreted string
    private string[] getArrayFromCommaSepratedValues(string value)
    {
        try
        {
            int counter = 0;
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

    // insert all the data into the temp_productImport table
    public void BindProductsFromLiveServer(DataTable dttblUpdate)
    {
        productManager objproduct = new productManager();
        DataTable dt = new DataTable("tmp_productImport");
        string str = "";
        try
        {
            dt = dttblUpdate;
            // lbltotalerrorcount.Text = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                #region BULK INSERT

                // Copy the DataTable to SQL Server using SqlBulkCopy
                objproduct.SqlBulkCopyOperationImportProduct(dt);

                #endregion
            }
            else
            {
                // lbltotalerrorcount.Text += "<tr><td colspan=\"6\"><b style=\"color:red;\">No products available on live server.</b></td></tr>";
            }
        }
        catch (Exception ex)
        {
            //throw ex; 
        }
        finally { dt.Dispose(); dt = null; dttblUpdate.Dispose(); dttblUpdate = null; }
    }

    #endregion

    #region --------------------- Inventory Import ---------------------------

    public bool newvalidateCSVandTXTFiles_Inventory(string filepath, string filetype)
    {
        //int totalproductinfeedcount = 0;
        int totalproductinfeedcount = 1;
        int totalerrorcount = 0;
        int totalsucesscount = 0;
        bool _isErrorsInRecord = false;

        FileStream fs = null;
        StreamWriter sr = null;
        StreamReader _fileStream = default(StreamReader);
        string _fileToRead = filepath;
        string _fileForErrorRecords = "";

        #region Create Datatable To Update Records
        DataTable dtalldatatoInsert = new DataTable();
        DataColumn dc = new DataColumn("sku");
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


        #endregion



        _fileForErrorRecords = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ImportInventory"].ToString());

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
                _arraytop = getArrayFromCommaSepratedValuesInventory(_readContent);
            }
            else if (filetype.ToLower().Equals("txt"))
            {
                //For TXT
                _arraytop = _readContent.Split(new char[] { ControlChars.Tab });
            }
            else if (filetype.ToLower().Equals("xls") || filetype.ToLower().Equals("xlsx"))
            {
                _arraytop = getArrayFromTabValuesInventory(_readContent);
            }

            if (_arraytop == null || _arraytop.Length < 2 || _arraytop.Length > 3)
            {
                totalerrorcount += 1;
                _isErrorsInRecord = true;
            }
        }


        string Sqlstring = null;


        string sku = null;
        string inventory = null;

        // static data import into temp table 
        string isStatus = "False";
        string ImportFileId = "";
        string FileError = "";
        string FileErrorLineNumber = "";

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
            _readContent = _readContent.Trim();
            //Check for line blank or not            
            if (_readContent.Length > 0)
            {
                readflag = true;
                string[] _array = null;

                if (filetype.ToLower().Equals("csv"))
                {
                    _array = getArrayFromCommaSepratedValuesInventory(_readContent);
                }
                else if (filetype.ToLower().Equals("txt"))
                {
                    //For TXT
                    _array = _readContent.Split(new char[] { ControlChars.Tab });
                }
                else if (filetype.ToLower().Equals("xls") || filetype.ToLower().Equals("xlsx"))
                {
                    _array = getArrayFromTabValuesInventory(_readContent);
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
                    if (_array.Length == 2)
                    {

                        if (_array[0] != null && _array[0] != "") { sku = _array[0].ToString().Trim(); }
                        else
                        {
                            sku = "";
                            //totalerrorcount += 1;
                            ////_isErrorsInRecord = true;
                            //error_code = "SKU missing. ";
                            //errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            //error_line = totalproductinfeedcount.ToString();
                        }
                        if (_array[1] != null && _array[1] != "") { inventory = _array[1].ToString().Trim(); } else { inventory = "0"; }

                        isStatus = "False";
                        ImportFileId = Convert.ToString(ImportId);
                        if (error_code != "") { FileError = error_code; } else { FileError = null; }
                        if (errorInRowNumber != "") { FileErrorLineNumber = error_line; } else { FileErrorLineNumber = "0"; }

                        if (checkValidDatatableDataInventory(sku, inventory) == false)
                        {
                            //****************Error Record****************
                            totalerrorcount += 1;
                            //_isErrorsInRecord = true;
                            //GlobalVarForError = false;
                            errorInRowNumber = totalproductinfeedcount.ToString() + ",";

                            //break;  // comment of 07_11_2016
                            //*********************************************
                            //sr.WriteLine(_readContent)
                        }

                        totalsucesscount += 1;

                        DataRow dr = dtalldatatoInsert.NewRow();
                        dr["sku"] = sku;
                        dr["inventory"] = inventory;
                        //static added value
                        dr["isStatus"] = isStatus;
                        dr["ImportFileId"] = ImportFileId;
                        dr["FileError"] = FileError;
                        dr["FileErrorLineNumber"] = FileErrorLineNumber;

                        dtalldatatoInsert.Rows.Add(dr);


                    }
                    else
                    {
                        //****************Error Record****************
                        totalerrorcount += 1;
                        _isErrorsInRecord = true;
                        errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                        //*********************************************
                        //sr.WriteLine(_readContent)
                    }
                }
            }
            totalRowNumberError += errorInRowNumber.ToString();
        }

        /// Beacause we have to minus header index from success count
        int totalSuccessCount = Convert.ToInt32(totalproductinfeedcount) - Convert.ToInt32(totalerrorcount) - 1;

        // insert data into temp_productImport table
        BindProductsFromLiveServerInventory(dtalldatatoInsert);

        _fileStream.Close();
        _fileStream.Dispose();

        if (!readflag)
        {
            IOException ioex = new IOException();
            throw ioex;
        }

        return true;

    }

    // insert all the data into the temp_productImport table
    public void BindProductsFromLiveServerInventory(DataTable dttblUpdate)
    {
        productManager objproduct = new productManager();
        DataTable dt = new DataTable("tmp_productImport");
        string str = "";
        try
        {
            dt = dttblUpdate;
            // lbltotalerrorcount.Text = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                #region BULK INSERT

                // Copy the DataTable to SQL Server using SqlBulkCopy
                objproduct.SqlBulkCopyOperationImportProduct(dt);

                #endregion
            }
            else
            {
                // lbltotalerrorcount.Text += "<tr><td colspan=\"6\"><b style=\"color:red;\">No products available on live server.</b></td></tr>";
            }
        }
        catch (Exception ex)
        {
            //throw ex; 
        }
        finally { dt.Dispose(); dt = null; dttblUpdate.Dispose(); dttblUpdate = null; }
    }

    // get array from comma sepreted string
    private string[] getArrayFromCommaSepratedValuesInventory(string value)
    {
        try
        {
            int counter = 0;
            string[] strArray = new string[2];

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

    private string[] getArrayFromTabValuesInventory(string value)
    {
        try
        {
            int counter = 0;
            //string[] strArray = new string[20];
            string[] strArray = new string[2];

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

    #endregion

    #region Check Validate All Data Inventory
    private bool checkValidDatatableDataInventory(string sku, string inventory)
    {

        bool isInsertRow = false;
        bool checkValidtion = true;


        #region sku
        if (CommonFunctions.IsValidValue(sku, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            error_code += "Invalid Sku";
            checkValidtion = false;
            //return isInsertRow;
        }
        #endregion


        #region inventory
        if (inventory != "")
        {
            if (CommonFunctions.IsValidValue(inventory, true, false, false))
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                checkValidtion = false;
                //return isInsertRow;
            }
        }
        #endregion


        return checkValidtion;
    }
    #endregion

    #region Check Validate All Data Product
    private bool checkValidDataProduct(string productName, string sku, string categoryname, string brandname, string price, string minimumQuantity, string inventory)
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
                checkValidtion = false;
                error_code += "Inventory missing. </br>";
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
                    checkValidtion = false;
                    error_code += "Price missing. </br>";
                    //return isInsertRow;
                }
            }
            else
            {
                isInsertRow = false;
                error_code += "Price missing. </br>";
                checkValidtion = false;
                //return isInsertRow;
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
                checkValidtion = false;
                error_code += "Minimum Quentity missing. </br>";
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
                checkValidtion = false;
                error_code += "Inventory must be gereter then minimumQuentity. ";
                //return isInsertRow;
            }
        }
        #endregion

        return checkValidtion;
    }
    #endregion

    #region Check Validate All Data Product
    private bool checkValidDataSinglsProduct(string productName, string sku, string price, string minimumQuantity, string inventory, string master_product_parent)
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
                checkValidtion = false;
                error_code += "Inventory missing. </br>";
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
                    checkValidtion = false;
                    error_code += "Price missing. </br>";
                    //return isInsertRow;
                }
            }
            else
            {
                isInsertRow = false;
                checkValidtion = false;
                error_code += "Price missing. </br>";
                //return isInsertRow;
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
                checkValidtion = false;
                error_code += "Minimum Quentity missing. </br>";
                //return isInsertRow;
            }
        }
        #endregion

        #region master_product_parent
        if (CommonFunctions.IsValidValue(master_product_parent, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            checkValidtion = false;
            error_code += "Master product parent missing. </br>";
            //return isInsertRow;
        }
        #endregion

        #region
        if (minimumQuantity != "" && inventory != "")
        {
            if (Convert.ToInt32(minimumQuantity) > Convert.ToInt32(inventory))
            {
                isInsertRow = false;
                checkValidtion = false;
                error_code += "Inventory must be gereter then minimumQuentity. ";
                //return isInsertRow;
            }
        }
        #endregion

        return checkValidtion;
    }
    #endregion

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

    protected void imgbtnDelete_Click(object sender, EventArgs e)
    {
        importjobManager objimportjob = new importjobManager();
        try
        {
            int con = 0;
            CheckBox chk = new CheckBox();

            DataSet dsadmin = new DataSet();

            for (int i = 0; i < gvImportjob.Rows.Count; i++)
            {
                chk = (CheckBox)(gvImportjob.Rows[i].FindControl("chkDelete"));
                if (chk.Checked == true)
                {
                    con += 1;

                    objimportjob.id = Convert.ToInt32(gvImportjob.DataKeys[gvImportjob.Rows[i].RowIndex].Value.ToString());

                    objimportjob.deleteImportJobTable(); // delete data from tmp_productImport table if avaliable for selected jobs
                    objimportjob.DeleteImportJobs();  // delete record from importjobhistory table whicj are selected

                }
            }

            Response.Redirect("importjobs.aspx?flag=delete");
        }
        catch (Exception ex) { throw ex; }
        finally { objimportjob = null; }
    }
    protected void imgbtnSearch_Click(object sender, EventArgs e)
    {
        gvImportjob.PageIndex = 0;
        pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        BindContents();
    }

    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        BindContents();
    }

    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            ddlpageSize.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    protected void ddlsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindContents();
    }
    protected void gvImportjob_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int ImportJobId = Convert.ToInt32(e.CommandArgument.ToString());
        if (ImportJobId != 0)
        {
            importjobManager objimportjob = new importjobManager();
            try
            {
                objimportjob.id = ImportJobId;
                objimportjob.RemoveSingleImportjob();
                Response.Redirect("importjobs.aspx?flag=delete");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { objimportjob = null; }
        }
    }
    protected void btnApply_Click(object sender, EventArgs e)
    {
        BindContents();
    }
}