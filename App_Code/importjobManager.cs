using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for importjobManager
/// </summary>
public class importjobManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public importjobManager()
    {
    }
    #endregion

    #region "----------------------------private variables------------------------"

    private int _id;
    private string _importfilename;
    private string _filestatus;
    private string _productname;
    private string _importType;
    private string _createDate;


    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    public int id { get { return _id; } set { _id = value; } }
    public string importfilename { get { return _importfilename; } set { _importfilename = value; } }
    public string filestatus { get { return _filestatus; } set { _filestatus = value; } }
    public string productname { get { return _productname; } set { _productname = value; } }
    public string importType { get { return _importType; } set { _importType = value; } }
    public string createDate { get { return _createDate; } set { _createDate = value; } }

    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    #endregion

    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// search import job details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchImportJob]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@importfilename", importfilename);
            sqlCmd.Parameters.AddWithValue("@importType", importType);
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

    //
    /// <summary>
    /// insert import job details
    /// </summary>
    public void InsertItem()
    {
        StrQuery += " insert into [importjobhistory]([importfilename],[filestatus],[importType]) values(@importfilename,@filestatus,@importType)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@importfilename", SqlDbType.VarChar, 100)).Value = importfilename;
            sqlcmd.Parameters.Add(new SqlParameter("@filestatus", SqlDbType.VarChar, 20)).Value = filestatus;
            sqlcmd.Parameters.Add(new SqlParameter("@importType", SqlDbType.VarChar, 20)).Value = importType;

            sqlcmd.ExecuteScalar();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// update import jobs details
    /// </summary>
    public void UpdateItem()
    {
        StrQuery = " update [importjobhistory] set [filestatus]=@filestatus where id=@id ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
            //sqlcmd.Parameters.Add(new SqlParameter("@importfilename", SqlDbType.VarChar, 100)).Value = importfilename;
            sqlcmd.Parameters.Add(new SqlParameter("@filestatus", SqlDbType.VarChar, 100)).Value = filestatus;

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// check the file name is exist or not
    /// </summary>
    /// <returns></returns>
    public int isExistFileName()
    {
        StrQuery = "select count(id) from importjobhistory where importfilename=@importfilename";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@importfilename", importfilename);
            int cntid = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return cntid;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// delete importjobs details
    /// </summary>
    public void DeleteImportJobs()
    {
        StrQuery = "delete from [importjobhistory] where id=@id";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    //
    /// <summary>
    /// delete data from import product job table
    /// </summary>
    public void deleteImportJobTable()
    {
        StrQuery = "delete from tmp_productImport where ImportFileId=@id";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@id", id);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get the maximum id from importjob history
    /// </summary>
    /// <returns></returns>
    public int getmaxid()
    {
        StrQuery = "Select Max(id) from importjobhistory";
        int i, strId = 0;
        SqlCommand objCmd = null;
        try
        {
            objcon.Open();
            objCmd = new SqlCommand(StrQuery, objcon);
            if (objCmd.ExecuteScalar() != null)
            {
                i = (int)objCmd.ExecuteScalar();
            }
            else
            {
                i = 0;
            }

            strId = (i == null ? 0 : i);
            return strId;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); objCmd.Dispose(); }
    }

    //
    /// <summary>
    /// delete data from import product job table
    /// </summary>
    public void RemoveSingleImportjob()
    {
        StrQuery = "delete from tmp_productImport where id=@id";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@id", id);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    #endregion

    //public void RemoveSingleImportjob()
    //{
    //    StrQuery = "delete from importjobhistory where id=@id";
    //    try
    //    {
    //        objcon.Open();
    //        SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
    //        sqlcmd.Parameters.AddWithValue("@id", id);
    //        sqlcmd.ExecuteNonQuery();
    //    }
    //    catch (Exception e)
    //    {
    //        throw e;
    //    }
    //    finally { objcon.Close(); }
    //}

    #region -------------------- Product Import Table Search ----------------

    //
    /// <summary>
    /// search import job details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchImportJobs()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchImportJobs]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@productname", productname);
            sqlCmd.Parameters.AddWithValue("@ImportFileId", id);
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
    /// search import job inventory details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchImportInventoryJobs()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchImportInventoryJobs]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@productname", productname);
            sqlCmd.Parameters.AddWithValue("@ImportFileId", id);
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

    #endregion


    public string SearchKey { get; set; }
    public DataTable SearchKeywordImportJobs()
    {
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = objcon;
            cmd.CommandText = "sp_SearchKeywordImportJobs";
            cmd.Parameters.AddWithValue("@SearchKey", SearchKey);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

}