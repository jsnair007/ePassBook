using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class TabBanking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        byte[] bZipFile = new byte[10];
        string sFileName="";

        try
        {
            UploadFile(bZipFile, sFileName);
            //string sInput = Request.QueryString.Get("INPUT");

            //sResponse = objPassBook.PassBookService(sInput);

            //Response.Write(sResponse);

        }
        catch (Exception eee)
        {
            clsEvent.LogExceptionEvent(eee.Message + " at : " + DateTime.Now.ToString());
           // sResponse = "116";
        }

    }

    public void UploadFile(byte[] byteArray, string filename)
    {
        FileStream targetStream = null;
        Stream sourceStream = new MemoryStream(byteArray);

        string uploadFolder = @"C:\upload\";
        string filePath = Path.Combine(uploadFolder, filename);

        using (targetStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            //read from the input stream in 6K chunks
            //and save to output stream
            const int bufferLen = 65000;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
            {
                targetStream.Write(buffer, 0, count);
            }
            targetStream.Close();
            sourceStream.Close();
        }
    }
}