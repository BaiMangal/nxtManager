using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using nxtAPIwrapper;
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
            string err = String.Empty;
            App.DVM.IsBusy = true;

            var aliasText = alias.Text;
            var uriText = uri.Text;
            var feeText = fee.Text;
            var deadlineText = deadline.Text;

            //Create Alias
            var createAliasTask = Task.Factory
                .StartNew(() => App.DVM.NXTApi.CreateAlias(
                        App.DVM.NXTAccSecureString,
                        aliasText,
                        uriText,
                        feeText,
                        deadlineText,
                        ref err))
                .ContinueWith(task =>
                {
                    if (task.Result.errorCode != null)
                    {
                        App.DVM.IsBusy = false;
                        App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ModernDialog.ShowMessage(task.Result.errorDescription, "Error", MessageBoxButton.OK);
                        }));
                    }
                    else
                    {
                        App.DVM.IsBusy = false;
                        App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ModernDialog.ShowMessage("The transaction was successfull.\nPlease note that aliases take some time to update.", "Success", MessageBoxButton.OK);
                        }));
                    }
                });
        }

        private void aliasToQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            var result = GetAliasURI(aliasToQuery.Text);
            if (String.IsNullOrEmpty(result.errorCode))
                noResolvedURI.Visibility = Visibility.Collapsed;
            else
                noResolvedURI.Visibility = Visibility.Visible;
            resolvedURI.Text = result.uri;
        }

        private void aliasToCreate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(alias.Text))
            {
                aliasFreeImage.Visibility = Visibility.Collapsed;
                aliasNotFreeImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                var result = GetAliasURI(alias.Text);
                if (String.IsNullOrEmpty(result.errorCode))
                {
                    aliasFreeImage.Visibility = Visibility.Collapsed;
                    aliasNotFreeImage.Visibility = Visibility.Visible;
                }
                else
                {
                    aliasFreeImage.Visibility = Visibility.Visible;
                    aliasNotFreeImage.Visibility = Visibility.Collapsed;
                }
            }
        }

        public AliasURI GetAliasURI(string alias)
        {
            string err = String.Empty;
            var result = new NXTApi().GetAliasURI(alias, ref err);
            return result;
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            App.DVM.IsAliasPageOpened = true;
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            App.DVM.IsAliasPageOpened = false;
        }

    }
}
