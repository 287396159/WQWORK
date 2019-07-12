using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace PersionAutoLocaSys
{
    static class Program
    {
        private static Form1 frm = null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                //设置应用的异常处理方式：异常捕获
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //设置出现未处理异常的委托
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                frm = new Form1();
                Application.Run(frm);
            }catch(Exception ex)
            {
                string str = GetExceptionMsg(ex, string.Empty);
                MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileOperation.WriteLog(DateTime.Now.ToString() + "\r\n" + str);
                if (null != frm && null != frm.MyParamSet)
                {
                    SysParam.SaveAllInfor();
                }
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            FileOperation.WriteLog(DateTime.Now.ToString() + "\r\n" + str);
            //查看若发现异常是在设置软体上出现的，此时是需要我们做数据保存
            if (null != frm.MyParamSet)
            {
                SysParam.SaveAllInfor();
            }
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            FileOperation.WriteLog(DateTime.Now.ToString() + "\r\n" + str);
            //查看若发现异常是在设置软体上出现的，此时是需要我们做数据保存
            if (null != frm.MyParamSet)
            {
                SysParam.SaveAllInfor();
            }
        }

        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
    }
}
