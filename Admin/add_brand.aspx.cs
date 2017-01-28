using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_add_brand : System.Web.UI.Page
{

    brandManager objBrand = new brandManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add/Modify Brand - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Add/Modify Brand";
        btncancel.Visible = false;
        if (!Page.IsPostBack)
        {
            BindBrandItem();
            BindLanguagesOnPageLoad();
            BindLanguagesOnTabLoad();
            Page.Title = "Add Brand - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add Brand";
            lblmsg.Visible = false;
            if (Request.QueryString["flag"] == "add")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "Brand added successfully";
            }

            if (Request.QueryString["flag"] == "delete")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "Brand deleted successfully";
            }

            if (Request.QueryString["fg"] == "edt")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "Brand updated successfully";
            }

            if (Request.QueryString["flag"] == "edit")
            {
                Title = "Modify Brand - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify Brand";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {
                        DataTable dtcontent = new DataTable();
                        objBrand.idbrand = Convert.ToInt32(Request.QueryString["id"]);

                        BindBrandLanguageOnedit(objBrand.idbrand);
                        dtcontent = objBrand.SelectSingleItemByBrandId();
                        if (dtcontent.Rows.Count > 0)
                        {
                            if (dtcontent.Rows[0]["imgName"].ToString() != "")
                            {
                                //RequiredFieldValidator10.Visible = false;
                                //reqimageg.Enabled = false;
                                spimg.Visible = true;
                                hdImage.Value = Server.HtmlDecode(dtcontent.Rows[0]["imgName"].ToString());
                                ancImage.HRef = "~/" + AppSettings.BRAND_ACTULE_ROOTURL + hdImage.Value;
                            }

                            txtsortorder.Text = dtcontent.Rows[0]["sortorder"].ToString();
                            //ddlactive.SelectedValue = Convert.ToString(dtcontent.Rows[0]["isactive"].ToString());
                            if (Convert.ToBoolean(dtcontent.Rows[0]["isactive"].ToString()) == false)
                            {
                                ddlactive.SelectedValue = "0";
                            }
                            else
                            {
                                ddlactive.SelectedValue = "1";
                            }

                            hfprevsort.Value = dtcontent.Rows[0]["sortorder"].ToString();

                        }
                        btncancel.Visible = true;
                    }
                    else
                        Response.Redirect("add_brand.aspx");
                }
                else
                    Response.Redirect("add_brand.aspx");
            }
            else
            {
                txtsortorder.Text = Convert.ToString(CommonFunctions.GetLastSortCount("brand", "sortorder"));
            }
        }
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
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>Brand  Name </label><div class='col-sm-12'><input type='text' class='form-control' id='txtbrandname" + (i + 1).ToString() + "' maxlength='100' name='txtbrandname" + (i + 1).ToString() + "'/><div id='brandmessage' style='color:red'></div></div></div>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>Brand Description </label><div class='col-sm-12'><textarea class='form-control' id='txtbranddesname" + (i + 1).ToString() + "'  maxlength='500' name='txtbranddesname" + (i + 1).ToString() + "'></textarea></div></div>";
                        strLanguage += "</div>";
                    }
                    else
                    {
                        strLanguage += "<input type='text' value='" + dt.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";
                        strLanguage += "  <div class='tab-pane' id='" + dt.Rows[i]["languageName"].ToString() + "'>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>اسم العلامة التجارية</label><div class='col-sm-12'><input type='text' class='form-control' id='txtbrandname" + (i + 1).ToString() + "' maxlength='100' name='txtbrandname" + (i + 1).ToString() + "'/></div></div>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>وصف العلامة التجارية</label><div class='col-sm-12'><textarea class='form-control' id='txtbranddesname" + (i + 1).ToString() + "' maxlength='500' name='txtbranddesname" + (i + 1).ToString() + "'></textarea></div></div>";
                        strLanguage += "</div>";
                    }
                }
            }
            ltrcategorylanguages.Text = strLanguage.ToString();
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Clear(); dt.Dispose(); objLanguage = null; }
    }

    private void BindBrandLanguageOnedit(int brandid)
    {
        DataTable dtLanguages = new DataTable();
        brandManager objbrandLanguage = new brandManager();
        try
        {
            string strLanguage = "";
            objbrandLanguage.idbrand = brandid;
            dtLanguages = objbrandLanguage.SelectBrandLanguagebyID();
            if (dtLanguages.Rows.Count > 0)
            {

                hdtotallanguage.Value = dtLanguages.Rows.Count.ToString();

                for (int i = 0; i < dtLanguages.Rows.Count; i++)
                {

                    if (i == 0)
                    {
                        strLanguage += "<input type='hidden' value='" + dtLanguages.Rows[i]["brandLanguage"].ToString() + "' name='hdid" + (i + 1).ToString() + "' id='hdid" + (i + 1).ToString() + "'>";
                        strLanguage += "<input type='text' value='" + dtLanguages.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";

                        strLanguage += "<div class='tab-pane active' id='" + dtLanguages.Rows[i]["languageName"].ToString() + "'>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>Brand  Name </label><div class='col-sm-12'><input type='text' class='form-control' id='txtbrandname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["brandName"].ToString() + "' maxlength='100' name='txtbrandname" + (i + 1).ToString() + "'/><div id='brandmessage' style='color:red'></div></div></div>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>Brand Description </label><div class='col-sm-12'><textarea class='form-control' id='txtbranddesname" + (i + 1).ToString() + "'  maxlength='500' name='txtbranddesname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["brandDescription"].ToString() + "'>" + dtLanguages.Rows[i]["brandDescription"].ToString() + "</textarea></div></div>";
                        strLanguage += "</div>";

                    }
                    else
                    {
                        strLanguage += "<input type='hidden' value='" + dtLanguages.Rows[i]["brandLanguage"].ToString() + "' name='hdid" + (i + 1).ToString() + "' id='hdid" + (i + 1).ToString() + "'>";
                        strLanguage += "<input type='text' value='" + dtLanguages.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";

                        strLanguage += "<div class='tab-pane' id='" + dtLanguages.Rows[i]["languageName"].ToString() + "'>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>اسم العلامة التجارية</label><div class='col-sm-12'><input type='text' class='form-control' id='txtbrandname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["brandName"].ToString() + "' maxlength='100' name='txtbrandname" + (i + 1).ToString() + "'/></div></div>";
                        strLanguage += "<div class='form-group'><label for='inputName' class='col-sm-12 control-label'>وصف العلامة التجارية</label><div class='col-sm-12'><textarea class='form-control' id='txtbranddesname" + (i + 1).ToString() + "'  maxlength='500' name='txtbranddesname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["brandDescription"].ToString() + "'>" + dtLanguages.Rows[i]["brandDescription"].ToString() + "</textarea></div></div>";
                        strLanguage += "</div>";

                    }

                }
            }
            ltrcategorylanguages.Text = strLanguage.ToString();
        }
        catch (Exception ex) { throw ex; }
        finally { dtLanguages.Dispose(); dtLanguages.Clear(); }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
        {
            if (RegExp.IsNumericValue(Request.QueryString["id"]))
            {
                DataTable dtcontent = new DataTable();

                string fullimagepath = string.Empty;
                string thumbimagepath = string.Empty;
                string rectimagepath = string.Empty;
                string mediumimagepath = string.Empty;

                fullimagepath = Server.MapPath("../" + AppSettings.BRAND_ACTULE_ROOTURL + hdImage.Value);
                thumbimagepath = Server.MapPath("../" + AppSettings.BRAND_THUMB_ROOTURL + hdImage.Value);
                rectimagepath = Server.MapPath("../" + AppSettings.BRAND_THUMBRECT_ROOTURL + hdImage.Value);
                mediumimagepath = Server.MapPath("../" + AppSettings.BRAND_MEDIUM_ROOTURL + hdImage.Value);

                CommonFunctions.DeleteFile(fullimagepath);
                CommonFunctions.DeleteFile(thumbimagepath);
                CommonFunctions.DeleteFile(rectimagepath);
                CommonFunctions.DeleteFile(mediumimagepath);

                objBrand.idbrand = Convert.ToInt32(Request.QueryString["id"]);
                objBrand.DeleteBrand();
                objBrand.DeleteBrandlanguage();
                //   lblmsg.Text = "Delete Brand Successfully";
                Response.Redirect("add_brand.aspx?flag=delete");

            }
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        //lblmsgs.Text = "";
        int maxID;
        if (Page.IsValid)
        {

            brandManager objbrandLanguage = new brandManager();

            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                if (i == 0)
                {

                    objBrand.brandName = Convert.ToString(Request.Form["txtbrandname" + (i + 1).ToString()]); //Convert.ToInt32(Request.Form["splangid" + (i + 1).ToString()]);
                    objBrand.branddescription = Convert.ToString(Request.Form["txtbranddesname" + (i + 1).ToString()]);
                    objBrand.sortorder = Convert.ToInt32(txtsortorder.Text);
                    objBrand.isactive = Convert.ToByte(ddlactive.SelectedValue);

                    if (Request.QueryString["flag"] == "edit")
                    {
                        objBrand.idbrand = Convert.ToInt32(Request.QueryString["id"]);

                        UpdateBrandLanguage(objBrand.idbrand);

                        if (objBrand.TitleExist())
                        {
                            lblmsgdiv.Visible = true;
                            lblmsg.Visible = false;
                            lblalert.Text = "Brand already exists.";
                            return;
                        }

                        //// update brand image
                        //if (txtImageName.HasFile)
                        //{
                        //    objBrand.imagepath = UploadImage(Convert.ToInt32(Request.QueryString["id"]));
                        //}
                        //else
                        //{
                        //    objBrand.imagepath = hdImage.Value;
                        //}

                        HttpFileCollection uploadedFiles = Request.Files;
                        if (uploadedFiles.Count > 0)
                        {
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
                                        string strImg = UploadImage(userPostedFile, Convert.ToInt32(Request.QueryString["id"]));
                                        if (strImg != "Noimage")
                                        {
                                            objBrand.imagepath = strImg;
                                        }
                                        else { objBrand.imagepath = ""; }
                                    }
                                }
                                else
                                { objBrand.imagepath = hdImage.Value; }
                            }
                        }
                        else
                        {
                            objBrand.imagepath = hdImage.Value;
                        }

                        objBrand.UpdateItem(Convert.ToInt32(hfprevsort.Value), Convert.ToInt32(txtsortorder.Text));
                        lblmsg.Visible = true;
                        lblmsgs.Text = "Brand updated successfully";
                        Response.Redirect("add_brand.aspx?fg=edt&flag=" + Request.QueryString["edit"]);
                        //lblmsg.Visible = true;
                        //lblmsgs.Text = "Brand updated successfully";
                    }
                    else
                    {
                        objBrand.idbrand = 0;
                        if (objBrand.TitleExist())
                        {
                            lblmsgdiv.Visible = true;
                            lblmsg.Visible = false;
                            lblalert.Text = "Brand already exists.";
                            return;
                        }

                        objBrand.imagepath = "";
                        objBrand.idbrand = 0;
                        objBrand.InsertItem();
                        maxID = objBrand.getmaxid();

                        InsertBrandLanguage(maxID);

                        ////insert brand image
                        //if (txtImageName.HasFile)
                        //{
                        //    objBrand.imagepath = UploadImage(maxID);
                        //}
                        //else
                        //{
                        //    objBrand.imagepath = "";
                        //}

                        HttpFileCollection uploadedFiles = Request.Files;
                        if (uploadedFiles.Count > 0)
                        {
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
                                        string strImg = UploadImage(userPostedFile, maxID);
                                        if (strImg != "Noimage")
                                        {
                                            objBrand.imagepath = strImg;
                                        }
                                        else { objBrand.imagepath = ""; }
                                    }
                                    else
                                    { objBrand.imagepath = ""; }
                                }
                            }
                        }
                        else
                        {
                            objBrand.imagepath = "";
                        }


                        objBrand.idbrand = maxID;
                        objBrand.UpdateImage();

                        Response.Redirect("add_brand.aspx?flag=add&key=" + Request.QueryString["key"]);
                    }

                }

            }
        }
    }

    public void InsertBrandLanguage(int brandId)
    {
        brandManager objbrandLanguage = new brandManager();
        try
        {
            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                objbrandLanguage.idbrand = brandId;
                objbrandLanguage.languageId = Convert.ToInt32(Request.Form["splangid" + (i + 1).ToString()]);
                objbrandLanguage.brandName = Convert.ToString(Request.Form["txtbrandname" + (i + 1).ToString()]);
                objbrandLanguage.branddescription = Convert.ToString(Request.Form["txtbranddesname" + (i + 1).ToString()]);
                objbrandLanguage.InsertBrandLanguage();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objbrandLanguage = null; }
    }

    public void UpdateBrandLanguage(int Id)
    {
        brandManager objbrandLanguage = new brandManager();
        try
        {
            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                objbrandLanguage.idbrand = Id;
                objbrandLanguage.brandLanguage = Convert.ToInt32(Request.Form["hdid" + (i + 1).ToString()]);
                objbrandLanguage.brandName = Convert.ToString(Request.Form["txtbrandname" + (i + 1).ToString()]);
                objbrandLanguage.branddescription = Convert.ToString(Request.Form["txtbranddesname" + (i + 1).ToString()]);
                int lan = i + 1;
                objbrandLanguage.languageId = lan;
                if (objbrandLanguage.brandLanguage == 0)
                {
                    objbrandLanguage.InsertBrandLanguage();
                }
                else
                {
                    objbrandLanguage.UpdateBrandLnaguage();
                }

            }
        }
        catch (Exception ex) { throw ex; }
        finally { objbrandLanguage = null; }
    }

    protected string UploadImage(HttpPostedFile fileObject, int maxID)
    {
        string actualfolder = string.Empty;
        string thumbfolder = string.Empty;
        string thumbrectfolder = string.Empty;
        string midiumfolder = string.Empty;
        //string smallfolder = string.Empty;


        actualfolder = Server.MapPath("../" + AppSettings.BRAND_ACTULE_ROOTURL);
        thumbfolder = Server.MapPath("../" + AppSettings.BRAND_THUMB_ROOTURL);
        thumbrectfolder = Server.MapPath("../" + AppSettings.BRAND_THUMBRECT_ROOTURL);
        midiumfolder = Server.MapPath("../" + AppSettings.BRAND_MEDIUM_ROOTURL);

        DirectoryInfo actDir = new DirectoryInfo(actualfolder);
        DirectoryInfo thumbDir = new DirectoryInfo(thumbfolder);
        DirectoryInfo thumbrectDir = new DirectoryInfo(thumbrectfolder);
        DirectoryInfo midiumDir = new DirectoryInfo(midiumfolder);


        //check if Directory exist if not create it
        if (!actDir.Exists) { Directory.CreateDirectory(actualfolder); }

        //check if Directory exist if not create it
        if (!thumbDir.Exists) { Directory.CreateDirectory(thumbfolder); }

        if (!thumbrectDir.Exists) { Directory.CreateDirectory(thumbrectfolder); }

        if (!midiumDir.Exists) { Directory.CreateDirectory(midiumfolder); }


        string filename = string.Empty;
        string fullimagepath = string.Empty;
        string thumbimagepath = string.Empty;
        string thumbrectimagepath = string.Empty;
        string midiumimagepath = string.Empty;

        if (Path.GetExtension(Path.GetFileName(fileObject.FileName)).ToLower() == ".jpg" && Path.GetExtension(Path.GetFileName(fileObject.FileName)).ToLower() == ".Png")
        {
            //filename = maxID + Path.GetExtension(Path.GetFileName(txtImageName.PostedFile.FileName));
            filename = maxID + Path.GetExtension(Path.GetFileName(fileObject.FileName));

            fullimagepath = actualfolder + filename;
            thumbimagepath = thumbfolder;
            thumbrectimagepath = thumbrectfolder;
            midiumimagepath = midiumfolder;

            //delete old files if Exists
            CommonFunctions.DeleteFile(fullimagepath);

            //delete old files if Exists
            CommonFunctions.DeleteFile(thumbimagepath);

            //delete old files if Exists
            CommonFunctions.DeleteFile(thumbrectimagepath);

            //delete old files if Exists
            CommonFunctions.DeleteFile(midiumimagepath);

            //save original image
            //txtImageName.PostedFile.SaveAs(fullimagepath);
            fileObject.SaveAs(fullimagepath);

            //generate thumb

            CommonFunctions.Thmbimages(fullimagepath, thumbfolder, filename, Convert.ToInt32(AppSettings.BRAND_THUMB_WIDTH), Convert.ToInt32(AppSettings.BRAND_THUMB_HEIGHT), 0);
            CommonFunctions.Thmbimages(fullimagepath, thumbrectfolder, filename, Convert.ToInt32(AppSettings.BRAND_THUMBRECT_WIDTH), Convert.ToInt32(AppSettings.BRAND_THUMBRECT_HEIGHT), 0);
            CommonFunctions.Thmbimages(fullimagepath, midiumfolder, filename, Convert.ToInt32(AppSettings.BRAND_MEDIUM_WIDTH), Convert.ToInt32(AppSettings.BRAND_MEDIUM_HEIGHT), 0);
        }
        else
        {
            filename = "Noimage";
        }
        return filename;


    }

    protected void lnkdelimg_Click(object sender, EventArgs e)
    {
        brandManager objcontent = new brandManager();
        string fullimagepath = string.Empty;
        string thumbimagepath = string.Empty;
        string rectimagepath = string.Empty;
        string mediumimagepath = string.Empty;

        fullimagepath = Server.MapPath("../" + AppSettings.BRAND_ACTULE_ROOTURL + hdImage.Value);
        thumbimagepath = Server.MapPath("../" + AppSettings.BRAND_THUMB_ROOTURL + hdImage.Value);
        rectimagepath = Server.MapPath("../" + AppSettings.BRAND_THUMBRECT_ROOTURL + hdImage.Value);
        mediumimagepath = Server.MapPath("../" + AppSettings.BRAND_MEDIUM_ROOTURL + hdImage.Value);

        CommonFunctions.DeleteFile(fullimagepath);
        CommonFunctions.DeleteFile(thumbimagepath);
        CommonFunctions.DeleteFile(rectimagepath);
        CommonFunctions.DeleteFile(mediumimagepath);

        objcontent.idbrand = Convert.ToInt32(Request.QueryString["id"]);
        objcontent.imagepath = "";
        objcontent.UpdateImage();
        //lblmsgs.Text = "Image deleted successfully";
        //reqimageg.Enabled = true;
        spimg.Visible = false;
        hdImage.Value = "";
        btncancel.Visible = false;
    }

    public void BindBrandItem()
    {
        DataTable dt = objBrand.GetBrandItem();
        ddlbrandlist.DataSource = dt;
        ddlbrandlist.DataBind();
    }


}