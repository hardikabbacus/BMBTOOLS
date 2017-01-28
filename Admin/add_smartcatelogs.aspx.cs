using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_add_smartcatelogs : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CatelogsProductCategoryPageSize"]);
    string id = "";
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Smart Catalogs - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        //ltrheading.Text = "Add/Modify Smart Catalogs";

        if (!IsPostBack)
        {
            #region SET PAGE NUMBER

            if (Request.QueryString["p"] == null) { pageNo = 1; }
            else if (Request.QueryString["p"] == "") { pageNo = 1; }
            else if (Convert.ToInt32(Request.QueryString["p"]) <= 0) { pageNo = 1; }
            else { pageNo = Convert.ToInt32(Request.QueryString["p"]); }
            #endregion
            BingpageSize();

            BindBrand();
            BindCategoryList();

            if (Request.QueryString["flag"] == "add")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "Smart catalogs added successfully.";
            }
            if (Request.QueryString["p"] != "")
            {
                if (Convert.ToString(Session["catskuType"]) == "S")
                {
                    bindproductscategory(Convert.ToString(Session["catskuType"]), Convert.ToString(Session["CatSku"]), "");
                }
                if (Convert.ToString(Session["catskuType"]) == "C")
                {
                    bindproductscategory(Convert.ToString(Session["catskuType"]), "", Convert.ToString(Session["CatSku"]));
                }
            }
            else
            {
                Session["catskuType"] = "";
                Session["CatSku"] = "";
            }
            if (CommonFunctions.IsQueryString("pageSize", true))
            {
                ddlpageSize.SelectedValue = Request.QueryString["pageSize"];
                pageSize = Convert.ToInt32(Request.QueryString["pageSize"]);
            }
            else
            {
                pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CatelogsProductCategoryPageSize"]);
                ddlpageSize.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["CatelogsProductCategoryPageSize"].ToString();
            }
        }
    }

    #region --- BIND CATEGORY ---

    public void BindCategoryList()
    {
        categoryManager objCategory = new categoryManager();
        DataTable dt = new DataTable();
        try
        {
            dt = objCategory.GetAllCategory();
            if (dt.Rows.Count > 0)
            {
                lstCategory.DataSource = dt;
                lstCategory.DataTextField = "categoryName";
                lstCategory.DataValueField = "categoryId";
                lstCategory.DataBind();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objCategory = null; }


    }

    #endregion

    #region --- BIND BRAND ---

    public void BindBrand()
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
                    System.Web.UI.WebControls.ListItem lisub = new System.Web.UI.WebControls.ListItem(dt.Rows[i]["brandName"].ToString(), dt.Rows[i]["brandId"].ToString());
                    lisub.Attributes.Add("style", "color:#6dace5");
                    ddlcatalogsBrand.Items.Add(lisub);
                }
            }
        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

    #endregion

    #region --- Btn Generatre Click ----

    protected void btnGeneratecatalogs_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            smartCatelogsManager objSmartCatalogs;
            catalogsOptionManager objCatalogOption;
            catelogsProductManager objCatalogProduct;
            productManager objProduct;
            int SmartCatalogsID = 0;
            try
            {
                #region  -- ADD IN SMART_CATALOGS

                objSmartCatalogs = new smartCatelogsManager();
                objSmartCatalogs.catelogName = Server.HtmlEncode(txtcatalogname.Text.Trim());
                objSmartCatalogs.prepareFor = Server.HtmlDecode(txtpreparedfor.Text.Trim());
                objSmartCatalogs.companyLogo = "";
                objSmartCatalogs.brandId = 0;

                if (Request.QueryString["flag"] == "edit")
                { }
                else
                {
                    SmartCatalogsID = Convert.ToInt32(objSmartCatalogs.InsertCatelogs());
                    objSmartCatalogs.smartCatelogId = SmartCatalogsID;
                    if (uploadlogo.HasFile)
                    { objSmartCatalogs.companyLogo = UploadImage(SmartCatalogsID); }
                    else if (hidlogo.Value != null)
                    { objSmartCatalogs.companyLogo = hidlogo.Value; }
                    else
                    { objSmartCatalogs.companyLogo = ""; }

                    objSmartCatalogs.UpdateCatelogsImages();
                }

                #endregion

                #region -- ADD INTO CATALOGS OPTIONS

                objCatalogOption = new catalogsOptionManager();

                if (hidbrandid.Value != "0" && hidbrandid.Value != null && hidbrandid.Value != "")
                {
                    objCatalogOption.DeleteOption();

                    string BrandId = hidbrandid.Value;
                    string[] LeftBrandId = BrandId.Split(',');
                    for (int lc = 0; lc < LeftBrandId.Count(); lc++)
                    {
                        objCatalogOption.smartCatelogId = Convert.ToInt32(SmartCatalogsID);
                        objCatalogOption.brandid = Convert.ToInt32(LeftBrandId[lc]);
                        objCatalogOption.pricelevel = Convert.ToInt32(ddlpricelavel.SelectedValue);
                        objCatalogOption.priceRange = Convert.ToChar(ddlpricerange.SelectedValue);
                        objCatalogOption.ranges = (txtpricerange.Text != "") ? Convert.ToInt32(txtpricerange.Text.ToString()) : Convert.ToInt32(0);
                        objCatalogOption.onlyProductwithPhoto = Convert.ToBoolean(chkProductPhoto.Checked);
                        objCatalogOption.InsertOption();
                    }
                }

                #endregion

                #region -- ADD INTO CATALOGS PRODUCT
                objCatalogProduct = new catelogsProductManager();
                if (rbtCategory.Checked == true)
                {
                    for (int i = lstDropCatSku.Items.Count - 1; i >= 0; i--)
                    {
                        objCatalogProduct.smartCatelogId = SmartCatalogsID;
                        objCatalogProduct.productId = Convert.ToInt32(lstDropCatSku.Items[i].Value);
                        objCatalogProduct.sortOrder = i + 1;
                        objCatalogProduct.types = Convert.ToChar("C");
                        objCatalogProduct.InsertCatelogsProduct();
                    }
                }
                else if (rbtsku.Checked == true)
                {
                    objProduct = new productManager();
                    string strSKU = txtSKuList.Text.Trim().Replace("\n", "");
                    var strList = strSKU.Split('\r');
                    if (strList.Length > 0)
                    {
                        for (int j = 0; j < strList.Length; j++)
                        {
                            objProduct.sku = strList[j];
                            int productid = Convert.ToInt32(objProduct.GetSkuCount());

                            objCatalogProduct.smartCatelogId = SmartCatalogsID;
                            objCatalogProduct.productId = Convert.ToInt32(productid);
                            objCatalogProduct.sortOrder = j + 1;
                            objCatalogProduct.types = Convert.ToChar("S");
                            objCatalogProduct.InsertCatelogsProduct();
                        }
                    }
                }

                #endregion

                Session["catskuType"] = "";
                Session["CatSku"] = "";

                Response.Redirect("add_smartcatelogs.aspx?flag=add");

                //GetTestPDF();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { objCatalogOption = null; objCatalogProduct = null; objSmartCatalogs = null; objProduct = null; }
        }
    }

    #endregion

    #region --- Btn AddtoArrange Click ----

    // CATEGOR ARRAGEMENT
    protected void btnAddtoArrange_Click(object sender, EventArgs e)
    {
        Session["catskuType"] = "";
        Session["CatSku"] = "";
        string Cat_id = string.Empty;
        int count = 0;
        string Str = string.Empty;

        for (int i = lstCategory.Items.Count - 1; i >= 0; i--)
        {

            if (lstCategory.Items[i].Selected)
            {
                Str += "<li class='ui-state-default'>";
                lstDropCatSku.Items.Add(lstCategory.Items[i]);
                //lstDropCatSku.Items.Remove(lstCategory.Items[i]);
                if (count == 0)
                {
                    Cat_id += lstCategory.Items[i].Value;
                    Str += lstCategory.Items[i].Value;
                    count++;
                }
                else
                {
                    Cat_id += "," + lstCategory.Items[i].Value;
                    Str += lstCategory.Items[i].Value;
                    count++;
                }
                Str += "</li>";
            }
        }
        ltrCat.Text = Str;

        Session["catskuType"] = "C";
        Session["CatSku"] = Cat_id;

        //bindproductscategory("C", "", Cat_id);
        bindproductscategory(Convert.ToString(Session["catskuType"]), "", Convert.ToString(Session["CatSku"]));
    }

    //SKU ARRAGEMENT
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        string strSKU = txtSKuList.Text.Trim().Replace("\n", "");
        var strList = strSKU.Split('\r');

        if (strList.Length > 0)
        {
            string SkuList = string.Empty;
            for (int j = 0; j < strList.Length; j++)
            {
                if (j == 0)
                {
                    SkuList += "('" + Convert.ToString(strList[j]) + "'";
                }
                else
                {
                    SkuList += ",'" + Convert.ToString(strList[j]) + "'";
                }
            }
            SkuList += ")";

            Session["catskuType"] = "S";
            Session["CatSku"] = SkuList;

            //bindproductscategory("S", SkuList, "");
            bindproductscategory(Convert.ToString(Session["catskuType"]), Convert.ToString(Session["CatSku"]), "");
        }

    }

    #endregion


    #region --- Upload Images ---

    protected string UploadImage(int maxID)
    {
        string actualfolder = string.Empty;
        string thumbfolder = string.Empty;

        actualfolder = Server.MapPath("../" + AppSettings.COMPANYLOGO_ACTULE_ROOTURL);
        thumbfolder = Server.MapPath("../" + AppSettings.COMPANYLOGO_THUMB_ROOTURL);

        DirectoryInfo actDir = new DirectoryInfo(actualfolder);
        DirectoryInfo thumbDir = new DirectoryInfo(thumbfolder);

        //check if Directory exist if not create it
        if (!actDir.Exists) { Directory.CreateDirectory(actualfolder); }

        //check if Directory exist if not create it
        if (!thumbDir.Exists) { Directory.CreateDirectory(thumbfolder); }

        string filename = string.Empty;
        string fullimagepath = string.Empty;
        string thumbimagepath = string.Empty;

        filename = maxID + Path.GetExtension(Path.GetFileName(uploadlogo.PostedFile.FileName));

        fullimagepath = actualfolder + filename;
        thumbimagepath = thumbfolder;

        //delete old files if Exists
        CommonFunctions.DeleteFile(fullimagepath);
        //delete old files if Exists
        CommonFunctions.DeleteFile(thumbimagepath);

        //save original image
        uploadlogo.PostedFile.SaveAs(fullimagepath);

        //generate thumb
        //CommonFunctions.Thmbimages(fullimagepath, smallfolder, filename, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ALBUM_SMALL_WIDTH"].ToString()), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ALBUM_SMALL_HEIGHT"].ToString()), 0);
        CommonFunctions.Thmbimages(fullimagepath, thumbfolder, filename, Convert.ToInt32(AppSettings.COMPANYLOGO_THUMB_WIDTH), Convert.ToInt32(AppSettings.COMPANYLOGO_THUMB_HEIGHT), 0);

        return filename;

    }

    #endregion

    #region --- BIND PRODUCT AS PER RADIOBUTTON SELECTION

    public void bindproductscategory(string types, string idSku, string isCategory)
    {
        catelogsProductManager objproduct = new catelogsProductManager();
        DataTable dtadmin = new DataTable();
        try
        {
            objproduct.types = Convert.ToChar(types);
            objproduct.categoryid = isCategory;
            objproduct.skuList = idSku;

            if (pageNo == 0) { pageNo = 1; }
            objproduct.pageNo = pageNo;
            objproduct.pageSize = pageSize;
            objproduct.SortExpression = SortExpression;
            querystring = "&pageSize=" + ddlpageSize.SelectedValue;
            dtadmin = objproduct.SearchItem();
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
            gvcatelogs.DataSource = dtadmin;
            gvcatelogs.DataBind();

            if (dtadmin.Rows.Count > 0)
            {
                int startRowOnPage = (gvcatelogs.PageIndex * gvcatelogs.PageSize) + 1;
                int lastRowOnPage = startRowOnPage + gvcatelogs.Rows.Count - 1;
                int totalRows = totalrecs;
                ltrcountrecord.Text = "<div class=\"countdiv\">Showing " + startRowOnPage.ToString() + " to " + lastRowOnPage + " of " + totalRows + " entries</div>";
            }
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "add_smartcatelogs.aspx");
            ltrpaggingbottom.Text = strpaging;
            //Ltrup.Text = strpaging;
            //LoadDropDownList();
        }
        catch (Exception ex) { throw ex; }
        finally { dtadmin.Dispose(); objproduct = null; }
    }

    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        if (Convert.ToString(Session["catskuType"]) == "S")
        {
            bindproductscategory(Convert.ToString(Session["catskuType"]), Convert.ToString(Session["CatSku"]), "");
        }
        if (Convert.ToString(Session["catskuType"]) == "C")
        {
            bindproductscategory(Convert.ToString(Session["catskuType"]), "", Convert.ToString(Session["CatSku"]));
        }
    }
    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            ddlpageSize.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
        }
    }

    //handle grid view page chenging
    protected void gvcatelogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvcatelogs.PageIndex = e.NewPageIndex;
        if (Convert.ToString(Session["catskuType"]) == "S")
        {
            bindproductscategory(Convert.ToString(Session["catskuType"]), Convert.ToString(Session["CatSku"]), "");
        }
        if (Convert.ToString(Session["catskuType"]) == "C")
        {
            bindproductscategory(Convert.ToString(Session["catskuType"]), "", Convert.ToString(Session["CatSku"]));
        }

        //ddlpage.SelectedIndex = e.NewPageIndex;
    }

    //handle row data bound
    protected void gvcatelogs_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gvcatelogs_Sorting(object sender, GridViewSortEventArgs e)
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
            if (Convert.ToString(Session["catskuType"]) == "S")
            {
                bindproductscategory(Convert.ToString(Session["catskuType"]), Convert.ToString(Session["CatSku"]), "");
            }
            if (Convert.ToString(Session["catskuType"]) == "C")
            {
                bindproductscategory(Convert.ToString(Session["catskuType"]), "", Convert.ToString(Session["CatSku"]));
            }
        }
    }

    private int GetSortColumnIndex()
    {
        // Iterate through the Columns collection to determine the index of the column being sorted.
        foreach (DataControlField field in gvcatelogs.Columns)
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
                    return gvcatelogs.Columns.IndexOf(field);
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

        if (Convert.ToString(gvcatelogs.SortDirection) == sortdirec)
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

    protected void ButtonRemove_Click(object sender, EventArgs e)
    {
        //Determine the RowIndex of the Row whose Button was clicked.
        int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
        gvcatelogs.Rows[rowIndex].Visible = false;
        //Get the value of column from the DataKeys using the RowIndex.
        //int id = Convert.ToInt32(gvcatelogs.DataKeys[rowIndex].Values[0]);
    }

    #endregion

    #region --- Preview Button ---

    protected void btnpreviewcataloga_Click(object sender, EventArgs e)
    {
        GetTestPDF();
    }

    #endregion


    #region  --- GENERATE PDF ---

    private void GetTestPDF()
    {
        string filename_temp = string.Empty;
        var filename = "";
        productManager objPress = new productManager();
        DataTable dt = new DataTable();
        try
        {
            dt = objPress.GetAllTheProducts();

            if (string.IsNullOrEmpty(filename)) { filename = DateTime.Now.ToString("_mmddyyyy_HHmmss") + ".pdf"; }
            string strPath = Server.MapPath("~") + "/Resources/PDF/";
            if (!Directory.Exists(strPath)) { Directory.CreateDirectory(strPath); }
            filename_temp = strPath + DateTime.Now.Ticks.ToString() + ".pdf";

            //iTextSharp.text.Document document = new iTextSharp.text.Document(new RectangleReadOnly(0f, 0f), 10f, 10f, 100f, 100f);

            //Font boldFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
            //Font ProductHead = new Font(Font.FontFamily.HELVETICA, 14, Font.NORMAL);

            Font boldFont = new Font(FontFactory.GetFont("FiraSans-Light", 10, Font.BOLD));
            Font ProductHead = new Font(FontFactory.GetFont("FiraSans-Light", 14, Font.NORMAL));
            Font ContentFont = new Font(FontFactory.GetFont("FiraSans-Light", 10, Font.NORMAL));
            Font FntColor = new Font(FontFactory.GetFont("FiraSans-Light", 10, BaseColor.WHITE));

            //Font FntColor = new Font(FontFactory.GetFont("HELVETICA", 13, BaseColor.WHITE);

            Rectangle pageSize = new Rectangle(PageSize.A4);
            pageSize.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EFF0F1"));
            Document document = new Document(pageSize, 10, 10, 100, 100);
            //document.SetPageSize(iTextSharp.text.PageSize.A4);

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename_temp, FileMode.Create));
            //PdfWriter.GetInstance(document, new FileStream(filename_temp, FileMode.Create));

            writer.PageEvent = new ITextEvents();

            document.Open();

            PdfPTable maintable = new PdfPTable(1);
            maintable.DefaultCell.Border = Rectangle.NO_BORDER;
            float[] Mainwidths = new float[] { 580f };
            maintable.SetWidthPercentage(Mainwidths, pageSize);

            PdfPTable table = new PdfPTable(3);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            float[] widths = new float[] { 10f, 35f, 70f };
            table.SetWidths(widths);
            table.DefaultCell.Padding = 5f;

            #region master product images and QR code

            PdfPCell tablecell1 = new PdfPCell();

            tablecell1 = new PdfPCell(new Phrase("Master Product Name", ProductHead));
            tablecell1.Colspan = 3;
            tablecell1.PaddingBottom = 5f;
            tablecell1.PaddingTop = 5f;
            tablecell1.Border = 0;
            table.AddCell(tablecell1);

            // QR Code

            BaseColor bcolorImg = new BaseColor(255, 255, 255);

            iTextSharp.text.Image t1image = null;
            using (FileStream fs = new FileStream(Server.MapPath("~") + "/resources/PDFTEST/QR.jpg", FileMode.Open))
            {
                t1image = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Jpeg);
            }

            t1image.ScaleToFit(50, 50);
            t1image.Alignment = Element.ALIGN_CENTER;
            tablecell1 = new PdfPCell(t1image);
            tablecell1.HorizontalAlignment = Element.ALIGN_LEFT;
            tablecell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablecell1.PaddingLeft = 10;
            tablecell1.FixedHeight = 150f;
            tablecell1.Border = 0;
            tablecell1.BorderColorLeft = BaseColor.LIGHT_GRAY;
            tablecell1.BorderWidthLeft = .5f;
            tablecell1.BorderColorTop = BaseColor.LIGHT_GRAY;
            tablecell1.BorderWidthTop = .5f;
            tablecell1.BorderColorBottom = BaseColor.LIGHT_GRAY;
            tablecell1.BorderWidthBottom = .5f;
            tablecell1.BackgroundColor = bcolorImg;
            table.AddCell(tablecell1);

            // Product Image

            iTextSharp.text.Image t1image1 = null;
            using (FileStream fs = new FileStream(Server.MapPath("~") + "/resources/PDFTEST/Product.jpg", FileMode.Open))
            {
                t1image1 = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Jpeg);
            }

            t1image1.ScaleToFit(80, 80);
            t1image1.Alignment = Element.ALIGN_CENTER;
            tablecell1 = new PdfPCell(t1image1);
            tablecell1.PaddingLeft = 51;
            tablecell1.HorizontalAlignment = Element.ALIGN_LEFT;
            tablecell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablecell1.PaddingRight = 31;
            tablecell1.Border = 0;
            tablecell1.BorderColorRight = BaseColor.LIGHT_GRAY;
            tablecell1.BorderWidthRight = .5f;
            tablecell1.BorderColorTop = BaseColor.LIGHT_GRAY;
            tablecell1.BorderWidthTop = .5f;
            tablecell1.BorderColorBottom = BaseColor.LIGHT_GRAY;
            tablecell1.BorderWidthBottom = .5f;
            tablecell1.BackgroundColor = bcolorImg;
            table.AddCell(tablecell1);

            #endregion

            #region  Master products sub product details
            // master product with its product details
            BaseColor hcolor = new BaseColor(233, 82, 28);
            PdfPTable table1 = new PdfPTable(4);
            table1.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell cell1 = new PdfPCell();

            cell1 = new PdfPCell(new Phrase("Item No", FntColor));
            cell1.Border = 0;
            cell1.BackgroundColor = hcolor;
            cell1.FixedHeight = 25f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.BorderColorLeft = BaseColor.LIGHT_GRAY;
            cell1.BorderWidthLeft = .5f;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Phrase("Varient", FntColor));
            cell1.Border = 0;
            cell1.BackgroundColor = hcolor;
            cell1.FixedHeight = 25f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Phrase("Qty.", FntColor));
            cell1.Border = 0;
            cell1.BackgroundColor = hcolor;
            cell1.FixedHeight = 25f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Phrase("Price", FntColor));
            cell1.Border = 0;
            cell1.BackgroundColor = hcolor;
            cell1.FixedHeight = 25f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.BorderColorRight = BaseColor.LIGHT_GRAY;
            cell1.BorderWidthRight = .5f;
            table1.AddCell(cell1);

            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                BaseColor bcolor = new BaseColor(239, 240, 241);
                BaseColor bcolor2 = new BaseColor(255, 255, 255);

                if (count == 0 || count % 2 == 0)
                {
                    cell1 = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.BackgroundColor = bcolor2;
                    cell1.FixedHeight = 25f;
                    cell1.BorderColorLeft = BaseColor.LIGHT_GRAY;
                    cell1.BorderWidthLeft = .5f;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.BackgroundColor = bcolor2;
                    cell1.FixedHeight = 25f;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.BackgroundColor = bcolor2;
                    cell1.FixedHeight = 25f;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.BackgroundColor = bcolor2;
                    cell1.FixedHeight = 25f;
                    cell1.BorderColorRight = BaseColor.LIGHT_GRAY;
                    cell1.BorderWidthRight = .5f;
                    table1.AddCell(cell1);
                }
                else
                {
                    cell1 = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.FixedHeight = 25f;
                    cell1.BackgroundColor = bcolor;
                    cell1.BorderColorLeft = BaseColor.LIGHT_GRAY;
                    cell1.BorderWidthLeft = .5f;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.FixedHeight = 25f;
                    cell1.BackgroundColor = bcolor;

                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.FixedHeight = 25f;
                    cell1.BackgroundColor = bcolor;

                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.FixedHeight = 25f;
                    cell1.BackgroundColor = bcolor;
                    cell1.BorderColorRight = BaseColor.LIGHT_GRAY;
                    cell1.BorderWidthRight = .5f;
                    table1.AddCell(cell1);
                }
                count++;
                if (count == 5)
                {

                    cell1 = new PdfPCell(new Phrase("More Item here", ContentFont));
                    cell1.Colspan = 4;
                    cell1.Border = 0;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.FixedHeight = 25f;
                    cell1.BackgroundColor = bcolor;
                    cell1.BorderColorRight = BaseColor.LIGHT_GRAY;
                    cell1.BorderWidthRight = .5f;
                    cell1.BorderColorLeft = BaseColor.LIGHT_GRAY;
                    cell1.BorderWidthLeft = .5f;
                    cell1.BorderColorBottom = BaseColor.LIGHT_GRAY;
                    cell1.BorderWidthBottom = .5f;
                    table1.AddCell(cell1);

                    break;
                }
            }

            table.AddCell(table1);

            tablecell1 = new PdfPCell(new Phrase(""));
            tablecell1.Colspan = 3;
            //tablecell1.FixedHeight = 4f;
            tablecell1.Border = 0;
            table.AddCell(tablecell1);

            #endregion

            #region varient product
            // Verient product

            PdfPTable Maintable2 = new PdfPTable(1);
            Maintable2.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell Maincell2 = new PdfPCell();

            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table2 = new PdfPTable(3);
                table2.WidthPercentage = 100;
                table2.DefaultCell.Border = Rectangle.NO_BORDER;
                float[] Table2widths = new float[] { 20f, 35f, 70f };
                table2.SetWidths(Table2widths);

                PdfPCell cell2 = new PdfPCell();

                BaseColor Vcolor = new BaseColor(236, 240, 245);
                BaseColor bcolorVer = new BaseColor(255, 255, 255);
                BaseColor ctcolor = new BaseColor(239, 240, 241);

                cell2 = new PdfPCell(new Phrase(dr["productname"].ToString(), ProductHead));
                cell2.Border = 0;
                cell2.Colspan = 3;
                cell2.PaddingTop = 10f;
                cell2.PaddingBottom = 10f;
                cell2.PaddingLeft = 5f;
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2.BackgroundColor = bcolorVer;
                table2.AddCell(cell2);


                iTextSharp.text.Image t1image2 = null;
                using (FileStream fs = new FileStream(Server.MapPath("~") + "/resources/PDFTEST/QR.jpg", FileMode.Open))
                {
                    t1image2 = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Jpeg);
                }

                t1image2.ScaleToFit(50, 50);
                t1image2.Alignment = Element.ALIGN_CENTER;

                cell2 = new PdfPCell(t1image2);
                cell2.Border = 0;
                cell2.PaddingLeft = 10;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2.BackgroundColor = bcolorVer;
                table2.AddCell(cell2);

                iTextSharp.text.Image t1image3 = null;
                using (FileStream fs = new FileStream(Server.MapPath("~") + "/resources/PDFTEST/Product.jpg", FileMode.Open))
                {
                    t1image3 = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Jpeg);
                }

                t1image3.ScaleToFit(70, 70);
                t1image3.Alignment = Element.ALIGN_CENTER;

                cell2 = new PdfPCell(t1image3);
                cell2.Border = 0;
                cell2.BackgroundColor = bcolorVer;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                table2.AddCell(cell2);

                // child table

                PdfPTable table2Child = new PdfPTable(3);
                table2Child.DefaultCell.Border = Rectangle.NO_BORDER;
                table2Child.DefaultCell.Padding = 10;
                table2Child.WidthPercentage = 100;

                PdfPCell cell2Child = new PdfPCell();
                cell2Child = new PdfPCell(new Phrase("Item No.", boldFont));
                cell2Child.Border = 0;
                cell2Child.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2Child.BackgroundColor = ctcolor;
                table2Child.AddCell(cell2Child);

                cell2Child = new PdfPCell(new Phrase("Min Qty", boldFont));
                cell2Child.Border = 0;
                cell2Child.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2Child.BackgroundColor = ctcolor;
                table2Child.AddCell(cell2Child);

                cell2Child = new PdfPCell(new Phrase("price", boldFont));
                cell2Child.Border = 0;
                cell2Child.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2Child.BackgroundColor = ctcolor;
                table2Child.AddCell(cell2Child);

                cell2Child = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                cell2Child.Border = 0;
                cell2Child.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2Child.BackgroundColor = ctcolor;
                table2Child.AddCell(cell2Child);

                cell2Child = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                cell2Child.Border = 0;
                cell2Child.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2Child.BackgroundColor = ctcolor;
                table2Child.AddCell(cell2Child);

                cell2Child = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                cell2Child.Border = 0;
                cell2Child.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2Child.BackgroundColor = ctcolor;
                table2Child.AddCell(cell2Child);

                table2.AddCell(table2Child);

                cell2 = new PdfPCell(new Phrase(""));
                cell2.Border = 0;
                cell2.Colspan = 3;
                cell2.PaddingTop = 10f;
                cell2.PaddingBottom = 10f;
                cell2.PaddingLeft = 5f;
                cell2.FixedHeight = 25f;
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2.BackgroundColor = bcolorVer;
                table2.AddCell(cell2);

                PdfPCell cell = new PdfPCell();
                cell.AddElement(table2);
                cell.BorderWidthBottom = .5f;
                cell.BorderColorBottom = BaseColor.LIGHT_GRAY;
                cell.BorderWidthLeft = .5f;
                cell.BorderColorLeft = BaseColor.LIGHT_GRAY;
                cell.BorderWidthTop = .5f;
                cell.BorderColorTop = BaseColor.LIGHT_GRAY;
                cell.BorderWidthRight = .5f;
                cell.BorderColorRight = BaseColor.LIGHT_GRAY;
                PdfPTable t1 = new PdfPTable(1);
                t1.AddCell(cell);
                Maintable2.AddCell(t1);

                //space
                Maincell2 = new PdfPCell(new Phrase(""));
                Maincell2.Colspan = 1;
                Maincell2.Border = 0;
                Maincell2.PaddingTop = 5f;
                Maincell2.FixedHeight = 10f;
                Maintable2.AddCell(Maincell2);

            }

            #endregion

            #region Single product and next is also single product

            // single product

            PdfPTable Outertable3 = new PdfPTable(2);
            float[] Table3Outwidths = new float[] { 50f, 50f };
            Outertable3.SetWidths(Table3Outwidths);
            Outertable3.DefaultCell.Border = Rectangle.NO_BORDER;
            Outertable3.DefaultCell.Padding = 5f;
            PdfPCell Outercell3 = new PdfPCell();

            int cnt = 0;
            foreach (DataRow dr in dt.Rows)
            {
                BaseColor bcolorSing = new BaseColor(255, 255, 255);
                PdfPTable table3 = new PdfPTable(3);
                table3.WidthPercentage = 100;
                table3.DefaultCell.BackgroundColor = bcolorSing;

                float[] Table3widths = new float[] { 15f, 25f, 25f };
                table3.SetWidths(Table3widths);
                table3.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell cell3 = new PdfPCell();

                cell3 = new PdfPCell(new Phrase(Convert.ToString(dr["productname"]), ProductHead));
                cell3.Colspan = 3;
                cell3.Border = 0;
                cell3.PaddingRight = 5f;
                cell3.FixedHeight = 25f;
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.BackgroundColor = bcolorSing;
                table3.AddCell(cell3);

                iTextSharp.text.Image t1image3 = null;
                using (FileStream fs = new FileStream(Server.MapPath("~") + "/resources/PDFTEST/QR.jpg", FileMode.Open))
                {
                    t1image3 = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Jpeg);
                }
                t1image3.ScaleToFit(50, 50);
                t1image3.Alignment = Element.ALIGN_CENTER;
                cell3 = new PdfPCell(t1image3);
                cell3.PaddingLeft = 5;
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell3.BackgroundColor = bcolorSing;
                cell3.PaddingRight = 5;
                cell3.Border = 0;

                table3.AddCell(cell3);

                iTextSharp.text.Image t1image4 = null;
                using (FileStream fs = new FileStream(Server.MapPath("~") + "/resources/PDFTEST/Product.jpg", FileMode.Open))
                {
                    t1image4 = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Jpeg);
                }
                t1image4.ScaleToFit(70, 70);
                t1image4.Alignment = Element.ALIGN_CENTER;
                cell3 = new PdfPCell(t1image4);
                cell3.PaddingLeft = 5;
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell3.BackgroundColor = bcolorSing;
                //cell3.PaddingRight = 5;
                cell3.Border = 0;
                table3.AddCell(cell3);

                PdfPTable table3Child = new PdfPTable(1);
                table3Child.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell cell1Child = new PdfPCell();
                cell1Child = new PdfPCell(new Phrase("Price", boldFont));
                cell1Child.Border = 0;
                cell1Child.FixedHeight = 25f;
                cell1Child.BackgroundColor = bcolorSing;
                table3Child.AddCell(cell1Child);

                cell1Child = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                cell1Child.Border = 0;
                cell1Child.FixedHeight = 25f;
                cell1Child.BackgroundColor = bcolorSing;
                table3Child.AddCell(cell1Child);

                cell1Child = new PdfPCell(new Phrase("Min Qty", boldFont));
                cell1Child.Border = 0;
                cell1Child.FixedHeight = 25f;
                cell1Child.BackgroundColor = bcolorSing;
                table3Child.AddCell(cell1Child);

                cell1Child = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                cell1Child.Border = 0;
                cell1Child.FixedHeight = 25f;
                cell1Child.BackgroundColor = bcolorSing;
                table3Child.AddCell(cell1Child);

                cell1Child = new PdfPCell(new Phrase("Item No.", boldFont));
                cell1Child.Border = 0;
                cell1Child.FixedHeight = 25f;
                cell1Child.BackgroundColor = bcolorSing;
                table3Child.AddCell(cell1Child);

                cell1Child = new PdfPCell(new Phrase(Convert.ToString(dr["productid"]), ContentFont));
                cell1Child.Border = 0;
                cell1Child.FixedHeight = 25f;
                cell1Child.BackgroundColor = bcolorSing;
                table3Child.AddCell(cell1Child);

                table3.AddCell(table3Child);
                cell3.AddElement(table3);
                table3.AddCell(cell3);

                PdfPCell cell = new PdfPCell();
                cell.AddElement(table3);
                cell.BorderWidthBottom = .5f;
                cell.BorderColorBottom = BaseColor.LIGHT_GRAY;
                cell.BorderWidthLeft = .5f;
                cell.BorderColorLeft = BaseColor.LIGHT_GRAY;
                cell.BorderWidthTop = .5f;
                cell.BorderColorTop = BaseColor.LIGHT_GRAY;
                cell.BorderWidthRight = .5f;
                cell.BorderColorRight = BaseColor.LIGHT_GRAY;
                PdfPTable t1 = new PdfPTable(1);
                t1.AddCell(cell);
                Outertable3.AddCell(t1);

                cnt++;

                if (cnt == 2)
                {
                    // space
                    Outercell3 = new PdfPCell(new Phrase(""));
                    Outercell3.Colspan = 2;
                    Outercell3.Border = 0;
                    Outercell3.PaddingTop = 15f;
                    Outercell3.FixedHeight = 15;
                    Outertable3.AddCell(Outercell3);

                    cnt = 0;
                }
            }

            #endregion



            maintable.AddCell(table);
            maintable.AddCell(table);
            maintable.AddCell(table);
            maintable.AddCell(Maintable2);
            maintable.AddCell(Outertable3);

            // writer.PageEvent = new Footer(); // footer

            document.Add(maintable);

            document.Close();

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename_temp);
            byte[] bytes = System.IO.File.ReadAllBytes(filename_temp);
            if (fileInfo.Exists)
            {
                this.Response.Clear();
                //this.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + hdpdfname.Value + "\"");
                this.Response.AddHeader("Content-Disposition", "attachment; filename=SmartCatalogs.pdf");
                this.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                this.Response.ContentType = "application/pdf";
                this.Response.BinaryWrite(bytes);
                this.Response.Flush();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objPress = null; dt = null; }
    }

    #endregion

    public class ITextEvents : PdfPageEventHelper
    {

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;


        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);

            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            //FiraSans-Light  fontfamily

            //iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            //iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            //iTextSharp.text.Font CategoryFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 14f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);

            Font baseFontBig = new Font(FontFactory.GetFont("FiraSans-Light", 12f, Font.BOLD));
            Font baseFontNormal = new Font(FontFactory.GetFont("FiraSans-Light", 12f, Font.NORMAL));
            Font CategoryFontBig = new Font(FontFactory.GetFont("FiraSans-Light", 14, Font.NORMAL, BaseColor.WHITE));

            Phrase p1Header = new Phrase("", baseFontNormal); // sub header

            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);
            //We will have to create separate cells to include image logo and 2 separate strings

            iTextSharp.text.Image t1image3 = null;
            using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~") + "/resources/PDFTEST/bmb_logo_pdf.png", FileMode.Open))
            {
                t1image3 = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Png);
            }
            t1image3.ScaleToFit(100, 100);
            //t1image3.Alignment = Element.ALIGN_LEFT;

            string CategoryName = "Category Name";

            //Row 1
            PdfPCell pdfCell1 = new PdfPCell(t1image3);
            PdfPCell pdfCell2 = new PdfPCell(p1Header);
            PdfPCell pdfCell3 = new PdfPCell(new Phrase(CategoryName, CategoryFontBig));  // set dynamic categoryname
            pdfCell3.FixedHeight = 25f;
            pdfCell3.BackgroundColor = BaseColor.BLACK;
            pdfCell3.HorizontalAlignment = Element.ALIGN_MIDDLE;
            pdfCell3.PaddingBottom = 8f;
            pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;

            String text = "Produce by: BMB Tools      " + "Website: bmbtools.tajrapp.com      " + "Contact us: +966 11 270 9251      " + "Page ";

            //Add paging to header  uncommnet for headder pagging
            {
                //cb.BeginText();
                //cb.SetFontAndSize(bf, 12);
                //cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
                //cb.ShowText(text);
                //cb.EndText();
                //float len = bf.GetWidthPoint(text, 12);
                ////Adds "12" in Page 1 of 12
                //cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
            }
            //Add paging to footer
            {

                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(570), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(570) + len, document.PageSize.GetBottom(30));

            }
            //Row 2
            PdfPCell pdfCell4 = new PdfPCell(new Phrase("", baseFontNormal)); // sub header
            //Row 3

            //PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
            PdfPCell pdfCell5 = new PdfPCell();
            PdfPCell pdfCell6 = new PdfPCell();
            //PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));
            PdfPCell pdfCell7 = new PdfPCell();


            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;


            pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;


            pdfCell4.Colspan = 3;



            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            pdfCell4.Border = 0;
            pdfCell5.Border = 0;
            pdfCell6.Border = 0;
            pdfCell7.Border = 0;


            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            pdfTab.AddCell(pdfCell4);
            pdfTab.AddCell(pdfCell5);
            pdfTab.AddCell(pdfCell6);
            pdfTab.AddCell(pdfCell7);

            pdfTab.TotalWidth = document.PageSize.Width - 0f;
            pdfTab.WidthPercentage = 100;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;


            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //uncomment for header line
            ////Move the pointer and draw line to separate header section from rest of page
            //cb.MoveTo(40, document.PageSize.Height - 50); // 100
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 50); // 100
            //cb.Stroke();

            // uncomment for footer line
            ////Move the pointer and draw line to separate footer section from rest of page
            //cb.MoveTo(40, document.PageSize.GetBottom(50));
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            //cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();


            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();


        }
    }

}

