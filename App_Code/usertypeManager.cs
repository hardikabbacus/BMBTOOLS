using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for usertypeManager
/// </summary>
public class usertypeManager
{

    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public usertypeManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "----------------------------private variables------------------------"

    private int _adminTypeId;
    private string _typeName;
    private byte _isactive;
    private int _sortorder;
    private string _updatedate;
    public string _createDate;


    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    public int adminTypeId { get { return _adminTypeId; } set { _adminTypeId = value; } }
    public string typeName { get { return _typeName; } set { _typeName = value; } }
    public byte isactive { get { return _isactive; } set { _isactive = value; } }
    public int sortorder { get { return _sortorder; } set { _sortorder = value; } }
    public string updatedate { get { return _updatedate; } set { _updatedate = value; } }
    public string createDate { get { return _createDate; } set { _createDate = value; } }

    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    #endregion

    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// search admin type details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchAdmintype]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@typeName", typeName);
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
    /// insert admin type details
    /// </summary>
    /// <returns></returns>
    public int InsertItem()
    {
        StrQuery += " declare @count int  set @count=0  select @count  =count(*) from [adminType] where sortorder = @sortorder ";
        StrQuery += " if(@count=1)  update [adminType] set sortorder = sortorder+1 where sortorder>=@sortorder ";
        StrQuery += " insert into [adminType]([typeName],[isactive],[sortorder]) values(@typeName,@isactive,@sortorder)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@typeName", SqlDbType.VarChar, 50)).Value = typeName;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;

            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update admin type details by idmenu
    /// </summary>
    /// <param name="prevSort"></param>
    /// <param name="changeSort"></param>
    public void UpdateItem(int prevSort, int changeSort)
    {
        StrQuery = " declare @count int set @count =0 select @count = count(*) from adminType where sortorder = @sortorder " +
                    " if(@count>=1)  begin  if(@prevSort > @changeSort) begin " +
                    " update adminType set sortorder= sortorder+ 1 where sortorder < @prevSort and sortorder >=@changeSort end " +
                    " if(@prevSort < @changeSort) begin update adminType set sortorder = sortorder - 1 where sortorder <= @changeSort and sortorder > @prevSort end end " +
                    " update [adminType] set [typeName]=@typeName,[isactive]=@isactive, " +
                    " [sortorder]=@sortorder,updatedate=@updatedate " +
                    " where adminTypeId=@adminTypeId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@adminTypeId", SqlDbType.Int)).Value = adminTypeId;
            sqlcmd.Parameters.Add(new SqlParameter("@typeName", SqlDbType.VarChar, 50)).Value = typeName;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;
            sqlcmd.Parameters.Add(new SqlParameter("@updatedate", SqlDbType.DateTime)).Value = DateTime.Now;
            sqlcmd.Parameters.Add(new SqlParameter("@prevSort", SqlDbType.Int)).Value = prevSort;
            sqlcmd.Parameters.Add(new SqlParameter("@changeSort", SqlDbType.Int)).Value = changeSort;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// delete type by idmenu
    /// </summary>
    public void DeleteAdminType()
    {
        StrQuery = "delete from [adminType] where adminTypeId=@adminTypeId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@adminTypeId", SqlDbType.Int)).Value = adminTypeId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    //
    /// <summary>
    /// Check admintype is already exist or not
    /// </summary>
    /// <returns></returns>
    public bool TitleExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(adminTypeId) from [adminType] where typeName = @typeName";
            if (adminTypeId != 0)
            {
                StrQuery += "  and adminTypeId <> @adminTypeId";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@typeName", SqlDbType.VarChar, 50)).Value = typeName;
            if (adminTypeId != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@adminTypeId", SqlDbType.Int)).Value = adminTypeId;
            }

            id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update admintype status
    /// </summary>
    public void UpdateStatus()
    {
        StrQuery = " update [adminType] set [isactive]=@isactive where adminTypeId=@adminTypeId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@adminTypeId", SqlDbType.Int)).Value = adminTypeId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// select single admintype details 
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItemById()
    {
        StrQuery = " select isnull([adminTypeId],0) as adminTypeId,isnull([typeName],'') as typeName,isnull([isactive],0) as isactive," +
                   " isnull([updatedate],'') as updatedate,isnull([createDate],'') as createDate,isnull([sortorder],0) as sortorder " +
                   " from [adminType] where adminTypeId=@adminTypeId ";

        StrQuery += " order by typeName ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@adminTypeId", SqlDbType.Int)).Value = adminTypeId;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// get the admin type 
    /// </summary>
    /// <returns></returns>
    public DataTable GetAllUserType()
    {
        StrQuery = " select isnull([adminTypeId],0) as adminTypeId,isnull([typeName],'') as typeName " +
                   " from [adminType] where isactive=1 order by typeName asc";

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