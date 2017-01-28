using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for languageManager
/// </summary>
public class languageManager
{

    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public languageManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "----------------------------private variables------------------------"

    private int _languageId;
    private string _languageName;
    private byte _isactive;
    private char _textAlign;

    private int _sortorder;

    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    public int languageId { get { return _languageId; } set { _languageId = value; } }
    public string languageName { get { return _languageName; } set { _languageName = value; } }
    public byte isactive { get { return _isactive; } set { _isactive = value; } }
    public char textAlign { get { return _textAlign; } set { _textAlign = value; } }

    public int sortorder { get { return _sortorder; } set { _sortorder = value; } }

    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    #endregion

    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// search language details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchLanguage]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@languagename", languageName);
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
    /// insert language details
    /// </summary>
    /// <returns></returns>
    public int InsertItem()
    {
        //StrQuery += " declare @count int  set @count=0  select @count  =count(*) from [menu] where sortorder = @sortorder ";
        //StrQuery += " if(@count=1)  update [menu] set sortorder = sortorder+1 where sortorder>=@sortorder ";
        StrQuery += " insert into [language]([languageName],[isactive],[textAlign]) values(@languageName,@isactive,@textAlign)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@languageName", SqlDbType.VarChar, 50)).Value = languageName;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@textAlign", SqlDbType.Char)).Value = textAlign;
            //sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;

            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update language details by languageid
    /// </summary>
    public void UpdateItem()
    {
        StrQuery = " update language set languageName=@languageName,isactive=@isactive,textAlign=@textAlign where languageId=@languageId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;
            sqlcmd.Parameters.Add(new SqlParameter("@languageName", SqlDbType.VarChar, 50)).Value = languageName;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@textAlign", SqlDbType.Char)).Value = textAlign;
            //sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// Check language name is already exist or not
    /// </summary>
    /// <returns></returns>
    public bool TitleExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(languageId) from [language] where languageName = @languageName";
            if (languageId != 0)
            {
                StrQuery += "  and languageId <> @languageId";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@languageName", SqlDbType.VarChar, 50)).Value = languageName;
            if (languageId != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;
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
    /// update language status
    /// </summary>
    public void UpdateStatus()
    {
        StrQuery = " update [language] set [isactive]=@isactive where languageId=@languageId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// delete language status
    /// </summary>
    public void DeleteItem()
    {
        StrQuery = " delete from  [language] where languageId=@languageId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// get single language
    /// </summary>
    /// <returns></returns>
    public DataTable GetSinglelanguage()
    {
        StrQuery = " select * from  [language] where languageId=@languageId ";
        dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }

    #endregion

}