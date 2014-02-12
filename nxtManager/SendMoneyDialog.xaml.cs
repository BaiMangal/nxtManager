using FirstFloor.ModernUI.Windows.Controls;
using nxtAPIwrapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace nxtManager
{
    public partial class SendMoneyDialog : ModernDialog, INotifyPropertyChanged
    {
        private double deadline = 900;
        [Range(1, 900, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Deadline
        {
            get { return deadline; }
            set { deadline = value; NotifyPropertyChanged("Deadline"); NotifyPropertyChanged("DeadlineInDateTime"); }
        }

        public DateTime DeadlineInDateTime
        {
            get { return DateTime.Now.AddMinutes(deadline); }
        }

        private double fee = 1;
        [Range(1, Double.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Fee
        {
            get { return fee; }
            set { fee = value; NotifyPropertyChanged("Fee"); }
        }

        private double amount;
        [Range(1, Double.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Amount
        {
            get { return amount; }
            set { amount = value; NotifyPropertyChanged("Amount"); }
        }

        private string recipient;
        public string Recipient
        {
            get { return recipient; }
            set { recipient = value; NotifyPropertyChanged("Recipient"); }
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; NotifyPropertyChanged("IsBusy"); }
        }

        public SendMoneyDialog()
        {
            this.DataContext = this;
            InitializeComponent();
            this.Buttons = new Button[] { };
        }

        private void TransferFunds(object sender, RoutedEventArgs e)
        {
            string err = String.Empty;
            IsBusy = true;

            //Send Money
            var sendMoneyTask = Task.Factory
                .StartNew(() => App.DVM.NXTApi.SendMoney(
                    App.DVM.NXTAccSecureString,
                    Recipient,
                    Amount.ToString(), ref err,
                    Fee.ToString(),
                    Deadline.ToString()))
                .ContinueWith(task =>
                {
                    if (task.Result.errorCode != null)
                    {
                        string errorMessage = "";
                        switch (task.Result.errorCode)
                        {
                            case "1": errorMessage = "Incorrect request"; break;
                            case "2": errorMessage = "Blockchain not up to date"; break;
                            case "3": errorMessage = "Parameter not specified"; break;
                            case "4": errorMessage = "Incorrect parameter"; break;
                            case "5": errorMessage = "Unknown object (block, transaction, etc.)"; break;
                            case "6": errorMessage = "Not enough funds"; break;
                        }

                        App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            IsBusy = false;
                            this.Close();
                            ModernDialog.ShowMessage(errorMessage, "Error", MessageBoxButton.OK);
                        }));
                    }
                    else
                    {
                        App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            IsBusy = false;
                            this.Close();
                            ModernDialog.ShowMessage("The transaction was successfull", "Success", MessageBoxButton.OK);
                        }));

                    }
                });


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
