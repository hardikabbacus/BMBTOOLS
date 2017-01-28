using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
/// <summary>
/// Summary description for smartCatelogsManager
/// </summary>
public class smartCatelogsManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public smartCatelogsManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "----------------------------private variables------------------------"

    //Payment

    private int _smartCatelogId;
    private string _catelogName;
    private string _companyLogo;
    private string _prepareFor;
    private int _brandId;
    public DateTime _CreateDate;
    public DateTime _UpdateDate;

    //other variable
    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    // order
    public int smartCatelogId { get { return _smartCatelogId; } set { _smartCatelogId = value; } }
    public string catelogName { get { return _catelogName; } set { _catelogName = value; } }
    public string companyLogo { get { return _companyLogo; } set { _companyLogo = value; } }
    public string prepareFor { get { return _prepareFor; } set { _prepareFor = value; } }
    public int brandId { get { return _brandId; } set { _brandId = value; } }
    public DateTime CreateDate { get { return _CreateDate; } set { _CreateDate = value; } }
    public DateTime UpdateDate { get { return _UpdateDate; } set { _UpdateDate = value; } }

    //other
    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    public string commonsearch { get; set; }

    #endregion

    #region

    //
    /// <summary>
    /// search smart catelogs details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchCatelogs]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@brandId", brandId);
            sqlCmd.Parameters.AddWithValue("@pageNo", pageNo);
            sqlCmd.Parameters.AddWithValue("@pageSize", pageSize);
            sqlCmd.Parameters.AddWithValue("@TotalRowsNum", TotalRecord);
            sqlCmd.Parameters.AddWithValue("@SortExpression", SortExpression);
            sqlCmd.Parameters["@TotalRowsNum"].Direction = ParameterDirection.Output;
            sqlCmd.Parameters["@TotalRowsNum"].SqlDbType = SqlDbType.Int;
            sqlCmd.Parameters["@TotalRowsNum"].Size = 4000;
            objcon.Open();
            sqlCmd.Connection = objcon;
            sqlCmd.CommandTimeout = 6000;
            SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
            sqlAdp.Fill(dt);
            TotalRecord = sqlCmd.Parameters["@TotalRowsNum"].Value == null ? 0 : Convert.ToInt32(sqlCmd.Parameters["@TOTALRowsNum"].Value);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// insert catelogs details
    /// </summary>
    public int InsertCatelogs()
    {
        StrQuery = "insert into tblSmartCatelogs (catelogName,companyLogo,prepareFor,brandId) values (@catelogName,@companyLogo,@prepareFor,@brandId)";
        StrQuery += " SELECT SCOPE_IDENTITY() ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@catelogName", catelogName);
            sqlcmd.Parameters.AddWithValue("@companyLogo", companyLogo);
            sqlcmd.Parameters.AddWithValue("@prepareFor", prepareFor);
            sqlcmd.Parameters.AddWithValue("@brandId", brandId);

            return (Convert.ToInt32(sqlcmd.ExecuteScalar()));
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// update catelogs details
    /// </summary>
    public void UpdateCatelogs()
    {
        StrQuery = "update tblSmartCatelogs set catelogName=@catelogName,companyLogo=@companyLogo,prepareFor=@prepareFor,brandId=@brandId where smartCatelogId=@smartCatelogId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@smartCatelogId", smartCatelogId);
            sqlcmd.Parameters.AddWithValue("@catelogName", catelogName);
            sqlcmd.Parameters.AddWithValue("@companyLogo", companyLogo);
            sqlcmd.Parameters.AddWithValue("@prepareFor", prepareFor);
            sqlcmd.Parameters.AddWithValue("@brandId", brandId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// update catelogs images details
    /// </summary>
    public void UpdateCatelogsImages()
    {
        StrQuery = "update tblSmartCatelogs set companyLogo=@companyLogo where smartCatelogId=@smartCatelogId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@smartCatelogId", smartCatelogId);
            sqlcmd.Parameters.AddWithValue("@companyLogo", companyLogo);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// delete catelogs record by catelogs id
    /// </summary>
    public void DeleteCatelogs()
    {
        StrQuery = "delete from tblSmartCatelogs where smartCatelogId=@smartCatelogId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@smartCatelogId", smartCatelogId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// get catelogs detail single record for edit 
    /// </summary>
    /// <returns></returns>
    public DataTable GetSingleCatelogsRecord()
    {
        StrQuery = " select isnull(catelogName,'') as catelogName,isnull(prepareFor,'') as prepareFor,isnull(brandId,0) as brandId ";
        StrQuery += " from tblSmartCatelogs where smartCatelogId=@smartCatelogId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@smartCatelogId", smartCatelogId);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            dt = new DataTable();
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    #endregion

}