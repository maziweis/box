using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FzBox.Implement
{
    public class CourseDecryptHandler : IHttpHandler
    {
        private const string _iv = "A$4^G@br";
        private const string _key = "Kings";
        private const string _fileKey = "KingsFEN";
        private const int FileEncryptStep = 2097152 * 2;//4M

        public void ProcessRequest(HttpContext context)
        {
            ///陈中意
            ///
            var Referre = context.Request.UrlReferrer;
            string url = "";
            if (Referre != null)
                url = Referre.ToString().ToLower();


            string filepath = context.Request.PhysicalPath;
            
            try
            {
                FileStream fsread = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                //if (fsread.Length > FileEncryptStep)
                //{
                //    context.Response.WriteFile(filepath);
                //    return;
                //}
                context.Response.Buffer = false;
                context.Response.AppendHeader("Content-Type", this.GetContentType(filepath));
                int count = 0;
                //context.Response.AppendHeader("Content-Length", fsread.Length.ToString());
                if (filepath.Contains("KingsunFiles"))
                {
                   
                    FileInfo DownloadFile = new FileInfo(filepath); //设置要下载的文件
                    string strFileName = DownloadFile.Name;
                    context.Response.Clear();                             //清除缓冲区流中的所有内容输出

                    context.Response.ClearHeaders();                      //清除缓冲区流中的所有头，不知道为什么，不写这句会显示错误页面

                    context.Response.Buffer = false;                      //设置缓冲输出为false
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.WriteFile(DownloadFile.FullName);
                    context.Response.Flush();        //向客户端发送当前所有缓冲的输出
                    context.Response.End();          //将当前所有缓冲的输出发送到客户端，这句户有时候会出错，可以尝试把这句话放在整个函数的
                    return;
                }
                if (fsread.Length > FileEncryptStep)
                {
                    context.Response.WriteFile(filepath);
                    return;
                }
                while (fsread.Position < fsread.Length)
                {
                    byte[] bytearrayinput = new byte[fsread.Length];
                    count = fsread.Read(bytearrayinput, 0, Convert.ToInt32(fsread.Length));

                    DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                    DES.Key = ASCIIEncoding.ASCII.GetBytes(_fileKey);
                    DES.IV = ASCIIEncoding.ASCII.GetBytes(_iv);
                    if (fsread.Position != fsread.Length)
                    {
                        DES.Padding = PaddingMode.None;
                    }
                    else
                    {
                        DES.Padding = PaddingMode.PKCS7;
                    }

                    ICryptoTransform desdecrypt = DES.CreateDecryptor();
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    CryptoStream cryptostreamDecr = new CryptoStream(context.Response.OutputStream, desdecrypt, CryptoStreamMode.Write);
                    cryptostreamDecr.Write(bytearrayinput, 0, count);
                    cryptostreamDecr.FlushFinalBlock();
                    cryptostreamDecr.Close();
                }

                context.Response.OutputStream.Flush();
                context.Response.OutputStream.Close();
                fsread.Close();
                return;
            }
            catch (Exception)
            {
                context.Response.StatusCode = 404;
                context.Response.SuppressContent = true;
                context.ApplicationInstance.CompleteRequest();
                return;
            }
        }
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
        private string GetContentType(string filePath)
        {
            string contentType = string.Empty;
            FileInfo file = new FileInfo(filePath);
            switch (file.Extension.ToLower())
            {
                case ".inf":
                    contentType = "text/html";
                    break;
                case ".ini":
                    contentType = "text/html";
                    break;
                case ".swf":
                    contentType = "application/x-shockwave-flash";
                    break;
                case ".exe":
                    contentType = "application/octet-stream";
                    break;
                case ".ocx":
                    contentType = "application/octet-stream";
                    break;
                case ".dll":
                    contentType = "application/octet-stream";
                    break;
                case ".htm":
                    contentType = "text/html";
                    break;
                case ".mp3":
                    contentType = "audio/mp3";
                    break;
                case ".xml":
                    contentType = "text/xml";
                    break;
                case ".ico":
                    contentType = "image/x-icon";
                    break;
                case ".fla":
                    contentType = "application/octet-stream";
                    break;
                case ".txt":
                    contentType = "text/html";
                    break;
                case ".jpg":
                    contentType = "image/jpeg";
                    break;
                case ".ppt":
                    contentType = "application/vnd.ms-powerpoint";
                    break;
                case ".dat":
                    contentType = "audio/mp3";
                    break;
                case ".ttf":
                    contentType = "application/octet-stream";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".db":
                    contentType = "application/octet-stream";
                    break;
                case ".mso":
                    contentType = "application/octet-stream";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".thmx":
                    contentType = "application/octet-stream";
                    break;
                case ".otf":
                    contentType = "application/octet-stream";
                    break;
                case ".flv":
                    contentType = "video/x-flv";
                    break;
                case ".bak":
                    contentType = "application/octet-stream";
                    break;
                case ".zip":
                    contentType = "application/x-zip-compressed";
                    break;
                case ".doc":
                    contentType = "application/msword";
                    break;
                case ".sun":
                    contentType = "application/octet-stream";
                    break;
                default:
                    break;
            }
            return contentType;
        }
    }
}
