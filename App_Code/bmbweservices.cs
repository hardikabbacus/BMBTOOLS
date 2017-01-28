using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for bmbweservices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class bmbweservices : System.Web.Services.WebService
{
    public bmbweservices()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    // add master product using pop up
    [WebMethod(EnableSession = true)]
    public void addMasterProduct(string productName, string sku, string productDescription)
    {
        productManager objproduct = new productManager();
        if (productName != "" && sku != "" && productDescription != "")
        {
            objproduct.productName = productName;
            objproduct.sku = sku;
            objproduct.productDescription = productDescription;
            objproduct.barcode = "";
            objproduct.isVarientProduct = 0;
            objproduct.isMasterProduct = 1;
            objproduct.price = 0;
            objproduct.cost = 0;
            objproduct.minimumQuantity = 0;
            objproduct.inventory = 0;
            objproduct.isactive = 0;
            objproduct.isFeatured = 0;

            objproduct.InsertItem();

            //return "success";
        }
        else
        {
            //return "fail";
        }
    }

}
