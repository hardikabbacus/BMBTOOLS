using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.EnterpriseServices;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for GlobalParameters
/// </summary>
public class GlobalParams
{
    #region "Contructor"
    public GlobalParams() { }
    #endregion

    #region "Public Variables"
    public static readonly string REQMSG = ConfigurationManager.AppSettings["REQMSG"];
    public static readonly string ADMIN_PAGGING = ConfigurationManager.AppSettings["AdminPagging"];
    public static readonly string ADMIN_PAGE_TITLE = ConfigurationManager.AppSettings["AminPageTitle"];

    public static readonly string DEFAULT_COUNTRY = ConfigurationManager.AppSettings["DEFAULT_COUNTRY"];
    public static readonly string DEFAULT_STATE = ConfigurationManager.AppSettings["DEFAULT_STATE"];

    public static readonly string PRICE_PARAMETERID = ConfigurationManager.AppSettings["PriceParameterId"];
    public static readonly string TOPPINGS_PARAMETERID = ConfigurationManager.AppSettings["TopingsParameterId"];
    public static readonly string REWARD_PARAMETERID = ConfigurationManager.AppSettings["RewardPointId"];
    public static readonly string FROM_EMAILID = ConfigurationManager.AppSettings["FromEmailID"];
    public static readonly string To_EMAILID = ConfigurationManager.AppSettings["ToEmailID"];
    public static readonly string ControlPanel_MenuOffer_Id = ConfigurationManager.AppSettings["ControlPanelMenuOfferId"];
    public static readonly string ControlPanel_MenuOfferDependent_Ids = ConfigurationManager.AppSettings["ControlPanelMenuOfferDependentIds"];
    public static readonly string MAX_IMAGE_SIZE = ConfigurationManager.AppSettings["MAX_IMAGE_SIZE"];

    public static readonly int TESTIMONIAL_PAGE_SIZE = 10;
    public static readonly int GALLERY_PAGE_SIZE = 15;
    public static readonly int LOCATION_PAGE_SIZE = 5;

    public static readonly int Question_Show_Days = 30;

    public static readonly int Question_Image_size = 10485760;
    public static readonly int Question_Video_size = 10500000;
    public static readonly int Question_Audio_size = 10498677;

    public static readonly int Profile_Image_size = 1048576;
    public static readonly string Registrationage = "16";
    public static readonly int termconditionid = 1;
    public static readonly int cookiepolicy = 5;
    
    #endregion

    #region "Public Variables"
    public static readonly string CurrentRestaurantID = ConfigurationManager.AppSettings["CurrentRestaurantID"];
    public static readonly string AdminSite_URL = ConfigurationManager.AppSettings["ADMIN_SITE_URL"];
    public static readonly string Demo_Path = ConfigurationManager.AppSettings["Demo_Path"];
    public static readonly string CurrentSiteName = ConfigurationManager.AppSettings["CurrentSiteName"];
    public static readonly string AboutUsID = ConfigurationManager.AppSettings["AboutUsID"];
    public static readonly string MenuID = ConfigurationManager.AppSettings["MenuID"];
    public static readonly string OfferCatID = ConfigurationManager.AppSettings["OfferCatId"];
    #endregion

  
    #region "Public Enums"

    public enum Notificationtype
    {
        Question = 1,
        FriendReq=2,
        Answer = 3,
        Comment=4
    }
    public enum UserType
    {
        Personal = 1,
        Business = 2
    }
    public enum QuestionType
    {
        Phrase = 1,
        image = 2,
        video = 3,
        audio=4
    }
    public enum AskType
    {
        Public = 1,
        Friend = 2,
        Group = 3
    }
    public enum Gender
    {
        Male = 1,
        Female = 2,
        All = 3
    }
    public enum ProductType
    {
        Fix = 1,
        Template = 2
    }
    public enum ProductShape
    {
        Circle = 1,
        Square = 2,
        Rectangle =3
    }

    public enum OrderStatus
    {
        Completed = 1,
        Pending = 2,
        Cancel = 3,
        Dispached = 4,
    }

    public enum PaymentType
    {
        Paypal = 1,
        CreditCard = 2
    }



    public static int getContentId(string contentName)
    {
        return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[contentName].ToString());
    }
    #endregion

    #region "Public Methods"
    public static string CalculateSize(int maximumSize)
    {
        int counter = 0;
        string result = "";

        while (maximumSize > 1023)
        {
            maximumSize = maximumSize / 1024;
            counter = counter + 1;
        }

        if (counter == 0)
        {
            result = maximumSize + " Bytes";
        }
        else if (counter == 1)
        {
            result = maximumSize + " KB";
        }
        else if (counter == 2)
        {
            result = maximumSize + " MB";
        }
        return result;
    }

    public static string getFullURL(string path, string filename)
    {
        string ServerName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
        string FinalUrl = "";
        string filepath = "";
        filepath = HttpContext.Current.Server.MapPath(path + "/" + filename);
        if (File.Exists(filepath))
        {
            if (ServerName.ToLower() == "localhost" || ServerName.ToLower() == "comp127")
            {
                FinalUrl = "http://" + ServerName.ToLower() + "/TakeEatEasy/" + path + "/" + filename;
            }
            else
            {
                FinalUrl = "http://" + ServerName.ToLower() + "/" + path + "/" + filename;
            }
        }
        return FinalUrl;
    }

    //check for abuse word
    public static string checkprofanity(string strmsg)
    {

        ArrayList restrictedword = new ArrayList();
        restrictedword.Add("ass");
        restrictedword.Add("ass lick");
        restrictedword.Add("asses");
        restrictedword.Add("asshole");
        restrictedword.Add("assholes");
        restrictedword.Add("asskisser");
        restrictedword.Add("asswipe");
        restrictedword.Add("balls");
        restrictedword.Add("bastard");
        restrictedword.Add("beastial");
        restrictedword.Add("beastiality");
        restrictedword.Add("beastility");
        restrictedword.Add("beaver");
        restrictedword.Add("belly whacker");
        restrictedword.Add("bestial");
        restrictedword.Add("bestiality");
        restrictedword.Add("bitch");
        restrictedword.Add("biatch");
        restrictedword.Add("bitcher");
        restrictedword.Add("bitchers");
        restrictedword.Add("bitches");
        restrictedword.Add("bitchin");
        restrictedword.Add("bitching");
        restrictedword.Add("blow job");
        restrictedword.Add("blowjob");
        restrictedword.Add("blowjobs");
        restrictedword.Add("bonehead");
        restrictedword.Add("boner");
        restrictedword.Add("brown eye");
        restrictedword.Add("browneye");
        restrictedword.Add("browntown");
        restrictedword.Add("bucket cunt");
        restrictedword.Add("bull shit");
        restrictedword.Add("bullshit");
        restrictedword.Add("bum");
        restrictedword.Add("bung hole");
        restrictedword.Add("butch");
        restrictedword.Add("butt");
        restrictedword.Add("butt breath");
        restrictedword.Add("butt fucker");
        restrictedword.Add("butt hair");
        restrictedword.Add("buttface");
        restrictedword.Add("buttfuck");
        restrictedword.Add("buttfucker");
        restrictedword.Add("butthead");
        restrictedword.Add("butthole");
        restrictedword.Add("buttpicker");
        restrictedword.Add("chink");
        restrictedword.Add("circle jerk");
        restrictedword.Add("clam");
        restrictedword.Add("clit");
        restrictedword.Add("cobia");
        restrictedword.Add("cock");
        restrictedword.Add("cocks");
        restrictedword.Add("cocksuck");
        restrictedword.Add("cocksucked");
        restrictedword.Add("cocksucker");
        restrictedword.Add("cocksucking");
        restrictedword.Add("cocksucks");
        restrictedword.Add("coitus");
        restrictedword.Add("cooter");
        restrictedword.Add("crap");
        restrictedword.Add("cum");
        restrictedword.Add("cummer");
        restrictedword.Add("cumming");
        restrictedword.Add("cums");
        restrictedword.Add("cumshot");
        restrictedword.Add("cunilingus");
        restrictedword.Add("cunillingus");
        restrictedword.Add("cunnilingus");
        restrictedword.Add("cunt");
        restrictedword.Add("cuntlick");
        restrictedword.Add("cuntlicker");
        restrictedword.Add("cuntlicking");
        restrictedword.Add("cunts");
        restrictedword.Add("cyberfuc");
        restrictedword.Add("cyberfuck");
        restrictedword.Add("cyberfucked");
        restrictedword.Add("cyberfucker");
        restrictedword.Add("cyberfuckers");
        restrictedword.Add("cyberfucking");
        restrictedword.Add("damn");
        restrictedword.Add("dick");
        restrictedword.Add("dike");
        restrictedword.Add("dildo");
        restrictedword.Add("dildos");
        restrictedword.Add("dink");
        restrictedword.Add("dinks");
        restrictedword.Add("dipshit");
        restrictedword.Add("dong");
        restrictedword.Add("douche bag");
        restrictedword.Add("dumbass");
        restrictedword.Add("dyke");
        restrictedword.Add("ejaculate");
        restrictedword.Add("ejaculated");
        restrictedword.Add("ejaculates");
        restrictedword.Add("ejaculating");
        restrictedword.Add("ejaculatings");
        restrictedword.Add("ejaculation");
        restrictedword.Add("fag");
        restrictedword.Add("fagget");
        restrictedword.Add("fagging");
        restrictedword.Add("faggit");
        restrictedword.Add("faggot");
        restrictedword.Add("faggs");
        restrictedword.Add("fagot");
        restrictedword.Add("fagots");
        restrictedword.Add("fags");
        restrictedword.Add("fart");
        restrictedword.Add("farted");
        restrictedword.Add("farting");
        restrictedword.Add("fartings");
        restrictedword.Add("farts");
        restrictedword.Add("farty");
        restrictedword.Add("fatass");
        restrictedword.Add("fatso");
        restrictedword.Add("felatio");
        restrictedword.Add("fellatio");
        restrictedword.Add("fingerfuck");
        restrictedword.Add("fingerfucked");
        restrictedword.Add("fingerfucker");
        restrictedword.Add("fingerfuckers");
        restrictedword.Add("fingerfucking");
        restrictedword.Add("fingerfucks");
        restrictedword.Add("fistfuck");
        restrictedword.Add("fistfucked");
        restrictedword.Add("fistfucker");
        restrictedword.Add("fistfuckers");
        restrictedword.Add("fistfucking");
        restrictedword.Add("fistfuckings");
        restrictedword.Add("fistfucks");
        restrictedword.Add("fuck");
        restrictedword.Add("fucked");
        restrictedword.Add("fucker");
        restrictedword.Add("fuckers");
        restrictedword.Add("fuckin");
        restrictedword.Add("fucking");
        restrictedword.Add("fuckings");
        restrictedword.Add("fuckme");
        restrictedword.Add("fucks");
        restrictedword.Add("fuk");
        restrictedword.Add("fuks");
        restrictedword.Add("furburger");
        restrictedword.Add("gangbang");
        restrictedword.Add("gangbanged");
        restrictedword.Add("gangbangs");
        restrictedword.Add("gaysex");
        restrictedword.Add("gazongers");
        restrictedword.Add("goddamn");
        restrictedword.Add("gonads");
        restrictedword.Add("gook");
        restrictedword.Add("guinne");
        restrictedword.Add("hard on");
        restrictedword.Add("hardcoresex");
        restrictedword.Add("homo");
        restrictedword.Add("hooker");
        restrictedword.Add("horniest");
        restrictedword.Add("horny");
        restrictedword.Add("hotsex");
        restrictedword.Add("hussy");
        restrictedword.Add("jack off");
        restrictedword.Add("jackass");
        restrictedword.Add("jacking off");
        restrictedword.Add("jackoff");
        restrictedword.Add("jack-off");
        restrictedword.Add("jap");
        restrictedword.Add("jerk");
        restrictedword.Add("jerk-off");
        restrictedword.Add("jism");
        restrictedword.Add("jiz");
        restrictedword.Add("jizm");
        restrictedword.Add("jizz");
        restrictedword.Add("kike");
        restrictedword.Add("kock");
        restrictedword.Add("kondum");
        restrictedword.Add("kondums");
        restrictedword.Add("kraut");
        restrictedword.Add("kum");
        restrictedword.Add("kummer");
        restrictedword.Add("kumming");
        restrictedword.Add("kums");
        restrictedword.Add("kunilingus");
        restrictedword.Add("lesbian");
        restrictedword.Add("lesbo");
        restrictedword.Add("merde");
        restrictedword.Add("mick");
        restrictedword.Add("mothafuck");
        restrictedword.Add("mothafucka");
        restrictedword.Add("mothafuckas");
        restrictedword.Add("mothafuckaz");
        restrictedword.Add("mothafucked");
        restrictedword.Add("mothafucker");
        restrictedword.Add("mothafuckers");
        restrictedword.Add("mothafuckin");
        restrictedword.Add("mothafucking");
        restrictedword.Add("mothafuckings");
        restrictedword.Add("mothafucks");
        restrictedword.Add("motherfuck");
        restrictedword.Add("motherfucked");
        restrictedword.Add("motherfucker");
        restrictedword.Add("motherfuckers");
        restrictedword.Add("motherfuckin");
        restrictedword.Add("motherfucking");
        restrictedword.Add("motherfuckings");
        restrictedword.Add("motherfucks");
        restrictedword.Add("muff");
        restrictedword.Add("nigger");
        restrictedword.Add("niggers");
        restrictedword.Add("orgasim");
        restrictedword.Add("orgasims");
        restrictedword.Add("orgasm");
        restrictedword.Add("orgasms");
        restrictedword.Add("pecker");
        restrictedword.Add("penis");
        restrictedword.Add("phonesex");
        restrictedword.Add("phuk");
        restrictedword.Add("phuked");
        restrictedword.Add("phuking");
        restrictedword.Add("phukked");
        restrictedword.Add("phukking");
        restrictedword.Add("phuks");
        restrictedword.Add("phuq");
        restrictedword.Add("pimp");
        restrictedword.Add("piss");
        restrictedword.Add("pissed");
        restrictedword.Add("pissrr");
        restrictedword.Add("pissers");
        restrictedword.Add("pisses");
        restrictedword.Add("pissin");
        restrictedword.Add("pissing");
        restrictedword.Add("pissoff");
        restrictedword.Add("prick");
        restrictedword.Add("pricks");
        restrictedword.Add("pussies");
        restrictedword.Add("pussy");
        restrictedword.Add("pussys");
        restrictedword.Add("queer");
        restrictedword.Add("retard");
        restrictedword.Add("schlong");
        restrictedword.Add("screw");
        restrictedword.Add("sheister");
        restrictedword.Add("shit");
        restrictedword.Add("shited");
        restrictedword.Add("shitfull");
        restrictedword.Add("shiting");
        restrictedword.Add("shitings");
        restrictedword.Add("shits");
        restrictedword.Add("shitted");
        restrictedword.Add("shitter");
        restrictedword.Add("shitters");
        restrictedword.Add("shitting");
        restrictedword.Add("shittings");
        restrictedword.Add("shitty");
        restrictedword.Add("slag");
        restrictedword.Add("sleaze");
        restrictedword.Add("slut");
        restrictedword.Add("sluts");
        restrictedword.Add("smut");
        restrictedword.Add("snatch");
        restrictedword.Add("spunk");
        restrictedword.Add("twat");
        restrictedword.Add("wetback");
        restrictedword.Add("whore");
        restrictedword.Add("wop");
        string str = strmsg;
        // string strnew = "";
        string strreplace = "";
        //  string[] strsp = str.Split(' ');
        int i = 0;
        //  string chkword = "";

        foreach (string profinityword in restrictedword)
        {
            if (str.ToLower().Contains(profinityword))
            {
                strreplace = "";
                for (i = 0; i <= profinityword.Length - 1; i++)
                {
                    strreplace = strreplace + "*";
                }
                str = Regex.Replace(str, profinityword, strreplace, RegexOptions.IgnoreCase);
            }
        }


        //foreach (string myword in strsp)
        //{
        //    chkword = "";
        //    foreach (string profinityword in restrictedword)
        //    {
        //        if (profinityword == myword.ToLower())
        //        {
        //            chkword = "";
        //            for (i = 0; i <= profinityword.Length - 1; i++)
        //            {
        //                chkword = chkword + "*";
        //            }
        //            break;
        //        }
        //        else
        //        {
        //            chkword = myword;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(strnew))
        //    {
        //        strnew = strnew + " " + chkword;
        //    }
        //    else
        //    {
        //        strnew = chkword;
        //    }
        //}
        return str;
    }
    #endregion

    #region "Search message"
    public static readonly string QuestionSearchmessage = "No question available at the moment, please keep checking this space in future .";
    public static readonly string AdminSearchmessage = "No admin available at the moment, please keep checking this space in future .";
    public static readonly string AdminMenuSearchmessage = "No admin menu available at the moment, please keep checking this space in future .";
    public static readonly string AnswerSearchmessage = "No answer available at the moment, please keep checking this space in future .";
    public static readonly string CitySearchmessage = "No city available at the moment, please keep checking this space in future .";
    public static readonly string ContactUsSearchmessage = "No contact available at the moment, please keep checking this space in future .";
    public static readonly string FieldSearchmessage = "No field available at the moment, please keep checking this space in future .";
    public static readonly string FieldValueSearchmessage = "No field value available at the moment, please keep checking this space in future .";
    public static readonly string NewsletterSearchmessage = "No newsletter available at the moment, please keep checking this space in future .";
    public static readonly string CommentSearchmessage = "No comment available at the moment, please keep checking this space in future .";
    public static readonly string ContentSearchmessage = "No content available at the moment, please keep checking this space in future .";
    public static readonly string PortfolioSearchmessage = "No Portfolio available at the moment, please keep checking this space in future .";
    public static readonly string PortfolioGallerySearchmessage = "No PortfolioGallery available at the moment, please keep checking this space in future .";
    public static readonly string PressSearchmessage = "No Press available at the moment, please keep checking this space in future .";
    public static readonly string VidoeSearchmessage = "No Video available at the moment, please keep checking this space in future .";
    public static readonly string PressGallerySearchmessage = "No PressGallery available at the moment, please keep checking this space in future .";
    public static readonly string CountrySearchmessage = "No country available at the moment, please keep checking this space in future .";
    public static readonly string CustomerSearchmessage = "No customer available at the moment, please keep checking this space in future .";
    public static readonly string ReportSearchmessage = "No report available at the moment, please keep checking this space in future .";
    public static readonly string StateSearchmessage = "No State available at the moment, please keep checking this space in future .";
    public static readonly string ConfigurationSearchmessage = "No Configuration available at the moment, please keep checking this space in future .";
    public static readonly string HomeBannerSearchmessage = "No home banner available at the moment, please keep checking this space in future .";
    public static readonly string UserSearchmessage = "No user available at the moment, please keep checking this space in future .";
    public static readonly string CategorySearchmessage = "No category available at the moment, please keep checking this space in future .";
    public static readonly string SubCategorySearchmessage = "No sub category available at the moment, please keep checking this space in future .";
    public static readonly string ProductSearchmessage = "No product available at the moment, please keep checking this space in future .";
    public static readonly string PdfSearchmessage = "No pdf available at the moment, please keep checking this space in future .";
    public static readonly string HomeImageSearchmessage = "No home image available at the moment, please keep checking this space in future .";
    public static readonly string CollectionCategory = "No category available at the moment, please keep checking this space in future .";
    public static readonly string Collection = "No collection available at the moment, please keep checking this space in future .";
    public static readonly string Press = "No press available at the moment, please keep checking this space in future .";

    #endregion

    #region"-----------Images File Path----------------"


    public static readonly string CATEGORY_ACTULE_ROOTURL = "resources/category/full/";
        public static readonly string CATEGORY_ACTULE_ROOTPATH = "resources\\category\\full\\";
        public static readonly string CATEGORY_THUMB_ROOTURL = "resources/category/thumb/";
        public static readonly string CATEGORY_THUMB_ROOTPATH = "resources\\category\\thumb\\";
        public static readonly string CATEGORY_MIDIUM_ROOTURL = "resources/category/midium/";
        public static readonly string CATEGORY_MIDIUM_ROOTPATH = "resources\\category\\midium\\";
        public static readonly string CATEGORY_SMALL_ROOTURL = "resources/category/small/";
        public static readonly string CATEGORY_SMALL_ROOTPATH = "resources\\category\\small\\";
        public static readonly string CATEGORY_SMALL_WIDTH = "75";
        public static readonly string CATEGORY_SMALL_HEIGHT = "75";
        public static readonly string CATEGORY_THUMB_WIDTH = "1020";
        public static readonly string CATEGORY_THUMB_HEIGHT = "414";
        public static readonly string CATEGORY_MIDIUM_WIDTH = "390";
        public static readonly string CATEGORY_MIDIUM_HEIGHT = "454";


        public static readonly string PRODUCT_ACTULE_ROOTURL = "resources/product/full/";
        public static readonly string PRODUCT_ACTULE_ROOTPATH = "resources\\product\\full\\";
        public static readonly string PRODUCT_THUMB_ROOTURL = "resources/product/thumb/";
        public static readonly string PRODUCT_THUMB_ROOTPATH = "resources\\product\\thumb\\";
        public static readonly string PRODUCT_MIDIUM_ROOTURL = "resources/product/midium/";
        public static readonly string PRODUCT_MIDIUM_ROOTPATH = "resources\\product\\midium\\";
        public static readonly string PRODUCT_SMALL_ROOTURL = "resources/product/small/";
        public static readonly string PRODUCT_SMALL_ROOTPATH = "resources\\product\\small\\";
        public static readonly string PRODUCT_SMALL_WIDTH = "75";
        public static readonly string PRODUCT_SMALL_HEIGHT = "75";
        public static readonly string PRODUCT_THUMB_WIDTH = "1020";
        public static readonly string PRODUCT_THUMB_HEIGHT = "414";
        public static readonly string PRODUCT_MIDIUM_WIDTH = "390";
        public static readonly string PRODUCT_MIDIUM_HEIGHT = "454";

        public static readonly string PRODUCT_TEMPLATE_ACTULE_ROOTURL = "resources/productimage/full/";
        public static readonly string PRODUCT_TEMPLATE_ACTULE_ROOTPATH = "resources\\productimage\\full\\";
        public static readonly string PRODUCT_TEMPLATE_THUMB_ROOTURL = "resources/productimage/thumb/";
        public static readonly string PRODUCT_TEMPLATE_THUMB_ROOTPATH = "resources\\productimage\\thumb\\";
        public static readonly string PRODUCT_TEMPLATE_MIDIUM_ROOTURL = "resources/productimage/midium/";
        public static readonly string PRODUCT_TEMPLATE_MIDIUM_ROOTPATH = "resources\\productimage\\midium\\";
        public static readonly string PRODUCT_TEMPLATE_SMALL_ROOTURL = "resources/productimage/small/";
        public static readonly string PRODUCT_TEMPLATE_SMALL_ROOTPATH = "resources\\productimage\\small\\";
        public static readonly string PRODUCT_TEMPLATE_SMALL_WIDTH = "75";
        public static readonly string PRODUCT_TEMPLATE_SMALL_HEIGHT = "75";
        public static readonly string PRODUCT_TEMPLATE_THUMB_WIDTH = "1020";
        public static readonly string PRODUCT_TEMPLATE_THUMB_HEIGHT = "414";
        public static readonly string PRODUCT_TEMPLATE_MIDIUM_WIDTH = "390";
        public static readonly string PRODUCT_TEMPLATE_MIDIUM_HEIGHT = "454";


        public static readonly string PRODUCT_CROPTEMPLATE_ACTULE_ROOTURL = "resources/productcropimage/full/";
        public static readonly string PRODUCT_CROPTEMPLATE_ACTULE_ROOTPATH = "resources\\productcropimage\\full\\";
        public static readonly string PRODUCT_CROPTEMPLATE_THUMB_ROOTURL = "resources/productcropimage/thumb/";
        public static readonly string PRODUCT_CROPTEMPLATE_THUMB_ROOTPATH = "resources\\productcropimage\\thumb\\";
        public static readonly string PRODUCT_CROPTEMPLATE_MIDIUM_ROOTURL = "resources/productcropimage/midium/";
        public static readonly string PRODUCT_CROPTEMPLATE_MIDIUM_ROOTPATH = "resources\\productcropimage\\midium\\";
        public static readonly string PRODUCT_CROPTEMPLATE_SMALL_ROOTURL = "resources/productcropimage/small/";
        public static readonly string PRODUCT_CROPTEMPLATE_SMALL_ROOTPATH = "resources\\productcropimage\\small\\";


        public static string PROFILE_IMAGE_Temp_ROOTPATH = "resources/customerprofile/temp/";
        public static readonly string CUSTOMERPROFILE_ACTULE_ROOTURL = "resources/customerprofile/full/";
        public static readonly string CUSTOMERPROFILE_ACTULE_ROOTPATH = "resources\\customerprofile\\full\\";
        public static readonly string CUSTOMERPROFILE_THUMB_ROOTURL = "resources/customerprofile/thumb/";
        public static readonly string CUSTOMERPROFILE_THUMB_ROOTPATH = "resources\\customerprofile\\thumb\\";
        public static readonly string CUSTOMERPROFILE_MIDIUM_ROOTURL = "resources/customerprofile/midium/";
        public static readonly string CUSTOMERPROFILE_MIDIUM_ROOTPATH = "resources\\customerprofile\\midium\\";
        public static readonly string CUSTOMERPROFILE_SMALL_ROOTURL = "resources/customerprofile/small/";
        public static readonly string CUSTOMERPROFILE_SMALL_ROOTPATH = "resources\\customerprofile\\small\\";
        public static readonly string CUSTOMERPROFILE_SMALL_WIDTH = "75";
        public static readonly string CUSTOMERPROFILE_SMALL_HEIGHT = "75";
        public static readonly string CUSTOMERPROFILE_THUMB_WIDTH = "100";
        public static readonly string CUSTOMERPROFILE_THUMB_HEIGHT = "100";
        public static readonly string CUSTOMERPROFILE_MIDIUM_WIDTH = "90";
        public static readonly string CUSTOMERPROFILE_MIDIUM_HEIGHT = "90";

        public static string Askquestion_IMAGE_Temp_ROOTPATH = "resources/questionmedia/temp/";
        public static readonly string QuestiomMedia_ACTULE_ROOTURL = "resources/questionmedia/full/";
        public static readonly string QuestiomMedia_ACTULE_ROOTPATH = "resources\\questionmedia\\full\\";
        public static readonly string QuestiomMedia_THUMB_ROOTURL = "resources/questionmedia/thumb/";
        public static readonly string QuestiomMedia_THUMB_ROOTPATH = "resources\\questionmedia\\thumb\\";
        public static readonly string QuestiomMedia_MIDIUM_ROOTURL = "resources/questionmedia/midium/";
        public static readonly string QuestiomMedia_MIDIUM_ROOTPATH = "resources\\questionmedia\\midium\\";
        public static readonly string QuestiomMedia_SMALL_ROOTURL = "resources/questionmedia/small/";
        public static readonly string QuestiomMedia_SMALL_ROOTPATH = "resources\\questionmedia\\small\\";
        public static readonly string QuestiomMedia_SMALL_WIDTH = "75";
        public static readonly string QuestiomMedia_SMALL_HEIGHT = "75";
        public static readonly string QuestiomMedia_THUMB_WIDTH = "755";
        public static readonly string QuestiomMedia_THUMB_HEIGHT = "301";
        public static readonly string QuestiomMedia_MIDIUM_WIDTH = "390";
        public static readonly string QuestiomMedia_MIDIUM_HEIGHT = "454";

        public static readonly string Group_ACTULE_ROOTURL = "resources/groupicon/full/";
        public static readonly string Group_ACTULE_ROOTPATH = "resources\\groupicon\\full\\";
        public static readonly string Group_THUMB_ROOTURL = "resources/groupicon/thumb/";
        public static readonly string Group_THUMB_ROOTPATH = "resources\\groupicon\\thumb\\";
        public static readonly string Group_MIDIUM_ROOTURL = "resources/groupicon/midium/";
        public static readonly string Group_MIDIUM_ROOTPATH = "resources\\groupicon\\midium\\";
        public static readonly string Group_SMALL_ROOTURL = "resources/groupicon/small/";
        public static readonly string Group_SMALL_ROOTPATH = "resources\\groupicon\\small\\";
        public static readonly string Group_SMALL_WIDTH = "75";
        public static readonly string Group_SMALL_HEIGHT = "75";
        public static readonly string Group_THUMB_WIDTH = "1020";
        public static readonly string Group_THUMB_HEIGHT = "414";
        public static readonly string Group_MIDIUM_WIDTH = "390";
        public static readonly string Group_MIDIUM_HEIGHT = "454";

        public static readonly string ORDERIMAGE_ROOTURL = "resources/orderimage/";
        public static readonly string ORDERIMAGE_ROOTPATH = "resources\\orderimage\\";

    
    #endregion

    #region "-----------------------------App Messages----------------------"

        public static readonly string InvalidParameter = "Prameters are invalid in format";
        public static readonly string ExceptionError = "It seems that there is some problem with request. Please try again.";
        public static readonly string NewVersionAvailable = "New version is available.\nWould you like to update?";
        public static readonly string NoUpdateAvailable = "No updates available for application.";
        public static readonly string InvalidUserNamePassword = "Please enter valid Username(Email) or Password.";
        public static readonly string FacebookUserLoginUsingFb = "It seems you are facebook user, So please login with facebook account.";
        public static readonly string NormalUserLoginUsingLoginScreen = "You are register normally,Please Login From there.";
        public static readonly string InactiveUser = "It seems you are inactive user, So please contact administrator : info@yesno.com.";
        public static readonly string CountryNotAvailable = "Country is not available.";
        public static readonly string StateNotAvailable = "State is not available.";
        public static readonly string CityNotAvailable = "City is not available.";
        public static readonly string CommentInserted = "Comment posted successfully";
        public static readonly string InvalidEmail = "The E-mail address you entered is invalid. Please enter valid E-mail address to retrieve  your password.";
        public static readonly string EmailSendToUser = "An email with a new password has been sent to ";
        public static readonly string NoCountryStateCityAvailable = "No record(s) available.";
        public static readonly string ContentNotAvailable = "Content is not available.";
        public static readonly string NewUserRegister = "You are successfully registered";

        public static readonly string UserAlreadyExist = "User already exists.";
        // old 03-04-2015 public static readonly string UserDetailUpdate = "Profile successfully updated.";
		public static readonly string UserDetailUpdate = "Profile Updated Successfully.";
        public static readonly string MessageSend = "Your message has been sent successfully.";
        public static readonly string NoticeAddNotificationSend = "Notification send successfully.";
        public static readonly string NoNotificationAvailable = "Notification is not available.";
        public static readonly string FileUploadingFail = "File Not Uploaded.";
        public static readonly string FileUploadingSuccessfully = "File uploaded successfully.";

        #endregion
   
}
public class CropImageData
{
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int rotate { get; set; }
}
public class WebSiteResponse
{
    public object result { get; set; }
    public string status { get; set; }
}
