using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Admin_productExport_Import : System.Web.UI.Page
{
    productManager objproduct = new productManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Generate Product CSV - " + System.Configuration.ConfigurationManager.AppSettings["AminPageTitle"];
        ltrheading.Text = "Generate Product CSV";
        if (!Page.IsPostBack)
        {

        }
    }

    private void GernerateProductCSV()
    {

        DataSet dsProduct = new DataSet();
        try
        {

            dsProduct = objproduct.SelectProductDetailForGenerateCSV();

            if (dsProduct.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                //For i As Integer = 0 To dsProduct.Tables(0).Columns.Count - 1
                //    If i = dsProduct.Tables(0).Columns.Count - 1 Then
                //        sb.Append(dsProduct.Tables(0).Columns(i).ColumnName)
                //    Else
                //        sb.Append(dsProduct.Tables(0).Columns(i).ColumnName + ",")
                //    End If

                //Next
                string strheade = "";
                strheade = "productId,productName,productDescription,sku,barcode,isVarientProduct,isMasterProduct,price,cost,minimumQuantity,inventory,isActive,isFeatured";
                sb.Append(strheade);
                sb.Append(Environment.NewLine);

                for (int j = 0; j <= dsProduct.Tables[0].Rows.Count - 1; j++)
                {
                    for (int k = 0; k <= dsProduct.Tables[0].Columns.Count - 1; k++)
                    {
                        if (k == dsProduct.Tables[0].Columns.Count - 1)
                        {
                            sb.Append(dsProduct.Tables[0].Rows[j][k].ToString());
                        }
                        else
                        {
                            if (dsProduct.Tables[0].Rows[j][k].ToString().Contains(","))
                            {
                                sb.Append("\"" + dsProduct.Tables[0].Rows[j][k].ToString() + "\",");
                            }
                            else
                            {
                                sb.Append(dsProduct.Tables[0].Rows[j][k].ToString() + ",");
                            }
                            //sb.Append(dsProduct.Tables[0].Rows[j][k].ToString() + ",");
                        }

                    }
                    sb.Append(Environment.NewLine);
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "text/csv";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ProductCSV.csv");
                HttpContext.Current.Response.AddHeader("Content-Length", sb.Length.ToString());
                HttpContext.Current.Response.AddHeader("Pragma", "public");
                HttpContext.Current.Response.Write(sb.ToString());
                HttpContext.Current.Response.End();
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { dsProduct.Dispose(); }
    }

    protected void btngeneratecsv_Click(object sender, EventArgs e)
    {
        GernerateProductCSV();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        productManager objproduct = new productManager();
        if (Page.IsValid)
        {

            string extension = Path.GetExtension(fluUploadCsv.FileName).ToLower();
            if (extension == ".csv")
            {
                string filetype = "csv";
                bool isValidFile = false;

                string fileUploadLocation = string.Empty;
                DirectoryInfo dirInfo = null;
                string uploadedFileName = string.Empty;
                string uploadedFileFullPath = string.Empty;

                string PhysicalPath = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ErrorUploadRootPath"].ToString());
                fileUploadLocation = (PhysicalPath);
                dirInfo = new DirectoryInfo(fileUploadLocation);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                uploadedFileName = fluUploadCsv.FileName;
                uploadedFileFullPath = (fileUploadLocation + "\\") + uploadedFileName;
                fluUploadCsv.PostedFile.SaveAs(uploadedFileFullPath);
                //Dim headers As String() = csv.GetFieldHeaders()
                isValidFile = newvalidateCSVandTXTFiles_Automation(uploadedFileFullPath, filetype);
                if (File.Exists(uploadedFileFullPath))
                {
                    File.Delete(uploadedFileFullPath);
                }

            }
            else
            {
                lblError.Text = "Please upload a valid csv file.";
            }
        }
    }

    public bool newvalidateCSVandTXTFiles_Automation(string filepath, string filetype)
    {

        //int totalproductinfeedcount = 0;
        int totalproductinfeedcount = 1;
        int totalerrorcount = 0;
        int totalsucesscount = 0;
        bool _isErrorsInRecord = false;

        FileStream fs = null;
        StreamWriter sr = null;
        StreamReader _fileStream = default(StreamReader);
        string _fileToRead = filepath;
        string _fileForErrorRecords = "";

        #region Create Datatable To Update Records
        DataTable dtalldatatoInsert = new DataTable();
        DataColumn dc = new DataColumn("productId");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("productName");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("productDescription");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("sku");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("barcode");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("isVarientProduct");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("isMasterProduct");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("price");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("cost");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("minimumQuantity");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("inventory");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("isActive");
        dtalldatatoInsert.Columns.Add(dc);
        dc = new DataColumn("isFeatured");
        dtalldatatoInsert.Columns.Add(dc);


        #endregion



        _fileForErrorRecords = Server.MapPath("../" + System.Configuration.ConfigurationManager.AppSettings["ErrorUploadRootPath"].ToString());

        DirectoryInfo dir = new DirectoryInfo(_fileForErrorRecords);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(_fileForErrorRecords);
        }

        _fileStream = File.OpenText(_fileToRead);
        string _readContent = string.Empty;

        //Adjust the top heading row, not include in DataTable
        if (!_fileStream.EndOfStream)
        {
            _readContent = _fileStream.ReadLine();
            string[] _arraytop = null;
            if (filetype.ToLower().Equals("csv"))
            {
                _arraytop = getArrayFromCommaSepratedValues(_readContent);
            }
            else if (filetype.ToLower().Equals("txt"))
            {
                //For TXT
                _arraytop = _readContent.Split(new char[] { ControlChars.Tab });
            }

            //if (_arraytop == null || _arraytop.Length < 20 || _arraytop.Length > 21)
            if (_arraytop == null || _arraytop.Length < 13 || _arraytop.Length > 14)
            {
                totalerrorcount += 1;
                _isErrorsInRecord = true;

                // _fileStream.Close();
                // _fileStream.Dispose();

            }
        }


        string Sqlstring = null;

        string productId = null;
        string productName = null;
        string productDescription = null;
        string sku = null;
        string barcode = null;
        string isVarientProduct = null;
        string isMasterProduct = null;
        string price = null;
        string cost = null;
        string minimumQuantity = null;
        string inventory = null;
        string isActive = null;
        string isFeatured = null;



        //totaltime = 0
        //Dim CurrencySymbol As String

        //CurrencySymbol = currencynew
        // counter for generating xml
        StringBuilder xmlforUpdate = new StringBuilder();

        bool readflag = false;
        string totalRowNumberError = "";

        bool GlobalVarForError = false;


        while (!_fileStream.EndOfStream)
        {
            _isErrorsInRecord = false;


            string errorInRowNumber = "";
            totalproductinfeedcount += 1;
            _readContent = _fileStream.ReadLine();
            _readContent = _readContent.Trim();
            //Check for line blank or not            
            if (_readContent.Length > 0)
            {
                readflag = true;
                string[] _array = null;

                if (filetype.ToLower().Equals("csv"))
                {
                    _array = getArrayFromCommaSepratedValues(_readContent);
                }
                else if (filetype.ToLower().Equals("txt"))
                {
                    //For TXT
                    _array = _readContent.Split(new char[] { ControlChars.Tab });
                }

                if (_array == null)
                {
                    // ***<<Check>>**
                    totalerrorcount += 1;
                    _isErrorsInRecord = true;
                    GlobalVarForError = true;
                    errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                    //sr.WriteLine(_readContent)
                }
                else
                {
                    //if (_array.Length == 20)
                    if (_array.Length == 13)
                    {
                        //Number of cols in csv/txt file
                        if (_array[0] != null)
                        {
                            if (_array[1] != null)
                            {
                                //if (_array[2] != null)
                                //{
                                if (_array[3] != null)
                                {

                                    productId = _array[0].ToString().Trim();
                                    productName = _array[1].ToString().Trim();
                                    productDescription = _array[2].ToString().Trim();
                                    sku = _array[3].ToString().Trim();
                                    barcode = _array[4].ToString().Trim();
                                    isVarientProduct = _array[5].ToString().Trim();
                                    isMasterProduct = _array[6].ToString().Trim();
                                    price = _array[7].ToString().Trim();

                                    //end code by tarak
                                    cost = _array[8].ToString().Trim().Replace(",", "");
                                    minimumQuantity = _array[9].ToString().Trim().Replace(",", "");
                                    inventory = _array[10].ToString().Trim();
                                    isActive = _array[11].ToString().Trim();
                                    isFeatured = _array[12].ToString().Trim();

                                }
                                else
                                {
                                    totalerrorcount += 1;
                                    _isErrorsInRecord = true;
                                    errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                                }
                                //}
                                //else
                                //{
                                //    totalerrorcount += 1;
                                //    _isErrorsInRecord = true;
                                //    errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                                //}
                            }
                            else
                            {
                                totalerrorcount += 1;
                                _isErrorsInRecord = true;
                                errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                            }
                        }
                        else
                        {
                            totalerrorcount += 1;
                            _isErrorsInRecord = true;
                            errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                        }


                        if (_isErrorsInRecord == false)
                        {
                            if (checkValidDatatableData(productId, productName, productDescription, sku, barcode, isVarientProduct, isMasterProduct, price, cost, minimumQuantity, inventory, isActive, isFeatured) == false)
                            {
                                //****************Error Record****************
                                totalerrorcount += 1;
                                _isErrorsInRecord = true;
                                GlobalVarForError = false;
                                errorInRowNumber = totalproductinfeedcount.ToString() + ",";

                                break;
                                //*********************************************
                                //sr.WriteLine(_readContent)
                            }
                            else
                            {
                                totalsucesscount += 1;

                                DataRow dr = dtalldatatoInsert.NewRow();
                                dr["productId"] = productId;
                                dr["productName"] = productName;
                                dr["productDescription"] = productDescription;
                                dr["sku"] = sku;
                                dr["barcode"] = barcode;
                                dr["isVarientProduct"] = isVarientProduct;
                                dr["isMasterProduct"] = isMasterProduct;
                                dr["price"] = price;
                                dr["cost"] = cost;
                                dr["minimumQuantity"] = minimumQuantity;
                                dr["inventory"] = inventory;
                                dr["isActive"] = isActive;
                                dr["isFeatured"] = isFeatured;

                                dtalldatatoInsert.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            GlobalVarForError = true;
                        }

                    }
                    else
                    {
                        //****************Error Record****************
                        totalerrorcount += 1;
                        _isErrorsInRecord = true;
                        errorInRowNumber = totalproductinfeedcount.ToString() + ",";
                        //*********************************************
                        //sr.WriteLine(_readContent)
                    }
                }
            }
            totalRowNumberError += errorInRowNumber.ToString();
        }

        /// Beacause we have to minus header index from success count
        //int totalSuccessCount = Convert.ToInt32(totalproductinfeedcount) - Convert.ToInt32(totalerrorcount);
        int totalSuccessCount = Convert.ToInt32(totalproductinfeedcount) - Convert.ToInt32(totalerrorcount) - 1;

        lbltotalsuccesscount.Text = "Total records are: " + totalproductinfeedcount + " <br/>";
        lbltotalsuccesscount.Text = "Total updated records are: " + totalSuccessCount + " <br/>";
        lblerrorinrow.Text = "Total error records are: " + totalRowNumberError + " <br/>";
        //lbltotalerrorcount

        if (GlobalVarForError == true)
        {
            BindProductsFromLiveServer(dtalldatatoInsert);
            lbltotalerrorcount.Text = "<br>Some records are not updated successfully due to invalid data, Following rows are not updated successfully. <br> Line numbers are: <b> " + totalRowNumberError.ToString().TrimEnd(',') + " </b> ";
        }
        else
        {
            BindProductsFromLiveServer(dtalldatatoInsert);
            lbltotalerrorcount.Text = "Error in line numbers is: <b> " + totalproductinfeedcount + " </b> First solve this error.";
        }
        _fileStream.Close();
        _fileStream.Dispose();

        if (!readflag)
        {
            IOException ioex = new IOException();
            throw ioex;
        }

        return true;
        //#End Region
    }

    #region "get array from comma separated string"
    private string[] getArrayFromCommaSepratedValues(string value)
    {
        try
        {
            int counter = 0;
            //string[] strArray = new string[20];
            string[] strArray = new string[13];
            int indexOfComma = 0;
            int indexOfQuote = 0;
            string strRestOfString = string.Empty;
            string interMediate = string.Empty;
            strRestOfString = value;
            string part = string.Empty;
            while (!string.IsNullOrEmpty(strRestOfString))
            {
                if (strRestOfString.StartsWith("\""))
                {
                    strRestOfString = strRestOfString.Substring(1);
                    indexOfQuote = strRestOfString.IndexOf("\"");
                    part = "\"" + strRestOfString.Substring(0, indexOfQuote + 1);
                    strArray[counter] = part.Trim('\"');
                    counter = counter + 1;
                    if (indexOfQuote != strRestOfString.Length - 1)
                    {
                        strRestOfString = strRestOfString.Substring(indexOfQuote + 2);
                        strRestOfString = strRestOfString.Trim();
                    }
                    else
                    {
                        strRestOfString = "";
                    }
                }
                else
                {
                    indexOfComma = strRestOfString.IndexOf(",");
                    if (indexOfComma == -1)
                    {
                        part = strRestOfString;
                    }
                    else
                    {
                        part = strRestOfString.Substring(0, indexOfComma);
                    }
                    strArray[counter] = part;
                    counter = counter + 1;
                    if (indexOfComma > -1)
                    {
                        strRestOfString = strRestOfString.Substring(indexOfComma + 1);
                        strRestOfString = strRestOfString.Trim();
                    }
                    else
                    {
                        strRestOfString = "";
                    }
                }
            }
            return strArray;
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region Check Validate All Data
    private bool checkValidDatatableData(string productId, string productName, string productDescription, string sku, string barcode, string isVarientProduct, string isMasterProduct,
        string price, string cost, string minimumQuantity, string inventory, string isActive, string isFeatured)
    {

        bool isInsertRow = false;

        #region productId
        if (CommonFunctions.IsValidValue(productId, true, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region sku
        if (CommonFunctions.IsValidValue(sku, false, false, true))
        {
            if (checkLength_DataType(sku, "string", 50, true).Length == 0)
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                return isInsertRow;
            }
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region productName
        if (CommonFunctions.IsValidValue(productName, false, false, true))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region productDescription
        if (checkLength_DataType(productDescription, "string", 4000, false).Length == 0)
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region barcode
        if (barcode != "")
        {
            if (CommonFunctions.IsValidValue(barcode, false, false, false))
            {
                if (checkLength_DataType(barcode, "string", 50, true).Length == 0)
                {
                    isInsertRow = true;
                }
                else
                {
                    isInsertRow = false;
                    return isInsertRow;
                }
            }
            else
            {
                isInsertRow = false;
                return isInsertRow;
            }
        }
        #endregion

        #region isActive
        if (CommonFunctions.IsValidValue(isActive, false, true, false))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region inventory
        if (inventory != "")
        {
            if (CommonFunctions.IsValidValue(inventory, true, false, false))
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                return isInsertRow;
            }
        }
        #endregion

        #region price
        if (price != "")
        {
            if (CommonFunctions.IsValidValue(price, true, true, false))
            {
                if (checkLength_DataType(price, "decimal", 0, true).Length == 0)
                {
                    isInsertRow = true;
                }
                else
                {
                    isInsertRow = false;
                    return isInsertRow;
                }
            }
            else
            {
                isInsertRow = false;
                return isInsertRow;
            }
        }
        #endregion

        #region cost
        if (cost != "")
        {
            if (CommonFunctions.IsValidValue(cost, true, true, false))
            {
                if (checkLength_DataType(price, "decimal", 0, true).Length == 0)
                {
                    isInsertRow = true;
                }
                else
                {
                    isInsertRow = false;
                    return isInsertRow;
                }
            }
            else
            {
                isInsertRow = false;
                return isInsertRow;
            }
        }
        #endregion

        #region minimumQuantity
        if (minimumQuantity != "")
        {
            if (CommonFunctions.IsValidValue(minimumQuantity, true, false, false))
            {
                isInsertRow = true;
            }
            else
            {
                isInsertRow = false;
                return isInsertRow;
            }
        }
        #endregion

        #region isMasterProduct
        if (CommonFunctions.IsValidValue(isMasterProduct, false, false, false))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region isVarientProduct
        if (CommonFunctions.IsValidValue(isVarientProduct, false, false, false))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        #region isFeatured
        if (CommonFunctions.IsValidValue(isFeatured, false, false, false))
        {
            isInsertRow = true;
        }
        else
        {
            isInsertRow = false;
            return isInsertRow;
        }
        #endregion

        return isInsertRow;
    }
    #endregion

    #region "Check Length and Data Type"

    /// <summary>
    /// This function checks for the DataType and Length of the fields value
    /// </summary>
    /// <param name="var">The parameter whose value has to be checked</param>
    /// <param name="dataType">DataType contains the datatype string to chcked against </param>
    /// <param name="maxLen">Parameter var maximum length should be less than or equal to maxLen</param>
    /// <param name="required">Indicates optional or compulsory field</param>
    /// <returns>string</returns>

    private string checkLength_DataType(string var, string dataType, int maxLen, bool required)
    {
        if (dataType == "string")
        {
            if (required)
            {
                if (var.Trim().Length <= 0)
                {
                    return "The value of this column can not be empty";
                }
            }

            if (var.Length > maxLen)
            {
                return "Maximium length of this field should be <=" + maxLen.ToString();
            }
        }
        else if (dataType == "int")
        {
            if (CommonFunctions.IsNumericValue(var))
            {

                if (required)
                {
                    if (var.Trim().Length <= 0)
                    {
                        return "The value of this column can not be empty";
                    }
                }

                try
                {
                    if (var.Trim().Length > 0)
                    {
                        int test = Convert.ToInt32(var);
                    }
                }
                catch
                {
                    return " DataType mismatch. Integer type is required";
                }
            }
        }
        else if (dataType == "decimal")
        {
            if (required)
            {
                if (var.Trim().Length <= 0)
                {
                    return "The value of this column can not be empty";
                }
            }
            try
            {
                if (var.Trim().Length > 0)
                {
                    decimal test = Convert.ToDecimal(var);
                }
            }
            catch
            {
                return " DataType mismatch. Decimal/Fractional type is required";
            }
        }
        return string.Empty;
    }

    #endregion

    public void BindProductsFromLiveServer(DataTable dttblUpdate)
    {
        DataTable dt = new DataTable("tmp_Updatable");
        string str = "";
        try
        {

            dt = dttblUpdate;
            lbltotalerrorcount.Text = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                #region BULK INSERT

                objproduct.DeleteTempProductRecords();

                // Copy the DataTable to SQL Server using SqlBulkCopy
                objproduct.SqlBulkCopyOperation(dt);

                objproduct.InsertUpdateProductFromTemp();
                //Response.Write("<br>SQL BULK COPY Insert comepleted<br>");

                //Response.Write("<br>Data transfer completed<br>");
                //Response.Write("<br>End Time ==> " + DateTime.Now + "<br>");
                //Response.Write("<br>Total " + dt.Rows.Count + " records are import.<br>");

                #endregion
            }
            else
            {
                lbltotalerrorcount.Text += "<tr><td colspan=\"6\"><b style=\"color:red;\">No products available on live server.</b></td></tr>";
            }
        }
        catch (Exception ex) { throw ex; }
        finally { dt.Dispose(); dt = null; dttblUpdate.Dispose(); dttblUpdate = null; }
    }


}