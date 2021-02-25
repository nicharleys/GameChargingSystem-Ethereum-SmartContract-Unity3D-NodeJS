using System.Collections.Generic;
using UnityEngine;

public class BlockChainFunction : MonoBehaviour, BlockChain {
    public void UnlockAccount(string useraddresstext, string userpasswordtext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "UnlockAccount");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void ContractStatus() {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "ContractStatus");
        parameters.Add("ContractStatus", "1");
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void ListenEvent(string useraddresstext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("useraddress", useraddresstext);
        PhotonNetwork.WebRpc("ListenEvent", parameters);
    }
    public void Allowance(string useraddresstext, string sendtoaddresstext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "Allowance");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("otheruseraddress", sendtoaddresstext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void BalanceOf(string useraddresstext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "BalanceOf");
        parameters.Add("useraddress", useraddresstext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void GiveApprove(string useraddresstext, string userpasswordtext, string sendtoaddresstext, string sendamounttext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "GiveApprove");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        parameters.Add("sendtoaddress", sendtoaddresstext);
        parameters.Add("sendamount", sendamounttext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void TwoPointDeal(string useraddresstext, string userpasswordtext, string sendtoaddresstext, string sendamounttext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "TwoPointDeal");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        parameters.Add("sendtoaddress", sendtoaddresstext);
        parameters.Add("sendamount", sendamounttext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void PutExchange(string useraddresstext, string userpasswordtext, string sendtoaddresstext, string balancestext, string item1text, string item2text) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "PutExchange");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        parameters.Add("sendtoaddress", sendtoaddresstext);
        parameters.Add("balances", balancestext);
        parameters.Add("item1", item1text);
        parameters.Add("item2", item2text);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void LookExchange_Self(string useraddresstext, string sendtoaddresstext) { 
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "LookExchange_Self");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("otheruseraddress", sendtoaddresstext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void LookExchange_Other(string useraddresstext, string sendtoaddresstext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "LookExchange_Other");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("otheruseraddress", sendtoaddresstext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void LookLottery(int playernumber) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "LookLottery");
        parameters.Add("LotteryStatus", "1");
        parameters.Add("playerNumbers", playernumber);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void ExchangeStatus(string useraddresstext, string userpasswordtext, string sendtoaddresstext) { 
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "ExchangeStatus");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        parameters.Add("sendtoaddress", sendtoaddresstext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void CancelExchange(string useraddresstext, string userpasswordtext, string sendtoaddresstext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "CancelExchange");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        parameters.Add("sendtoaddress", sendtoaddresstext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void ExchangeItem(string useraddresstext, string userpasswordtext, string sendtoaddresstext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "ExchangeItem");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        parameters.Add("sendtoaddress", sendtoaddresstext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
    public void BuyLottery(string useraddresstext, string userpasswordtext, string sendamounttext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "BuyLottery");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        parameters.Add("sendamount", sendamounttext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
}
