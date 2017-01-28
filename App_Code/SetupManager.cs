using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SetupManager
/// </summary>
public class SetupManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

	public SetupManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region "----------------------------private variables------------------------"

    private int _companyid;
    private string _companyname;
    private string _streetAddress;
    private string _city;
    private string _country;
    private string _telephone;
    public string _fax;
    public string _supportEmail;
    public string _aboutCompany;
    private string _imagepath;

    #endregion

    #region "----------------------------public properties------------------------"

    public int companyid { get { return _companyid; } set { _companyid= value; } }
    public string companyname { get { return _companyname; } set { _companyname = value; } }
    public string streetAddress { get { return _streetAddress; } set { _streetAddress = value; } }
    public string city { get { return _city; } set { _city = value; } }
    public string country { get { return _country; } set { _country = value; } }
    public string telephone { get { return _telephone; } set { _telephone = value; } }
    public string fax { get { return _fax; } set { _fax = value; } }
    public string supportEmail { get { return _supportEmail; } set { _supportEmail = value; } }
    public string aboutCompany { get { return _aboutCompany; } set { _aboutCompany = value; } }
    public string imagepath { get { return _imagepath; } set { _imagepath = value; } }

    #endregion

    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// select Company Details
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItem()
    {
        StrQuery = " select isnull([companyname ],'') as companyname ,isnull([streetAddress ],'') as streetAddress ," +
                   " isnull([city],'') as city , " +
                   " isnull([country],'') as country ,   " +
                   " isnull([telephone],'') as telephone ,  " +
                   " isnull([fax],'') as fax , isnull([logo],'') as logo , " +
                   " isnull([supportEmail],'') as supportEmail   , " +
                   " isnull([aboutCompany],'') as aboutCompany    " +
                   " from [company] where companyid =@companyid  ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@companyid ", SqlDbType.Int)).Value = companyid;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    //
    /// <summary>
    /// update Company Details
    /// </summary>
    public void UpdateItem()
    {
        StrQuery = " update [company] set [companyname]=@companyname ,[streetAddress]=@streetAddress ,[city]=@city ,[country]=@country ,[telephone]=@telephone,[fax]=@fax,[supportEmail]=@supportEmail,";
        if(imagepath !="")
        {
            StrQuery+="[logo]=@logo,";
        }
        StrQuery+="[aboutCompany]=@aboutCompany where companyid=@companyid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@companyname", SqlDbType.VarChar, 200)).Value = companyname;
            sqlcmd.Parameters.Add(new SqlParameter("@streetAddress", SqlDbType.VarChar,500)).Value = streetAddress;
            sqlcmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar, 100)).Value = city;
            sqlcmd.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar,100)).Value = country;
            sqlcmd.Parameters.Add(new SqlParameter("@telephone", SqlDbType.VarChar,50)).Value = telephone;
            sqlcmd.Parameters.Add(new SqlParameter("@fax", SqlDbType.VarChar,50)).Value = fax;
            sqlcmd.Parameters.Add(new SqlParameter("@supportEmail", SqlDbType.VarChar,150)).Value = supportEmail;
            sqlcmd.Parameters.Add(new SqlParameter("@logo", SqlDbType.VarChar, 500)).Value = imagepath;
            sqlcmd.Parameters.Add(new SqlParameter("@aboutCompany", SqlDbType.Text)).Value = aboutCompany;
            sqlcmd.Parameters.Add(new SqlParameter("@companyid", SqlDbType.Int)).Value = companyid;

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }


    #endregion

}