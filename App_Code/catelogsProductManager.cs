using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for catelogsProductManager
/// </summary>
public class catelogsProductManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public catelogsProductManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "----------------------------private variables------------------------"

    //Payment

    private int _CatelogProductId;
    private int _smartCatelogId;
    private int _productId;
    private int _sortOrder;
    private char _types;
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
    public int CatelogProductId { get { return _CatelogProductId; } set { _CatelogProductId = value; } }
    public int smartCatelogId { get { return _smartCatelogId; } set { _smartCatelogId = value; } }
    public int productId { get { return _productId; } set { _productId = value; } }
    public int sortOrder { get { return _sortOrder; } set { _sortOrder = value; } }
    public char types { get { return _types; } set { _types = value; } }
    public DateTime CreateDate { get { return _CreateDate; } set { _CreateDate = value; } }
    public DateTime UpdateDate { get { return _UpdateDate; } set { _UpdateDate = value; } }

    //other
    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    public string commonsearch { get; set; }
    public string categoryid { get; set; }
    public string skuList { get; set; }

    #endregion

    #region

    //
    /// <summary>
    /// search catelogs product details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchCatelogsProductCategory]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@Types", types);
            sqlCmd.Parameters.AddWithValue("@CategoryId", categoryid);
            sqlCmd.Parameters.AddWithValue("@skuList", skuList);
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
    /// insert catelogs product details
    /// </summary>
    public void InsertCatelogsProduct()
    {
        StrQuery = "insert into tblCatelogsProduct (smartCatelogId,productId,sortOrder,types) values (@smartCatelogId,@productId,@sortOrder,@types)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@smartCatelogId", smartCatelogId);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@sortOrder", sortOrder);
            sqlcmd.Parameters.AddWithValue("@types", types);

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// update catelogs product details
    /// </summary>
    public void UpdateCatelogsProduct()
    {
        StrQuery = "update tblCatelogsProduct set smartCatelogId=@smartCatelogId,productId=@productId,sortOrder=@sortOrder,types=@types where CatelogProductId=@CatelogProductId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@CatelogProductId", CatelogProductId);
            sqlcmd.Parameters.AddWithValue("@smartCatelogId", smartCatelogId);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@sortOrder", sortOrder);
            sqlcmd.Parameters.AddWithValue("@types", types);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// delete catelogs product record by catelogs product id
    /// </summary>
    public void DeleteCatelogsProduct()
    {
        StrQuery = "delete from tblCatelogsProduct where CatelogProductId=@CatelogProductId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@CatelogProductId", CatelogProductId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get catelogs detail single record for edit 
    /// </summary>
    /// <returns></returns>
    public DataTable GetSingleCatelogsProductRecord()
    {
        StrQuery = " select isnull(p.productName,'') as productName,ISNULL(cp.sortorder,0) as sortorder,ISNULL(cp.productid,0) as productid,isnull(types,0) as types ";
        StrQuery += " from tblCatelogsProduct as cp ";
        StrQuery += " inner join products as p on p.productId=cp.productId where CatelogProductId=@CatelogProductId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@CatelogProductId", CatelogProductId);
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