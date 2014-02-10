using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
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
    /// Interaction logic for Aliases.xaml
    /// </summary>
    public partial class Aliases : Page, IContent
    {
        public Aliases()
        {
            InitializeComponent();
        }

        private void CreateAlias(object sender, RoutedEventArgs e)
        {
            var result = App.DVM.CreateAlias(alias.Text, uri.Text, fee.Text, deadline.Text);
            if (result.errorCode != null)
            {
                ModernDialog.ShowMessage(result.errorDescription, "Error", MessageBoxButton.OK);
            }
            else
            {
                ModernDialog.ShowMessage("The transaction was successfull", "Success", MessageBoxButton.OK);
            }
        }

        private void aliasToQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            var result = App.DVM.GetAliasURI(aliasToQuery.Text);
            if (String.IsNullOrEmpty(result.errorCode))
                noResolvedURI.Visibility = Visibility.Collapsed;
            else
                noResolvedURI.Visibility = Visibility.Visible;
            resolvedURI.Text = result.uri;
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
        }
    }
}
