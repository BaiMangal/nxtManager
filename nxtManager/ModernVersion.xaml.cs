using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace nxtManager
{
    /// <summary>
    /// Interaction logic for ModernVersion.xaml
    /// </summary>
    public partial class ModernVersion : ModernWindow
    {
        public ModernVersion()
        {
            App.DVM = new ViewModel();
            this.DataContext = App.DVM;
            App.DVM.PropertyChanged += DVM_PropertyChanged;

            InitializeComponent();

            this.Loaded += ModernVersion_Loaded;
            this.Closing += ModernVersion_Closing;
        }

        void DVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoaded" && App.DVM.IsLoaded)
                ContentSource = new Uri("/Pages/AccountAndTransactions.xaml", UriKind.Relative);
            if (e.PropertyName == "IsLoaded" && !App.DVM.IsLoaded)
                ContentSource = new Uri("/Pages/Console.xaml", UriKind.Relative);
            if (e.PropertyName == "NXTApiState")
                MenuLinks.DisplayName = "NXT Manager (beta) - NRS: " + App.DVM.NXTApiState.version;
        }

        void ModernVersion_Loaded(object sender, RoutedEventArgs e)
        {
            startNXTServer();
        }

        void ModernVersion_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stopNXTServer();
            if (App.DVM.IsLoaded)
            {
                e.Cancel = true;
                App.DVM.BusyMessage = "The application is closing. Please wait...";
                App.DVM.IsLoaded = false;
            }
        }

        public void startNXTServer()
        {
            try
            {
                createStartFile();
                consoleControl.StartProcess("cmd.exe", "/k start_nxt_auto.bat");
            }
            catch (Exception e)
            {
                ModernDialog.ShowMessage(e.ToString(), "Error", MessageBoxButton.OK);
            }
        }

        public void stopNXTServer()
        {
            try
            {
                createStopFile();

                Process newProcess = new Process();
                newProcess.StartInfo.FileName = "cmd.exe";
                newProcess.StartInfo.Arguments = "/k stop_nxt_auto.bat";
                newProcess.StartInfo.UseShellExecute = false;
                newProcess.StartInfo.CreateNoWindow = true;
                newProcess.StartInfo.RedirectStandardOutput = true;
                newProcess.Start();
            }
            catch (Exception e)
            {
                ModernDialog.ShowMessage(e.ToString(), "Error", MessageBoxButton.OK);
            }
        }

        public void createStartFile()
        {
            if (!File.Exists("start_nxt_auto.bat"))
            {
                string[] lines = { "cd nxt", "java -jar start.jar STOP.PORT=28282 STOP.KEY=BaiMangal" };
                File.WriteAllLines("start_nxt_auto.bat", lines);
            }
        }
        public void createStopFile()
        {
            if (!File.Exists("stop_nxt_auto.bat"))
            {
                string[] lines = { "cd nxt", "java -jar start.jar STOP.PORT=28282 STOP.KEY=BaiMangal --stop", "exit" };
                File.WriteAllLines("stop_nxt_auto.bat", lines);
            }
        }
    }
}
