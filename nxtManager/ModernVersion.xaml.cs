using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace nxtManager
{
    public partial class ModernVersion : ModernWindow
    {
        public ModernVersion()
        {
            App.DVM = new ViewModel();
            this.DataContext = App.DVM;

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
            App.DVM.PropertyChanged += DVM_PropertyChanged;
            startNXTServer();
        }

        void ModernVersion_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!App.DVM.IsShuttingDown)
            {
                var result = ModernDialog.ShowMessage("Do you really want to exit?", "Exit", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    App.DVM.IsShuttingDown = true;
                    stopNXTServer();
                    if (App.DVM.IsLoaded)
                    {
                        e.Cancel = true;
                        App.DVM.BusyMessage = "The application is closing. Please wait...";
                        App.DVM.IsLoaded = false;
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                ModernDialog.ShowMessage("The application is already shutting down. We are saving the block information. Please wait.", "Exitting", MessageBoxButton.OK);
                e.Cancel = true;
            }
        }

        public void startNXTServer()
        {
            try
            {
                Process NRSProcess = new Process();
                NRSProcess.StartInfo.FileName = "java";
                NRSProcess.StartInfo.Arguments = "-jar start.jar STOP.PORT=28282 STOP.KEY=BaiMangal";
                NRSProcess.StartInfo.UseShellExecute = false;
                NRSProcess.StartInfo.CreateNoWindow = true;
                NRSProcess.StartInfo.RedirectStandardOutput = true;
                NRSProcess.StartInfo.RedirectStandardError = true;
                NRSProcess.StartInfo.WorkingDirectory = "nxt";
                NRSProcess.EnableRaisingEvents = true;
                NRSProcess.Exited += NRSProcess_Exited;
                NRSProcess.OutputDataReceived += NRSProcess_OutputDataReceived;
                NRSProcess.ErrorDataReceived += NRSProcess_ErrorDataReceived;
                NRSProcess.Start();
                NRSProcess.BeginOutputReadLine();
                NRSProcess.BeginErrorReadLine();
            }
            catch (Exception e)
            {
                ModernDialog.ShowMessage(e.ToString(), "Error", MessageBoxButton.OK);
            }
        }

        string errorLog = String.Empty;
        void NRSProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && !e.Data.StartsWith("WARNING"))
                errorLog += e.Data + "\r\n";
        }

        void NRSProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            App.DVM.ConsoleOutput += e.Data + "\n";
        }

        void NRSProcess_Exited(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(errorLog) == false)
            {
                FileInfo logFile = new FileInfo(Environment.CurrentDirectory + @"\error.log");
                if (logFile.Exists)
                {
                    logFile.CopyTo(Environment.CurrentDirectory + @"\error.log.bak", true);
                }
                var writer = logFile.CreateText();
                writer.Write(errorLog);

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ModernDialog.ShowMessage("There was an exception in the NRS backend. The log was saved in: " + logFile.FullName, "Error", MessageBoxButton.OK);
                    Environment.Exit(0);
                }));
            }
            else
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ModernDialog.ShowMessage("There was an unidentified error in the NRS backend. Restart the application please.", "Error", MessageBoxButton.OK);
                    Environment.Exit(0);
                }));
            }
        }

        public void stopNXTServer()
        {
            try
            {
                Process StopNRSProcess = new Process();
                StopNRSProcess.StartInfo.FileName = "java";
                StopNRSProcess.StartInfo.Arguments = "-jar start.jar STOP.PORT=28282 STOP.KEY=BaiMangal --stop";
                StopNRSProcess.StartInfo.UseShellExecute = false;
                StopNRSProcess.StartInfo.CreateNoWindow = true;
                StopNRSProcess.StartInfo.WorkingDirectory = "nxt";
                StopNRSProcess.Start();
            }
            catch (Exception e)
            {
                ModernDialog.ShowMessage(e.ToString(), "Error", MessageBoxButton.OK);
            }
        }
    }
}
