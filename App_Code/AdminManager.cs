using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for AdminManager
/// </summary>
public class AdminManager
{
    public AdminManager()
    {
        // TODO: Add constructor logic here
    }

    String StrQuery;
    DataSet ds = new DataSet();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);


    #region "----------------------------private variables------------------------"
    private int _adminid;
    private string _firstname;
    private string _lastname;
    private string _emailaddress;
    private string _userid;
    private string _password;
    private int _admintypeid;
    private byte _isactive;
    private string _mobile;

    private int _idmenu;
    private string _title;
    private int _parentid;
    private string _pageurl;
    private int _sortorder;

    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"
    public int adminid { get { return _adminid; } set { _adminid = value; } }
    public string userid { get { return _userid; } set { _userid = value; } }
    public string password { get { return _password; } set { _password = value; } }
    public string firstname { get { return _firstname; } set { _firstname = value; } }
    public string lastname { get { return _lastname; } set { _lastname = value; } }
    public string emailaddress { get { return _emailaddress; } set { _emailaddress = value; } }
    public int admintypeid { get { return _admintypeid; } set { _admintypeid = value; } }
    public byte isactive { get { return _isactive; } set { _isactive = value; } }
    public int idmenu { get { return _idmenu; } set { _idmenu = value; } }
    public string title { get { return _title; } set { _title = value; } }
    public int parentid { get { return _parentid; } set { _parentid = value; } }
    public string pageurl { get { return _pageurl; } set { _pageurl = value; } }
    public int sortorder { get { return _sortorder; } set { _sortorder = value; } }
    public string mobile { get { return _mobile; } set { _mobile = value; } }

    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }


    #endregion
    #region "----------------------------public methods---------------------------"

    /// <summary>
    /// Check Admin Authentication 
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //check user authentication
    public DataSet CheckAuthentication()
    {
        //StrQuery = " select isnull([adminid],0) as adminid,isnull([userid],'') as userid,isnull([password],'') as password,isnull([firstname],'') as firstname,isnull([lastname],'') as lastname,isnull([emailaddress],'') as emailaddress,isnull(admintypeid,0) as admintypeid from administrator where userid=@userid and password=@password and isactive=1";

        StrQuery = "select isnull(a.[adminid],0) as adminid,isnull(a.[userid],'') as userid,isnull(a.[password],'') as password, ";
        StrQuery += " isnull(a.[firstname],'') as firstname,isnull(a.[lastname],'') as lastname,isnull(a.[emailaddress],'') as emailaddress ";
        StrQuery += " ,isnull(a.[admintypeid],0) as admintypeid,ISNULL(at.[typeName],'') as typeName ";
        StrQuery += " from administrator as a ";
        StrQuery += " inner join adminType as at on at.adminTypeId = a.admintypeid ";
        StrQuery += " where userid=@userid and password=@password and a.isActive=1";

        try
        {
            ds = new DataSet();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@userid", userid);
            sqlcmd.Parameters.AddWithValue("@password", password);
            SqlDataAdapter sqladp = new SqlDataAdapter(sqlcmd);

            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Get Admin Detail By UserId
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //check user forgot password
    public DataSet ForgotPasswordAdmin()
    {
        StrQuery = " Select isnull([adminid],0) as adminid,isnull([userid],'') as userid,isnull([password],'') as password,isnull([firstname],'') as firstname,isnull([lastname],'') as lastname,isnull([emailaddress],'') as emailaddress from administrator where userid=@userid";
        try
        {
            ds = new DataSet();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@userid", userid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Check Whether anyother is register with this emailid 
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //check user email id exists
    public Boolean EmailIdExist()
    {
        try
        {
            int id;
            StrQuery = "select count(adminid) from administrator where emailaddress = @emailid";

            if (adminid != 0)
            {
                StrQuery += " and adminid <> @adminid";
            }

            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            objcon.Open();
            sqlcmd.Parameters.AddWithValue("@emailid", emailaddress);
            if (adminid != 0)
            {
                sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            }

            id = Convert.ToInt32(sqlcmd.ExecuteScalar());

            if (id == 0)
                return false;
            else
                return true;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// Check Whether anyother is register with this userid
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //check user email id exists
    public Boolean UseridExist()
    {
        try
        {
            int id;
            StrQuery = "select count(adminid) from administrator where userid = @userid";

            if (adminid != 0)
            {
                StrQuery += " and adminid <> @adminid";
            }

            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            objcon.Open();
            sqlcmd.Parameters.AddWithValue("@userid", userid);
            if (adminid != 0)
            {
                sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            }

            id = Convert.ToInt32(sqlcmd.ExecuteScalar());

            if (id == 0)
                return false;
            else
                return true;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }


    /// <summary>
    /// Search Item
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //search admin
    public DataTable SearchItem()
    {

        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchUser]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            //sqlCmd.Parameters.AddWithValue("@userid", userid);
            //sqlCmd.Parameters.AddWithValue("@emailaddress", emailaddress);
            sqlCmd.Parameters.AddWithValue("@firstname", firstname);
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
    /// Select User Detail Order By First Name
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //select all admin items
    public DataSet SelectAllItem()
    {
        StrQuery = "select isnull([adminid],0) as adminid,isnull([userid],'') as userid,isnull([password],'') as password,isnull([firstname],'') as firstname,isnull([lastname],'') as lastname,isnull([emailaddress],'') as emailaddress from administrator order by firstname ";

        try
        {
            ds = new DataSet();
            SqlDataAdapter sqladp = new SqlDataAdapter(StrQuery, objcon);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); }
    }

    /// <summary>
    /// Select Admin Detail By AdminId 
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //select single admin details
    public DataSet SelectSingleItem()
    {
        StrQuery = "select isnull([adminid],0) as adminid,isnull([userid],'') as userid,isnull([password],'') as password,isnull([firstname],'') as firstname,isnull([lastname],'') as lastname,isnull([emailaddress],'') as emailaddress,isnull(isActive,0) as isActive,isnull(admintypeid,0) as admintypeid,isnull(mobile,'') as mobile from administrator where adminid=@adminid";
        StrQuery += " select isnull([idmenu],0) as SelectedAdminMenuId,isnull(adminid,0) as userid from adminrights where adminid=@adminid ";

        try
        {
            ds = new DataSet();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Select Admin Detail By UserId 
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //select admin details by userid
    public DataSet SelectAdminByUserId()
    {
        StrQuery = "select isnull([adminid],0) as adminid,isnull([userid],'') as userid,isnull([password],'') as password,isnull([firstname],'') as firstname,isnull([lastname],'') as lastname,isnull([emailaddress],'') as emailaddress from administrator where userid=@userid";

        try
        {
            ds = new DataSet();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@userid", userid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    public void InsertUserMenu(string strQ)
    {
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(strQ, objcon);
            //sqlcmd.Parameters.Add(new SqlParameter("@delete_yn", SqlDbType.Char, 1)).Value = delete_yn;
            //sqlcmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.Int)).Value = user_id;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// Delete Admin Detail By AdminId 
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //delete admin details
    public void DeleteItem()
    {
        StrQuery = "delete from administrator where adminid=@adminid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    /// <summary>
    /// Insert Admin Detail 
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //insert admin details
    public int InsertItem()
    {
        StrQuery = "insert into administrator(userid,password,firstname,lastname,emailaddress,isActive,creationdate,modificationdate,admintypeid,mobile) values(@userid,@password,@firstname,@lastname,@emailaddress,@isactive,getutcdate(),getutcdate(),@admintypeid,@mobile)";
        StrQuery += " SELECT SCOPE_IDENTITY()";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@userid", userid);
            sqlcmd.Parameters.AddWithValue("@password", password);
            sqlcmd.Parameters.AddWithValue("@firstname", firstname);
            sqlcmd.Parameters.AddWithValue("@lastname", lastname);
            sqlcmd.Parameters.AddWithValue("@emailaddress", emailaddress);
            sqlcmd.Parameters.AddWithValue("@admintypeid", admintypeid);//admintypeid
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            sqlcmd.Parameters.AddWithValue("@mobile", mobile);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// Update Admin Detail 
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //update admin details
    public void UpdateItem()
    {
        StrQuery = "update administrator set userid=@userid,password=@password,firstname=@firstname,lastname=@lastname,emailaddress=@emailaddress,modificationdate=getutcdate(),isactive=@isactive,admintypeid=@admintypeid,mobile=@mobile where adminid=@adminid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@userid", userid);
            sqlcmd.Parameters.AddWithValue("@password", password);
            sqlcmd.Parameters.AddWithValue("@firstname", firstname);
            sqlcmd.Parameters.AddWithValue("@lastname", lastname);
            sqlcmd.Parameters.AddWithValue("@emailaddress", emailaddress);
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            sqlcmd.Parameters.AddWithValue("@admintypeid", admintypeid);//admintypeid
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            sqlcmd.Parameters.AddWithValue("@mobile", mobile);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// Get Admin Name By AdminId 
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //fetch admin name
    public string getName()
    {
        StrQuery = "select firstname + ' ' + lastname from administrator where adminid =@adminid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            return Convert.ToString(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }
    #endregion

    #region "----------------------------public methods---------------------------"

    /// <summary>
    /// Get AdminSide Menu List
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //select admin menu
    public DataSet SelectMenuItem()
    {
        StrQuery = "select isnull([idmenu],0) as idmenu,isnull([title],'') as title,isnull([isactive],0) as isactive " +
                   " ,isnull([parentid],0) as parentid,isnull([pageurl],'') as pageurl,isnull([sortorder],0) as sortorder " +
                   " from [menu] where isactive=1";

        try
        {
            ds = new DataSet();
            SqlDataAdapter sqladp = new SqlDataAdapter(StrQuery, objcon);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); }
    }

    /// <summary>
    /// Get the Parent Menu List
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //select single admin parent menu items
    public DataSet selectParentMenus()
    {
        StrQuery = "select isnull(menu.[idmenu],0) as idmenu,isnull([title],'') as title,isnull(menu.[isactive],0) as isactive " +
                   " ,isnull([parentid],0) as parentid,isnull([pageurl],'') as pageurl,isnull([sortorder],0) as sortorder " +
                   " from [menu] " +
                   " where parentid=0 and isactive=1 and ismenu=1  order by sortorder";

        try
        {
            ds = new DataSet();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Get the Sub-Menu List
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //select single admin sub menu items
    public DataSet selectSubMenus()
    {
        StrQuery = "select isnull(menu.[idmenu],0) as idmenu,isnull([title],'') as title,isnull(menu.[isactive],0) as isactive " +
                   " ,isnull([parentid],0) as parentid,isnull([pageurl],'') as pageurl,isnull([sortorder],0) as sortorder " +
                   " from [menu] " +
                   " where parentid=@parentid and isactive=1 and ismenu=1 order by sortorder";

        try
        {
            ds = new DataSet();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@parentid", parentid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Get ParentMenuItem By Admin Rights
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //select single admin parent menu items
    public DataTable selectAdminParentMenus()
    {
        StrQuery = "  select isnull(menu.[idmenu],0) as idmenu,isnull([title],'') as title,isnull(menu.[isactive],0) as isactive ";
        StrQuery += " ,isnull([parentid],0) as parentid,isnull([pageurl],'') as pageurl,isnull([sortorder],0) as sortorder ";
        StrQuery += " from [menu] ";
        StrQuery += " inner join adminRights on adminRights.idmenu = menu.idmenu ";
        //StrQuery += " inner join administrator on administrator.adminid = adminRights.adminid ";
        //StrQuery += " where administrator.adminid=@adminid and parentid=0  order by sortorder";
        StrQuery += " inner join administrator on administrator.admintypeid = adminRights.adminid ";
        StrQuery += " where administrator.admintypeid=@admintypeid and parentid=0 ";
        StrQuery += "group by menu.idmenu,title,menu.isactive,parentid,pageurl,sortorder order by sortorder ";


        //" where administrator.adminid=@adminid order by sortorder";

        try
        {
            DataTable dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@admintypeid", admintypeid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Get SubMenuItem By Admin Rights
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //select single admin sub menu items
    public DataTable selectAdminSubMenus()
    {
        StrQuery = "select isnull(menu.[idmenu],0) as idmenu,isnull([title],'') as title,isnull(menu.[isactive],0) as isactive " +
                   " ,isnull([parentid],0) as parentid,isnull([pageurl],'') as pageurl,isnull([sortorder],0) as sortorder " +
                   " from [menu] " +
                   //" inner join adminRights on adminRights.idmenu = menu.idmenu " +
                   //" inner join administrator on administrator.adminid = adminRights.adminid " +
                   //" where administrator.adminid=@adminid and parentid=@parentid  order by sortorder";
                   " where parentid=@parentid  order by sortorder";

        try
        {
            DataTable dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            sqlcmd.Parameters.AddWithValue("@parentid", parentid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Delete Menu Rights By AdminId
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //delete admin rights
    public void DeleteAdminRightsItem()
    {
        StrQuery = "delete from adminRights where adminid=@adminid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    /// <summary>
    /// Insert Menu Rights By AdminId
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //insert admin details
    public void InsertAdminRolesItem()
    {
        StrQuery = "insert into [adminRights]([idmenu],[adminid]) values (@idmenu,@adminid)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@idmenu", idmenu);
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// Get The Menu Count For Admin
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //parent child count for admin
    public int getAdminmenu()
    {
        StrQuery = " select count(*) from [menu] " +
                   " inner join adminRights on adminRights.idmenu = menu.idmenu " +
                   " inner join administrator on administrator.adminid = adminRights.adminid " +
                   " where adminRights.idmenu=@idmenu and administrator.adminid=@adminid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@idmenu", SqlDbType.Int)).Value = idmenu;
            sqlcmd.Parameters.Add(new SqlParameter("@adminid", SqlDbType.Int)).Value = adminid;
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// Get The Top SubMenu Count For Admin
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //top menu count for admin
    public int gettopmenuchild()
    {
        StrQuery = " select count(*) from [menu] " +
                   " inner join adminRights on adminRights.idmenu = menu.idmenu " +
                   " inner join administrator on administrator.adminid = adminRights.adminid " +
                   " where administrator.adminid=@adminid" +
                   " and menu.parentid=@idmenu ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@idmenu", SqlDbType.Int)).Value = idmenu;
            sqlcmd.Parameters.Add(new SqlParameter("@adminid", SqlDbType.Int)).Value = adminid;
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// Bind The Top Menu For Admin
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //make top menu bar
    public string getTopMenu()
    {
        string strmenu = string.Empty;
        DataTable dtmenu = new DataTable();
        dtmenu = selectAdminParentMenus();
        strmenu += "<ul>\n";
        strmenu += "\t<li><a href='home.aspx' title='Home'><img src='images/icon6.png' style='vertical-align: middle;width:20px;float:left;margin-top:12px;' alt=''><span>Home</span></a></li>\n";
        if (dtmenu.Rows.Count > 0)
        {
            for (int i = 0; i < dtmenu.Rows.Count; i++)
            {
                _idmenu = Convert.ToInt32(dtmenu.Rows[i]["idmenu"]);
                if (gettopmenuchild() > 0)
                {
                    strmenu += "\t<li  class='link'><a href='javascript:void(0);' title='" + dtmenu.Rows[i]["title"] + "'><span>" + dtmenu.Rows[i]["title"] + "</span></a>\n\t\t<ul>\n" + getSubMenus(adminid, Convert.ToInt32(dtmenu.Rows[i]["idmenu"]), 0) + "\t\t</ul>\n\t</li>\n";
                }
                else
                {
                    strmenu += "\t<li><a href='" + dtmenu.Rows[i]["pageurl"] + "' title='" + dtmenu.Rows[i]["title"] + "'><span>" + dtmenu.Rows[i]["title"] + "</span></a></li>\n";
                }
            }
        }
        strmenu += "\t<li><a href='logout.ashx'><img style='vertical-align: middle;width:20px;float:left;margin-top:12px;' src='images/icon8.png' alt=''><span>Logout</span></a></li>";
        strmenu += "\n</ul>\n";
        return strmenu;
    }

    /// <summary>
    /// Bind The Top SubMenu For Admin
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //make Top Sub Menus
    public string getSubMenus(int idadmin, int parentid, int intlevel)
    {
        string strmenu = string.Empty;
        DataTable dtmenu = new DataTable();
        _adminid = idadmin;
        _parentid = parentid;
        dtmenu = selectAdminSubMenus();
        if (dtmenu.Rows.Count > 0)
        {
            intlevel += 1;
            string starttab = "\t\t";
            for (int j = 0; j < intlevel; j++) { starttab += "\t"; }

            for (int i = 0; i < dtmenu.Rows.Count; i++)
            {
                _parentid = Convert.ToInt32(dtmenu.Rows[i]["idmenu"]);
                strmenu += starttab + "<li><a href='" + dtmenu.Rows[i]["pageurl"] + "' title='" + dtmenu.Rows[i]["title"] + "'><span>" + dtmenu.Rows[i]["title"] + "</span></a></li>\n";
            }
        }
        return strmenu;
    }

    /// <summary>
    /// Bind The Menu On HomePage For Admin
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //home page menu for admin
    public DataTable getAdminHomePagemenu()
    {
        StrQuery = "select isnull(menu.[idmenu],0) as idmenu,isnull([title],'') as title,isnull(menu.[isactive],0) as isactive " +
                    " ,isnull([parentid],0) as parentid,isnull([pageurl],'') as pageurl,isnull(imagepath,'') as imagepath,isnull(menu.[sortorder],0) as sortorder " +
                    " from [menu] " +
                    " inner join adminRights on adminRights.idmenu = menu.idmenu " +
                    " inner join administrator on administrator.adminid = adminRights.adminid " +
                    " where administrator.adminid=@adminid and menu.isactive=1 and pageurl<>''";
        try
        {
            DataTable dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Get Menu Access Pages For Admin
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public DataTable getAdminAccesPages()
    {
        StrQuery = " select pageurl  from menu where (idmenu in (select idmenu from adminRights where adminid=@adminid )  or parentid in (select idmenu from adminRights where adminid=@adminid)) and parentid >0  ";

        try
        {
            DataTable dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@adminid", adminid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    public DataTable GetAllAdminType()
    {
        StrQuery = "select * from adminType where isactive=1";
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlDataAdapter sqlsda = new SqlDataAdapter(StrQuery, objcon);
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

    public string SearchKey { get; set; }
    public DataTable SearchKeywordManageUser()
    {
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = objcon;
            cmd.CommandText = "sp_SearchKeywordManageUser";
            cmd.Parameters.AddWithValue("@SearchKey", SearchKey);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

}
