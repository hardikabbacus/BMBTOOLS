﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Admin_viewcontent : System.Web.UI.Page
{
    int pageNo = new int();
    int pageSize = Convert.ToInt32(AppSettings.PAGESIZE);
    string id = "";
    int totalrecs = 0;
    int totalpages = 0;
    String querystring = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "CMS List - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
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

            if (Request.QueryString["flag"] == "add")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "CMS details has been added successfully";
            }
            else if (Request.QueryString["flag"] == "edit")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "CMS details has been updated successfully";
            }
            else if (Request.QueryString["flag"] == "delete")
            {
                lblmsg.Visible = true;
                lblmsgs.Text = "CMS details has been deleted successfully";
            }

            //----------------------Maintain Search------------------------------------
            if (Request.QueryString["key"] != "")
            {
                txtsearch.Text = Request.QueryString["key"];
            }

            BingpageSize();
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
            BindContent();

            if (Session["AdminType"] != null && Convert.ToString(Session["AdminType"]) == "subadmin")
            {
                //imgbtnDelete.Visible = false;
                //addtop.Visible = false;
                //addbottom.Visible = false;
            }

        }
    }

    //Bind product
    private void BindContent(string search = "")
    {
        this.Form.DefaultButton = imgbtnSearch.UniqueID;

        ContentManager objcont = new ContentManager();
        DataTable dtadmin = new DataTable();
        try
        {
            if (txtsearch.Text != "")
            {
                objcont.ContentTitle = txtsearch.Text.Trim();
            }
            else
            {
                objcont.ContentTitle = txtsearch.Text.Trim();
            }

            if (pageNo == 0) { pageNo = 1; }
            objcont.pageNo = pageNo;
            objcont.pageSize = pageSize;
            objcont.SortExpression = SortExpression;
            querystring = "&pageSize=" + ddlpageSize.SelectedValue + "&key=" + txtsearch.Text;
            dtadmin = objcont.SearchItem();
            totalrecs = objcont.TotalRecord;
            if (pageNo == 1)
            {
                objcont.pageNo = 1;
                pageNo = 1;
            }
            else if (pageNo == 0)
            {
                objcont.pageNo = 1;
                pageNo = 1;
            }
            else
            {
                objcont.pageNo = (pageNo - 1) * pageSize;
            }
            objcont.pageSize = pageSize;
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
            String strpaging = CommonFunctions.AdminPagingv2(totalpages, pageNo, querystring, "viewcontent.aspx");
            ltrpaggingbottom.Text = strpaging;
            //Ltrup.Text = strpaging;
            LoadDropDownList();
        }
        catch (Exception ex) { throw ex; }
        finally { dtadmin.Dispose(); objcont = null; }
    }

    //search for user selections
    protected void imgbtnSearch_Click(object sender, EventArgs e)
    {
        //trmsg.Visible = false;
        gvAdmin.PageIndex = 0;
        pageNo = Convert.ToInt32(Request.QueryString["p"]);
        pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        BindContent();
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

    //handle user page selection
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        BindContent("search");
    }

    protected void ddlpageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pageSize = Convert.ToInt32(ddlpageSize.SelectedItem.Value);
        //pageNo = Convert.ToInt32(ddlpage.SelectedItem.Value);
        BindContent("search");
    }
    protected void BingpageSize()
    {
        for (int i = AppSettings.PAGESIZEMINIMUM; i <= AppSettings.PAGESIZELIMIT; i = i + AppSettings.PAGESIZEINTERVAL)
        {
            ddlpageSize.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    //handle grid view page chenging
    protected void gvAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAdmin.PageIndex = e.NewPageIndex;
        BindContent();
        //ddlpage.SelectedIndex = e.NewPageIndex;
    }

    //handle delete event
    protected void imgbtnDelete_Click(object sender, EventArgs e)
    {
        ContentManager objcontent = new ContentManager();
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
                    objcontent.Id = Convert.ToInt32(gvAdmin.DataKeys[gvAdmin.Rows[i].RowIndex].Value.ToString());
                    // objuser.DeleteAdminRightsItem();
                    objcontent.DeleteItem();
                    //Menu delete logic goes here 
                    //if (System.IO.File.Exists(Server.MapPath("~") + "/admin/menu/" + objuser.adminid + ".htm"))
                    //{
                    //    System.IO.File.Delete(Server.MapPath("~") + "/admin/menu/" + objuser.adminid + ".htm");
                    //}
                }
            }
            Response.Redirect("viewcontent.aspx?flag=delete&key=" + txtsearch.Text + "  &pageSize=" + ddlpageSize.SelectedValue + "");
            //Response.Redirect("viewinventory.aspx?flag=delete");
        }
        catch (Exception ex) { throw ex; }
        finally { objcontent = null; }
    }

    //handle row data bound
    protected void gvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gvAdmin_Sorting(object sender, GridViewSortEventArgs e)
    {

        if (ddlpageSize.SelectedValue != "")
        {
            pageSize = Convert.ToInt32(ddlpageSize.SelectedValue);
        }
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
            BindContent();
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

    protected void lnkStatus_click(object sender, EventArgs e)
    {
        ContentManager objprod = new ContentManager();
        GridViewRow row = ((LinkButton)sender).Parent.Parent as GridViewRow;
        objprod.DisplayStatus = Convert.ToBoolean(Convert.ToInt32(((LinkButton)sender).CommandArgument) == 0 ? 1 : 0);

        objprod.Id = Convert.ToInt32(gvAdmin.DataKeys[gvAdmin.Rows[row.RowIndex].RowIndex].Value.ToString());
        objprod.UpdateStatus();
        BindContent();
        //trmsg.Visible = true;
        //lblmsgs.Text = "Admin Menu details are updated successfully";
    }

    protected void gvAdmin_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvAdmin.EditIndex = e.NewEditIndex;
        BindContent();
    }
    protected void gvAdmin_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    protected void gvAdmin_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        Response.Redirect("add_content.aspx");
    }

}