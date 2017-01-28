using System;
using System.IO;
using System.Net;
using System.Data;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.Script.Serialization;

using System.Collections;
using System.Drawing.Drawing2D;

/// <summary>
/// Summary description for CommonFunctions
/// </summary>
/// 
public class CommonFunctions
{
    SqlConnection objCon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);

    public CommonFunctions() { }
    //image operations
    //GLOBAL VARIABLE 
    public enum ORDER_STATUS
    {
        STATUS_NEW = 1,
        STATUS_IN_PROGRESS = 2,
        STATUS_DESPATCHED = 3,
    };
    public enum PAYMENT_STATUS
    {
        PAYMENT_STATUS_PENDING = 0,
        PAYMENT_STATUS_APPROVE = 1,
        PAYMENT_STATUS_DECLINED = 2,
        PAYMENT_STATUS_ERROR = 3,
        PAYMENT_STATUS_HEALD = 4,
    };



    #region "-----------------------------Image Operations------------------------------"
    //fixed size image
    public static Image FixedSizeImage(Image imgPhoto, int Width, int Height)
    {
        Bitmap bmpOut = null;
        try
        {
            Bitmap oBMP = new Bitmap(imgPhoto);
            System.Drawing.Imaging.ImageFormat oFormat = oBMP.RawFormat;
            int NewWidth = 0;
            int NewHeight = 0;

            //*** If the image is smaller than a thumbnail just return it

            if (oBMP.Width <= Width && oBMP.Height <= Height)
            {
                return oBMP;
            }

            double per = 0;
            if (oBMP.Width > Width | oBMP.Height > Height)
            {
                if (oBMP.Width > oBMP.Height)
                {
                    per = (100 * Width) / oBMP.Width;
                    NewWidth = Convert.ToInt32((oBMP.Width * per) / 100);
                    NewHeight = Convert.ToInt32((oBMP.Height * per) / 100);
                }
                else
                {
                    per = (100 * Height) / oBMP.Height;
                    NewWidth = Convert.ToInt32((oBMP.Width * per) / 100);
                    NewHeight = Convert.ToInt32((oBMP.Height * per) / 100);
                }

                if (NewHeight > Height | NewWidth > Width)
                {
                    if (NewWidth > NewHeight)
                    {
                        per = (100 * Width) / NewWidth;
                    }
                    else
                    {
                        per = (100 * Height) / NewHeight;
                    }
                    NewWidth = Convert.ToInt32((NewWidth * per) / 100);
                    NewHeight = Convert.ToInt32((NewHeight * per) / 100);

                }
            }

            bmpOut = new Bitmap(NewWidth, NewHeight);

            Graphics g = Graphics.FromImage(bmpOut);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.FillRectangle(Brushes.White, 0, 0, NewWidth, NewHeight);

            g.DrawImage(oBMP, 0, 0, NewWidth, NewHeight);

            oBMP.Dispose();
        }
        catch
        {
            return null;
        }
        return bmpOut;
    }

    //getNewBounds (ratio/propration)
    //getNewBounds (ratio/propration)
    public static string getNewBounds(int origWidth, int origHeight, int targWidth, int targHeight)
    {
        // To preserve the aspect ratio
        float ratioX = (float)targWidth / (float)origWidth;
        float ratioY = (float)targHeight / (float)origHeight;
        float ratio = Math.Min(ratioX, ratioY);

        // New width and height based on aspect ratio
        int newWidth = (int)(origWidth * ratio);
        int newHeight = (int)(origHeight * ratio);

        //double width2HtRatio = origWidth / origHeight;
        //double Ht2widthRatio = origHeight / origWidth;
        int useWidth = 0;
        int useHeight = 0;
        //if (origWidth < targWidth & origHeight < targHeight)
        //{
        //    useWidth = origWidth;
        //    useHeight = origHeight;
        //}
        //else if (origWidth > targWidth)
        //{
        //    useWidth = targWidth;
        //    useHeight = Convert.ToInt32(useWidth * ratioX);
        //}
        //else if (origHeight > targHeight)
        //{
        //    useHeight = targHeight;
        //    useWidth = Convert.ToInt32(useHeight * ratioY);
        //}
        //else if (origWidth < targWidth)
        //{
        //    useWidth = targWidth;
        //    useHeight = Convert.ToInt32(useWidth * ratioX);
        //}
        //else if (origHeight < targHeight)
        //{
        //    useHeight = targHeight;
        //    useWidth = Convert.ToInt32(useHeight * ratioY);
        //}
        //else
        //{
        useWidth = newWidth;
        useHeight = newHeight;
        //}
        //Return newDimensions
        string NewSize = useWidth + "," + useHeight;
        return NewSize;
    }
    // getNewBounds (ratio/propration)
    public static string oldgetNewBounds(int origWidth, int origHeight, int targWidth, int targHeight)
    {
        double width2HtRatio = origWidth / origHeight;
        double Ht2widthRatio = origHeight / origWidth;
        int useWidth = 0;
        int useHeight = 0;
        if (origWidth < targWidth & origHeight < targHeight)
        {
            useWidth = origWidth;
            useHeight = origHeight;
        }
        else if (origWidth > targWidth)
        {
            useWidth = targWidth;
            useHeight = Convert.ToInt32(useWidth * Ht2widthRatio);
        }
        else if (origHeight > targHeight)
        {
            useHeight = targHeight;
            useWidth = Convert.ToInt32(useHeight * width2HtRatio);
        }
        else if (origWidth < targWidth)
        {
            useWidth = targWidth;
            useHeight = Convert.ToInt32(useWidth * Ht2widthRatio);
        }
        else if (origHeight < targHeight)
        {
            useHeight = targHeight;
            useWidth = Convert.ToInt32(useHeight * width2HtRatio);
        }
        else
        {
            useWidth = targWidth;
            useHeight = targHeight;
        }
        //Return newDimensions
        string NewSize = useWidth + "," + useHeight;
        return NewSize;
    }
    //create Thmbimages
    public static string Thmbimages(string MainPath, string ThmbPath, string Filename, int Passwidth, int Passheight, int FixFlag)
    {
        int width = 0;
        int height = 0;
        IntPtr inp = new IntPtr();
        System.Drawing.Image orginalimg = default(System.Drawing.Image);

        orginalimg = System.Drawing.Image.FromFile(MainPath);
        width = orginalimg.Width;
        height = orginalimg.Height;

        if (width < Passwidth & height < Passheight)
        {
            Passwidth = width;
            Passheight = height;
        }

        if (FixFlag != 0)
        {
            width = Passwidth;
            height = Passheight;
        }
        else
        {
            string Size = null;

            Size = getNewBounds(width, height, Passwidth, Passheight);
            string[] array = Size.Split(',');
            width = Convert.ToInt16(array[0]);
            height = Convert.ToInt16(array[1]);

            string NewSize = "";

            if (width > Passwidth | height > Passheight)
            {
                NewSize = getNewBounds(width, height, Passwidth, Passheight);
            }

            if (NewSize.Length > 0)
            {
                string[] array1 = NewSize.Split(',');
                width = Convert.ToInt16(array1[0]);
                height = Convert.ToInt16(array1[1]);
            }

            if (width <= 0 & height > 0)
            {
                width = Passwidth;
            }
            else if (height <= 0 & width > 0)
            {
                height = Passheight;
            }

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            Bitmap thumbnail = new Bitmap(width, height);

            Graphics graphic = Graphics.FromImage(thumbnail);
            graphic.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.FillRectangle(Brushes.White, 0, 0, width, height);

            graphic.DrawImage(orginalimg, 0, 0, width, height);

            SaveJPGWithCompressionSetting(thumbnail, ThmbPath + Filename, 100);
            memoryStream.Dispose();
            graphic.Dispose();
            orginalimg.Dispose();

            thumbnail.Dispose();
            orginalimg.Dispose();
            return Filename;
        }

        return Filename;
    }

    //create Thmbimages with out text
    public static string Thmbimageswithouttext(string MainPath, string ThmbPath, string Filename, int Passwidth, int Passheight, int FixFlag)
    {
        int width = 0;
        int height = 0;
        IntPtr inp = new IntPtr();
        System.Drawing.Image orginalimg = default(System.Drawing.Image);

        orginalimg = System.Drawing.Image.FromFile(MainPath);
        width = orginalimg.Width;
        height = orginalimg.Height;

        if (width < Passwidth & height < Passheight)
        {
            Passwidth = width;
            Passheight = height;
        }

        if (FixFlag != 0)
        {
            width = Passwidth;
            height = Passheight;
        }
        else
        {
            string Size = null;

            Size = getNewBounds(width, height, Passwidth, Passheight);
            string[] array = Size.Split(',');
            width = Convert.ToInt16(array[0]);
            height = Convert.ToInt16(array[1]);

            string NewSize = "";

            if (width > Passwidth | height > Passheight)
            {
                NewSize = getNewBounds(width, height, Passwidth, Passheight);
            }

            if (NewSize.Length > 0)
            {
                string[] array1 = NewSize.Split(',');
                width = Convert.ToInt16(array1[0]);
                height = Convert.ToInt16(array1[1]);
            }

            if (width <= 0 & height > 0)
            {
                width = Passwidth;
            }
            else if (height <= 0 & width > 0)
            {
                height = Passheight;
            }

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            Bitmap thumbnail = new Bitmap(width, height);

            Graphics graphic = Graphics.FromImage(thumbnail);
            graphic.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.FillRectangle(Brushes.White, 0, 0, width, height);

            graphic.DrawImage(orginalimg, 0, 0, width, height);

            SaveJPGWithCompressionSetting(thumbnail, ThmbPath + Filename, 100);
            memoryStream.Dispose();
            graphic.Dispose();
            orginalimg.Dispose();

            thumbnail.Dispose();
            orginalimg.Dispose();
            return Filename;
        }

        return Filename;
    }

    //Save JPGWith Compression Setting
    public static void SaveJPGWithCompressionSetting(System.Drawing.Image image, string szFileName, long lCompression)
    {
        //On Error GoTo chkErr
        //Dim errOcr As Boolean
        //GC.Collect()
        EncoderParameters eps = new EncoderParameters(1);
        eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, lCompression);
        ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
        image.Save(szFileName, ici, eps);
        return;
        //chkErr: MsgBox("Error: " & Err.Number & " " & Err.Description & vbCrLf & "Choose a different name for file.")
        //errOcr = True
        // ERROR: Not supported in C#: ResumeStatement
    }

    //GetEncoderInfo
    public static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        int j = 0;
        ImageCodecInfo[] encoders = null;
        encoders = ImageCodecInfo.GetImageEncoders();
        for (j = 0; j <= encoders.Length; j++)
        {
            if (encoders[j].MimeType == mimeType)
            {
                return encoders[j];
            }
        }
        return null;
    }

    //calculate size
    public static string CalculateSize(int filesize)
    {
        int count = 0;
        string strresult = string.Empty;

        while (filesize > 1023)
        {
            filesize = filesize / 1024;
            count += 1;
        }

        if (count == 0)
        {
            strresult = filesize + " Bytes";
        }//Bytes
        else if (count == 1)
        {
            strresult = filesize + " KB";
        }//KB
        else if (count == 2)
        {
            strresult = filesize + " MB";
        }//MB

        return strresult;
    }

    public static string convertMMtoPixel(string mm, string density)
    {
        string pixel = "0";
        pixel = Convert.ToString(Convert.ToDecimal(Convert.ToDecimal(density) / Convert.ToDecimal(25.4)) * Convert.ToDecimal(mm));
        return pixel.ToLower();
    }

    #endregion

    //encoding & decoding
    #region "----------------------Encoding/Decoding------------------------------------"
    public static string strAdminCode = "verydealicious";
    //
    //**************************************************************************************************
    // Global Definitions for this site
    //**************************************************************************************************
    public static string strProtocol = "2.23";
    public static string[] arrBase64EncMap = new string[65];
    public static int[] arrBase64DecMap = new int[128];
    public static string[,] arrProducts = new string[4, 3];
    public static string strNewLine = "<P>" + "/n";
    const string BASE_64_MAP_INIT = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    //** Base 64 encoding routine.  Used to ensure the encrypted "crypt" field **
    //** can be safely transmitted over HTTP **

    private string base64Encode(string sData)
    {
        try
        {
            byte[] encData_byte = new byte[sData.Length];

            encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);

            string encodedData = Convert.ToBase64String(encData_byte);

            return encodedData;

        }
        catch (Exception ex)
        {
            throw new Exception("Error in base64Encode" + ex.Message);
        }
    }

    private static string Encrypt(string strText, string strEncrypt)
    {
        byte[] byKey = new byte[20];
        byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputArray = System.Text.Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, dv), CryptoStreamMode.Write);
            cs.Write(inputArray, 0, inputArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception ex) { throw ex; }
    }

    private static string Decrypt(string strText, string strEncrypt)
    {
        byte[] bKey = new byte[20];
        byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        try
        {
            bKey = System.Text.Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = inputByteArray = Convert.FromBase64String(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string encryptString(string strText)
    {
        return Encrypt(strText, "Yes0rN0p@$$");
    }

    public static string decryptString(string str)
    {
        return Decrypt(str, "Yes0rN0p@$$");
    }

    #endregion

    //common utilities
    #region "-------------------------common utilities----------------------------------"
    //convert object to json


    //get date in ddmmyyyy format
    public static string ddmmyyyyDate(string strdate)
    {

        const string pattern = "^(?<Day>\\d{1,2})(\\/)(?<Month>\\d{1,2})(\\/)(?<Year>\\d{4})$";

        System.Text.RegularExpressions.MatchCollection mc = default(System.Text.RegularExpressions.MatchCollection);
        mc = System.Text.RegularExpressions.Regex.Matches(strdate, pattern);

        if (mc.Count > 0)
        {
            string strMonth = mc[0].Groups["Month"].Value.PadLeft(2, '0');
            string strDay = mc[0].Groups["Day"].Value.PadLeft(2, '0');
            string strYear = mc[0].Groups["Year"].Value;

            string strDateNew = strDay + "/" + strMonth + "/" + strYear;
            return strDateNew;
        }
        else
        {
            return strdate;
        }
    }

    //get date in mmddyyyy format
    public static string mmddyyyyDate(string strdate)
    {

        const string pattern = "^(?<Day>\\d{1,2})(\\/)(?<Month>\\d{1,2})(\\/)(?<Year>\\d{4})$";

        System.Text.RegularExpressions.MatchCollection mc = default(System.Text.RegularExpressions.MatchCollection);
        mc = System.Text.RegularExpressions.Regex.Matches(strdate, pattern);

        if (mc.Count > 0)
        {
            string strMonth = mc[0].Groups["Month"].Value.PadLeft(2, '0');
            string strDay = mc[0].Groups["Day"].Value.PadLeft(2, '0');
            string strYear = mc[0].Groups["Year"].Value;

            string strDateNew = strMonth + "/" + strDay + "/" + strYear;
            return strDateNew;
        }
        else
        {
            return strdate;
        }
    }

    //get sanitize input
    public static string SanitizeInput(string input)
    {
        System.Text.RegularExpressions.Regex badCharReplace = new System.Text.RegularExpressions.Regex("([<>\"'%;()&])");
        string goodChars = badCharReplace.Replace(input, "");
        return goodChars;
    }

    // get last identity
    public static int GetLastIdentity(string tablename)
    {
        SqlConnection objCon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        string StrQuery = "";
        StrQuery = " select IDENT_CURRENT(@tablename) ";
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            objCon.Open();
            sqlcmd = new SqlCommand(StrQuery, objCon);
            sqlcmd.Parameters.AddWithValue("@tablename", tablename);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objCon.Close();
        }
    }

    public static string SelectSingleField(string tablename, string selectcolumnname, string idcolumn, string idvalue)
    {
        SqlConnection objCon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        string StrQuery = "";
        StrQuery = " select isnull(" + selectcolumnname + ",'') as " + selectcolumnname + " from " + tablename + " where " + idcolumn + "=" + idvalue;
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            objCon.Open();
            sqlcmd = new SqlCommand(StrQuery, objCon);
            sqlcmd.Parameters.AddWithValue("@tablename", tablename);
            return Convert.ToString(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objCon.Close(); }
    }

    // get last identity pluse one
    public static int GetLastIdentityPlusOne(string tablename)
    {
        SqlConnection objCon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        string strquery = "";
        strquery = " declare @numbercount int select @numbercount = count(*) from " + tablename + " if @numbercount = 0 begin select IDENT_CURRENT(@tablename) end else begin	select (IDENT_CURRENT(@tablename) +1)	end ";
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            objCon.Open();
            sqlcmd = new SqlCommand(strquery, objCon);
            sqlcmd.Parameters.AddWithValue("@tablename", tablename);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objCon.Close();
        }
    }

    //get last sort count
    public static int GetLastSortCount(string tablename, string columnname)
    {
        SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        string strquery = "";
        strquery = " declare @numbercount int select @numbercount = count(*) from " + tablename + "" +
                   " if @numbercount = 0 begin select isnull(MAX(" + columnname + "),1) from " + tablename + "  end  " +
                   " else begin	select (MAX(" + columnname + ")+ 1) from " + tablename + " 	end ";
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            objcon.Open();
            sqlcmd = new SqlCommand(strquery, objcon);
            sqlcmd.Parameters.AddWithValue("@columnname", columnname);
            sqlcmd.Parameters.AddWithValue("@tablename", tablename);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public static int GetLastSortCountOfChild(string tablename, string columnname, int parentid)
    {
        SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        string strquery = "";
        strquery = " declare @numbercount int select @numbercount = count(*) from " + tablename + " where parentid=@parentid " +
                   " if @numbercount = 0 begin select isnull(MAX(" + columnname + "),1) from " + tablename + " where parentid=@parentid  end " +
                   " else begin	select (MAX(" + columnname + ")+ 1) from " + tablename + " where parentid=@parentid	end ";

        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            objcon.Open();
            sqlcmd = new SqlCommand(strquery, objcon);
            sqlcmd.Parameters.AddWithValue("@columnname", columnname);
            sqlcmd.Parameters.AddWithValue("@tablename", tablename);
            sqlcmd.Parameters.AddWithValue("@parentid", parentid);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// fatch any single value
    /// </summary>
    /// <param name="tablename">Name of table</param>
    /// <param name="columnname">Name of column  </param>
    /// <param name="condition">Condition if any </param>
    /// <returns> Object </returns>


    public static object FatchSingleFildData(string tablename, string columnname, string condition)
    {
        SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            objcon.Open();
            sqlcmd = new SqlCommand("FatchSingleFildData", objcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@columnname", columnname);
            sqlcmd.Parameters.AddWithValue("@tablename", tablename);
            sqlcmd.Parameters.AddWithValue("@condition", condition);
            return sqlcmd.ExecuteScalar();
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }
    /// <summary>
    /// Retuen Multiple column with datatable 
    /// </summary>
    /// <param name="tablename">Name of Table </param>
    /// <param name="columnnames">columnname seperate by comma </param>
    /// <param name="condition"> condition if any</param>
    /// <returns>Data Tables</returns>
    public static DataTable FatchMultipleFilds(string tablename, string columnnames, string condition)
    {
        SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            DataTable dt = new DataTable();
            objcon.Open();
            sqlcmd = new SqlCommand("FatchMultipleFilds", objcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@columnnames", columnnames);
            sqlcmd.Parameters.AddWithValue("@tablename", tablename);
            sqlcmd.Parameters.AddWithValue("@condition", condition);
            SqlDataAdapter sqladp = new SqlDataAdapter(sqlcmd);
            sqladp.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
        finally { objcon.Close(); }
    }
    //Check directory exist
    public static void Checkdirectoryexist(string directorypath)
    {
        if (!Directory.Exists(directorypath))
        {
            Directory.CreateDirectory(directorypath);
        }
    }

    public static bool IsNumericValue(string strTextEntry)
    {
        if (strTextEntry != null)
        {
            System.Text.RegularExpressions.Regex objNotWholePattern = new System.Text.RegularExpressions.Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strTextEntry) && (strTextEntry != "");
        }
        else
        {
            return false;
        }
    }

    public static bool IsBooleanValue(string strTextEntry)
    {
        if (strTextEntry != null)
        {
            System.Text.RegularExpressions.Regex objNotWholePattern = new System.Text.RegularExpressions.Regex("[^([Tt][Rr][Uu][Ee]|[Ff][Aa][Ll][Ss][Ee])$]");
            return !objNotWholePattern.IsMatch(strTextEntry) && (strTextEntry != "");
        }
        else
        {
            return false;
        }
    }

    //Delete file
    public static void DeleteFile(string strPath)
    {
        if (File.Exists(strPath))
        {
            File.Delete(strPath);
        }
    }

    //Get file contents
    public static string GetFileContents(string filename)
    {
        string filecontent = string.Empty;
        System.IO.StreamReader objStreamReader;
        objStreamReader = System.IO.File.OpenText(filename);
        filecontent = objStreamReader.ReadToEnd();
        objStreamReader.Close();
        return filecontent;
    }

    //Copy files from folder to another
    public static void copyFileAndFolder(string SourceDirectory, string DestinationDirectory)
    {
        try
        {
            //cretae directory if not exists
            createDirectory(DestinationDirectory);

            string[] f = Directory.GetFiles(SourceDirectory);
            string[] strSplit = null;
            string strFile = string.Empty;
            for (int i = 0; i <= f.GetUpperBound(0); i++) //UBound(f)
            {
                strSplit = f[i].Split(Convert.ToChar("\\"));
                strFile = strSplit[strSplit.Length - 1];

                //delete if file exists
                DeleteFile(DestinationDirectory + "\\" + strFile);

                //copy file
                File.Copy(f[i], DestinationDirectory + "\\" + strFile);
            }
        }
        catch (Exception ex) { throw ex; }
    }

    //Copy Directory
    public static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
    {
        bool ret = false;
        try
        {
            SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
            DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

            if (Directory.Exists(SourcePath))
            {
                if (Directory.Exists(DestinationPath) == false)
                    Directory.CreateDirectory(DestinationPath);

                foreach (string fls in Directory.GetFiles(SourcePath))
                {
                    FileInfo flinfo = new FileInfo(fls);
                    flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                }
                foreach (string drs in Directory.GetDirectories(SourcePath))
                {
                    DirectoryInfo drinfo = new DirectoryInfo(drs);
                    if (drinfo.Name != ".svn")
                    {
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
            }
            ret = true;
        }
        catch (Exception ex)
        {
            string strex = ex.ToString();
            ret = false;
        }
        return ret;
    }

    //Create Directory
    public static void createDirectory(string path)
    {
        DirectoryInfo Dir = new DirectoryInfo(path);
        if (!Dir.Exists) { Directory.CreateDirectory(path); }
    }

    //Copy Single File
    public static void copySingleFile(string SourceFile, string DestinationFile)
    {
        if (File.Exists(SourceFile))
        {
            //delete if file exists
            DeleteFile(DestinationFile);
            //copy file
            File.Copy(SourceFile, DestinationFile);
        }
    }




    //distroy cookie
    public static void DestroyCookie(string CookieName)
    {
        System.Web.HttpCookie CookieObj = System.Web.HttpContext.Current.Request.Cookies[CookieName];

        if (CookieObj != null)
        {
            CookieObj.Expires = DateTime.Now.AddDays(-1);
            System.Web.HttpContext.Current.Response.Cookies.Add(CookieObj);
        }

    }

    //getcoupon code
    public static string GetCouponNo()
    {
        Guid g = Guid.NewGuid();
        string random = g.ToString();
        //random = random.Replace("-", "");
        return random.Substring(0, 11).ToUpper();
    }

    //get count down
    public static string ShowCountDown(TimeSpan time1)
    {
        if (time1.Days != 0 && time1.Hours == 0 && time1.Minutes == 0 && time1.Seconds == 0)
        {
            return "<b>" + time1.Days + "</b> days";
        }
        else if (time1.Days != 0 && time1.Hours != 0 && time1.Minutes == 0 && time1.Seconds == 0)
        {
            return "<b>" + time1.Days + "</b> days <br /><b>" + time1.Hours + "</b> hours ";
        }
        else if (time1.Days != 0 && time1.Hours != 0 && time1.Minutes != 0 && time1.Seconds == 0)
        {
            return "<b>" + time1.Days + "</b> days <br /><b>" + time1.Hours + "</b> hours <br /><b>" + time1.Minutes + "</b> minutes";
        }
        else if (time1.Days == 0 && time1.Hours != 0 && time1.Minutes == 0 && time1.Seconds == 0)
        {
            return "<b>" + time1.Hours + "</b> hours";
        }
        else if (time1.Days == 0 && time1.Hours == 0 && time1.Minutes != 0 && time1.Seconds == 0)
        {
            return "<b>" + time1.Minutes + "</b> minutes <br /><b>" + time1.Seconds + "</b> seconds";
        }
        else if (time1.Days != 0 && time1.Hours == 0 && time1.Minutes != 0 && time1.Seconds == 0)
        {
            return "<b>" + time1.Minutes + "</b> minutes";
        }
        else if (time1.Days != 0 && time1.Hours != 0 && time1.Minutes != 0 && time1.Seconds == 0)
        {
            return "<b>" + time1.Seconds + "</b> seconds";
        }
        else if (time1.Days != 0 && time1.Hours == 0 && time1.Minutes == 0 && time1.Seconds != 0)
        {
            return "<b>" + time1.Days + "</b> days <br /><b>" + time1.Seconds + "</b> seconds";
        }
        else if (time1.Days != 0 && time1.Hours != 0 && time1.Minutes == 0 && time1.Seconds != 0)
        {
            return "<b>" + time1.Days + "</b> days <br /><b>" + time1.Hours + "</b> hours <br /><b>" + time1.Seconds + "</b> seconds";
        }
        else
        {
            return "<b>" + time1.Days + "</b> days <br /><b>" + time1.Hours + "</b> hours <br /><b>" + time1.Minutes + "</b> minutes <br /><b>" + time1.Seconds + "</b> seconds";
        }
    }

    //Get Previous Page with querystring
    public static string GetPreviousPageWithQueryString()
    {
        string PreviousPageName = "";
        if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
        {
            PreviousPageName = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
        }
        return PreviousPageName;
    }

    //Get Previous Page Name
    public static string GetPreviousPageName()
    {
        string PreviousPageName = "";
        if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
        {
            PreviousPageName = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            String strurl = PreviousPageName;
            String[] strvalue;
            strvalue = strurl.Split(Convert.ToChar("/"));
            PreviousPageName = strvalue[strvalue.Length - 1];
        }
        return PreviousPageName;
    }

    //convert hex to int
    public static int HexToInt(string hexString)
    {
        return int.Parse(hexString, System.Globalization.NumberStyles.HexNumber, null);
    }

    //convert int to hex
    public static string IntToHex(int intNumber)
    {
        return (intNumber).ToString("x");
    }
    public static double ConvertToInches(Double feet)
    {
        return feet * 12;
    }

    public static double ConvertToFeet(Double inches)
    {
        return inches / 12;
    }

    public static string getHeightWidth(string strparam)
    {
        string ft = "";
        string inch = "";
        string result = "";
        int inchvalue = 0;
        double dblInput = ConvertToFeet(Convert.ToDouble(strparam));
        string[] strsize = dblInput.ToString().Split(Convert.ToChar("."));
        string[] strinchvalue = strparam.Split('.');
        if (strinchvalue.Length > 0)
        {
            inchvalue = Convert.ToInt32(strinchvalue[0]);
        }
        if (strsize.Length > 1)
        {
            ft = String.Format("{0:0.00}", strsize[0]) + "'";
            if (strsize[0] != "0")
            {
                inch = Convert.ToString(inchvalue - (Convert.ToInt32(strsize[0]) * 12)) + "''";
            }
            else
            {
                inch = Convert.ToString(inchvalue) + "''";
            }
        }
        else
        {
            ft = String.Format("{0:0.00}", strsize[0]) + "'";
        }

        result = ft + " " + inch;

        return result;
    }
    // write file to text
    public static void writefile(string content)
    {

        string filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\resources\\ErrorLogs\\ship.txt";
        StreamWriter w;
        w = File.AppendText(filePath);
        w.WriteLine(content);
        w.Flush();
        w.Close();
    }

    //generate random text
    public static string GenerateRandomText()
    {
        Random _rand = new Random();
        string _randomTextChars = "abcdefghijklmnopqrstuvwxyz012346789ACDEFGHJKLNPQRTUVXYZ";
        System.Text.StringBuilder sb = new System.Text.StringBuilder(25);
        int maxLength = _randomTextChars.Length;
        for (int n = 0; n <= 30; n++)
        {
            sb.Append(_randomTextChars.Substring(_rand.Next(maxLength), 1));
        }
        return sb.ToString();
    }
    #endregion

    //paging
    #region "-----------------------------Paging Functions--------------------------------"
    public static string GetPagingDone(int thisPageNo, int totalCount, int pageSize, string pageName, string extraQstringToAdd)
    {
        int pageno = 0;
        int start = 0;
        int loop = totalCount / pageSize;
        int remainder = totalCount % pageSize;
        StringBuilder strB = new StringBuilder("<br /><b><font color=\"green\">Page:</font> ", 500);
        for (int i = 0; i < loop; i++)
        {
            pageno = i + 1;
            if (pageno.Equals(thisPageNo))
                strB.Append(pageno + "&nbsp;| ");
            else
                strB.Append("<a href=" + System.Configuration.ConfigurationManager.AppSettings["serverurl"] + pageName + "?start=" + start + "&page=" + pageno + extraQstringToAdd + "\" title=\"Go to Page " + pageno + "\">" + pageno + "</a> | ");
            start += pageSize;
        }

        if (remainder > 0)
        {
            pageno++;
            if (pageno.Equals(thisPageNo))
                strB.Append("<b>" + pageno + "&nbsp;</b>| ");
            else
                strB.Append("<a href=" + System.Configuration.ConfigurationManager.AppSettings["serverurl"] + pageName + "?start=" + start + "&page=" + pageno + extraQstringToAdd + "\" title=\"Go to Page " + pageno + "\">" + pageno + "</a> | ");
        }
        return strB.ToString() + "</b></span>";
    }

    //paging
    public static string Dynamicpaging(int pageno, int totalpages, string pageName, string extraQstringToAdd)
    {
        StringBuilder sbPaging = new StringBuilder();
        string temp = "";
        sbPaging.Append("<ul>");
        if (pageno > 1 && pageno <= totalpages)
        {
            sbPaging.Append("<li><a href='" + System.Configuration.ConfigurationManager.AppSettings["serverurl"] + pageName + "?page=" + (pageno - 1) + extraQstringToAdd + "'  >«« Previous</a></li>");
        }
        for (int i = 0; i < totalpages; i++)
        {
            if (pageno == (i + 1))
                temp = "<li><a class='cur' href='" + System.Configuration.ConfigurationManager.AppSettings["serverurl"] + pageName + "?page=" + (i + 1) + extraQstringToAdd + "' >" + Convert.ToString(i + 1) + "</a></li>";
            else
                temp = "<li><a href='" + System.Configuration.ConfigurationManager.AppSettings["serverurl"] + pageName + "?page=" + (i + 1) + extraQstringToAdd + "' >" + Convert.ToString(i + 1) + "</a></li>";

            if (i >= pageno - 6 && i <= pageno + 5)
                sbPaging.Append(temp);
            else
                sbPaging.Append("");
        }
        if (pageno >= 1 && pageno != totalpages)
        {
            sbPaging.Append("<li><a href='" + System.Configuration.ConfigurationManager.AppSettings["serverurl"] + pageName + "?page=" + (pageno + 1) + extraQstringToAdd + "'  >Next »»</a></li>");
        }
        sbPaging.Append("</ul>");
        return sbPaging.ToString();
    }

    //paging
    public static string DynamicPagingJQuery(int pageno, int totalpages, string pageName, string extraQstringToAdd)
    {
        StringBuilder sbPaging = new StringBuilder();
        string temp = "";
        sbPaging.Append("<ul>");
        if (pageno > 1 && pageno <= totalpages)
        {
            sbPaging.Append("<li><a href='javascript:void(0);' onclick=javascript:loadMurals('" + pageName + "','divcontainer','" + Convert.ToInt32(pageno - 1) + "');>« Previous</a></li>");
        }
        for (int i = 0; i < totalpages; i++)
        {
            if (pageno == (i + 1))
                temp = "<li><a class='cur' href='javascript:void(0);' onclick=javascript:loadMurals('" + pageName + "','divcontainer','" + Convert.ToString(i + 1) + "');>" + Convert.ToString(i + 1) + "</a></li>";
            else
                temp = "<li><a href='javascript:void(0);' onclick=javascript:loadMurals('" + pageName + "','divcontainer','" + Convert.ToString(i + 1) + "');>" + Convert.ToString(i + 1) + "</a></li>";

            if (i >= pageno - 6 && i <= pageno + 5)
                sbPaging.Append(temp);
            else
                sbPaging.Append("");
        }
        if (pageno >= 1 && pageno != totalpages)
        {
            sbPaging.Append("<li><a href='javascript:void(0);' onclick=javascript:loadMurals('" + pageName + "','divcontainer','" + Convert.ToInt32(pageno + 1) + "');>Next »</a></li>");
        }
        sbPaging.Append("</ul>");
        return sbPaging.ToString();
    }

    //dynamic paging
    public static string FrontPaggingURL(int Count, int PageSize, string Urlpage, string strQuerystring, int Page)
    {
        int i = 0;
        StringBuilder strPage = new StringBuilder();
        double tempCount = Count;
        double TotalPage = tempCount / PageSize;
        TotalPage = Convert.ToDouble(TotalPage.ToString("0.00"));
        string[] TotalPageArry = TotalPage.ToString().Split('.');
        if (TotalPageArry.Length == 2)
        {
            if (Convert.ToInt32(TotalPageArry[1]) > 0)
            {
                TotalPage = Convert.ToInt32(TotalPageArry[0]) + 1;
            }
            else
            {
                TotalPage = TotalPage;
            }
        }
        else
        {
            TotalPage = TotalPage;
        }
        if (Page == 0)
        {
            Page = 1;
        }

        int Last = 0;
        int First = 0;

        if (Page > 5)
        {
            if (TotalPage > Page + 5)
            {
                Last = Page + 3;
                First = Page - 5;
            }
            else
            {
                Last = Convert.ToInt32(TotalPage) - 1;
                if (Page == TotalPage)
                {
                    First = Page - 5;
                }
                else if (Page == TotalPage - 1)
                {
                    First = Page - 4;
                }
                else if (Page == TotalPage - 2)
                {
                    First = Page - 3;
                }
                else if (Page == TotalPage - 3)
                {
                    First = Page - 2;
                }
                else if (Page == TotalPage - 4)
                {
                    First = Page - 1;
                }
                else
                {
                    First = Page - 5;
                }
            }
        }
        else if (TotalPage > 5)
        {
            First = 0;
            Last = 5;
        }
        else
        {
            First = 0;
            Last = Convert.ToInt32(TotalPage) - 1;
        }
        string url = "";
        strPage.Append("<ul>");
        if (TotalPage > 1)
        {
            if (Page == 1)
            {
                // strPage.Append("<li><a class=\"prev\" href=\"javascript:void(0);\">Previous</a></li>");
            }
            else
            {
                strPage.Append("<li><a class=\"prev\" href=\"javascript:void(0);\" onclick=\"" + Urlpage.ToString().Replace("#P#", (Page - 1).ToString()) + "\">Previous</a></li>");
            }
        }
        for (i = First; i <= Last; i++)
        {
            if (i == Page - 1)
            {
                if (i == Last)
                {
                    strPage.Append("<li><a href=\"javascript:void(0);\" class=\"active\">" + (i + 1) + "</a></li>");
                }
                else
                {
                    strPage.Append("<li><a  href=\"javascript:void(0);\" class=\"active\">" + (i + 1) + "</a></li>");
                }
            }
            else
            {
                if (i == Last)
                {
                    strPage.Append("<li><a href=\"javascript:void(0);\" onclick=\"" + Urlpage.ToString().Replace("#P#", (i + 1).ToString()) + "\" style=\"border:none;\">" + (i + 1) + "</a></li>");
                }
                else
                {
                    strPage.Append("<li><a href=\"javascript:void(0);\" onclick=\"" + Urlpage.ToString().Replace("#P#", (i + 1).ToString()) + "\">" + (i + 1) + "</a></li>");
                }
            }

            if (Last != i)
            {
            }
        }

        if (TotalPage > 1)
        {
            if (Page == 1)
            {
                if (Page < TotalPage)
                {
                    strPage.Append("<li><a class=\"next\" href=\"javascript:void(0);\" onclick=\"" + Urlpage.ToString().Replace("#P#", (Page + 1).ToString()) + "\">Next</a></li>");
                }
                else
                {
                    strPage.Append("<li><a class=\"next\" href=\"javascript:void(0);\">Next</a></li>");
                }
            }
            else if (Page < TotalPage)
            {
                strPage.Append("<li><a class=\"next\" href=\"javascript:void(0);\" onclick=\"" + Urlpage.ToString().Replace("#P#", (Page + 1).ToString()) + "\">Next</a></li>");
            }
            else
            {
                // strPage.Append("<li><a class=\"next\" href=\"javascript:void(0);\">Next</a></li>");
            }
        }
        strPage.Append("</ul>");
        return strPage.ToString();
    }

    #endregion

    //mail functions
    #region "-----------------------------Mail Functions--------------------------------"

    public static void SendMailWithAttachment(string from, string Recipients, string RecipientsCC, string mailbody, string Subject, string user, string password, string smtpServer, string attachment)
    {
        smtpServer = "mail.webtechsystem.com";
        user = "mark@webtechsystem.com";
        password = "5UGjGAU77iaCKGIE@";

        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        message.To.Add(Recipients);
        //message.Attachments.Add(new System.Net.Mail.Attachment("FileName"));
        if (RecipientsCC != "")
            message.CC.Add(RecipientsCC);
        //else
        //    message.CC.Add("hiren.daraji@webtechsystem.com");
        message.Subject = Subject;
        message.From = new System.Net.Mail.MailAddress(from);
        message.Body = mailbody;
        System.Net.Mail.Attachment aa = new System.Net.Mail.Attachment(attachment);
        message.Attachments.Add(aa);
        message.IsBodyHtml = true;
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpServer, 25);
        smtp.Credentials = new NetworkCredential(user, password);
        //smtp.Send(message);
    }

   

    public static void SendMail(string from, string Recipients, string RecipientsCC, string RecipientsBCC, string mailbody, string Subject, string user, string password, string smtpServer)
    {

        smtpServer = "mail.webtechsystem.com";
        user = "mark@webtechsystem.com";
        password = "m@Rak!iL6@";
        // password = "5UGjGAU77iaCKGIE@";
        //smtpServer = "mail.webtechsystem.com";
        //user = "mail@mgtdemo.co.uk";
        //password = "Mail@007";
        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        message.To.Add(Recipients);
        //message.Attachments.Add(new System.Net.Mail.Attachment("FileName"));
        if (RecipientsCC != "")
        {
            message.CC.Add(RecipientsCC);
        }
        else
        {
        }

        if (RecipientsBCC != "")
        {
            message.Bcc.Add(RecipientsBCC);
        }
        else
        {
        }

        message.Subject = Subject;
        message.From = new System.Net.Mail.MailAddress(from);
        message.Body = mailbody;
        message.IsBodyHtml = true;

        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpServer, 25);
        smtp.Credentials = new NetworkCredential(user, password);
        smtp.Send(message);


    }

    public static string GenerateUniqueCode()
    {
        int minsize = 10;
        int maxsize = 25;
        int size = 0;
        char[] chars = new char[62];
        string str = "";
        str = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        chars = str.ToCharArray();
        size = maxsize;
        byte[] data = new byte[1];
        RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
        crypto.GetNonZeroBytes(data);
        size = maxsize;
        data = new byte[(size)];
        crypto.GetNonZeroBytes(data);
        StringBuilder result = new StringBuilder(size);
        foreach (byte b in data)
        {
            result.Append(chars[(b % (chars.Length - 1))]);
        }
        return result.ToString();
    }

    //getcoupon code
    //public static string GetCouponNo()
    //{
    //    Guid g = Guid.NewGuid();
    //    string random = g.ToString();
    //    //random = random.Replace("-", "");
    //    return random.Substring(0, 4).ToUpper();
    //}

    public static void SendMail2(string from, string Recipients, string RecipientsCC, string mailbody, string Subject, string user, string password, string smtpServer)
    {
        smtpServer = "mail.webtechsystem.com";
        user = "mark@webtechsystem.com";
        password = "5UGjGAU77iaCKGIE@"; // //5UGjGAU77iaCKGIE@

        // user = "sudhansu@webtechsystem.com";
        // password = "Windows@88";
        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        message.To.Add(Recipients);
        //message.Attachments.Add(new System.Net.Mail.Attachment("FileName"));
        if (RecipientsCC != "")
            message.CC.Add(RecipientsCC);
        //else
        //    message.CC.Add("hiren.daraji@webtechsystem.com");
        message.Subject = Subject;
        message.From = new System.Net.Mail.MailAddress(from);
        message.Body = mailbody;
        message.IsBodyHtml = true;
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpServer, 25);
        smtp.Credentials = new NetworkCredential(user, password);
        smtp.Send(message);
    }

    public static void SendMailWithAttachment2(string from, string Recipients, string RecipientsCC, string mailbody, string Subject, string user, string password, string smtpServer, byte[] attachment)
    {
        smtpServer = "mail.webtechsystem.com";
        user = "mark@webtechsystem.com";
        password = "5UGjGAU77iaCKGIE@";

        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        message.To.Add(Recipients);
        //message.Attachments.Add(new System.Net.Mail.Attachment("FileName"));
        if (RecipientsCC != "")
            message.CC.Add(RecipientsCC);
        //else
        //    message.CC.Add("hiren.daraji@webtechsystem.com");
        message.Subject = Subject;
        message.From = new System.Net.Mail.MailAddress(from);
        message.Body = mailbody;
        message.Attachments.Add(new Attachment(new MemoryStream(attachment), "Invoice.pdf"));
        message.IsBodyHtml = true;
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpServer, 25);
        smtp.Credentials = new NetworkCredential(user, password);
        smtp.Send(message);
    }


    #endregion
    //check for abuse word
    public static string checkprofanity(string strmsg)
    {

        ArrayList restrictedword = new ArrayList();
        restrictedword.Add("ass");
        restrictedword.Add("ass lick");
        restrictedword.Add("asses");
        restrictedword.Add("asshole");
        restrictedword.Add("assholes");
        restrictedword.Add("asskisser");
        restrictedword.Add("asswipe");
        restrictedword.Add("balls");
        restrictedword.Add("bastard");
        restrictedword.Add("beastial");
        restrictedword.Add("beastiality");
        restrictedword.Add("beastility");
        restrictedword.Add("beaver");
        restrictedword.Add("belly whacker");
        restrictedword.Add("bestial");
        restrictedword.Add("bestiality");
        restrictedword.Add("bitch");
        restrictedword.Add("bitcher");
        restrictedword.Add("bitchers");
        restrictedword.Add("bitches");
        restrictedword.Add("bitchin");
        restrictedword.Add("bitching");
        restrictedword.Add("blow job");
        restrictedword.Add("blowjob");
        restrictedword.Add("blowjobs");
        restrictedword.Add("bonehead");
        restrictedword.Add("boner");
        restrictedword.Add("brown eye");
        restrictedword.Add("browneye");
        restrictedword.Add("browntown");
        restrictedword.Add("bucket cunt");
        restrictedword.Add("bull shit");
        restrictedword.Add("bullshit");
        restrictedword.Add("bum");
        restrictedword.Add("bung hole");
        restrictedword.Add("butch");
        restrictedword.Add("butt");
        restrictedword.Add("butt breath");
        restrictedword.Add("butt fucker");
        restrictedword.Add("butt hair");
        restrictedword.Add("buttface");
        restrictedword.Add("buttfuck");
        restrictedword.Add("buttfucker");
        restrictedword.Add("butthead");
        restrictedword.Add("butthole");
        restrictedword.Add("buttpicker");
        restrictedword.Add("chink");
        restrictedword.Add("circle jerk");
        restrictedword.Add("clam");
        restrictedword.Add("clit");
        restrictedword.Add("cobia");
        restrictedword.Add("cock");
        restrictedword.Add("cocks");
        restrictedword.Add("cocksuck");
        restrictedword.Add("cocksucked");
        restrictedword.Add("cocksucker");
        restrictedword.Add("cocksucking");
        restrictedword.Add("cocksucks");
        restrictedword.Add("cooter");
        restrictedword.Add("crap");
        restrictedword.Add("cum");
        restrictedword.Add("cummer");
        restrictedword.Add("cumming");
        restrictedword.Add("cums");
        restrictedword.Add("cumshot");
        restrictedword.Add("cunilingus");
        restrictedword.Add("cunillingus");
        restrictedword.Add("cunnilingus");
        restrictedword.Add("cunt");
        restrictedword.Add("cuntlick");
        restrictedword.Add("cuntlicker");
        restrictedword.Add("cuntlicking");
        restrictedword.Add("cunts");
        restrictedword.Add("cyberfuc");
        restrictedword.Add("cyberfuck");
        restrictedword.Add("cyberfucked");
        restrictedword.Add("cyberfucker");
        restrictedword.Add("cyberfuckers");
        restrictedword.Add("cyberfucking");
        restrictedword.Add("damn");
        restrictedword.Add("dick");
        restrictedword.Add("dike");
        restrictedword.Add("dildo");
        restrictedword.Add("dildos");
        restrictedword.Add("dink");
        restrictedword.Add("dinks");
        restrictedword.Add("dipshit");
        restrictedword.Add("dong");
        restrictedword.Add("douche bag");
        restrictedword.Add("dumbass");
        restrictedword.Add("dyke");
        restrictedword.Add("ejaculate");
        restrictedword.Add("ejaculated");
        restrictedword.Add("ejaculates");
        restrictedword.Add("ejaculating");
        restrictedword.Add("ejaculatings");
        restrictedword.Add("ejaculation");
        restrictedword.Add("fag");
        restrictedword.Add("fagget");
        restrictedword.Add("fagging");
        restrictedword.Add("faggit");
        restrictedword.Add("faggot");
        restrictedword.Add("faggs");
        restrictedword.Add("fagot");
        restrictedword.Add("fagots");
        restrictedword.Add("fags");
        restrictedword.Add("fart");
        restrictedword.Add("farted");
        restrictedword.Add("farting");
        restrictedword.Add("fartings");
        restrictedword.Add("farts");
        restrictedword.Add("farty");
        restrictedword.Add("fatass");
        restrictedword.Add("fatso");
        restrictedword.Add("felatio");
        restrictedword.Add("fellatio");
        restrictedword.Add("fingerfuck");
        restrictedword.Add("fingerfucked");
        restrictedword.Add("fingerfucker");
        restrictedword.Add("fingerfuckers");
        restrictedword.Add("fingerfucking");
        restrictedword.Add("fingerfucks");
        restrictedword.Add("fistfuck");
        restrictedword.Add("fistfucked");
        restrictedword.Add("fistfucker");
        restrictedword.Add("fistfuckers");
        restrictedword.Add("fistfucking");
        restrictedword.Add("fistfuckings");
        restrictedword.Add("fistfucks");
        restrictedword.Add("fuck");
        restrictedword.Add("fucked");
        restrictedword.Add("fucker");
        restrictedword.Add("fuckers");
        restrictedword.Add("fuckin");
        restrictedword.Add("fucking");
        restrictedword.Add("fuckings");
        restrictedword.Add("fuckme");
        restrictedword.Add("fucks");
        restrictedword.Add("fuk");
        restrictedword.Add("fuks");
        restrictedword.Add("furburger");
        restrictedword.Add("gangbang");
        restrictedword.Add("gangbanged");
        restrictedword.Add("gangbangs");
        restrictedword.Add("gaysex");
        restrictedword.Add("gazongers");
        restrictedword.Add("goddamn");
        restrictedword.Add("gonads");
        restrictedword.Add("gook");
        restrictedword.Add("guinne");
        restrictedword.Add("hard on");
        restrictedword.Add("hardcoresex");
        restrictedword.Add("homo");
        restrictedword.Add("hooker");
        restrictedword.Add("horniest");
        restrictedword.Add("horny");
        restrictedword.Add("hotsex");
        restrictedword.Add("hussy");
        restrictedword.Add("jack off");
        restrictedword.Add("jackass");
        restrictedword.Add("jacking off");
        restrictedword.Add("jackoff");
        restrictedword.Add("jack-off");
        restrictedword.Add("jap");
        restrictedword.Add("jerk");
        restrictedword.Add("jerk-off");
        restrictedword.Add("jism");
        restrictedword.Add("jiz");
        restrictedword.Add("jizm");
        restrictedword.Add("jizz");
        restrictedword.Add("kike");
        restrictedword.Add("kock");
        restrictedword.Add("kondum");
        restrictedword.Add("kondums");
        restrictedword.Add("kraut");
        restrictedword.Add("kum");
        restrictedword.Add("kummer");
        restrictedword.Add("kumming");
        restrictedword.Add("kums");
        restrictedword.Add("kunilingus");
        restrictedword.Add("lesbian");
        restrictedword.Add("lesbo");
        restrictedword.Add("merde");
        restrictedword.Add("mick");
        restrictedword.Add("mothafuck");
        restrictedword.Add("mothafucka");
        restrictedword.Add("mothafuckas");
        restrictedword.Add("mothafuckaz");
        restrictedword.Add("mothafucked");
        restrictedword.Add("mothafucker");
        restrictedword.Add("mothafuckers");
        restrictedword.Add("mothafuckin");
        restrictedword.Add("mothafucking");
        restrictedword.Add("mothafuckings");
        restrictedword.Add("mothafucks");
        restrictedword.Add("motherfuck");
        restrictedword.Add("motherfucked");
        restrictedword.Add("motherfucker");
        restrictedword.Add("motherfuckers");
        restrictedword.Add("motherfuckin");
        restrictedword.Add("motherfucking");
        restrictedword.Add("motherfuckings");
        restrictedword.Add("motherfucks");
        restrictedword.Add("muff");
        restrictedword.Add("nigger");
        restrictedword.Add("niggers");
        restrictedword.Add("orgasim");
        restrictedword.Add("orgasims");
        restrictedword.Add("orgasm");
        restrictedword.Add("orgasms");
        restrictedword.Add("pecker");
        restrictedword.Add("penis");
        restrictedword.Add("phonesex");
        restrictedword.Add("phuk");
        restrictedword.Add("phuked");
        restrictedword.Add("phuking");
        restrictedword.Add("phukked");
        restrictedword.Add("phukking");
        restrictedword.Add("phuks");
        restrictedword.Add("phuq");
        restrictedword.Add("pimp");
        restrictedword.Add("piss");
        restrictedword.Add("pissed");
        restrictedword.Add("pissrr");
        restrictedword.Add("pissers");
        restrictedword.Add("pisses");
        restrictedword.Add("pissin");
        restrictedword.Add("pissing");
        restrictedword.Add("pissoff");
        restrictedword.Add("prick");
        restrictedword.Add("pricks");
        restrictedword.Add("pussies");
        restrictedword.Add("pussy");
        restrictedword.Add("pussys");
        restrictedword.Add("queer");
        restrictedword.Add("retard");
        restrictedword.Add("schlong");
        restrictedword.Add("screw");
        restrictedword.Add("sheister");
        restrictedword.Add("shit");
        restrictedword.Add("shited");
        restrictedword.Add("shitfull");
        restrictedword.Add("shiting");
        restrictedword.Add("shitings");
        restrictedword.Add("shits");
        restrictedword.Add("shitted");
        restrictedword.Add("shitter");
        restrictedword.Add("shitters");
        restrictedword.Add("shitting");
        restrictedword.Add("shittings");
        restrictedword.Add("shitty");
        restrictedword.Add("slag");
        restrictedword.Add("sleaze");
        restrictedword.Add("slut");
        restrictedword.Add("sluts");
        restrictedword.Add("smut");
        restrictedword.Add("snatch");
        restrictedword.Add("spunk");
        restrictedword.Add("twat");
        restrictedword.Add("wetback");
        restrictedword.Add("whore");
        restrictedword.Add("wop");
        string str = strmsg;
        string strnew = "";
        string[] strsp = str.Split(' ');
        int i = 0;
        string chkword = "";
        foreach (string myword in strsp)
        {
            chkword = "";
            foreach (string profinityword in restrictedword)
            {
                if (profinityword == myword.ToLower())
                {
                    chkword = myword.Substring(0, 1);
                    for (i = 1; i <= profinityword.Length - 1; i++)
                    {
                        chkword = chkword + "*";
                    }
                    break;
                }
                else
                {
                    chkword = myword;
                }
            }
            if (!string.IsNullOrEmpty(strnew))
            {
                strnew = strnew + " " + chkword;
            }
            else
            {
                strnew = chkword;
            }
        }
        return strnew;
    }

    public static string replaceEncode(string strencode)
    {
        strencode = strencode.Replace("-", "2D").Replace(" ", "-").Replace("/", "5C").Replace("*", "2A").Replace("%", "0x25").Replace(".", "2A").Replace("!", "");
        return strencode.ToLower();
    }

    public static string replaceDecode(string strdecode)
    {
        strdecode = strdecode.Replace("-", " ").Replace("2d", "-").Replace("2D", "-").Replace("5C", "/").Replace("2A", "*").Replace("0x25", "%").Replace("2A", ".");
        return strdecode.ToLower();
    }

    public static int GetLastSortCountOfChild_Category(string tablename, string columnname, int parentid)
    {
        SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        string strquery = "";
        strquery = " declare @numbercount int select @numbercount = count(*) from " + tablename + " where parentid=@parentid " +
                   " if @numbercount = 0 begin select isnull(MAX(" + columnname + "),1) from " + tablename + " where parentid=@parentid end " +
                   " else begin	select (MAX(" + columnname + ")+ 1) from " + tablename + " where parentid=@parentid	end ";

        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            objcon.Open();
            sqlcmd = new SqlCommand(strquery, objcon);
            sqlcmd.Parameters.AddWithValue("@columnname", columnname);
            sqlcmd.Parameters.AddWithValue("@tablename", tablename);
            sqlcmd.Parameters.AddWithValue("@parentid", parentid);
            return Convert.ToInt32(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objcon.Close();
        }
    }
    //Get file contents
    //public static string GetConfigurationDetail(string keyname)
    //{
    //    configurationManager objconfig = new configurationManager();
    //    string sitename = objconfig.getValuebyKey(keyname);
    //    return sitename;
    //}

    //get files from given directory
    public static string[] GetAllFilesFromDirectory(string path)
    {
        string[] filePaths = { };
        if (path != "")
        {
            filePaths = Directory.GetFiles(path);
        }
        return filePaths;
    }
    //first character is upper case
    public static string CapitalizeFirstLetter(string msg)
    {
        if (msg.Length > 1)
        {
            msg = msg.Substring(0, 1).ToUpper() + msg.Substring(1);
        }
        else
        {
            msg = msg == "" ? msg : msg.ToUpper();
        }
        return msg;
    }
    //get appsetting
    public static string getAppSetting(string key)
    {
        return System.Configuration.ConfigurationManager.AppSettings[key];
    }
    //get base url
    public static string getBaseURL()
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
        return baseUrl;
    }
    //get latitude and longitude
    public static string getlatlong(string add1, string add2, string city, string state, string county, string zipcode)
    {
        string url = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + HttpUtility.HtmlEncode(zipcode) + " " + HttpUtility.HtmlEncode(add1) + " " + HttpUtility.HtmlEncode(add2) + " " + HttpUtility.HtmlEncode(city) + " " + HttpUtility.HtmlEncode(state) + " " + HttpUtility.HtmlEncode(county) + "&sensor=false";
        WebResponse response1 = default(WebResponse);
        WebRequest request = WebRequest.Create(new Uri(url));
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
    //get radom number
    public static string getRandomCode(int length, bool hasUpperCase, bool hasLowerCase, bool hasNumbers)
    {
        string CharacterSetInUpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string CharacterSetInLowerCase = "abcdefghijklmnopqrstuvwxyz";
        string NumberSet = "0123456789";
        string characterSet = "";
        if (hasLowerCase)
            characterSet += CharacterSetInLowerCase;
        if (hasUpperCase)
            characterSet += CharacterSetInUpperCase;
        if (hasNumbers)
            characterSet += NumberSet;
        if (characterSet != "" && length > 0)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(characterSet, length).Select(set => set[random.Next(set.Length)]).ToArray());
        }
        else
        {
            throw new Exception("Specified length should be greater than 0 or choose atleast one characterset.");
        }
    }
    public static string getDateFormatByCulture(string culturename)
    {
        try
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culturename == "" ? "en-IN" : culturename);
            return ci.DateTimeFormat.ShortDatePattern;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static string GetrandomAlfaNum()
    {
        try
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string ConvertObjectToJson(object obj)
    {
        try
        {
            AppResponse objResponse = new AppResponse();
            if (obj != null)
            {
                objResponse.result = obj;
                objResponse.status = APIMESSAGES.SUCCESS;
            }
            else
            {
                objResponse.result = new object();
                objResponse.status = APIMESSAGES.FAIL;
            }
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(objResponse);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void createVideoThumb1(string flvVideoname, string outdfile_prefix, string videooutputpath, string imagefullpath)
    {
        string RootPath = AppSettings.SITE_ROOTPATH;
        ProcessStartInfo opr = new ProcessStartInfo(AppSettings.SITE_ROOTPATH + System.Configuration.ConfigurationManager.AppSettings["ffmpegpath"]);
        opr.WindowStyle = ProcessWindowStyle.Hidden;
        opr.Arguments = "-i " + RootPath + videooutputpath + flvVideoname + " -an -ss " + System.Configuration.ConfigurationManager.AppSettings["flvmidduration"] + " -an -r 1 -vframes 1 -y " + RootPath + imagefullpath + outdfile_prefix + "-%d.jpg";
        Process pr = Process.Start(opr);
        string outFile = RootPath + imagefullpath + outdfile_prefix + "-1.jpg";
        DateTime dtnow = default(DateTime);
        dtnow = DateTime.Now;
        dtnow = dtnow.AddMinutes(1);
        while ((DateTime.Now < dtnow & !System.IO.File.Exists(outFile)))
        {
        }
    }

    public static string GetConfigKeyValue(string passkeyvalue)
    {
        SqlConnection objcon = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConString"]);
        string strquery = "";
        strquery = "  select isnull(configvalue,'') from configuration where configkey='" + passkeyvalue + "' ";
        try
        {
            SqlCommand sqlcmd = new SqlCommand();
            objcon.Open();
            sqlcmd = new SqlCommand(strquery, objcon);
            return Convert.ToString(sqlcmd.ExecuteScalar());
        }
        catch (Exception ex) { throw ex; }
        finally { objcon.Close(); }
    }

    public void createVideoThumb(string flvVideoname, string outdfile_prefix, string videooutputpath, string imagefullpath)
    {
        string RootPath = AppSettings.MAIN_SITE_ROOTPATH;
        ProcessStartInfo opr = new ProcessStartInfo(RootPath + AppSettings.FFMPEGPATH);
        opr.WindowStyle = ProcessWindowStyle.Hidden;
        //opr.UseShellExecute = false; // hardik 19-02-2016
        opr.Arguments = "-i " + RootPath + videooutputpath + flvVideoname + " -an -ss " + AppSettings.FLVMIDDURATION + " -an -r 1 -vframes 1 -y " + RootPath + imagefullpath + outdfile_prefix + "-%d.jpg";
        Process pr = Process.Start(opr);
        string outFile = RootPath + imagefullpath + outdfile_prefix + "-1.jpg";
        DateTime dtnow = default(DateTime);
        dtnow = DateTime.Now;
        dtnow = dtnow.AddMinutes(1);
        while ((DateTime.Now < dtnow & !System.IO.File.Exists(outFile)))
        {
        }
    }

    public class AppResponse
    {
        public object result { get; set; }
        public string status { get; set; }
    }

    public static class APIMESSAGES
    {
        public static string FAIL = "fail";
        public static string SUCCESS = "success";
        public static string ERROR = "error";
        public static string INVALID = "invalid";
        public static string CHANGE = "change";
        public static string EXISTS = "EXISTS";
        public static string DELETED = "DELETED";
        public static string UPDATED = "UPDATED";
        public static string NORECORDFOUND = "NORECORDFOUND";
    }

    public static string AdminPaging(int totalpages, int pageno, string QeryString, string pagename)
    {
        string strleft = "";
        string strright = "";
        string strQueryStringParameters = "";
        strQueryStringParameters = QeryString;
        if (totalpages > 0)
        {
            if (pageno != 1)
            {
                strleft += " <li  class='pgfirst'><a href='" + pagename + "?p=1" + strQueryStringParameters + "' style='background: none;'><span>First</span></a></li>";
                strleft += " <li  class='pgfirst'><a href='" + pagename + "?p=" + (pageno - 1).ToString() + strQueryStringParameters + "' style='background: none;'><span>Prev</span</a></li>";
            }
            if (pageno != totalpages)
            {
                strright += "<li  class='pgfirst'><a href='" + pagename + "?p=" + (pageno + 1).ToString() + strQueryStringParameters + "' style='background: none;'><span>Next</span</a></li>";
                strright += "<li  class='pgfirst'><a href='" + pagename + "?p=" + totalpages.ToString() + strQueryStringParameters + "' style='background: none;'><span>Last</span</a></li>";
            }
            string strmid = "";
            if (totalpages > 1)
            {
                for (int i = 0; i < totalpages; i++)
                {
                    string tmp = "";
                    if ((i + 1) == pageno)
                    {
                        if ((i + 1) == Convert.ToInt32(pageno)) { tmp = "<li><a class='active' style='float:left;'>" + (i + 1).ToString() + "</a></li>"; }
                        else { tmp = "<li><a style='float:left' href='" + pagename + "?p=" + (i + 1).ToString() + strQueryStringParameters + "'>" + (i + 1).ToString() + "</a></li>"; }
                    }
                    else
                    {
                        if ((i + 1) == Convert.ToInt32(pageno)) { tmp = "<li><a class='active' style='float:left'>" + (i + 1).ToString() + "</a></li>"; }
                        else { tmp = "<li><a style='float:left' href='" + pagename + "?p=" + (i + 1).ToString() + strQueryStringParameters + "'>" + (i + 1).ToString() + "</a></li>"; }
                    }
                    if (i >= pageno - 6 && i <= pageno + 4) { strmid += tmp; }
                    else { strmid += ""; }
                }
            }
            return strleft + strmid + strright;
        }
        else { return ""; }
    }

    public static string AdminPagingv2(int totalpages, int pageno, string QeryString, string pagename)
    {
        string strleft = "";
        string strright = "";
        string strQueryStringParameters = "";
        strQueryStringParameters = QeryString;
        if (totalpages > 0)
        {
            if (pageno != 1)
            {
                strleft += " <li  class='paginate_button previous'><a href='" + pagename + "?p=1" + strQueryStringParameters + "' tabindex='0' data-dt-idx='0' aria-controls='example1'><span>First</span></a></li>";
                strleft += " <li  class='paginate_button previous'><a href='" + pagename + "?p=" + (pageno - 1).ToString() + strQueryStringParameters + "'tabindex='0' data-dt-idx='0' aria-controls='example1'><span>Prev</span</a></li>";
            }
            if (pageno != totalpages)
            {
                strright += "<li  class='paginate_button next'><a href='" + pagename + "?p=" + (pageno + 1).ToString() + strQueryStringParameters + "' tabindex='0' data-dt-idx='7' aria-controls='example1'><span>Next</span</a></li>";
                strright += "<li  class='paginate_button next'><a href='" + pagename + "?p=" + totalpages.ToString() + strQueryStringParameters + "' tabindex='0' data-dt-idx='7' aria-controls='example1'><span>Last</span</a></li>";
            }
            string strmid = "";
            if (totalpages > 1)
            {
                for (int i = 0; i < totalpages; i++)
                {
                    string tmp = "";
                    if ((i + 1) == pageno)
                    {
                        if ((i + 1) == Convert.ToInt32(pageno)) { tmp = "<li class='paginate_button active'><a tabindex='0' data-dt-idx='1' aria-controls='example1'>" + (i + 1).ToString() + "</a></li>"; }
                        else { tmp = "<li class='paginate_button'><a tabindex='0' data-dt-idx='1' aria-controls='example1' href='" + pagename + "?p=" + (i + 1).ToString() + strQueryStringParameters + "'>" + (i + 1).ToString() + "</a></li>"; }
                    }
                    else
                    {
                        if ((i + 1) == Convert.ToInt32(pageno)) { tmp = "<li class='paginate_button active'><a tabindex='0' data-dt-idx='1' aria-controls='example1'>" + (i + 1).ToString() + "</a></li>"; }
                        else { tmp = "<li class='paginate_button'><a tabindex='0' data-dt-idx='1' aria-controls='example1' href='" + pagename + "?p=" + (i + 1).ToString() + strQueryStringParameters + "'>" + (i + 1).ToString() + "</a></li>"; }
                    }
                    if (i >= pageno - 6 && i <= pageno + 4) { strmid += tmp; }
                    else { strmid += ""; }
                }
            }
            return strleft + strmid + strright;
        }
        else { return ""; }
    }

    public static bool IsQueryString(string strQueryString, bool isINTEGER)
    {
        bool flag = false;
        if (isINTEGER)
        {
            if (System.Web.HttpContext.Current.Request.QueryString[strQueryString] != null)
            {
                if (System.Web.HttpContext.Current.Request.QueryString[strQueryString].ToString() != "")
                {
                    if (RegExp.IsNumericValue(System.Web.HttpContext.Current.Request.QueryString[strQueryString].ToString()))
                    {
                        flag = true;
                    }
                }
            }
        }
        else
        {
            if (System.Web.HttpContext.Current.Request.QueryString[strQueryString] != null)
            {
                if (System.Web.HttpContext.Current.Request.QueryString[strQueryString].ToString() != "")
                {
                    flag = true;
                }
            }
        }
        return flag;
    }

    public static bool IsValidRequestDotForm(string strRequestDotForm, bool isINTEGER)
    {
        bool flag = false;
        if (isINTEGER)
        {
            if (System.Web.HttpContext.Current.Request.Form[strRequestDotForm] != null)
            {
                if (System.Web.HttpContext.Current.Request.Form[strRequestDotForm].ToString() != "")
                {
                    if (RegExp.IsNumericValue(System.Web.HttpContext.Current.Request.Form[strRequestDotForm].ToString()))
                    {
                        if (Convert.ToInt32(System.Web.HttpContext.Current.Request.Form[strRequestDotForm].ToString()) > 0)
                        {
                            flag = true;
                        }
                    }
                }
            }
        }
        else
        {
            if (System.Web.HttpContext.Current.Request.Form[strRequestDotForm] != null)
            {
                if (System.Web.HttpContext.Current.Request.Form[strRequestDotForm].ToString() != "")
                {
                    flag = true;
                }
            }
        }

        return flag;
    }

    public static bool IsValidValue(string input, bool isInt, bool isDecimal, bool isGreterthenZero)
    {
        bool flag = false;
        if (isInt)
        {
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    if (isDecimal)
                    {
                        if (CommonFunctions.IsDecimalValue(input))
                        {
                            if (isGreterthenZero)
                            {
                                if (Convert.ToDecimal(input) > 0)
                                {
                                    flag = true;
                                }
                            }
                            else
                            {
                                flag = true;
                            }

                        }
                    }
                    else
                    {
                        if (CommonFunctions.IsNumericValue(input))
                        {
                            if (isGreterthenZero)
                            {
                                if (Convert.ToInt64(input) > 0)
                                {
                                    flag = true;
                                }
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    flag = true;
                }
            }
        }
        return flag;
    }

    public static bool IsValidValue1(string input, bool isInt, bool isDecimal, bool isGreterthenZero)
    {
        bool flag = false;
        if (isInt)
        {
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    if (isDecimal)
                    {
                        if (CommonFunctions.IsDecimalValue(input))
                        {
                            if (isGreterthenZero)
                            {
                                if (Convert.ToDecimal(input) > -1)
                                {
                                    flag = true;
                                }
                            }
                            else
                            {
                                flag = true;
                            }

                        }
                    }
                    else
                    {
                        if (CommonFunctions.IsNumericValue(input))
                        {
                            if (isGreterthenZero)
                            {
                                if (Convert.ToInt64(input) > -1)
                                {
                                    flag = true;
                                }
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    flag = true;
                }
            }
        }
        return flag;
    }

    public static bool IsDecimalValue(string strTextEntry)
    {
        System.Text.RegularExpressions.Regex objNotWholePattern = new System.Text.RegularExpressions.Regex("^\\d{0,8}(\\.\\d{1,4})?$");
        return objNotWholePattern.IsMatch(strTextEntry) && (!string.IsNullOrEmpty(strTextEntry));
    }



    //video conversion

    #region

    //public string convertotflv(string imgName, int iMaxId, ref int duration, string videoName, ref string errMsg, string videooutputpath, string imageoutputpath, string imagefullpath)
    //{
    //    string filename = null;
    //    MediaHandler _mhandler = new MediaHandler();

    //    //Dim RootPath As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)
    //    string RootPath = AppSettings.MAIN_SITE_ROOTPATH;

    //    _mhandler.FFMPEGPath = RootPath + AppSettings.FFMPEGPATH;
    //    _mhandler.InputPath = RootPath + AppSettings.TEMPBANNERVIDEO_ROOTPATH;
    //    _mhandler.OutputPath = RootPath + videooutputpath;
    //    // SETUP
    //    // Place sample video in _root\contents\original folder where _root represent root of your web application.
    //    // Rename sample.mp4 with your sample video name.
    //    _mhandler.FileName = imgName;
    //    if (!string.IsNullOrEmpty(videoName))
    //    {
    //        _mhandler.OutputFileName = videoName + ".flv";
    //        filename = videoName;
    //    }
    //    else
    //    {
    //        _mhandler.OutputFileName = iMaxId + ".flv";
    //        filename = Convert.ToString(iMaxId);
    //    }
    //    filename = filename + ".flv";
    //    _mhandler.Width = 648;
    //    _mhandler.Height = 362;
    //    _mhandler.MaxQuality = true;
    //    _mhandler.Video_Bitrate = Convert.ToDouble(AppSettings.FLVVIDEOBITRATE);
    //    _mhandler.Audio_Bitrate = Convert.ToDouble(AppSettings.FLVAUDIOBITRATE);
    //    _mhandler.Audio_SamplingRate = Convert.ToInt32(AppSettings.FLVSAMPLINGRATE);

    //    VideoInfo info = _mhandler.Encode_FLV();
    //    duration = Convert.ToInt32(info.Duration_Sec);
    //    // retrieve valudes

    //    if (info.ErrorCode > 0)
    //    {
    //        if (info.ErrorCode == 121) //old 121
    //        {
    //            if (string.IsNullOrEmpty(videoName))
    //            {
    //                videoName = Convert.ToString(iMaxId);
    //                createVideoThumb(filename, videoName, videooutputpath, imagefullpath);

    //                string path = null;
    //                if (string.IsNullOrEmpty(videoName))
    //                {
    //                    path = RootPath + imagefullpath + iMaxId + "-1.jpg";
    //                }
    //                else
    //                {
    //                    path = RootPath + imagefullpath + videoName + "-1.jpg";
    //                }

    //                if (System.IO.File.Exists(path))
    //                {
    //                    // 22_02_2016
    //                    Thmbimages(path, RootPath + imageoutputpath, iMaxId + ".jpg", 240, 180, 0);
    //                    System.IO.File.Delete(path);

    //                    // 22_02_2016
    //                    //Thmbimages(path, RootPath + imageoutputpath, iMaxId + ".jpg", 370, 257, 0);
    //                    //Thmbimages(path, RootPath + AppSettings.HOMEBANNERIMAGE_THUMB_ROOTPATH.ToString(), iMaxId + ".jpg", Convert.ToInt32(AppSettings.HOMEBANNERIMAGE_THUMB_WIDTH.ToString()), Convert.ToInt32(AppSettings.HOMEBANNERIMAGE_THUMB_HEIGHT.ToString()), 0);

    //                }
    //            }
    //        }
    //        else
    //        {
    //            //errMsg = "ErrorCode: " & info.ErrorCode & " ErrorMessage: " & info.ErrorMessage
    //            errMsg = "Video file conversion failed.Please try again with another file format.";
    //            return "1";
    //        }

    //    }
    //    else
    //    {
    //        if (string.IsNullOrEmpty(videoName))
    //        {
    //            videoName = Convert.ToString(iMaxId);
    //            createVideoThumb(filename, videoName, videooutputpath, imagefullpath);

    //            string path = null;
    //            if (string.IsNullOrEmpty(videoName))
    //            {
    //                path = RootPath + imagefullpath + iMaxId + "1.jpg";
    //            }
    //            else
    //            {
    //                path = RootPath + imagefullpath + videoName + "-1.jpg";
    //            }


    //            Thmbimages(path, RootPath + imageoutputpath, iMaxId + ".jpg", 370, 257, 0);
    //            Thmbimages(path, RootPath + AppSettings.HOMEBANNERIMAGE_THUMB_ROOTPATH.ToString(), iMaxId + ".jpg", Convert.ToInt32(AppSettings.HOMEBANNERIMAGE_THUMB_WIDTH.ToString()), Convert.ToInt32(AppSettings.HOMEBANNERIMAGE_THUMB_HEIGHT.ToString()), 0);
    //            // System.IO.File.Delete(path);

    //        }
    //    }
    //    return filename;
    //}


    //public string convertotflvForCommon(string imgName, int iMaxId, ref int duration, string videoName, ref string errMsg, string videooutputpath, string imageoutputpath, string imagefullpath, string InputPath, string ThumbPath, string ThumbWidth, string ThumbHeight)
    //{
    //    string filename = null;
    //    MediaHandler _mhandler = new MediaHandler();

    //    //Dim RootPath As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)
    //    string RootPath = AppSettings.MAIN_SITE_ROOTPATH;

    //    _mhandler.FFMPEGPath = RootPath + AppSettings.FFMPEGPATH;
    //    _mhandler.InputPath = RootPath + InputPath;
    //    _mhandler.OutputPath = RootPath + videooutputpath;
    //    // SETUP
    //    // Place sample video in _root\contents\original folder where _root represent root of your web application.
    //    // Rename sample.mp4 with your sample video name.
    //    _mhandler.FileName = imgName;
    //    if (!string.IsNullOrEmpty(videoName))
    //    {
    //        _mhandler.OutputFileName = videoName + ".flv";
    //        filename = videoName;
    //    }
    //    else
    //    {
    //        _mhandler.OutputFileName = iMaxId + ".flv";
    //        filename = Convert.ToString(iMaxId);
    //    }
    //    filename = filename + ".flv";
    //    _mhandler.Width = 648;
    //    _mhandler.Height = 362;
    //    _mhandler.MaxQuality = true;
    //    _mhandler.Video_Bitrate = Convert.ToDouble(AppSettings.FLVVIDEOBITRATE);
    //    _mhandler.Audio_Bitrate = Convert.ToDouble(AppSettings.FLVAUDIOBITRATE);
    //    _mhandler.Audio_SamplingRate = Convert.ToInt32(AppSettings.FLVSAMPLINGRATE);

    //    VideoInfo info = _mhandler.Encode_FLV();
    //    duration = Convert.ToInt32(info.Duration_Sec);
    //    // retrieve valudes

    //    if (info.ErrorCode > 0)
    //    {
    //        if (info.ErrorCode == 121) //old 121
    //        {
    //            if (string.IsNullOrEmpty(videoName))
    //            {
    //                videoName = Convert.ToString(iMaxId);
    //                createVideoThumb(filename, videoName, videooutputpath, imagefullpath);

    //                string path = null;
    //                if (string.IsNullOrEmpty(videoName))
    //                {
    //                    path = RootPath + imagefullpath + iMaxId + "-1.jpg";
    //                }
    //                else
    //                {
    //                    path = RootPath + imagefullpath + videoName + "-1.jpg";
    //                }

    //                if (System.IO.File.Exists(path))
    //                {
    //                    // 22_02_2016
    //                    Thmbimages(path, RootPath + imageoutputpath, iMaxId + ".jpg", 240, 180, 0);
    //                    System.IO.File.Delete(path);

    //                    // 22_02_2016
    //                    //Thmbimages(path, RootPath + imageoutputpath, iMaxId + ".jpg", 370, 257, 0);
    //                    //Thmbimages(path, RootPath + AppSettings.HOMEBANNERIMAGE_THUMB_ROOTPATH.ToString(), iMaxId + ".jpg", Convert.ToInt32(AppSettings.HOMEBANNERIMAGE_THUMB_WIDTH.ToString()), Convert.ToInt32(AppSettings.HOMEBANNERIMAGE_THUMB_HEIGHT.ToString()), 0);

    //                }
    //            }
    //        }
    //        else
    //        {
    //            //errMsg = "ErrorCode: " & info.ErrorCode & " ErrorMessage: " & info.ErrorMessage
    //            errMsg = "Video file conversion failed.Please try again with another file format.";
    //            return "1";
    //        }

    //    }
    //    else
    //    {
    //        if (string.IsNullOrEmpty(videoName))
    //        {
    //            videoName = Convert.ToString(iMaxId);
    //            createVideoThumb(filename, videoName, videooutputpath, imagefullpath);

    //            string path = null;
    //            if (string.IsNullOrEmpty(videoName))
    //            {
    //                path = RootPath + imagefullpath + iMaxId + "1.jpg";
    //            }
    //            else
    //            {
    //                path = RootPath + imagefullpath + videoName + "-1.jpg";
    //            }


    //            Thmbimages(path, RootPath + imageoutputpath, iMaxId + ".jpg", 370, 257, 0);
    //            Thmbimages(path, RootPath + ThumbPath.ToString(), iMaxId + ".jpg", Convert.ToInt32(ThumbWidth.ToString()), Convert.ToInt32(ThumbHeight.ToString()), 0);
    //            // System.IO.File.Delete(path);

    //        }
    //    }
    //    return filename;
    //}

    #endregion
}

public sealed class ControlChars
{
    public const char Back = '\b';
    public const char Cr = '\r';
    public const string CrLf = "\r\n";
    public const char FormFeed = '\f';
    public const char Lf = '\n';
    public const string NewLine = "\r\n";
    public const char NullChar = '\0';
    public const char Quote = '"';
    public const char Tab = '\t';
    public const char VerticalTab = '\v';
}

//regular expressions and messages
#region "--------------------------Regular Expressions---------------------------"

public class RegExp
{
    public static readonly string Url = "[a-zA-Z0-9-_\\$]+(//.[a-za-z0-9-_//$]+)?\\??" + "[a-zA-Z0-9-_\\$]+=?[a-zA-Z0-9-_\\$]+(&[a-zA-Z0-9-_\\$]+=" + "[a-zA-Z0-9-_\\$]+)*";

    public static readonly string Date = "(0[1-9]|[12][0-9]|3[01])[-]" + "(0[1-9]|1[012])[-]((175[7-9])|(17[6-9][0-9])|(1[8-9][0-9][0-9])|" + "([2-9][0-9][0-9][0-9]))";
    // supports dates from 1-1-1757 to 31-12-9999 for SQL Server 2000 Date Range 

    public static readonly string Time = "(0[1-9]|[1][0-2])[:]" + "(0[0-9]|[1-5][0-9])[:](0[0-9]|[1-5][0-9])[ ][A|a|P|p][M|m]";

    public static readonly string Number = "[-+]?[0-9]*\\.?[0-9]*";
    public static readonly string Digit = "[0-9]*";
    public static readonly string NonNegative = "[+]?[0-9]*\\.?[0-9]*";

    public static string MaxLength(int len)
    {
        return "[\\s\\S]{0," + len.ToString() + "}";
    }

    public static readonly string SoundFilesUpload = "^(([a-zA-Z]:)|(\\\\{2}\\w+)\\$?)(\\\\(\\w[\\w].*))";
    public static readonly string ImageFilesUpload = "^.+\\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$";
    public static readonly string Emailid = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

    public static bool IsNumericValue(string strTextEntry)
    {
        if (strTextEntry != null)
        {
            System.Text.RegularExpressions.Regex objNotWholePattern = new System.Text.RegularExpressions.Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strTextEntry) && (strTextEntry != "");
        }
        else
        {
            return false;
        }
    }
}

public class ValidationMessages
{

    public static readonly string Url = "Please enter a valid URL.<br>Valid " + "characters are all alphanumeric characters and .?" + "&_=-$<br> example: home.aspx?id=5&name=$my_name";
    public static readonly string Required = "Required";
    public static readonly string Date = "Please enter a valid date in dd-MM-yyyy format.";
    public static readonly string Time = "Please enter a valid time in hh:mm:ss am format.";
    public static readonly string Number = "Must be a valid number.";
    public static readonly string Digit = "Must be a valid whole number.";
    public static readonly string NonNegative = "Must be a non-negative number.";
    public static readonly string SoundFilesUpload = "Only mp3, m3u or mpeg files are allowed!";
    public static readonly string ImageFilesUpload = "Only (jpeg, jpg, bmp, gif or png) files are allowed!";
    public static readonly string Emailid = "Invalid e-mail address! Please re-enter.";

    public static string MaxLength(int len)
    {
        return " Maximum " + len.ToString() + " characters are allowed ";
    }

    public static string IsDate(string strdate)
    {

        const string pattern = "^(?<Day>\\d{1,2})(\\/)(?<Month>\\d{1,2})(\\/)(?<Year>\\d{4})$";

        System.Text.RegularExpressions.MatchCollection mc = default(System.Text.RegularExpressions.MatchCollection);
        mc = System.Text.RegularExpressions.Regex.Matches(strdate, pattern);

        if (mc.Count > 0)
        {
            //If mc.Item(0).Groups.Count >= 7 Then
            string strMonth = mc[0].Groups["Month"].Value.PadLeft(2, '0');
            string strDay = mc[0].Groups["Day"].Value.PadLeft(2, '0');
            string strYear = mc[0].Groups["Year"].Value;

            int intDay = Convert.ToInt32(strMonth);
            int intMonth = Convert.ToInt32(strDay);
            int intYear = Convert.ToInt32(strYear);

            if (intMonth < 1 || intMonth > 12)
            {
                return "Month must be between 1 and 12.";
            }

            if (intDay < 1 || intDay > 31)
            {
                return "Day must be between 1 and 31.";
            }

            if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intDay == 31))
            {
                return "Month " + strMonth + " doesn`t have 31 days!";
            }

            if (intMonth == 2)
            {
                //check for february 29th
                int isleap = 0;//= (intYear % 4 == 0 && (intYear % 100 != 0 || intYear % 400 == 0));
                if (intDay > 29 || (intDay == 29 & !Convert.ToBoolean(isleap)))
                {
                    return "February " + strYear + " doesn`t have " + strDay + " days!";
                }
            }
            //Format the date as 01/02/2006
            //Dim strDateNew As String = strMonth & "/" & strDay & "/" & strYear
            //End If
            return null;
        }
        else
        {
            return "Please enter date as dd/mm/yyyy. Your current selection reads: " + strdate;
        }
    }



}
#endregion

