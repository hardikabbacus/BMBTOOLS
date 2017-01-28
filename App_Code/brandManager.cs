using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for brandManager
/// </summary>
public class brandManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
	
	public brandManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region "----------------------------private variables------------------------"

    private int _idbrand;
    private string _brandName;
    private string _branddescription;
    private string _imagepath;
    private int _sortorder;
    private byte _isactive;
    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    public int idbrand { get { return _idbrand; } set { _idbrand = value; } }
    public string brandName { get { return _brandName; } set { _brandName = value; } }
    public string branddescription { get { return _branddescription; } set { _branddescription = value; } }
    public string imagepath { get { return _imagepath; } set { _imagepath = value; } }
    public int sortorder { get { return _sortorder; } set { _sortorder = value; } }
    public byte isactive { get { return _isactive; } set { _isactive = value; } } 
    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    #endregion

    #region "----------------------------public methods-------------------------"

    
    /// <summary>
    /// select single Category details by Brand Id
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItemByBrandId()
    {
        StrQuery = " select isnull([brandName],'') as brandName,isnull([brandDescription],'') as brandDescription,isnull([isactive],0) as isactive," +
                   "isnull([sortorder],0) as sortorder," +
                   " isnull([imgName],'') as imgName " +
                   " from [brand] where brandId=@brandId ";

        StrQuery += " order by brandName ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@brandId", SqlDbType.Int)).Value = idbrand;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    
    /// <summary>
    /// delete Category details by CategoryId
    /// </summary>
    public void DeleteBrand()
    {
        StrQuery = "delete from [brand] where brandId=@brandId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@brandId", SqlDbType.Int)).Value = idbrand;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    /// <summary>
    /// delete brand langeage using brand id
    /// </summary>
    public void DeleteBrandlanguage()
    {
        StrQuery = "delete from [brandLanguage] where brandId=@brandId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@brandId", SqlDbType.Int)).Value = idbrand;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    /// <summary>
    /// insert brand details
    /// </summary>
    /// <returns></returns>
    public int InsertItem()
    {
        StrQuery += " declare @count int  set @count=0  select @count  =count(*) from [brand] where sortorder = @sortorder ";
        StrQuery += " if(@count=1)  update [brand] set sortorder = sortorder+1 where sortorder>=@sortorder ";
        StrQuery += " insert into [brand]([brandName],[brandDescription],[imgName],[sortorder],[isActive]) values(@brandName,@brandDescription,@imgName,@sortorder,@isActive)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);

            sqlcmd.Parameters.Add(new SqlParameter("@brandName", SqlDbType.VarChar, 100)).Value = brandName;
            sqlcmd.Parameters.Add(new SqlParameter("@brandDescription", SqlDbType.Text)).Value = branddescription;
            sqlcmd.Parameters.Add(new SqlParameter("@imgName", SqlDbType.VarChar, 200)).Value = imagepath;
            sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    
    /// <summary>
    /// update brand details by brandid
    /// </summary>
    /// <param name="prevSort"></param>
    /// <param name="changeSort"></param>
    public void UpdateItem(int prevSort, int changeSort)
    {
        StrQuery = " declare @count int set @count =0 select @count = count(*) from brand where sortorder = @sortorder " +
                    " if(@count>=1)  begin  if(@prevSort > @changeSort) begin " +
                    " update brand set sortorder= sortorder+ 1 where sortorder < @prevSort and sortorder >=@changeSort end " +
                    " if(@prevSort < @changeSort) begin update brand set sortorder = sortorder - 1 where sortorder <= @changeSort and sortorder > @prevSort end end " +
                    " update [brand] set [brandName]=@brandName,[brandDescription]=@brandDescription,[imgName]=@imgName,[isactive]=@isactive, " +
                    " [sortorder]=@sortorder " +
                    " where brandId=@brandId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@brandName", SqlDbType.VarChar, 100)).Value = brandName;
            sqlcmd.Parameters.Add(new SqlParameter("@brandDescription", SqlDbType.Text)).Value = branddescription;
            sqlcmd.Parameters.Add(new SqlParameter("@imgName", SqlDbType.VarChar, 50)).Value = imagepath;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;
            sqlcmd.Parameters.Add(new SqlParameter("@brandId", SqlDbType.Int)).Value = idbrand;
            sqlcmd.Parameters.Add(new SqlParameter("@prevSort", SqlDbType.Int)).Value = prevSort;
            sqlcmd.Parameters.Add(new SqlParameter("@changeSort", SqlDbType.Int)).Value = changeSort;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    
    /// <summary>
    /// function for check brand name is already exist or not
    /// </summary>
    /// <returns></returns>
    public bool TitleExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(brandId) from [brand] where brandName = @brandName";
            if (idbrand != 0)
            {
                StrQuery += "  and brandId <> @brandId";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@brandName", SqlDbType.VarChar, 50)).Value = brandName;
            if (idbrand != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@brandId", SqlDbType.Int)).Value = idbrand;
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

    
    /// <summary>
    /// update brand status by brand id
    /// </summary>
    public void UpdateStatus()
    {
        StrQuery = " update [brand] set [isactive]=@isactive where brandId=@brandId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@brandId", SqlDbType.Int)).Value = idbrand;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    
    /// <summary>
    /// update brand image by brandid
    /// </summary>
    public void UpdateImage()
    {
        StrQuery = " update [brand] set [imgName]=@imgName " +
                    " where [brandId]=@brandId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@imgName", imagepath);
            sqlcmd.Parameters.AddWithValue("@brandId", idbrand);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    
    /// <summary>
    /// get the maximum brand id
    /// </summary>
    /// <returns></returns>
    public int getmaxid()
    {
        StrQuery = "Select Max(brandId) from brand";
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
    
    /// <summary>
    /// get parent  brand
    /// </summary>
    /// <returns></returns>
    public DataTable GetBrandItem()
    {
        try
        {
            StrQuery = "select * from brand";
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
    #endregion


    #region --- private variable for category language ---

    private int _brandLanguage;
    private int _languageId;
    private string _createDate;
    private string _updatedate;

    #endregion

    #region --- public property for category language ---

    public int brandLanguage { get { return _brandLanguage; } set { _brandLanguage = value; } }
    public int languageId { get { return _languageId; } set { _languageId = value; } }
    public string createDate { get { return _createDate; } set { _createDate = value; } }
    public string updatedate { get { return _updatedate; } set { _updatedate = value; } }

    #endregion
    #region --- public method for category language ---

    /// <summary>
    /// insert brand language name and description
    /// </summary>
    /// <returns></returns>
    public int InsertBrandLanguage()
    {
        StrQuery = " insert into [brandLanguage]([brandId],[languageId],[brandName],[brandDescription]) values(@brandId,@languageId,@brandName,@brandDescription)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@brandId", SqlDbType.Int)).Value = idbrand;
            sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;
            sqlcmd.Parameters.Add(new SqlParameter("@brandName", SqlDbType.NVarChar, 100)).Value = brandName;
            sqlcmd.Parameters.Add(new SqlParameter("@brandDescription", SqlDbType.NVarChar)).Value = branddescription;

            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// update brand language name and description by languageid
    /// </summary>
    public void UpdateBrandLnaguage()
    {
        StrQuery = "update brandLanguage set brandName=@brandName,brandDescription=@brandDescription where brandId=@brandId And languageId=@languageId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@brandId", SqlDbType.Int)).Value = idbrand;
            sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;

            sqlcmd.Parameters.Add(new SqlParameter("@brandName", SqlDbType.NVarChar, 100)).Value = brandName;
            sqlcmd.Parameters.Add(new SqlParameter("@brandDescription", SqlDbType.NVarChar)).Value = branddescription;

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get the language description by language id
    /// </summary>
    /// <returns></returns>
    public DataTable SelectBrandLanguagebyID()
    {
        try
        {
            StrQuery = "select isnull(languagename,'') as languagename ,isnull(brandLanguage,0) as brandLanguage,isnull(textAlign,'false') as textAlign,isnull(brandId,0) as brandId,isnull(L.LanguageId,0) as LanguageId,isnull(brandName,'') as brandName,isnull(brandDescription,'') as brandDescription from brandLanguage cl  inner join language l on cl.LanguageId = l.languageid where brandId=@brandId";

            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@brandId", idbrand);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion
}