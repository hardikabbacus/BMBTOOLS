using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for paymentManager
/// </summary>
public class paymentManager
{

    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public paymentManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "----------------------------private variables------------------------"

    //Payment

    private int _paymentid;
    private int _orderid;
    private int _customerid;
    private int _paystatus;
    private decimal _payammount;
    public string _createDate;
    public string _paynotes;

    //other variable
    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    // order
    public int paymentid { get { return _paymentid; } set { _paymentid = value; } }
    public int orderid { get { return _orderid; } set { _orderid = value; } }
    public int paystatus { get { return _paystatus; } set { _paystatus = value; } }
    public int customerid { get { return _customerid; } set { _customerid = value; } }
    public decimal payammount { get { return _payammount; } set { _payammount = value; } }
    public string createDate { get { return _createDate; } set { _createDate = value; } }
    public string paynotes { get { return _paynotes; } set { _paynotes = value; } }

    //other
    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    public string contactName { get; set; }
    public string companyName { get; set; }
    public int firstyear { get; set; }
    public int lastyear { get; set; }
    public int startmonth { get; set; }
    public int endmonth { get; set; }
    public string commonsearch { get; set; }

    #endregion

    #region

    //
    /// <summary>
    /// search payment details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchPayment]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@paymentid", paymentid);
            sqlCmd.Parameters.AddWithValue("@orderid", orderid);
            sqlCmd.Parameters.AddWithValue("@companyName", companyName);
            sqlCmd.Parameters.AddWithValue("@commonsearch", commonsearch);
            sqlCmd.Parameters.AddWithValue("@firstyear", firstyear);
            sqlCmd.Parameters.AddWithValue("@lastyear", lastyear);
            sqlCmd.Parameters.AddWithValue("@startmonth", startmonth);
            sqlCmd.Parameters.AddWithValue("@endmonth", endmonth);
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
    /// insert payment details
    /// </summary>
    public void InsertPayment()
    {
        StrQuery = "insert into tblPayment (customerid,orderid,payammount,paynotes,paystatus,CreatedDate) values (@customerid,@orderid,@payammount,@paynotes,@paystatus,getdate())";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@customerid", customerid);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqlcmd.Parameters.AddWithValue("@payammount", payammount);
            sqlcmd.Parameters.AddWithValue("@paynotes", paynotes);
            sqlcmd.Parameters.AddWithValue("@paystatus", paystatus);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// update payment details
    /// </summary>
    public void UpdatePayment()
    {
        StrQuery = "update tblpayment set customerid=@customerid,orderid=@orderid,paynotes=@paynotes,paystatus=@paystatus,payammount=@payammount where paymentid=@paymentid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@paymentid", paymentid);
            sqlcmd.Parameters.AddWithValue("@customerid", customerid);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqlcmd.Parameters.AddWithValue("@payammount", payammount);
            sqlcmd.Parameters.AddWithValue("@paynotes", paynotes);
            sqlcmd.Parameters.AddWithValue("@paystatus", paystatus);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// get customerid by customer name
    /// </summary>
    /// <returns></returns>
    public int GetCustomerIdByCustomerName()
    {
        StrQuery = "select isnull(customerid,0) as customerid from customer where contactName=@contactName";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@contactName", contactName);
            int cust_id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return cust_id;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// delete payment record by payment id
    /// </summary>
    public void DeletePaymentRecord()
    {
        StrQuery = "delete from tblpayment where paymentid=@paymentid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@paymentid", paymentid);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // 
    /// <summary>
    /// get payment detail single record for edit 
    /// </summary>
    /// <returns></returns>
    public DataTable GetSinglePaymentRecord()
    {
        StrQuery = " select ISNULL(p.paymentid,0) as paymentid,ISNULL(p.orderid,0) as orderid,ISNULL(p.payammount,0) as payammount,isnull(paynotes,'') as paynotes ";
        StrQuery += " ,ISNULL(contactname,'') as contactname from tblpayment as p ";
        StrQuery += " inner join customer as c on c.customerid=p.customerid ";
        StrQuery += " where paymentid=@paymentid ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@paymentid", paymentid);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            dt = new DataTable();
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// get the payment status by the order id
    /// </summary>
    /// <returns></returns>
    public int GetPaymentAmountStatusFromOrderid()
    {
        StrQuery = "select paystatus from tblpayment where orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            int ordstatus = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return ordstatus;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public string SearchKey { get; set; }
    public DataTable SearchKeywordPayment()
    {
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = objcon;
            cmd.CommandText = "sp_SearchKeywordPayment";
            cmd.Parameters.AddWithValue("@SearchKey", SearchKey);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    #endregion

}