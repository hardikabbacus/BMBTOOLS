using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
/// <summary>
/// Summary description for smsnotifactionManager
/// </summary>
public class smsnotifactionManager
{

    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    public smsnotifactionManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region "----------------------------private variables------------------------"

    private int _SmsNotificationsId;
    private string _TwilioSid;
    private string _TwilioAuthToken;
    private string _Descp;
    private int _smstype;
    private byte _IsActive;
    #endregion

    #region "----------------------------public properties------------------------"

    public int SmsNotificationsId { get { return _SmsNotificationsId; } set { _SmsNotificationsId = value; } }
    public string TwilioSid { get { return _TwilioSid; } set { _TwilioSid = value; } }
    public string TwilioAuthToken { get { return _TwilioAuthToken; } set { _TwilioAuthToken = value; } }
    public string Descp { get { return _Descp; } set { _Descp = value; } }
    public int smstype { get { return _smstype; } set { _smstype = value; } }
    public byte IsActive { get { return _IsActive; } set { _IsActive = value; } }


    #endregion

    public enum smsTemplate
    {
        Order_Recieved = 1,
        Order_Shipped = 2,
        Arabic_Recieved = 3,
        Arabic_Shipped = 4
    };

    #region "----------------------------public methods-------------------------"
    //
    /// <summary>
    /// select smsnotification Details
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItem()
    {
        StrQuery = " select isnull([TwilioSid],'') as TwilioSid ,isnull([TwilioAuthToken],0) as TwilioAuthToken ," +
                   " isnull([Descp],'') as Descp , " +
                   " isnull([smstype],'') as smstype    " +
                   " from [SmsNotifications] where smstype=@smstype ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@smstype ", SqlDbType.Int)).Value = smstype;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    //
    /// <summary>
    /// update smsnotifaction Details
    /// </summary>
    public void UpdateItem()
    {
        StrQuery = " update [SmsNotifications] set [TwilioSid]=@TwilioSid,[TwilioAuthToken]=@TwilioAuthToken ,[Descp]=@Descp where smstype=@smstype";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@TwilioSid", SqlDbType.VarChar, 200)).Value = TwilioSid;
            sqlcmd.Parameters.Add(new SqlParameter("@TwilioAuthToken", SqlDbType.NVarChar)).Value = TwilioAuthToken;
            sqlcmd.Parameters.Add(new SqlParameter("@Descp", SqlDbType.NText)).Value = Descp;
            sqlcmd.Parameters.Add(new SqlParameter("@smstype", SqlDbType.Int)).Value = smstype;

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// select single TwilioSid,TwilioAuthToken
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleRecord()
    {
        StrQuery = "select top 1 isnull(TwilioSid,'') as TwilioSid,isnull(TwilioAuthToken,'') as TwilioAuthToken,isnull(IsActive,0) as IsActive from [SmsNotifications]";
        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }

    }

    #endregion

}