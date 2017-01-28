using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Xml;

/// <summary>
/// Summary description for customerManager
/// </summary>
public class customerManager
{
    public customerManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    String StrQuery;
    DataSet ds = new DataSet();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region "----------------------------private variables------------------------"
    private int _customerId;
    private string _contactName;
    private string _companyName;
    private string _streetAddress;
    private string _city;
    private string _country;
    private string _storePhoneNumber;
    private string _gpsLocation;
    private string _mobile;
    private string _email;
    public string _newPassword;
    private int _languagePreferance;
    private string _globleDiscountRate;
    private bool _caseOnDelivery;
    private bool _allowCreditcard;
    private string _xdays;
    private string _reducePercent;
    private string _creditLimit;
    private bool _isActive;
    private string _createDate;
    private string _updateDate;
    private int _customerType;


    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    public int customerId { get { return _customerId; } set { _customerId = value; } }
    public string contactName { get { return _contactName; } set { _contactName = value; } }
    public string companyName { get { return _companyName; } set { _companyName = value; } }
    public string streetAddress { get { return _streetAddress; } set { _streetAddress = value; } }
    public string city { get { return _city; } set { _city = value; } }
    public string country { get { return _country; } set { _country = value; } }
    public string storePhoneNumber { get { return _storePhoneNumber; } set { _storePhoneNumber = value; } }
    public string gpsLocation { get { return _gpsLocation; } set { _gpsLocation = value; } }
    public string mobile { get { return _mobile; } set { _mobile = value; } }
    public string email { get { return _email; } set { _email = value; } }
    public string newPassword { get { return _newPassword; } set { _newPassword = value; } }
    public int languagePreferance { get { return _languagePreferance; } set { _languagePreferance = value; } }
    public string globleDiscountRate { get { return _globleDiscountRate; } set { _globleDiscountRate = value; } }
    public bool caseOnDelivery { get { return _caseOnDelivery; } set { _caseOnDelivery = value; } }
    public bool allowCreditcard { get { return _allowCreditcard; } set { _allowCreditcard = value; } }
    public string xdays { get { return _xdays; } set { _xdays = value; } }
    public string reducePercent { get { return _reducePercent; } set { _reducePercent = value; } }
    public string creditLimit { get { return _creditLimit; } set { _creditLimit = value; } }
    public bool isActive { get { return _isActive; } set { _isActive = value; } }
    public string createDate { get { return _createDate; } set { _createDate = value; } }
    public string updateDate { get { return _updateDate; } set { _updateDate = value; } }
    public int customerType { get { return _customerType; } set { _customerType = value; } }


    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }


    #endregion

    #region

    //
    /// <summary>
    /// search customer details
    /// </summary>
    /// <returns></returns>
    public DataTable SearchItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchCustomer]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@contactName", contactName);
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
    /// insert Customer details
    /// </summary>
    public void InsertItem()
    {
        StrQuery = " insert into customer (contactName,companyName,streetAddress,city,country,storePhoneNumber,gpsLocation,mobile ";
        StrQuery += " ,email,newPassword,languagePreferance,globleDiscountRate,caseOnDelivery,allowCreditcard,xdays,reducePercent ";
        StrQuery += " ,creditLimit,isActive,createDate,customerType)  ";
        StrQuery += " values (@contactName,@companyName,@streetAddress,@city,@country,@storePhoneNumber,@gpsLocation,@mobile ";
        StrQuery += " ,@email,@newPassword,@languagePreferance,@globleDiscountRate,@caseOnDelivery,@allowCreditcard,@xdays,@reducePercent ";
        StrQuery += " ,@creditLimit,@isActive,getdate(),@customerType)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@contactName", SqlDbType.VarChar, 250)).Value = contactName;
            sqlcmd.Parameters.Add(new SqlParameter("@companyName", SqlDbType.VarChar, 250)).Value = companyName;
            sqlcmd.Parameters.Add(new SqlParameter("@streetAddress", SqlDbType.NVarChar)).Value = streetAddress;
            sqlcmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar, 100)).Value = city;
            sqlcmd.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar, 100)).Value = country;
            sqlcmd.Parameters.Add(new SqlParameter("@storePhoneNumber", SqlDbType.VarChar, 40)).Value = storePhoneNumber;
            sqlcmd.Parameters.Add(new SqlParameter("@gpsLocation", SqlDbType.VarChar, 100)).Value = gpsLocation;
            sqlcmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar, 40)).Value = mobile;
            sqlcmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 140)).Value = email;
            sqlcmd.Parameters.Add(new SqlParameter("@newPassword", SqlDbType.VarChar, 20)).Value = newPassword;
            sqlcmd.Parameters.Add(new SqlParameter("@languagePreferance", SqlDbType.Int)).Value = languagePreferance;
            sqlcmd.Parameters.Add(new SqlParameter("@globleDiscountRate", SqlDbType.VarChar, 10)).Value = globleDiscountRate;
            sqlcmd.Parameters.Add(new SqlParameter("@caseOnDelivery", SqlDbType.Bit)).Value = caseOnDelivery;
            sqlcmd.Parameters.Add(new SqlParameter("@allowCreditcard", SqlDbType.Bit)).Value = allowCreditcard;
            sqlcmd.Parameters.Add(new SqlParameter("@xdays", SqlDbType.VarChar, 15)).Value = xdays;
            sqlcmd.Parameters.Add(new SqlParameter("@reducePercent", SqlDbType.VarChar, 2)).Value = reducePercent;
            sqlcmd.Parameters.Add(new SqlParameter("@creditLimit", SqlDbType.VarChar, 15)).Value = creditLimit;
            sqlcmd.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit)).Value = isActive;
            sqlcmd.Parameters.Add(new SqlParameter("@customerType", SqlDbType.Int)).Value = customerType;
            sqlcmd.ExecuteScalar();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update customer details by customerid
    /// </summary>
    public void UpdateItem()
    {
        StrQuery = " update customer set contactName=@contactName,companyName=@companyName,streetAddress=@streetAddress,city=@city, ";
        StrQuery += " country=@country,storePhoneNumber=@storePhoneNumber,gpsLocation=@gpsLocation,mobile=@mobile ";
        StrQuery += " ,email=@email,newPassword=@newPassword,languagePreferance=@languagePreferance,globleDiscountRate=@globleDiscountRate, ";
        StrQuery += " caseOnDelivery=@caseOnDelivery,allowCreditcard=@allowCreditcard,xdays=@xdays,reducePercent=@reducePercent ";
        StrQuery += " ,creditLimit=@creditLimit,isActive=@isActive,updateDate=getdate(),customerType=@customerType where customerId=@customerId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = customerId;
            sqlcmd.Parameters.Add(new SqlParameter("@contactName", SqlDbType.VarChar, 250)).Value = contactName;
            sqlcmd.Parameters.Add(new SqlParameter("@companyName", SqlDbType.VarChar, 250)).Value = companyName;
            sqlcmd.Parameters.Add(new SqlParameter("@streetAddress", SqlDbType.NVarChar)).Value = streetAddress;
            sqlcmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar, 100)).Value = city;
            sqlcmd.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar, 100)).Value = country;
            sqlcmd.Parameters.Add(new SqlParameter("@storePhoneNumber", SqlDbType.VarChar, 40)).Value = storePhoneNumber;
            sqlcmd.Parameters.Add(new SqlParameter("@gpsLocation", SqlDbType.VarChar, 100)).Value = gpsLocation;
            sqlcmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar, 40)).Value = mobile;
            sqlcmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 140)).Value = email;
            sqlcmd.Parameters.Add(new SqlParameter("@newPassword", SqlDbType.VarChar, 20)).Value = newPassword;
            sqlcmd.Parameters.Add(new SqlParameter("@languagePreferance", SqlDbType.Int)).Value = languagePreferance;
            sqlcmd.Parameters.Add(new SqlParameter("@globleDiscountRate", SqlDbType.VarChar, 10)).Value = globleDiscountRate;
            sqlcmd.Parameters.Add(new SqlParameter("@caseOnDelivery", SqlDbType.Bit)).Value = caseOnDelivery;
            sqlcmd.Parameters.Add(new SqlParameter("@allowCreditcard", SqlDbType.Bit)).Value = allowCreditcard;
            sqlcmd.Parameters.Add(new SqlParameter("@xdays", SqlDbType.VarChar, 15)).Value = xdays;
            sqlcmd.Parameters.Add(new SqlParameter("@reducePercent", SqlDbType.VarChar, 2)).Value = reducePercent;
            sqlcmd.Parameters.Add(new SqlParameter("@creditLimit", SqlDbType.VarChar, 15)).Value = creditLimit;
            sqlcmd.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit)).Value = isActive;
            sqlcmd.Parameters.Add(new SqlParameter("@customerType", SqlDbType.Int)).Value = customerType;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// update customer details status
    /// </summary>
    public void UpdateStatus()
    {
        StrQuery = " update [customer] set [isActive]=@isActive where customerId=@customerId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit)).Value = isActive;
            sqlcmd.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = customerId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// delete customer details
    /// </summary>
    public void DeleteItem()
    {
        StrQuery = " delete from  [customer] where customerId=@customerId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = customerId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// get single customer details
    /// </summary>
    /// <returns></returns>
    public DataTable GetSingleCustomer()
    {
        StrQuery = " select isnull(contactName,'') as contactName,isnull(companyName,'') as companyName,isnull(streetAddress,'') as streetAddress, ";
        StrQuery += " isnull(city,'') as city,isnull(country,'') as country,isnull(storePhoneNumber,'') as storePhoneNumber,isnull(gpsLocation,'') as gpsLocation, ";
        StrQuery += " isnull(mobile,'') as mobile,isnull(email,'') as email,isnull(newPassword,'') as newPassword,isnull(languagePreferance,0) as languagePreferance, ";
        StrQuery += " isnull(globleDiscountRate,'') as globleDiscountRate,isnull(caseOnDelivery,0) as caseOnDelivery,isnull(allowCreditcard,0) as allowCreditcard, ";
        StrQuery += " isnull(xdays,'') as xdays,isnull(reducePercent,'') as reducePercent ";
        StrQuery += " ,isnull(creditLimit,'') as creditLimit,isnull(isActive,0) as isActive,isnull(createDate,'') as createDate,isnull(customerType,0) as customerType ";
        StrQuery += " from customer where customerId=@customerId ";

        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = customerId;
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }


    //
    //public string getlatlong(string add1, string add2, string city, string state, string county, string zipcode)
    /// <summary>
    /// get latitude and longitude
    /// </summary>
    /// <param name="add1"></param>
    /// <param name="city"></param>
    /// <param name="county"></param>
    /// <returns></returns>
    public string getlatlong(string add1, string city, string county)
    {
        //string url = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + HttpUtility.HtmlEncode(zipcode) + " " + HttpUtility.HtmlEncode(add1) + " " + HttpUtility.HtmlEncode(add2) + " " + HttpUtility.HtmlEncode(city) + " " + HttpUtility.HtmlEncode(state) + " " + HttpUtility.HtmlEncode(county) + "&sensor=false";
        //string url = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + HttpUtility.HtmlEncode(add1) + "+" + HttpUtility.HtmlEncode(add2) + "+" + HttpUtility.HtmlEncode(city) + "+" + HttpUtility.HtmlEncode(state) + "+" + HttpUtility.HtmlEncode(zipcode) + "+" + HttpUtility.HtmlEncode(county) + "&sensor=false";
        string url = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + HttpUtility.HtmlEncode(add1) + "+" + HttpUtility.HtmlEncode(city) + "+" + HttpUtility.HtmlEncode(county) + "&sensor=false";
        WebResponse response1 = default(WebResponse);
        WebRequest request = WebRequest.Create(new Uri(url.Replace(' ', '+')));
        response1 = request.GetResponse();
        XmlTextReader xmlResponse = new XmlTextReader(response1.GetResponseStream());
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(xmlResponse);
        XmlNode status = xmldoc.SelectSingleNode("GeocodeResponse/status");

        string lat = "";
        string lang = "";

        if (status.InnerText.ToLower() == "ok")
        {
            XmlNode LattNode = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lat");
            XmlNode LongNode = xmldoc.SelectSingleNode("GeocodeResponse/result/geometry/location/lng");
            if (string.IsNullOrEmpty(LongNode.InnerText.Trim()) | LongNode.InnerText.Trim() == "-")
            {
                lang = "0";
            }
            else
            {
                lang = LongNode.InnerText.Trim();
            }

            if (string.IsNullOrEmpty(LattNode.InnerText.Trim()))
            {
                lat = "0";
            }
            else
            {
                lat = LattNode.InnerText.Trim();
            }
        }
        else
        {
            lang = "0";
            lat = "0";
        }
        return Convert.ToDecimal(lat) + "," + Convert.ToDecimal(lang);
    }

    // 
    /// <summary>
    /// bind customer name for autocomplit 
    /// </summary>
    /// <returns></returns>
    public DataTable BindCustomerName()
    {
        //StrQuery = "select isnull(contactname,'') as contactname from customer where contactName LIKE ''+@contactName+'%'";
        StrQuery = "select isnull(companyName,'') as companyName from customer where companyName LIKE '%'+@companyName+'%'";
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.Add(new SqlParameter("@companyName", SqlDbType.VarChar)).Value = companyName;
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }

    // 
    /// <summary>
    /// get customer id from customer name
    /// </summary>
    /// <returns></returns>
    public int GetCustomerId()
    {
        StrQuery = "select isnull(customerid,0) as customerid from customer where contactName=@contactName";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@contactName", contactName);
            int cust_id = 0;
            cust_id = Convert.ToInt32(sqlcmd.ExecuteScalar());
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
    /// get customerid,discount,credit from contactname
    /// </summary>
    /// <returns></returns>
    public DataTable GetCustomerIdAndDiscountCredit()
    {
        DataTable dt = new DataTable();
        //StrQuery = "select isnull(customerid,0) as customerid,isnull(globleDiscountRate,'') as globleDiscountRate,isnull(creditLimit,'') as creditLimit,isnull(reducePercent,'') as reducePercent  from customer where contactName=@contactName";
        StrQuery = "select isnull(customerid,0) as customerid,isnull(globleDiscountRate,'') as globleDiscountRate,isnull(creditLimit,'') as creditLimit,isnull(reducePercent,'') as reducePercent  from customer where companyName=@companyName";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@companyName", companyName);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
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
    /// get customerid,discount,credit from contactname
    /// </summary>
    /// <returns></returns>
    public DataTable GetDiscountAndCreditfromCustomerid()
    {
        DataTable dt = new DataTable();
        StrQuery = "select isnull(globleDiscountRate,'') as globleDiscountRate,isnull(creditLimit,'') as creditLimit from customer where customerId=@customerId";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@customerId", customerId);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
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
    /// get total of orders and total of ammount
    /// </summary>
    /// <returns></returns>
    public DataTable GetSalesAndOrderTotalByCustomerId()
    {
        DataTable dt = new DataTable();
        StrQuery = "select ISNULL(count(orderid),0) as TotalOrder,ISNULL(SUM(totalammount),0) as totalSales from tblorder ";
        StrQuery += "where  (orderstatus <> 0 or orderstatus is not null) and customerId=@customerId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("customerId", customerId);
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

    //
    /// <summary>
    /// get customer details from order view by customerid
    /// </summary>
    /// <returns></returns>
    public DataTable GetCustomerDetailsForOrderView()
    {
        StrQuery = " select isnull(customerid,0) as customerid,ISNULL(companyName,'') as companyName,isnull(streetAddress,'') as streetAddress ";
        StrQuery += " ,isnull(contactName,'') as contactName,isnull(mobile,'') as mobile,isnull(city,'') as city,isnull(country,'') as country ";
        StrQuery += " from customer where customerid=@customerId ";

        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = customerId;
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }

    // 
    /// <summary>
    /// get the company id from conpany name
    /// </summary>
    /// <returns></returns>
    public int GetCompanyIdFromCompanyName()
    {
        StrQuery = "select isnull(customerid,0) as customerid from customer where companyName=@companyName";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@companyName", companyName);
            int StrCompId = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return StrCompId;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //
    /// <summary>
    /// get discount,credit from contactname
    /// </summary>
    /// <returns></returns>
    public DataTable GetdDiscountCredit()
    {
        DataTable dt = new DataTable();
        StrQuery = "select isnull(globleDiscountRate,'') as globleDiscountRate,isnull(creditLimit,'') as creditLimit from customer where customerId=@customerId";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@customerId", customerId);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
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
    /// update credit limits and globlediscount
    /// </summary>
    public void UpdateCustomerCreditLimits()
    {
        StrQuery = "update customer set creditLimit=@creditLimit,globleDiscountRate=@globleDiscountRate where customerId=@customerId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@creditLimit", creditLimit);
            sqlcmd.Parameters.AddWithValue("@globleDiscountRate", globleDiscountRate);
            sqlcmd.Parameters.AddWithValue("@customerId", customerId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public string GetCustomerCreditFromName()
    {
        StrQuery = "select isnull(creditLimit,'') as creditLimit from customer where companyName=@companyName";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@companyName", companyName);
            string Credit = Convert.ToString(sqlcmd.ExecuteScalar());
            return Credit;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public decimal GetCustomerCreditFromID()
    {
        StrQuery = "select isnull(creditLimit,'') as creditLimit from customer where customerId=@customerId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@customerId", customerId);
            decimal Credit = Convert.ToDecimal(sqlcmd.ExecuteScalar());
            return Credit;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }
    //
    /// <summary>
    /// get discount,credit from contactname
    /// </summary>
    /// <returns></returns>
    public DataTable GetCustomerIdAndDiscountCreditByID()
    {
        DataTable dt = new DataTable();
        StrQuery = "select isnull(globleDiscountRate,'') as globleDiscountRate,isnull(creditLimit,'') as creditLimit,isnull(reducePercent,'') as reducePercent  from customer where customerId=@customerId";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@customerId", customerId);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public void UpdateCustomerCreditsLimits()
    {
        StrQuery = "update customer set creditLimit=@creditLimit where customerid=@customerId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@customerId", customerId);
            sqlcmd.Parameters.AddWithValue("@creditLimit", creditLimit);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }


    #endregion

    public string SearchKey { get; set; }
    public DataTable SearchKeywordCustomer()
    {
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = objcon;
            cmd.CommandText = "sp_SearchKeywordCustomer";
            cmd.Parameters.AddWithValue("@SearchKey", SearchKey);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }


}