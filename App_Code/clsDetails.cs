using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsDetails
/// </summary>
public class clsDetails
{
	public clsDetails()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public class UserDetails
    {
        string sOPCode = string.Empty;
        string sCountryCode = string.Empty;
        string sMobile = string.Empty;
        string sDeviceToken = string.Empty;
        string sDeviceType = string.Empty;
        //string sOldMPIN = string.Empty;
        //string sNewMPIN = string.Empty;
        //string sSessionId = string.Empty;

        public string OPCODE { get; set; }
        public string COUNTRYCODE { get; set; }
        public string MOBILENO{ get; set; }
        public string DEVICETYPE{ get; set; }
        public string DEVICEID{ get; set; }
        
        //public string SESSIONID { get; set; }
        
    }
}