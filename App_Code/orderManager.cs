using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for orderManager
/// </summary>
public class orderManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public orderManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "----------------------------private variables------------------------"

    //order 
    private int _orderid;
    private int _customerid;
    private int _orderstatus;
    private decimal _totalammount;
    private string _createDate;
    private string _customerName;
    private int _payType;

    //order details
    private int _orderdetailid;
    private int _productid;
    private decimal _price;
    private int _qty;
    private decimal _netprice;
    private int _globleDiscountRate;
    private string _productName;
    private decimal _costPrice;
    private decimal _finalPrice;

    //other variable
    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    // order
    public int orderid { get { return _orderid; } set { _orderid = value; } }
    public int customerid { get { return _customerid; } set { _customerid = value; } }
    public int orderstatus { get { return _orderstatus; } set { _orderstatus = value; } }
    public decimal totalammount { get { return _totalammount; } set { _totalammount = value; } }
    public string createDate { get { return _createDate; } set { _createDate = value; } }
    public int payType { get { return _payType; } set { _payType = value; } }
    public string customerName { get { return _customerName; } set { _customerName = value; } }

    // order details
    public int orderdetailid { get { return _orderdetailid; } set { _orderdetailid = value; } }
    public int productid { get { return _productid; } set { _productid = value; } }
    public decimal price { get { return _price; } set { _price = value; } }
    public int qty { get { return _qty; } set { _qty = value; } }
    public decimal netprice { get { return _netprice; } set { _netprice = value; } }
    public int globleDiscountRate { get { return _globleDiscountRate; } set { _globleDiscountRate = value; } }
    public string productName { get { return _productName; } set { _productName = value; } }
    public decimal costPrice { get { return _costPrice; } set { _costPrice = value; } }
    public decimal finalPrice { get { return _finalPrice; } set { _finalPrice = value; } }


    //other
    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    public string contactName { get; set; }
    public int statusid { get; set; }
    public int firstyear { get; set; }
    public int lastyear { get; set; }
    public int startmonth { get; set; }
    public int endmonth { get; set; }

    #endregion

    #region

    //
    /// <summary>
    /// search order details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchOrder]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@orderid", orderid);
            sqlCmd.Parameters.AddWithValue("@contactName", contactName);
            sqlCmd.Parameters.AddWithValue("@statusid", statusid);
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

    //
    /// <summary>
    /// search order product details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchOrderetaild()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchOrderDetailsProduct]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@orderid", orderid);
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
    /// get all the order 
    /// </summary>
    /// <returns></returns>
    public DataTable GetAllStatus()
    {
        StrQuery = "select * from tblOrderStatus";
        dt = new DataTable();
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

    //
    /// <summary>
    /// insert order 
    /// </summary>
    /// <returns></returns>
    public int InsertOrder()
    {
        StrQuery = "insert into tblOrder (customerid,contactName,payType) values (@customerid,@contactName,@payType)";
        StrQuery += " SELECT SCOPE_IDENTITY()";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@customerid", customerid);
            sqlcmd.Parameters.AddWithValue("@contactName", contactName);
            sqlcmd.Parameters.AddWithValue("@payType", payType);
            sqlcmd.Parameters.AddWithValue("@orderstatus", orderstatus);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }


    // 
    /// <summary>
    /// update order 
    /// </summary>
    public void UpdateOrder()
    {
        StrQuery = "update tblOrder set totalammount=@totalammount,CreatedDate=getdate() where orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@totalammount", totalammount);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqlcmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update order status
    /// </summary>
    public void UpdateOrderStatusByOrderId()
    {
        StrQuery = "update tblOrder set orderstatus=@orderstatus,payType=@payType where orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderstatus", orderstatus);
            sqlcmd.Parameters.AddWithValue("@payType", payType);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
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
    /// insert Order details
    /// </summary>
    public void InsertOrderDetail()
    {
        StrQuery = "insert into tblOrderDetail (orderid,productid,price,qty,netprice,productName,globleDiscountRate,costPrice,finalPrice,CreatedDate) values (@orderid,@productid,@price,@qty,@netprice,@productName,@globleDiscountRate,@costPrice,@finalPrice,getdate())";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqlcmd.Parameters.AddWithValue("@productid", productid);
            sqlcmd.Parameters.AddWithValue("@price", price);
            sqlcmd.Parameters.AddWithValue("@qty", qty);
            sqlcmd.Parameters.AddWithValue("@netprice", netprice);
            sqlcmd.Parameters.AddWithValue("@productName", productName);
            sqlcmd.Parameters.AddWithValue("@globleDiscountRate", globleDiscountRate);
            sqlcmd.Parameters.AddWithValue("@costPrice", costPrice);
            sqlcmd.Parameters.AddWithValue("@finalPrice", finalPrice);
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
    /// update Order details
    /// </summary>
    public void UpdateOrderDetail()
    {
        StrQuery = "update tblOrderDetail set qty=@qty,netprice=@netprice,globleDiscountRate=@globleDiscountRate,productName=@productName,costPrice=@costPrice,finalPrice=@finalPrice,CreatedDate=getdate() where productid=@productid and orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqlcmd.Parameters.AddWithValue("@productid", productid);
            sqlcmd.Parameters.AddWithValue("@qty", qty);
            sqlcmd.Parameters.AddWithValue("@netprice", netprice);
            sqlcmd.Parameters.AddWithValue("@productName", productName);
            sqlcmd.Parameters.AddWithValue("@globleDiscountRate", globleDiscountRate);
            sqlcmd.Parameters.AddWithValue("@costPrice", costPrice);
            sqlcmd.Parameters.AddWithValue("@finalPrice", finalPrice);
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
    /// delete order details
    /// </summary>
    public void DeleteOrderDetails()
    {
        StrQuery = "delete from tblOrderDetail where orderdetailid=@orderdetailid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("orderdetailid", orderdetailid);
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
    /// get total ammount from order id
    /// </summary>
    /// <returns></returns>
    public decimal GetTotalAmmountFromOrderId()
    {
        StrQuery = "select isnull(totalammount,0) as totalammount from tblorder where orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            decimal Total = Convert.ToDecimal(sqlcmd.ExecuteScalar());
            return Total;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //  
    /// <summary>
    /// get netprice sum from tblorderdetails by orderid
    /// </summary>
    /// <returns></returns>
    public decimal GetSumOfItemPriceUsingOrderId()
    {
        StrQuery = "select isnull(sum(netprice),0) as netprice from tblorderdetail where orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            decimal Total = Convert.ToDecimal(sqlcmd.ExecuteScalar());
            return Total;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// get orderdetail from productid and orderid
    /// </summary>
    /// <returns></returns>
    public DataTable GetOrderDetailByproIdordId()
    {
        StrQuery = "select isnull(orderdetailid,0) as orderdetailid,isnull(orderid,0) as orderid ";
        StrQuery += " ,isnull(productid,0) as productid,isnull(price,0) as price,isnull(qty,0) as qty ";
        StrQuery += " ,isnull(netprice,0) as netprice from tblorderdetail where orderid=@orderid and productid=@productid ";
        dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqlcmd.Parameters.AddWithValue("@productid", productid);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;

        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); dt.Dispose(); }
    }

    /// <summary>
    /// get ordernumber for autocomplete 
    /// </summary>
    /// <returns></returns>
    public DataTable SearchOrderId()
    {
        //StrQuery = "select isnull(o.orderid,0) as orderid from tblorder as o where orderstatus is not null and o.orderid like ''+convert(varchar(max),@orderid)+'%'";

        StrQuery = "select isnull(o.orderid,0) as orderid from tblorder as o inner join customer as c on c.customerId = o.customerid ";
        StrQuery += " where orderstatus is not null and o.orderid like ''+convert(varchar(max),@orderid)+'%' ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// get the customer record from orderid
    /// </summary>
    /// <returns></returns>
    public DataTable FindCustomerFromOrderID()
    {
        StrQuery = "select isnull(o.orderid,0) as orderid,isnull(o.customerid,0) as customerid,ISNULL(c.contactname,'') as contactname,isnull(o.totalammount,0) as totalammount from tblOrder as o ";
        StrQuery += "inner join customer as c on c.customerId=o.customerid ";
        StrQuery += "where orderstatus is not null and o.orderid like ''+convert(varchar(max),@orderid)+'%'";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// get the contact name for autocomplete
    /// </summary>
    /// <returns></returns>
    public DataTable searchCustomerName()
    {
        StrQuery = "select distinct(c.contactname) from tblOrder as o ";
        StrQuery += "inner join customer as c on c.customerid=o.customerid ";
        StrQuery += "where c.contactname like ''+@contactName+'%'";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@contactName", contactName);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// get the order details from the contact person name
    /// </summary>
    /// <returns></returns>
    public DataTable FindOrderidFromCustomername()
    {
        StrQuery = "select isnull(o.orderid,0) as orderid,isnull(o.customerid,0) as customerid from tblOrder as o ";
        StrQuery += "inner join customer as c on c.customerid=o.customerid ";
        StrQuery += "where c.contactname like ''+@contactName+'%'";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@contactName", contactName);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get the order status
    /// </summary>
    /// <returns></returns>
    public int GetOrderStatus()
    {
        StrQuery = "select orderstatus from tblorder where orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            int orderstatus = 0;
            orderstatus = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return orderstatus;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// get the credit limits from the customer by orderid 
    /// </summary>
    /// <param name="OrderId"></param>
    /// <returns></returns>
    public decimal Getcreditlimitamountbycustomer(string OrderId)
    {
        StrQuery = "select isnull(c.creditLimit,0)as CreditLimit from tblorder as o inner join customer as c on o.customerid=c.customerId where o.orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", OrderId);
            decimal Total = Convert.ToDecimal(sqlcmd.ExecuteScalar());
            return Total;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }
    #endregion

    /// <summary>
    /// update the orderstatus by the orderid
    /// </summary>
    /// <param name="status"></param>
    public void UpdateStatus(int status)
    {
        StrQuery = "update tblorder set orderstatus=@orderstatus where orderid=@orderid ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderstatus", status);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqlcmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get the customer details by customerid
    /// </summary>
    /// <param name="CustomerId"></param>
    /// <returns></returns>
    public DataTable GetCustomerDetail(int CustomerId)
    {
        dt = new DataTable();
        StrQuery = "select isnull(c.Email,'')as Email,isnull(c.companyName,'')as Companyname,isnull(c.globleDiscountRate,'') as Discount from Customer as c where c.customerId=@customerId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@customerId", CustomerId);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// get the emailnotifaction details
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public DataTable GetSubject(int status)
    {
        dt = new DataTable();
        StrQuery = "select isnull(e.FromEmail,'')as FromEmail,isnull(e.EmailSubject,'')as EmailSubject,isnull(e.EmailBody,'')as EmailBody from EmailNotifications as e where e.EmailNotificationsId=@EmailNotificationId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@EmailNotificationId", status);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get order details by order id
    /// </summary>
    /// <param name="OrderId"></param>
    /// <returns></returns>
    public DataTable GetOrderDetail(int OrderId)
    {
        dt = new DataTable();
        //StrQuery = "select isnull(orderdetailid,0)as OrderDetailId, ISNULL(od.orderid,0)as OrderId,ISNULL(p.productName,'')as ProductName,isnull(od.price,0)as Price,ISNULL(od.qty,0)as Qty,isnull(od.netprice,0)as SubTotal,isnull(p.cost,0)as Cost,isnull((select SUM(netprice)from tblorderdetail where orderid=@OrderId),0) as TotalNetPrice from tblorderdetail as od inner join products as p on od.productid=p.productid  where od.orderid=@OrderId";

        StrQuery = " select isnull(orderdetailid,0)as OrderDetailId, ISNULL(od.orderid,0)as OrderId,CONVERT(VARCHAR(10), o.CreatedDate, 103) as OrderDate, ";
        StrQuery += " ISNULL(p.productName,'')as ProductName,isnull(od.price,0)as Price,ISNULL(od.qty,0)as Qty,isnull(od.netprice,0)as SubTotal,  ";
        StrQuery += " isnull(od.finalprice,0)as finalprice,isnull(od.globleDiscountRate,0) as globleDiscountRate,  ";
        StrQuery += " isnull(p.cost,0)as Cost,isnull((select SUM(netprice)from tblorderdetail where orderid=@OrderId),0) as TotalNetPrice   ";
        StrQuery += " from tblorder as o  ";
        StrQuery += " inner join tblorderdetail as od on o.orderid=od.orderid  ";
        StrQuery += " inner join products as p on od.productid=p.productid   ";
        StrQuery += " where od.orderid=@OrderId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@OrderId", OrderId);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// get order details by order id for pdf
    /// </summary>
    /// <param name="OrderId"></param>
    /// <returns></returns>
    public DataTable GetOrderDetailForPdf(int OrderId)
    {
        dt = new DataTable();
        //StrQuery = "select isnull(orderdetailid,0)as OrderDetailId, ISNULL(od.orderid,0)as OrderId,ISNULL(p.productName,'')as ProductName,isnull(od.price,0)as Price,ISNULL(od.qty,0)as Qty,isnull(od.netprice,0)as SubTotal,isnull(p.cost,0)as Cost,isnull((select SUM(netprice)from tblorderdetail where orderid=@OrderId),0) as TotalNetPrice from tblorderdetail as od inner join products as p on od.productid=p.productid  where od.orderid=@OrderId";

        StrQuery = " select isnull(o.orderid,0) as orderid,isnull(o.createddate,0) as orderdate,isnull(o.contactname,'') as contactname,isnull(o.payType,0) as payType, ";
        StrQuery += " ISNULL(c.mobile,0) as mobile,isnull(c.Email,'') as email,ISNULL(c.companyname,'') as companyname,ISNULL(c.streetAddress,'') as streetaddress, ";
        StrQuery += "isnull(c.city,'') as city,ISNULL(c.country,'') as country,isnull(od.productname,'') as productname,ISNULL(od.qty,0) as qty,isnull(od.price,0) as price ";
        StrQuery += ",isnull(od.globleDiscountRate,0) as discount,ISNULL(od.finalprice,0) as finalprice ";
        StrQuery += ",isnull(od.netprice,0) as netprice,isnull(p.sku,'') as sku,isnull(o.totalammount,0) as totalammount ";
        StrQuery += "from tblorder as o ";
        StrQuery += "inner join tblOrderDetail as od on od.orderid=o.orderid ";
        StrQuery += "inner join customer as c on c.customerId=o.customerId ";
        StrQuery += "inner join products as p on p.productid=od.productid ";
        StrQuery += " where od.orderid=@OrderId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@OrderId", OrderId);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// get the ammount from orderid
    /// </summary>
    /// <returns></returns>
    public int getTotalAmountFromOrderId()
    {
        StrQuery = "select isnull(totalammount,0) as totalammount from tblorder where orderid=@orderid";
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            int Amount = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return Amount;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public void updatePayType()
    {
        StrQuery = "update tblorder set payType=@payType where orderid=@orderid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            sqlcmd.Parameters.AddWithValue("@payType", payType);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public DataTable SearchKeywordOrderid()
    {
        dt = new DataTable();
        StrQuery = "select isnull(orderid,0) as orderid from tblorder where orderstatus <> 0 and orderstatus is not null and orderid like '%'+convert(varchar(max),@orderid)+'%'";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@orderid", orderid);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); dt.Dispose(); }
    }


    /// <summary>
    /// Get inventory from the product by productid
    /// </summary>
    /// <returns></returns>
    public int GetInventoryByProductid()
    {
        StrQuery = "select isnull(inventory,0) as inventory from products where productid=@productid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productid);
            int pInvt = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return pInvt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }
}