using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.UI.WebControls;
using System.Net;
using System.Xml;
using QRCoder;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{



    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    //[WebMethod]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public List<productManager> BindPopImages(string productid)
    //{

    //    productManager objProduct = new productManager();
    //    objProduct.productId = Convert.ToInt32(productid);
    //    List<productManager> lmp = new List<productManager>();
    //    DataTable dt = new DataTable();
    //    dt = objProduct.SelectProductImage();
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        productManager PMS = new productManager();
    //        PMS.imageName = dr["imageName"].ToString();
    //        lmp.Add(PMS);

    //    }
    //    return lmp;

    //}

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BindPopImages(string productid)
    {
        productManager objProduct = new productManager();
        objProduct.productId = Convert.ToInt32(productid);
        List<productManager> lmp = new List<productManager>();
        DataTable dt = new DataTable();

        dt = objProduct.SelectProductImage();
        string strImg = string.Empty;
        if (dt.Rows.Count > 0)
        {
            strImg += "<ul>";
            foreach (DataRow dr in dt.Rows)
            {
                strImg += "<li id=\"" + dr["productImagesId"].ToString() + "\">";
                strImg += "<img src=\"../" + AppSettings.PRODUCT_MEDIUM_ROOTURL + dr["imageName"].ToString() + "\" width='100px' height='100px' /><br />";
                strImg += "<a href=\"javascript:void(0)\" onclick=\"DeleteProductImage(" + dr["productImagesId"].ToString() + ");\"> Delete </a>";
                strImg += "</li>";
            }
            strImg += "</ul>";
        }
        return strImg;

    }

    // add mater product
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string addMasterProduct(string productName, string sku, string productDescription, string ddlPopCategoryProduct, string ddlPopBrandProduct, string ArabicName, string ArabicDiscription)
    {
        productManager objproduct = new productManager();
        if (productName != "" && sku != "")
        {
            objproduct.productName = Server.HtmlEncode(productName);
            objproduct.sku = sku;
            objproduct.productDescription = Server.HtmlEncode(productDescription);
            objproduct.barcode = "";
            objproduct.isVarientProduct = 0;
            objproduct.isMasterProduct = 1;
            objproduct.price = 0;
            objproduct.cost = 0;
            objproduct.minimumQuantity = 0;
            objproduct.inventory = 0;
            objproduct.varientItem = "";
            objproduct.isactive = 1;
            objproduct.isFeatured = 0;
            objproduct.varientItem = "";

            if (objproduct.SkuExist())
            {
                return "Product sku is already exist";
            }
            if (objproduct.TitleExist())
            {
                return "Product name is already exist";
            }

            objproduct.InsertItem();

            int maxid = objproduct.getmaxid();
            objproduct.productId = maxid;

            objproduct.QRCOde = UploadQRCode(sku);
            objproduct.UpdateQRCode();

            if (ddlPopCategoryProduct != null || ddlPopCategoryProduct != "0")
            {
                string[] PopCategory = ddlPopCategoryProduct.Split(',');
                for (int i = 0; i < PopCategory.Count(); i++)
                {

                    objproduct.categoryId = Convert.ToInt32(PopCategory[i]);
                    objproduct.InsertProductCategroyItem();
                }
            }
            if (ddlPopBrandProduct != null || ddlPopBrandProduct != "0")
            {
                string[] PopBrand = ddlPopBrandProduct.Split(',');
                for (int j = 0; j < PopBrand.Count(); j++)
                {
                    objproduct.productId = maxid;
                    objproduct.barndId = Convert.ToInt32(PopBrand[j]);
                    objproduct.InsertProductBrandItem();
                }
            }

            objproduct.languageId = 1;
            objproduct.InsertProductLanguageItem();

            objproduct.languageId = 2;
            objproduct.productName = Server.HtmlEncode(ArabicName);
            objproduct.productDescription = Server.HtmlEncode(ArabicDiscription);
            objproduct.InsertProductLanguageItem();

            //int maxID = objproduct.getmaxid();

            return "Master product inserted successfully";
        }
        else
        {
            return "Master product not inserted";
        }

    }

    // Create QR Code and save image in folder
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
            actualfolder = HttpContext.Current.Server.MapPath("~/" + AppSettings.QRCODE_ROOTURL);
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


    // add category from Product page 
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string addCategoryProduct(string parentid, string categoryName, string categoryDescription, string isactive)
    {
        categoryManager objcategory = new categoryManager();
        string sortor = Convert.ToString(CommonFunctions.GetLastSortCount("category", "sortorder"));
        if (parentid != "" && categoryName != "")
        {
            objcategory.parentid = Convert.ToInt32(parentid);
            objcategory.categoryName = categoryName;
            objcategory.catedesc = categoryDescription;
            objcategory.imagepath = "";
            objcategory.sortorder = Convert.ToInt32(sortor);
            objcategory.isactive = 1;
            //objcategory.InsertItem();
            if (objcategory.TitleExist())
            {
                return "Category name already exists.";
            }
            else
            {
                objcategory.InsertItem();
                return "Category added successfully";
            }
        }
        else
        {
            return "fail";
        }
    }

    // bind parent menu
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> BindParentMenu()
    {
        categoryManager objcategory = new categoryManager();
        DataTable dt = new DataTable();
        DataTable dtsub = new DataTable();
        try
        {
            List<string> Category_parent = new List<string>();
            dt = objcategory.GetParentCategory(true);
            if (dt != null && dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //ListItem li = new ListItem(dt.Rows[i]["categoryName"].ToString(), dt.Rows[i]["categoryId"].ToString());
                    //li.Attributes.Add("style", "color:#a7688a;font-weight:bold;");
                    Category_parent.Add(dt.Rows[i]["categoryName"].ToString());

                    objcategory.parentid = Convert.ToInt32(dt.Rows[i]["categoryId"].ToString());
                    dtsub = objcategory.GetSubCategory();
                    if (dtsub != null && dtsub.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtsub.Rows.Count; j++)
                        {
                            //ListItem lisub = new ListItem("--" + dtsub.Rows[j]["categoryName"].ToString(), dtsub.Rows[j]["categoryId"].ToString());
                            //lisub.Attributes.Add("style", "color:#6dace5");
                            Category_parent.Add("--" + dtsub.Rows[j]["categoryName"].ToString());

                        }
                    }
                }
                return Category_parent;

            }
            else
            {
                return Category_parent;
            }

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

    // bind category for product page leftside
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BindLeftCategoryProduct(string productid)
    {
        string strid = string.Empty;
        productManager objprodu = new productManager();
        DataTable dt = new DataTable();
        try
        {
            objprodu.productId = Convert.ToInt32(productid);
            dt = objprodu.SelectProductCategory();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        strid = dt.Rows[i]["categoryId"].ToString();
                    }
                    else
                    {
                        strid += "," + dt.Rows[i]["categoryId"].ToString();
                    }
                }
                return strid;
            }
            else
            {
                return strid;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { dt.Dispose(); objprodu = null; }
    }

    // bind Brand for product page leftside
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BindLeftBrandProduct(string productid)
    {
        string strid = string.Empty;
        productManager objprodu = new productManager();
        DataTable dt = new DataTable();
        try
        {
            objprodu.productId = Convert.ToInt32(productid);
            dt = objprodu.SelectProductBrand();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        strid = dt.Rows[i]["barndid"].ToString();
                    }
                    else
                    {
                        strid += "," + dt.Rows[i]["barndid"].ToString();
                    }
                }
                return strid;
            }
            else
            {
                return strid;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { dt.Dispose(); objprodu = null; }
    }

    // bind Tags for product page leftside
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BindLeftTagsProduct(string productid)
    {
        string strid = string.Empty;
        productManager objprodu = new productManager();
        DataTable dt = new DataTable();
        try
        {
            objprodu.productId = Convert.ToInt32(productid);
            dt = objprodu.SelectProductTag();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        strid = dt.Rows[i]["tagName"].ToString();
                    }
                    else
                    {
                        strid += "," + dt.Rows[i]["tagName"].ToString();
                    }
                }
                return strid;
            }
            else
            {
                return strid;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { dt.Dispose(); objprodu = null; }
    }

    // bind masterproduct for product page leftside
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<productManager> BindMasterProduct_Product(string productId)
    {
        productManager objProduct = new productManager();
        objProduct.productId = Convert.ToInt32(productId);
        List<productManager> MPN = new List<productManager>();
        DataTable dt = new DataTable();
        dt = objProduct.bindMsaterProductByProductId();
        foreach (DataRow dr in dt.Rows)
        {
            productManager PM = new productManager();
            PM.productId = Convert.ToInt32(dr["masterproductid"].ToString());
            PM.productName = dr["productname"].ToString();
            PM.sku = dr["sku"].ToString();
            MPN.Add(PM);
        }

        return MPN;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string updateMasterProduct(string productid, string productName, string sku, string productDescription, string ddlPopCategoryProduct, string ddlPopBrandProduct, string ArabicName, string ArabicDiscription)
    {
        productManager objproduct = new productManager();
        if (productName != "" && sku != "")
        {
            objproduct.productId = Convert.ToInt32(productid);
            objproduct.productName = productName;
            objproduct.sku = sku;
            objproduct.productDescription = productDescription;
            objproduct.barcode = "";
            objproduct.isVarientProduct = 0;
            objproduct.isMasterProduct = 1;
            objproduct.price = 0;
            objproduct.cost = 0;
            objproduct.minimumQuantity = 0;
            objproduct.inventory = 0;
            objproduct.varientItem = "";
            objproduct.isactive = 1;
            objproduct.isFeatured = 0;
            objproduct.varientItem = "";

            if (objproduct.TitleExist())
            {
                return "Product name is already exist";
            }
            if (objproduct.SkuExist())
            {
                return "Product sku is already exist";
            }

            objproduct.UpdateItem();

            objproduct.QRCOde = UploadQRCode(sku);
            objproduct.UpdateQRCode();

            //int maxid = objproduct.getmaxid();
            objproduct.productId = Convert.ToInt32(productid);

            if (ddlPopCategoryProduct != null || ddlPopCategoryProduct != "0")
            {
                objproduct.DeleteProductCategory();

                string[] PopCategory = ddlPopCategoryProduct.Split(',');
                for (int i = 0; i < PopCategory.Count(); i++)
                {
                    objproduct.productId = Convert.ToInt32(productid);
                    objproduct.categoryId = Convert.ToInt32(PopCategory[i]);
                    objproduct.InsertProductCategroyItem();
                }
            }
            if (ddlPopBrandProduct != null || ddlPopBrandProduct != "0")
            {
                objproduct.DeleteProductBrand();
                string[] PopBrand = ddlPopBrandProduct.Split(',');
                for (int j = 0; j < PopBrand.Count(); j++)
                {
                    objproduct.productId = Convert.ToInt32(productid);
                    objproduct.barndId = Convert.ToInt32(PopBrand[j]);
                    objproduct.InsertProductBrandItem();
                }
            }

            //language entry
            objproduct.DeleteProductLanguage();

            objproduct.languageId = 1;
            objproduct.InsertProductLanguageItem();

            objproduct.languageId = 2;
            objproduct.productName = Server.HtmlEncode(ArabicName);
            objproduct.productDescription = Server.HtmlEncode(ArabicDiscription);
            objproduct.InsertProductLanguageItem();

            return "Master product updated successfully";
        }
        else
        {
            return "Error in update master product";
        }
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<productManager> BindMasterProductdata(string id)
    {

        productManager objProduct = new productManager();
        objProduct.productId = Convert.ToInt32(id);
        List<productManager> lmp = new List<productManager>();
        DataTable dt = new DataTable();
        dt = objProduct.SelectSingleItemById();
        productManager PMS = new productManager();
        foreach (DataRow dr in dt.Rows)
        {

            PMS.sku = dr["sku"].ToString();
            PMS.productName = dr["productName"].ToString();
            PMS.productDescription = dr["productDescription"].ToString();
        }

        DataTable dt1 = new DataTable();
        dt1 = objProduct.GetProductNameAndDescriptionByLanguageid();
        if (dt1.Rows.Count > 1)
        {
            for (int i = 0; i < dt1.Rows.Count - 1; i++)
            {
                PMS.ArabicName = dt1.Rows[i + 1]["ArabicName"].ToString();
                PMS.ArabicDescription = dt1.Rows[i + 1]["ArabicDescription"].ToString();
                lmp.Add(PMS);
            }
        }
        else
        {
            PMS.ArabicName = "";
            PMS.ArabicDescription = "";
            lmp.Add(PMS);
        }

        return lmp;

    }

    public static ParentCategory[] BindCategoryProductPage()
    {
        DataTable dt = new DataTable();
        categoryManager objcategory = new categoryManager();
        List<ParentCategory> details = new List<ParentCategory>();
        dt = objcategory.GetParentCategoryProductPage();

        foreach (DataRow dtrow in dt.Rows)
        {
            ParentCategory country = new ParentCategory();
            country.categoryId = Convert.ToInt32(dtrow["CountryId"].ToString());
            country.categoryName = dtrow["CountryName"].ToString();
            details.Add(country);
        }

        return details.ToArray();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetAutoCompleteCategory()
    {
        JavaScriptSerializer jsonscript = new JavaScriptSerializer();
        categoryManager objcategory = new categoryManager();
        try
        {
            DataTable dt = new DataTable();
            dt = objcategory.GetParentCategoryProductPage();
            List<string> tags = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tags.Add(Convert.ToString(dt.Rows[i]["Name"]));
            }
            return jsonscript.Serialize(tags);
        }
        catch (Exception ex) { throw ex; }
        finally { objcategory = null; }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<productManager> SKUProductNameBind()
    {

        productManager objProduct = new productManager();
        List<productManager> lmp = new List<productManager>();
        DataTable dt = new DataTable();
        dt = objProduct.SKUBind();
        foreach (DataRow dr in dt.Rows)
        {
            productManager PMS = new productManager();
            PMS.productId = Convert.ToInt32(dr["productid"].ToString());
            PMS.productName = dr["productname"].ToString();
            PMS.sku = dr["sku"].ToString();
            lmp.Add(PMS);

        }
        return lmp;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<productManager> SKUBind()
    {
        productManager objProduct = new productManager();
        List<productManager> lmp = new List<productManager>();
        DataTable dt = new DataTable();
        dt = objProduct.SKUBind();
        foreach (DataRow dr in dt.Rows)
        {
            productManager PMS = new productManager();
            PMS.productId = Convert.ToInt32(dr["productid"].ToString());
            PMS.sku = dr["sku"].ToString();
            lmp.Add(PMS);
        }
        return lmp;
    }

    public class ParentCategory
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<productManager> MasterproductBind(string sku)
    {
        productManager objProduct = new productManager();
        objProduct.sku = Convert.ToString(sku);
        List<productManager> MPN = new List<productManager>();
        DataTable dt = new DataTable();
        dt = objProduct.MasterproductBind();
        foreach (DataRow dr in dt.Rows)
        {
            productManager PM = new productManager();
            PM.productId = Convert.ToInt32(dr["productid"].ToString());
            PM.productName = dr["productname"].ToString();
            PM.sku = dr["sku"].ToString();
            MPN.Add(PM);
        }

        return MPN;
    }


    // add user type userrole page
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string addUserType(string typeName, string isactive)
    {
        usertypeManager objUserType = new usertypeManager();
        string sortor = Convert.ToString(CommonFunctions.GetLastSortCount("adminType", "sortorder"));
        if (typeName != "" && isactive != "")
        {
            objUserType.typeName = typeName;
            objUserType.sortorder = Convert.ToInt32(sortor);
            objUserType.isactive = Convert.ToByte(isactive);
            objUserType.InsertItem();

            return "success";
        }
        else
        {
            return "fail";
        }
    }


    // bind user role
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<usertypeManager> BindUserType()
    {
        usertypeManager objuser = new usertypeManager();
        List<usertypeManager> lmp = new List<usertypeManager>();
        DataTable dt = new DataTable();

        try
        {
            dt = objuser.GetAllUserType();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    usertypeManager PMS = new usertypeManager();
                    PMS.adminTypeId = Convert.ToInt32(dr["adminTypeId"].ToString());
                    PMS.typeName = dr["typeName"].ToString();
                    lmp.Add(PMS);
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

        return lmp;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<categoryManager> BindPopCategory()
    {

        // productManager objProduct = new productManager();
        categoryManager objCategory = new categoryManager();

        List<categoryManager> lmp = new List<categoryManager>();
        DataTable dt = new DataTable();
        //dt = objProduct.SKUBind();
        dt = objCategory.GetAllCategory();
        foreach (DataRow dr in dt.Rows)
        {
            categoryManager CMS = new categoryManager();
            CMS.categoryId = Convert.ToInt32(dr["categoryId"].ToString());
            CMS.categoryName = dr["categoryName"].ToString();
            lmp.Add(CMS);

        }
        return lmp;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<brandManager> BindPopBrand()
    {

        // productManager objProduct = new productManager();
        brandManager objBrand = new brandManager();
        List<brandManager> lmp = new List<brandManager>();
        DataTable dt = new DataTable();
        //dt = objProduct.SKUBind();
        dt = objBrand.GetBrandItem();
        foreach (DataRow dr in dt.Rows)
        {
            brandManager CMS = new brandManager();
            CMS.idbrand = Convert.ToInt32(dr["brandid"].ToString());
            CMS.brandName = dr["brandName"].ToString();
            lmp.Add(CMS);

        }
        return lmp;
    }


    // image upload drag and drop
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string Upload()
    {
        HttpContext postedContext = HttpContext.Current;
        HttpPostedFile file = postedContext.Request.Files[0];
        string name = file.FileName;
        byte[] binaryWriteArray = new
        byte[file.InputStream.Length];
        file.InputStream.Read(binaryWriteArray, 0,
        (int)file.InputStream.Length);
        FileStream objfilestream = new FileStream(Server.MapPath("uploads//" + name), FileMode.Create, FileAccess.ReadWrite);
        objfilestream.Write(binaryWriteArray, 0,
        binaryWriteArray.Length);
        objfilestream.Close();
        string[][] JaggedArray = new string[1][];
        JaggedArray[0] = new string[] { "File was uploaded successfully" };
        JavaScriptSerializer js = new JavaScriptSerializer();
        string strJSON = js.Serialize(JaggedArray);
        return strJSON;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> MasterproductBindSKU(string sku)
    {
        productManager objProduct = new productManager();
        objProduct.sku = Convert.ToString(sku);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objProduct.MasterproductBindSKU();

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(dr["sku"].ToString());

            }
        }
        else
        {
            MPN.Add("No SKU Found");
        }

        return MPN;
    }

    //Bind Master Product Name with Auto Suggestion Box
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> MasterproductName(string productname)
    {
        productManager objProduct = new productManager();
        objProduct.productName = Convert.ToString(productname);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objProduct.BindMasterProductName();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(dr["productname"].ToString());

            }
        }
        else
        {
            MPN.Add("No Product Name Found");
        }

        return MPN;
    }

    //Find SKU With Product Name
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<productManager> MasterproductNameBind(string productname)
    {
        productManager objProduct = new productManager();
        objProduct.productName = Convert.ToString(productname);
        List<productManager> MPN = new List<productManager>();
        DataTable dt = new DataTable();
        dt = objProduct.MasterproductBindWithName();
        foreach (DataRow dr in dt.Rows)
        {
            productManager PM = new productManager();
            PM.productId = Convert.ToInt32(dr["productid"].ToString());
            PM.productName = dr["productname"].ToString();
            PM.sku = dr["sku"].ToString();
            MPN.Add(PM);
        }

        return MPN;
    }

    //get address longitude and letitude
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string FindLongitudeLetitude(string Address)
    {
        string address = Address.Replace("coma1", ",");
        string url = "http://maps.google.com/maps/api/geocode/xml?address=" + address + "&sensor=false";
        WebResponse response1 = default(WebResponse);
        WebRequest request = WebRequest.Create(new Uri(url.Replace(' ', '+')));
        response1 = request.GetResponse();
        XmlTextReader xmlResponse = new XmlTextReader(response1.GetResponseStream());
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(xmlResponse);
        XmlNode status = xmldoc.SelectSingleNode("GeocodeResponse/status");

        string lat = "";
        string lang = "";

        if (status.InnerText.ToLower() == "ok")
        {
            XmlNode LattNode = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lat");
            XmlNode LongNode = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lng");
            if (string.IsNullOrEmpty(LongNode.InnerText.Trim()) | LongNode.InnerText.Trim() == "-")
            {
                lang = "0";
            }
            else
            {
                lang = LongNode.InnerText.Trim();
            }

            if (string.IsNullOrEmpty(LattNode.InnerText.Trim()))
            {
                lat = "0";
            }
            else
            {
                lat = LattNode.InnerText.Trim();
            }
        }
        else
        {
            lang = "0";
            lat = "0";
        }
        return Convert.ToDecimal(lat) + "," + Convert.ToDecimal(lang);
    }

    //delete product image

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string DeleteProductImage(string productimageid)
    {
        productManager objpro = new productManager();
        if (productimageid != "0" || productimageid == "")
        {
            objpro.productImagesId = Convert.ToInt32(productimageid);
            string strImg = objpro.getProductImageName();
            if (strImg != "")
            {
                string fullimagepath = string.Empty;
                string thumbimagepath = string.Empty;
                string mediumimagepath = string.Empty;
                string thumbrectimagepath = string.Empty;

                fullimagepath = Server.MapPath(AppSettings.PRODUCT_ACTULE_ROOTURL + strImg);
                thumbimagepath = Server.MapPath(AppSettings.PRODUCT_THUMB_ROOTURL + strImg);
                mediumimagepath = Server.MapPath(AppSettings.PRODUCT_MEDIUM_ROOTURL + strImg);
                thumbrectimagepath = Server.MapPath(AppSettings.PRODUCT_THUMBRECT_ROOTURL + strImg);

                CommonFunctions.DeleteFile(fullimagepath);
                CommonFunctions.DeleteFile(thumbimagepath);
                CommonFunctions.DeleteFile(mediumimagepath);
                CommonFunctions.DeleteFile(thumbrectimagepath);
            }

            objpro.DeleteProductImageByProductImageId();

            return "success";
        }
        else
        {
            return "fail";
        }

    }

    // bind all the sku
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> BindAllSKU(string sku)
    {
        productManager objProduct = new productManager();
        objProduct.sku = Convert.ToString(sku);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objProduct.BindAllSKU();

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(dr["sku"].ToString());

            }
        }
        else
        {
            MPN.Add("No SKU Found");
        }

        return MPN;
    }

    //check sku is already exist
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string SkuIsExist(string prodid, string skuname)
    {
        productManager objproduct = new productManager();
        if (skuname != "")
        {
            objproduct.productId = Convert.ToInt32(prodid);
            objproduct.sku = Convert.ToString(skuname);
            string checkexist = string.Empty;
            checkexist = objproduct.SkuIsExist();

            return checkexist;
        }
        else
        {
            return "fail";
        }
    }

    // 
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> BindCustomerName(string companyName)
    {
        customerManager objCustomer = new customerManager();
        objCustomer.companyName = Convert.ToString(companyName);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        try
        {
            dt = objCustomer.BindCustomerName();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MPN.Add(dr["companyName"].ToString());
                }
            }
            else
            {
                MPN.Add("No record found");
            }

            return MPN;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { dt.Dispose(); objCustomer = null; }

    }

    // Search Order id
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> searchOrderId(string orderid, string contactname)
    {
        orderManager objProduct = new orderManager();
        objProduct.orderid = Convert.ToInt32(orderid);
        objProduct.contactName = Convert.ToString(contactname);

        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objProduct.SearchOrderId();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(dr["orderid"].ToString());
            }
        }
        else
        {
            MPN.Add("No order Found");
        }

        return MPN;
    }

    // customer bind from order id
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<orderManager> FindCustomerFromOrderID(string orderid)
    {
        orderManager objProduct = new orderManager();
        objProduct.orderid = Convert.ToInt32(orderid);
        List<orderManager> MPN = new List<orderManager>();
        DataTable dt = new DataTable();
        dt = objProduct.FindCustomerFromOrderID();

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                orderManager PM = new orderManager();
                PM.orderid = Convert.ToInt32(dr["orderid"].ToString());
                PM.customerid = Convert.ToInt32(dr["customerid"].ToString());
                PM.contactName = Convert.ToString(dr["contactName"].ToString());
                PM.totalammount = Convert.ToDecimal(dr["totalammount"].ToString());
                MPN.Add(PM);
            }
        }


        return MPN;
    }

    // Search Customer name
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> searchCustomerName(string contactName)
    {
        orderManager objProduct = new orderManager();
        objProduct.contactName = Convert.ToString(contactName);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objProduct.searchCustomerName();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(dr["contactName"].ToString());
            }
        }
        else
        {
            MPN.Add("No Customer Found");
        }
        return MPN;
    }

    // customer bind from order id
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<orderManager> FindOrderidFromCustomername(string contactName)
    {
        orderManager objProduct = new orderManager();
        objProduct.contactName = Convert.ToString(contactName);
        List<orderManager> MPN = new List<orderManager>();
        DataTable dt = new DataTable();
        dt = objProduct.FindOrderidFromCustomername();

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                orderManager PM = new orderManager();
                PM.orderid = Convert.ToInt32(dr["orderid"].ToString());
                PM.customerid = Convert.ToInt32(dr["customerid"].ToString());
                MPN.Add(PM);
            }
        }

        return MPN;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<paymentManager> BindPaymentData(string id)
    {
        paymentManager objProduct = new paymentManager();
        objProduct.paymentid = Convert.ToInt32(id);
        List<paymentManager> lmp = new List<paymentManager>();
        DataTable dt = new DataTable();
        dt = objProduct.GetSinglePaymentRecord();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                paymentManager PMS = new paymentManager();
                PMS.paymentid = Convert.ToInt32(dr["paymentid"].ToString());
                PMS.orderid = Convert.ToInt32(dr["orderid"].ToString());
                PMS.payammount = Convert.ToDecimal(dr["payammount"].ToString());
                PMS.paynotes = dr["paynotes"].ToString();
                PMS.contactName = dr["contactName"].ToString();
                lmp.Add(PMS);

            }
        }
        return lmp;

    }

    /// <summary>
    /// get creditlimits
    /// </summary>
    /// <param name="prodid"></param>
    /// <param name="skuname"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCustomerCreditFromName(string companyName)
    {
        customerManager objcustomer = new customerManager();
        try
        {
            if (companyName != "")
            {
                objcustomer.contactName = Convert.ToString(companyName);
                objcustomer.companyName = Convert.ToString(companyName);
                string Credits = string.Empty;
                Credits = objcustomer.GetCustomerCreditFromName();
                return Credits;
            }
            else
            {
                return "fail";
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
        finally { objcustomer = null; }

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<dashbordManager> GetMounthlyRecord()
    {
        dashbordManager objProduct = new dashbordManager();

        List<dashbordManager> lmp = new List<dashbordManager>();
        DataTable dt = new DataTable();
        dt = objProduct.GetMounthlyRecord();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dashbordManager PMS = new dashbordManager();
                PMS.Years = Convert.ToInt32(dr["Years"].ToString());
                PMS.Months = Convert.ToInt32(dr["Months"].ToString());
                PMS.Sales = Convert.ToDecimal(dr["Sales"].ToString());
                lmp.Add(PMS);
            }
        }
        return lmp;

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string UpdateCustomerCreditsLimits(string customerid, string Credits)
    {
        customerManager objcust = new customerManager();
        if (customerid != "" && Credits != "")
        {
            objcust.creditLimit = Convert.ToString(Credits);
            objcust.customerId = Convert.ToInt32(customerid);
            objcust.UpdateCustomerCreditsLimits();
            return "Credit limits update successfully.";
        }
        else
        {
            return "Fail";
        }
    }

    /// <summary>
    /// Search keyword for view product page
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> KeyWordAutoPopuletSearchProduct(string SearchKey)
    {
        productManager objProduct = new productManager();
        objProduct.SearchKey = Convert.ToString(SearchKey);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objProduct.KeyWordAutoPopuletSearchProduct();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(Server.HtmlDecode(dr["SearchKey"].ToString()));
            }
        }
        else
        {
            MPN.Add("No record found");
        }
        return MPN;
    }

    /// <summary>
    /// Search keyword for view master product page
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> KeyWordAutoPopuletSearchMasterProduct(string SearchKey)
    {
        productManager objProduct = new productManager();
        objProduct.SearchKey = Convert.ToString(SearchKey);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objProduct.KeyWordAutoPopuletSearchMasterProduct();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(Server.HtmlDecode(dr["SearchKey"].ToString()));
            }
        }
        else
        {
            MPN.Add("No record found");
        }
        return MPN;
    }

    /// <summary>
    /// Search keyword for view customer page
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> SearchKeywordCustomer(string SearchKey)
    {
        customerManager objcust = new customerManager();
        objcust.SearchKey = Convert.ToString(SearchKey);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objcust.SearchKeywordCustomer();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(Server.HtmlDecode(dr["SearchKey"].ToString()));
            }
        }
        else
        {
            MPN.Add("No record found");
        }
        return MPN;
    }

    /// <summary>
    /// Search keyword for view payment page
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> SearchKeywordPayment(string SearchKey)
    {
        paymentManager objpay = new paymentManager();
        objpay.SearchKey = Convert.ToString(SearchKey);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objpay.SearchKeywordPayment();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(Server.HtmlDecode(dr["SearchKey"].ToString()));
            }
        }
        else
        {
            MPN.Add("No record found");
        }
        return MPN;
    }
    /// <summary>
    /// Search keyword for view manage user page
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> SearchKeywordManageUser(string SearchKey)
    {
        AdminManager objpay = new AdminManager();
        objpay.SearchKey = Convert.ToString(SearchKey);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objpay.SearchKeywordManageUser();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(Server.HtmlDecode(dr["SearchKey"].ToString()));
            }
        }
        else
        {
            MPN.Add("No record found");
        }
        return MPN;
    }

    /// <summary>
    /// Search keyword for view manage user page
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> SearchKeywordImportJobs(string SearchKey)
    {
        importjobManager objimp = new importjobManager();
        objimp.SearchKey = Convert.ToString(SearchKey);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objimp.SearchKeywordImportJobs();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(Server.HtmlDecode(dr["SearchKey"].ToString()));
            }
        }
        else
        {
            MPN.Add("No record found");
        }
        return MPN;
    }

    /// <summary>
    /// Search keyword for view order page
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> SearchKeywordOrderid(string SearchKey)
    {
        orderManager objimp = new orderManager();
        objimp.orderid = Convert.ToInt32(SearchKey);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objimp.SearchKeywordOrderid();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(Server.HtmlDecode(dr["orderid"].ToString()));
            }
        }
        else
        {
            MPN.Add("No record found");
        }
        return MPN;
    }

    /// <summary>
    /// Search keyword for add order page
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<string> KeyWordAutoPopuletProductName(string SearchKey)
    {
        productManager objpro = new productManager();
        objpro.productName = Convert.ToString(SearchKey);
        List<string> MPN = new List<string>();
        DataTable dt = new DataTable();
        dt = objpro.KeyWordAutoPopuletProductName();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MPN.Add(Server.HtmlDecode(dr["productname"].ToString()));
            }
        }
        else
        {
            MPN.Add("No record found");
        }
        return MPN;
    }

    /// <summary>
    /// get the product id from sku
    /// </summary>
    /// <param name="contactName"></param>
    /// <returns></returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int GetProdutctidCount(string skuname)
    {
        productManager objpro = new productManager();
        objpro.sku = skuname;
        int Productid = Convert.ToInt32(objpro.GetProdutctidCount());
        return Productid;
    }

    //delete product image

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string UpdateProductImageSetMain(string productimageid)
    {
        productManager objpro = new productManager();
        int ids = 0;
        if (productimageid != "0" || productimageid == "")
        {
            objpro.productImagesId = Convert.ToInt32(productimageid);
            ids = objpro.getProductidFromProductimageId();
            if (ids != 0)
            {
                objpro.productId = ids;
                objpro.mainImage = Convert.ToChar("N");
                objpro.UpdateProductImageSetMainByProductid();
            }
            objpro.mainImage = Convert.ToChar("M");
            objpro.UpdateProductImageByProductImageIdSetMain();
            int count = objpro.getImagesCountByProductID();

            return Convert.ToString(count);
        }
        else
        {
            return "fail";
        }
    }


}
