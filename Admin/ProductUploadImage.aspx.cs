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

public partial class Admin_ProductUploadImage : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UploadImagePageSize"]);
    string id = "";
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";
    string CheckSku = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Manage Image List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        gvAdmin.PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UploadImagePageSize"]);
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
                pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UploadImagePageSize"]);
                ddlpageSize.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["UploadImagePageSize"].ToString();
            }
            BindProduct();

        }
    }

    // page size as par select dropdown
    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            ddlpageSize.Items.Add(new ListItem(i.ToString(), i.ToString()));
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
            //objproduct.SortExpression = SortExpression;
            querystring = "&pageSize=" + ddlpageSize.SelectedValue + "&key=" + txtsearch.Text;
            dtadmin = objproduct.SearchProductImageItemTodaysOnly();
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
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "productUploadImage.aspx");
            ltrpaggingbottom.Text = strpaging;
            //Ltrup.Text = strpaging;
            //LoadDropDownList();
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
        BindProduct("search");
    }

    // gridview events
    protected void gvAdmin_DataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvAdmin.Rows)
        {
            int key = (int)gvAdmin.DataKeys[row.RowIndex].Value;
            if (row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlSkus = (DropDownList)row.FindControl("ddlsku");

                productManager objprod = new productManager();
                DataTable dt = new DataTable();
                dt = objprod.BindAllSkuDDL();
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlSkus.DataSource = dt;
                        ddlSkus.DataTextField = "sku";
                        ddlSkus.DataValueField = "productid";
                        ddlSkus.DataBind();

                        Label sku = (Label)row.FindControl("hidsku");
                        Label productid = (Label)row.FindControl("lblProductid");
                        if (sku.Text != null && sku.Text != "")
                        {
                            //ddlSkus.SelectedItem.Text = sku.Text;
                            ddlSkus.SelectedValue = productid.Text;
                        }
                        else { ddlSkus.Items.Insert(0, new ListItem(" search ", "0")); }

                        //ddlSkus.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else { ddlSkus.Items.Insert(0, new ListItem(" search ", "0")); }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { objprod = null; dt = null; }
            }
        }
    }

    protected void gvAdmin_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("UpdateSku"))
        {
            int index = Convert.ToInt32(e.CommandArgument.ToString());

            DropDownList ddlSkus = (DropDownList)gvAdmin.Rows[index].FindControl("ddlsku");
            TextBox txtImgLabl = (TextBox)gvAdmin.Rows[index].FindControl("txtImgLabl");
            HiddenField lblProdutImageId = (HiddenField)gvAdmin.Rows[index].FindControl("hidProductImageIds");
            HiddenField hidimage = (HiddenField)gvAdmin.Rows[index].FindControl("hdmenuimage");
            Label hidsku = (Label)gvAdmin.Rows[index].FindControl("hidsku");

            productManager objprodct = new productManager();
            try
            {
                char[] imgArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

                string filname = string.Empty;

                //string chkFine = hidimage.Value;
                //var lastChar = chkFine[chkFine.Length - 1];

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

                newThumb = Server.MapPath("../resources/product/thumb/") + ddlSkus.SelectedItem.Text + "." + strimg[1];
                newActual = Server.MapPath("../resources/product/full/") + ddlSkus.SelectedItem.Text + "." + strimg[1];
                newMedium = Server.MapPath("../resources/product/medium/") + ddlSkus.SelectedItem.Text + "." + strimg[1];

                if (!File.Exists(newThumb))
                {
                    filname = ddlSkus.SelectedItem.Text + "." + strimg[1];
                    File.Copy(strthumb, newThumb);
                    File.Copy(strtActual, newActual);
                    File.Copy(strtMedium, newMedium);

                    ////delete file
                    File.Delete(strthumb);
                    File.Delete(strtActual);
                    File.Delete(strtMedium);

                }
                else if (Convert.ToString(hidimage.Value) == Convert.ToString(ddlSkus.SelectedItem.Text + "." + strimg[1]))
                {
                    TemnewThumb = Server.MapPath("../resources/product/TempThumb/") + ddlSkus.SelectedItem.Text + "." + strimg[1];
                    TempnewActual = Server.MapPath("../resources/product/TempFull/") + ddlSkus.SelectedItem.Text + "." + strimg[1];
                    TempnewMedium = Server.MapPath("../resources/product/TempMedium/") + ddlSkus.SelectedItem.Text + "." + strimg[1];

                    File.Copy(strthumb, TemnewThumb);
                    File.Copy(strtActual, TempnewActual);
                    File.Copy(strtMedium, TempnewMedium);

                    ////delete file
                    File.Delete(strthumb);
                    File.Delete(strtActual);
                    File.Delete(strtMedium);

                    filname = ddlSkus.SelectedItem.Text + "." + strimg[1];
                    File.Copy(TemnewThumb, newThumb);
                    File.Copy(TempnewActual, newActual);
                    File.Copy(TempnewMedium, newMedium);

                    ////delete file
                    File.Delete(TemnewThumb);
                    File.Delete(TempnewActual);
                    File.Delete(TempnewMedium);
                }
                else
                {
                    for (int fl = 0; fl <= imgArray.Length; fl++)
                    {
                        newThumb = Server.MapPath("../resources/product/thumb/") + ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1];
                        newActual = Server.MapPath("../resources/product/full/") + ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1];
                        newMedium = Server.MapPath("../resources/product/medium/") + ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1];

                        if (!File.Exists(newThumb))
                        {
                            filname = ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1];
                            File.Copy(strthumb, newThumb);
                            File.Copy(strtActual, newActual);
                            File.Copy(strtMedium, newMedium);

                            ////delete file
                            File.Delete(strthumb);
                            File.Delete(strtActual);
                            File.Delete(strtMedium);

                            break;
                        }
                        if (Convert.ToString(hidimage.Value) == Convert.ToString(ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1]))
                        {
                            TemnewThumb = Server.MapPath("../resources/product/TempThumb/") + ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1];
                            TempnewActual = Server.MapPath("../resources/product/TempFull/") + ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1];
                            TempnewMedium = Server.MapPath("../resources/product/TempMedium/") + ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1];

                            File.Copy(strthumb, TemnewThumb);
                            File.Copy(strtActual, TempnewActual);
                            File.Copy(strtMedium, TempnewMedium);

                            ////delete file
                            File.Delete(strthumb);
                            File.Delete(strtActual);
                            File.Delete(strtMedium);

                            filname = ddlSkus.SelectedItem.Text + imgArray[fl] + "." + strimg[1];
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
                }


                objprodct.imageName = filname;
                objprodct.productImagesId = Convert.ToInt32(lblProdutImageId.Value);
                objprodct.productId = Convert.ToInt32(ddlSkus.SelectedValue); // ddl set
                string pr_name = objprodct.GetProductNameByProductid();

                //if (txtImgLabl.Text != "")
                //{ objprodct.imgLabel = txtImgLabl.Text; }
                //else { objprodct.imgLabel = filname; }
                objprodct.imgLabel = Server.HtmlEncode(pr_name);

                objprodct.UpdateProductID();

                //delete file
                //File.Delete(strPath);

                //gvAdmin.EditIndex = -1;
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

        //// bind drop donwlist

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DropDownList ddlSKUlist = (e.Row.FindControl("ddlsku") as DropDownList);

        //    productManager objprod = new productManager();
        //    DataTable dt = new DataTable();
        //    dt = objprod.BindAllSkuDDL();
        //    try
        //    {
        //        ddlSKUlist.DataSource = dt;
        //        ddlSKUlist.DataTextField = "sku";
        //        ddlSKUlist.DataValueField = "productid";
        //        ddlSKUlist.DataBind();

        //        string sku = (e.Row.FindControl("hidsku") as Label).Text;

        //        if (sku != null && sku != "")
        //        {
        //            ddlSKUlist.SelectedItem.Text = sku;
        //        }
        //        else
        //        {
        //            ddlSKUlist.Items.Insert(0, new ListItem("Select", "0"));
        //        }

        //        //if (sku == "")
        //        //{ ddlSKUlist.Items.Insert(0, new ListItem("Select", "0")); }
        //        //else
        //        //{ ddlSKUlist.Items.FindByText(sku).Selected = true; }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally { dt = null; }
        //}

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

    // Sort direction
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

    //sort expression
    private string SortExpression
    {
        get
        {
            if (ViewState["SortExpression"] == null) { ViewState["SortExpression"] = String.Empty; }
            return ViewState["SortExpression"].ToString();
        }
        set { ViewState["SortExpression"] = value; }
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

            fullimagepath = Server.MapPath("../" + AppSettings.PRODUCT_ACTULE_ROOTURL + dtimgs);
            thumbimagepath = Server.MapPath("../" + AppSettings.PRODUCT_THUMB_ROOTURL + dtimgs);
            mediumimagepath = Server.MapPath("../" + AppSettings.PRODUCT_MEDIUM_ROOTURL + dtimgs);

            CommonFunctions.DeleteFile(fullimagepath);
            CommonFunctions.DeleteFile(thumbimagepath);
            CommonFunctions.DeleteFile(mediumimagepath);

        }

        objprodelete.DeleteProductImageByProductImageId();
        BindProduct();
        lblmsg.Visible = true;
        lblmsgs.Text = "Product image has been deleted successfully";
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


                        fullimagepath = Server.MapPath("../" + AppSettings.PRODUCT_ACTULE_ROOTURL + dtimgs);
                        thumbimagepath = Server.MapPath("../" + AppSettings.PRODUCT_THUMB_ROOTURL + dtimgs);
                        mediumimagepath = Server.MapPath("../" + AppSettings.PRODUCT_MEDIUM_ROOTURL + dtimgs);


                        CommonFunctions.DeleteFile(fullimagepath);
                        CommonFunctions.DeleteFile(thumbimagepath);
                        CommonFunctions.DeleteFile(mediumimagepath);

                    }

                    objproduct.DeleteProductImageByProductImageId();

                }
            }
            Response.Redirect("ProductUploadImage.aspx?flag=delete&key=" + txtsearch.Text + "  &pageSize=" + ddlpageSize.SelectedValue + "");

        }
        catch (Exception ex) { throw ex; }
        finally { objproduct = null; }
    }

    #region Upload images

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            productManager objProduct = new productManager();
            ////insert image product
            HttpFileCollection uploadedFiles = Request.Files;
            bool flg = false;
            string strImagesNameExist = string.Empty;
            string FileNameCount = string.Empty;

            for (int im = 0; im < uploadedFiles.Count; im++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[im];
                string name = userPostedFile.FileName;
                if (name != "null" && name != "")
                {
                    bool imgFlag = false;
                    string extension = Path.GetExtension(userPostedFile.FileName).ToString();
                    string name2 = name.Replace(extension, "");
                    string nameBox = Request.Form["newr" + name2];
                    if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg")
                    {
                        if (nameBox == null)
                        {
                            #region check SKU and ImageName

                            string NameFile = name2;
                            int prodimageID;

                            string flname = name2.Substring(name2.Length - 1);
                            if (!CommonFunctions.IsValidValue(flname, true, false, false))
                            {
                                name2 = name2.TrimEnd(name2[name2.Length - 1]);
                            }
                            objProduct.sku = name2;
                            int prodId = objProduct.GetSkuCount();
                            int imgcnt = objProduct.GetproductsImageCount(prodId);

                            // for check images exist or not
                            DataTable dtImgname = new DataTable();
                            dtImgname = objProduct.GetProductImageNameByProductId(prodId);  // get the images from productid
                            if (dtImgname.Rows.Count > 0)
                            {
                                for (int ie = 0; ie < dtImgname.Rows.Count; ie++)
                                {
                                    if (Convert.ToString(dtImgname.Rows[ie]["imageName"]) == Convert.ToString(NameFile + ".jpg"))
                                    {
                                        if (strImagesNameExist == "")
                                        {
                                            strImagesNameExist += dtImgname.Rows[ie]["imageName"].ToString();
                                        }
                                        else
                                        {
                                            strImagesNameExist += "," + dtImgname.Rows[ie]["imageName"].ToString();
                                        }
                                        imgFlag = true;
                                        break;
                                    }

                                    // added on 20_12_2016
                                    if (Convert.ToString(dtImgname.Rows[ie]["imageName"]) == Convert.ToString(NameFile + ".png"))
                                    {
                                        if (strImagesNameExist == "")
                                        {
                                            strImagesNameExist += dtImgname.Rows[ie]["imageName"].ToString();
                                        }
                                        else
                                        {
                                            strImagesNameExist += "," + dtImgname.Rows[ie]["imageName"].ToString();
                                        }
                                        imgFlag = true;
                                        break;
                                    }
                                }
                            }

                            if (imgFlag == false)
                            {
                                if (strImagesNameExist == "")
                                {
                                    lblmsg.Visible = false;
                                }

                                objProduct.productId = 0;
                                objProduct.isactive = 1;
                                //objProduct.imgLabel = NameFile;
                                objProduct.InsertProductImageItem();

                                if (prodId != 0 && imgcnt != 0 && flg == false) // if product id and image count both are in the table
                                {
                                    if (imgFlag == false)  // if false then updates record other wise display message
                                    {
                                        prodimageID = objProduct.GetmaximageProductId();  // gat max productimageid
                                        objProduct.productId = prodId;
                                        objProduct.productImagesId = prodimageID;
                                        objProduct.actualImageName = userPostedFile.FileName;
                                        objProduct.imageName = UploadImageSKU(userPostedFile, name2, true);
                                        objProduct.isactive = 1;
                                        objProduct.sortOrder = 0;
                                        objProduct.imgLabel = objProduct.imageName;
                                        objProduct.UpdateProductImagetem();
                                    }
                                    else
                                    {
                                        lblmsg.Visible = true;
                                        lblmsgs.Text = strImagesNameExist + "Images are already exist.";
                                    }

                                }
                                else if (prodId != 0)  // no images avaliable in product images table
                                {
                                    flg = true;
                                    prodimageID = objProduct.GetmaximageProductId(); // gat max productimageid
                                    objProduct.productId = prodId;
                                    objProduct.productImagesId = prodimageID;
                                    objProduct.actualImageName = userPostedFile.FileName;
                                    objProduct.imageName = UploadImageSKU(userPostedFile, NameFile, false);
                                    objProduct.isactive = 1;
                                    objProduct.sortOrder = 0;
                                    string pr_name = objProduct.GetProductNameByProductid();
                                    objProduct.imgLabel = Server.HtmlEncode(pr_name);
                                    objProduct.UpdateProductImagetem();
                                }
                                else
                                {
                                    prodimageID = objProduct.GetmaximageProductId();   // gat max productimageid
                                    objProduct.productImagesId = prodimageID;
                                    objProduct.actualImageName = userPostedFile.FileName;
                                    objProduct.imageName = UploadImage(userPostedFile, prodimageID.ToString());
                                    objProduct.imgLabel = objProduct.imageName;
                                    objProduct.UpdateImage();
                                }
                            }
                            else
                            {
                                //lblmsg.Visible = true;
                                //lblmsgs.Text = strImagesNameExist + "  Images are already exist.";

                                string[] FileMaxId = name.Split('.');
                                string filesName = UploadImage(userPostedFile, FileMaxId[0].ToString());

                                if (FileNameCount == "")
                                { FileNameCount = filesName; }
                                else { FileNameCount += "," + filesName; }

                                lblmsg.Visible = true;
                                lblmsgs.Text = "image " + FileNameCount + " updates successfully.";

                            }

                            #endregion

                            //int prodimageID = objProduct.GetmaximageProductId();
                            //objProduct.productImagesId = prodimageID;
                            //objProduct.actualImageName = userPostedFile.FileName;
                            ////objProduct.imageName = UploadImage(userPostedFile, prodimageID);
                            ////objProduct.imgLabel = objProduct.imageName;
                            ////objProduct.UpdateImage();
                        }
                        else
                        { }
                    }
                }
            }
            BindProduct();
        }
    }

    protected string UploadImage(HttpPostedFile fileObject, string maxID)
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

    protected string UploadImageSKU(HttpPostedFile fileObject, string maxID, bool flg)
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

        string newThumbSku = string.Empty;

        if (flg == true)
        {
            newThumbSku = Server.MapPath("../resources/product/thumb/") + maxID + Path.GetExtension(Path.GetFileName(fileObject.FileName));
            if (!File.Exists(newThumbSku))
            {
                filename = maxID + Path.GetExtension(Path.GetFileName(fileObject.FileName));
            }
            else
            {
                char[] imgArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
                for (int fl = 0; fl <= imgArray.Length; fl++)
                {
                    string newThumb = Server.MapPath("../resources/product/thumb/") + maxID + imgArray[fl] + Path.GetExtension(Path.GetFileName(fileObject.FileName));
                    if (!File.Exists(newThumb))
                    {
                        filename = maxID + imgArray[fl] + Path.GetExtension(Path.GetFileName(fileObject.FileName));
                        break;
                    }
                }
            }
        }
        else
        {
            filename = maxID + Path.GetExtension(Path.GetFileName(fileObject.FileName));
        }

        //filename = maxID + Path.GetExtension(Path.GetFileName(imgupload.PostedFile.FileName));
        //filename = maxID + Path.GetExtension(Path.GetFileName(fileObject.FileName));

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

    protected void BtnMultiUpload_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            foreach (GridViewRow row in gvAdmin.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        string lblProdutImageId = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                        string hidimage = row.Cells[1].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                        string sku = row.Cells[2].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem.Text;
                        string TxtImgLbl = row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Text;
                        string productid = row.Cells[2].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem.Value;

                        productManager objprodct = new productManager();
                        try
                        {
                            char[] imgArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

                            string filname = string.Empty;

                            //string chkFine = hidimage.Value;
                            //var lastChar = chkFine[chkFine.Length - 1];

                            string[] strimg = hidimage.Split('.');
                            string strthumb = Server.MapPath("../resources/product/thumb/") + hidimage.ToString();
                            string strtActual = Server.MapPath("../resources/product/full/") + hidimage.ToString();
                            string strtMedium = Server.MapPath("../resources/product/medium/") + hidimage.ToString();

                            //string newThumbSku = Server.MapPath("../resources/product/thumb/") + sku + strimg[1];
                            //if (File.Exists(newThumbSku))
                            //{
                            //    string newActualSku = Server.MapPath("../resources/product/full/") + sku + strimg[1];
                            //    string newMediumSku = Server.MapPath("../resources/product/medium/") + sku + strimg[1];

                            //    filname = sku + strimg[1];
                            //    File.Copy(strthumb, newThumbSku);
                            //    File.Copy(strtActual, newActualSku);
                            //    File.Copy(strtMedium, newMediumSku);

                            //    ////delete file
                            //    File.Delete(strthumb);
                            //    File.Delete(strtActual);
                            //    File.Delete(strtMedium);

                            //    break;
                            //}
                            //else
                            //{
                            for (int fl = 0; fl <= imgArray.Length; fl++)
                            {
                                string newThumb = Server.MapPath("../resources/product/thumb/") + sku + imgArray[fl] + "." + strimg[1];
                                string newActual = Server.MapPath("../resources/product/full/") + sku + imgArray[fl] + "." + strimg[1];
                                string newMedium = Server.MapPath("../resources/product/medium/") + sku + imgArray[fl] + "." + strimg[1];

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
                                if (Convert.ToString(hidimage) == Convert.ToString(sku + imgArray[fl] + "." + strimg[1]))
                                {
                                    string TemnewThumb = Server.MapPath("../resources/product/TempThumb/") + sku + imgArray[fl] + "." + strimg[1];
                                    string TempnewActual = Server.MapPath("../resources/product/TempFull/") + sku + imgArray[fl] + "." + strimg[1];
                                    string TempnewMedium = Server.MapPath("../resources/product/TempMedium/") + sku + imgArray[fl] + "." + strimg[1];

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
                            //}

                            objprodct.imageName = filname;
                            objprodct.productImagesId = Convert.ToInt32(lblProdutImageId);
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

                            //gvAdmin.EditIndex = -1;
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
                }
            }
        }
    }
    protected void BtnShowAllImages_Click(object sender, EventArgs e)
    {
        Response.Redirect("productImageUpload.aspx");
    }
}