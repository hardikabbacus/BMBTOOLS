using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ServicesManager
/// </summary>
public class ServicesManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

	public ServicesManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region "----------------------------private variables------------------------"

    private int _serviceid;
    private string _facebookUrl;
    private string _twitterUrl;
    private string _adrollAdvId;
    private string _adrollAdvPixId;
    private string _googleAnalytics;
    public string _googleMap;
    #endregion


    #region "----------------------------public properties------------------------"

    public int serviceid { get { return _serviceid; } set { _serviceid = value; } }
    public string facebookUrl { get { return _facebookUrl; } set { _facebookUrl = value; } }
    public string twitterUrl { get { return _twitterUrl; } set { _twitterUrl = value; } }
    public string adrollAdvId { get { return _adrollAdvId; } set { _adrollAdvId = value; } }
    public string adrollAdvPixId { get { return _adrollAdvPixId; } set { _adrollAdvPixId = value; } }
    public string googleAnalytics { get { return _googleAnalytics; } set { _googleAnalytics = value; } }
    public string googleMap { get { return _googleMap; } set { _googleMap = value; } }
   
    #endregion

    public enum EmailTemplate
    {
        New_User = 1,
        Order_Recieved = 2,
        Order_Shipped = 3,
        Invoice = 4,
        Lost_Password = 5
    };


    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// select single service Details
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItem()
    {
        StrQuery = " select isnull([facebookUrl ],'') as facebookUrl ,isnull([twitterUrl ],'') as twitterUrl ," +
                   " isnull([adrollAdvId],'') as adrollAdvId , " +
                   " isnull([adrollAdvPixId],'') as adrollAdvPixId ,   " +
                   " isnull([googleAnalytics],'') as googleAnalytics ,  " +
                   " isnull([googleMap],'') as googleMap  " +
                   " from [bmbservices] where serviceid =@serviceid  ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@serviceid ", SqlDbType.Int)).Value = serviceid;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    //
    /// <summary>
    /// update services Details
    /// </summary>
    public void UpdateItem()
    {
        StrQuery = " update [bmbservices] set [facebookUrl]=@facebookUrl ,[twitterUrl]=@twitterUrl ,[adrollAdvId]=@adrollAdvId ,[adrollAdvPixId]=@adrollAdvPixId ,[googleAnalytics]=@googleAnalytics,[googleMap]=@googleMap where serviceid=@serviceid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@facebookUrl", SqlDbType.VarChar, 200)).Value = facebookUrl;
            sqlcmd.Parameters.Add(new SqlParameter("@twitterUrl", SqlDbType.VarChar, 500)).Value = twitterUrl;
            sqlcmd.Parameters.Add(new SqlParameter("@adrollAdvId", SqlDbType.VarChar, 100)).Value = adrollAdvId;
            sqlcmd.Parameters.Add(new SqlParameter("@adrollAdvPixId", SqlDbType.VarChar, 100)).Value = adrollAdvPixId;
            sqlcmd.Parameters.Add(new SqlParameter("@googleAnalytics", SqlDbType.VarChar, 50)).Value = googleAnalytics;
            sqlcmd.Parameters.Add(new SqlParameter("@googleMap", SqlDbType.VarChar, 50)).Value = googleMap;
            sqlcmd.Parameters.Add(new SqlParameter("@serviceid", SqlDbType.Int)).Value = serviceid;

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }


    #endregion
}