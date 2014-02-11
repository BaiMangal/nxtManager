using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using System.Windows.Controls;

namespace nxtManager
{
    public partial class UnlockAccountDialog : ModernDialog
    {
        public UnlockAccountDialog()
        {
            InitializeComponent();
            this.Buttons = new Button[] { };
        }

        private void Unlock(object sender, RoutedEventArgs e)
        {
            App.DVM.UnlockAccount(SecretPhrase.SecurePassword);
            DialogResult = true;
            this.Close();
        }
    }
}
