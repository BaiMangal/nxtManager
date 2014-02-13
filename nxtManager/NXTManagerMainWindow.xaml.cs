using FirstFloor.ModernUI.Windows.Controls;
using nxtAPIwrapper;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace nxtManager
{
    public partial class NXTManagerMainWindow : ModernWindow
    {
        public NXTManagerMainWindow()
        {
            string err = String.Empty;
            var nxtApiState = new NXTApi().GetState(ref err);
            if (nxtApiState.version != null)
                App.DVM = new ViewModel(true);
            else
                App.DVM = new ViewModel();

            this.DataContext = App.DVM;

            InitializeComponent();

            this.Loaded += NXTManagerMainWindow_Loaded;
            this.Closing += NXTManagerMainWindow_Closing;
        }

        void DVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoaded" && App.DVM.IsLoaded)
                ContentSource = new Uri("/Pages/AccountAndTransactions.xaml", UriKind.Relative);
            if (e.PropertyName == "IsLoaded" && !App.DVM.IsLoaded)
                ContentSource = new Uri("/Pages/Console.xaml", UriKind.Relative);
            if (e.PropertyName == "NXTApiState")
                MenuLinks.DisplayName = "NXT Manager (beta) - NRS: " + App.DVM.NXTApiState.version;
            if (e.PropertyName == "IsAPIConnected" && !App.DVM.IsAPIConnected && App.DVM.ExternalNRSActive)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!nrsExitedMessageShown)
                    {
                        nrsExitedMessageShown = true;
                        App.DVM.LockAccount();
                        startNXTServer();
                        App.DVM.IsLoaded = false;
                        ModernDialog.ShowMessage("The external NRS that you were using has stopped. The application must now start the built in one.", "Error", MessageBoxButton.OK);

                        MenuLinks.Links.Add(consoleLink);
                    }
                }));
            }
        }

        void NXTManagerMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.DVM.PropertyChanged += DVM_PropertyChanged;
            if (!App.DVM.ExternalNRSActive)
            {
                startNXTServer();
            }
            else
            {
                var result = ModernDialog.ShowMessage("You are running an external NRS Backend. Note that if you stop it the nxtManager will crash.", "External NRS backend detected", MessageBoxButton.OK);
                MenuLinks.Links.Remove(consoleLink);
            }
        }

        void NXTManagerMainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!App.DVM.IsShuttingDown)
            {
                var result = ModernDialog.ShowMessage("Do you really want to exit?", "Exit", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    App.DVM.IsShuttingDown = true;
                    if (!App.DVM.ExternalNRSActive)
                    {
                        stopNXTServer();
                        if (App.DVM.IsLoaded)
                        {
                            e.Cancel = true;
                            App.DVM.BusyMessage = "The application is closing. Please wait...";
                            App.DVM.IsLoaded = false;
                        }
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

        string javaExec = "java";
        public void startNXTServer()
        {
            try
            {
                Process info = new Process();
                info.StartInfo.FileName = javaExec;
                info.StartInfo.Arguments = " -version";
                info.StartInfo.UseShellExecute = false;
                info.StartInfo.CreateNoWindow = true;
                info.Start();
            }
            catch (Exception)
            {
                javaExec = @"Java\bin\java.exe";

                try
                {
                    Process info = new Process();
                    info.StartInfo.FileName = javaExec;
                    info.StartInfo.Arguments = " -version";
                    info.StartInfo.UseShellExecute = false;
                    info.StartInfo.CreateNoWindow = true;
                    info.Start();
                }
                catch (Exception)
                {
                    ModernDialog.ShowMessage("You need the Java Runtime Environment in order to run the NRS Backend.", "Error", MessageBoxButton.OK);
                    Environment.Exit(0);
                }
            }

            try
            {
                Process NRSProcess = new Process();
                NRSProcess.StartInfo.FileName = javaExec;
                NRSProcess.StartInfo.Arguments = " -jar start.jar STOP.PORT=28282 STOP.KEY=BaiMangal";
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
                ModernDialog.ShowMessage("There was an error starting the NRS Backend. \r\n\r\nDetails:\r\n" + e.ToString(), "Error", MessageBoxButton.OK);
                Environment.Exit(0);
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

        bool nrsExitedMessageShown = false;
        void NRSProcess_Exited(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(errorLog) == false)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ModernDialog.ShowMessage("There was an exception in the NRS backend. The log was saved in: " + Environment.CurrentDirectory + @"\error.log", "Error", MessageBoxButton.OK);
                    throw new Exception(errorLog);
                }));
            }
            else
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (App.DVM.IsShuttingDown)
                        Environment.Exit(0);
                    else
                        throw new Exception("The NRS process ended unexpectedly.");
                }));
            }
        }

        public void stopNXTServer()
        {
            try
            {
                Process StopNRSProcess = new Process();
                StopNRSProcess.StartInfo.FileName = javaExec;
                StopNRSProcess.StartInfo.Arguments = " -jar start.jar STOP.PORT=28282 STOP.KEY=BaiMangal --stop";
                StopNRSProcess.StartInfo.UseShellExecute = false;
                StopNRSProcess.StartInfo.CreateNoWindow = true;
                StopNRSProcess.StartInfo.WorkingDirectory = "nxt";
                StopNRSProcess.Start();
            }
            catch (Exception e)
            {
                ModernDialog.ShowMessage("There was an error stopping the NRS Backend. \r\n\r\nDetails:\r\n" + e.ToString(), "Error", MessageBoxButton.OK);
                Environment.Exit(0);
            }
        }
    }
}
