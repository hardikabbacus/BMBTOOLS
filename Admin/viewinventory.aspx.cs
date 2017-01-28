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

public partial class Admin_viewinventory : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ManageInventryPageSize"]);
    string id = "";
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";
    int ImportId = 0;
    string error_code = "";
    string error_line = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Product Inventory List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        gvAdmin.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ManageInventryPageSize"]);
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
                lblmsgs.Text = "Product inventory has been added successfully";
            }
            else if (Request.QueryString["flag"] == "edit")
            {
                //trmsg.Visible = true;
                lblmsgs.Text = "Product inventory has been updated successfully";
            }
            else if (Request.QueryString["flag"] == "delete")
            {
                //trmsg.Visible = true;
                lblmsgs.Text = "Product inventory has been deleted successfully";
            }

            //----------------------Maintain Search------------------------------------
            if (Request.QueryString["key"] != "")
            {
                txtsearch.Text = Request.QueryString["key"];
            }

            if (CommonFunctions.IsQueryString("pageSize", true))
            {
                ddlpageSize.SelectedValue = Request.QueryString["pageSize"];
                pageSize = Convert.ToInt32(Request.QueryString["pageSize"]);
            }
            else
            {
                pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ManageInventryPageSize"]);
                ddlpageSize.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["ManageInventryPageSize"].ToString();
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

    //Bind product
    private void BindProduct(string search = "")
    {
        productManager objproduct = new productManager();
        DataTable dtadmin = new DataTable();
        try
        {
            if (txtsearch.Text != "")
            {
                string strg = txtsearch.Text.Trim();
                string[] arystr = strg.Split('\n');
                //string[] arystr = strg.Split(',');
                string orgstr = string.Empty;
                if (arystr.Length > 0)
                {
                    for (int k = 0; k < arystr.Length; k++)
                    {
                        if (k == 0)
                        {
                            orgstr += "('" + Convert.ToString(arystr[k]) + "'";
                        }
                        else
                        {
                            orgstr += ",'" + Convert.ToString(arystr[k]) + "'";
                        }
                    }
                    orgstr += ")";
                }
                //objproduct.sku = txtsearch.Text.Trim();
                objproduct.sku = orgstr.ToString();
            }
            else
            {
                objproduct.sku = txtsearch.Text.Trim();
            }

            //less than greter than equal to
            if (txtfilter.Text != "") { objproduct.InventoryFilter = txtfilter.Text.Trim(); } else { objproduct.InventoryFilter = txtfilter.Text.Trim(); }
            if (ddlfilter.SelectedValue != "0") { objproduct.filter = ddlfilter.SelectedValue; } else { objproduct.filter = ddlfilter.SelectedValue; }

            if (pageNo == 0) { pageNo = 1; }
            objproduct.pageNo = pageNo;
            objproduct.pageSize = pageSize;
            objproduct.SortExpression = SortExpression;
            querystring = "&pageSize=" + ddlpageSize.SelectedValue + "&key=" + txtsearch.Text;
            //querystring = "&key=" + txtsearch.Text;
            dtadmin = objproduct.SearchProductInventoryItem();
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
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "viewinventory.aspx");
            ltrpaggingbottom.Text = strpaging;
            //Ltrup.Text = strpaging;

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
        pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        BindProduct();
    }

    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        BindProduct();
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
                    // objuser.DeleteAdminRightsItem();
                    objproduct.DeleteItem();
                    //Menu delete logic goes here 
                    //if (System.IO.File.Exists(Server.MapPath("~") + "/admin/menu/" + objuser.adminid + ".htm"))
                    //{
                    //    System.IO.File.Delete(Server.MapPath("~") + "/admin/menu/" + objuser.adminid + ".htm");
                    //}
                }
            }
            Response.Redirect("viewinventory.aspx?flag=delete&key=" + txtsearch.Text + "");
            //Response.Redirect("viewinventory.aspx?flag=delete");
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


    protected void gvAdmin_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvAdmin.EditIndex = e.NewEditIndex;
        BindProduct();
    }
    protected void gvAdmin_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string productid = gvAdmin.DataKeys[e.RowIndex].Values["productId"].ToString();
        TextBox inventory = (TextBox)gvAdmin.Rows[e.RowIndex].FindControl("txtInventory");

        productManager objprodct = new productManager();
        try
        {
            objprodct.inventory = Convert.ToInt32(inventory.Text);
            objprodct.productId = Convert.ToInt32(productid);
            objprodct.UpdateInventory();
            gvAdmin.EditIndex = -1;
            BindProduct();
            lblmsg.Visible = true;
            lblmsgs.Text = "Product inventory updated successfully.";

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objprodct = null; }
        //con.Open();
        //SqlCommand cmd = new SqlCommand("update stores set stor_name='" + stor_name.Text + "', stor_address='" + stor_address.Text + "', city='" + city.Text + "', state='" + state.Text + "', zip='" + zip.Text + "' where stor_id=" + stor_id, con);
        //cmd.ExecuteNonQuery();
        //con.Close();
        //lblmsg.BackColor = Color.Blue;
        //lblmsg.ForeColor = Color.White;
        //lblmsg.Text = stor_id + "        Updated successfully........    ";
        //gridView.EditIndex = -1;
        //loadStores();
    }
    protected void gvAdmin_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvAdmin.EditIndex = -1;
        BindProduct();
    }

    protected void btnApply_Click1(object sender, EventArgs e)
    {
        BindProduct();
    }

    // create csv

    #region inventory csv

    productManager objproduct = new productManager();

    private void GernerateProductCSV()
    {

        DataTable dsProduct = new DataTable();
        try
        {
            if (txtsearch.Text != "")
            {
                string strg = txtsearch.Text.Trim();
                //string[] arystr = strg.Split(',');
                string[] arystr = strg.Split('\n');
                string orgstr = string.Empty;
                if (arystr.Length > 0)
                {
                    for (int k = 0; k < arystr.Length; k++)
                    {
                        if (k == 0)
                        {
                            orgstr += "('" + Convert.ToString(arystr[k].Replace("\r", "")) + "'";
                        }
                        else
                        {
                            orgstr += ",'" + Convert.ToString(arystr[k].Replace("\r", "")) + "'";
                        }
                    }
                    orgstr += ")";
                }
                //objproduct.sku = txtsearch.Text.Trim();
                objproduct.sku = orgstr.ToString();
            }
            else
            {
                objproduct.sku = txtsearch.Text.Trim();
            }

            if (txtfilter.Text != "") { objproduct.InventoryFilter = txtfilter.Text.Trim(); } else { objproduct.InventoryFilter = txtfilter.Text.Trim(); }
            if (ddlfilter.SelectedValue != "0") { objproduct.filter = ddlfilter.SelectedValue; } else { objproduct.filter = ddlfilter.SelectedValue; }

            dsProduct = objproduct.SelectInventoryDetailForGenerateXls();

            string strtodaydate = Convert.ToDateTime(System.DateTime.Now.Date).ToString("dd/MM/yyyy");
            string strtotime = Convert.ToDateTime(System.DateTime.Now).ToString("hh:mm");
            string strFileName = "Inventory-" + strtodaydate.ToString().Replace("/", "-") + "__" + strtotime.Replace(":", "-") + ".xls";

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
                Response.Write("sku\tinventory");
                Response.End();
            }



        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { dsProduct.Dispose(); }
    }

    protected void btndownload_Click(object sender, EventArgs e)
    {
        GernerateProductCSV();
    }
    protected void btnimport_Click(object sender, EventArgs e)
    {
        productManager objproduct = new productManager();
        if (Page.IsValid)
        {

            string extension = Path.GetExtension(fluUploadCsv.FileName).ToLower();
            if (extension == ".csv" || extension == ".xls" || extension == ".xlsx")
            {
                string filetype = "xls";
                bool isValidFile = false;

                string fileUploadLocation = string.Empty;
                DirectoryInfo dirInfo = null;
                string uploadedFileName = string.Empty;
                string uploadedFileFullPath = string.Empty;
                string PhysicalPath = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ErrorInventoryUploadRootPath"].ToString());
                fileUploadLocation = (PhysicalPath);
                dirInfo = new DirectoryInfo(fileUploadLocation);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                uploadedFileName = fluUploadCsv.FileName;
                uploadedFileFullPath = (fileUploadLocation + "\\") + uploadedFileName;
                fluUploadCsv.PostedFile.SaveAs(uploadedFileFullPath);

                importjobManager objImport = new importjobManager();

                //count line of the file
                var lines = File.ReadAllLines(uploadedFileFullPath);
                var totalcols = lines[0].Split('\t').Length;

                if (totalcols == 2)
                {
                    try
                    {
                        objImport.importfilename = uploadedFileName;
                        objImport.filestatus = "ForReview";
                        objImport.importType = Convert.ToString(2);
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

                    Response.Redirect("ViewImportInventoryJobs.aspx?id=" + ImportId + "&filename=" + uploadedFileName + "&filetp=2");
                }
                else
                {
                    lblError.Text = "Please upload a valid csv file.";
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



        _fileForErrorRecords = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ErrorInventoryUploadRootPath"].ToString());

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

            if (_arraytop == null || _arraytop.Length < 2 || _arraytop.Length > 3)
            {
                totalerrorcount += 1;
                _isErrorsInRecord = true;

                // _fileStream.Close();
                // _fileStream.Dispose();

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
            _readContent = _readContent.Trim();
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
                    if (_array.Length == 2)
                    {
                        ////Number of cols in csv/txt file
                        //if (_array[0] != null)
                        //{
                        //    if (_array[1] != null)
                        //    {

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
                        if (error_code != "") { FileError = error_code; } else { FileError = ""; }
                        if (errorInRowNumber != "") { FileErrorLineNumber = error_line; } else { FileErrorLineNumber = "0"; }

                        //    }
                        //    else
                        //    {
                        //        totalerrorcount += 1;
                        //        _isErrorsInRecord = true;
                        //        errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                        //    }
                        //}
                        //else
                        //{
                        //    totalerrorcount += 1;
                        //    _isErrorsInRecord = true;
                        //    errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                        //}


                        //if (_isErrorsInRecord == false)
                        //{
                        if (checkValidDatatableData(sku, inventory) == false)
                        {
                            //****************Error Record****************
                            totalerrorcount += 1;
                            //_isErrorsInRecord = true;
                            //GlobalVarForError = false;
                            error_line = totalproductinfeedcount.ToString();
                            errorInRowNumber = totalproductinfeedcount.ToString() + ",";

                            //break;  // comment of 07_11_2016
                            //*********************************************
                            //sr.WriteLine(_readContent)
                        }
                        //    else
                        //    {
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
                        //    }
                        //}
                        //else
                        //{
                        //    GlobalVarForError = true;
                        //}

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
            //uncomment for error display
            //lbltotalerrorcount.Text = "Error in line numbers is: <b> " + totalproductinfeedcount + " </b> First solve this error.";
            lbltotalsuccesscount.Text = "Total records are: " + totalproductinfeedcount + " <br/>";
            lbltotalsuccesscount.Text = "Total updated records are: " + totalSuccessCount + " <br/>";
            lblerrorinrow.Text = "Total error records are: " + totalRowNumberError + " <br/>";
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

    #region "get array from comma separated string"
    private string[] getArrayFromCommaSepratedValues(string value)
    {
        try
        {
            int counter = 0;
            //string[] strArray = new string[20];
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

    private string[] getArrayFromTabValues(string value)
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

    #region Check Validate All Data
    private bool checkValidDatatableData(string sku, string inventory)
    {

        bool isInsertRow = false;


        #region sku
        if (CommonFunctions.IsValidValue(sku, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            //error_code += "Invalid Sku";

            return isInsertRow;
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

    public void BindProductsFromLiveServer(DataTable dttblUpdate)
    {
        //DataTable dt = new DataTable("tmp_Inventory");

        DataTable dt = new DataTable("tmp_productImport");

        string str = "";
        try
        {

            dt = dttblUpdate;
            //lbltotalerrorcount.Text = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                #region BULK INSERT

                //objproduct.DeleteTempInventoryRecords();

                //// Copy the DataTable to SQL Server using SqlBulkCopy
                //objproduct.SqlBulkCopyOperationInventory(dt);

                //objproduct.InsertUpdateInventoryFromTemp();

                objproduct.SqlBulkCopyOperationImportProduct(dt);


                #endregion
            }
            else
            {
                lbltotalerrorcount.Text += "<tr><td colspan=\"6\"><b style=\"color:red;\">No products available on live server.</b></td></tr>";
            }

            //Response.Redirect("viewinventory.aspx?flag=csv");
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); dt = null; dttblUpdate.Dispose(); dttblUpdate = null; }
    }

    #endregion


    protected void btnUpdateAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvAdmin.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    productManager objprodct = new productManager();
                    try
                    {
                        objprodct.productId = Convert.ToInt32(row.Cells[4].Controls.OfType<HiddenField>().FirstOrDefault().Value);
                        objprodct.inventory = Convert.ToInt32(row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Text);
                        objprodct.UpdateInventory();
                        gvAdmin.EditIndex = -1;
                        //BindProduct();
                        lblmsg.Visible = true;
                        lblmsgs.Text = "Product inventory updated successfully.";

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { objprodct = null; }
                }
            }
        }
        BindProduct();
        lblmsg.Visible = true;
        lblmsgs.Text = "Product inventory updated successfully.";
    }

    protected void OnCheckedChanged(object sender, EventArgs e)
    {
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

    }

}