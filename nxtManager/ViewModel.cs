using nxtAPIwrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace nxtManager
{
    public class ViewModel : INotifyPropertyChanged
    {
        public static Boolean IsExternalAPIBackendUsed = false;
        public static DateTime GenesisBlockTime = new DateTime(2013, 10, 24, 12, 0, 0, 0);

        #region Update Peers Page

        private bool isPeersPageOpened;
        public bool IsPeersPageOpened
        {
            get
            {
                return isPeersPageOpened;
            }
            set
            {
                isPeersPageOpened = value;
                PeersUpdateTimer.Change(500, Timeout.Infinite);
                NotifyPropertyChanged("IsPeersPageOpened");
            }
        }
        public Timer PeersUpdateTimer = new Timer(PeersUpdateTimerCallback);
        private static void PeersUpdateTimerCallback(object state)
        {
            if (App.Current != null && App.Current.Dispatcher != null)
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (App.DVM != null && !App.DVM.IsShuttingDown && App.DVM.IsPeersPageOpened)
                    {
                        var tasks = App.DVM.UpdatePeers();
                        Task.Factory.ContinueWhenAll(tasks, (task) => App.DVM.PeersUpdateTimer.Change(0, Timeout.Infinite));
                    }
                }));
        }

        public Task[] UpdatePeers()
        {
            string err = String.Empty;
            //Get Active Peers
            var getActivePeers = Task.Factory
                .StartNew(() => App.DVM.NXTApi.GetDetailedActivePeers(ref err))
                .ContinueWith(task => App.DVM.NXTActivePeers = new ObservableCollection<Peer>(task.Result));

            //Get Well Known Peers
            var getWellKnownPeers = Task.Factory
                .StartNew(() => App.DVM.NXTApi.GetWellKnownPeers(App.DVM.WellKnownPeersList, ref err))
                .ContinueWith(task => App.DVM.NXTWellKnownPeers = new ObservableCollection<Peer>(task.Result));

            return new Task[] { getActivePeers, getWellKnownPeers };
        }

        #endregion Update Peers Page

        #region Update Blocks Page

        private bool isBlocksPageOpened;
        public bool IsBlocksPageOpened
        {
            get
            {
                return isBlocksPageOpened;
            }
            set
            {
                isBlocksPageOpened = value;
                BlocksUpdateTimer.Change(500, Timeout.Infinite);
                NotifyPropertyChanged("IsBlocksPageOpened");
            }
        }
        public Timer BlocksUpdateTimer = new Timer(BlocksUpdateTimerCallback);
        private static void BlocksUpdateTimerCallback(object state)
        {
            if (App.Current != null && App.Current.Dispatcher != null)
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (App.DVM != null && !App.DVM.IsShuttingDown && App.DVM.IsBlocksPageOpened)
                    {
                        var tasks = App.DVM.UpdateBlocks();
                        Task.Factory.ContinueWhenAll(tasks, (task) => App.DVM.PeersUpdateTimer.Change(20000, Timeout.Infinite));
                    }
                }));
        }

        public Task[] UpdateBlocks()
        {
            string err = String.Empty;
            //Get Blocks
            var getRecentBlocksTask = Task.Factory
                .StartNew(() => App.DVM.NXTApi.GetLatestBlocks(60, ref err))
                .ContinueWith(task => App.DVM.NXTRecentBlocks = new ObservableCollection<Block>(task.Result))
                .ContinueWith(task => App.DVM.BlocksUpdateTimer.Change(20000, Timeout.Infinite));

            return new Task[] { getRecentBlocksTask };
        }

        #endregion Update Blocks Page

        #region Update Account Page

        private bool isAccountPageOpened;
        public bool IsAccountPageOpened
        {
            get
            {
                return isAccountPageOpened;
            }
            set
            {
                isAccountPageOpened = value;
                AccountUpdateTimer.Change(1000, Timeout.Infinite);
                NotifyPropertyChanged("IsAccountPageOpened");
            }
        }

        public Timer AccountUpdateTimer = new Timer(AccountUpdateTimerCallback);
        private static void AccountUpdateTimerCallback(object state)
        {
            if (App.Current != null && App.Current.Dispatcher != null)
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (App.DVM != null && !App.DVM.IsShuttingDown && App.DVM.IsAccountPageOpened && App.DVM.IsAccountUnlocked)
                    {
                        var tasks = App.DVM.UpdateAccount();
                        Task.Factory.ContinueWhenAll(tasks, (task) =>
                        {
                            App.DVM.IsAccountUnlockedAndLoaded = true;
                            App.DVM.AccountUpdateTimer.Change(2000, Timeout.Infinite);
                        });
                    }
                }));
        }

        private Task[] UpdateAccount()
        {
            string err = String.Empty;
            //Get Balance
            var getAccountIdTask = Task.Factory
                .StartNew(() => NXTApi.GetAccountBalance(NXTAcc.accountId, ref err))
                .ContinueWith(task => NXTAccBalance = task.Result);

            //Get Account Transactions
            var getAccountTransactionsTask = Task.Factory
                .StartNew(() => NXTApi.GetAccountTransactions(NXTAcc.accountId, ref err))
                .ContinueWith(task => NXTAccTransactions = new ObservableCollection<Transaction>(task.Result));

            //Get Unconfirmed Transactions
            var getUnconfirmedTransactionsTask = Task.Factory
                .StartNew(() => NXTApi.GetUnconfirmedTransactions(ref err))
                .ContinueWith(task => NXTUnconfirmedTransactions = new ObservableCollection<Transaction>(task.Result));

            return new Task[] { getAccountIdTask, getAccountTransactionsTask, getUnconfirmedTransactionsTask };
        }

        #endregion Update Account Page

        #region Update Alias Page

        private bool isAliasPageOpened;
        public bool IsAliasPageOpened
        {
            get
            {
                return isAliasPageOpened;
            }
            set
            {
                isAliasPageOpened = value;
                AliasUpdateTimer.Change(1000, Timeout.Infinite);
                NotifyPropertyChanged("IsAliasPageOpened");
            }
        }

        public Timer AliasUpdateTimer = new Timer(AliasUpdateTimerCallback);
        private static void AliasUpdateTimerCallback(object state)
        {
            if (App.Current != null && App.Current.Dispatcher != null)
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (App.DVM != null && !App.DVM.IsShuttingDown && App.DVM.IsAliasPageOpened && App.DVM.IsAccountUnlocked)
                    {
                        var tasks = App.DVM.UpdateAlias();
                        Task.Factory.ContinueWhenAll(tasks, (task) => App.DVM.AliasUpdateTimer.Change(1000, Timeout.Infinite));
                    }
                }));
        }

        public Task[] UpdateAlias()
        {
            string err = String.Empty;
            //Get Aliases
            var getAccountAliasesTask = Task.Factory
                .StartNew(() => NXTApi.GetAccountAliases(NXTAcc.accountId, ref err))
                .ContinueWith(task => NXTAccAliases = new ObservableCollection<Alias>(task.Result));

            return new Task[] { getAccountAliasesTask };
        }

        #endregion Update Alias Page

        #region Properties

        private State nxtApiState;
        public State NXTApiState
        {
            get
            {
                if (nxtApiState == null)
                    nxtApiState = new State();
                return nxtApiState;
            }
            set
            {
                nxtApiState = value;
                NotifyPropertyChanged("NXTApiState");
            }
        }

        private NXTApi nxtApi;
        public NXTApi NXTApi
        {
            get
            {
                if (nxtApi == null)
                    nxtApi = new NXTApi();
                return nxtApi;
            }
            set
            {
                nxtApi = value;
                NotifyPropertyChanged("NXTApi");
            }
        }

        private Account nxtacc;
        public Account NXTAcc
        {
            get
            {
                if (nxtacc == null)
                    nxtacc = new Account();
                return nxtacc;
            }
            set
            {
                nxtacc = value;
                NotifyPropertyChanged("NXTAcc");
            }
        }

        private AccountBalance nxtAccBalance;
        public AccountBalance NXTAccBalance
        {
            get
            {
                return nxtAccBalance;
            }
            set
            {
                nxtAccBalance = value;
                NotifyPropertyChanged("NXTAccBalance");
                NotifyPropertyChanged("IsSendMoneyEnabled");
            }
        }

        private ObservableCollection<Transaction> nxtAccTransactions;
        public ObservableCollection<Transaction> NXTAccTransactions
        {
            get
            {
                if (nxtAccTransactions == null)
                    nxtAccTransactions = new ObservableCollection<Transaction>();
                return nxtAccTransactions;
            }
            set
            {
                nxtAccTransactions = value;
                NotifyPropertyChanged("NXTAccTransactions");
            }
        }

        private ObservableCollection<Transaction> nxtUnconfirmedTransactions;
        public ObservableCollection<Transaction> NXTUnconfirmedTransactions
        {
            get
            {
                if (nxtUnconfirmedTransactions == null)
                    nxtUnconfirmedTransactions = new ObservableCollection<Transaction>();
                return nxtUnconfirmedTransactions;
            }
            set
            {
                nxtUnconfirmedTransactions = value;
                NotifyPropertyChanged("NXTUnconfirmedTransactions");
            }
        }

        private ObservableCollection<Alias> nxtAccAliases;
        public ObservableCollection<Alias> NXTAccAliases
        {
            get
            {
                if (nxtAccAliases == null)
                    nxtAccAliases = new ObservableCollection<Alias>();
                return nxtAccAliases;
            }
            set
            {
                nxtAccAliases = value;
                NotifyPropertyChanged("NXTAccAliases");
            }
        }

        private ObservableCollection<Peer> nxtActivePeers;
        public ObservableCollection<Peer> NXTActivePeers
        {
            get
            {
                if (nxtActivePeers == null)
                    nxtActivePeers = new ObservableCollection<Peer>();
                return nxtActivePeers;
            }
            set
            {
                nxtActivePeers = value;
                NotifyPropertyChanged("NXTActivePeers");
            }
        }

        private ObservableCollection<Peer> nxtWellKnownPeers;
        public ObservableCollection<Peer> NXTWellKnownPeers
        {
            get
            {
                if (nxtWellKnownPeers == null)
                    nxtWellKnownPeers = new ObservableCollection<Peer>();
                return nxtWellKnownPeers;
            }
            set
            {
                nxtWellKnownPeers = value;
                NotifyPropertyChanged("NXTWellKnownPeers");
            }
        }

        private ObservableCollection<Block> nxtRecentBlocks;
        public ObservableCollection<Block> NXTRecentBlocks
        {
            get
            {
                if (nxtRecentBlocks == null)
                    nxtRecentBlocks = new ObservableCollection<Block>();
                return nxtRecentBlocks;
            }
            set
            {
                nxtRecentBlocks = value;
                NotifyPropertyChanged("NXTRecentBlocks");
            }
        }

        private SecureString nxtAccSecureString;
        public SecureString NXTAccSecureString
        {
            get
            {
                return nxtAccSecureString;
            }
            set
            {
                nxtAccSecureString = value;
                NotifyPropertyChanged("NXTAccSecureString");
            }
        }


        private bool isAccountUnlocked;
        public bool IsAccountUnlocked
        {
            get
            {
                return isAccountUnlocked;
            }
            set
            {
                isAccountUnlocked = value;
                if (value)
                {
                    AccountUpdateTimer.Change(0, Timeout.Infinite);
                    AliasUpdateTimer.Change(0, Timeout.Infinite);
                }
                else
                    IsAccountUnlockedAndLoaded = false;
                NotifyPropertyChanged("IsAccountUnlocked");
            }
        }

        private bool isAccountUnlockedAndLoaded;
        public bool IsAccountUnlockedAndLoaded
        {
            get
            {
                return isAccountUnlockedAndLoaded;
            }
            set
            {
                isAccountUnlockedAndLoaded = value;

                NotifyPropertyChanged("IsAccountUnlockedAndLoaded");
                NotifyPropertyChanged("IsSendMoneyEnabled");
                NotifyPropertyChanged("AccountControlVisibility");
            }
        }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }
            set
            {
                isLoaded = value;
                NotifyPropertyChanged("IsLoaded");
            }
        }

        private bool isAPIConnected = false;
        public bool IsAPIConnected
        {
            get
            {
                return isAPIConnected;
            }
            set
            {
                if (value && value != isAPIConnected)
                    CheckBlockChainDownloadProgress();

                CheckConnectionToNRS();

                isAPIConnected = value;
                NotifyPropertyChanged("IsAPIConnected");
            }
        }

        private bool isShuttingDown = false;
        public bool IsShuttingDown
        {
            get
            {
                return isShuttingDown;
            }
            set
            {
                isShuttingDown = value;
                NotifyPropertyChanged("IsShuttingDown");
            }
        }

        private bool externalNRSActive = false;
        public bool ExternalNRSActive
        {
            get
            {
                return externalNRSActive;
            }
            set
            {
                externalNRSActive = value;
                if (value)
                    IsAPIConnected = true;
                NotifyPropertyChanged("ExternalNRSActive");
            }
        }

        private string busyMessage = "The application is starting. Please wait...";
        public string BusyMessage
        {
            get
            {
                return busyMessage;
            }
            set
            {
                busyMessage = value;
                NotifyPropertyChanged("BusyMessage");
            }
        }

        private string consoleOutput;
        public string ConsoleOutput
        {
            get
            {
                return consoleOutput;
            }
            set
            {
                if (value.Contains("BaiMangal"))
                    value = value.Split(new string[] { "BaiMangal" }, StringSplitOptions.RemoveEmptyEntries)[1];
                consoleOutput = value;
                if (value.Contains("wellKnownPeers"))
                {
                    var wellKnownPeers = value.Substring((value.IndexOf("\"wellKnownPeers\" = \"") + 20));
                    WellKnownPeersList = wellKnownPeers.Substring(0, wellKnownPeers.IndexOf("\"")).Split(new string[] { "; ", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                NotifyPropertyChanged("ConsoleOutput");
            }
        }

        private List<string> wellKnownPeersList;
        public List<string> WellKnownPeersList
        {
            get
            {
                if (wellKnownPeersList == null)
                    return new List<string>();
                return wellKnownPeersList;
            }
            set
            {
                wellKnownPeersList = value;
                NotifyPropertyChanged("WellKnownPeersList");
            }
        }

        public Visibility AccountControlVisibility
        {
            get
            {
                if (IsAccountUnlockedAndLoaded)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public bool IsSendMoneyEnabled
        {
            get
            {
                if (!IsAccountUnlockedAndLoaded || NXTAccBalance == null)
                    return false;

                return NXTAccBalance.balance > 0;
            }
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }

        #endregion Properties

        #region Initialize

        public ViewModel(bool externalNRSActive = false)
        {
            string err = String.Empty;
            ExternalNRSActive = externalNRSActive;
            if (!externalNRSActive)
            {
                Task.Factory.StartNew(() =>
                {
                    do
                    {
                        NXTApiState = NXTApi.GetState(ref err);
                    }
                    while (NXTApiState.version == null);
                }).ContinueWith(task => IsAPIConnected = true);
            }
            else
            {
                CheckConnectionToNRS();
            }
        }

        private void CheckConnectionToNRS()
        {
            string err = String.Empty;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                NXTApiState = NXTApi.GetState(ref err);
            }).ContinueWith(task => IsAPIConnected = NXTApiState.version != null);
        }

        private void CheckBlockChainDownloadProgress()
        {
            string err = String.Empty;
            Task.Factory
                .StartNew(() =>
                {
                    NXTApiState = NXTApi.GetState(ref err);
                    var timestamp = Double.Parse(NXTApiState.time);
                    var lastBlock = NXTApiState.lastBlock;
                    var numOfBlocks = Double.Parse(NXTApiState.numberOfBlocks);
                    var detailedLastBlock = NXTApi.GetBlock(lastBlock, ref err);
                    var lastBlockTimestamp = Double.Parse(detailedLastBlock.timestamp);
                    var lastBlockHeight = Double.Parse(detailedLastBlock.height);
                    var currentProgressRate = lastBlockTimestamp / 60 / lastBlockHeight;
                    var currentProgress = lastBlockTimestamp / timestamp;

                    if (detailedLastBlock.nextBlock == null && timestamp - lastBlockTimestamp < 900)
                    {
                        return 100.0;
                    }
                    else
                        return currentProgress * 100.0;
                })
                .ContinueWith(task =>
                {
                    if (task.Result < 100.0)
                    {
                        if ((double)task.Result == 0)
                            BusyMessage = "Syncing NXT blockchain. You can't use the app until this is completed. It can take 5-10min.";
                        else
                            BusyMessage = "Syncing NXT blockchain. Please wait. Current progress is about: " + task.Result.ToString("0.00") + "%";

                        CheckBlockChainDownloadProgress();
                    }
                    else
                    {
                        UpdateBlocks();
                        UpdatePeers();

                        App.Current.Dispatcher.BeginInvoke(new Action(() => IsLoaded = true));
                    }
                });
        }

        #endregion Initialize

        #region Account Management

        public void UnlockAccount(SecureString secureString)
        {
            string err = String.Empty;
            NXTAccSecureString = secureString;

            //Get Account Id
            var getAccountIdTask = Task.Factory
                .StartNew(() => NXTApi.GetAccountId(NXTAccSecureString, ref err))
                .ContinueWith(task => NXTAcc = task.Result)
                .ContinueWith(task => IsAccountUnlocked = true);
        }

        public void LockAccount()
        {
            IsAccountUnlocked = false;
            NXTAcc = null;
            NXTAccBalance = null;
            NXTAccSecureString = null;
            NXTAccTransactions = null;
            NXTUnconfirmedTransactions = null;
            NXTAccAliases = null;
        }

        #endregion Account Management

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
