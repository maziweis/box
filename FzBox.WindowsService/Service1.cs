using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceProcess;

namespace FzBox.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer timer2 = new System.Timers.Timer();

        public Service1()
        {
            InitializeComponent();

            timer.Elapsed += new System.Timers.ElapsedEventHandler(UpLogTimedEvent);
            timer.Interval = 60 * 1000;


            timer2.Elapsed += new System.Timers.ElapsedEventHandler(AutoUpdateTimedEvent);
            timer2.Interval = 60 * 1000;
        }

        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
            timer.Start();

            timer2.Enabled = true;
            timer2.Start();
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            timer.Stop();

            timer2.Enabled = false;
            timer2.Stop();
        }

        #region "上传操作日志"
        private void UpLogTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            int hour = DateTime.Now.Hour;
            if (hour == 0)//凌晨0点
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\FzBox.OperationLog.exe";
                Process p = Process.Start(path);
                p.WaitForExit();
                string clearpath = AppDomain.CurrentDomain.BaseDirectory + "\\FzBox.ClearData.exe";
                Process p1 = Process.Start(clearpath);
                p1.WaitForExit();
            }
        }
        #endregion

        #region "自动更新"
        private void AutoUpdateTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            int h = DateTime.Now.Hour;
            //凌晨0点 + 该程序此刻没有运行 + 该程序没有运行过 满足3个条件才执行以下代码
            if (h == 0 && !IsRun("FzBox.AutoUpdate") && !File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\record\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt"))
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\FzBox.AutoUpdate.exe";
                Process p = Process.Start(path);
                p.WaitForExit();
                Write();//记录该程序运行过
            }
        }
        #endregion

        #region 写入文件
        public void Write()
        {
            string _url = AppDomain.CurrentDomain.BaseDirectory + "/record";
            if (!Directory.Exists(_url))
            {
                Directory.CreateDirectory(_url);
            }
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\record\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(DateTime.Now + "——程序执行成功!");
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }
        #endregion

        #region 判断控制台程序是否已经运行
        /// <summary>
        /// 判断控制台程序是否已经运行
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsRun(string name)
        {
            if (GetPidByProcessName(name).Count > 1)//表明已经有程序在运行
                return true;
            return false;
        }

        public List<int> GetPidByProcessName(string processName)
        {
            List<int> _list = new List<int>();

            Process[] arrayProcess = Process.GetProcessesByName(processName);
            foreach (Process p in arrayProcess)
            { _list.Add(p.Id); }
            return _list;
        }
        #endregion

    }
}