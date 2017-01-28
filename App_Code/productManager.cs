using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for productManager
/// </summary>
public class productManager
{
    String StrQuery;
    DataTable dt = new DataTable();
    SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    #region
    public productManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region "----------------------------private variables------------------------"

    private int _productId;
    private string _productName;
    private string _productDescription;
    private string _sku;
    private string _barcode;
    private byte _isVarientProduct;
    private byte _isMasterProduct;
    private decimal _price;
    private decimal _cost;
    private int _minimumQuantity;
    private int _inventory;
    private byte _isactive;
    private byte _isFeatured;
    private string _updatedate;
    public string _createDate;
    private string _varientItem;
    private int _isDelete;
    private decimal _WholesalePrice;
    private decimal _SuperMarketPrice;
    private decimal _ConvinientStorePrice;
    private char _mainImage;
    private string _QRCOde;

    private int _productsLanguage;
    private int _productCategoryId;
    private int _categoryId;
    private int _productBrandId;
    private int _barndId;

    private int _productImagesId;
    private string _imageName;
    private string _actualImageName;
    private int _sortOrder;
    private string _imgLabel;


    public int _pageNo;
    public int _pageSize;
    public int _TotalRecord;
    public string _SortExpression;

    #endregion

    #region "----------------------------public properties------------------------"

    public int productId { get { return _productId; } set { _productId = value; } }
    public string productName { get { return _productName; } set { _productName = value; } }
    public string productDescription { get { return _productDescription; } set { _productDescription = value; } }
    public string sku { get { return _sku; } set { _sku = value; } }
    public string barcode { get { return _barcode; } set { _barcode = value; } }
    public byte isVarientProduct { get { return _isVarientProduct; } set { _isVarientProduct = value; } }
    public byte isMasterProduct { get { return _isMasterProduct; } set { _isMasterProduct = value; } }
    public decimal price { get { return _price; } set { _price = value; } }
    public decimal cost { get { return _cost; } set { _cost = value; } }
    public int minimumQuantity { get { return _minimumQuantity; } set { _minimumQuantity = value; } }
    public int inventory { get { return _inventory; } set { _inventory = value; } }
    public byte isactive { get { return _isactive; } set { _isactive = value; } }
    public byte isFeatured { get { return _isFeatured; } set { _isFeatured = value; } }
    public string updatedate { get { return _updatedate; } set { _updatedate = value; } }
    public string createDate { get { return _createDate; } set { _createDate = value; } }
    public string varientItem { get { return _varientItem; } set { _varientItem = value; } }
    public int isDelete { get { return _isDelete; } set { _isDelete = value; } }
    public decimal WholesalePrice { get { return _WholesalePrice; } set { _WholesalePrice = value; } }
    public decimal SuperMarketPrice { get { return _SuperMarketPrice; } set { _SuperMarketPrice = value; } }
    public decimal ConvinientStorePrice { get { return _ConvinientStorePrice; } set { _ConvinientStorePrice = value; } }
    public char mainImage { get { return _mainImage; } set { _mainImage = value; } }
    public string QRCOde { get { return _QRCOde; } set { _QRCOde = value; } }

    public int productsLanguage { get { return _productsLanguage; } set { _productsLanguage = value; } }
    public int productCategoryId { get { return _productCategoryId; } set { _productCategoryId = value; } }
    public int categoryId { get { return _categoryId; } set { _categoryId = value; } }
    public int productBrandId { get { return _productBrandId; } set { _productBrandId = value; } }
    public int barndId { get { return _barndId; } set { _barndId = value; } }
    public int id { get; set; }
    public int masterProductId { get; set; }
    public int productTag { get; set; }
    public string tagName { get; set; }
    public int languageId { get; set; }

    public int productImagesId { get { return _productImagesId; } set { _productImagesId = value; } }
    public string imageName { get { return _imageName; } set { _imageName = value; } }
    public string actualImageName { get { return _actualImageName; } set { _actualImageName = value; } }
    public int sortOrder { get { return _sortOrder; } set { _sortOrder = value; } }
    public string imgLabel { get { return _imgLabel; } set { _imgLabel = value; } }

    public int pageNo { get { return _pageNo; } set { _pageNo = value; } }
    public int pageSize { get { return _pageSize; } set { _pageSize = value; } }
    public int TotalRecord { get { return _TotalRecord; } set { _TotalRecord = value; } }
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    public string CategoryName { get; set; }
    public string CheckAll { get; set; }
    public string filter { get; set; }
    public string InventoryFilter { get; set; }


    public string masterProductName { get; set; }
    public string brandname { get; set; }
    public int parentid { get; set; }
    public string subcategoryname { get; set; }
    public byte isStatus { get; set; }
    public string FileError { get; set; }
    public int FileErrorLineNumber { get; set; }

    public string ArabicName { get; set; }
    public string ArabicDescription { get; set; }

    #endregion

    #region "----------------------------public methods-------------------------"

    //search product details
    // public DataTable SearchItem()
    // {
    // DataTable dt = new DataTable();
    // try
    // {
    // SqlCommand sqlCmd = new SqlCommand();
    // sqlCmd.CommandText = "[sp_SearchProduct]";
    // sqlCmd.CommandType = CommandType.StoredProcedure;
    // sqlCmd.Parameters.AddWithValue("@productName", productName);
    // sqlCmd.Parameters.AddWithValue("@categoryId", categoryId);
    // sqlCmd.Parameters.AddWithValue("@brandId", barndId);
    // sqlCmd.Parameters.AddWithValue("@isactive", isactive);
    // sqlCmd.Parameters.AddWithValue("@pageNo", pageNo);
    // sqlCmd.Parameters.AddWithValue("@pageSize", pageSize);
    // sqlCmd.Parameters.AddWithValue("@TotalRowsNum", TotalRecord);
    // sqlCmd.Parameters.AddWithValue("@SortExpression", SortExpression);
    // sqlCmd.Parameters["@TotalRowsNum"].Direction = ParameterDirection.Output;
    // sqlCmd.Parameters["@TotalRowsNum"].SqlDbType = SqlDbType.Int;
    // sqlCmd.Parameters["@TotalRowsNum"].Size = 4000;
    // objcon.Open();
    // sqlCmd.Connection = objcon;
    // sqlCmd.CommandTimeout = 6000;
    // SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
    // sqlAdp.Fill(dt);
    // TotalRecord = sqlCmd.Parameters["@TotalRowsNum"].Value == null ? 0 : Convert.ToInt32(sqlCmd.Parameters["@TOTALRowsNum"].Value);
    // return dt;
    // }
    // catch (Exception ex) { throw ex; }
    // finally { dt.Dispose(); objcon.Close(); }
    // }

    public DataSet SearchItem()
    {
        DataSet ds = new DataSet();
        try
        {
            objcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = objcon;
            //cmd.CommandText = "sp_SearchProduct";
            cmd.CommandText = "sp_SearchProductCatBrand";
            cmd.Parameters.AddWithValue("@productName", productName);
            cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
            cmd.Parameters.AddWithValue("@isactive", isactive);
            cmd.Parameters.AddWithValue("@isFeatured", isFeatured);
            cmd.Parameters.AddWithValue("@CheckAll", CheckAll);
            cmd.Parameters.AddWithValue("@pageNo", pageNo);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SortExpression", SortExpression);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    //search master product details
    public DataTable SearchMasterProductItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchMasterProduct]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@productName", productName);
            sqlCmd.Parameters.AddWithValue("@categoryId", categoryId);
            sqlCmd.Parameters.AddWithValue("@brandId", barndId);
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

    //search product inventory details
    public DataTable SearchProductInventoryItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchProductInventory]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@sku", sku);
            sqlCmd.Parameters.AddWithValue("@filter", filter);
            sqlCmd.Parameters.AddWithValue("@inventory", InventoryFilter);
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

    //insert product details
    public int InsertItem()
    {
        StrQuery = " insert into [products]([productName],[productDescription],[sku],[barcode],[isVarientProduct],[isMasterProduct],[price],[cost],[minimumQuantity],[inventory],[isActive],[isFeatured],[createdate],[varientItem],[isDelete],[WholesalePrice],[SuperMarketPrice],[ConvinientStorePrice]) ";
        StrQuery += " values(@productName,@productDescription,@sku,@barcode,@isVarientProduct,@isMasterProduct,@price,@cost,@minimumQuantity,@inventory,@isactive,@isFeatured,@createDate,@varientItem,@isDelete,@WholesalePrice,@SuperMarketPrice,@ConvinientStorePrice)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productName", productName);
            sqlcmd.Parameters.AddWithValue("@productDescription", productDescription);
            sqlcmd.Parameters.AddWithValue("@sku", sku);
            sqlcmd.Parameters.AddWithValue("@barcode", barcode);
            sqlcmd.Parameters.AddWithValue("@isVarientProduct", isVarientProduct);
            sqlcmd.Parameters.AddWithValue("@isMasterProduct", isMasterProduct);
            sqlcmd.Parameters.AddWithValue("@price", price);
            sqlcmd.Parameters.AddWithValue("@cost", cost);
            sqlcmd.Parameters.AddWithValue("@minimumQuantity", minimumQuantity);
            sqlcmd.Parameters.AddWithValue("@inventory", inventory);
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            sqlcmd.Parameters.AddWithValue("@isFeatured", isFeatured);
            sqlcmd.Parameters.AddWithValue("@createDate", DateTime.Now);
            sqlcmd.Parameters.AddWithValue("@varientItem", varientItem);
            sqlcmd.Parameters.AddWithValue("@isDelete", 0);
            sqlcmd.Parameters.AddWithValue("@WholesalePrice", WholesalePrice);
            sqlcmd.Parameters.AddWithValue("@SuperMarketPrice", SuperMarketPrice);
            sqlcmd.Parameters.AddWithValue("@ConvinientStorePrice", ConvinientStorePrice);

            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //update product details by id
    public void UpdateItem()
    {
        StrQuery = " update products set productName=@productName,productDescription=@productDescription,sku=@sku,barcode=@barcode,isVarientProduct=@isVarientProduct,isMasterProduct=@isMasterProduct ";
        StrQuery += " ,price=@price,cost=@cost,minimumQuantity=@minimumQuantity,inventory=@inventory,isactive=@isactive,isFeatured=@isFeatured,updatedate=@updatedate,varientItem=@varientItem,isDelete=@isDelete ";
        StrQuery += " ,WholesalePrice=@WholesalePrice,SuperMarketPrice=@SuperMarketPrice,ConvinientStorePrice=@ConvinientStorePrice where productId=@productId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@productName", productName);
            sqlcmd.Parameters.AddWithValue("@productDescription", productDescription);
            sqlcmd.Parameters.AddWithValue("@sku", sku);
            sqlcmd.Parameters.AddWithValue("@barcode", barcode);
            sqlcmd.Parameters.AddWithValue("@isVarientProduct", isVarientProduct);
            sqlcmd.Parameters.AddWithValue("@isMasterProduct", isMasterProduct);
            sqlcmd.Parameters.AddWithValue("@price", price);
            sqlcmd.Parameters.AddWithValue("@cost", cost);
            sqlcmd.Parameters.AddWithValue("@minimumQuantity", minimumQuantity);
            sqlcmd.Parameters.AddWithValue("@inventory", inventory);
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            sqlcmd.Parameters.AddWithValue("@isFeatured", isFeatured);
            sqlcmd.Parameters.AddWithValue("@updatedate", DateTime.Now);
            sqlcmd.Parameters.AddWithValue("@varientItem", varientItem);
            sqlcmd.Parameters.AddWithValue("@isDelete", 0);
            sqlcmd.Parameters.AddWithValue("@WholesalePrice", WholesalePrice);
            sqlcmd.Parameters.AddWithValue("@SuperMarketPrice", SuperMarketPrice);
            sqlcmd.Parameters.AddWithValue("@ConvinientStorePrice", ConvinientStorePrice);

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //delete product
    public void DeleteItem()
    {
        //StrQuery = "delete from [products] where productId=@productId";
        StrQuery = " update products set isDelete=@isDelete where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.Parameters.AddWithValue("@isDelete", 1);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //Check product is already exist or not
    public bool TitleExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(productId) from [products] where productName = @productName";
            if (productId != 0)
            {
                StrQuery += "  and productId <> @productId";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productName", SqlDbType.VarChar, 50)).Value = productName;
            if (productId != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
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

    //Check product is already exist or not
    public bool SkuExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(productId) from [products] where sku = @sku";
            if (productId != 0)
            {
                StrQuery += "  and productId <> @productId";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@sku", SqlDbType.VarChar, 50)).Value = sku;
            if (productId != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
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

    //update product status
    public void UpdateStatus()
    {
        StrQuery = " update [products] set [isactive]=@isactive where productId=@productId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Bit)).Value = isactive;
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //select single admintype details 
    public DataTable SelectSingleItemById()
    {
        StrQuery = " select isnull(productId,0) as productId,isnull(productName,'') as productName,isnull(productDescription,'') as productDescription,isnull(varientItem,'') as varientItem ";
        StrQuery += " ,isnull(sku,'') as sku,isnull(barcode,'') as barcode,isnull(isVarientProduct,0) as isVarientProduct,isnull(isMasterProduct,0) as isMasterProduct ";
        StrQuery += " ,isnull(price,0) as price,isnull(cost,0) as cost,isnull(minimumQuantity,0) as minimumQuantity,isnull(inventory,0) as inventory,isnull(isActive,0) as isActive ";
        StrQuery += " ,isnull(isFeatured,0) as isFeatured,isnull(createdate,'') as createdate,isnull(updatedate,'') as updatedate,isnull(isvarientproduct,0) as isvarientproduct ";
        StrQuery += " ,isnull(WholesalePrice,0) as WholesalePrice,isnull(SuperMarketPrice,0) as SuperMarketPrice,isnull(ConvinientStorePrice,0) as ConvinientStorePrice ";
        StrQuery += " from products";
        StrQuery += " where productId=@productId ";
        StrQuery += " order by productName ";

        try
        {
            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();

            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;

            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    public int getmaxid()
    {
        StrQuery = "Select Max(productId) from products";
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

    public DataTable MasterproductBindWithName()
    {
        StrQuery = "select productid,sku,productname from products where isMasterproduct=1 and productname=@productname";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.Add(new SqlParameter("@productname", SqlDbType.VarChar)).Value = productName;
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    public DataTable MasterproductBind()
    {
        StrQuery = "select productid,productname,sku from products where isMasterproduct=1 and sku=@sku";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.Add(new SqlParameter("@sku", SqlDbType.VarChar)).Value = sku;
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    public DataTable bindMsaterProductByProductId()
    {
        StrQuery = "select isnull(mp.masterproductid,0) as masterproductid,isnull(p.productname,'') as productname,isnull(p.sku,'') as sku ";
        StrQuery += " from masterProduct as mp ";
        StrQuery += " inner join products as p on p.productid=mp.masterproductid ";
        StrQuery += " where mp.productid=@productid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.Add(new SqlParameter("@productid", SqlDbType.Int)).Value = productId;
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    //update product status
    public void UpdateInventory()
    {
        StrQuery = " update [products] set [inventory]=@inventory where productId=@productId ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@inventory", SqlDbType.Int)).Value = inventory;
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public int NextProductId()
    {
        StrQuery = "select top 1 * from products where productid > @productId and ismasterproduct=0 order by productId asc";
        try
        {
            int nextprod_id = 0;
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            nextprod_id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return nextprod_id;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public int PreviewProductId()
    {
        StrQuery = "select top 1 * from products where productid < @productId and ismasterproduct=0 order by productId desc";
        try
        {
            int previewprod_id = 0;
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            previewprod_id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return previewprod_id;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public DataTable MasterproductBindSKU()
    {
        StrQuery = "select productid,productname,sku from products where isMasterproduct=1 and sku LIKE ''+@sku+'%'";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.Add(new SqlParameter("@sku", SqlDbType.VarChar)).Value = sku;
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    public int GetBrandIdFromBrandName()
    {
        StrQuery = "select brandid from brand where brandname=@brandname";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@brandname", productName);
            int bradid = 0;
            bradid = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return bradid;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //Bind Master Product Name
    public DataTable BindMasterProductName()
    {
        StrQuery = "select productid,productname,sku from products where isMasterproduct=1 and productname LIKE ''+@productname+'%'";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.Add(new SqlParameter("@productname", SqlDbType.VarChar)).Value = productName;
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    //Bind all sku for dropdownlist
    public DataTable BindAllSkuDDL()
    {
        StrQuery = "select isnull(sku,'') as sku,isnull(productid,0) as productid from products";
        //StrQuery = "select isnull(sku,'') as sku,isnull(productid,0) as productid from products where ismasterproduct=0";
        try
        {
            objcon.Open();
            SqlDataAdapter sqlsda = new SqlDataAdapter(StrQuery, objcon);
            DataTable dt = new DataTable();
            sqlsda.Fill(dt);
            return dt;

        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //get sku from product id
    public string GetSkuFromProductid()
    {
        StrQuery = "select isnull(sku,'') as sku from products where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            string prSku = Convert.ToString(sqlcmd.ExecuteScalar());
            return prSku;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // get the product count from SKU
    public int GetSkuCount()
    {
        StrQuery = "select isnull(productid,0) as productid from products where sku=@sku";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@sku", sku);
            int cunt = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return cunt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public int GetproductsImageCount(int proid)
    {
        StrQuery = "select productid from productimages where productId=" + proid + "";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            int cont = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return cont;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public DataTable GetProductImageNameByProductId(int proid)
    {
        StrQuery = "select isnull(ImageName,'') as imageName from productimages  where productId=" + proid + "";
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
        finally { objcon.Close(); dt.Dispose(); }
    }

    // get the product count from productid
    public int GetProdutctidCount()
    {
        StrQuery = "select isnull(productid,0) as productid from products where sku=@sku";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@sku", sku);
            int prdouctcount = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return prdouctcount;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //Check product is already exist or not
    public string SkuIsExist()
    {
        try
        {
            int id = 0;

            StrQuery = "select count(productId) from [products] where sku = @sku";
            if (productId != 0)
            {
                StrQuery += "  and productId <> @productId";
            }
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@sku", SqlDbType.VarChar, 50)).Value = sku;
            if (productId != 0)
            {
                sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            }

            id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            if (id == 0)
            {
                return "No";
            }
            else
            {
                return "Yes";
            }
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    /// <summary>
    /// Get product name from the product id for image label
    /// </summary>
    /// <returns></returns>
    public string GetProductNameByProductid()
    {
        StrQuery = "select isnull(productName,'') as productName from products where productid=@productid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            string pName = Convert.ToString(sqlcmd.ExecuteScalar());
            return pName;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public void UpdateQRCode()
    {
        StrQuery = "update products set QRCOde=@QRCOde where productid=@productid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@QRCOde", QRCOde);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }


    #endregion


    #region  ----------------------- PRODUCT LANGUAGE ------------------------------------------

    //insert product langauage details
    public int InsertProductLanguageItem()
    {
        StrQuery = " insert into [productsLanguage]([languageid],[productId],[productName],[productDescription]) values(@languageid,@productId,@productName,@productDescription)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@languageId", languageId);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@productName", productName);
            sqlcmd.Parameters.AddWithValue("@productDescription", productDescription);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //update product language by
    public void UpdateProductLanguageItem()
    {
        StrQuery = " update productsLanguage set productName=@productName,productDescription=@productDescription,productId=@productId where productsLanguage=@productsLanguage ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@productName", productName);
            sqlcmd.Parameters.AddWithValue("@productDescription", productDescription);
            sqlcmd.Parameters.AddWithValue("@productsLanguage", productsLanguage);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //update product language by productid and language id
    public void UpdateProductLanguage()
    {
        StrQuery = " update productsLanguage set productName=@productName,productDescription=@productDescription,productId=@productId where productsLanguage=@productsLanguage and languageId=@languageId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@languageId", languageId);
            sqlcmd.Parameters.AddWithValue("@productName", productName);
            sqlcmd.Parameters.AddWithValue("@productDescription", productDescription);
            sqlcmd.Parameters.AddWithValue("@productsLanguage", productsLanguage);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //delete product language
    public void DeleteProductLanguage()
    {
        StrQuery = "delete from [productsLanguage] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    //select product language
    public void selectProductLanguage()
    {
        StrQuery = "select * from [productsLanguage] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    public DataTable SelectProductLanguagebyID()
    {
        try
        {
            //StrQuery = "select isnull(languagename,'') as languagename ,isnull(brandLanguage,0) as brandLanguage,isnull(textAlign,'false') as textAlign ";
            //StrQuery += ",isnull(brandId,0) as brandId,isnull(L.LanguageId,0) as LanguageId,isnull(brandName,'') as brandName, ";
            //StrQuery += " isnull(brandDescription,'') as brandDescription from productsLanguage cl  inner join language l on cl.LanguageId = l.languageid where brandId=@brandId";

            StrQuery = " select isnull(languagename,'') as languagename ,isnull(productsLanguage,0) as productsLanguage,";
            StrQuery += " isnull(textAlign,'') as textAlign,isnull(productId,0) as productId,isnull(L.LanguageId,0) as LanguageId, ";
            StrQuery += "isnull(productName,'') as productName,isnull(ProductDescription,'') as ProductDescription ";
            StrQuery += "from productsLanguage cl  ";
            StrQuery += "inner join language l on cl.LanguageId = l.languageid where productId=@productId";

            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
        finally { objcon.Close(); }
    }


    public DataTable SelectProductNameDeasripeionID()
    {
        try
        {

            StrQuery = " select isnull(sku,'') as sku,isnull(barcode,'') as barcode,isnull(productName,'') as productName ,isnull(ProductDescription,'') as ProductDescription ";
            StrQuery += "from products  ";
            StrQuery += "where productId=@productId";

            dt = new DataTable();
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
        finally { objcon.Close(); }
    }

    // bind all the sku
    public DataTable BindAllSKU()
    {
        StrQuery = "select top 5 productid,productname,sku from products where sku LIKE ''+@sku+'%'";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqlcmd.Parameters.Add(new SqlParameter("@sku", SqlDbType.VarChar)).Value = sku;
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    #endregion

    #region  --------------- PRODUCT CATEGORY <TAGES> AUTOSUGGESSION ----------------------------

    //insert product langauage details
    public int InsertProductCategroyItem()
    {
        StrQuery = " insert into [productCategory]([productId],[categoryId]) values (@productId,@categoryId)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@categoryId ", categoryId);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //update product category
    public void UpdateProductCategoryItem()
    {
        StrQuery = " update productCategory set productId=@productId,categoryId=@categoryId where productCategoryId=@productCategoryId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@categoryId", categoryId);
            sqlcmd.Parameters.AddWithValue("@productCategoryId", productCategoryId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //delete product category
    public void DeleteProductCategory()
    {
        StrQuery = "delete from [productCategory] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //select product category
    public DataTable SelectProductCategory()
    {
        DataTable dt = new DataTable();
        StrQuery = "select * from [productCategory] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }

    #endregion


    #region  --------------- PRODUCT BRAND <TAGES> AUTOSUGGESSION ----------------------------

    //insert product brand details
    public int InsertProductBrandItem()
    {
        StrQuery = " insert into [productBrand]([productId],[barndId]) values (@productId,@barndId)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@barndId ", barndId);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //update product Brand
    public void UpdateProductBrandtem()
    {
        StrQuery = " update productBrand set productId=@productId,barndId=@barndId where productBrandId=@productBrandId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@barndId", barndId);
            sqlcmd.Parameters.AddWithValue("@productBrandId", productBrandId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //delete product brand
    public void DeleteProductBrand()
    {
        StrQuery = "delete from [productBrand] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }

    }

    //select product Brand
    public DataTable SelectProductBrand()
    {
        StrQuery = "select * from [productBrand] where productId=@productId";
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }

    #endregion

    #region --------------------- PRODUCT IMAGE -----------------------------

    //insert product brand details

    public int InsertProductImageItem()
    {
        StrQuery = " insert into [productImages]([productId],[isactive],[sortOrder]) values (@productId,@isactive,@sortOrder)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            sqlcmd.Parameters.AddWithValue("@sortOrder", sortOrder);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //insert product brand details
    public int UpdateImage()
    {
        StrQuery = "update productImages set imageName=@imageName,actualImageName=@actualImageName,imgLabel=@imgLabel where productimagesid=@productimagesid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productimagesid", productImagesId);
            sqlcmd.Parameters.AddWithValue("@imageName", imageName);
            sqlcmd.Parameters.AddWithValue("@actualImageName", actualImageName);
            sqlcmd.Parameters.AddWithValue("@imgLabel", imgLabel);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //update product image
    public void UpdateProductImagetem()
    {
        StrQuery = " update productImages set productId=@productId,imageName=@imageName,actualImageName=@actualImageName,isactive=@isactive,sortOrder=@sortOrder,imgLabel=@imgLabel where productImagesId=@productImagesId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productImagesId", productImagesId);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@imageName", imageName);
            sqlcmd.Parameters.AddWithValue("@actualImageName", actualImageName);
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            sqlcmd.Parameters.AddWithValue("@sortOrder", sortOrder);
            sqlcmd.Parameters.AddWithValue("@imgLabel", imgLabel);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //delete product brand
    public void DeleteProductImage()
    {
        StrQuery = "delete from [productImages] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //select product Brand
    public DataTable SelectProductImage()
    {
        DataTable dt = new DataTable();
        StrQuery = "select isnull(productImagesId,0) as productImagesId,isnull(productId,0) as productId,ISNULL(imageName,'') as imageName,ISNULL(actualImageName,'') as actualImageName,isnull(mainImage,'') as mainImage from [productImages] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }

    public int GetmaximageProductId()
    {
        int imageid = 0;
        StrQuery = "Select Max(productImagesId) from productImages";
        SqlCommand objCmd = null;
        try
        {
            objcon.Open();
            objCmd = new SqlCommand(StrQuery, objcon);
            imageid = Convert.ToInt32(objCmd.ExecuteScalar());
            return imageid;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); objCmd.Dispose(); }
    }

    public void DeleteProductImageByProductImageId()
    {
        StrQuery = "delete from [productImages] where productImagesId=@productImagesId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productImagesId", SqlDbType.Int)).Value = productImagesId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public int InsertDuplicateProductImageItem()
    {
        StrQuery = " insert into [productImages]([productId],[imageName],[actualImageName],[isactive],[sortOrder],[imgLabel]) values (@productId,@imageName,@actualImageName,@isactive,@sortOrder,@imgLabel)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@imageName", imageName);
            sqlcmd.Parameters.AddWithValue("@actualImageName", actualImageName);
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            sqlcmd.Parameters.AddWithValue("@sortOrder", sortOrder);
            sqlcmd.Parameters.AddWithValue("@imgLabel", imgLabel);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    // get all the images from Product Image
    public DataTable SearchProductImageItem()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchProductImages]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@sku", sku);
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

    // get all the images from Product Image
    public DataTable SearchProductImageItemTodaysOnly()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "[sp_SearchProductImagesUpload]";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@sku", sku);
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

    //update product id using productImageId
    public void UpdateProductID()
    {
        StrQuery = "update productImages set productId=@productId,imageName=@imageName,imgLabel=@imgLabel where productImagesId=@productImagesId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productImagesId", productImagesId);
            sqlcmd.Parameters.AddWithValue("@imageName", imageName);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@imgLabel", imgLabel);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public string getProductImageName()
    {
        StrQuery = "select isnull(imagename,'') as imagename from productimages where productImagesId=@productImagesId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productImagesId", productImagesId);
            string strImg = Convert.ToString(sqlcmd.ExecuteScalar());
            return strImg;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { objcon.Close(); }
    }

    // get all product images
    public DataTable getAllProductImageByProductid()
    {
        StrQuery = "select isnull(imagename,'') as imagename,isnull(productImagesId,0) as productImagesId from productimages where productid=@productid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            dt = new DataTable();
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); dt.Dispose(); }
    }

    // update image name by productImageId
    public void UpdateImageNameByProductImageId()
    {
        StrQuery = "update productimages set imageName=@imageName where productImagesId=@productImagesId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productImagesId", productImagesId);
            sqlcmd.Parameters.AddWithValue("@imageName", imageName);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    //24_01_2017

    public void UpdateProductImageByProductImageIdSetMain()
    {
        StrQuery = "update [productImages] set mainImage=@mainImage where productImagesId=@productImagesId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productImagesId", SqlDbType.Int)).Value = productImagesId;
            sqlcmd.Parameters.AddWithValue("@mainImage", mainImage);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public void UpdateProductImageSetMainByProductid()
    {
        StrQuery = "update [productImages] set mainImage=@mainImage where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.Parameters.AddWithValue("@mainImage", mainImage);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public int getProductidFromProductimageId()
    {
        StrQuery = "select isnull(productid,0) as productid from productImages where productImagesId=@productImagesId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productImagesId", productImagesId);
            int ids = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return ids;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public int getImagesCountByProductID()
    {
        StrQuery = "select count(*) from productImages where productId=@productId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            int ids = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return ids;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    #endregion

    #region   ---------------------------  ADD PRODUCT AND MASTER PRODUCT LINK ------------------------

    //insert master and product link
    public int InsertMasterProductLinkItem()
    {
        StrQuery = " insert into [masterProduct]([productid],[masterProductId]) values (@productid,@masterProductId)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@masterProductId ", masterProductId);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //update master product link
    public void UpdatematerProductLinkitem()
    {
        StrQuery = " update masterProduct set productId=@productId,masterProductId=@masterProductId where id=@id ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@id", id);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@masterProductId ", masterProductId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //delete product brand
    public void DeleteMasterProductLink()
    {
        StrQuery = "delete from [masterProduct] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //select product Brand
    public DataTable SelectMasterProductLink()
    {
        DataTable dt = new DataTable();
        StrQuery = "select * from [masterProduct] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }

    //update master product link
    public void UpdatematerProductLinkitemByProductid()
    {
        StrQuery = " update masterProduct set masterProductId=@masterProductId where productId=@productId ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@masterProductId ", masterProductId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }


    #endregion

    #region ---------------------------- PRODUCT TAGES -------------------------------

    //insert product tag
    public int InsertProductTagItem()
    {
        StrQuery = " insert into [productTag]([productId],[tagName],[sortOrder],[isactive]) values (@productId,@tagName,@sortOrder,@isactive)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@tagName ", tagName);
            sqlcmd.Parameters.AddWithValue("@sortOrder", sortOrder);
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //update product tag
    public void UpdateProductTagItem()
    {
        StrQuery = " update productTag set productId=@productId,tagName=@tagName,sortOrder=@sortOrder,isactive=@isactive where productTag=@productTag ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productTag", productTag);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
            sqlcmd.Parameters.AddWithValue("@tagName ", tagName);
            sqlcmd.Parameters.AddWithValue("@sortOrder", sortOrder);
            sqlcmd.Parameters.AddWithValue("@isactive", isactive);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //delete product tag
    public void DeleteProductTag()
    {
        StrQuery = "delete from [productTag] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    //select product tag
    public DataTable SelectProductTag()
    {
        DataTable dt = new DataTable();
        StrQuery = "select * from [productTag] where productId=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); dt.Dispose(); }
    }

    #endregion

    #region ---------------------------- PRODUCT CSV ----------------------------------

    public DataSet SelectProductDetailForGenerateCSV()
    {
        DataSet ds = new DataSet();
        try
        {
            StrQuery = "SP_GetallproductdetialsforCSV";

            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    public DataTable SelectProductDetailForGenerateXls()
    {
        DataTable ds = new DataTable();
        try
        {
            StrQuery = "SP_GetallproductdetialsforCSV";

            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    public void DeleteTempProductRecords()
    {
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand("truncate table tmp_Updatable ", objcon);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public void SqlBulkCopyOperation(DataTable dtUpdate)
    {
        objcon.Open();
        try
        {
            using (SqlBulkCopy s = new SqlBulkCopy(objcon))
            {
                s.DestinationTableName = "tmp_Updatable";
                foreach (var column in dtUpdate.Columns)
                {
                    s.ColumnMappings.Add(column.ToString(), column.ToString());
                }
                s.WriteToServer(dtUpdate);
                s.Close();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public void InsertUpdateProductFromTemp()
    {
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "changeall";
            sqlcmd.CommandTimeout = 6000;
            sqlcmd.Connection = objcon;
            objcon.Open();
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }


    }

    #endregion

    #region   ---------------------------- INVENTORY CSV --------------------------------------

    public DataSet SelectInventoryDetailForGenerateCSV()
    {
        DataSet ds = new DataSet();
        try
        {
            StrQuery = "SP_GetallInventorydetialsforCSV";

            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@sku", sku);
            sqlcmd.Parameters.AddWithValue("@filter", filter);
            sqlcmd.Parameters.AddWithValue("@inventory", InventoryFilter);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    public DataTable SelectInventoryDetailForGenerateXls()
    {
        DataTable ds = new DataTable();
        try
        {
            StrQuery = "SP_GetallInventorydetialsforCSV";

            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@sku", sku);
            sqlcmd.Parameters.AddWithValue("@filter", filter);
            sqlcmd.Parameters.AddWithValue("@inventory", InventoryFilter);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    public void DeleteTempInventoryRecords()
    {
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand("truncate table tmp_Inventory ", objcon);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public void SqlBulkCopyOperationInventory(DataTable dtUpdate)
    {
        objcon.Open();
        try
        {
            using (SqlBulkCopy s = new SqlBulkCopy(objcon))
            {
                s.DestinationTableName = "tmp_Inventory";
                foreach (var column in dtUpdate.Columns)
                {
                    s.ColumnMappings.Add(column.ToString(), column.ToString());
                }
                s.WriteToServer(dtUpdate);
                s.Close();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public void InsertUpdateInventoryFromTemp()
    {
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "changeall_Inventory";
            sqlcmd.CommandTimeout = 6000;
            sqlcmd.Connection = objcon;
            objcon.Open();
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }


    }

    #endregion


    #region ---------------------------- MASTER PRODUCT CSV ----------------------------------

    public DataSet SelectMasterProductDetailForGenerateCSV()
    {
        DataSet ds = new DataSet();
        try
        {
            StrQuery = "SP_GetallMasterProductforCSV";

            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    public DataTable SelectMasterProductDetailForGeneratexls()
    {
        DataTable ds = new DataTable();
        try
        {
            StrQuery = "SP_GetallMasterProductforCSV";

            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    public void DeleteTempMasterProductRecords()
    {
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand("truncate table tmp_MasterProductCsv ", objcon);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public void SqlBulkMasterProductCopyOperation(DataTable dtUpdate)
    {
        objcon.Open();
        try
        {
            using (SqlBulkCopy s = new SqlBulkCopy(objcon))
            {
                s.DestinationTableName = "tmp_MasterProductCsv";
                foreach (var column in dtUpdate.Columns)
                {
                    s.ColumnMappings.Add(column.ToString(), column.ToString());
                }
                s.WriteToServer(dtUpdate);
                s.Close();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public void InsertUpdateMasterProductFromTemp()
    {
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "changeall_MasterProduct";
            sqlcmd.CommandTimeout = 6000;
            sqlcmd.Connection = objcon;
            objcon.Open();
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }


    }

    #endregion

    #region -------------------------------------  Import Product -----------------------------------------

    public void SqlBulkCopyOperationImportProduct(DataTable dtUpdate)
    {
        objcon.Open();
        try
        {
            using (SqlBulkCopy s = new SqlBulkCopy(objcon))
            {
                s.DestinationTableName = "tmp_productImport";
                foreach (var column in dtUpdate.Columns)
                {
                    s.ColumnMappings.Add(column.ToString(), column.ToString());
                }
                s.WriteToServer(dtUpdate);
                s.Close();
            }
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public DataTable GetSingleImportProductValue()
    {
        StrQuery = "select ISNULL(productname,'') as productname,ISNULL(productdescription,'') as productdescription,isnull(sku,'') as sku, ";
        StrQuery += " isnull(barcode,'') as barcode,ISNULL(isvarientproduct,0) as isvarientproduct,ISNULL(ismasterproduct,0) as ismasterproduct,isnull(price,0) as price,  ";
        StrQuery += " ISNULL(cost,0) as cost,ISNULL(minimumQuantity,0) as minimumquantity,ISNULL(inventory,0) as inventory  ";
        StrQuery += " ,isnull(isactive,0) as isactive,isnull(isfeatured,0) as isfeatured, ";
        StrQuery += " isnull(varientitem,'') as varientitem,isnull(master_product_parent,'') as master_product_parent,isnull(categoryname,'') as categoryname, ";
        StrQuery += " isnull(subcategoryname,'') as subcategoryname,isnull(brandname,'') as brandname,isnull(ArabicName,'') as ArabicName,isnull(ArabicDesc,'') as ArabicDesc ";
        StrQuery += " from tmp_productImport where id=@id ";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@id", id);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            dt = new DataTable();
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); dt = null; }
    }

    // update single record
    public void updateSingleImportProduct()
    {
        StrQuery = "update tmp_productImport set FileError=@FileError,FileErrorLineNumber=@FileErrorLineNumber where id=@id";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@FileError", FileError);
            sqlcmd.Parameters.AddWithValue("@FileErrorLineNumber", FileErrorLineNumber);
            sqlcmd.Parameters.AddWithValue("@id", id);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }


    public DataTable GetSingleImportInventoryValue()
    {
        StrQuery = "select isnull(sku,'') as sku, ";
        StrQuery += " ISNULL(inventory,0) as inventory ";
        StrQuery += " from tmp_productImport where id=@id";

        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@id", id);
            SqlDataAdapter sqlsda = new SqlDataAdapter(sqlcmd);
            dt = new DataTable();
            sqlsda.Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); dt = null; }
    }

    // update temp table status

    public void UpdateTempTableStatus()
    {
        StrQuery = "update tmp_productImport set isStatus=@isStatus where id=@id";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@id", id);
            sqlcmd.Parameters.AddWithValue("@isStatus", isStatus);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    #endregion

    #region ------------------------------ Order -------------------------------

    public DataSet SearchOrder()
    {
        DataSet ds = new DataSet();
        try
        {
            objcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = objcon;
            //cmd.CommandText = "sp_SearchProduct";
            cmd.CommandText = "sp_SearchOrderProduct";
            cmd.Parameters.AddWithValue("@productName", productName);
            cmd.Parameters.AddWithValue("@pageNo", pageNo);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@TotalRowsNum", TotalRecord);
            cmd.Parameters.AddWithValue("@SortExpression", SortExpression);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(ds);
            return ds;
        }
        catch (Exception ex) { throw ex; }
        finally { ds.Dispose(); objcon.Close(); }
    }

    #endregion

    public DataTable SKUBind()
    {
        StrQuery = "select isnull(productid,0) as productid,isnull(sku,'')  as sku,isnull(productname,'') as productname from products where isMasterproduct=1";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            SqlDataAdapter sqladp = new SqlDataAdapter();
            sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }

    // select category id from category name
    public int getCategoryId()
    {
        StrQuery = "select categoryid from category where CategoryName=@CategoryName ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@CategoryName", CategoryName);
            int ct_id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return ct_id;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }


    // get categoryid by parent id
    public int GetCategoryidByParentID()
    {
        StrQuery = "select categoryid from category where CategoryName=@CategoryName and parentid=@parentid";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@CategoryName", subcategoryname);
            sqlcmd.Parameters.AddWithValue("@parentid", parentid);
            int ct_id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return ct_id;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // select brand id from brand name

    public int getBrandId()
    {
        StrQuery = "select brandid from brand where brandname=@brandname ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@brandname", brandname);
            int ct_id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return ct_id;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    // get master product id
    public int getMasterProductid()
    {
        StrQuery = "select productid from products where productname=@productname ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productname", masterProductName);
            int ct_id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            return ct_id;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public void deleteFromTempTable()
    {
        StrQuery = "delete from tmp_productImport where id=@id";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@id", id);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally { objcon.Close(); }
    }

    public string SearchKey { get; set; }
    public DataTable KeyWordAutoPopuletSearchProduct()
    {
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = objcon;
            cmd.CommandText = "sp_SearchKeywordProduct";
            cmd.Parameters.AddWithValue("@SearchKey", SearchKey);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }
    public DataTable KeyWordAutoPopuletSearchMasterProduct()
    {
        DataTable dt = new DataTable();
        try
        {
            objcon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = objcon;
            cmd.CommandText = "sp_SearchKeywordMasterProduct";
            cmd.Parameters.AddWithValue("@SearchKey", SearchKey);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            return dt;
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); objcon.Close(); }
    }

    public DataTable KeyWordAutoPopuletProductName()
    {
        dt = new DataTable();
        StrQuery = "select top 5 isnull(productname,'') as productname from products where ismasterproduct=0 and productname like '%'+convert(varchar(max),@productName)+'%' ";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productName", productName);
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

    public DataTable GetProductNameAndDescriptionByLanguageid()
    {
        dt = new DataTable();

        StrQuery = "select ISNULL(productname,'') as ArabicName,ISNULL(productdescription,'') as ArabicDescription from ";
        StrQuery += " productsLanguage where productid=@productId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@productId", productId);
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

    #region   TEMP ERROR

    public string BoolError { get; set; }
    public int ImportFileId { get; set; }
    public string BoolErrorLine { get; set; }

    public void InsertTempError()
    {
        StrQuery = "insert into TempBoolError (BoolError,ImportFileId,BoolErrorLine) values (@BoolError,@ImportFileId,@BoolErrorLine)";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@BoolError", BoolError);
            sqlcmd.Parameters.AddWithValue("@ImportFileId", ImportFileId);
            sqlcmd.Parameters.AddWithValue("@BoolErrorLine", BoolErrorLine);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objcon.Close();
        }
    }

    public void DeleteTempError()
    {
        StrQuery = "delete from TempBoolError where ImportFileId=@ImportFileId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@ImportFileId", ImportFileId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objcon.Close();
        }
    }

    public DataTable GetAllTempErrorByImportFileId()
    {
        StrQuery = "select isnull(BoolError,'') as BoolError,isnull(ImportFileId,0) as ImportFileId,isnull(BoolErrorLine,'') as BoolErrorLine from TempBoolError where ImportFileId=@ImportFileId";
        try
        {
            objcon.Open();
            SqlCommand sqlcmd = new SqlCommand(StrQuery, objcon);
            sqlcmd.Parameters.AddWithValue("@ImportFileId", ImportFileId);
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

    #endregion

    public DataTable GetAllTheProducts()
    {
        dt = new DataTable();

        StrQuery = "select top 8  isnull(p.productName,'') as productname,isnull(p.productid,0) as productid,";
        StrQuery += " ISNULL((select top 1 imageName from productimages as ip where ip.productid=p.productid),'') as imagename from products as p ";
        StrQuery += " order by productid asc ";

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

}