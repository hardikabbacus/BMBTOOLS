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

public partial class Admin_productImageUpload : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(AppSettings.PAGESIZE);
    string id = "";
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Manage Image List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
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

            BingpageSize();

            if (Request.QueryString["flag"] == "add")
            {
                //trmsg.Visible = true;
                lblmsg.Visible = true;
                lblmsgs.Text = "Product image has been added successfully";
            }
            else if (Request.QueryString["flag"] == "edit")
            {
                //trmsg.Visible = true;
                lblmsg.Visible = true;
                lblmsgs.Text = "Product image has been updated successfully";
            }
            else if (Request.QueryString["flag"] == "delete")
            {
                //trmsg.Visible = true;
                lblmsg.Visible = true;
                lblmsgs.Text = "Product image has been deleted successfully";
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
                pageSize = AppSettings.PAGESIZE;
                ddlpageSize.SelectedValue = AppSettings.PAGESIZE.ToString();
            }
            BindProduct();

        }
    }

    //Bind product
    private void BindProduct(string search = "")
    {
        this.Form.DefaultButton = imgbtnSearch.UniqueID;

        productManager objproduct = new productManager();
        DataTable dtadmin = new DataTable();
        try
        {
            if (txtsearch.Text != "")
            {
                objproduct.sku = txtsearch.Text.Trim();
            }
            else
            {
                objproduct.sku = txtsearch.Text.Trim();
            }

            if (pageNo == 0) { pageNo = 1; }
            objproduct.pageNo = pageNo;
            objproduct.pageSize = pageSize;
            objproduct.SortExpression = SortExpression;
            querystring = "&pageSize=" + ddlpageSize.SelectedValue + "&key=" + txtsearch.Text;
            dtadmin = objproduct.SearchProductImageItem();
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
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "productImageUpload.aspx");
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
        pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
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




    //handle grid view page chenging
    protected void gvAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAdmin.PageIndex = e.NewPageIndex;
        BindProduct();
        //ddlpage.SelectedIndex = e.NewPageIndex;
    }

    //handle delete event
    protected void btnDelete_Click(object sender, EventArgs e)
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
                    objproduct.productImagesId = Convert.ToInt32(gvAdmin.DataKeys[gvAdmin.Rows[i].RowIndex].Value.ToString());

                    string dtimgs = objproduct.getProductImageName();
                    if (dtimgs != "")
                    {
                        string fullimagepath = string.Empty;
                        string thumbimagepath = string.Empty;
                        string mediumimagepath = string.Empty;
                        string thumbrectimagepath = string.Empty;

                        fullimagepath = Server.MapPath(AppSettings.PRODUCT_ACTULE_ROOTURL + dtimgs);
                        thumbimagepath = Server.MapPath(AppSettings.PRODUCT_THUMB_ROOTURL + dtimgs);
                        mediumimagepath = Server.MapPath(AppSettings.PRODUCT_MEDIUM_ROOTURL + dtimgs);
                        thumbrectimagepath = Server.MapPath(AppSettings.PRODUCT_THUMBRECT_ROOTURL + dtimgs);

                        CommonFunctions.DeleteFile(fullimagepath);
                        CommonFunctions.DeleteFile(thumbimagepath);
                        CommonFunctions.DeleteFile(mediumimagepath);
                        CommonFunctions.DeleteFile(thumbrectimagepath);
                    }

                    objproduct.DeleteProductImageByProductImageId();

                }
            }
            Response.Redirect("ProductImageUpload.aspx?flag=delete&key=" + txtsearch.Text + "  &pageSize=" + ddlpageSize.SelectedValue + "");

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

        // bind drop donwlist

        if (e.Row.RowType == DataControlRowType.DataRow && gvAdmin.EditIndex == e.Row.RowIndex)
        {
            DropDownList ddlCities = (DropDownList)e.Row.FindControl("ddlsku");
            Label lblskus = (Label)e.Row.FindControl("lblCity");

            productManager objprod = new productManager();
            DataTable dt = new DataTable();
            dt = objprod.BindAllSkuDDL();
            try
            {
                ddlCities.DataSource = dt;
                ddlCities.DataTextField = "sku";
                ddlCities.DataValueField = "productid";
                ddlCities.DataBind();
                if (lblskus.Text == "")
                { ddlCities.Items.Insert(0, new ListItem("search", "0")); }
                else
                { ddlCities.Items.FindByText(lblskus.Text).Selected = true; }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { dt = null; }
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

    protected void gvAdmin_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvAdmin.EditIndex = e.NewEditIndex;
        BindProduct("");
    }

    protected void gvAdmin_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string productImagesId = gvAdmin.DataKeys[e.RowIndex].Values["productImagesId"].ToString();
        //TextBox inventory = (TextBox)gvAdmin.Rows[e.RowIndex].FindControl("txtInventory");
        string productid = (gvAdmin.Rows[e.RowIndex].FindControl("ddlsku") as DropDownList).SelectedItem.Value;
        string sku = (gvAdmin.Rows[e.RowIndex].FindControl("ddlsku") as DropDownList).SelectedItem.Text;
        string ImgLabl = (gvAdmin.Rows[e.RowIndex].FindControl("txtImgLabl") as TextBox).Text;
        HiddenField hidimage = (HiddenField)gvAdmin.Rows[e.RowIndex].FindControl("hdmenuimage");

        productManager objprodct = new productManager();
        try
        {
            char[] imgArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            string filname = string.Empty;

            string chkFine = hidimage.Value;
            var lastChar = chkFine[chkFine.Length - 1];

            string[] strimg = hidimage.Value.Split('.');
            string strthumb = Server.MapPath("../resources/product/thumb/") + hidimage.Value.ToString();
            string strtActual = Server.MapPath("../resources/product/full/") + hidimage.Value.ToString();
            string strtMedium = Server.MapPath("../resources/product/medium/") + hidimage.Value.ToString();

            string newThumb = string.Empty;
            string newActual = string.Empty;
            string newMedium = string.Empty;

            string TemnewThumb = string.Empty;
            string TempnewActual = string.Empty;
            string TempnewMedium = string.Empty;

            for (int fl = 0; fl <= imgArray.Length; fl++)
            {
                newThumb = Server.MapPath("../resources/product/thumb/") + sku + imgArray[fl] + "." + strimg[1];
                newActual = Server.MapPath("../resources/product/full/") + sku + imgArray[fl] + "." + strimg[1];
                newMedium = Server.MapPath("../resources/product/medium/") + sku + imgArray[fl] + "." + strimg[1];

                if (Convert.ToString(hidimage.Value) == Convert.ToString(sku + "." + strimg[1]))
                {
                    TemnewThumb = Server.MapPath("../resources/product/TempThumb/") + sku + "." + strimg[1];
                    TempnewActual = Server.MapPath("../resources/product/TempFull/") + sku + "." + strimg[1];
                    TempnewMedium = Server.MapPath("../resources/product/TempMedium/") + sku + "." + strimg[1];

                    newThumb = Server.MapPath("../resources/product/thumb/") + sku + "." + strimg[1];
                    newActual = Server.MapPath("../resources/product/full/") + sku + "." + strimg[1];
                    newMedium = Server.MapPath("../resources/product/medium/") + sku + "." + strimg[1];

                    File.Copy(strthumb, TemnewThumb);
                    File.Copy(strtActual, TempnewActual);
                    File.Copy(strtMedium, TempnewMedium);

                    ////delete file
                    File.Delete(strthumb);
                    File.Delete(strtActual);
                    File.Delete(strtMedium);

                    filname = sku + "." + strimg[1];
                    File.Copy(TemnewThumb, newThumb);
                    File.Copy(TempnewActual, newActual);
                    File.Copy(TempnewMedium, newMedium);

                    ////delete file
                    File.Delete(TemnewThumb);
                    File.Delete(TempnewActual);
                    File.Delete(TempnewMedium);
                    break;
                }

                if (!File.Exists(newThumb))
                {
                    filname = sku + imgArray[fl] + "." + strimg[1];
                    File.Copy(strthumb, newThumb);
                    File.Copy(strtActual, newActual);
                    File.Copy(strtMedium, newMedium);

                    ////delete file
                    File.Delete(strthumb);
                    File.Delete(strtActual);
                    File.Delete(strtMedium);

                    break;
                }
                if (Convert.ToString(hidimage.Value) == Convert.ToString(sku + imgArray[fl] + "." + strimg[1]))
                {
                    TemnewThumb = Server.MapPath("../resources/product/TempThumb/") + sku + imgArray[fl] + "." + strimg[1];
                    TempnewActual = Server.MapPath("../resources/product/TempFull/") + sku + imgArray[fl] + "." + strimg[1];
                    TempnewMedium = Server.MapPath("../resources/product/TempMedium/") + sku + imgArray[fl] + "." + strimg[1];

                    File.Copy(strthumb, TemnewThumb);
                    File.Copy(strtActual, TempnewActual);
                    File.Copy(strtMedium, TempnewMedium);

                    ////delete file
                    File.Delete(strthumb);
                    File.Delete(strtActual);
                    File.Delete(strtMedium);

                    filname = sku + imgArray[fl] + "." + strimg[1];
                    File.Copy(TemnewThumb, newThumb);
                    File.Copy(TempnewActual, newActual);
                    File.Copy(TempnewMedium, newMedium);

                    ////delete file
                    File.Delete(TemnewThumb);
                    File.Delete(TempnewActual);
                    File.Delete(TempnewMedium);
                    break;
                }
            }

            objprodct.imageName = filname;
            objprodct.productImagesId = Convert.ToInt32(productImagesId);
            objprodct.productId = Convert.ToInt32(productid); // ddl set
            string pr_name = objprodct.GetProductNameByProductid();

            //if (TxtImgLbl != "")
            //{ objprodct.imgLabel = TxtImgLbl; }
            //else
            //{ objprodct.imgLabel = filname; }
            objprodct.imgLabel = Server.HtmlEncode(pr_name);

            objprodct.UpdateProductID();

            //delete file
            //File.Delete(strPath);

            gvAdmin.EditIndex = -1;
            BindProduct();
            lblmsg.Visible = true;
            lblmsgs.Text = "Product image updated successfully.";

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objprodct = null; }

    }
    protected void gvAdmin_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvAdmin.EditIndex = -1;
        BindProduct();
    }

    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        BindProduct("search");
    }
    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            ddlpageSize.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    protected void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        productManager objprodelete = new productManager();
        ImageButton lnkRemove = (ImageButton)sender;
        objprodelete.productImagesId = Convert.ToInt32(lnkRemove.CommandArgument);

        string dtimgs = objprodelete.getProductImageName();
        if (dtimgs != "")
        {
            string fullimagepath = string.Empty;
            string thumbimagepath = string.Empty;
            string mediumimagepath = string.Empty;
            string thumbrectimagepath = string.Empty;

            fullimagepath = Server.MapPath(AppSettings.PRODUCT_ACTULE_ROOTURL + dtimgs);
            thumbimagepath = Server.MapPath(AppSettings.PRODUCT_THUMB_ROOTURL + dtimgs);
            mediumimagepath = Server.MapPath(AppSettings.PRODUCT_MEDIUM_ROOTURL + dtimgs);
            thumbrectimagepath = Server.MapPath(AppSettings.PRODUCT_THUMBRECT_ROOTURL + dtimgs);

            CommonFunctions.DeleteFile(fullimagepath);
            CommonFunctions.DeleteFile(thumbimagepath);
            CommonFunctions.DeleteFile(mediumimagepath);
            CommonFunctions.DeleteFile(thumbrectimagepath);
        }

        objprodelete.DeleteProductImageByProductImageId();
        BindProduct();
        lblmsg.Visible = true;
        lblmsgs.Text = "Product image has been deleted successfully";

    }

    #region Upload images

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        productManager objProduct = new productManager();
        ////insert image product
        HttpFileCollection uploadedFiles = Request.Files;
        for (int im = 0; im < uploadedFiles.Count; im++)
        {
            HttpPostedFile userPostedFile = uploadedFiles[im];
            string name = userPostedFile.FileName;
            if (name != "null" && name != "")
            {
                string extension = Path.GetExtension(userPostedFile.FileName).ToString();
                string name2 = name.Replace(extension, "");
                string nameBox = Request.Form["newr" + name2];
                if (nameBox == null)
                {
                    objProduct.productId = 0;
                    objProduct.isactive = 1;
                    objProduct.InsertProductImageItem();

                    int prodimageID = objProduct.GetmaximageProductId();
                    objProduct.productImagesId = prodimageID;
                    objProduct.actualImageName = userPostedFile.FileName;
                    objProduct.imageName = UploadImage(userPostedFile, prodimageID);
                    objProduct.imgLabel = objProduct.imageName;
                    objProduct.UpdateImage();
                }
                else
                { }
            }
        }
        BindProduct();
    }

    protected string UploadImage(HttpPostedFile fileObject, int maxID)
    {
        string actualfolder = string.Empty;
        string thumbfolder = string.Empty;
        string midiumfolder = string.Empty;

        actualfolder = Server.MapPath("../" + AppSettings.PRODUCT_ACTULE_ROOTURL);
        thumbfolder = Server.MapPath("../" + AppSettings.PRODUCT_THUMB_ROOTURL);
        midiumfolder = Server.MapPath("../" + AppSettings.PRODUCT_MEDIUM_ROOTURL);

        DirectoryInfo actDir = new DirectoryInfo(actualfolder);
        DirectoryInfo thumbDir = new DirectoryInfo(thumbfolder);
        DirectoryInfo midiumDir = new DirectoryInfo(midiumfolder);


        //check if Directory exist if not create it
        if (!actDir.Exists) { Directory.CreateDirectory(actualfolder); }

        //check if Directory exist if not create it
        if (!thumbDir.Exists) { Directory.CreateDirectory(thumbfolder); }

        if (!midiumDir.Exists) { Directory.CreateDirectory(midiumfolder); }


        string filename = string.Empty;
        string fullimagepath = string.Empty;
        string thumbimagepath = string.Empty;
        string midiumimagepath = string.Empty;


        //filename = maxID + Path.GetExtension(Path.GetFileName(imgupload.PostedFile.FileName));
        filename = maxID + Path.GetExtension(Path.GetFileName(fileObject.FileName));

        fullimagepath = actualfolder + filename;
        thumbimagepath = thumbfolder;
        midiumimagepath = midiumfolder;

        //delete old files if Exists
        CommonFunctions.DeleteFile(fullimagepath);

        //delete old files if Exists
        CommonFunctions.DeleteFile(thumbimagepath);

        //delete old files if Exists
        CommonFunctions.DeleteFile(midiumimagepath);

        //save original image 
        //imgupload.PostedFile.SaveAs(fullimagepath);
        fileObject.SaveAs(fullimagepath);

        //generate thumb

        CommonFunctions.Thmbimages(fullimagepath, thumbfolder, filename, Convert.ToInt32(AppSettings.PRODUCT_THUMB_WIDTH), Convert.ToInt32(AppSettings.PRODUCT_THUMB_HEIGHT), 0);
        CommonFunctions.Thmbimages(fullimagepath, midiumfolder, filename, Convert.ToInt32(AppSettings.PRODUCT_MEDIUM_WIDTH), Convert.ToInt32(AppSettings.PRODUCT_MEDIUM_HEIGHT), 0);

        return filename;


    }

    #endregion

}