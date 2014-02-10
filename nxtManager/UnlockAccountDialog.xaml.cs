using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Interaction logic for UnlockAccountDialog.xaml
    /// </summary>
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
