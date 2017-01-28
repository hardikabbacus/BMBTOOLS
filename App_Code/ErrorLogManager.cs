using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Net;

/// <summary>
/// Summary description for ErrorLogManager
/// </summary>
public class ErrorLogManager
{
    #region "Contructor"
    public ErrorLogManager()
    {

    }
    #endregion

    #region "Private Member Variable"
    private string _ErrorDate = "";
    private string _ErrorFrom = "";
    private string _ErrorText = "";
    private string _ApiRequestWithParam = "";
    private Exception _TraceError;
    private string _RootFilePath = "";
    #endregion

    #region "Static Member Variable"
    public static readonly string ErrorLog_File_Path = "resources/ErrorLogs";
    #endregion

    #region "Public Property"

    public string ErrorDate
    {
        get { return _ErrorDate; }
        set { _ErrorDate = value; }
    }

    public string ErrorFrom
    {
        get { return _ErrorFrom; }
        set { _ErrorFrom = value; }
    }

    public string ApiRequestWithParam
    {
        get { return _ApiRequestWithParam; }
        set { _ApiRequestWithParam = value; }
    }

    public Exception TraceError
    {
        get { return _TraceError; }
        set { _TraceError = value; }
    }

    public string RootFilePath { get{ return _RootFilePath; } set { _RootFilePath = value; } }

    #endregion

    #region "Public Methods"
    public void WriteLog()
    {
        StreamWriter LogFileStream = null;
        string deviceOs = "";
        try
        {
            ErrorDate = DateTime.Now.ToFileTime().ToString();
            ErrorFrom = HttpContext.Current.Request.UserAgent.ToString().ToLower();
            if (ErrorFrom.Contains("darwin"))
            {
                deviceOs = "iOS.";
            }
            else if (ErrorFrom.Contains("android"))
            {
                deviceOs = "Android.";
            }
            else
            {
                deviceOs = "";
            }

            RootFilePath = HttpContext.Current.Server.MapPath("~") + "\\resources\\ErrorLogs\\";

            string fileName = RootFilePath + deviceOs + "Error.log." + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

            LogFileStream = default(StreamWriter);

            if ((File.Exists(fileName)))
            {
                LogFileStream = File.AppendText(fileName);
            }
            else
            {
                LogFileStream = File.CreateText(fileName);
            }

            LogFileStream.Write("\n User Agent : ");
            LogFileStream.Write(ErrorFrom);

            LogFileStream.Write("\n Request Parameter : ");
            LogFileStream.Write(ApiRequestWithParam);

            if (TraceError != null)
            {
                LogFileStream.Write("\n Errors : ");
                LogFileStream.Write("\n Requested Url : ");
                LogFileStream.WriteLine(HttpContext.Current.Request.Url.ToString());
                LogFileStream.Write("\n Message : ");
                LogFileStream.WriteLine(TraceError.Message);
                LogFileStream.Write(" StackTrace  : ");
                LogFileStream.WriteLine(TraceError.StackTrace);
                LogFileStream.Write(" TargetSite  : ");
                LogFileStream.WriteLine(TraceError.TargetSite);
            }

            LogFileStream.Flush();
            LogFileStream.Close();
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
            if (LogFileStream != null)
            {
                LogFileStream.Dispose();
            }
        }
    }

    public DataTable getLogs()
    {
        DataTable dtError = null;
        DataRow row = null;
        FileInfo fileInfo = null;

        string filename = "";
        string[] logFilelist = { };

        try
        {
            RootFilePath = HttpContext.Current.Server.MapPath("~/" + ErrorLog_File_Path);
            logFilelist = CommonFunctions.GetAllFilesFromDirectory(RootFilePath);

            dtError = new DataTable("Errorlogs");
            dtError.Columns.Add("srno", typeof(string));
            dtError.Columns.Add("createddate", typeof(DateTime));
            dtError.Columns.Add("osname", typeof(string));
            dtError.Columns.Add("filename", typeof(string));

            for (int i = 0; i < logFilelist.Length; i++)
            {
                //get file info
                fileInfo = new FileInfo(logFilelist[i]);
                filename = fileInfo.Name;

                row = dtError.NewRow();
                row["srno"] = (i + 1).ToString();
                row["createddate"] = fileInfo.CreationTime;

                if (filename.Split('.')[0].ToLower() == "error")
                {
                    row["osname"] = "Website log";
                }
                else
                {
                    row["osname"] = filename.Split('.')[0] + " log";
                }

                row["filename"] = filename;
                dtError.Rows.Add(row);
            }
            
            DataView dvLogs = new DataView();
            dvLogs.Table = dtError;
            dvLogs.Sort = "createddate desc";
            return dvLogs.ToTable();
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion
}