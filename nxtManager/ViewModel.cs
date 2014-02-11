using nxtAPIwrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace nxtManager
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Properties

        public static Boolean IsExternalAPIBackendUsed = false;
        public static DateTime GenesisBlockTime = new DateTime(2013, 10, 24, 12, 0, 0, 0);

        public Timer UpdateTimer = new Timer(UpdateTimerCallback);

        private static void UpdateTimerCallback(object state)
        {
            if (App.DVM.AccountUnlocked && App.Current != null && App.Current.Dispatcher != null)
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    App.DVM.UpdateAccount();
                }));
        }

        public Process NXTProcess { get; set; }
        public State NXTServletState { get; set; }
        public nxtAPI NXTApi { get; set; }

        private Account nxtacc;
        public Account NXTAcc
        {
            get
            {
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
                NotifyPropertyChanged("SendMoneyVisibility");
            }
        }

        private ObservableCollection<Transaction> nxtAccTransactions;
        public ObservableCollection<Transaction> NXTAccTransactions
        {
            get
            {
                return nxtAccTransactions;
            }
            set
            {
                nxtAccTransactions = value;
                NotifyPropertyChanged("NXTAccTransactions");
            }
        }

        private ObservableCollection<Transaction> nxtAccUnconfirmedTransactions;
        public ObservableCollection<Transaction> NXTAccUnconfirmedTransactions
        {
            get
            {
                return nxtAccUnconfirmedTransactions;
            }
            set
            {
                nxtAccUnconfirmedTransactions = value;
                NotifyPropertyChanged("NXTAccUnconfirmedTransactions");
            }
        }

        private ObservableCollection<Alias> nxtAccAliases;
        public ObservableCollection<Alias> NXTAccAliases
        {
            get
            {
                return nxtAccAliases;
            }
            set
            {
                nxtAccAliases = value;
                NotifyPropertyChanged("NXTAccAliases");
            }
        }

        private ObservableCollection<Peer> nxtPeers;
        public ObservableCollection<Peer> NXTPeers
        {
            get
            {
                return nxtPeers;
            }
            set
            {
                nxtPeers = value;
                NotifyPropertyChanged("NXTPeers");
            }
        }

        private ObservableCollection<Peer> nxtActivePeers;
        public ObservableCollection<Peer> NXTActivePeers
        {
            get
            {
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

        private State nxtApiState;
        public State NXTApiState
        {
            get
            {
                return nxtApiState;
            }
            set
            {
                nxtApiState = value;
                NotifyPropertyChanged("NXTApiState");
            }
        }

        private bool accountUnlocked;
        public bool AccountUnlocked
        {
            get
            {
                return accountUnlocked;
            }
            set
            {
                accountUnlocked = value;
                UpdateTimer.Change(0, Timeout.Infinite);
                NotifyPropertyChanged("AccountUnlocked");
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
                    WellKnownPeers = wellKnownPeers.Substring(0, wellKnownPeers.IndexOf("\"")).Split(new string[] { "; ", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                NotifyPropertyChanged("ConsoleOutput");
            }
        }

        private List<string> wellKnownPeers;
        public List<string> WellKnownPeers
        {
            get
            {
                return wellKnownPeers;
            }
            set
            {
                wellKnownPeers = value;
                NotifyPropertyChanged("WellKnownPeers");
            }
        }

        #endregion Properties

        #region GUI Helper Properties

        private double mainWindowContentGridWidth = 0;
        public double MainWindowContentGridWidth
        {
            get
            {
                return mainWindowContentGridWidth;
            }
            set
            {
                mainWindowContentGridWidth = value;
                NotifyPropertyChanged("MainWindowContentGridWidth");
                NotifyPropertyChanged("ColumnWidth");
            }
        }
        private Visibility accountControlVisibility = Visibility.Collapsed;
        public Visibility AccountControlVisibility
        {
            get
            {
                return accountControlVisibility;
            }
            set
            {
                accountControlVisibility = value;
                NotifyPropertyChanged("AccountControlVisibility");
                NotifyPropertyChanged("SendMoneyVisibility");
                NotifyPropertyChanged("AccountsVisibility");
                NotifyPropertyChanged("PeersVisibility");
                NotifyPropertyChanged("BlocksVisibility");
                NotifyPropertyChanged("TransactionsVisibility");
                NotifyPropertyChanged("AliasesVisibility");
                NotifyPropertyChanged("ColumnWidth");
            }
        }

        private Visibility accountsVisibility = Visibility.Collapsed;
        public Visibility AccountsVisibility
        {
            get
            {
                if (AccountControlVisibility == Visibility.Collapsed)
                    return Visibility.Collapsed;
                return accountsVisibility;
            }
            set
            {
                accountsVisibility = value;
                NotifyPropertyChanged("AccountsVisibility");
                NotifyPropertyChanged("ColumnWidth");
            }
        }

        private Visibility peersVisibility = Visibility.Collapsed;
        public Visibility PeersVisibility
        {
            get
            {
                if (AccountControlVisibility == Visibility.Collapsed)
                    return Visibility.Collapsed;
                return peersVisibility;
            }
            set
            {
                peersVisibility = value;
                NotifyPropertyChanged("PeersVisibility");
                NotifyPropertyChanged("ColumnWidth");
            }
        }

        private Visibility blocksVisibility = Visibility.Collapsed;
        public Visibility BlocksVisibility
        {
            get
            {
                if (AccountControlVisibility == Visibility.Collapsed)
                    return Visibility.Collapsed;
                return blocksVisibility;
            }
            set
            {
                blocksVisibility = value;
                NotifyPropertyChanged("BlocksVisibility");
                NotifyPropertyChanged("ColumnWidth");
            }
        }

        private Visibility aliasesVisibility = Visibility.Collapsed;
        public Visibility AliasesVisibility
        {
            get
            {
                return aliasesVisibility;
            }
            set
            {
                aliasesVisibility = value;
                NotifyPropertyChanged("AliasesVisibility");
                NotifyPropertyChanged("ColumnWidth");
            }
        }

        private Visibility transactionsVisibility = Visibility.Collapsed;
        public Visibility TransactionsVisibility
        {
            get
            {
                if (AccountControlVisibility == Visibility.Collapsed)
                    return Visibility.Collapsed;
                return transactionsVisibility;
            }
            set
            {
                transactionsVisibility = value;
                NotifyPropertyChanged("TransactionsVisibility");
                NotifyPropertyChanged("ColumnWidth");
            }
        }

        public Visibility SendMoneyVisibility
        {
            get
            {
                if (AccountControlVisibility == Visibility.Collapsed || NXTAccBalance == null)
                    return Visibility.Collapsed;

                return ((NXTAccBalance.balance > 0) ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public double ColumnWidth
        {
            get
            {
                double visibleColumns = 0;

                if (AccountsVisibility == Visibility.Visible)
                    visibleColumns++;
                if (TransactionsVisibility == Visibility.Visible)
                    visibleColumns++;
                if (BlocksVisibility == Visibility.Visible)
                    visibleColumns++;
                if (PeersVisibility == Visibility.Visible)
                    visibleColumns++;
                if (AliasesVisibility == Visibility.Visible)
                    visibleColumns++;

                if (visibleColumns == 0)
                    return 0;

                return MainWindowContentGridWidth / visibleColumns;
            }
        }

        #endregion GUI Helper Properties

        public ViewModel()
        {
            InitializeBackend();
        }

        #region Backend logic

        private void InitializeBackend()
        {
            string err = "";
            NXTServletState = new State();
            NXTApi = new nxtAPI();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                do
                {
                    NXTServletState = NXTApi.getState(ref err);
                }
                while (NXTServletState.version == null);
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                IsLoaded = true;

                //Get Peers
                if (NXTPeers == null)
                {
                    NXTPeers = new ObservableCollection<Peer>();
                    NXTActivePeers = new ObservableCollection<Peer>();
                }
                else
                {
                    NXTPeers.Clear();
                    NXTActivePeers.Clear();
                }
                var peers = NXTApi.getPeers(ref err);
                if (peers != null && peers.peers != null)
                {
                    foreach (var peer in peers.peers)
                    {
                        var detailedPeer = NXTApi.getPeerDetails(peer, ref err);
                        NXTPeers.Add(detailedPeer);
                        if (detailedPeer.state == 1)
                            NXTActivePeers.Add(detailedPeer);
                    }
                }

                //Get Well Known Peers
                NXTWellKnownPeers = new ObservableCollection<Peer>();
                foreach (var peer in WellKnownPeers)
                {
                    var wellKnownPeer = NXTApi.getPeerDetails(peer, ref err);
                    NXTWellKnownPeers.Add(wellKnownPeer);
                }

                //Get Blocks
                NXTApiState = NXTApi.getState(ref err);
                if (NXTRecentBlocks == null)
                    NXTRecentBlocks = new ObservableCollection<Block>();
                else
                    NXTRecentBlocks.Clear();
                var prevBlock = NXTApiState.lastBlock;
                for (int i = 0; i < 60; i++)
                {
                    var block = NXTApi.getBlock(prevBlock, ref err);
                    block.blockID = prevBlock;
                    NXTRecentBlocks.Add(block);
                    prevBlock = block.previousBlock;
                }
            };
            worker.RunWorkerAsync();
        }

        public void KillBackend()
        {
            if (!IsExternalAPIBackendUsed)
            {
                StopProgram(NXTProcess);
            }
        }

        // Enumerated type for the control messages sent to the handler routine
        public enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(CtrlTypes dwCtrlEvent, uint dwProcessGroupId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool AttachConsole(int processId);

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        [DllImport("Kernel32")]
        public static extern bool FreeConsole();

        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        public void StopProgram(Process proc)
        {
            if (AttachConsole(proc.Id))
            {
                SetConsoleCtrlHandler(null, true);
                GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, 0);
                proc.WaitForExit(2000);
                FreeConsole();
                SetConsoleCtrlHandler(null, false);
            }
        }

        #endregion Backend logic

        #region Account Management


        public void UnlockAccount(SecureString secureString)
        {
            string err = "";
            NXTAccSecureString = secureString;
            
            NXTAcc = NXTApi.getAccountId(ConvertToUnsecureString(NXTAccSecureString), ref err);

            UpdateAccount();

            AccountUnlocked = true;
        }

        bool isCurrentlyUpdating = false;
        private void UpdateAccount()
        {
            isCurrentlyUpdating = true;

            string err = "";

            //Get Balance
            NXTAccBalance = NXTApi.getAccountBalance(NXTAcc.accountId, ref err);

            //Get Transactions
            if (NXTAccTransactions == null)
                NXTAccTransactions = new ObservableCollection<Transaction>();
            else
                NXTAccTransactions.Clear();
            var transactionIDs = NXTApi.getAccountTransactionIDs(NXTAcc.accountId, "0", ref err);
            if (transactionIDs != null && transactionIDs.transactionIds != null)
            {
                foreach (var id in transactionIDs.transactionIds)
                {
                    NXTAccTransactions.Insert(0, NXTApi.getTransactionDetails(id, ref err));
                }
            }
            AccountControlVisibility = Visibility.Visible;

            //Get Aliases
            if (NXTAccAliases == null)
                NXTAccAliases = new ObservableCollection<Alias>();
            else
                NXTAccAliases.Clear();
            var listAliases = NXTApi.getListAliases(NXTAcc.accountId, ref err);
            if (listAliases != null && listAliases.aliases != null)
                NXTAccAliases = new ObservableCollection<Alias>(listAliases.aliases);

            //Get Peers
            if (NXTPeers == null)
            {
                NXTPeers = new ObservableCollection<Peer>();
                NXTActivePeers = new ObservableCollection<Peer>();
            }
            else
            {
                NXTPeers.Clear();
                NXTActivePeers.Clear();
            }
            var peers = NXTApi.getPeers(ref err);
            if (peers != null && peers.peers != null)
            {
                foreach (var peer in peers.peers)
                {
                    var detailedPeer = NXTApi.getPeerDetails(peer, ref err);
                    NXTPeers.Add(detailedPeer);
                    if (detailedPeer.state != 0)
                        NXTActivePeers.Add(detailedPeer);
                }
            }

            //Get Blocks
            var state = NXTApi.getState(ref err);
            if (NXTRecentBlocks == null)
                NXTRecentBlocks = new ObservableCollection<Block>();
            else
                NXTRecentBlocks.Clear();
            var prevBlock = state.lastBlock;
            for (int i = 0; i < 60; i++)
            {
                var block = NXTApi.getBlock(prevBlock, ref err);
                block.blockID = prevBlock;
                NXTRecentBlocks.Add(block);
                prevBlock = block.previousBlock;
            }

            AccountControlVisibility = Visibility.Visible;
            isCurrentlyUpdating = false;


            UpdateTimer.Change(5000, Timeout.Infinite);
        }

        public void LockAccount()
        {
            AccountUnlocked = false;
            NXTAcc = null;
            NXTAccBalance = null;
            NXTAccSecureString = null;
            NXTAccTransactions = null;
            NXTAccUnconfirmedTransactions = null;
            NXTAccAliases = null;
            AccountControlVisibility = Visibility.Collapsed;
        }

        public static string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException("secureString");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        #endregion Account Management

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private ConsoleControl.WPF.ConsoleControl mainConsoleControlStart;

        protected void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion

        public SendNXT SendMoney(string recipient, double amount, double fee, double deadline)
        {
            string err = String.Empty;
            var result = NXTApi.SendMoney(
                ConvertToUnsecureString(NXTAccSecureString),
                recipient,
                amount.ToString(),
                ref err,
                fee.ToString(),
                deadline.ToString());
            return result;
        }

        public Alias CreateAlias(string alias, string uri, string fee = "1", string deadline = "900")
        {
            string err = String.Empty;
            var result = NXTApi.createAlias(
                ConvertToUnsecureString(NXTAccSecureString),
                alias,
                uri,
                fee,
                deadline,
                ref err);
            return result;
        }

        public AliasURI GetAliasURI(string alias)
        {
            string err = String.Empty;
            var result = NXTApi.getAliasURI(alias, ref err);
            return result;
        }

    }
}
