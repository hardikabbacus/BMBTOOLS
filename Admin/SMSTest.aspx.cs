using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Twilio;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

public partial class Admin_SMSTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        orderManager objorder = new orderManager();
        DataTable getOrderDetail = new DataTable();
        //getOrderDetail = objorder.GetOrderDetail(Convert.ToInt32(Request.QueryString["orderid"]));
        getOrderDetail = objorder.GetOrderDetailForPdf(Convert.ToInt32(32));
        if (getOrderDetail.Rows.Count > 0)
        {
            //string Str = getstring(Discount[0], GetData.Rows[0]["EmailBody"].ToString(), getOrderDetail);
            string Str = getstring(getOrderDetail);
            StringBuilder customermailstrin = new StringBuilder();
            if (Str != "")
            {
                customermailstrin.Append(Str);

                StringReader sr = new StringReader(Str);
                Document pdfDoc = new Document(PageSize.A4, 0f, 0f, 0f, 0f);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + Convert.ToInt32(Request.QueryString["orderid"]) + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bytes);
                    Response.End();
                }

            }
        }
    }

    private string getstring(DataTable getOrderDetail)
    {
        orderManager objorder = new orderManager();

        string strMsg1 = string.Empty;
        string strMsg = string.Empty;
        string strMsgall = string.Empty;

        //strMsg1 += "<table cellpadding='0' cellspacing='0'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td><table width='100%' cellpadding='0' cellspacing='0'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='padding:0;width:50px'><img style='width: 50px;' src='##SITEURL##images/top.jpg' /></td>";
        //strMsg1 += "<td style='background:#F7F8F8; text-align:right;'><table style='font-family: Arial, Helvetica, sans-serif; font-size: 14px;float:right;padding-right:0;width:100%' cellpadding='0' cellspacing='0'>";
        //strMsg1 += "<tbody>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td style='font-weight: bold;width: 200px; text-align: left;color:#020000'>Order Number:</td>";
        //strMsg1 += "<td style='text-align: right;  width: 200px;color:#727071'>" + getOrderDetail.Rows[0]["orderid"] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td style='font-weight: bold;width: 200px; text-align: left;color:#020000'>Date:</td>";
        //DateTime dtime = new DateTime();
        //dtime = Convert.ToDateTime(getOrderDetail.Rows[0]["orderdate"].ToString());
        //strMsg1 += "<td style='text-align: right; width:200px;color:#727071'>" + dtime.ToShortDateString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</tbody>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td colspan='2' style='padding:0;background:#F7F8F8; text-align:left;'><img style='width:154px;' src='##SITEURL##images/main_logo.png' /></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td></td>";
        //strMsg1 += "<td style='border-bottom:1px solid #cbcdce;background:#f7f8f8'>&nbsp;</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td><table cellpadding='0' cellspacing='0'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td><img style='width: 50px; height:800px;' src='##SITEURL##images/mid.jpg' /></td>";
        //strMsg1 += "<td valign='top' style='background:#f7f8f8;padding-right:10px;padding-left:10px'><table style='width: 730px;' cellpadding='0' cellspacing='0' border='0'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td><table style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; color: #000;' cellpadding='0' cellspacing='0' border='0' width='100%'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='width: 250px; padding: 5px;'><table  width='100%'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='width: 80px; font-weight: bold; font-size: 12px; text-align: left;'>Company Name:</td>";
        //strMsg1 += "<td style='font-size: 12px; text-align: right;'>" + getOrderDetail.Rows[0]["companyname"] + "</td>";
        //strMsg1 += "</tr>";

        //string address = string.Empty;
        //if (getOrderDetail.Rows[0]["streetaddress"].ToString() != "")
        //{ address = getOrderDetail.Rows[0]["streetaddress"].ToString(); }
        //if (getOrderDetail.Rows[0]["city"].ToString() != "")
        //{ address += "," + getOrderDetail.Rows[0]["city"].ToString(); }
        //if (getOrderDetail.Rows[0]["country"].ToString() != "") { address += "," + getOrderDetail.Rows[0]["country"].ToString(); }

        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='width: 80px; font-weight: bold; font-size: 12px; text-align: left; vertical-align:top'>Address:</td>";
        //strMsg1 += "<td style='font-size: 12px; text-align: right;'>" + address + "</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "<td style='width: 250px; padding: 10px; font-size: 12px;'><table width='100%'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='width: 120px; font-weight: bold; font-size: 12px; text-align: left;'>Contact Person:</td>";
        //strMsg1 += "<td style='text-align: right; font-size: 12px;'>" + getOrderDetail.Rows[0]["contactname"] + "</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='width: 120px; font-weight: bold; font-size: 12px; text-align: left;'>Mobile:</td>";
        //strMsg1 += "<td style='text-align: right; font-size: 12px;'>" + getOrderDetail.Rows[0]["mobile"] + "</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='width: 120px; font-weight: bold; font-size: 12px; text-align: left;'>Email:</td>";
        //strMsg1 += "<td style='text-align: right; font-size: 12px;'>" + getOrderDetail.Rows[0]["email"] + "</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr style=''>";
        //strMsg1 += "<td><div style='background:#fff;'><table cellpadding='0' cellspacing='0' border='0' style='width: 730px; font-family: Arial, Helvetica, sans-serif; font-size: 12px; padding: 0;border:1px solid #ccc'>";
        //strMsg1 += "<tr style='height: 30px;'>";
        //strMsg1 += "<td style='border-top: 1px solid #ccc;border-left: 1px solid #ccc;border-bottom: 1px solid #ccc;border-right: 1px solid #ccc; width:100px; padding: 5px;'>SKU</td>";
        //strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:200px;'>Product Name</td>";
        //strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:50px;'>Qty</td>";
        //strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:50px;'>Unit Price</td>";
        //strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px; width:50px;'>Discount</td>";
        //strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:65px;'>Final Price</td>";
        //strMsg1 += "<td style='border-top: 1px solid #ccc;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc; padding: 5px;width:65px;'>Total</td>";
        //strMsg1 += "</tr>";
        //decimal subtotal = 0;
        //for (int i = 0; i < getOrderDetail.Rows.Count; i++)
        //{
        //    int cnt = Convert.ToInt32(getOrderDetail.Rows.Count);
        //    if (i == 0)
        //    {
        //        strMsg1 += "<tr style='height: 30px;'>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; width:100px;  border-left: 1px solid #ccc;  padding: 5px;text-align:left;'>&nbsp;&nbsp;" + getOrderDetail.Rows[i]["sku"] + "</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; padding: 5px;width:200px;'>" + Server.HtmlDecode(getOrderDetail.Rows[i]["Productname"].ToString()) + "</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; width:50px;  padding: 5px;'>" + getOrderDetail.Rows[i]["qty"] + "PCS</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["price"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["discount"] + "%&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["finalprice"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='font-weight: bold; border-right: 1px solid #ccc;  padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["netprice"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "</tr>";
        //    }
        //    if ((cnt - 1) == i)
        //    {
        //        strMsg1 += "<tr style='height: 30px;'>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; width:100px; border-left: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:left;'>&nbsp;&nbsp;" + getOrderDetail.Rows[i]["sku"] + "</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;width:200px;'>" + Server.HtmlDecode(getOrderDetail.Rows[i]["Productname"].ToString()) + "</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; border-bottom: 1px solid #ccc;width:50px; padding: 5px;'>" + getOrderDetail.Rows[i]["qty"] + "PCS</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["price"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["discount"] + "%&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["finalprice"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='font-weight: bold; border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["netprice"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "</tr>";
        //    }
        //    else
        //    {
        //        strMsg1 += "<tr style='height: 30px;'>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; width:100px; border-left: 1px solid #ccc;  padding: 5px;text-align:left;'>&nbsp;&nbsp;" + getOrderDetail.Rows[i]["sku"] + "</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc;  padding: 5px;width:200px;'>" + Server.HtmlDecode(getOrderDetail.Rows[i]["Productname"].ToString()) + "</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; width:50px; padding: 5px;'>" + getOrderDetail.Rows[i]["qty"] + "PCS</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc;  padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["price"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc;  padding: 5px;text-align:right;width:50px;'>" + getOrderDetail.Rows[i]["discount"] + "%&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='border-right: 1px solid #ccc; 1px solid #ccc; padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["finalprice"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "<td style='font-weight: bold; border-right: 1px solid #ccc;  padding: 5px;text-align:right;width:65px;'>" + getOrderDetail.Rows[i]["netprice"] + "&nbsp;&nbsp;</td>";
        //        strMsg1 += "</tr>";
        //    }
        //    subtotal = Convert.ToDecimal(Convert.ToDecimal(subtotal) + Convert.ToDecimal(Convert.ToDecimal(getOrderDetail.Rows[i]["price"]) * Convert.ToInt32(getOrderDetail.Rows[i]["qty"])));
        //}
        //decimal totaldiscount = 0;
        //totaldiscount = Convert.ToDecimal(Convert.ToDecimal(subtotal) - Convert.ToDecimal(getOrderDetail.Rows[0]["totalammount"]));
        //strMsg1 += "<tr>";
        //strMsg1 += "<td colspan='4' style='padding: 5px; height: 50px; vertical-align: top; text-align: left;border-bottom: 1px solid #ccc;border-left: 1px solid #ccc;'>&nbsp;&nbsp;Customer Signature</td>";
        //strMsg1 += "<td colspan='3' style='padding: 10px; border: 1px solid #ccc;padding-top:0'><table width='100%' cellpadding='0' cellspacing='0'>";
        //strMsg1 += "<tr style='margin: 5px; height: 25px;'>";
        //strMsg1 += "<td style='margin-right: 20px; width: 300px; text-align: left; font-size: 14px;'>Subtotal:</td>";
        //strMsg1 += "<td style='text-align: right;width:200px; font-size: 14px;'>" + subtotal + "SAR</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr style='margin: 5px; height: 25px;'>";
        //strMsg1 += "<td style='margin-right: 20px; width: 300px; text-align: left; font-size: 14px;'>Total Discouns:</td>";
        //strMsg1 += "<td style='text-align: right;width:200px; font-size: 14px;'>" + totaldiscount + "SAR</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr style='margin: 2px; height: 30px;'>";
        //strMsg1 += "<td style='margin-right: 20px; width: 300px; text-align: left; font-size: 14px; font-weight: bold;border-top: 1px solid #ccc;padding-top:10px'>Grand Total:</td>";
        //strMsg1 += "<td style='text-align: right; width:200px; font-size: 14px; font-weight: bold;border-top: 1px solid #ccc;padding-top:10px'>" + getOrderDetail.Rows[0]["totalammount"] + "SAR</td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table>";
        //strMsg1 += "</td>";
        //strMsg1 += "</tr>";
        ////strMsg1 += "<tr>";
        ////strMsg1 += "<td colspan='8'>&nbsp;</td>";
        ////strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td colspan='4' style='background:#F7F8F8;border-top: 1px solid #ccc;' ></td>";
        //strMsg1 += "<td colspan='3' style='padding: 10px; border-left: 1px solid rgb(204, 204, 204);border-right: 1px solid rgb(204, 204, 204);border-bottom: 1px solid rgb(204, 204, 204); height:50px;'><div>";
        //strMsg1 += "<label style='margin-right: 20px; float: left; width: 120px; text-align: left; font-size: 14px;'>Payment Method:</label>";
        //strMsg1 += "<span style='float: right; font-size: 14px; font-weight: bold;'>Cash</span> </div></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></div></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='background:#f7f8f8; text-align:left;'><img style='width:200px;' src='##SITEURL##images/blk.jpg' /></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='background:#F7F8F8'>";
        //strMsg1 += "<table width='100%'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td style='width:50px'><img src='##SITEURL##images/botom.jpg' /></td>";
        //strMsg1 += "<td style='width:661px'><table cellpadding='0' style='float:right;font-family: Arial, Helvetica, sans-serif; font-size: 11px;width:450px'>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>";
        //strMsg1 += "<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>";
        //strMsg1 += "<th style='padding-right:30px;color:#f05222;text-align:left'>Showroom</th>";
        //strMsg1 += "<th style='padding-right:30px;color:#f05222;text-align:left'>Contact Numbers</th>";
        //strMsg1 += "<th style='padding-right:30px;color:#f05222;text-align:left'>Email</th>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'>Prince Sulaiman Street</td>";
        //strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'><span style='color:#f05222'>T</span> +966 11 270 9251</td>";
        //strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'><a style='color:#818284;text-decoration:none' href='mailto:support@bmbtools.com'>support@bmbtools.com</a></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "<tr>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
        //strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'>Al Faisalya District, Riyadh</td>";
        //strMsg1 += "<td style='padding-right:30px;color:#818284;text-align:left'><span style='color:#f05222'>F</span> +966 11 270 6276</td>";
        //strMsg1 += "<td></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table></td>";
        //strMsg1 += "</tr>";
        //strMsg1 += "</table>";

        strMsgall = CommonFunctions.GetFileContents(Server.MapPath("../MailTemplate/Invoice-test.html"));
        strMsgall = strMsgall.Replace("##CONTENT##", strMsg1);
        strMsgall = strMsgall.Replace("##SITEURL##", System.Configuration.ConfigurationManager.AppSettings["SITEURL"]);
        strMsgall = strMsgall.Replace("##COMPANY##", System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"]);

        return strMsgall;
    }

    public void testsendSms(int smsType)
    {
        DataTable dtData = null;
        smsnotifactionManager objCustomerMgr = new smsnotifactionManager();
        TwilioApiManager objTWApi = new TwilioApiManager();
        try
        {
            objCustomerMgr.smstype = smsType;


            dtData = objCustomerMgr.SelectSingleItem();

            if (dtData.Rows.Count > 0)
            {
                //string isdCode = Convert.ToString(dsData.Tables[0].Rows[0]["isdCode"]);
                //string phoneNo = Convert.ToString(dsData.Tables[0].Rows[0]["phone"]).StartsWith(isdCode) == true ? Convert.ToString(dsData.Tables[0].Rows[0]["phone"]) : isdCode + Convert.ToString(dsData.Tables[0].Rows[0]["phone"]);

                //objTWApi.sendsms(Convert.ToString(dtData.Rows[0]["pass"]), "8401325282", TwilioApiManager.SmsType.ForgotPassword);
                string test = objTWApi.sendsms(Convert.ToString("Test message"), "+918401325282", TwilioApiManager.SmsType.ForgotPassword);


                //var client = new TwilioRestClient(Environment.GetEnvironmentVariable("SK73966f2606d7bdfa353e87c19d029024"), Environment.GetEnvironmentVariable("5p4EekmAELZJOJapr15xgVzserG3J3Vs"));
                //var message = client.SendSmsMessage("+918401325282", "+918866242261", "test sms");
                //if (message.RestException != null)
                //{
                //    //Debug.Writeline(message.RestException.Message);
                //    Response.Write(message.RestException.Message);
                //}
            }

        }
        catch (Exception ex)
        {
            //throw;
        }
        finally
        {
            dtData = null;
            objTWApi = null;
            objCustomerMgr = null;
        }
    }
    protected void btnsend_Click(object sender, EventArgs e)
    {
        testsendSms(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMSOrderRecived"]));

        //var client = new TwilioRestClient(Environment.GetEnvironmentVariable("SK73966f2606d7bdfa353e87c19d029024"), Environment.GetEnvironmentVariable("5p4EekmAELZJOJapr15xgVzserG3J3Vs"));
        //var message = client.SendSmsMessage("+918401325282", "+918401325282", "test sms");
        //if (message.RestException != null)
        //{
        //    //Debug.Writeline(message.RestException.Message);
        //    Response.Write(message.RestException.Message);
        //}
    }
}