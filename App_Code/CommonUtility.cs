using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using System.Xml;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
/// <summary>
/// Summary description for CommonUtility
/// </summary>


public class CommonUtility
{
    public CommonUtility()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}
public static class AppSettings
{
    // COMMON FOR ADMIN AND SITE
    public static readonly string FROMEMAILADDRESS = "BMB<>";
    public static readonly string ADMIN_PAGE_TITLE = "BMB";
    public static readonly string FRONT_PAGE_TITLE = "- BMB";
    public static readonly string MANDATORY_MSG = "<span class='mandatorymsg'>* = Mandatory Fields</span>";
    public static readonly string CURRENCY_SIGN = "£";
    public static readonly string CURRENCY_CODE = "GBP";
    public static readonly string CURRENCY_FORMAT = "en-GB";
    public static readonly string SITE_ROOTPATH = HttpContext.Current.Server.MapPath("~/Admin");
    public static readonly string RESOURCE_ROOTPATH = HttpContext.Current.Server.MapPath("~/resources/");
    public static readonly string MAIN_SITE_ROOTPATH = HttpContext.Current.Server.MapPath("~/");
    public static readonly string ADMIN_ROOTURL = "admin/";
    public static readonly string ADMIN_ROOTPATH = HttpContext.Current.Server.MapPath("~/Admin");
    public static readonly string DisplayStatus_Active = "Active";
    public static readonly string DisplayStatus_Inactive = "Inactive";
    public static readonly int ADMIN_PAGESIZE = 40;
    public static readonly string CmsBanners_ROOTURL = "resources/CmsBanners/";
    public static readonly string CmsBannersContent_ROOTURL = "resources/CmsBannersContent/";
    public static readonly string PortFolio_ROOTURL = "resources/PortFolio/";
    public static readonly string PortFolioGallery_ROOTURL = "resources/PortFolioGallery/";
    public static readonly string Press_ROOTURL = "resources/Press/";
    public static readonly string Video_ROOTURL = "resources/VideoImage/";
    public static readonly string VideoFile_ROOTURL = "resources/Videofile/";
    public static readonly string PortfolioFile_ROOTURL = "resources/PortFolioVideo/";
    public static readonly string PressGallery_ROOTURL = "resources/PressGallery/";


    public static readonly string StatusMessage = "Status updated sucessfully.";
    public static readonly string ADMIN_MENU_ROOTURL = "resources/AdminMenu/";
    public static readonly string ADMIN_TYPE_MENU_ROOTURL = "resources/AdminTypeMenu/";
    public static readonly string BANNER_ROOTURL = "resources/Banner/";
    public static readonly string TESTIMONIAL_ROOTURL = "resources/Testimonial/";
    public static readonly string Amenities_Icon_Path = "/resources/amenities/";
    public static readonly string No_image = "/resources/Noimage.jpg";
    public static readonly string SITEURL = ConfigurationManager.AppSettings["serverurl_2015"];
    public static readonly string NEWSITEURL = "http://localhost:8260/";
    //public static readonly string NEWSITEURL = ConfigurationManager.AppSettings["NewServerURL"];
    //public static readonly string SITEURL = "http://champalimaudcollection.com.192-168-0-11.wests-serve.com/";
    public static readonly int PAGESIZE = 25;
    public static readonly int PAGESIZEMINIMUM = 5;
    public static readonly int PAGESIZELIMIT = 100;
    public static readonly int PAGESIZEINTERVAL = 5;

    public static readonly int FRONT_PAGESIZE = 10;

    public static readonly int CSS_JS_VERSION = 1;
    public static readonly int ADMIN_TYPE = 1;

    //for amenity
    public static readonly int Building_amenities = 13;
    public static readonly int House_amenities = 12;

    // MAIL CONFIGURATION

    public static readonly string SMTP = "mail.webtechsystem.com";
    public static readonly string SMTP_USER = "mark@webtechsystem.com";
    public static readonly string SMTP_PASS = "5UGjGAU77iaCKGIE@";

    public static readonly string FORGOTPASSWORD_ADMIN_CCEMAIL = "hardik@webtechsystem.com";
    public static readonly string FORGOTPASSWORD_ADMIN_BCCEMAIL = "hardik@webtechsystem.com";
    public static readonly string AdminEmail = "hardik@webtechsystem.com";


    // SQL COMMON VARRIABLE
    public static readonly int STOREDPROCEDURE_COMMANDTIMEOUT = 6000;
    public static readonly string OWNERNAME = "[dbo]."; // LOCAL
    //public static readonly string OWNERNAME = "[dbo]."; //LIVE






    // RESOURCES PATH


    public static readonly string FFMPEGPATH = "ffmpeg\\ffmpeg.exe";
    public static readonly string TEMPBANNERVIDEO_ROOTURL = "resources/HomeBanner/TempVideo/";
    public static readonly string TEMPBANNERVIDEO_ROOTPATH = "resources\\HomeBanner\\TempVideo\\";

    public static readonly string BANNERVIDEO_ACTULE_ROOTURL = "resources/HomeBanner/CustomVideo/";
    public static readonly string BANNERVIDEO_ACTULE_ROOTPATH = "resources\\HomeBanner\\CustomVideo\\";
    public static readonly int BANNERVIDEO_MAX_SIZE = 20971520;

    public static readonly int FLVVIDEOBITRATE = 500;
    public static readonly int FLVAUDIOBITRATE = 64;
    public static readonly int FLVSAMPLINGRATE = 22050;
    public static readonly string FLVMIDDURATION = "00:00:01";


    public static readonly string NOIMG_ACTULE_ROOTURL = "resources/NOIMG/full/";
    public static readonly string LOGO_ACTULE_ROOTURL = "resources/LOGO/full/";
    public static readonly string CATEGORY_ACTULE_ROOTURL = "resources/category/full/";
    public static readonly string CATEGORY_THUMB_ROOTURL = "resources/category/thumb/";
    public static readonly string CATEGORY_MEDIUM_ROOTURL = "resources/category/medium/";
    public static readonly string CATEGORY_THUMBRECT_ROOTURL = "resources/category/thumbrect/";

    public static readonly int CATEGORY_THUMB_WIDTH = 490; //235
    public static readonly int CATEGORY_THUMB_HEIGHT = 490;
    public static readonly int CATEGORY_MEDIUM_WIDTH = 235;
    public static readonly int CATEGORY_MEDIUM_HEIGHT = 235;
    public static readonly int CATEGORY_THUMBRECT_WIDTH = 490;
    public static readonly int CATEGORY_THUMBRECT_HEIGHT = 235;

    public static readonly string BRAND_ACTULE_ROOTURL = "resources/brand/full/";
    public static readonly string BRAND_THUMB_ROOTURL = "resources/brand/thumb/";
    public static readonly string BRAND_THUMBRECT_ROOTURL = "resources/brand/thumbrect/";
    public static readonly string BRAND_MEDIUM_ROOTURL = "resources/brand/medium/";

    public static readonly int BRAND_MEDIUM_WIDTH = 235;
    public static readonly int BRAND_MEDIUM_HEIGHT = 235;
    public static readonly int BRAND_THUMB_WIDTH = 490; //235
    public static readonly int BRAND_THUMB_HEIGHT = 490;
    public static readonly int BRAND_THUMBRECT_WIDTH = 490;
    public static readonly int BRAND_THUMBRECT_HEIGHT = 235;

    public static readonly string PRODUCT_ACTULE_ROOTURL = "resources/product/full/";
    public static readonly string PRODUCT_THUMB_ROOTURL = "resources/product/thumb/";
    public static readonly string PRODUCT_MEDIUM_ROOTURL = "resources/product/medium/";
    public static readonly string PRODUCT_THUMBRECT_ROOTURL = "resources/product/thumbrect/";

    public static readonly int PRODUCT_THUMB_WIDTH = 490; //235
    public static readonly int PRODUCT_THUMB_HEIGHT = 490;
    public static readonly int PRODUCT_MEDIUM_WIDTH = 235;
    public static readonly int PRODUCT_MEDIUM_HEIGHT = 235;
    public static readonly int PRODUCT_THUMBRECT_WIDTH = 490;
    public static readonly int PRODUCT_THUMBRECT_HEIGHT = 235;

    public static readonly string COMPANYLOGO_ACTULE_ROOTURL = "resources/companylogo/full/";
    public static readonly string COMPANYLOGO_THUMB_ROOTURL = "resources/companylogo/thumb/";
    public static readonly int COMPANYLOGO_THUMB_WIDTH = 300; //235
    public static readonly int COMPANYLOGO_THUMB_HEIGHT = 300;

    public static readonly string QRCODE_ROOTURL = "resources/QRCode/";


}

#region Enum

public enum ActiveStatus
{
    Active = 'A',
    DeActive = 'D'
}

public enum LikeUnlike
{
    Like = 'L',
    Unlike = 'U'
}

#endregion



