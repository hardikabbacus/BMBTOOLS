using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommanlanguagesManager
/// </summary>
public class CommanlanguagesManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
	public CommanlanguagesManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region "----------------------------public properties------------------------"

    public int languageid { get; set; }
    public string languagename { get; set; }
    public string languagecode { get; set; }
    public byte isactive { get; set; }
    public byte textdirection { get; set; }

    #endregion


    #region ------------------------------Public Methods--------------


    /// <summary>
    /// select all the languages
    /// </summary>
    /// <returns></returns>
    public DataTable SelectLanguageForField()
    {
        StrQuery = "select isnull(languageid,0) as languageid,isnull(languagename,'') as languagename,";
        StrQuery += "isnull(textAlign,'false') as textAlign,";
        StrQuery += "isnull(isactive,'true') as isactive from language where isactive=1";

        try
        {
            dt = new DataTable();
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