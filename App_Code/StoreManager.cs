using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StoreManager
/// </summary>
public class StoreManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

	public StoreManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region "----------------------------private variables------------------------"

    private int _storeid;
    private string _product_listing;
    private int _category;
    private string _sorting;
    private int _filter;
    private string _imgnotfound;
    #endregion

    #region "----------------------------public properties------------------------"

    public int storeid { get { return _storeid; } set { _storeid= value; } }
    public string product_listing { get { return _product_listing; } set { _product_listing = value; } }
    public int category { get { return _category; } set { _category = value; } }
    public string sorting { get { return _sorting; } set { _sorting = value; } }
    public int filter { get { return _filter; } set { _filter = value; } }
    public string imgnotfound { get { return _imgnotfound; } set { _imgnotfound = value; } }

    #endregion

    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// select storesetting Details
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItem()
    {
        StrQuery = " select isnull([product_listing ],'') as product_listing ,isnull([category ],0) as category ," +
                   " isnull([sorting],'') as sorting , " +
                   " isnull([filter],0) as filter ,   " +
                   " isnull([imgnotfound],'') as imgnotfound   " +
                   " from [bmbstoresetting] where storeid =@storeid  ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@storeid ", SqlDbType.Int)).Value = storeid;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    //
    /// <summary>
    /// update storesetting Details
    /// </summary>
    public void UpdateItem()
    {
        StrQuery = " update [bmbstoresetting] set [product_listing]=@product_listing ,[category]=@category ,[sorting]=@sorting ,[filter]=@filter ";
        if (imgnotfound != "")
        {
            StrQuery += ",[imgnotfound]=@imgnotfound ";
        }
        StrQuery += " where storeid=@storeid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@product_listing", SqlDbType.VarChar, 500)).Value = product_listing;
            sqlcmd.Parameters.Add(new SqlParameter("@category", SqlDbType.Int)).Value = category;
            sqlcmd.Parameters.Add(new SqlParameter("@sorting", SqlDbType.VarChar, 100)).Value = sorting;
            sqlcmd.Parameters.Add(new SqlParameter("@filter", SqlDbType.Int)).Value = filter;
            sqlcmd.Parameters.Add(new SqlParameter("@imgnotfound", SqlDbType.VarChar, 50)).Value = imgnotfound;
            sqlcmd.Parameters.Add(new SqlParameter("@storeid", SqlDbType.Int)).Value = storeid;

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }


    #endregion
}