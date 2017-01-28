using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for ContentManager
/// </summary>
public class ContentManager
{
    public ContentManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    String StrQuery;
    DataSet ds = new DataSet();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    public int Id { get; set; }
    public string ContentTitle { get; set; }
    public string ContentDesc { get; set; }
    public string MetaTitle { get; set; }
    public string MetaKeyword { get; set; }
    public string MetaDescription { get; set; }
    public bool DisplayStatus { get; set; }
    public int DisplayRank { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool success { get; set; }

    public int pageNo { get; set; }
    public int pageSize { get; set; }
    public int TotalRecord { get; set; }
    public string SortExpression { get; set; }

    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// search content type details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchContent]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@ContentTitle", ContentTitle);
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
    /// insert content details
    /// </summary>
    /// <returns></returns>
    public int InsertItem()
    {
        StrQuery += " declare @count int  set @count=0  select @count  =count(*) from [tblContentCms] where DisplayRank = @DisplayRank ";
        StrQuery += " if(@count=1)  update [tblContentCms] set DisplayRank = DisplayRank+1 where DisplayRank>=@DisplayRank ";
        StrQuery += " insert into [tblContentCms]([ContentTitle],[ContentDesc],[MetaTitle],[MetaKeyword],[MetaDescription],[DisplayStatus],[DisplayRank],[CreatedDate]) ";
        StrQuery += " values(@ContentTitle,@ContentDesc,@MetaTitle,@MetaKeyword,@MetaDescription,@DisplayStatus,@DisplayRank,@CreatedDate)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@ContentTitle", ContentTitle);
            sqlcmd.Parameters.AddWithValue("@ContentDesc", ContentDesc);
            sqlcmd.Parameters.AddWithValue("@MetaTitle", MetaTitle);
            sqlcmd.Parameters.AddWithValue("@MetaKeyword", MetaKeyword);
            sqlcmd.Parameters.AddWithValue("@MetaDescription", MetaDescription);
            sqlcmd.Parameters.AddWithValue("@DisplayStatus", DisplayStatus);
            sqlcmd.Parameters.AddWithValue("@DisplayRank", DisplayRank);
            sqlcmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update content details
    /// </summary>
    /// <param name="prevSort"></param>
    /// <param name="changeSort"></param>
    public void UpdateItem(int prevSort, int changeSort)
    {
        StrQuery = " declare @count int set @count =0 select @count = count(*) from tblContentCms where DisplayRank = @DisplayRank " +
                    " if(@count>=1)  begin  if(@prevSort > @changeSort) begin " +
                    " update tblContentCms set DisplayRank= DisplayRank+ 1 where DisplayRank < @prevSort and DisplayRank >=@changeSort end " +
                    " if(@prevSort < @changeSort) begin update tblContentCms set DisplayRank = DisplayRank - 1 where DisplayRank <= @changeSort and DisplayRank > @prevSort end end " +
                    " update [tblContentCms] set [ContentTitle]=@ContentTitle,[DisplayStatus]=@DisplayStatus,[ContentDesc]=@ContentDesc,[MetaTitle]=@MetaTitle,[MetaKeyword]=@MetaKeyword,[MetaDescription]=@MetaDescription, " +
                    " [DisplayRank]=@DisplayRank,ModifiedDate=@ModifiedDate " +
                    " where Id=@Id ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@id", Id);
            sqlcmd.Parameters.AddWithValue("@ContentTitle", ContentTitle);
            sqlcmd.Parameters.AddWithValue("@ContentDesc", ContentDesc);
            sqlcmd.Parameters.AddWithValue("@MetaTitle", MetaTitle);
            sqlcmd.Parameters.AddWithValue("@MetaKeyword", MetaKeyword);
            sqlcmd.Parameters.AddWithValue("@MetaDescription", MetaDescription);
            sqlcmd.Parameters.AddWithValue("@DisplayStatus", DisplayStatus);
            sqlcmd.Parameters.AddWithValue("@DisplayRank", DisplayRank);
            sqlcmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            sqlcmd.Parameters.Add(new SqlParameter("@prevSort", SqlDbType.Int)).Value = prevSort;
            sqlcmd.Parameters.Add(new SqlParameter("@changeSort", SqlDbType.Int)).Value = changeSort;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// delete content details by content id
    /// </summary>
    public void DeleteItem()
    {
        StrQuery = "delete from [tblContentCms] where Id=@Id";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    //
    /// <summary>
    /// Check content title is exist or not
    /// </summary>
    /// <returns></returns>
    public bool TitleExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(Id) from [tblContentCms] where ContentTitle = @ContentTitle";
            if (Id != 0)
            {
                StrQuery += "  and Id <> @Id";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@ContentTitle", ContentTitle);
            if (Id != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
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
    /// update content status
    /// </summary>
    public void UpdateStatus()
    {
        StrQuery = " update [tblContentCms] set [DisplayStatus]=@DisplayStatus where Id=@Id ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@DisplayStatus", SqlDbType.Bit)).Value = DisplayStatus;
            sqlcmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// get content single record 
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItemById()
    {
        StrQuery = " select isnull([Id],0) as eId,isnull([ContentTitle],'') as ContentTitle,isnull([ContentDesc],'') as ContentDesc,isnull([MetaDescription],'') as MetaDescription, " +
                   " isnull([MetaTitle],'') as MetaTitle,isnull([MetaKeyword],'') as MetaKeyword,isnull([DisplayRank],0) as DisplayRank,isnull([DisplayStatus],0) as DisplayStatus " +
                   " from [tblContentCms] where Id=@Id ";

        StrQuery += " order by ContentTitle ";
        DataTable dt = new DataTable();
        try
        {

            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// get the content all the record
    /// </summary>
    /// <returns></returns>
    public DataTable GetAllContent()
    {
        StrQuery = " select isnull([Id],0) as eId,isnull([ContentTitle],'') as ContentTitle,isnull([ContentDesc],'') as ContentDesc,isnull([MetaDescription],'') as MetaDescription, " +
                   " isnull([MetaTitle],'') as MetaTitle,isnull([MetaKeyword],'') as MetaKeyword,isnull([DisplayRank],0) as DisplayRank,isnull([DisplayStatus],0) as DisplayStatus " +
                   " from [tblContentCms] ";
        StrQuery += " order by ContentTitle ";
        DataTable dt = new DataTable();
        try
        {
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