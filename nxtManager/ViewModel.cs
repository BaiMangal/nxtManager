﻿using nxtAPIwrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows;

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

        private ObservableCollection<Transaction> nxtUnconfirmedTransactions;
        public ObservableCollection<Transaction> NXTUnconfirmedTransactions
        {
            get
            {
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
            bool firstTime = true;
            BackgroundWorker worker_checkApiStart = new BackgroundWorker();
            worker_checkApiStart.DoWork += (s, e) =>
            {
                do
                {
                    NXTServletState = NXTApi.getState(ref err);
                }
                while (NXTServletState.version == null);
            };
            worker_checkApiStart.RunWorkerCompleted += (s, e) =>
            {
                var worker_checkBlocksDownload = new BackgroundWorker();
                worker_checkBlocksDownload.DoWork += (sndr, evt) =>
                {
                    if (!firstTime)
                    {
                        Thread.Sleep(2000);
                        firstTime = false;
                    }

                    NXTServletState = NXTApi.getState(ref err);
                    var timestamp = Double.Parse(NXTServletState.time);
                    var lastBlock = NXTServletState.lastBlock;
                    var numOfBlocks = Double.Parse(NXTServletState.numberOfBlocks);
                    var detailedLastBlock = NXTApi.getBlock(lastBlock, ref err);
                    var lastBlockTimestamp = Double.Parse(detailedLastBlock.timestamp);
                    var lastBlockHeight = Double.Parse(detailedLastBlock.height);
                    var currentProgressRate = lastBlockTimestamp / 60 / lastBlockHeight;
                    var currentProgress = lastBlockTimestamp / timestamp;

                    if (detailedLastBlock.nextBlock == null && timestamp - lastBlockTimestamp < 900)
                    {
                        evt.Result = true;
                    }
                    else
                        evt.Result = currentProgress * 100.0;
                };
                worker_checkBlocksDownload.RunWorkerCompleted += (sndr, evt) =>
                {
                    if (evt.Result is double)
                    {
                        if ((double)evt.Result == 0)
                            BusyMessage = "Syncing NXT blockchain. You can't use the app until this is completed. It can take 5-10min.";
                        else
                            BusyMessage = "Syncing NXT blockchain. Please wait. Current progress is about: " + ((double)evt.Result).ToString("0.00") + "%";
                        worker_checkBlocksDownload.RunWorkerAsync();
                    }
                    else if (evt.Result is bool && (bool)evt.Result)
                    {
                        OnLoaded();
                    }
                };
                worker_checkBlocksDownload.RunWorkerAsync();
            };
            worker_checkApiStart.RunWorkerAsync();
        }


        public void OnLoaded()
        {
            string err = "";

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
            if (WellKnownPeers != null)
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


            IsLoaded = true;
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

        private void UpdateAccount()
        {
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

            //Get Unconfirmed Transactions
            if (NXTUnconfirmedTransactions == null)
                NXTUnconfirmedTransactions = new ObservableCollection<Transaction>();
            else
                NXTUnconfirmedTransactions.Clear();
            var unconfirmedTransactionIDs = NXTApi.getUnconfirmedTransactionIDs(ref err);
            if (unconfirmedTransactionIDs != null && unconfirmedTransactionIDs.unconfirmedTransactionIds != null)
            {
                foreach (var id in unconfirmedTransactionIDs.unconfirmedTransactionIds)
                {
                    NXTUnconfirmedTransactions.Insert(0, NXTApi.getTransactionDetails(id, ref err));
                }
            }


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

            UpdateTimer.Change(5000, Timeout.Infinite);
        }

        public void LockAccount()
        {
            AccountUnlocked = false;
            NXTAcc = null;
            NXTAccBalance = null;
            NXTAccSecureString = null;
            NXTAccTransactions = null;
            NXTUnconfirmedTransactions = null;
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
