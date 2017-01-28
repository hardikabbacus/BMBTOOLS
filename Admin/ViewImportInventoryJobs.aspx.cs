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

public partial class Admin_ViewImportInventoryJobs : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(AppSettings.PAGESIZE);
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";
    int ImportId = 0;
    string error_code = string.Empty;

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
            querystring = "&pageSize=" + ddlpageSize.SelectedValue;

            dtcontents = objimportjob.SearchImportInventoryJobs();
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
                lblinsertfilecount.Text = "Proccessed " + dtcontents.Rows[0]["Proccessed"] + " rows of which " + dtcontents.Rows[0]["NoErrorLine"] + " passed validation and " + dtcontents.Rows[0]["ErrorLine"] + " failed.";
                int startRowOnPage = (gvImportjob.PageIndex * gvImportjob.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + gvImportjob.Rows.Count - 1;
                int totalRows = totalrecs;
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPaging(totalpages, pageNo, querystring, "ViewImportInventoryJobs.aspx");
            ltrpaggingbottom.Text = strpaging;
            //LoadDropDownList();
        }
        catch (Exception ex)
        {
            //throw ex; 
        }
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
                    string Id = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;

                    productManager objprodct = new productManager();
                    DataTable dtpro = new DataTable();
                    try
                    {
                        // get the data from temp table
                        objprodct.id = Convert.ToInt32(Id);
                        dtpro = objprodct.GetSingleImportInventoryValue();

                        string sku = null;
                        string inventory = null;

                        //error_code = string.Empty;
                        bool checkValid = false;

                        sku = Convert.ToString(dtpro.Rows[0]["sku"]);
                        inventory = Convert.ToString(dtpro.Rows[0]["inventory"]);

                        checkValid = checkValidDatatableDataInventory(sku, inventory);
                        if (checkValid == false)
                        {
                            objprodct.FileError = error_code;
                            objprodct.FileErrorLineNumber = Convert.ToInt32(1);
                            objprodct.updateSingleImportProduct();
                        }
                        else
                        {
                            if (dtpro.Rows.Count > 0)
                            {
                                objprodct.sku = Convert.ToString(dtpro.Rows[0]["sku"]);
                                objprodct.inventory = Convert.ToInt32(dtpro.Rows[0]["inventory"]);
                            }

                            int count = objprodct.GetSkuCount();

                            if (count > 0)
                            {
                                objprodct.productId = count;
                                objprodct.UpdateInventory();
                                // update status of page
                                objprodct.isStatus = Convert.ToByte(1);
                                objprodct.UpdateTempTableStatus();

                                objprodct.FileError = error_code;
                                objprodct.FileErrorLineNumber = Convert.ToInt32(0);
                                objprodct.updateSingleImportProduct();
                            }
                            else
                            {
                                lblmsg.Visible = true;
                                lblmsgs.Text = "SKU dose not exist";
                            }
                        }

                        //gvAdmin.EditIndex = -1;
                        //BindContents();

                    }
                    catch (Exception ex)
                    {
                        // throw ex;
                    }
                    finally { objprodct = null; dtpro = null; }
                }
            }
        }

        BindContents();
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
        //                // throw ex;
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

    #region Check Validate All Data
    private bool checkValidDatatableDataInventory(string sku, string inventory)
    {

        bool isInsertRow = false;
        error_code = "";

        #region sku
        if (CommonFunctions.IsValidValue(sku, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            error_code += "Invalid Sku";
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
}