using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_add_category : System.Web.UI.Page
{

    categoryManager objCategory = new categoryManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add/Modify Category - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Add/Modify Category";
        btncancel.Visible = false;
        if (!Page.IsPostBack)
        {
            BindParentMenu();
            BindCategoryMenu();
            //BindLanguagesOnPageLoad();
            Page.Title = "Add Category - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add Category";

            lblp.Visible = false;
            if (Request.QueryString["flag"] == "add")
            {
                lblp.Visible = true;
                lblp.Attributes["class"] = "alert alert-success alert-dismissible msgsucess";
                lblmsgs.Text = "Category added successfully";
            }

            if (Request.QueryString["fg"] == "edt")
            {
                lblp.Visible = true;
                lblp.Attributes["class"] = "alert alert-success alert-dismissible msgsucess";
                lblmsgs.Text = "Category updated successfully";
            }

            if (Request.QueryString["flag"] == "delete")
            {
                lblp.Visible = true;
                lblp.Attributes["class"] = "alert alert-success alert-dismissible msgsucess";
                lblmsgs.Text = "Category deleted successfully";
            }

            if (Request.QueryString["flag"] == "edit")
            {
                Title = "Modify Category - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify Category";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {


                        DataTable dtcontent = new DataTable();
                        DataTable dtcontentlanguages = new DataTable();

                        objCategory.idcategory = Convert.ToInt32(Request.QueryString["id"]);

                        // BindCategoryLanguageOnedit(objCategory.idcategory);
                        dtcontent = objCategory.SelectSingleItemByCategoryId();
                        dtcontentlanguages = objCategory.SelectSingleItemByCategoryLanguagesId();

                        if (dtcontent.Rows.Count > 0)
                        {
                            txtcategoryname.Text = Server.HtmlDecode(dtcontent.Rows[0]["categoryName"].ToString());
                            txtcategorydes.Text = Server.HtmlDecode(dtcontent.Rows[0]["categoryDescription"].ToString());
                            ddlParentcategory.SelectedValue = Convert.ToString(dtcontent.Rows[0]["parentid"].ToString());
                            //lblimages.Value = Server.HtmlDecode(dtcontent.Rows[0]["imgName"].ToString());
                            if (dtcontentlanguages.Rows.Count > 0)
                            {

                                txtarbicname.Text = Server.HtmlDecode(dtcontentlanguages.Rows[0]["categoryName"].ToString());
                                txtarbicdes.Text = Server.HtmlDecode(dtcontentlanguages.Rows[0]["categoryDescription"].ToString());
                            }


                            if (dtcontent.Rows[0]["imgName"].ToString() != "")
                            {
                                //RequiredFieldValidator10.Visible = false;
                                //reqimageg.Enabled = false;
                                spimg.Visible = true;
                                hdImage.Value = Server.HtmlDecode(dtcontent.Rows[0]["imgName"].ToString());
                                ancImage.HRef = "~/" + AppSettings.CATEGORY_ACTULE_ROOTURL + hdImage.Value;
                            }

                            //txtImageName.Text = Server.HtmlDecode(dtcontent.Rows[0]["imgName"].ToString());
                            txtsortorder.Text = dtcontent.Rows[0]["sortorder"].ToString();

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
                        Response.Redirect("add_category.aspx");
                }
                else
                    Response.Redirect("add_category.aspx");
            }
            else
            {
                txtsortorder.Text = Convert.ToString(CommonFunctions.GetLastSortCount("category", "sortorder"));
            }
        }
    }

    public void BindParentMenu()
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
                    li.Attributes.Add("style", "color:#666;font-weight:bold;");
                    ddlParentcategory.Items.Add(li);
                    objCategory.parentid = Convert.ToInt32(dt.Rows[i]["categoryId"].ToString());
                    dtsub = objCategory.GetSubCategory();
                    //if (dtsub != null && dtsub.Rows.Count > 0)
                    //{
                    //    for (int j = 0; j < dtsub.Rows.Count; j++)
                    //    {
                    //        ListItem lisub = new ListItem("--" + Server.HtmlDecode(dtsub.Rows[j]["categoryName"].ToString()), dtsub.Rows[j]["categoryId"].ToString());
                    //        lisub.Attributes.Add("style", "color:#666");
                    //        ddlParentcategory.Items.Add(lisub);

                    //    }
                    //}
                }
                ddlParentcategory.Items.Insert(0, new ListItem("--Select Parent Category--", "0"));
            }
            else
            {
                ddlParentcategory.Items.Insert(0, new ListItem("--No Category Available--", "0"));
            }

        }
        catch (Exception ex) { throw ex; }
        finally { }
    }

    public void BindCategoryMenu()
    {
        DataTable dt = objCategory.GetParentCategory();
        DataList1.DataSource = dt;
        DataList1.DataBind();
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        //lblmsgs.Text = "";
        int maxID;
        if (Page.IsValid)
        {
            objCategory.categoryName = Server.HtmlEncode(txtcategoryname.Text);
            objCategory.catedesc = Server.HtmlEncode(txtcategorydes.Text);
            objCategory.sortorder = Convert.ToInt32(txtsortorder.Text);

            objCategory.isactive = Convert.ToByte(ddlactive.SelectedValue);

            objCategory.parentid = Convert.ToInt32(ddlParentcategory.SelectedValue);
            //objCategory.imagepath = Server.HtmlEncode(txtImageName.Text);

            if (Request.QueryString["flag"] == "edit")
            {
                objCategory.idcategory = Convert.ToInt32(Request.QueryString["id"]);
                UpdateCountryLanguage(objCategory.idcategory);
                if (objCategory.TitleExist())
                {
                    lblp.Visible = true;
                    lblp.Attributes["class"] = "alert alert-danger alert-dismissible msgsucess";
                    iconemsg.Attributes["class"] = "icon fa fa-ban";
                    lblmsgs.Text = "Category name already exists.";
                    return;
                }

                // update images
                //if (txtImageName.HasFile)
                //{
                //    objCategory.imagepath = UploadImage(Convert.ToInt32(Request.QueryString["id"]));
                //}
                //else
                //{
                //    objCategory.imagepath = hdImage.Value;
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
                                string imgStr = UploadImage(userPostedFile, Convert.ToInt32(Request.QueryString["id"]));
                                if (imgStr != "Noimage")
                                {
                                    objCategory.imagepath = imgStr;
                                }
                                else { objCategory.imagepath = ""; }
                            }
                        }
                        else
                        { objCategory.imagepath = hdImage.Value; }
                    }
                }
                else
                {
                    objCategory.imagepath = hdImage.Value;
                }


                objCategory.UpdateItem(Convert.ToInt32(hfprevsort.Value), Convert.ToInt32(txtsortorder.Text));

                if (System.Configuration.ConfigurationManager.AppSettings["English"] == "1")
                {
                    objCategory.languageId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["English"]);
                    objCategory.categoryId = Convert.ToInt32(Request.QueryString["id"]);
                    objCategory.categoryName = Server.HtmlEncode(txtcategoryname.Text);
                    objCategory.categoryDescription = Server.HtmlEncode(txtcategorydes.Text);
                    objCategory.UpdateCategoryLnaguage();
                }

                if (System.Configuration.ConfigurationManager.AppSettings["Arabic"] == "2")
                {
                    objCategory.languageId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Arabic"]);
                    objCategory.categoryId = Convert.ToInt32(Request.QueryString["id"]);
                    objCategory.categoryName = Server.HtmlEncode(txtarbicname.Text);
                    objCategory.categoryDescription = Server.HtmlEncode(txtarbicdes.Text);
                    objCategory.UpdateCategoryLnaguage();
                }
                lblmsg.Text = "Category updated successfully";
                //Response.Redirect("add_category.aspx?flag=edit&fg=edt&id=" + Request.QueryString["id"]);
                Response.Redirect("add_category.aspx?fg=edt&flag=" + Request.QueryString["edit"]);
            }
            else
            {
                objCategory.idcategory = 0;
                if (objCategory.TitleExist())
                {
                    lblp.Visible = true;
                    lblp.Attributes["class"] = "alert alert-danger alert-dismissible msgsucess";
                    iconemsg.Attributes["class"] = "icon fa fa-ban";
                    lblmsgs.Text = "Category name already exists.";
                    return;
                }

                objCategory.imagepath = "";
                objCategory.idcategory = 0;
                objCategory.InsertItem();
                maxID = objCategory.getmaxid();

                //1
                //InsertCountryLanguage(maxID);
                //2
                if (System.Configuration.ConfigurationManager.AppSettings["English"] == "1")
                {
                    objCategory.languageId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["English"]);
                    objCategory.categoryId = maxID;
                    objCategory.categoryName = Server.HtmlEncode(txtcategoryname.Text);
                    objCategory.categoryDescription = Server.HtmlEncode(txtcategorydes.Text);
                    objCategory.InsertCategoryLanguage();
                }

                if (System.Configuration.ConfigurationManager.AppSettings["Arabic"] == "2")
                {
                    objCategory.languageId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Arabic"]);
                    objCategory.categoryId = maxID;
                    objCategory.categoryName = Server.HtmlEncode(txtarbicname.Text);
                    objCategory.categoryDescription = Server.HtmlEncode(txtarbicdes.Text);
                    objCategory.InsertCategoryLanguage();
                }

                // insert image
                //if (txtImageName.HasFile)
                //{
                //    objCategory.imagepath = UploadImage(maxID);
                //}
                //else
                //{
                //    objCategory.imagepath = "";
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
                                string imgStr = UploadImage(userPostedFile, maxID);
                                if (imgStr != "Noimage")
                                {
                                    objCategory.imagepath = imgStr;
                                }
                                else { objCategory.imagepath = ""; }
                            }
                        }
                    }
                }
                else
                {
                    objCategory.imagepath = "";
                }

                objCategory.idcategory = maxID;
                objCategory.UpdateImage();
                lblmsg.Text = "Add category successfully";


                Response.Redirect("add_category.aspx?flag=add&id=" + Request.QueryString["id"]);
            }
        }
    }
    protected void lnkdelimg_Click(object sender, EventArgs e)
    {
        categoryManager objcontent = new categoryManager();
        string fullimagepath = string.Empty;
        string thumbimagepath = string.Empty;
        string mediumimagepath = string.Empty;
        string thumbrectimagepath = string.Empty;

        fullimagepath = Server.MapPath("../" + AppSettings.CATEGORY_ACTULE_ROOTURL + hdImage.Value);
        thumbimagepath = Server.MapPath("../" + AppSettings.CATEGORY_THUMB_ROOTURL + hdImage.Value);
        mediumimagepath = Server.MapPath("../" + AppSettings.CATEGORY_MEDIUM_ROOTURL + hdImage.Value);
        thumbrectimagepath = Server.MapPath("../" + AppSettings.CATEGORY_THUMBRECT_ROOTURL + hdImage.Value);

        CommonFunctions.DeleteFile(fullimagepath);
        CommonFunctions.DeleteFile(thumbimagepath);
        CommonFunctions.DeleteFile(mediumimagepath);
        CommonFunctions.DeleteFile(thumbrectimagepath);

        objcontent.idcategory = Convert.ToInt32(Request.QueryString["id"]);
        objcontent.imagepath = "";
        objcontent.UpdateImage();
        ////lblmsgs.Text = "Image deleted successfully";
        //reqimageg.Enabled = true;
        spimg.Visible = false;
        hdImage.Value = "";
        btncancel.Visible = true;
    }

    protected string UploadImage(HttpPostedFile fileObject, int maxID)
    {
        string actualfolder = string.Empty;
        string thumbfolder = string.Empty;
        string thumbrectfolder = string.Empty;
        string midiumfolder = string.Empty;

        actualfolder = Server.MapPath("../" + AppSettings.CATEGORY_ACTULE_ROOTURL);
        thumbfolder = Server.MapPath("../" + AppSettings.CATEGORY_THUMB_ROOTURL);
        thumbrectfolder = Server.MapPath("../" + AppSettings.CATEGORY_THUMBRECT_ROOTURL);
        midiumfolder = Server.MapPath("../" + AppSettings.CATEGORY_MEDIUM_ROOTURL);

        DirectoryInfo actDir = new DirectoryInfo(actualfolder);
        DirectoryInfo thumbDir = new DirectoryInfo(thumbfolder);
        DirectoryInfo thumbrectDir = new DirectoryInfo(thumbrectfolder);
        DirectoryInfo midiumDir = new DirectoryInfo(midiumfolder);
        //DirectoryInfo smallDir = new DirectoryInfo(smallfolder);


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

        if (Path.GetExtension(Path.GetFileName(fileObject.FileName)).ToLower() == ".jpg" || Path.GetExtension(Path.GetFileName(fileObject.FileName)).ToLower() == ".png")
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
            CommonFunctions.Thmbimages(fullimagepath, thumbfolder, filename, Convert.ToInt32(AppSettings.CATEGORY_THUMB_WIDTH), Convert.ToInt32(AppSettings.CATEGORY_THUMB_HEIGHT), 0);
            CommonFunctions.Thmbimages(fullimagepath, thumbrectfolder, filename, Convert.ToInt32(AppSettings.CATEGORY_THUMBRECT_WIDTH), Convert.ToInt32(AppSettings.CATEGORY_THUMB_HEIGHT), 0);
            CommonFunctions.Thmbimages(fullimagepath, midiumfolder, filename, Convert.ToInt32(AppSettings.CATEGORY_MEDIUM_WIDTH), Convert.ToInt32(AppSettings.CATEGORY_MEDIUM_HEIGHT), 0);
        }
        else
        {
            filename = "Noimage";
        }
        return filename;


    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        DataTable dt = new DataTable();
        Panel p = (Panel)e.Item.FindControl("panelsubcat"); //Find panel 
        HyperLink l = (HyperLink)e.Item.FindControl("MainCat"); //Get Main category
        l.Attributes.Add("onclick", "showsubmenu('" + p.ClientID + "','" + l.ClientID + "')");
        Label id = (Label)e.Item.FindControl("ID"); //Find main category id
        int catid = Convert.ToInt16(id.Text.ToString());
        dt = objCategory.GetCategory(catid);

        DataList d = (DataList)e.Item.FindControl("subcat"); //Find another gridview
        d.DataSource = dt;//Set datasorue
        d.DataBind();
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
        {
            objCategory.parentid = Convert.ToInt32(Request.QueryString["id"]);
            int count = objCategory.SelectSubselectCount();
            if (count > 0)
            {
                lblp.Visible = true;
                lblp.Attributes["class"] = "alert alert-danger alert-dismissible msgsucess";
                iconemsg.Attributes["class"] = "icon fa fa-ban";
                lblmsgs.Text = "There are some records associated with this,please delete those records first";
            }
            else
            {
                if (RegExp.IsNumericValue(Request.QueryString["id"]))
                {
                    DataTable dtcontent = new DataTable();

                    string fullimagepath = string.Empty;
                    string thumbimagepath = string.Empty;
                    string mediumimagepath = string.Empty;
                    string thumbrectimagepath = string.Empty;

                    fullimagepath = Server.MapPath("../" + AppSettings.CATEGORY_ACTULE_ROOTURL + hdImage.Value);
                    thumbimagepath = Server.MapPath("../" + AppSettings.CATEGORY_THUMB_ROOTURL + hdImage.Value);
                    mediumimagepath = Server.MapPath("../" + AppSettings.CATEGORY_MEDIUM_ROOTURL + hdImage.Value);
                    thumbrectimagepath = Server.MapPath("../" + AppSettings.CATEGORY_THUMBRECT_ROOTURL + hdImage.Value);

                    CommonFunctions.DeleteFile(fullimagepath);
                    CommonFunctions.DeleteFile(thumbimagepath);
                    CommonFunctions.DeleteFile(mediumimagepath);
                    CommonFunctions.DeleteFile(thumbrectimagepath);

                    objCategory.idcategory = Convert.ToInt32(Request.QueryString["id"]);
                    objCategory.DeleteCategory();
                    objCategory.DeleteCategorylanguage();
                    lblmsg.Text = "Delete Category Successfully";
                    Response.Redirect("add_category.aspx?flag=delete");

                }
            }
        }
    }

    public void InsertCountryLanguage(int categoryId)
    {
        categoryManager objcategoryLanguage = new categoryManager();
        try
        {
            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                objcategoryLanguage.categoryId = categoryId;
                objcategoryLanguage.languageId = Convert.ToInt32(Request.Form["splangid" + (i + 1).ToString()]);
                objcategoryLanguage.categoryName = Convert.ToString(Request.Form["txtcategoryname" + (i + 1).ToString()]);
                objcategoryLanguage.categoryDescription = Convert.ToString(Request.Form["txtcategorydesname" + (i + 1).ToString()]);
                objcategoryLanguage.InsertCategoryLanguage();
            }
        }
        catch (Exception ex)
        { //throw ex; 
        }
        finally { objcategoryLanguage = null; }
    }

    public void UpdateCountryLanguage(int Id)
    {
        categoryManager objcategoryLanguage = new categoryManager();
        try
        {
            for (int i = 0; i < Convert.ToInt32(hdtotallanguage.Value); i++)
            {
                objcategoryLanguage.categoryId = Id;
                objcategoryLanguage.categoryLanguage = Convert.ToInt32(Request.Form["hdid" + (i + 1).ToString()]);
                objcategoryLanguage.categoryName = Convert.ToString(Request.Form["txtcategoryname" + (i + 1).ToString()]);
                objcategoryLanguage.categoryDescription = Convert.ToString(Request.Form["txtcategorydesname" + (i + 1).ToString()]);
                int lan = i + 1;
                objcategoryLanguage.languageId = lan;
                if (objcategoryLanguage.categoryLanguage == 0)
                { objcategoryLanguage.InsertCategoryLanguage(); }
                else
                {
                    objcategoryLanguage.UpdateCategoryLnaguage();
                }

            }
        }
        catch (Exception ex) { throw ex; }
        finally { objcategoryLanguage = null; }
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

                    strLanguage += "<input type='text' value='" + dt.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";
                    //strLanguage += "<td width='25%'><span style='font-weight:bold;'>" + dt.Rows[i]["languagename"].ToString() + "</span></td>";
                    if (dt.Rows[i]["textAlign"].ToString().ToLower() == "true")
                    {
                        strLanguage += "<label for='exampleInputEmail1'>" + dt.Rows[i]["languageName"].ToString() + " Name </label><input type='text' class='form-control' id='txtcategoryname" + (i + 1).ToString() + "' maxlength='100' name='txtcategoryname" + (i + 1).ToString() + "'>";
                        strLanguage += "<label for='exampleInputEmail1'>" + dt.Rows[i]["languageName"].ToString() + " Description </label><input type='text' class='form-control' id='txtcategorydesname" + (i + 1).ToString() + "'  maxlength='100' name='txtcategorydesname" + (i + 1).ToString() + "'>";
                    }
                    else
                    {
                        strLanguage += "<label for='exampleInputEmail1'>" + dt.Rows[i]["languageName"].ToString() + " Name </label><input type='text' class='form-control' id='txtcategoryname" + (i + 1).ToString() + "' maxlength='100' name='txtcategoryname" + (i + 1).ToString() + "'>";
                        strLanguage += "<label for='exampleInputEmail1'>" + dt.Rows[i]["languageName"].ToString() + " Description </label><input type='text' class='form-control' id='txtcategorydesname" + (i + 1).ToString() + "' maxlength='100' name='txtcategorydesname" + (i + 1).ToString() + "'>";
                    }
                    // strLanguage += "<td width='15%' align='center'><input type='checkbox' name='chkisactive" + (i + 1).ToString() + "' id='chkisactive" + (i + 1).ToString() + "' /></td>";

                }


            }
            // ltrcategorylanguages.Text = strLanguage.ToString();
            //tdlanguage.InnerHtml = strLanguage.ToString();
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Clear(); dt.Dispose(); objLanguage = null; }
    }

    private void BindCategoryLanguageOnedit(int catid)
    {
        DataTable dtLanguages = new DataTable();
        categoryManager objcatLanguage = new categoryManager();
        try
        {
            string strLanguage = "";
            objcatLanguage.categoryId = catid;
            dtLanguages = objcatLanguage.SelectCategoryLanguagebyID();
            if (dtLanguages.Rows.Count > 0)
            {

                hdtotallanguage.Value = dtLanguages.Rows.Count.ToString();

                for (int i = 0; i < dtLanguages.Rows.Count; i++)
                {

                    strLanguage += "<input type='hidden' value='" + dtLanguages.Rows[i]["categoryLanguage"].ToString() + "' name='hdid" + (i + 1).ToString() + "' id='hdid" + (i + 1).ToString() + "'>";
                    strLanguage += "<input type='text' value='" + dtLanguages.Rows[i]["languageid"].ToString() + "' name='splangid" + (i + 1).ToString() + "' id='splangid" + (i + 1).ToString() + "' style='display:none;' />";
                    //strLanguage += "<td width='25%'><span style='font-weight:bold;'>" + dtLanguages.Rows[i]["languagename"].ToString() + "</span></td>";
                    if (dtLanguages.Rows[i]["textAlign"].ToString().ToLower() == "true")
                    {
                        // strLanguage += "<td width='60%' align='center'><input type='text' value='" + dtLanguages.Rows[i]["CountryName"].ToString() + "' style='width:480px;text-align:right;' class='input1' id='txtcountryname" + (i + 1).ToString() + "' maxlength='100' name='txtcountryname" + (i + 1).ToString() + "'></td>";

                        strLanguage += "<label for='exampleInputEmail1'>" + dtLanguages.Rows[i]["languageName"].ToString() + " Name </label><input type='text' class='form-control' id='txtcategoryname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["categoryName"].ToString() + "' maxlength='100' name='txtcategoryname" + (i + 1).ToString() + "'>";
                        strLanguage += "<label for='exampleInputEmail1'>" + dtLanguages.Rows[i]["languageName"].ToString() + " Description </label><input type='text' class='form-control' id='txtcategorydesname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["categoryDescription"].ToString() + "' maxlength='100' name='txtcategorydesname" + (i + 1).ToString() + "'>";
                    }
                    else
                    {
                        // strLanguage += "<td width='60%' align='center'><input type='text' value='" + dtLanguages.Rows[i]["CountryName"].ToString() + "' style='width:480px;text-align:left;' class='input1' id='txtcountryname" + (i + 1).ToString() + "' maxlength='100' name='txtcountryname" + (i + 1).ToString() + "'></td>";

                        strLanguage += "<label for='exampleInputEmail1'>" + dtLanguages.Rows[i]["languageName"].ToString() + " Name </label><input type='text' class='form-control' id='txtcategoryname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["categoryName"].ToString() + "' maxlength='100' name='txtcategoryname" + (i + 1).ToString() + "'>";
                        strLanguage += "<label for='exampleInputEmail1'>" + dtLanguages.Rows[i]["languageName"].ToString() + " Description </label><input type='text' class='form-control' id='txtcategorydesname" + (i + 1).ToString() + "' value='" + dtLanguages.Rows[i]["categoryDescription"].ToString() + "' maxlength='100' name='txtcategorydesname" + (i + 1).ToString() + "'>";
                    }

                }

                // ltrcategorylanguages.Text = strLanguage.ToString();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { dtLanguages.Dispose(); dtLanguages.Clear(); objcatLanguage = null; }
    }
}