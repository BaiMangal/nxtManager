using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Web;

namespace nxtAPIwrapper
{
    public class NXTApi
    {
        private string _path;

        public NXTApi(string path = "http://localhost:7874")
        {
            _path = path;
        }

        public State GetState(ref string err)
        {
            State nxtSate = new State();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getState");
                nxtSate = JsonConvert.DeserializeObject<State>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtSate;
        }

        public Block GetBlock(string blockaddress, ref string err)
        {
            Block nxtBlock = new Block();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getBlock&block=" + HttpUtility.UrlEncode(blockaddress));
                nxtBlock = JsonConvert.DeserializeObject<Block>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtBlock;
        }

        public List<Block> GetLatestBlocks(int count, ref string err)
        {
            List<Block> result = new List<Block>();
            var state = GetState(ref err);
            var prevBlock = state.lastBlock;
            for (int i = 0; i < count; i++)
            {
                var block = GetBlock(prevBlock, ref err);
                block.blockID = prevBlock;
                result.Add(block);
                prevBlock = block.previousBlock;
            }
            return result;
        }

        public PeersList GetPeers(ref string err)
        {
            PeersList nxtPeers = new PeersList();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getPeers");
                nxtPeers = JsonConvert.DeserializeObject<PeersList>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtPeers;
        }

        public Peer GetPeerDetails(string peer, ref string err)
        {
            Peer nxtPeer = new Peer();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getPeer&peer=" + HttpUtility.UrlEncode(peer));
                nxtPeer = JsonConvert.DeserializeObject<Peer>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtPeer;
        }

        public List<Peer> GetDetailedActivePeers(ref string err)
        {
            List<Peer> result = new List<Peer>();
            var peers = GetPeers(ref err);
            if (peers != null && peers.peers != null)
                foreach (var peer in peers.peers)
                {
                    var peerDetails = GetPeerDetails(peer, ref err);
                    if (peerDetails.state == 1)
                        result.Add(peerDetails);
                }
            return result;
        }

        public List<Peer> GetWellKnownPeers(List<string> WellKnownPeersList, ref string err)
        {
            List<Peer> result = new List<Peer>();
            if (WellKnownPeersList != null)
            {
                foreach (var peer in WellKnownPeersList)
                {
                    var peerDetails = GetPeerDetails(peer, ref err);
                    result.Add(peerDetails);
                }
            }
            return result;
        }


        public Account GetAccountId(SecureString secretPhrase, ref string err)
        {
            Account nxtAccountID = new Account();
            var path = _path + "/nxt?requestType=getAccountId&secretPhrase=" + HttpUtility.UrlEncode(ConvertToUnsecureString(secretPhrase));
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(path);
                nxtAccountID = JsonConvert.DeserializeObject<Account>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtAccountID;
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
        public AccountBalance GetAccountBalance(string accountid, ref string err)
        {
            AccountBalance nxtAccountBalance = new AccountBalance();
            var path = _path + "/nxt?requestType=getBalance&account=" + HttpUtility.UrlEncode(accountid);
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(path);
                nxtAccountBalance = JsonConvert.DeserializeObject<AccountBalance>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtAccountBalance;
        }

        public AccountTransactionIDs GetAccountTransactionIDs(string account, string timestamp, ref string err)
        {
            AccountTransactionIDs nxtTransactionIDs = new AccountTransactionIDs();
            var path = _path + "/nxt?requestType=getAccountTransactionIds&account=" + HttpUtility.UrlEncode(account) + "&timestamp=" + HttpUtility.UrlEncode(timestamp);
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(path);
                nxtTransactionIDs = JsonConvert.DeserializeObject<AccountTransactionIDs>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtTransactionIDs;
        }

        public List<Transaction> GetAccountTransactions(string account, ref string err)
        {
            List<Transaction> result = new List<Transaction>();
            var accountTransactions = GetAccountTransactionIDs(account, "0", ref err);
            if (accountTransactions != null && accountTransactions.transactionIds != null)
            {
                foreach (var id in accountTransactions.transactionIds)
                {
                    result.Insert(0, GetTransactionDetails(id, ref err));
                }
            }
            return result;
        }

        public UnconfirmedTransactionIDs GetUnconfirmedTransactionIDs(ref string err)
        {
            UnconfirmedTransactionIDs nxtUnconfirmedTransactionIDs = new UnconfirmedTransactionIDs();
            var path = _path + "/nxt?requestType=getUnconfirmedTransactionIds";
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(path);
                nxtUnconfirmedTransactionIDs = JsonConvert.DeserializeObject<UnconfirmedTransactionIDs>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtUnconfirmedTransactionIDs;
        }

        public List<Transaction> GetUnconfirmedTransactions(ref string err)
        {
            List<Transaction> result = new List<Transaction>();
            var unconfirmedTransactionIDs = GetUnconfirmedTransactionIDs(ref err);
            if (unconfirmedTransactionIDs != null && unconfirmedTransactionIDs.unconfirmedTransactionIds != null)
            {
                foreach (var id in unconfirmedTransactionIDs.unconfirmedTransactionIds)
                {
                    result.Insert(0, GetTransactionDetails(id, ref err));
                }
            }
            return result;
        }

        public Transaction GetTransactionDetails(string transaction, ref string err)
        {
            Transaction nxtTransaction = new Transaction();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getTransaction&transaction=" + HttpUtility.UrlEncode(transaction));
                nxtTransaction = JsonConvert.DeserializeObject<Transaction>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtTransaction;
        }

        public List<Alias> GetAccountAliases(string accountid, ref string err)
        {
            List<Alias> result = new List<Alias>();
            ListAliases nxtListAliases = new ListAliases();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=listAccountAliases&account=" + HttpUtility.UrlEncode(accountid));
                nxtListAliases = JsonConvert.DeserializeObject<ListAliases>(rawData);
                if (nxtListAliases != null && nxtListAliases.aliases != null)
                    result = nxtListAliases.aliases;
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return result;
        }

        public SendNXT SendMoney(SecureString secretPhrase, string recipient, string amount, ref string err, string fee = "1", string deadline = "900")
        {
            SendNXT nxtSendResult = new SendNXT();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path +
                    "/nxt?requestType=sendMoney&secretPhrase=" +
                    HttpUtility.UrlEncode(ConvertToUnsecureString(secretPhrase)) + "&recipient=" +
                    HttpUtility.UrlEncode(recipient) + "&amount=" +
                    HttpUtility.UrlEncode(amount) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline));
                nxtSendResult = JsonConvert.DeserializeObject<SendNXT>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtSendResult;
        }

        public Alias CreateAlias(SecureString secretPhrase, string alias, string uri, string fee, string deadline, ref string err)
        {
            Alias nxtAlias = new Alias();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=assignAlias&secretPhrase=" +
                    HttpUtility.UrlEncode(ConvertToUnsecureString(secretPhrase)) + "&alias=" +
                    HttpUtility.UrlEncode(alias) + "&uri=" +
                    HttpUtility.UrlEncode(uri) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline));
                nxtAlias = JsonConvert.DeserializeObject<Alias>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtAlias;
        }

        public AliasURI GetAliasURI(string alias, ref string err)
        {
            AliasURI nxtAliasURI = new AliasURI();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAliasURI&alias=" + HttpUtility.UrlEncode(alias));
                nxtAliasURI = JsonConvert.DeserializeObject<AliasURI>(rawData);
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return nxtAliasURI;
        }

    }
}
