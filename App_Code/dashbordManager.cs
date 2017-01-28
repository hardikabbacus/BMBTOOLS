using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
/// <summary>
/// Summary description for dashbordManager
/// </summary>
public class dashbordManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public dashbordManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Variable

    public int Years { get; set; }
    public int Months { get; set; }
    public decimal Sales { get; set; }

    #endregion

    public DataSet getDeshboardRecords()
    {
        DataSet ds = new DataSet();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_DashBordSearch]";
            sqlCmd.CommandType = CommandType.StoredProcedure;

            objcon.Open();
            sqlCmd.Connection = objcon;
            SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
            sqlAdp.Fill(ds);

            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    /// <summary>
    /// Get Every monthly record for CHART
    /// </summary>
    /// <returns></returns>
    public DataTable GetMounthlyRecord()
    {
        dt = new DataTable();
        StrQuery = "SELECT top 12 isnull(year(od.createddate),0) as Years, isnull(month(od.createddate),0) as Months, isnull(sum(od.netprice),0) as Sales ";
        StrQuery += " FROM tblOrderDetail as od inner join tblorder as o on o.orderid=od.orderid where o.orderstatus is not null ";
        StrQuery += " GROUP BY year(od.createddate), month(od.createddate)";

        try
        {
            objcon.Open();
            SqlDataAdapter sqlsda = new SqlDataAdapter(StrQuery, objcon);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objcon.Close(); }

    }

}