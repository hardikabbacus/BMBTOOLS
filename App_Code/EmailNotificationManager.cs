using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmailNotificationManager
/// </summary>
public class EmailNotificationManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

  

	public EmailNotificationManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region "----------------------------private variables------------------------"

    private int _EmailNotificationsId;
    private string _FromEmail;
    private int _EmailType;
    private string _EmailSubject;
    private string _EmailBody;
    private byte _IsActive;
    #endregion

    #region "----------------------------public properties------------------------"

    public int EmailNotificationsId { get { return _EmailNotificationsId; } set { _EmailNotificationsId = value; } }
    public string FromEmail { get { return _FromEmail; } set { _FromEmail = value; } }
    public int EmailType { get { return _EmailType; } set { _EmailType = value; } }
    public string EmailSubject { get { return _EmailSubject; } set { _EmailSubject = value; } }
    public string EmailBody { get { return _EmailBody; } set { _EmailBody = value; } }
    public byte IsActive { get { return _IsActive; } set { _IsActive = value; } }
   

    #endregion
    public enum EmailTemplate
    {
        New_User = 1,
        Orders_Recieved = 2,
        Orders_Shipped = 3,
        Invoice = 4,
        Lost_Password = 5
    };

    #region "----------------------------public methods-------------------------"
    //
    /// <summary>
    /// select emailnotifaction Details
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItem()
    {
        StrQuery = " select isnull([FromEmail],'') as FromEmail ,isnull([EmailType],0) as EmailType ," +
                   " isnull([EmailSubject],'') as EmailSubject , " +
                   " isnull([EmailBody],'') as EmailBody    " +
                   " from [EmailNotifications] where EmailType =@EmailType  ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@EmailType ", SqlDbType.Int)).Value = EmailType;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    
    /// <summary>
    /// update emailnotification Details
    /// </summary>
    public void UpdateItem()
    {
        StrQuery = " update [EmailNotifications] set [FromEmail]=@FromEmail,[EmailSubject]=@EmailSubject ,[EmailBody]=@EmailBody where EmailType=@EmailType";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@FromEmail", SqlDbType.VarChar, 200)).Value = FromEmail;
            sqlcmd.Parameters.Add(new SqlParameter("@EmailType", SqlDbType.Int)).Value = EmailType;
            sqlcmd.Parameters.Add(new SqlParameter("@EmailSubject", SqlDbType.VarChar, 2000)).Value = EmailSubject;
            sqlcmd.Parameters.Add(new SqlParameter("@EmailBody", SqlDbType.NText)).Value = EmailBody;
           // sqlcmd.Parameters.Add(new SqlParameter("@EmailNotificationsId", SqlDbType.Int)).Value = EmailNotificationsId;

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }
    #endregion

}