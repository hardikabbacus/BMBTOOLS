using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for categoryManager
/// </summary>
public class categoryManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    public categoryManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #region "----------------------------private variables------------------------"

    private int _idcategory;
    private string _categoryName;
    private byte _isactive;
    private int _parentid;
    private string _pageurl;
    private int _sortorder;
    private string _imagepath;
    private byte _ismenu;
    private byte _isseparate;
    private string _catedesc;

    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion


    #region "----------------------------public properties------------------------"

    public int idcategory { get { return _idcategory; } set { _idcategory = value; } }
    public string categoryName { get { return _categoryName; } set { _categoryName = value; } }
    public byte isactive { get { return _isactive; } set { _isactive = value; } }
    public int parentid { get { return _parentid; } set { _parentid = value; } }
    public string pageurl { get { return _pageurl; } set { _pageurl = value; } }
    public int sortorder { get { return _sortorder; } set { _sortorder = value; } }
    public string imagepath { get { return _imagepath; } set { _imagepath = value; } }
    // public byte categoryName { get { return _categoryName; } set { _categoryName = value; } }
    public byte isseparate { get { return _isseparate; } set { _isseparate = value; } }
    public string catedesc { get { return _catedesc; } set { _catedesc = value; } }

    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    #endregion

    #region "----------------------------public methods-------------------------"

    //
    /// <summary>
    /// select single Category details by category Id
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItemByCategoryId()
    {
        StrQuery = " select isnull([categoryName],'') as categoryName,isnull([categoryDescription],'') as categoryDescription,isnull([isactive],0) as isactive," +
                   " isnull([parentid],0) as parentid , isnull([sortorder],0) as sortorder," +
                   " isnull([imgName],'') as imgName " +
                   " from [category] where categoryId=@categoryId ";

        StrQuery += " order by categoryName ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = idcategory;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// get the detail for single category name and description  by language id
    /// </summary>
    /// <returns></returns>
    public DataTable SelectSingleItemByCategoryLanguagesId()
    {
        StrQuery = " select isnull([categoryName],'') as categoryName,isnull([categoryDescription],'') as categoryDescription,isnull([categoryLanguage],0) as categoryLanguage " +
                   " from [categoryLanguage] where languageId=2 and categoryId=@categoryId ";

        StrQuery += " order by categoryName ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = idcategory;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    //
    /// <summary>
    /// delete Category details by CategoryId
    /// </summary>
    public void DeleteCategory()
    {
        StrQuery = "delete from [category] where categoryId=@categoryId";


        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = idcategory;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    //
    /// <summary>
    /// delete Category details by CategoryId
    /// </summary>
    public void DeleteCategorylanguage()
    {

        StrQuery = "delete from [categoryLanguage] where categoryId=@categoryId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = idcategory;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    //
    /// <summary>
    /// insert Category details into category table
    /// </summary>
    /// <returns></returns>
    public int InsertItem()
    {
        StrQuery += " declare @count int  set @count=0  select @count  =count(*) from [category] where sortorder = @sortorder ";
        StrQuery += " if(@count=1)  update [category] set sortorder = sortorder+1 where sortorder>=@sortorder ";
        StrQuery += " insert into [category]([parentId],[categoryName],[categoryDescription],[imgName],[sortorder],[isActive]) values(@parentId,@categoryName,@categoryDescription,@imgName,@sortorder,@isActive)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@parentId", SqlDbType.Int)).Value = parentid;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.VarChar, 50)).Value = categoryName;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryDescription", SqlDbType.VarChar, 50)).Value = catedesc;
            sqlcmd.Parameters.Add(new SqlParameter("@imgName", SqlDbType.VarChar, 200)).Value = imagepath;
            sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update Category details by CategoryId
    /// </summary>
    /// <param name="prevSort"></param>
    /// <param name="changeSort"></param>
    public void UpdateItem(int prevSort, int changeSort)
    {
        StrQuery = " declare @count int set @count =0 select @count = count(*) from category where sortorder = @sortorder " +
                    " if(@count>=1)  begin  if(@prevSort > @changeSort) begin " +
                    " update category set sortorder= sortorder+ 1 where sortorder < @prevSort and sortorder >=@changeSort end " +
                    " if(@prevSort < @changeSort) begin update category set sortorder = sortorder - 1 where sortorder <= @changeSort and sortorder > @prevSort end end " +
                    " update [category] set [categoryName]=@categoryName,[categoryDescription]=@categoryDescription,[imgName]=@imgName,[isactive]=@isactive,[parentid]=@parentid, " +
                    " [sortorder]=@sortorder " +
                    " where categoryId=@categoryId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.VarChar, 50)).Value = categoryName;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryDescription", SqlDbType.VarChar, 250)).Value = catedesc;
            sqlcmd.Parameters.Add(new SqlParameter("@imgName", SqlDbType.VarChar, 50)).Value = imagepath;
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@parentid", SqlDbType.Int)).Value = parentid;
            sqlcmd.Parameters.Add(new SqlParameter("@sortorder", SqlDbType.Int)).Value = sortorder;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = idcategory;
            sqlcmd.Parameters.Add(new SqlParameter("@prevSort", SqlDbType.Int)).Value = prevSort;
            sqlcmd.Parameters.Add(new SqlParameter("@changeSort", SqlDbType.Int)).Value = changeSort;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// get parent Categoryname from category
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public DataTable GetParentCategory(bool flag)
    {
        try
        {
            StrQuery = "select categoryId,categoryName from [category] where 1=1 ";
            if (flag == true)
            {
                StrQuery += " and parentid=0 ";
            }
            StrQuery += " and isactive = 1 order by categoryName ";
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
    /// get Categoryname,categoryid from category
    /// </summary>
    /// <returns></returns>
    public DataTable GetAllCategory()
    {
        try
        {
            StrQuery = "select categoryId,categoryName from [category] where 1=1 ";

            StrQuery += " and isactive = 1 order by categoryName ";
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
    /// get all parent Categoryname from category
    /// </summary>
    /// <returns></returns>
    public DataTable GetParentCategory()
    {
        try
        {


            StrQuery = "select * from category where parentId=0";


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

    //
    /// <summary>
    /// get category from categoryid
    /// </summary>
    /// <param name="catid"></param>
    /// <returns></returns>
    public DataTable GetCategory(int catid)
    {
        try
        {
            StrQuery = "select * from category where parentId=" + catid;

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
    /// get the all the subcategory categoryname and subcategoryid
    /// </summary>
    /// <returns></returns>
    public DataTable GetSubCategory()
    {
        StrQuery = "select categoryId,categoryName from [category] where 1=1 ";
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
    ///  select category parent category for delete
    /// </summary>
    /// <returns></returns>
    public int SelectSubselectCount()
    {
        StrQuery = "select count(categoryid) from category where parentid=@parentid";
        try
        {
            int count = 0;
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@parentid", parentid);
            count = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return count;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get all the category form drop down
    /// </summary>
    /// <returns></returns>
    public DataTable GetAllCategorySearchDDL()
    {
        StrQuery = "select categoryId,categoryname from [category] where isactive=1";
        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); dt.Dispose(); }
    }

    //
    /// <summary>
    /// Check Category is already exist or not
    /// </summary>
    /// <returns></returns>
    public bool TitleExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(categoryId) from [category] where categoryName = @categoryName";
            if (idcategory != 0)
            {
                StrQuery += "  and categoryId <> @categoryId";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.VarChar, 50)).Value = categoryName;
            if (idcategory != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = idcategory;
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
    /// update Category status
    /// </summary>
    public void UpdateStatus()
    {
        StrQuery = " update [category] set [isactive]=@isactive where categoryId=@categoryId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = idcategory;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// updat category image from categoryid
    /// </summary>
    public void UpdateImage()
    {
        StrQuery = " update [category] set [imgName]=@imgName " +
                    " where [categoryId]=@categoryId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@imgName", imagepath);
            sqlcmd.Parameters.AddWithValue("@categoryId", idcategory);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get the maximum id from category
    /// </summary>
    /// <returns></returns>
    public int getmaxid()
    {
        StrQuery = "Select Max(categoryId) from category";
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

    //get parent  Category
    /// <summary>
    /// get all the category details
    /// </summary>
    /// <returns></returns>
    public DataTable GetParentCategoryProductPage()
    {
        try
        {
            StrQuery = "select * from category where isactive=0";

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

    private int _categoryLanguage;
    private int _categoryId;
    private int _languageId;

    private string _categoryDescription;
    private string _createDate;
    private string _updatedate;

    #endregion

    #region --- public property for category language ---

    public int categoryLanguage { get { return _categoryLanguage; } set { _categoryLanguage = value; } }
    public int categoryId { get { return _categoryId; } set { _categoryId = value; } }
    public int languageId { get { return _languageId; } set { _languageId = value; } }

    public string categoryDescription { get { return _categoryDescription; } set { _categoryDescription = value; } }
    public string createDate { get { return _createDate; } set { _createDate = value; } }
    public string updatedate { get { return _updatedate; } set { _updatedate = value; } }

    #endregion

    #region --- public method for category language ---

    /// <summary>
    /// insert categorylanguage  categoryname and description
    /// </summary>
    /// <returns></returns>
    public int InsertCategoryLanguage()
    {
        StrQuery = " insert into [categoryLanguage]([categoryId],[languageId],[categoryName],[categoryDescription]) values(@categoryId,@languageId,@categoryName,@categoryDescription)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = categoryId;
            sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.NVarChar, 100)).Value = categoryName;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryDescription", SqlDbType.NVarChar)).Value = categoryDescription;

            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// update categorylanguage  categoryname and description from categorylanguage id
    /// </summary>
    public void UpdateCategoryLnaguage()
    {
        StrQuery = " update categoryLanguage set categoryName=@categoryName,categoryDescription=@categoryDescription where languageId=@languageId and categoryId=@categoryId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@languageId", SqlDbType.Int)).Value = languageId;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int)).Value = categoryId;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.NVarChar, 100)).Value = categoryName;
            sqlcmd.Parameters.Add(new SqlParameter("@categoryDescription", SqlDbType.NVarChar)).Value = categoryDescription;

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    ///  get all the category detail language wise
    /// </summary>
    /// <returns></returns>
    public DataTable SelectCategoryLanguagebyID()
    {
        try
        {
            StrQuery = " select isnull(languagename,'') as languagename ,isnull(categoryLanguage,0) as categoryLanguage,isnull(textAlign,'false') as textAlign,isnull(categoryId,0) as categoryId,isnull(L.LanguageId,0) as LanguageId,isnull(categoryName,'') as categoryName,isnull(categoryDescription,'') as categoryDescription from categoryLanguage cl  inner join language l on cl.LanguageId = l.languageid where categoryId=@categoryId";

            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@categoryId", categoryId);
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