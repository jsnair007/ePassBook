using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// Summary description for jsoncsharputiil
/// </summary>
public class JsonCsharpUtiil
{
    public JsonCsharpUtiil()
	{
	}
    public static clsInput Json2ClsObj(string processJsonStr)
    {
        clsInput obj = new clsInput();
        if (processJsonStr.IndexOf("{") > -1 && processJsonStr.IndexOf(":") > -1 && processJsonStr.IndexOf("}") > -1)
        {
            if (processJsonStr.ToUpper().Contains("SELECT") ||
                processJsonStr.ToUpper().Contains("INSERT") ||
                processJsonStr.ToUpper().Contains("DELETE") ||
                processJsonStr.ToUpper().Contains("TRUNCATE") ||
                processJsonStr.ToUpper().Contains("DROP") ||
                processJsonStr.ToUpper().Contains("EXEC") ||
                processJsonStr.ToUpper().Contains("EXECUTE") ||
                processJsonStr.ToUpper().Contains("TABLE"))
            {
                return obj;
            }
            processJsonStr = processJsonStr.Replace("\"", "").Trim();
            string[] jsonStrOB = processJsonStr.Split('{');
            if (processJsonStr.ToUpper().Contains("INPUT"))
            {
                jsonStrOB[2] = jsonStrOB[2].Trim().Replace("}", "");
                jsonStrOB[2] = jsonStrOB[2].Trim().Replace("}}", "");
                jsonStrOB[2] = jsonStrOB[2].Trim().Replace("{", "");
                jsonStrOB[2] = jsonStrOB[2].Trim().Replace("{{", "");
                processJsonStr = jsonStrOB[2];
            }
            else
            {
                processJsonStr = jsonStrOB[1];
                processJsonStr = processJsonStr.Trim().Replace("}", "");
                processJsonStr = processJsonStr.Trim().Replace("}}", "");
                processJsonStr = processJsonStr.Trim().Replace("{", "");
                processJsonStr = processJsonStr.Trim().Replace("{{", "");
            }

            string[] jsonStrP = processJsonStr.Split(',');
            foreach (string a in jsonStrP)
            {
                string[] jsonStrC = a.Split(':');
                if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "OPCODE")
                {
                    obj.opcode = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "MOBILENO")
                {
                    obj.mobileno = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "COUNTRYCODE")
                {
                    obj.countrycode = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "DEVICETYPE")
                {
                    obj.devicetype = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "DEVICEID")
                {
                    obj.deviceid = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "OLDPIN")
                {
                    obj.oldpin = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "NEWPIN")
                {
                    obj.newpin = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "PUSHTOKEN")
                {
                    obj.pushtoken = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "MPIN")
                {
                    obj.mpin = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "SESSIONID")
                {
                    obj.sessionid = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "CUSTOMERID")
                {
                    obj.customerid = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "ACCOUNTNO")
                {
                    obj.accountno = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "ACCOUNTTYPE")
                {
                    obj.accounttype = jsonStrC[1].Trim();
                }
                else if (jsonStrC[0] != null && jsonStrC[0] != "" && jsonStrC[0].Trim() == "LASTTRANSACTIONDATETIME")
                {
                    obj.lasttransactiondatetime = jsonStrC[1].Trim();
                }
            }
        }
        return obj;
    }

    public static string getDecryptString(string encData, string key)
    {
        // @!1234xyz# main key for json string
        // .@!++987  password key
        string urlWithChkSum = Decrypt(encData, key);
        return validateCheckSum(urlWithChkSum);
    }

    public static string getEncryptString(string strData, string key)
    {
        //"@!1234xyz#" main key string
        string checkSumValue = CalculateMD5Hash(strData);
        string strDataWithCheckSum = strData + "|checkSum=" + checkSumValue;
        return Encrypt(strDataWithCheckSum, key);
    }

    private static string Decrypt(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 0x80;
        rijndaelCipher.BlockSize = 0x80;
        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[0x10];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }
    public static string CalculateMD5Hash(string strInput)
    {
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(strInput);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }
    private static string validateCheckSum(string urlWithChkSum)
    {
        string urlWithData = "";
        int indexChkSum = urlWithChkSum.IndexOf("checkSum");
        int length_encData = urlWithChkSum.Length;
        string[] chckDataStr = urlWithChkSum.Split('|');
        int i = 0;
        string checkSum = "";
        string VBDVATBank = "";
        foreach (string pipeStr in chckDataStr)
        {
            if (chckDataStr[i].Contains("checkSum"))
            {
                string b01CheckSum = chckDataStr[i];
                string[] checkSumVal = b01CheckSum.Split('=');
                checkSum = checkSumVal[1];
            }
            i++;
        }
        try
        {
            VBDVATBank = urlWithChkSum.Substring(0, indexChkSum - 1);
        }
        catch (Exception e)
        {
            return urlWithData;
        }
        string checkSumLcl = CalculateMD5Hash(VBDVATBank);
        if (checkSumLcl.Equals(checkSum))
        {
            urlWithData = VBDVATBank;
        }
        return urlWithData;
    }

    private static string Encrypt(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 0x80;
        rijndaelCipher.BlockSize = 0x80;
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[0x10];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }
    public static string ObjectToJSON(Object oObject)
    {
        string strXml = ToXML(oObject);
        XmlDocument xmldoc = new XmlDocument();
        try
        {
            xmldoc.LoadXml(strXml);
            return XmlToJSON(xmldoc);
        }
        catch (Exception easd)
        {
            return "ERROR";
        }
    }
    private static string ToXML(Object oObject)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
        using (MemoryStream xmlStream = new MemoryStream())
        {
            xmlSerializer.Serialize(xmlStream, oObject);
            xmlStream.Position = 0;
            xmlDoc.Load(xmlStream);
            return xmlDoc.InnerXml;
        }
    }
    private static string XmlToJSON(XmlDocument xmlDoc)
    {
        StringBuilder sbJSON = new StringBuilder();
        sbJSON.Append("{ ");
        XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
        sbJSON.Append("}");
        return sbJSON.ToString();
    }


    //  XmlToJSONnode:  Output an XmlElement, possibly as part of a higher array
    private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
    {
        if (showNodeName)
            sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
        sbJSON.Append("{");
        // Build a sorted list of key-value pairs
        //  where   key is case-sensitive nodeName
        //          value is an ArrayList of string or XmlElement
        //  so that we know whether the nodeName is an array or not.
        SortedList childNodeNames = new SortedList();


        //  Add in all node attributes
        if (node.Attributes != null)
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name != null && (attr.Name.Contains("xmlns:xsi") || attr.Name.Contains("xmlns:xsd")))
                {

                }
                else
                {
                    StoreChildNode(childNodeNames, attr.Name, attr.InnerText);
                }
            }


        //  Add in all nodes
        foreach (XmlNode cnode in node.ChildNodes)
        {
            if (cnode is XmlText)
                StoreChildNode(childNodeNames, "value", cnode.InnerText);
            else if (cnode is XmlElement)
                StoreChildNode(childNodeNames, cnode.Name, cnode);
        }


        // Now output all stored info
        foreach (string childname in childNodeNames.Keys)
        {
            ArrayList alChild = (ArrayList)childNodeNames[childname];
            if (alChild.Count == 1)
                OutputNode(childname, alChild[0], sbJSON, true);
            else
            {
                sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                foreach (object Child in alChild)
                    OutputNode(childname, Child, sbJSON, false);
                sbJSON.Remove(sbJSON.Length - 2, 2);
                sbJSON.Append(" ], ");
            }
        }
        sbJSON.Remove(sbJSON.Length - 2, 2);
        sbJSON.Append(" }");
    }


    //  StoreChildNode: Store data associated with each nodeName
    //                  so that we know whether the nodeName is an array or not.
    private static void StoreChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
    {
        // Pre-process contraction of XmlElement-s
        if (nodeValue is XmlElement)
        {
            // Convert  <aa></aa> into "aa":null
            //          <aa>xx</aa> into "aa":"xx"
            XmlNode cnode = (XmlNode)nodeValue;
            if (cnode.Attributes.Count == 0)
            {
                XmlNodeList children = cnode.ChildNodes;
                if (children.Count == 0)
                    nodeValue = null;
                else if (children.Count == 1 && (children[0] is XmlText))
                    nodeValue = ((XmlText)(children[0])).InnerText;
            }
        }
        // Add nodeValue to ArrayList associated with each nodeName
        // If nodeName doesn't exist then add it
        object oValuesAL = childNodeNames[nodeName];
        ArrayList ValuesAL;
        if (oValuesAL == null)
        {
            ValuesAL = new ArrayList();
            childNodeNames[nodeName] = ValuesAL;
        }
        else
            ValuesAL = (ArrayList)oValuesAL;
        ValuesAL.Add(nodeValue);
    }


    private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
    {
        if (alChild == null)
        {
            if (showNodeName)
                sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
            sbJSON.Append("null");
        }
        else if (alChild is string)
        {
            if (showNodeName)
                sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
            string sChild = (string)alChild;
            sChild = sChild.Trim();
            sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
        }
        else
            XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName);
        sbJSON.Append(", ");
    }


    // Make a string safe for JSON
    private static string SafeJSON(string sIn)
    {
        StringBuilder sbOut = new StringBuilder(sIn.Length);
        foreach (char ch in sIn)
        {
            if (Char.IsControl(ch) || ch == '\'')
            {
                int ich = (int)ch;
                sbOut.Append(@"\u" + ich.ToString("x4"));
                continue;
            }
            else if (ch == '\"' || ch == '\\' || ch == '/')
            {
                sbOut.Append('\\');
            }
            sbOut.Append(ch);
        }
        return sbOut.ToString();
    }

    public class clsInput
    {
        public clsInput()
        {
        }
        public string opcode { get; set; }
        public string mobileno { get; set; }
        public string countrycode { get; set; }
        public string devicetype { get; set; }
        public string deviceid { get; set; }
        public string oldpin { get; set; }
        public string newpin { get; set; }
        public string pushtoken { get; set; }
        public string mpin { get; set; }
        public string sessionid { get; set; }
        public string customerid { get; set; }
        public string accountno { get; set; }
        public string accounttype { get; set; }
        public string lasttransactiondatetime { get; set; }
    }
    public class clsOutput
    {
        public clsSuccess successStatus { get; set; }
        public clsError errorStatus { get; set; }
        public clsInvalidCredential InvalidCredential { get; set; }
        public clsBlocked blocked { get; set; }
        public clsAnotherReason anotherStatus { get; set; }
    }
    public class clsSuccess
    {
        public string status { get; set; }
    }
    public class clsError
    {
        public string status { get; set; }
    }
    public class clsInvalidCredential
    {
        public string status { get; set; }
        public string attemp { get; set; }

    }
    public class clsBlocked
    {
        public string status { get; set; }
    }
    public class clsAnotherReason
    {
        public string status { get; set; }
    }
}
 


//
// TODO: Add constructor logic here
//

//INPUT 

//{
//    "OPCODE": "ENROLLMENT",
//    "MOBILENO": "4393745",
//    "COUNTRYCODE": "91",
//    "DEVICETYPE": "IOS/ANDROID",
//    "DEVICEID": "IDOFDEVICE"
//}

//INPUT 
//{
//    "OPCODE": "CHANGEMPIN",
//    "OLDPIN": "md5ofoldpin",
//    "NEWPIN": "md5ofnewpin",
//    "DEVICETYPE": "IOS/ANDROID",
//    "DEVICEID": "IDOFDEVICE",
//    "PUSHTOKEN" : "push token to send notification"
//}

//INPUT 
//{
//    "OPCODE": "LOGIN",
//    "MPIN": "md5ofpin",
//    "DEVICETYPE": "IOS/ANDROID",
//    "DEVICEID": "IDOFDEVICE",
//    "PUSHTOKEN" : "push token to send notification"

//}
//input
//{
//    "OPCODE": "ACCOUNTS",
//    "SESSIONID": "LOGGEDIN SESSION",
//    "DEVICETYPE": "IOS/ANDROID",
//    "DEVICEID": "IDOFDEVICE",
//    "CUSTOMERID": "custId"
//}

//input
//{
//    "OPCODE": "TRANSACTIONS",
//    "SESSIONID": "xxxxx",
//    "ACCOUNTNO": "91",
//    "ACCOUNTTYPE": "type",
//    "CUSTOMERID": "custId",
//    "DEVICETYPE": "IOS/ANDROID",
//    "DEVICEID": "IDOFDEVICE"
//    "LASTTRANSACTIONDATETIME" : ""
//}

//input
//{
//    "OPCODE": "ACCOUNTUPDATE",
//    "SESSIONID": "xxxxx",
//    "ACCOUNTNO": "91",
//    "ACCOUNTTYPE": "type",
//    "CUSTOMERID": "custId",
//    "DEVICETYPE": "IOS/ANDROID",
//    "DEVICEID": "IDOFDEVICE"
//}
//input

//{
//    "OPCODE": "BANNERS",
//    "CUSTOMERID": "custId",
//    "DEVICETYPE": "IOS/ANDROID",
//    "DEVICEID": "IDOFDEVICE"
//}


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     