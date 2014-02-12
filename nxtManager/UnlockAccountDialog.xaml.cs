using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace nxtManager
{
    public partial class UnlockAccountDialog : ModernDialog, INotifyPropertyChanged
    {
        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; NotifyPropertyChanged("IsBusy"); }
        }
        private AccountCreationType accountCreationType;
        public AccountCreationType AccountCreationType
        {
            get { return accountCreationType; }
            set { accountCreationType = value; NotifyPropertyChanged("AccountCreationType"); }
        }

        public UnlockAccountDialog(AccountCreationType accountCreationType)
        {
            AccountCreationType = accountCreationType;
            this.DataContext = this;
            App.DVM.PropertyChanged += DVM_PropertyChanged;
            InitializeComponent();
            this.Buttons = new Button[] { };

            if (AccountCreationType == AccountCreationType.Create)
            {
                this.Title = "Create account";
                this.unlockAccountButton.Content = "Create account";
            }
            else if (AccountCreationType == AccountCreationType.Unlock)
            {
                this.Title = "Unlock account";
                this.unlockAccountButton.Content = "Unlock account";
            }
        }

        void DVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsAccountUnlockedAndLoaded" && App.DVM.IsAccountUnlockedAndLoaded)
            {
                App.DVM.PropertyChanged -= DVM_PropertyChanged;

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    IsBusy = false;
                    this.Close();
                }));
            }
        }

        private void Unlock(object sender, RoutedEventArgs e)
        {
            IsBusy = true;

            if (SecretPhrase.SecurePassword.Length < 30)
            {
                ModernDialog.ShowMessage("You should use a secret phrase that is at least 30 symbols.\n" +
                    "Anything shorter WILL get hacked an you will loose your NXT",
                    "Secret too short", MessageBoxButton.OK);
                if (AccountCreationType == AccountCreationType.Create)
                {
                    IsBusy = false;
                    return;
                }
            }

            App.DVM.UnlockAccount(SecretPhrase.SecurePassword);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsBusy && !App.DVM.IsAccountUnlockedAndLoaded)
                e.Cancel = true;

            base.OnClosing(e);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #endregion
    }

    public enum AccountCreationType
    {
        Create,
        Unlock
    }
}
