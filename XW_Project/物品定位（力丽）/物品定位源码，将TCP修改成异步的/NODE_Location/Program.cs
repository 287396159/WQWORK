using CiXinLocation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //添加非UI上的异常.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //处理UI线程异常   
            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;

                WriteLog( DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")+" : "+ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show(" Error",
                        " Could not write the error to the log. Reason: "
                        + exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }

        static void WriteLog(string str)
        {

            FileModel.getFlModel().setErrorAppData("\r\nProgram.WriteLog" + str);
            /*if (!Directory.Exists("ErrLog"))
            {
                Directory.CreateDirectory("ErrLog");
            }
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "ErrLog.txt";
            using (var sw = new StreamWriter(@"ErrLog\" + fileName, true))
            {
                sw.WriteLine("***********************************************************************");
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                sw.WriteLine(str);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }*/
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                FileModel.getFlModel().setErrorAppData("\r\nApplication_ThreadException" + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                Console.Write("Application_ThreadException", ex);
               // LogNet.Log.WriteLog("Application_ThreadException", ex);
            }

            //MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")));
        }



    }
}
