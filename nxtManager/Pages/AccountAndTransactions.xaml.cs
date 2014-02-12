using FirstFloor.ModernUI.Windows;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AccountAndTransactions.xaml
    /// </summary>
    public partial class AccountAndTransactions : Page, IContent
    {
        public AccountAndTransactions()
        {
            InitializeComponent();
            this.SizeChanged += AccountAndTransactions_SizeChanged;
        }

        void AccountAndTransactions_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (LayboutGrid.ActualHeight > TopStackPanel.ActualHeight)
                RecentTransactionsListBox.Height = LayboutGrid.ActualHeight - TopStackPanel.ActualHeight;
        }

        void AccountAndTransactions_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UnlockAccount(object sender, RoutedEventArgs e)
        {
            UnlockAccountDialog uad = new UnlockAccountDialog(AccountCreationType.Unlock);
            var result = uad.ShowDialog();
        }

        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            UnlockAccountDialog uad = new UnlockAccountDialog(AccountCreationType.Create);
            var result = uad.ShowDialog();
        }

        private void SendMoney(object sender, RoutedEventArgs e)
        {
            SendMoneyDialog smd = new SendMoneyDialog();
            var result = smd.ShowDialog();
        }

        private void LockAccount(object sender, RoutedEventArgs e)
        {
            App.DVM.LockAccount();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            App.DVM.IsAccountPageOpened = true;
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            App.DVM.IsAccountPageOpened = false;
        }
    }
}
