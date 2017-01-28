using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for catalogsOptionManager
/// </summary>
public class catalogsOptionManager
{

    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public catalogsOptionManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "----------------------------private variables------------------------"

    //option

    private int _optionId;
    private int _brandid;
    private int _pricelevel;
    private char _priceRange;
    private int _ranges;
    private int _smartCatelogId;
    private bool _onlyProductwithPhoto;
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
    public int optionId { get { return _optionId; } set { _optionId = value; } }
    public int brandid { get { return _brandid; } set { _brandid = value; } }
    public int pricelevel { get { return _pricelevel; } set { _pricelevel = value; } }
    public char priceRange { get { return _priceRange; } set { _priceRange = value; } }
    public int ranges { get { return _ranges; } set { _ranges = value; } }
    public int smartCatelogId { get { return _smartCatelogId; } set { _smartCatelogId = value; } }
    public bool onlyProductwithPhoto { get { return _onlyProductwithPhoto; } set { _onlyProductwithPhoto = value; } }
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
            sqlCmd.Parameters.AddWithValue("@brandid", brandid);
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
    /// insert option details
    /// </summary>
    public void InsertOption()
    {
        StrQuery = "insert into catelogsOptions (brandid,pricelevel,priceRange,ranges,smartCatelogId,onlyProductwithPhoto) values (@brandid,@pricelevel,@priceRange,@ranges,@smartCatelogId,@onlyProductwithPhoto)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@brandid", brandid);
            sqlcmd.Parameters.AddWithValue("@pricelevel", pricelevel);
            sqlcmd.Parameters.AddWithValue("@priceRange", priceRange);
            sqlcmd.Parameters.AddWithValue("@ranges", ranges);
            sqlcmd.Parameters.AddWithValue("@smartCatelogId", smartCatelogId);
            sqlcmd.Parameters.AddWithValue("@onlyProductwithPhoto", onlyProductwithPhoto);

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
    /// update catelogs details
    /// </summary>
    public void UpdateOptions()
    {
        StrQuery = "update catelogsOptions set brandid=@brandid,pricelevel=@pricelevel,priceRange=@priceRange,ranges=@ranges,smartCatelogId=@smartCatelogId,onlyProductwithPhoto=@onlyProductwithPhoto where optionId=@optionId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@optionId", optionId);
            sqlcmd.Parameters.AddWithValue("@brandid", brandid);
            sqlcmd.Parameters.AddWithValue("@pricelevel", pricelevel);
            sqlcmd.Parameters.AddWithValue("@priceRange", priceRange);
            sqlcmd.Parameters.AddWithValue("@ranges", ranges);
            sqlcmd.Parameters.AddWithValue("@smartCatelogId", smartCatelogId);
            sqlcmd.Parameters.AddWithValue("@onlyProductwithPhoto", onlyProductwithPhoto);

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// delete option record by smartCatelogId 
    /// </summary>
    public void DeleteOption()
    {
        StrQuery = "delete from catelogsOptions where smartCatelogId=@smartCatelogId";
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
    /// get options detail single record for edit 
    /// </summary>
    /// <returns></returns>
    public DataTable GetSingleOptionRecord()
    {
        StrQuery = " select isnull(brandid,0) as brandid,isnull(pricelevel,0) as pricelevel,isnull(priceRange,'') as priceRange,isnull(ranges,'') as ranges ";
        StrQuery += ",isnull(onlyProductwithPhoto,0) as onlyProductwithPhoto from catelogsOptions where optionId=@optionId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@optionId", optionId);
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