using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SILDMS.Utillity
{
    public static class FolderGenerator
    {
        public static bool Generate(string ownerLevel, string owner, string docCat, string docType, string docPro)
        {
            //path = "";
            //if(string.IsNullOrEmpty(ownerLevel) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(docCat) || string.IsNullOrEmpty(docType) || string.IsNullOrEmpty(docPro))
            //{ return false; }
            ////path = System.Configuration.ConfigurationManager.AppSettings["Path"].Trim() + ownerLevel.Trim() + "\\" + owner.Trim() + "\\" + docCat.Trim() + "\\" + docType.Trim() + "\\" + docPro.Trim(); // your code goes here;
            //path = "ftp://172.16.201.74/" + ownerLevel.Trim() + "\\" + owner.Trim() + "\\" + docCat.Trim() + "\\" + docType.Trim() + "\\" + docPro.Trim(); // your code goes here;
            //if (!Directory.Exists(path))
            //{
            //    DirectoryInfo di = Directory.CreateDirectory(path);              
            //}            
            //return true;

            WebRequest request = WebRequest.Create("ftp://172.16.201.74/" + ownerLevel.Trim() +
                                                   "\\" + owner.Trim() + "\\" + docCat.Trim() +
                                                   "\\" + docType.Trim() + "\\" + docPro.Trim());
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential("SILU2/badhon", "12345");
            using (var resp = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine(resp.StatusCode);
            }
            return true;
        }
        public static void MakeFTPDir(string ftpAddress,string port, string pathToCreate, string login, string password)
        {
            FtpWebRequest reqFTP = null;
            Stream ftpStream = null;

            string[] subDirs = pathToCreate.Split('/');

            string currentDir = string.Format("ftp://{0}:"+port, ftpAddress);

            foreach (string subDir in subDirs)
            {
                try
                {
                    currentDir = currentDir + "/" + subDir;
              

                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(currentDir);
                    reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                    reqFTP.UseBinary = true;
                    reqFTP.Credentials = new NetworkCredential(login, password);
                    reqFTP.Proxy = new WebProxy(); 
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    ftpStream = response.GetResponseStream();
                    ftpStream.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    continue;
                    //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                }
            }
        }
        //public static void MakeFTPDir(string ftpAddress, string pathToCreate, string login, string password)
        //{
        //    FtpWebRequest reqFTP = null;
        //    Stream ftpStream = null;

        //    string[] subDirs = pathToCreate.Split('/');

        //    string currentDir = string.Format("ftp://{0}", ftpAddress);

        //    foreach (string subDir in subDirs)
        //    {
        //        try
        //        {
        //            currentDir = currentDir + "/" + subDir;
        //            reqFTP = (FtpWebRequest) FtpWebRequest.Create(currentDir);
        //            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
        //            reqFTP.UseBinary = true;
        //            reqFTP.Credentials = new NetworkCredential(login, password);
        //            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
        //            ftpStream = response.GetResponseStream();
        //            ftpStream.Close();
        //            response.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
        //        }
        //    }
        //}
    }
}
