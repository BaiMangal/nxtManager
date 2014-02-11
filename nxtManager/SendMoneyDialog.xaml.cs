using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
            get { return DateTime.Now.AddHours(deadline); }
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

        public SendMoneyDialog()
        {
            this.DataContext = this;
            InitializeComponent();
            this.Buttons = new Button[] { };
        }

        private void TransferFunds(object sender, RoutedEventArgs e)
        {
            var result = App.DVM.SendMoney(Recipient, Amount, Fee, Deadline);
            if (result.errorCode != null)
            {
                string errorMessage = "";
                switch (result.errorCode)
                {
                    case "1": errorMessage = "Incorrect request"; break;
                    case "2": errorMessage = "Blockchain not up to date"; break;
                    case "3": errorMessage = "Parameter not specified"; break;
                    case "4": errorMessage = "Incorrect parameter"; break;
                    case "5": errorMessage = "Unknown object (block, transaction, etc.)"; break;
                    case "6": errorMessage = "Not enough funds"; break;
                }

                DialogResult = false;
                ModernDialog.ShowMessage(errorMessage, "Error", MessageBoxButton.OK);
            }
            else
            {
                DialogResult = true;
                ModernDialog.ShowMessage("The transaction was successfull", "Success", MessageBoxButton.OK);
            }
            this.Close();
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
