using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace nxtAPIwrapper
{
    public class nxtAPI
    {


        /*
        * Account requests start here
        */

        private string _path;

        public nxtAPI(string path = "http://localhost:7874")
        {
            _path = path;
        }

        public Account getAccountId(string secretPhrase, ref string err)
        {
            Account nxtAccountID = new Account();
            var path = _path + "/nxt?requestType=getAccountId&secretPhrase=" + HttpUtility.UrlEncode(secretPhrase);
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
                err = e.ToString();
            }
            return nxtAccountID;
        }

        public List<AccountBlockIDs> getAccountBlockIDs(string account, string timestamp, ref string err)
        {
            List<AccountBlockIDs> nxtBlockIDs = new List<AccountBlockIDs>();
            var path = _path + "/nxt?requestType=getAccountBlockIds&account=" + HttpUtility.UrlEncode(account) + "&timestamp=" + HttpUtility.UrlEncode(timestamp);
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(path);
                nxtBlockIDs = JsonConvert.DeserializeObject<List<AccountBlockIDs>>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtBlockIDs;
        }

        public AccountPublicKey getAccountPublicKey(string account, ref string err)
        {
            AccountPublicKey nxtPublicKey = new AccountPublicKey();
            var path = _path + "/nxt?requestType=getAccountPublicKey&account=" + HttpUtility.UrlEncode(account);
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(path);
                nxtPublicKey = JsonConvert.DeserializeObject<AccountPublicKey>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtPublicKey;
        }

        public AccountTransactionIDs getAccountTransactionIDs(string account, string timestamp, ref string err)
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
                err = e.ToString(); //sek
            }
            return nxtTransactionIDs;
        }

        public AccountBalance getAccountBalance(string accountid, ref string err)
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
                err = e.ToString();
            }
            return nxtAccountBalance;
        }

        public AccountGuaranteedBalance getAccountGuaranteedBalance(string accountid, string numberOfConfirmations, ref string err)
        {
            AccountGuaranteedBalance nxtAccountGuaranteedBalance = new AccountGuaranteedBalance();
            var path = _path + "/nxt?requestType=getGuaranteedBalance&account=" + HttpUtility.UrlEncode(accountid) + "&numberOfConfirmations=" + HttpUtility.UrlEncode(numberOfConfirmations);
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(path);
                nxtAccountGuaranteedBalance = JsonConvert.DeserializeObject<AccountGuaranteedBalance>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAccountGuaranteedBalance;
        }

        /*
        * Alias requests start here
        */

        public Alias createAlias(string secretPhrase, string alias, string uri, string fee, string deadline, ref string err)
        {
            Alias nxtAlias = new Alias();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=assignAlias&secretPhrase=" +
                    HttpUtility.UrlEncode(secretPhrase) + "&alias=" +
                    HttpUtility.UrlEncode(alias) + "&uri=" +
                    HttpUtility.UrlEncode(uri) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline));
                nxtAlias = JsonConvert.DeserializeObject<Alias>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAlias;
        }

        public AliasID getAliasID(string alias, ref string err)
        {
            AliasID nxtAliasID = new AliasID();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAliasId&alias=" + HttpUtility.UrlEncode(alias));
                nxtAliasID = JsonConvert.DeserializeObject<AliasID>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAliasID;
        }

        public List<AliasIDs> getAliasIDs(string timestamp, ref string err)
        {
            List<AliasIDs> nxtAliasIDs = new List<AliasIDs>();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAliasIds&timestamp=" + HttpUtility.UrlEncode(timestamp));
                nxtAliasIDs = JsonConvert.DeserializeObject<List<AliasIDs>>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAliasIDs;
        }

        public AliasURI getAliasURI(string alias, ref string err)
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
                err = e.ToString();
            }
            return nxtAliasURI;
        }
        //da se naglasi!!!!
        public ListAliases getListAliases(string accountid, ref string err)
        {
            ListAliases nxtListAliases = new ListAliases();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=listAccountAliases&account=" + HttpUtility.UrlEncode(accountid));
                nxtListAliases = JsonConvert.DeserializeObject<ListAliases>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtListAliases;
        }

        /*
        * Arbitrary Messages
        */

        public AM sendAM(string secretPhrase, string recipient, string message, string deadline, ref string err, string referencedTransaction = "")
        {
            AM nxtAM = new AM();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var add_referencedTransaction_to_URL = (referencedTransaction == "") ? "" : "&referencedTransaction=" + HttpUtility.UrlEncode(referencedTransaction);
                var rawData = client.DownloadString(_path + "/nxt?requestType=sendMessage&secretPhrase=" +
                    HttpUtility.UrlEncode(secretPhrase) + "&recipient=" +
                    HttpUtility.UrlEncode(recipient) + "&message=" +
                    HttpUtility.UrlEncode(message) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline) + add_referencedTransaction_to_URL);
                nxtAM = JsonConvert.DeserializeObject<AM>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAM;
        }

        /*
        * Blocks
        */

        //da se naglasi!!!!

        public Block getBlock(string blockaddress, ref string err)
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
                err = e.ToString();
            }
            return nxtBlock;
        }

        /*
*  Transactinos
*/

        public Transaction getTransactionDetails(string transaction, ref string err)
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
                err = e.ToString();
            }
            return nxtTransaction;
        }

        //send NXT
        public SendNXT SendMoney(string secretPhrase, string recipient, string amount, ref string err, string fee = "1", string deadline = "900")
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
                    HttpUtility.UrlEncode(secretPhrase) + "&recipient=" +
                    HttpUtility.UrlEncode(recipient) + "&amount=" +
                    HttpUtility.UrlEncode(amount) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline));
                nxtSendResult = JsonConvert.DeserializeObject<SendNXT>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtSendResult;
        }

        public SendNXT ProcessSendNXT(string secretPhrase, string recipient, string amount, ref string err, string fee = "1", string deadline = "900", string referencedTransaction = "")
        {
            SendNXT nxtSendMoneyReturn = new SendNXT();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=sendMoney&secretPhrase=" +
                    HttpUtility.UrlEncode(secretPhrase) + "&recipient=" +
                    HttpUtility.UrlEncode(recipient) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline) + "&amount=" +
                    HttpUtility.UrlEncode(amount) + "&referencedTransaction=" +
                    HttpUtility.UrlEncode(referencedTransaction));
                nxtSendMoneyReturn = JsonConvert.DeserializeObject<SendNXT>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtSendMoneyReturn;
        }

        public State getState(ref string err)
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
                err = e.ToString();
            }
            return nxtSate;
        }


        //Assets
        public AssetTransaction issueAsset(string secretPhrase, string assetName, string assetDescription, string assetQuantity, string assetFee, ref string err, string referencedTransaction = "")
        {
            AssetTransaction nxtAssetTransaction = new AssetTransaction();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=issueAsset&secretPhrase=" +
                    HttpUtility.UrlEncode(secretPhrase) + "&name=" +
                    HttpUtility.UrlEncode(assetName) + "&description=" +
                    HttpUtility.UrlEncode(assetDescription) + "&quantity=" +
                    HttpUtility.UrlEncode(assetQuantity) + "&fee=" +
                    HttpUtility.UrlEncode(assetFee) + (referencedTransaction.Length > 0 ? "&referencedTransaction=" + HttpUtility.UrlEncode(referencedTransaction) : ""));
                nxtAssetTransaction = JsonConvert.DeserializeObject<AssetTransaction>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetTransaction;
        }

        public AssetIDs getAssetIDs(ref string err)
        {
            AssetIDs nxtAssetIDs = new AssetIDs();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAssetIds");
                nxtAssetIDs = JsonConvert.DeserializeObject<AssetIDs>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetIDs;
        }
        public Asset getAsset(string assetID, ref string err)
        {
            Asset nxtAsset = new Asset();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAsset&asset=" + HttpUtility.UrlEncode(assetID));
                nxtAsset = JsonConvert.DeserializeObject<Asset>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAsset;
        }

        public AccountAssets getAccount(string account, ref string err)
        {
            AccountAssets nxtAsset = new AccountAssets();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAccount&account=" + HttpUtility.UrlEncode(account));
                var asset = JsonConvert.DeserializeObject(rawData);
                nxtAsset = JsonConvert.DeserializeObject<AccountAssets>(rawData);

            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAsset;
        }

        public AssetTransaction transferAsset(string secretPhrase, string assetRecipient, string assetID, string assetQuantity, string assetFee, string deadline, ref string err, string referencedTransaction = "")
        {
            AssetTransaction nxtAssetTransaction = new AssetTransaction();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=transferAsset&secretPhrase=" +
                    HttpUtility.UrlEncode(secretPhrase) + "&recipient=" +
                    HttpUtility.UrlEncode(assetRecipient) + "&asset=" +
                    HttpUtility.UrlEncode(assetID) + "&quantity=" +
                    HttpUtility.UrlEncode(assetQuantity) + "&fee=" +
                    HttpUtility.UrlEncode(assetFee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline) + (referencedTransaction.Length > 0 ? "&referencedTransaction=" + HttpUtility.UrlEncode(referencedTransaction) : ""));
                nxtAssetTransaction = JsonConvert.DeserializeObject<AssetTransaction>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetTransaction;
        }

        public AssetTransaction placeAssetAskOrder(string secretPhrase, string assetID, string assetQuantity, string assetPriceNXTCents, string fee, string deadline, ref string err, string referencedTransaction = "")
        {
            AssetTransaction nxtAssetTransaction = new AssetTransaction();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=placeAskOrder&secretPhrase=" +
                    HttpUtility.UrlEncode(secretPhrase) + "&asset=" +
                    HttpUtility.UrlEncode(assetID) + "&quantity=" +
                    HttpUtility.UrlEncode(assetQuantity) + "&quantity=" +
                    HttpUtility.UrlEncode(assetQuantity) + "&price=" +
                    HttpUtility.UrlEncode(assetPriceNXTCents) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline) + (referencedTransaction.Length > 0 ? "&referencedTransaction=" + HttpUtility.UrlEncode(referencedTransaction) : ""));
                nxtAssetTransaction = JsonConvert.DeserializeObject<AssetTransaction>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetTransaction;
        }

        public AssetTransaction placeAssetBidOrder(string secretPhrase, string assetID, string assetQuantity, string assetPriceNXTCents, string fee, string deadline, ref string err, string referencedTransaction = "")
        {
            AssetTransaction nxtAssetTransaction = new AssetTransaction();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=placeBidOrder&secretPhrase=" +
                    HttpUtility.UrlEncode(secretPhrase) + "&asset=" +
                    HttpUtility.UrlEncode(assetID) + "&quantity=" +
                    HttpUtility.UrlEncode(assetQuantity) + "&quantity=" +
                    HttpUtility.UrlEncode(assetQuantity) + "&price=" +
                    HttpUtility.UrlEncode(assetPriceNXTCents) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline) + (referencedTransaction.Length > 0 ? "&referencedTransaction=" + HttpUtility.UrlEncode(referencedTransaction) : ""));
                nxtAssetTransaction = JsonConvert.DeserializeObject<AssetTransaction>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetTransaction;
        }

        public AssetAskOrderIDs getAskAssetOrderIds(string asset, string timestamp, ref string err)
        {
            AssetAskOrderIDs nxtAskOrderIDs = new AssetAskOrderIDs();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAskOrderIds&asset=" + HttpUtility.UrlEncode(asset) + "&timestamp=" + HttpUtility.UrlEncode(timestamp));
                nxtAskOrderIDs = JsonConvert.DeserializeObject<AssetAskOrderIDs>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAskOrderIDs;
        }

        public AssetBidOrderIDs getBidAssetOrderIds(string asset, string timestamp, ref string err)
        {
            AssetBidOrderIDs nxtBidOrderIDs = new AssetBidOrderIDs();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getBidOrderIds&asset=" + HttpUtility.UrlEncode(asset) + "&timestamp=" + HttpUtility.UrlEncode(timestamp));
                nxtBidOrderIDs = JsonConvert.DeserializeObject<AssetBidOrderIDs>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtBidOrderIDs;
        }

        public AssetBidOrderIDs getAccountCurrentAssetBidOrderIds(string accountID, string assetID, ref string err)
        {
            AssetBidOrderIDs nxtBidOrderIDs = new AssetBidOrderIDs();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAccountCurrentBidOrderIds&account=" + HttpUtility.UrlEncode(accountID) + "&asset=" + HttpUtility.UrlEncode(assetID));
                nxtBidOrderIDs = JsonConvert.DeserializeObject<AssetBidOrderIDs>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtBidOrderIDs;
        }
        public AssetAskOrderIDs getAccountCurrentAssetAskOrderIds(string accountID, string assetID, ref string err)
        {
            AssetAskOrderIDs nxtAskOrderIDs = new AssetAskOrderIDs();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAccountCurrentAskOrderIds&account=" + HttpUtility.UrlEncode(accountID) + "&asset=" + HttpUtility.UrlEncode(assetID));
                nxtAskOrderIDs = JsonConvert.DeserializeObject<AssetAskOrderIDs>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAskOrderIDs;
        }
        public AssetOrder getAskAssetOrder(string order, ref string err)
        {
            AssetOrder nxtAssetOrder = new AssetOrder();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getAskOrder&order=" + HttpUtility.UrlEncode(order));
                nxtAssetOrder = JsonConvert.DeserializeObject<AssetOrder>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetOrder;
        }
        public AssetOrder getBidAssetOrder(string order, ref string err)
        {
            AssetOrder nxtAssetOrder = new AssetOrder();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=getBidOrder&order=" + HttpUtility.UrlEncode(order));
                nxtAssetOrder = JsonConvert.DeserializeObject<AssetOrder>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetOrder;
        }


        public AssetTransaction cancelAskAssetOrder(string secretPhrase, string order, string fee, string deadline, ref string err, string referencedTransaction = "")
        {
            AssetTransaction nxtAssetTransaction = new AssetTransaction();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=cancelAskOrder&secretPhrase" +
                    HttpUtility.UrlEncode(secretPhrase) + "&order=" +
                    HttpUtility.UrlEncode(order) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline) + (referencedTransaction.Length > 0 ? "&referencedTransaction=" + HttpUtility.UrlEncode(referencedTransaction) : ""));
                nxtAssetTransaction = JsonConvert.DeserializeObject<AssetTransaction>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetTransaction;
        }
        public AssetTransaction cancelBidAssetOrder(string secretPhrase, string order, string fee, string deadline, ref string err, string referencedTransaction = "")
        {
            AssetTransaction nxtAssetTransaction = new AssetTransaction();
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            try
            {
                var rawData = client.DownloadString(_path + "/nxt?requestType=cancelBidOrder&secretPhrase" +
                    HttpUtility.UrlEncode(secretPhrase) + "&order=" +
                    HttpUtility.UrlEncode(order) + "&fee=" +
                    HttpUtility.UrlEncode(fee) + "&deadline=" +
                    HttpUtility.UrlEncode(deadline) + (referencedTransaction.Length > 0 ? "&referencedTransaction=" + HttpUtility.UrlEncode(referencedTransaction) : ""));
                nxtAssetTransaction = JsonConvert.DeserializeObject<AssetTransaction>(rawData);
            }
            catch (Exception e)
            {
                err = e.ToString();
            }
            return nxtAssetTransaction;
        }

        /*
         Peers
         */

        public PeersList getPeers(ref string err)
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
                err = e.ToString();
            }
            return nxtPeers;
        }


        public Peer getPeerDetails(string peer, ref string err)
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
                err = e.ToString();
            }
            return nxtPeer;
        }
    }
}
