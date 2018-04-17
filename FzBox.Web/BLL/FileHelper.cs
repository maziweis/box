using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FzBox.Web.BLL
{
    public class FileHelper
    {
        private const string _iv = "A$4^G@br";
        private const string _key = "Kings";
        private const string _fileKey = "KingsFEN";
        public const int FileEncryptStep = 2097152 * 20;//40M

        /// <summary>
        /// 文件解密
        /// </summary>
        /// <param name="inputFilePath">密文输入文件路径</param>
        /// <param name="outputPath">明文输出文件路径</param>
        /// <param name="message">消息</param>
        /// <returns>是否成功</returns>
        public static bool DecryptFile(string inputFilePath, string outputPath)
        {
            try
            {
                string _fileurl = HttpContext.Current.Server.MapPath("/Resources/DecFile/");
                if (!Directory.Exists(_fileurl))
                {
                    Directory.CreateDirectory(_fileurl);
                }
                FileStream fsread = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                File.Delete(outputPath);
                int count = 0;
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
                    FileStream writerS = new FileStream(outputPath, FileMode.Append);
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    CryptoStream cryptostreamDecr = new CryptoStream(writerS, desdecrypt, CryptoStreamMode.Write);
                    cryptostreamDecr.Write(bytearrayinput, 0, count);
                    cryptostreamDecr.FlushFinalBlock();
                    writerS.Flush();
                    cryptostreamDecr.Close();
                    writerS.Close();
                }
                fsread.Close();
                return true;
            }
            catch (Exception e)
            {
                //message = "失败！提示:" + e.Message;
                return false;
            }
        }

        public static void deletefile(string outputPath)
        {
            string _fileurl = HttpContext.Current.Server.MapPath("/Resources/DecFile/");
            if (Directory.Exists(_fileurl))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(_fileurl);
                    FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    foreach (FileSystemInfo i in fileinfo)
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                        }
                        else
                        {
                            File.Delete(i.FullName);      //删除指定文件
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}