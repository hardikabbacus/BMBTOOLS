<%@ WebHandler Language="C#" Class="ImagesHandler" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
public class ImagesHandler : IHttpHandler
{

    productManager objProduct = new productManager();
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request.Files.Count > 0)
        {
            HttpFileCollection hfc = context.Request.Files;
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile postedfile = context.Request.Files[i];
                if (postedfile.ContentLength > 0)
                {
                    HttpPostedFile PostedFile = hfc[i];
                    string fileactualname = PostedFile.FileName;

                    if (Convert.ToInt32(context.Request.QueryString["id"]) != 0)
                    {
                        objProduct.productId = Convert.ToInt32(context.Request.QueryString["id"]);
                        //objProduct.DeleteProductImage();
                    }
                    else
                    {
                        int maxID = objProduct.getmaxid();
                        objProduct.productId = maxID;
                    }

                    //int maxID = objProduct.getmaxid();
                    //objProduct.productId = maxID;

                    string sku = objProduct.GetSkuFromProductid();
                    objProduct.isactive = 1;
                    //objProduct.InsertProductImageItem();
                    //int prodimageID = objProduct.GetmaximageProductId();


                    string actualfolder = string.Empty;
                    string thumbfolder = string.Empty;
                    string midiumfolder = string.Empty;


                    actualfolder = System.Web.HttpContext.Current.Server.MapPath("../" + AppSettings.PRODUCT_ACTULE_ROOTURL);
                    thumbfolder = System.Web.HttpContext.Current.Server.MapPath("../" + AppSettings.PRODUCT_THUMB_ROOTURL);
                    midiumfolder = System.Web.HttpContext.Current.Server.MapPath("../" + AppSettings.PRODUCT_MEDIUM_ROOTURL);


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

                    string newThumbSku = System.Web.HttpContext.Current.Server.MapPath("../resources/product/thumb/") + sku + Path.GetExtension(Path.GetFileName(PostedFile.FileName));
                    if (!File.Exists(newThumbSku))
                    {
                        filename = sku + Path.GetExtension(Path.GetFileName(PostedFile.FileName));
                    }
                    else
                    {
                        char[] imgArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
                        for (int fl = 0; fl <= imgArray.Length; fl++)
                        {
                            string newThumb = System.Web.HttpContext.Current.Server.MapPath("../resources/product/thumb/") + sku + imgArray[fl] + Path.GetExtension(Path.GetFileName(PostedFile.FileName));
                            if (!File.Exists(newThumb))
                            {
                                filename = sku + imgArray[fl] + Path.GetExtension(Path.GetFileName(PostedFile.FileName));
                                break;
                            }
                        }
                    }
                    //filename = prodimageID + Path.GetExtension(Path.GetFileName(PostedFile.FileName));

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
                    PostedFile.SaveAs(fullimagepath);

                    //generate thumb
                    CommonFunctions.Thmbimages(fullimagepath, thumbfolder, filename, Convert.ToInt32(AppSettings.PRODUCT_THUMB_WIDTH), Convert.ToInt32(AppSettings.PRODUCT_THUMB_HEIGHT), 0);
                    CommonFunctions.Thmbimages(fullimagepath, midiumfolder, filename, Convert.ToInt32(AppSettings.PRODUCT_MEDIUM_WIDTH), Convert.ToInt32(AppSettings.PRODUCT_MEDIUM_HEIGHT), 0);

                    //objProduct.productImagesId = prodimageID;
                    objProduct.actualImageName = fileactualname;
                    objProduct.imageName = filename;
                    string pr_name = objProduct.GetProductNameByProductid();
                    objProduct.imgLabel = pr_name;
                    //objProduct.UpdateImage();

                    objProduct.sortOrder = 0;
                    objProduct.InsertDuplicateProductImageItem();

                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Please Select Files");

                }
            }
        }
        else
        {
            context.Response.Write("<script type='text/javascript'>alert('Please Select file ');</script>");

        }
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}