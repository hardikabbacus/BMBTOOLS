using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_add_customer : System.Web.UI.Page
{
    customerManager objcustomer = new customerManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Add/Modify Customer - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Add/Modify Customer";
        if (!Page.IsPostBack)
        {
            Page.Title = "Add Customer - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
            ltrheading.Text = "Add Customer";
            if (Request.QueryString["flag"] == "edit")
            {
                Title = "Modify Customer - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
                ltrheading.Text = "Modify Customer";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    if (RegExp.IsNumericValue(Request.QueryString["id"]))
                    {
                        DataTable dtcontent = new DataTable();
                        DataTable dttotalsales = new DataTable();
                        objcustomer.customerId = Convert.ToInt32(Request.QueryString["id"]);
                        dtcontent = objcustomer.GetSingleCustomer();

                        //customer total order and sales
                        dttotalsales = objcustomer.GetSalesAndOrderTotalByCustomerId();
                        if (dttotalsales.Rows.Count > 0)
                        {
                            lblnoOrders.InnerText = Convert.ToString(dttotalsales.Rows[0]["TotalOrder"]);
                            lblTotalSale.InnerText = Convert.ToString(dttotalsales.Rows[0]["totalSales"]);
                        }
                        // customer content
                        if (dtcontent.Rows.Count > 0)
                        {
                            txtcontactname.Text = Convert.ToString(dtcontent.Rows[0]["contactName"]);
                            txtcompanyname.Text = Convert.ToString(dtcontent.Rows[0]["companyName"]);
                            txtstreetaddress.Text = Convert.ToString(dtcontent.Rows[0]["streetAddress"]);
                            txtcity.Text = Convert.ToString(dtcontent.Rows[0]["city"]);
                            txtcountry.Text = Convert.ToString(dtcontent.Rows[0]["country"]);
                            txtstorenumber.Text = Convert.ToString(dtcontent.Rows[0]["storePhoneNumber"]);
                            txtgpslocation.Text = Convert.ToString(dtcontent.Rows[0]["gpsLocation"]);
                            hidlongleti.Value = Convert.ToString(dtcontent.Rows[0]["gpsLocation"]);
                            txtmobile.Text = Convert.ToString(dtcontent.Rows[0]["mobile"]);
                            txtemail.Text = Convert.ToString(dtcontent.Rows[0]["email"]);
                            txtpassword.Text = Convert.ToString(dtcontent.Rows[0]["newPassword"]);
                            txtglobelrate.Text = Convert.ToString(dtcontent.Rows[0]["globleDiscountRate"]);
                            txtcredit.Text = Convert.ToString(dtcontent.Rows[0]["creditLimit"]);
                            txtreducepercent.Text = Convert.ToString(dtcontent.Rows[0]["reducePercent"]);
                            ddldays.SelectedValue = Convert.ToString(dtcontent.Rows[0]["xdays"]);
                            chkcod.Checked = Convert.ToBoolean(dtcontent.Rows[0]["caseOnDelivery"]);
                            chkallowcradit.Checked = Convert.ToBoolean(dtcontent.Rows[0]["allowCreditcard"]);
                            ddlcustomertype.SelectedValue = Convert.ToString(dtcontent.Rows[0]["customerType"]);

                            if (Convert.ToInt32(dtcontent.Rows[0]["languagePreferance"]) == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["English"]))
                            {
                                rbtenglish.Checked = true;
                            }
                            else { rbtaribic.Checked = true; }
                            if (Convert.ToString(dtcontent.Rows[0]["newPassword"]) != "")
                            {
                                //reqtxtpassword.Enabled = false;
                                hidpass.Value = Convert.ToString(dtcontent.Rows[0]["newPassword"]);
                            }
                            ltrJoineddate.Text = Convert.ToDateTime(dtcontent.Rows[0]["createDate"]).ToShortDateString();
                            divCustomerSales.Visible = true;

                            ScriptManager.RegisterStartupScript(this, GetType(), "initialize", "initialize();", true);

                        }
                    }
                    else
                        Response.Redirect("viewcustomer.aspx");
                }
                else
                    Response.Redirect("viewcustomer.aspx");
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "initialize", "initialize();", true);

        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        lblmsgs.Text = "";
        if (Page.IsValid)
        {
            objcustomer.contactName = Convert.ToString(txtcontactname.Text.Trim());
            objcustomer.companyName = Convert.ToString(txtcompanyname.Text.Trim());
            objcustomer.streetAddress = Convert.ToString(txtstreetaddress.Text.Trim());
            objcustomer.city = Convert.ToString(txtcity.Text.Trim());
            objcustomer.country = Convert.ToString(txtcountry.Text.Trim());
            objcustomer.storePhoneNumber = Convert.ToString(txtstorenumber.Text.Trim());
            objcustomer.customerType = Convert.ToInt32(ddlcustomertype.SelectedValue);

            //if (txtgpslocation.Text == "")
            //{
            objcustomer.gpsLocation = objcustomer.getlatlong(txtstreetaddress.Text.Trim(), txtcity.Text.Trim(), txtcountry.Text.Trim());
            //}
            //else
            //{
            //    objcustomer.gpsLocation = Convert.ToString(txtgpslocation.Text.Trim());
            //}

            objcustomer.mobile = Convert.ToString(txtmobile.Text.Trim());
            objcustomer.email = Convert.ToString(txtemail.Text.Trim());

            if (txtpassword.Text == "")
            {
                //hidpass.Value
                objcustomer.newPassword = Convert.ToString(hidpass.Value);
            }
            else
            {
                objcustomer.newPassword = Convert.ToString(txtpassword.Text.Trim());
            }
            if (rbtenglish.Checked == true)
            {
                objcustomer.languagePreferance = Convert.ToInt32(1);
            }
            else if (rbtaribic.Checked == true)
            {
                objcustomer.languagePreferance = Convert.ToInt32(2);
            }
            objcustomer.globleDiscountRate = Convert.ToString(txtglobelrate.Text.Trim());
            if (chkcod.Checked == true)
            {
                objcustomer.caseOnDelivery = Convert.ToBoolean(true);
            }
            else
            {
                objcustomer.caseOnDelivery = Convert.ToBoolean(false);
            }
            if (chkallowcradit.Checked == true)
            {
                objcustomer.allowCreditcard = Convert.ToBoolean(true);
            }
            else
            {
                objcustomer.allowCreditcard = Convert.ToBoolean(false);
            }
            objcustomer.xdays = Convert.ToString(ddldays.SelectedValue);
            objcustomer.reducePercent = Convert.ToString(txtreducepercent.Text.ToString().Trim());
            objcustomer.creditLimit = Convert.ToString(txtcredit.Text.ToString().Trim());
            objcustomer.isActive = Convert.ToBoolean(true);

            if (Request.QueryString["flag"] == "edit")
            {
                objcustomer.customerId = Convert.ToInt32(Request.QueryString["id"]);

                objcustomer.UpdateItem();
                Response.Redirect("viewcustomer.aspx?flag=edit&key=" + Request.QueryString["key"]);
            }
            else
            {
                objcustomer.InsertItem();

                // SendMail(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailNotificationsNewUserId"]));

                Response.Redirect("viewcustomer.aspx?flag=add&key=" + Request.QueryString["key"]);
            }
        }
    }

    #region  send mail

    private void SendMail(int status)
    {
        orderManager objorder = new orderManager();

        DataTable GetData = new DataTable();
        GetData = objorder.GetSubject(status);
        if (GetData.Rows.Count > 0)
        {
            String strSubject = string.Empty;
            string strFrom = string.Empty;
            string strTo = string.Empty;
            string customername = string.Empty;
            string username = string.Empty;
            string password = string.Empty;

            strSubject = GetData.Rows[0]["EmailSubject"].ToString();
            strFrom = GetData.Rows[0]["FromEmail"].ToString();
            strTo = txtemail.Text.Trim();

            customername = txtcontactname.Text.Trim();
            username = txtemail.Text.Trim();
            password = txtpassword.Text.Trim();


            String strMsg = string.Empty;

            strMsg = GetData.Rows[0]["EmailBody"].ToString();

            strMsg = strMsg.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
            //string strMsgall = CommonFunctions.GetFileContents(Server.MapPath("../MailTemplate/NewUser.html"));
            strMsg = strMsg.Replace("##COMPANY##", System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"]);
            strMsg = strMsg.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
            strMsg = strMsg.Replace("##CustomerName##", customername);
            strMsg = strMsg.Replace("##UserName##", username);
            strMsg = strMsg.Replace("##Password##", password);

            if (strTo != null)
            {
                CommonFunctions.SendMail2(strFrom, strTo, "", strMsg, strSubject, "", "", "");
                //CommonFunctions.SendMail2(strFrom, "hardik@webtechsystem.com", "", strMsgall, strSubject, "", "", "");
            }
        }
    }

    #endregion

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("viewcustomer.aspx");
    }
}