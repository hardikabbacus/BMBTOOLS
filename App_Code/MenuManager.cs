using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for MenuManager
/// </summary>
public class MenuManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    public MenuManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region "----------------------------private variables------------------------"

    private int _idmenu;
    private string _title;
    private byte _isactive;
    private int _parentid;
    private string _pageurl;
    private int _sortorder;
    private string _imagepath;
    private byte _ismenu;
    private byte _isseparate;
    private string _menudesc;

    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    public int idmenu { get { return _idmenu; } set { _idmenu = value; } }
    public string title { get { return _title; } set { _title = value; } }
    public byte isactive { get { return _isactive; } set { _isactive = value; } }
    public int parentid { get { return _parentid; } set { _parentid = value; } }
    public string pageurl { get { return _pageurl; } set { _pageurl = value; } }
    public int sortorder { get { return _sortorder; } set { _sortorder = value; } }
    public string imagepath { get { return _imagepath; } set { _imagepath = value; } }
    public byte ismenu { get { return _ismenu; } set { _ismenu = value; } }
    public byte isseparate { get { return _isseparate; } set { _isseparate = value; } }
    public string menudesc { get { return _menudesc; } set { _menudesc = value; } }

    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    #endregion

    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// search Menu details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchAdminmenu]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@title", title);
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
    /// Select Sub Menu By Parent Menu id
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSubMenu()
    {
        StrQuery = " select isnull([idmenu],0) as idmenu,isnull([title],'') as title,isnull([isactive],0) as isactive," +
                   " isnull([parentid],0) as parentid , isnull([pageurl],'') as pageurl,isnull([sortorder],0) as sortorder," +
                   " isnull([imagepath],'') as imagepath,isnull([ismenu],'') as ismenu " +
                   " from [menu] where parentid=@parentid ";

        if (parentid != 0)
        {
            StrQuery += " and parentid=@parentid ";
        }
        StrQuery += " order by title ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            if (parentid != 0)
                sqlcmd.Parameters.Add(new SqlParameter("@parentid", SqlDbType.Int)).Value = parentid;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    //
    /// <summary>
    /// select single menu details by idmenu
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItemByMenuId()
    {
        StrQuery = " select isnull([idmenu],0) as idmenu,isnull([title],'') as title,isnull([isactive],0) as isactive," +
                   " isnull([parentid],0) as parentid , isnull([pageurl],'') as pageurl,isnull([sortorder],0) as sortorder," +
                   " isnull([imagepath],'') as imagepath,isnull([ismenu],'') as ismenu,isnull([menuDesc],'') as menuDesc,isnull([isseparate],0) as isseparate " +
                   " from [menu] where idmenu=@idmenu ";

        StrQuery += " order by title ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@idmenu", SqlDbType.Int)).Value = idmenu;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    //
    /// <summary>
    /// delete menu details by idmenu
    /// </summary>
    public void DeleteMenu()
    {
        StrQuery = "delete from [menu] where idmenu=@idmenu";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@idmenu", SqlDbType.Int)).Value = idmenu;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    //
    /// <summary>
    /// insert menu details
    /// </summary>
    /// <returns></returns>
    public int InsertItem()
    {
        StrQuery += " declare @count int  set @count=0  select @count  =count(*) from [menu] where sortorder = @sortorder ";
        StrQuery += " if(@count=1)  update [menu] set sortorder = sortorder+1 where sortorder>=@sortorder ";
        StrQuery += " insert into [menu]([title],[isactive],[parentid],[pageurl],[sortorder],[imagepath],[ismenu],[isseparate],[menuDesc]) values(@title,@isactive,@parentid,@pageurl,@sortorder,@imagepath,@ismenu,@isseparate,@menuDesc)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar, 50)).Value = title;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@parentid", SqlDbType.Int)).Value = parentid;
            sqlcmd.Parameters.Add(new SqlParameter("@pageurl", SqlDbType.VarChar, 200)).Value = pageurl;
            sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;
            sqlcmd.Parameters.Add(new SqlParameter("@imagepath", SqlDbType.VarChar, 50)).Value = imagepath;
            sqlcmd.Parameters.Add(new SqlParameter("@ismenu", SqlDbType.Int)).Value = ismenu;
            sqlcmd.Parameters.Add(new SqlParameter("@isseparate", SqlDbType.Int)).Value = isseparate;
            sqlcmd.Parameters.AddWithValue("@menuDesc", menudesc);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update menu details by idmenu
    /// </summary>
    /// <param name="prevSort"></param>
    /// <param name="changeSort"></param>
    public void UpdateItem(int prevSort, int changeSort)
    {
        StrQuery = " declare @count int set @count =0 select @count = count(*) from menu where sortorder = @sortorder " +
                    " if(@count>=1)  begin  if(@prevSort > @changeSort) begin " +
                    " update menu set sortorder= sortorder+ 1 where sortorder < @prevSort and sortorder >=@changeSort end " +
                    " if(@prevSort < @changeSort) begin update menu set sortorder = sortorder - 1 where sortorder <= @changeSort and sortorder > @prevSort end end " +
                    " update [menu] set [title]=@title,[isactive]=@isactive,[parentid]=@parentid,[pageurl]=@pageurl, " +
                    " [sortorder]=@sortorder,[imagepath]=@imagepath,[ismenu]=@ismenu,[isseparate]=@isseparate,[menuDesc]=@menuDesc " +
                    " where idmenu=@idmenu ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar, 50)).Value = title;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@parentid", SqlDbType.Int)).Value = parentid;
            sqlcmd.Parameters.Add(new SqlParameter("@pageurl", SqlDbType.VarChar, 200)).Value = pageurl;
            sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;
            sqlcmd.Parameters.Add(new SqlParameter("@imagepath", SqlDbType.VarChar, 50)).Value = imagepath;
            sqlcmd.Parameters.Add(new SqlParameter("@ismenu", SqlDbType.Int)).Value = ismenu;
            sqlcmd.Parameters.Add(new SqlParameter("@idmenu", SqlDbType.Int)).Value = idmenu;
            sqlcmd.Parameters.Add(new SqlParameter("@isseparate", SqlDbType.Int)).Value = isseparate;
            sqlcmd.Parameters.AddWithValue("@menuDesc", menudesc);
            sqlcmd.Parameters.Add(new SqlParameter("@prevSort", SqlDbType.Int)).Value = prevSort;
            sqlcmd.Parameters.Add(new SqlParameter("@changeSort", SqlDbType.Int)).Value = changeSort;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// get parent  menu
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public DataTable GetParentMenu(bool flag)
    {
        try
        {
            StrQuery = "select idmenu,title from [menu] where 1=1 ";

            if (flag == true)
            {
                StrQuery += " and parentid=0 ";
            }
            StrQuery += " and isactive = 1 order by sortorder ";

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
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get all the sub menu
    /// </summary>
    /// <returns></returns>
    public DataTable GetSubMenu()
    {
        StrQuery = "select idmenu,title from [menu] where 1=1 ";
        StrQuery += " and parentid=@parentid ";
        StrQuery += " and isactive = 1 order by sortorder ";
        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.Add(new SqlParameter("@parentid", SqlDbType.Int)).Value = parentid;
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    //
    /// <summary>
    /// Check Menu is already exist or not
    /// </summary>
    /// <returns></returns>
    public bool TitleExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(idmenu) from [menu] where title = @title";
            if (idmenu != 0)
            {
                StrQuery += "  and idmenu <> @idmenu";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar, 50)).Value = title;
            if (idmenu != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@idmenu", SqlDbType.Int)).Value = idmenu;
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
    /// update menu status
    /// </summary>
    public void UpdateStatus()
    {
        StrQuery = " update [menu] set [isactive]=@isactive where idmenu=@idmenu ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@idmenu", SqlDbType.Int)).Value = idmenu;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// fetch menu for admin id and parent id
    /// </summary>
    /// <returns></returns>
    public DataTable FetchMenu()
    {
        StrQuery = " select m.* from adminrights ar inner join menu m on m.idmenu=ar.idmenu where ar.adminid=@idmenu and m.parentid=@parentid order by m.sortorder";
        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@idmenu", idmenu);
            sqlcmd.Parameters.AddWithValue("@parentid", parentid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }
    /// <summary>
    /// fetch admin rights from adminid
    /// </summary>
    /// <returns></returns>
    public DataTable FetchMenuForHome()
    {
        StrQuery = " select m.* from adminrights ar inner join menu m on m.idmenu=ar.idmenu where ar.adminid=@idmenu order by m.sortorder";
        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@idmenu", idmenu);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }
    /// <summary>
    /// select menu for left
    /// </summary>
    /// <returns></returns>
    public DataTable SelectLeftmenu()
    {
        StrQuery = " select isnull([idmenu],0) as idmenu,isnull([title],'') as title,isnull([isactive],0) as isactive," +
                 " isnull([parentid],0) as parentid , isnull([pageurl],'') as pageurl,isnull([sortorder],0) as sortorder," +
                 " isnull([imagepath],'') as imagepath,isnull([ismenu],'') as ismenu " +
                 " from [menu] where parentid=@parentid and isactive=1 ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@parentid", parentid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    #endregion
}
