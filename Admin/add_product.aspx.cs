using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_add_product : System.Web.UI.Page
{
    productManager objProduct = new productManager();
    categoryManager objCategory = new categoryManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add/Modify Product - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Add/Modify Product";

        if (!Page.IsPostBack)
        {
            //BindBrandItem();
            BindLanguagesOnPageLoad();
            BindLanguagesOnTabLoad();
            BindParentCategory();
            BindCategoryLeftSideProduct();
            //BindCategoryPOPUPProduct();
            BindBrandLeftSideProduct();
            //BindBrandPOPUPProduct();

            Page.Title = "Add Product - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add Product";
            lblmsg.Visible = false;
            if (Request.QueryString["flag"] == "add")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "Product added successfully";
            }

            lnkNext.Visible = false;
            lnkPreview.Visible = false;
            btnDuplicate.Visible = false;

            if (Request.QueryString["flag"] == "edit")
            {
                Title = "Modify Product - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify Product";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {
                        DataTable dtcontent = new DataTable();

                        BindBrandLanguageOnedit(Convert.ToInt32(Request.QueryString["id"]));

                        objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                        hidprodID.Value = Request.QueryString["id"].ToString();

                        btnDuplicate.Visible = true;
                        lnkNext.Visible = true;
                        lnkPreview.Visible = true;
                        int NextId = Convert.ToInt32(objProduct.NextProductId());
                        int PreviewId = Convert.ToInt32(objProduct.PreviewProductId());
                        if (NextId > 0) { lnkNext.HRef = "add_product.aspx?flag=edit&id=" + NextId; } else { lnkNext.Visible = false; }
                        if (PreviewId > 0) { lnkPreview.HRef = "add_product.aspx?flag=edit&id=" + PreviewId; } else { lnkPreview.Visible = false; }

                        dtcontent = objProduct.SelectSingleItemById();
                        if (dtcontent.Rows.Count > 0)
                        {
                            txtprice.Text = dtcontent.Rows[0]["WholesalePrice"].ToString();
                            txtWholePrice.Text = dtcontent.Rows[0]["WholesalePrice"].ToString();
                            txtSuperMarketPrice.Text = dtcontent.Rows[0]["SuperMarketPrice"].ToString();
                            txtConvinitPrice.Text = dtcontent.Rows[0]["ConvinientStorePrice"].ToString();
                            txtcost.Text = dtcontent.Rows[0]["cost"].ToString();
                            txtminqty.Text = dtcontent.Rows[0]["minimumQuantity"].ToString();
                            txtinventry.Text = dtcontent.Rows[0]["inventory"].ToString();
                            txtVarient.Text = Server.HtmlDecode(dtcontent.Rows[0]["varientItem"].ToString());

                            DataTable dtimg = new DataTable();
                            dtimg = objProduct.SelectProductImage();
                            if (dtimg.Rows.Count > 0)
                            {
                                string strImg = string.Empty;
                                strImg += "<div class='prod_upload'>";
                                for (int p = 0; p < dtimg.Rows.Count; p++)
                                {
                                    strImg += "<span id=\"" + dtimg.Rows[p]["productImagesId"].ToString() + "\" class=\"prod_screen\">";
                                    strImg += "<a href=\"../" + AppSettings.PRODUCT_ACTULE_ROOTURL + dtimg.Rows[p]["imageName"].ToString() + "\" class=\"pop3\" rel=\"group1\">";
                                    strImg += "<img src=\"../" + AppSettings.PRODUCT_MEDIUM_ROOTURL + dtimg.Rows[p]["imageName"].ToString() + "\" width='150px' height='150px' /></a>";
                                    if (dtimg.Rows[p]["mainImage"].ToString() == "M")
                                    {
                                        strImg += "<a id='img_" + p + "' class='set_img' href=\"javascript:void(0)\" onclick=\"UpdateProductImageSetMain(" + dtimg.Rows[p]["productImagesId"].ToString() + ",this.id);\">Main</a>";
                                    }
                                    else
                                    {
                                        strImg += "<a id='img_" + p + "' class='set_img' href=\"javascript:void(0)\" onclick=\"UpdateProductImageSetMain(" + dtimg.Rows[p]["productImagesId"].ToString() + ",this.id);\">Set</a>";
                                    }
                                    strImg += "<a href=\"javascript:void(0)\" class='del_prod' onclick=\"DeleteProductImage(" + dtimg.Rows[p]["productImagesId"].ToString() + ");\"> Delete </a>";
                                    strImg += "</span>";
                                }
                                strImg += "</div>";
                                imgltr.Text = strImg;
                            }

                            chkactive.Checked = Convert.ToBoolean(dtcontent.Rows[0]["isactive"].ToString());
                            chkfeatured.Checked = Convert.ToBoolean(dtcontent.Rows[0]["isFeatured"].ToString());

                            if (Convert.ToBoolean(dtcontent.Rows[0]["isvarientproduct"].ToString()) == true)
                            { rbtvariant.Checked = true; rbtsingleprod.Checked = false; }
                            else
                            { rbtsingleprod.Checked = true; rbtvariant.Checked = false; }

                            int days = 0;

                            if (dtcontent.Rows[0]["updatedate"].ToString() != "1/1/1900 12:00:00 AM")//inventory_log
                            {
                                days = Convert.ToInt32((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(dtcontent.Rows[0]["updatedate"].ToString())).Days);
                                inventory_log.InnerText = "Updated " + Convert.ToString(days) + " days ago.";
                            }
                            else
                            {
                                days = Convert.ToInt32((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(dtcontent.Rows[0]["createdate"].ToString())).Days);
                                inventory_log.InnerText = "Updated " + Convert.ToString(days) + " days ago.";
                            }
                            //hfprevsort.Value = dtcontent.Rows[0]["sortorder"].ToString();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "BindLeftCategoryProduct('" + Request.QueryString["id"] + "');BindLeftBrandProduct('" + Request.QueryString["id"] + "');BindLeftTagsProduct('" + Request.QueryString["id"] + "');BindMasterProductForProduct('" + Request.QueryString["id"] + "');", true);
                    }
                    else
                        Response.Redirect("add_product.aspx");
                }
                else
                    Response.Redirect("add_product.aspx");
            }
            //else
            //{
            //    txtsortorder.Text = Convert.ToString(CommonFunctions.GetLastSortCount("brand", "sortorder"));
            //}
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        //lblmsgs.Text = "";
        int maxID;
        if (Page.IsValid)
        {

            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                if (i == 0)
                {
                    objProduct.productName = Server.HtmlEncode(Convert.ToString(Request.Form["txtbrandname" + (i + 1).ToString()])); //Convert.ToInt32(Request.Form["splangid" + (i + 1).ToString()]);
                    objProduct.productDescription = Server.HtmlEncode(Convert.ToString(Request.Form["txtbranddesname" + (i + 1).ToString()]));

                    objProduct.sku = Server.HtmlEncode(Convert.ToString(Request.Form["txtsku"]));
                    objProduct.barcode = Server.HtmlEncode(Convert.ToString(Request.Form["txtbarcode"]));
                    //objProduct.barcode = txtbarcode.Text;
                    // veriant select

                    objProduct.QRCOde = UploadQRCode(Convert.ToString(objProduct.sku));

                    if (rbtvariant.Checked == true)
                    { objProduct.isVarientProduct = 1; }
                    else
                    { objProduct.isVarientProduct = 0; }

                    //single select 
                    //if (rbtsingleprod.Checked == true)
                    //{ objProduct.isMasterProduct = 0; }
                    //else { objProduct.isMasterProduct = 1; }
                    objProduct.isMasterProduct = 0;

                    objProduct.price = (txtWholePrice.Text != "") ? Convert.ToDecimal(txtWholePrice.Text.ToString()) : Convert.ToDecimal(0);
                    objProduct.WholesalePrice = (txtWholePrice.Text != "") ? Convert.ToDecimal(txtWholePrice.Text.ToString()) : Convert.ToDecimal(0);
                    objProduct.SuperMarketPrice = (txtSuperMarketPrice.Text != "") ? Convert.ToDecimal(txtSuperMarketPrice.Text.ToString()) : Convert.ToDecimal(0);
                    objProduct.ConvinientStorePrice = (txtConvinitPrice.Text != "") ? Convert.ToDecimal(txtConvinitPrice.Text.ToString()) : Convert.ToDecimal(0);

                    objProduct.cost = Convert.ToDecimal(txtcost.Text.ToString());
                    objProduct.minimumQuantity = Convert.ToInt32(txtminqty.Text);
                    objProduct.inventory = Convert.ToInt32(txtinventry.Text);

                    objProduct.varientItem = Server.HtmlEncode(txtVarient.Text.Trim());

                    if (chkactive.Checked == true) { objProduct.isactive = Convert.ToByte(1); } else { objProduct.isactive = Convert.ToByte(0); }
                    if (chkfeatured.Checked == true) { objProduct.isFeatured = Convert.ToByte(1); } else { objProduct.isFeatured = Convert.ToByte(0); }


                    if (Request.QueryString["flag"] == "edit")
                    {
                        objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);

                        objProduct.UpdateQRCode();
                        // if (objProduct.TitleExist())
                        // {
                        // lblmsg.Visible = true;
                        // lblmsgs.Text = "Product name already exists.";
                        // return;
                        // }
                        if (objProduct.SkuExist())
                        {
                            lblmsg.Visible = true;
                            lblmsgs.Text = "Sku already exists.";
                            return;
                        }

                        // update product
                        objProduct.UpdateItem();

                        // update product language
                        UpdateProductLanguage(objProduct.productId);

                        //// update product images

                        if (Request.QueryString["dpsku"] == "dpsku" || hidSku.Value == "")
                        {
                            DataTable dtimages = new DataTable();
                            dtimages = objProduct.getAllProductImageByProductid();
                            if (dtimages.Rows.Count > 0)
                            {
                                for (int di = 0; di < dtimages.Rows.Count; di++)
                                {
                                    char[] imgArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

                                    string filname = string.Empty;
                                    string[] strimg = dtimages.Rows[di]["imagename"].ToString().Split('.');

                                    string strthumb = Server.MapPath("../resources/product/thumb/") + dtimages.Rows[di]["imagename"].ToString();
                                    string strtActual = Server.MapPath("../resources/product/full/") + dtimages.Rows[di]["imagename"].ToString();
                                    string strtMedium = Server.MapPath("../resources/product/medium/") + dtimages.Rows[di]["imagename"].ToString();

                                    string newThumb = string.Empty;
                                    string newActual = string.Empty;
                                    string newMedium = string.Empty;

                                    newThumb = Server.MapPath("../resources/product/thumb/") + objProduct.sku + "." + strimg[1];
                                    newActual = Server.MapPath("../resources/product/full/") + objProduct.sku + "." + strimg[1];
                                    newMedium = Server.MapPath("../resources/product/medium/") + objProduct.sku + "." + strimg[1];

                                    if (!File.Exists(newThumb))
                                    {
                                        filname = objProduct.sku + "." + strimg[1];
                                        File.Copy(strthumb, newThumb);
                                        File.Copy(strtActual, newActual);
                                        File.Copy(strtMedium, newMedium);

                                        //delete file
                                        File.Delete(strthumb);
                                        File.Delete(strtActual);
                                        File.Delete(strtMedium);

                                        objProduct.imageName = filname;
                                        objProduct.productImagesId = Convert.ToInt32(dtimages.Rows[di]["productImagesId"]);
                                        objProduct.UpdateImageNameByProductImageId();
                                    }
                                    else
                                    {
                                        for (int fl = 0; fl <= imgArray.Length; fl++)
                                        {
                                            newThumb = Server.MapPath("../resources/product/thumb/") + objProduct.sku + imgArray[fl] + "." + strimg[1];
                                            newActual = Server.MapPath("../resources/product/full/") + objProduct.sku + imgArray[fl] + "." + strimg[1];
                                            newMedium = Server.MapPath("../resources/product/medium/") + objProduct.sku + imgArray[fl] + "." + strimg[1];

                                            if (!File.Exists(newThumb))
                                            {
                                                filname = objProduct.sku + imgArray[fl] + "." + strimg[1];
                                                File.Copy(strthumb, newThumb);
                                                File.Copy(strtActual, newActual);
                                                File.Copy(strtMedium, newMedium);

                                                //delete file
                                                File.Delete(strthumb);
                                                File.Delete(strtActual);
                                                File.Delete(strtMedium);

                                                objProduct.imageName = filname;
                                                objProduct.productImagesId = Convert.ToInt32(dtimages.Rows[di]["productImagesId"]);
                                                objProduct.UpdateImageNameByProductImageId();

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        HttpFileCollection uploadedFiles = Request.Files;
                        for (int im = 0; im < uploadedFiles.Count - 1; im++)
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
                                    string strImag = UploadImage(userPostedFile, objProduct.sku);
                                    if (strImag != "Noimage")
                                    {
                                        objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                        objProduct.isactive = 1;
                                        objProduct.InsertProductImageItem();

                                        int prodimageID = objProduct.GetmaximageProductId();
                                        objProduct.productImagesId = prodimageID;
                                        objProduct.actualImageName = userPostedFile.FileName;
                                        //create image name   imagename = productid + img_imgcount 
                                        objProduct.imageName = strImag;
                                        objProduct.imgLabel = objProduct.productName;
                                        objProduct.UpdateImage();
                                    }
                                }
                                else
                                { }
                            }
                        }

                        if (rbtvariant.Checked == true)
                        {
                            objProduct.DeleteProductCategory();
                            objProduct.DeleteProductBrand();
                        }
                        else
                        {
                            //delete product and master product link
                            objProduct.DeleteMasterProductLink();

                            //Update product's category
                            if (hidLeftCategoryIdProduct.Value != "0" && hidLeftCategoryIdProduct.Value != null && hidLeftCategoryIdProduct.Value != "")
                            {
                                objProduct.DeleteProductCategory();

                                string LeftCategoryProduct = hidLeftCategoryIdProduct.Value;
                                string[] LeftCategoryIdProduct = LeftCategoryProduct.Split(',');
                                for (int lc = 0; lc < LeftCategoryIdProduct.Count(); lc++)
                                {
                                    objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                    objProduct.categoryId = Convert.ToInt32(LeftCategoryIdProduct[lc]);
                                    objProduct.InsertProductCategroyItem();
                                }
                            }

                            // update product's brand
                            if (hidLeftBrandIdProduct.Value != "0" && hidLeftBrandIdProduct.Value != null && hidLeftBrandIdProduct.Value != "")
                            {
                                objProduct.DeleteProductBrand();

                                string LeftBrandProduct = hidLeftBrandIdProduct.Value;
                                string[] LeftBrandIdProduct = LeftBrandProduct.Split(',');
                                for (int lb = 0; lb < LeftBrandIdProduct.Count(); lb++)
                                {
                                    objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                    objProduct.barndId = Convert.ToInt32(LeftBrandIdProduct[lb]);
                                    objProduct.InsertProductBrandItem();
                                }
                            }
                        }

                        // update product tags
                        if (hidLeftTagsProduct.Value != "" && hidLeftTagsProduct.Value != null && hidLeftTagsProduct.Value != "0")
                        {
                            objProduct.DeleteProductTag();

                            string LeftTagsProduct = hidLeftTagsProduct.Value;
                            string[] LeftagesidProduct = LeftTagsProduct.Split(',');
                            for (int lb = 0; lb < LeftagesidProduct.Count(); lb++)
                            {
                                string sortor = Convert.ToString(CommonFunctions.GetLastSortCount("productTag", "sortorder"));

                                objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                objProduct.tagName = Convert.ToString(LeftagesidProduct[lb]);
                                objProduct.sortOrder = Convert.ToInt32(sortor);
                                objProduct.isactive = 1;
                                objProduct.InsertProductTagItem();
                            }
                        }

                        //update product's master product
                        string check = string.Empty;
                        for (int sk = 1; sk <= Convert.ToInt32(mastercount.Value); sk++)
                        {
                            check = Request.Form["lbl" + sk.ToString()];
                            if (check != "" && check != null)
                            {
                                objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                objProduct.DeleteMasterProductLink();
                                objProduct.masterProductId = Convert.ToInt32((Request.Form["lbl" + sk.ToString()]));
                                objProduct.InsertMasterProductLinkItem();
                            }
                        }

                        lblmsgs.Text = "Product updated successfully.";
                        //Response.Redirect("viewproducts.aspx?flag=edit&id=" + Request.QueryString["id"]);
                        Response.Redirect("add_product.aspx?flag=edit&id=" + Request.QueryString["id"]);
                    }
                    else
                    {
                        objProduct.productId = 0;
                        // if (objProduct.TitleExist())
                        // {
                        // lblmsg.Visible = true;
                        // lblmsgs.Text = "Product name already exists.";
                        // return;
                        // }
                        if (objProduct.SkuExist())
                        {
                            lblmsg.Visible = true;
                            lblmsgs.Text = "Sku already exists.";
                            return;
                        }

                        objProduct.productId = 0;
                        objProduct.InsertItem();
                        maxID = objProduct.getmaxid();

                        objProduct.productId = maxID;
                        objProduct.UpdateQRCode();

                        InsertProductLanguage(maxID);



                        ////insert image product
                        HttpFileCollection uploadedFiles = Request.Files;
                        for (int im = 0; im < uploadedFiles.Count - 1; im++)
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
                                    string strImag = UploadImage(userPostedFile, objProduct.sku);
                                    if (strImag != "Noimage")
                                    {
                                        objProduct.productId = maxID;
                                        objProduct.isactive = 1;
                                        objProduct.InsertProductImageItem();

                                        int prodimageID = objProduct.GetmaximageProductId();
                                        objProduct.productImagesId = prodimageID;
                                        objProduct.actualImageName = userPostedFile.FileName;
                                        //create image name   imagename = productid + img_imgcount 
                                        objProduct.imageName = strImag;
                                        objProduct.imgLabel = objProduct.productName;
                                        objProduct.UpdateImage();
                                    }
                                }
                                else
                                { }
                            }
                        }


                        //product and category
                        if (hidLeftCategoryIdProduct.Value != "0" && hidLeftCategoryIdProduct.Value != null && hidLeftCategoryIdProduct.Value != "")
                        {
                            string LeftCategoryProduct = hidLeftCategoryIdProduct.Value;
                            string[] LeftCategoryIdProduct = LeftCategoryProduct.Split(',');
                            for (int lc = 0; lc < LeftCategoryIdProduct.Count(); lc++)
                            {
                                objProduct.productId = maxID;
                                objProduct.categoryId = Convert.ToInt32(LeftCategoryIdProduct[lc]);
                                objProduct.InsertProductCategroyItem();
                            }
                        }

                        //product and brands
                        if (hidLeftBrandIdProduct.Value != "0" && hidLeftBrandIdProduct.Value != null && hidLeftBrandIdProduct.Value != "")
                        {
                            string LeftBrandProduct = hidLeftBrandIdProduct.Value;
                            string[] LeftBrandIdProduct = LeftBrandProduct.Split(',');
                            for (int lb = 0; lb < LeftBrandIdProduct.Count(); lb++)
                            {
                                objProduct.productId = maxID;
                                objProduct.barndId = Convert.ToInt32(LeftBrandIdProduct[lb]);
                                objProduct.InsertProductBrandItem();
                            }
                        }

                        // product tags
                        if (hidLeftTagsProduct.Value != "" && hidLeftTagsProduct.Value != null && hidLeftTagsProduct.Value != "0")
                        {
                            string LeftTagsProduct = hidLeftTagsProduct.Value;
                            string[] LeftagesidProduct = LeftTagsProduct.Split(',');
                            for (int lb = 0; lb < LeftagesidProduct.Count(); lb++)
                            {
                                string sortor = Convert.ToString(CommonFunctions.GetLastSortCount("productTag", "sortorder"));

                                objProduct.productId = maxID;
                                objProduct.tagName = Convert.ToString(LeftagesidProduct[lb]);
                                objProduct.sortOrder = Convert.ToInt32(sortor);
                                objProduct.isactive = 1;
                                objProduct.InsertProductTagItem();
                            }
                        }

                        //master product and product
                        string check = string.Empty;
                        for (int sk = 1; sk <= Convert.ToInt32(mastercount.Value); sk++)
                        {
                            check = Request.Form["lbl" + sk.ToString()];
                            if (check != "")
                            {
                                objProduct.productId = maxID;
                                objProduct.masterProductId = Convert.ToInt32((Request.Form["lbl" + sk.ToString()]));
                                objProduct.InsertMasterProductLinkItem();
                            }
                        }

                        Response.Redirect("viewproducts.aspx?flag=add&key=" + Request.QueryString["key"]);
                    }

                }

            }
        }
    }

    public void BindParentCategory()
    {
        DataTable dt = new DataTable();
        DataTable dtsub = new DataTable();
        try
        {
            dt = objCategory.GetParentCategory(true);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem li = new ListItem(Server.HtmlDecode(dt.Rows[i]["categoryName"].ToString()), dt.Rows[i]["categoryId"].ToString());
                    li.Attributes.Add("style", "color:#a7688a;font-weight:bold;");
                    ddlPOPProductParentCategory.Items.Add(li);
                    objCategory.parentid = Convert.ToInt32(dt.Rows[i]["categoryId"].ToString());
                    dtsub = objCategory.GetSubCategory();
                    if (dtsub != null && dtsub.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtsub.Rows.Count; j++)
                        {
                            ListItem lisub = new ListItem("--" + Server.HtmlDecode(dtsub.Rows[j]["categoryName"].ToString()), dtsub.Rows[j]["categoryId"].ToString());
                            lisub.Attributes.Add("style", "color:#6dace5");
                            ddlPOPProductParentCategory.Items.Add(lisub);

                        }
                    }
                }
                ddlPOPProductParentCategory.Items.Insert(0, new ListItem("--Select Parent Category--", "0"));
            }
            else
            {
                ddlPOPProductParentCategory.Items.Insert(0, new ListItem("--No Category Available--", "0"));
            }

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

    protected void lnkdelimg_Click(object sender, EventArgs e)
    {

    }

    private void BindLanguagesOnPageLoad()
    {
        string strLanguage = "";

        DataTable dt = new DataTable();
        CommanlanguagesManager objLanguage = new CommanlanguagesManager();
        try
        {
            dt = objLanguage.SelectLanguageForField();
            if (dt.Rows.Count > 0)
            {
                int languagecount = 0;
                hdtotallanguage.Value = dt.Rows.Count.ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    languagecount++;
                    if (i == 0)
                    {
                        strLanguage += "<input type='text' value='" + dt.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";
                        strLanguage += "<div class='tab-pane active' id='" + dt.Rows[i]["languageName"].ToString() + "'>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>Product  Name </label><div class='col-sm-12'><input type='text' class='form-control' id='txtbrandname" + (i + 1).ToString() + "' maxlength='100' name='txtbrandname" + (i + 1).ToString() + "'/><div id='productnamemsg' style='color:red'></div></div></div>";
                        #region "--------------------SKU And Barcode ------------------"
                        strLanguage += "<div class='form-group'>";

                        strLanguage += "<label for='inputName' class='col-sm-2 control-label'>SKU</label>";
                        strLanguage += "<div class='col-sm-4'><input type='text' class='form-control' id='txtsku' maxlength='20' onchange='return onTxtSkuChange();' onkeypress='return isNumberKey(event);' name='txtsku'/><div id='skumsg' style='color:red'></div></div>";
                        strLanguage += "<label for='inputName' class='col-sm-2 control-label'>Barcode</label>";
                        strLanguage += "<div class='col-sm-4'><input type='text' class='form-control' id='txtbarcode' maxlength='100' name='txtbarcode'/></div></div>";
                        #endregion
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>Product Description </label><div class='col-sm-12'><textarea  class='form-control ckeditor' id='txtbranddesname" + (i + 1).ToString() + "'  maxlength='500' name='txtbranddesname" + (i + 1).ToString() + "'></textarea ></div></div>";
                        strLanguage += "</div>";



                    }
                    else
                    {
                        strLanguage += "<input type='text' value='" + dt.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";
                        strLanguage += "  <div class='tab-pane' id='" + dt.Rows[i]["languageName"].ToString() + "'>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>اسم الصنف </label><div class='col-sm-12'><input type='text' class='form-control' id='txtbrandname" + (i + 1).ToString() + "' maxlength='100' name='txtbrandname" + (i + 1).ToString() + "' /><div id='productnamemsg' style='color:red'></div></div></div>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>الوصف </label><div class='col-sm-12'><textarea  class='form-control ckeditor' id='txtbranddesname" + (i + 1).ToString() + "' maxlength='500' name='txtbranddesname" + (i + 1).ToString() + "' ></textarea ></div></div>";
                        strLanguage += "</div>";
                    }
                }
            }
            ltrcategorylanguages.Text = strLanguage.ToString();

        }
        catch (Exception ex) { throw ex; }
        finally { dt.Clear(); dt.Dispose(); objLanguage = null; }
    }

    private void BindLanguagesOnTabLoad()
    {
        string strLanguage = "";
        DataTable dt = new DataTable();

        CommanlanguagesManager objLanguage = new CommanlanguagesManager();
        try
        {
            dt = objLanguage.SelectLanguageForField();
            if (dt.Rows.Count > 0)
            {

                int languagecount = 0;
                hdtotallanguage.Value = dt.Rows.Count.ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    languagecount++;

                    strLanguage += "<input type='text' value='" + dt.Rows[i]["languageid"].ToString() + "' name='splangidtab" + (i + 1).ToString() + "' id='splangidtab" + (i + 1).ToString() + "' style='display:none;' />";
                    if (i == 0)
                    {
                        strLanguage += "<li class='active'><a href='#" + dt.Rows[i]["languageName"].ToString() + "' data-toggle='tab'>" + dt.Rows[i]["languageName"].ToString() + " </a></li>";
                    }
                    else
                    {
                        strLanguage += "<li ><a href='#" + dt.Rows[i]["languageName"].ToString() + "' data-toggle='tab'>" + dt.Rows[i]["languageName"].ToString() + " </a></li>";
                    }
                }
            }
            ltrtab.Text = strLanguage.ToString();

        }
        catch (Exception ex) { throw ex; }
        finally { dt.Clear(); dt.Dispose(); objLanguage = null; }
    }

    private void BindBrandLanguageOnedit(int brandid)
    {
        DataTable dtLanguages = new DataTable();

        try
        {
            string strLanguage = "";
            objProduct.productId = brandid;
            dtLanguages = objProduct.SelectProductLanguagebyID();
            if (dtLanguages.Rows.Count > 0)
            {

                hdtotallanguage.Value = dtLanguages.Rows.Count.ToString();

                for (int i = 0; i < dtLanguages.Rows.Count; i++)
                {


                    if (i == 0)
                    {

                        DataTable dtprname = new DataTable();
                        dtprname = objProduct.SelectProductNameDeasripeionID();

                        strLanguage += "<input type='hidden' value='" + dtLanguages.Rows[i]["productsLanguage"].ToString() + "' name='hdid" + (i + 1).ToString() + "' id='hdid" + (i + 1).ToString() + "'>";
                        strLanguage += "<input type='text' value='" + dtLanguages.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";

                        strLanguage += "<div class='tab-pane active' id='" + dtLanguages.Rows[i]["languageName"].ToString() + "'>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>Product  Name </label><div class='col-sm-12'><input type='text' class='form-control' id='txtbrandname" + (i + 1).ToString() + "' value='" + Server.HtmlDecode(dtprname.Rows[0]["productName"].ToString()) + "' maxlength='100' name='txtbrandname" + (i + 1).ToString() + "'><div id='productnamemsg' style='color:red'></div></div></div>";

                        #region "--------------------SKU And Barcode ------------------"
                        hidSku.Value = dtprname.Rows[0]["sku"].ToString();
                        strLanguage += "<div class='form-group'>";
                        strLanguage += "<label for='inputName' class='col-sm-2 control-label'>SKU</label>";
                        strLanguage += "<div class='col-sm-4'><input type='text' class='form-control' id='txtsku'  onchange='return onTxtSkuChange();'  maxlength='20' name='txtsku' onkeypress='return isNumberKey(event);' value='" + dtprname.Rows[0]["sku"].ToString() + "'/><div id='skumsg' style='color:red'></div></div>";
                        strLanguage += "<label for='inputName' class='col-sm-2 control-label'>Barcode</label>";
                        strLanguage += "<div class='col-sm-4'><input type='text' class='form-control' id='txtbarcode' maxlength='100' name='txtbarcode' value='" + dtprname.Rows[0]["barcode"].ToString() + "'  /></div></div>";
                        #endregion

                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>Product Description </label><div class='col-sm-12'><textarea class='form-control ckeditor' id='txtbranddesname" + (i + 1).ToString() + "'  maxlength='500' name='txtbranddesname" + (i + 1).ToString() + "' value='" + Server.HtmlDecode(dtprname.Rows[0]["ProductDescription"].ToString()) + "'>" + Server.HtmlDecode(dtprname.Rows[0]["ProductDescription"].ToString()) + "</textarea></div></div>";
                        strLanguage += "</div>";

                    }
                    else
                    {
                        strLanguage += "<input type='hidden' value='" + dtLanguages.Rows[i]["productsLanguage"].ToString() + "' name='hdid" + (i + 1).ToString() + "' id='hdid" + (i + 1).ToString() + "'>";
                        strLanguage += "<input type='text' value='" + dtLanguages.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";

                        strLanguage += "<div class='tab-pane' id='" + dtLanguages.Rows[i]["languageName"].ToString() + "'>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>اسم الصنف </label><div class='col-sm-12'><input type='text' class='form-control' id='txtbrandname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["productName"].ToString() + "' maxlength='100' name='txtbrandname" + (i + 1).ToString() + "'><div id='productnamemsg' style='color:red'></div></div></div>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>الوصف </label><div class='col-sm-12'><textarea class='form-control ckeditor' id='txtbranddesname" + (i + 1).ToString() + "' maxlength='500' name='txtbranddesname" + (i + 1).ToString() + "' value='" + Server.HtmlDecode(dtLanguages.Rows[i]["ProductDescription"].ToString()) + "'>" + Server.HtmlDecode(dtLanguages.Rows[i]["ProductDescription"].ToString()) + "</textarea></div></div>";
                        strLanguage += "</div>";

                    }

                }

                ltrcategorylanguages.Text = strLanguage.ToString();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { dtLanguages.Dispose(); dtLanguages.Clear(); }
    }

    public void InsertProductLanguage(int brandId)
    {
        try
        {
            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                objProduct.productId = brandId;
                objProduct.languageId = Convert.ToInt32(Request.Form["splangid" + (i + 1).ToString()]);
                objProduct.productName = Server.HtmlEncode(Convert.ToString(Request.Form["txtbrandname" + (i + 1).ToString()]));
                objProduct.productDescription = Server.HtmlEncode(Convert.ToString(Request.Form["txtbranddesname" + (i + 1).ToString()]));
                objProduct.InsertProductLanguageItem();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

    public void BindCategoryLeftSideProduct()
    {
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
                    ListItem li = new ListItem(Server.HtmlDecode(dt.Rows[i]["categoryName"].ToString()), dt.Rows[i]["categoryId"].ToString());
                    li.Attributes.Add("style", "color:#a7688a;font-weight:bold;");
                    ddlcategoryleftside.Items.Add(li);
                    objCategory.parentid = Convert.ToInt32(dt.Rows[i]["categoryId"].ToString());
                    dtsub = objCategory.GetSubCategory();
                    if (dtsub != null && dtsub.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtsub.Rows.Count; j++)
                        {
                            ListItem lisub = new ListItem(Server.HtmlDecode(dtsub.Rows[j]["categoryName"].ToString()), dtsub.Rows[j]["categoryId"].ToString());
                            lisub.Attributes.Add("style", "color:#6dace5");
                            ddlcategoryleftside.Items.Add(lisub);

                        }
                    }
                }

            }

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

    public void BindCategoryPOPUPProduct()
    {
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
                    ListItem li = new ListItem(Server.HtmlDecode(dt.Rows[i]["categoryName"].ToString()), dt.Rows[i]["categoryId"].ToString());
                    li.Attributes.Add("style", "color:#a7688a;font-weight:bold;");
                    ddlPOPCategoryProduct.Items.Add(li);
                    objCategory.parentid = Convert.ToInt32(dt.Rows[i]["categoryId"].ToString());
                    dtsub = objCategory.GetSubCategory();
                    if (dtsub != null && dtsub.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtsub.Rows.Count; j++)
                        {
                            ListItem lisub = new ListItem(Server.HtmlDecode(dtsub.Rows[j]["categoryName"].ToString()), dtsub.Rows[j]["categoryId"].ToString());
                            lisub.Attributes.Add("style", "color:#6dace5");
                            ddlPOPCategoryProduct.Items.Add(lisub);

                        }
                    }
                }

            }

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

    public void BindBrandLeftSideProduct()
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
                    ddlBrandleftside.Items.Add(lisub);
                }

            }

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

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
        finally { }
    }

    public void UpdateProductLanguage(int Id)
    {
        productManager objbrandLanguage = new productManager();
        try
        {
            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                objbrandLanguage.productId = Id;
                objbrandLanguage.productsLanguage = Convert.ToInt32(Request.Form["hdid" + (i + 1).ToString()]);
                objbrandLanguage.productName = Convert.ToString(Request.Form["txtbrandname" + (i + 1).ToString()]);
                objbrandLanguage.productDescription = Convert.ToString(Request.Form["txtbranddesname" + (i + 1).ToString()]);
                int lan = i + 1;
                objbrandLanguage.languageId = lan;
                if (objbrandLanguage.productsLanguage == 0)
                {
                    objbrandLanguage.InsertProductLanguageItem();
                }
                else
                {
                    objbrandLanguage.UpdateProductLanguageItem();
                }

            }
        }
        catch (Exception ex) { throw ex; }
        finally { objbrandLanguage = null; }
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

        if (Path.GetExtension(Path.GetFileName(fileObject.FileName)).ToLower() == ".jpg" || Path.GetExtension(Path.GetFileName(fileObject.FileName)).ToLower() == ".png")
        {

            string newThumbSku = Server.MapPath("../resources/product/thumb/") + maxID + Path.GetExtension(Path.GetFileName(fileObject.FileName));
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
        }
        else
        {
            filename = "Noimage";
        }
        return filename;


    }

    // save sku
    protected string UploadQRCode(String SkuName)
    {
        string code = SkuName;
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        imgBarCode.Height = 100;
        imgBarCode.Width = 100;
        using (Bitmap bitMap = qrCode.GetGraphic(20))
        {
            byte[] byteImage;
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byteImage = ms.ToArray();
                imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
            //plBarCode.Controls.Add(imgBarCode);

            //save QR code to images
            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(byteImage))
            {
                image = System.Drawing.Image.FromStream(ms);
            }
            //image.Save(Server.MapPath("~/Images/test.jpeg"));

            string actualfolder = string.Empty;
            actualfolder = Server.MapPath("../" + AppSettings.QRCODE_ROOTURL);
            DirectoryInfo actDir = new DirectoryInfo(actualfolder);
            //check if Directory exist if not create it
            if (!actDir.Exists) { Directory.CreateDirectory(actualfolder); }
            string filename = string.Empty;
            string fullimagepath = string.Empty;
            fullimagepath = actualfolder + SkuName + ".jpg";
            //delete old files if Exists
            CommonFunctions.DeleteFile(fullimagepath);
            //save original image
            //txtImageName.PostedFile.SaveAs(fullimagepath);
            image.Save(fullimagepath);
        }
        return Convert.ToString(SkuName + ".jpg");
    }

    //public void BindBrandItem()
    //{
    //    DataTable dt = objProduct.GetBrandItem();
    //    ddlbrandlist.DataSource = dt;
    //    ddlbrandlist.DataBind();
    //}

    protected void btnDuplicate_Click(object sender, EventArgs e)
    {
        int maxID;
        if (Page.IsValid)
        {

            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                if (i == 0)
                {
                    objProduct.productName = Server.HtmlEncode(Convert.ToString(Request.Form["txtbrandname" + (i + 1).ToString()])); //Convert.ToInt32(Request.Form["splangid" + (i + 1).ToString()]);
                    objProduct.productDescription = Server.HtmlEncode(Convert.ToString(Request.Form["txtbranddesname" + (i + 1).ToString()]));

                    objProduct.sku = Server.HtmlEncode(Convert.ToString(Request.Form["txtsku"]));
                    //objProduct.sku = "";
                    objProduct.barcode = Server.HtmlEncode(Convert.ToString(Request.Form["txtbarcode"]));
                    // veriant select

                    if (rbtvariant.Checked == true)
                    { objProduct.isVarientProduct = 1; }
                    else
                    { objProduct.isVarientProduct = 0; }

                    objProduct.isMasterProduct = 0;

                    objProduct.price = (txtWholePrice.Text != "") ? Convert.ToDecimal(txtWholePrice.Text.ToString()) : Convert.ToDecimal(0);
                    objProduct.WholesalePrice = (txtWholePrice.Text != "") ? Convert.ToDecimal(txtWholePrice.Text.ToString()) : Convert.ToDecimal(0);
                    objProduct.SuperMarketPrice = (txtSuperMarketPrice.Text != "") ? Convert.ToDecimal(txtSuperMarketPrice.Text.ToString()) : Convert.ToDecimal(0);
                    objProduct.ConvinientStorePrice = (txtConvinitPrice.Text != "") ? Convert.ToDecimal(txtConvinitPrice.Text.ToString()) : Convert.ToDecimal(0);

                    objProduct.cost = Convert.ToDecimal(txtcost.Text.ToString());
                    objProduct.minimumQuantity = Convert.ToInt32(txtminqty.Text);
                    objProduct.inventory = Convert.ToInt32(txtinventry.Text);
                    objProduct.varientItem = Server.HtmlEncode(txtVarient.Text.Trim());

                    if (chkactive.Checked == true) { objProduct.isactive = Convert.ToByte(1); } else { objProduct.isactive = Convert.ToByte(0); }
                    if (chkfeatured.Checked == true) { objProduct.isFeatured = Convert.ToByte(1); } else { objProduct.isFeatured = Convert.ToByte(0); }

                    #region -- First Update Record

                    if (Request.QueryString["flag"] == "edit")
                    {
                        objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);

                        //if (objProduct.TitleExist())
                        //{
                        //    lblmsg.Visible = true;
                        //    lblmsgs.Text = "Product name already exists.";
                        //    return;
                        //}
                        if (objProduct.SkuExist())
                        {
                            lblmsg.Visible = true;
                            lblmsgs.Text = "Sku already exists.";
                            return;
                        }

                        // update product
                        objProduct.UpdateItem();

                        // update product language
                        UpdateProductLanguage(objProduct.productId);

                        //// update product images

                        if (Request.QueryString["dpsku"] == "dpsku")
                        {
                            DataTable dtimages = new DataTable();
                            dtimages = objProduct.getAllProductImageByProductid();
                            if (dtimages.Rows.Count > 0)
                            {
                                for (int di = 0; di < dtimages.Rows.Count; di++)
                                {
                                    char[] imgArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

                                    string filname = string.Empty;
                                    string[] strimg = dtimages.Rows[di]["imagename"].ToString().Split('.');

                                    string strthumb = Server.MapPath("../resources/product/thumb/") + dtimages.Rows[di]["imagename"].ToString();
                                    string strtActual = Server.MapPath("../resources/product/full/") + dtimages.Rows[di]["imagename"].ToString();
                                    string strtMedium = Server.MapPath("../resources/product/medium/") + dtimages.Rows[di]["imagename"].ToString();

                                    for (int fl = 0; fl <= imgArray.Length; fl++)
                                    {
                                        string newThumb = Server.MapPath("../resources/product/thumb/") + objProduct.sku + imgArray[fl] + "." + strimg[1];
                                        string newActual = Server.MapPath("../resources/product/full/") + objProduct.sku + imgArray[fl] + "." + strimg[1];
                                        string newMedium = Server.MapPath("../resources/product/medium/") + objProduct.sku + imgArray[fl] + "." + strimg[1];

                                        if (!File.Exists(newThumb))
                                        {
                                            filname = objProduct.sku + imgArray[fl] + "." + strimg[1];
                                            File.Copy(strthumb, newThumb);
                                            File.Copy(strtActual, newActual);
                                            File.Copy(strtMedium, newMedium);

                                            //delete file
                                            File.Delete(strthumb);
                                            File.Delete(strtActual);
                                            File.Delete(strtMedium);

                                            objProduct.imageName = filname;
                                            objProduct.productImagesId = Convert.ToInt32(dtimages.Rows[di]["productImagesId"]);
                                            objProduct.UpdateImageNameByProductImageId();

                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        HttpFileCollection uploadedFiles = Request.Files;
                        for (int im = 0; im < uploadedFiles.Count - 1; im++)
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
                                    objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                    objProduct.isactive = 1;
                                    objProduct.InsertProductImageItem();

                                    int prodimageID = objProduct.GetmaximageProductId();
                                    objProduct.productImagesId = prodimageID;
                                    objProduct.actualImageName = userPostedFile.FileName;
                                    //create image name   imagename = productid + img_imgcount 
                                    objProduct.imageName = UploadImage(userPostedFile, objProduct.sku);
                                    objProduct.imgLabel = objProduct.imageName;
                                    objProduct.UpdateImage();
                                }
                                else
                                { }
                            }
                        }


                        if (rbtvariant.Checked == true)
                        {
                            objProduct.DeleteProductCategory();
                            objProduct.DeleteProductBrand();
                        }
                        else
                        {
                            //delete product and master product link
                            objProduct.DeleteMasterProductLink();

                            //Update product's category
                            if (hidLeftCategoryIdProduct.Value != "0" && hidLeftCategoryIdProduct.Value != null && hidLeftCategoryIdProduct.Value != "")
                            {
                                objProduct.DeleteProductCategory();

                                string LeftCategoryProduct = hidLeftCategoryIdProduct.Value;
                                string[] LeftCategoryIdProduct = LeftCategoryProduct.Split(',');
                                for (int lc = 0; lc < LeftCategoryIdProduct.Count(); lc++)
                                {
                                    objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                    objProduct.categoryId = Convert.ToInt32(LeftCategoryIdProduct[lc]);
                                    objProduct.InsertProductCategroyItem();
                                }
                            }

                            // update product's brand
                            if (hidLeftBrandIdProduct.Value != "0" && hidLeftBrandIdProduct.Value != null && hidLeftBrandIdProduct.Value != "")
                            {
                                objProduct.DeleteProductBrand();

                                string LeftBrandProduct = hidLeftBrandIdProduct.Value;
                                string[] LeftBrandIdProduct = LeftBrandProduct.Split(',');
                                for (int lb = 0; lb < LeftBrandIdProduct.Count(); lb++)
                                {
                                    objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                    objProduct.barndId = Convert.ToInt32(LeftBrandIdProduct[lb]);
                                    objProduct.InsertProductBrandItem();
                                }
                            }
                        }

                        // update product tags
                        if (hidLeftTagsProduct.Value != "" && hidLeftTagsProduct.Value != null && hidLeftTagsProduct.Value != "0")
                        {
                            objProduct.DeleteProductTag();

                            string LeftTagsProduct = hidLeftTagsProduct.Value;
                            string[] LeftagesidProduct = LeftTagsProduct.Split(',');
                            for (int lb = 0; lb < LeftagesidProduct.Count(); lb++)
                            {
                                string sortor = Convert.ToString(CommonFunctions.GetLastSortCount("productTag", "sortorder"));

                                objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                objProduct.tagName = Convert.ToString(LeftagesidProduct[lb]);
                                objProduct.sortOrder = Convert.ToInt32(sortor);
                                objProduct.isactive = 1;
                                objProduct.InsertProductTagItem();
                            }
                        }

                        //update product's master product
                        string check = string.Empty;
                        for (int sk = 1; sk <= Convert.ToInt32(mastercount.Value); sk++)
                        {
                            check = Request.Form["lbl" + sk.ToString()];
                            if (check != "" && check != null)
                            {
                                objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                                objProduct.DeleteMasterProductLink();
                                objProduct.masterProductId = Convert.ToInt32((Request.Form["lbl" + sk.ToString()]));
                                objProduct.InsertMasterProductLinkItem();
                            }
                        }

                        lblmsgs.Text = "Product updated successfully.";
                        //Response.Redirect("viewproducts.aspx?flag=edit&id=" + Request.QueryString["id"]);
                    }

                    #endregion


                    #region --- Duplicate Record

                    if (lblmsgs.Text == "Product updated successfully.")
                    {

                        objProduct.sku = "";
                        objProduct.productId = 0;
                        objProduct.InsertItem();
                        maxID = objProduct.getmaxid();

                        InsertProductLanguage(maxID);

                        ////insert image product
                        objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                        DataTable dtdupimg = new DataTable();
                        dtdupimg = objProduct.SelectProductImage();
                        if (dtdupimg.Rows.Count > 0)
                        {
                            for (int dp = 0; dp < dtdupimg.Rows.Count; dp++)
                            {
                                //char[] imgArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
                                string[] strimg = Convert.ToString(dtdupimg.Rows[dp]["imageName"]).Split('.');
                                int prodimageID = objProduct.GetmaximageProductId();

                                string strthumb = Server.MapPath("../resources/product/thumb/") + Convert.ToString(dtdupimg.Rows[dp]["imageName"]);
                                string strtActual = Server.MapPath("../resources/product/full/") + Convert.ToString(dtdupimg.Rows[dp]["imageName"]);
                                string strtMedium = Server.MapPath("../resources/product/medium/") + Convert.ToString(dtdupimg.Rows[dp]["imageName"]);

                                string newThumb = Server.MapPath("../resources/product/thumb/") + prodimageID + "." + strimg[1];
                                string newActual = Server.MapPath("../resources/product/full/") + prodimageID + "." + strimg[1];
                                string newMedium = Server.MapPath("../resources/product/medium/") + prodimageID + "." + strimg[1];
                                if (!File.Exists(newThumb))
                                {
                                    File.Copy(strthumb, newThumb);
                                    File.Copy(strtActual, newActual);
                                    File.Copy(strtMedium, newMedium);
                                }

                                objProduct.productId = maxID;
                                //objProduct.imageName = Convert.ToString(dtdupimg.Rows[dp]["imageName"]);
                                objProduct.imageName = prodimageID + "." + strimg[1];
                                objProduct.actualImageName = Convert.ToString(dtdupimg.Rows[dp]["actualImageName"]);
                                objProduct.isactive = 0;
                                objProduct.sortOrder = 0;
                                objProduct.imgLabel = objProduct.productName;
                                objProduct.InsertDuplicateProductImageItem();
                            }
                        }

                        //product and category
                        if (hidLeftCategoryIdProduct.Value != "0" && hidLeftCategoryIdProduct.Value != null && hidLeftCategoryIdProduct.Value != "")
                        {
                            string LeftCategoryProduct = hidLeftCategoryIdProduct.Value;
                            string[] LeftCategoryIdProduct = LeftCategoryProduct.Split(',');
                            for (int lc = 0; lc < LeftCategoryIdProduct.Count(); lc++)
                            {
                                objProduct.productId = maxID;
                                objProduct.categoryId = Convert.ToInt32(LeftCategoryIdProduct[lc]);
                                objProduct.InsertProductCategroyItem();
                            }
                        }

                        //product and brands
                        if (hidLeftBrandIdProduct.Value != "0" && hidLeftBrandIdProduct.Value != null && hidLeftBrandIdProduct.Value != "")
                        {
                            string LeftBrandProduct = hidLeftBrandIdProduct.Value;
                            string[] LeftBrandIdProduct = LeftBrandProduct.Split(',');
                            for (int lb = 0; lb < LeftBrandIdProduct.Count(); lb++)
                            {
                                objProduct.productId = maxID;
                                objProduct.barndId = Convert.ToInt32(LeftBrandIdProduct[lb]);
                                objProduct.InsertProductBrandItem();
                            }
                        }

                        // product tags
                        if (hidLeftTagsProduct.Value != "" && hidLeftTagsProduct.Value != null && hidLeftTagsProduct.Value != "0")
                        {
                            string LeftTagsProduct = hidLeftTagsProduct.Value;
                            string[] LeftagesidProduct = LeftTagsProduct.Split(',');
                            for (int lb = 0; lb < LeftagesidProduct.Count(); lb++)
                            {
                                string sortor = Convert.ToString(CommonFunctions.GetLastSortCount("productTag", "sortorder"));

                                objProduct.productId = maxID;
                                objProduct.tagName = Convert.ToString(LeftagesidProduct[lb]);
                                objProduct.sortOrder = Convert.ToInt32(sortor);
                                objProduct.isactive = 1;
                                objProduct.InsertProductTagItem();
                            }
                        }

                        //master product and product
                        DataTable dtmaster = new DataTable();
                        objProduct.productId = Convert.ToInt32(Request.QueryString["id"]);
                        dtmaster = objProduct.SelectMasterProductLink();

                        if (dtmaster.Rows.Count > 0)
                        {
                            objProduct.productId = maxID;
                            objProduct.masterProductId = Convert.ToInt32(dtmaster.Rows[0]["masterProductId"]);
                            objProduct.InsertMasterProductLinkItem();
                        }

                        Response.Redirect("add_product.aspx?flag=edit&dpsku=dpsku&id=" + maxID);
                    }
                    #endregion
                }

            }


        }
    }

}