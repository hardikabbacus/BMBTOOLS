using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;

/// <summary>
/// Summary description for MakeCallManager
/// </summary>
public class TwilioApiManager
{
    //Twillo login

    //faris@bmbtools.com
    //Lawazim12!

    //Phone +1 (647) 930-0022
    //SID: ACb20295ef26ea36e0f518775c3b0aa62a
    //AUTH TOKEN: 19ed154fcfada94fe442ee66cea6a077

    //Twillo details

    #region "Contructor"
    public TwilioApiManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "Private Member"

    //test
    //private string _AccountSid = "AC08115896ccb309055d5d96447610baf3";
    //private string _AuthToken = "5b11b58198ceebbd4f40cd1ac025002e";

    ////live
    private string _AccountSid = "AC08115896ccb309055d5d96447610baf3";  //   
    private string _AuthToken = "5b11b58198ceebbd4f40cd1ac025002e";     // 

    //private string _From = "+12015282301";
    //live
    private string _From = "+918401325282";

    //test
    //private string _VoiceUrl = "http://developer.smartrestaurants.com" + "/voicecall/vc_call.aspx";
    //live

    private string _VoiceUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/voicecall/vc_call.aspx";

    private string _OrderTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Response><Say voice=\"woman\">You have received new order. Order No. {0}</Say></Response>";
    private string _ReservationTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Response><Say voice=\"woman\">Table reservation number {0} by {1} for {2} people for {3} {4}</Say></Response>";

    private string _SMS_ActivationCode = "Your Smart Restaurant account verification code is {0}";
    private string _SMS_ForgotPassword = "Your password is {0}";
    private string _SMS_OrderActivationCode = "Your Smart Restaurant account is not verified yet. Please verify now with {0}";

    #endregion

    #region "enum"
    public enum Type
    {
        Order = 1,
        Reservation = 2
    }

    public enum SmsType
    {
        ForgotPassword = 1,
        ActivationCode = 2,
        OrderActivationCode = 3
    }

    #endregion

    #region "Public Property"

    public string AccountSid
    {
        get { return _AccountSid; }
        set { _AccountSid = value; }
    }

    public string AuthToken
    {
        get { return _AuthToken; }
        set { _AuthToken = value; }
    }

    public string OrderTemplate
    {
        get { return _OrderTemplate; }
    }

    public string ReservationTemplate
    {
        get { return _ReservationTemplate; }
    }

    public string VoiceUrl
    {
        get { return _VoiceUrl; }
    }

    public string From
    {
        get { return _From; }
        set { _From = value; }
    }

    #endregion

    #region  "public methods"

    public string call(string orderNo, string phone)
    {
        try
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            string url = VoiceUrl + "?t=ord&ono=" + orderNo.ToString();

            var options = new CallOptions();
            options.Url = url;
            options.To = phone;
            options.From = From;

            var call = twilio.InitiateOutboundCall(options);
            return Convert.ToString(call.Sid);
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }

    public string call(int bookingNo, string name, int persons, string reservationdate, string reservationtime, string phone)
    {
        try
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            string url = VoiceUrl + "?t=rsv&bno=" + HttpUtility.UrlEncode(bookingNo.ToString()) + "&nm=" + HttpUtility.UrlEncode(name) + "&np=" + HttpUtility.UrlEncode(persons.ToString()) + "&rd=" + HttpUtility.UrlEncode(reservationdate) + "&rt=" + HttpUtility.UrlEncode(reservationtime);
            var options = new CallOptions();
            options.Url = url;
            options.To = phone;
            options.From = From;
            var call = twilio.InitiateOutboundCall(options);
            return Convert.ToString(call.Sid);
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }

    public string sendsms(string data, string phone, SmsType smstype)
    {
        try
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            string smstext = "";
            if (smstype == SmsType.ActivationCode)
            {
                smstext = string.Format(_SMS_ActivationCode, data);
            }
            else if (smstype == SmsType.ForgotPassword)
            {
                smstext = string.Format(_SMS_ForgotPassword, data);
            }
            else if (smstype == SmsType.OrderActivationCode)
            {
                smstext = string.Format(_SMS_OrderActivationCode, data);
            }

            var message = twilio.SendSmsMessage(From, phone, smstext, "");

            if (message.RestException != null)
            {
                return message.RestException.Message;
            }

            return message.Sid;
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }

    #endregion
}