using FirstFloor.ModernUI.Windows;
using nxtAPIwrapper;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace nxtManager.Pages
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : Page, IContent
    {
        public Console()
        {
            InitializeComponent();
            this.Loaded += Console_Loaded;
            this.Unloaded += Console_Unloaded;
        }

        private void Console_Loaded(object sender, RoutedEventArgs e)
        {
            ModernVersion m_mainform = (ModernVersion)Application.Current.MainWindow;
            m_mainform.consoleControl.OnProcessOutput += ConsoleControl_OnProcessOutput;

            var consoleOutput = m_mainform.consoleControl.Content as RichTextBox;
            TextRange textRange = new TextRange(
                consoleOutput.Document.ContentStart,
                consoleOutput.Document.ContentEnd
            );
            App.DVM.ConsoleOutput = textRange.Text;
        }

        void Console_Unloaded(object sender, RoutedEventArgs e)
        {
            ModernVersion m_mainform = (ModernVersion)Application.Current.MainWindow;
            m_mainform.consoleControl.OnProcessOutput -= ConsoleControl_OnProcessOutput;
        }

        void ConsoleControl_OnProcessOutput(object sender, ConsoleControlAPI.ProcessEventArgs args)
        {
            ModernVersion m_mainform = (ModernVersion)Application.Current.MainWindow;
            if (args.Content.Trim().Contains("started successfully"))
            {
                //m_mainform.ContentSource = new Uri("/Pages/AccountAndTransactions.xaml", UriKind.Relative);
            }
            else if (args.Content.Trim().Contains("stopped."))
            {
                Application.Current.Shutdown();
            }
            else
            {
                if (m_mainform != null && m_mainform.consoleControl != null)
                {
                    var consoleOutput = m_mainform.consoleControl.Content as RichTextBox;
                    TextRange textRange = new TextRange(
                        consoleOutput.Document.ContentStart,
                        consoleOutput.Document.ContentEnd
                    );
                    App.DVM.ConsoleOutput = textRange.Text;
                }
            }
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (App.DVM == null || !App.DVM.IsLoaded)
                e.Cancel = true;
        }
    }
}
