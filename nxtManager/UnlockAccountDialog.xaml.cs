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

        public UnlockAccountDialog()
        {
            this.DataContext = this;
            App.DVM.PropertyChanged += DVM_PropertyChanged;
            InitializeComponent();
            this.Buttons = new Button[] { };
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
}
