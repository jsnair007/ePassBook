using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using PassBook;

public partial class VPTransReceiver : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PassBook.ServiceClient objPassBook = new PassBook.ServiceClient();
        string sResponse = "";
        string hostName = Dns.GetHostName();

        //string myIP = Request.UserHostAddress;
        //string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
        //string myIP1 = Dns.GetHostByAddress(hostName).AddressList[0].ToString();
        //clsEvent.LogEvent("Source IP Address is :" + myIP);
        //Console.ReadKey();  

        try
        {
            string sInput = Request.QueryString.Get("INPUT");
            sResponse= objPassBook.PassBookService(sInput);
            string decryptedResponse = JsonCsharpUtiil.getDecryptString(sResponse, "ditgm@1985$$");
            //Response.Write(decryptedResponse);
	    Response.Write(sResponse);

        }
        catch (Exception eee)
        {
            clsEvent.LogExceptionEvent(eee.Message+ " at : "+DateTime.Now.ToString());
            sResponse = "116";
        }
        
    }
}