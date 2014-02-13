using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace nxtManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ViewModel DVM { get; set; }

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            FileInfo logFile = new FileInfo(Environment.CurrentDirectory + @"\error.log");
            if (logFile.Exists)
            {
                logFile.CopyTo(Environment.CurrentDirectory + @"\error.log.bak", true);
            }
            var writer = logFile.CreateText();

            StringBuilder message = new StringBuilder();
            message.AppendLine("----------------------");
            message.AppendLine(DateTime.Now.ToString());
            message.AppendLine();
            if (e.ExceptionObject != null && e.ExceptionObject is Exception)
            {
                var ex = e.ExceptionObject as Exception;
                message.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    message.AppendLine(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        message.AppendLine(ex.InnerException.InnerException.Message);
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            message.AppendLine(ex.InnerException.InnerException.InnerException.Message);
                        }
                    }
                }
            }
            message.AppendLine("----------------------");
            writer.Write(message.ToString());
            writer.Close();
        }
    }
}
