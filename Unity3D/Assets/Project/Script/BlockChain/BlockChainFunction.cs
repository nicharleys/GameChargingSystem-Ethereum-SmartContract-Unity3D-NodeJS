using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class BlockChainFunction : MonoBehaviour {
    public ControllGameUI controllGameUI;

    public Text giveCredit_sendamount; //傳送金額
    public Text moneyTransaction_sendamount; 

    public Text showname;
    public Text showowner;

    public Text showUserMoney_GiveCredit;
    public Text showUserMoney_MoneyTransaction;
    public Text showUserMoney_ItemTransaction;
    public Text showUserMoney_LotteryTransaction;
    public Text showUserAllowance_GiveCredit;
    public Text ShowOtherItem1;
    public Text ShowOtherItem2;
    public Text ShowOtherBalance;
    public Text ShowOtherStatus;
    public Text ShowSelfItem1;
    public Text ShowSelfItem2;
    public Text ShowSelfBalance;
    public Text ShowSelfStatus;

    public Toggle moneyTransaction_Toggle;
    public InputField moneyTransaction_AddressValue;

    public Toggle GiveCredit_Toggle;
    public InputField changeAddressPosition_Value;
    public Text changeAddressPosition_Text;
    public Text nowClickAddress;

    public InputField itemTransactionBalances_Value;
    public InputField itemTransactionItem1_Value;
    public InputField itemTransactionItem2_Value;

    public Text lottery_Owner;
    public Text lottery_Money;
    public Text lottery_Player;
    private int nowPlayerNumber;
    private int oldPlayerNumber;

    public ContractStatus contractStatus;//JSON資料儲存
    public ListenEvent_New listenEvent_New;
    public ListenEvent_Old listenEvent_Old;
    public GiveApprove giveApprove;
    public TwoPointDeal twoPointDeal;
    public BalanceOf balanceOf;//JSON資料儲存
    public Allowance allowance;
    public LookExchange_Self lookExchange_Self;
    public LookExchange_Other lookExchange_Other;
    public LookLottery lookLottery;
    public PutExchange putExchange;
    public ExchangeStatus exchangeStatus;
    public CancelExchange cancelExchange;
    public ExchangeItem exchangeItem;
    public BuyLottery buyLottery;

    public string transformStatus = "finished"; //processing, finished
    public string listenStatus = "finished"; //processing, finished, stop
    public string waitProcess = "";

    public Button giveCreditBack_button;
    public Toggle giveCreditChange_Toggle;
    public Button giveCreditChange_button;
    public InputField giveCreditValue_InputField;
    public Button giveCreditEnter_button;
    public InputField giveCreditChange_InputField;
    public Button moneyTransactionBack_button;
    public Toggle moneyTransactionCredit_Toggle;
    public InputField moneyTransactionVale_InputField;
    public Button moneyTransactionEnter_button;
    public InputField moneyTransactionCredit_InputField;
    public Button ItemTransactionBack_button;
    public Button ItemTransactionGive_button;
    public Button ItemTransactionChangeStatus_button;
    public Button ItemTransactionCancel_button;
    public Button ItemTransactionEnter_button;
    public InputField lotteryTransaction_Value;
    public Button lotteryTransactionBack_button;
    public Button lotteryTransactionLook_button;
    public Button lotteryTransactionBuy_button;
    public Button lotteryTransactionLookNext_button;
    public Button lotteryTransactionLookBack_button;
    public InputField lotteryTransactionLookValue_InputField;

    public int playernumber = 0;

    private string tempReturnValue;
    private string checkReturnValue;

    private int countObject = 0; //計算資料筆數
    private float timer_f = 0f;//時間計算
    private int timer_i = 0;
    private bool controllStart = true; //時間開關，用秒數區隔各函數啟用時間，避免WebRPC在單秒內重複傳送與接收資料造成錯誤
    private bool timeStart = false;
    private bool controllExecute = true; //時間開關，控制函式執行等待
    public bool waitMessage = true;
    void Awake() {
        StartCoroutine("TimeStart");
        lotteryTransactionLookValue_InputField.text = "0";
        oldPlayerNumber = nowPlayerNumber;
    }
    void Update() {
        TimeCount();
        showname.text = contractStatus.getnamereturn;
        showowner.text = contractStatus.getownerreturn;
        showUserMoney_GiveCredit.text = balanceOf.value;
        showUserMoney_MoneyTransaction.text = balanceOf.value;
        showUserMoney_ItemTransaction.text = balanceOf.value;
        showUserMoney_LotteryTransaction.text = balanceOf.value;
        showUserAllowance_GiveCredit.text = allowance.value;
        ShowOtherItem1.text = lookExchange_Other.item1;
        ShowOtherItem2.text = lookExchange_Other.item2;
        ShowOtherBalance.text = lookExchange_Other.balances;
        ShowOtherStatus.text = lookExchange_Other.status;
        ShowSelfItem1.text = lookExchange_Self.item1;
        ShowSelfItem2.text = lookExchange_Self.item2;
        ShowSelfBalance.text = lookExchange_Self.balances;
        ShowSelfStatus.text = lookExchange_Self.status;
        lottery_Owner.text = lookLottery.getlotteryOwnerreturn;
        lottery_Money.text = lookLottery.getlookMoneyreturn;
        lottery_Player.text = lookLottery.getlookPlayersreturn;
        nowPlayerNumber = int.Parse(lotteryTransactionLookValue_InputField.text);
        if (oldPlayerNumber != nowPlayerNumber) {
            oldPlayerNumber = nowPlayerNumber;
            StartLookLottery();
        }
}
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
    public void StartGiveApprove() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "GiveApprove";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "GiveApprove";
        }
        if (listenStatus == "stop" && waitMessage == false) { 
            if (transformStatus == "finished") {
                transformStatus = "processing";
                GiveApprove(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, giveCredit_sendamount.text);
            }
       }
    }
    public void StartTwoPointDeal() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "TwoPointDeal";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "TwoPointDeal";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                if (moneyTransaction_Toggle.isOn == true) {
                    TwoPointDeal(moneyTransaction_AddressValue.text, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, moneyTransaction_sendamount.text);
                }
                else if (moneyTransaction_Toggle.isOn == false) {
                    TwoPointDeal(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, moneyTransaction_sendamount.text);
                }
            }
        }
    }
    public void CheckBalanceOf() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "BalanceOf";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "BalanceOf";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                BalanceOf(PhotonNetwork.playerName);
            }
        }
    }
    public void ChangeAddressPosition() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "ChangeAddress";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "ChangeAddress";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                if (GiveCredit_Toggle.isOn == true) {
                    if (changeAddressPosition_Text.text == "對方給予") {
                        if (changeAddressPosition_Value.text.Length == 42) {
                            Allowance(changeAddressPosition_Value.text, PhotonNetwork.playerName);
                        }
                        else if (changeAddressPosition_Value.text.Length != 42) {
                            transformStatus = "finished";
                            waitProcess = "";
                            listenStatus = "finished";
                            ChangbuttonOnOrOff(true);
                        }
                    }
                    else if (changeAddressPosition_Text.text == "自己給予") {
                        if (changeAddressPosition_Value.text.Length == 42) {
                            Allowance(PhotonNetwork.playerName, changeAddressPosition_Value.text);
                        }
                        else if (changeAddressPosition_Value.text.Length != 42) {
                            transformStatus = "finished";
                            waitProcess = "";
                            listenStatus = "finished";
                            ChangbuttonOnOrOff(true);
                        }
                    }
                }
                else if (GiveCredit_Toggle.isOn == false) {
                    if (changeAddressPosition_Text.text == "對方給予") {
                        if (nowClickAddress.text.Length == 42) {
                            Allowance(nowClickAddress.text, PhotonNetwork.playerName);
                        }
                        else if (changeAddressPosition_Value.text.Length != 42) {
                            transformStatus = "finished";
                            waitProcess = "";
                            listenStatus = "finished";
                            ChangbuttonOnOrOff(true);
                        }
                    }
                    else if (changeAddressPosition_Text.text == "自己給予") {
                        if (nowClickAddress.text.Length == 42) {
                            Allowance(PhotonNetwork.playerName, nowClickAddress.text);
                        }
                        else if (changeAddressPosition_Value.text.Length != 42) {
                            transformStatus = "finished";
                            waitProcess = "";
                            listenStatus = "finished";
                            ChangbuttonOnOrOff(true);
                        }
                    }
                }
            }
        }
    }
    public void StartPutExchange() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "PutExchange";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "PutExchange";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                PutExchange(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, itemTransactionBalances_Value.text, itemTransactionItem1_Value.text, itemTransactionItem2_Value.text);
            }
        }
    }
    public void StartLookExchange_Self() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "LookExchange_self";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "LookExchange_self";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                LookExchange_Self(PhotonNetwork.playerName, ClickObject.Instance.OtherUserAddress);
            }
        }
    }
    public void StartLookExchange_Other() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "LookExchange_Other";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "LookExchange_Other";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                LookExchange_Other(PhotonNetwork.playerName, ClickObject.Instance.OtherUserAddress);
            }
        }
    }
    public void StartExchangeStatus() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "ExchangeStatus";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "ExchangeStatus";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                ExchangeStatus(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
            }
        }
    }
    public void StartCancelExchange() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "CancelExchange";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "CancelExchange";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                CancelExchange(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
            }
        }
    }
    public void StartExchangeItem() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "ExchangeItem";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "ExchangeItem";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                ExchangeItem(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
            }
        }
    }
    public void StartBuyLottery() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "BuyLottery";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "BuyLottery";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                BuyLottery(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, lotteryTransaction_Value.text);
            }
        }
    }
    public void StartLookLottery() {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = "LookLottery";
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = "LookLottery";
        }
        if (listenStatus == "stop" && waitMessage == false) {
            if (transformStatus == "finished") {
                transformStatus = "processing";
                LookLottery(int.Parse(lotteryTransactionLookValue_InputField.text));
            }
        }
    }
    // 接收WebRPC回傳
    void OnWebRpcResponse(OperationResponse operationResponse) {
        if (operationResponse.ReturnCode != 0) {
            Debug.Log("WebRPC 操作失敗. Response: " + operationResponse.ToStringFull());
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
            return;
        }
        WebRpcResponse webRpcResponse = new WebRpcResponse(operationResponse);
        if (webRpcResponse.ReturnCode != 0) {
            Debug.Log("WebRPC '" + webRpcResponse.Name + "發生問題. Error: " + webRpcResponse.ReturnCode + " Message: " + webRpcResponse.DebugMessage);
            return;
        }
        Dictionary<string, object> parameters = webRpcResponse.Parameters;
        foreach (KeyValuePair<string, object> pair in parameters) {
            countObject++;
            if (countObject == parameters.Count) {
                tempReturnValue += string.Format(@"""{0}"" : ""{1}""", pair.Key, pair.Value);
            }
            else {
                tempReturnValue += string.Format(@"""{0}"" : ""{1}""", pair.Key, pair.Value) + ", ";
            }
        }
        //判斷事件
        if ((string)parameters["callnumber"] == "1") {
            contractStatus = JsonUtility.FromJson<ContractStatus>("{" + tempReturnValue + "}");
        }
        else if ((string)parameters["callnumber"] == "2") {
            listenEvent_New = JsonUtility.FromJson<ListenEvent_New>("{" + tempReturnValue + "}");
            waitMessage = false;
            listenStatus = "finished";
        }
        else if ((string)parameters["callnumber"] == "3") {
            allowance = JsonUtility.FromJson<Allowance>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
            CheckBalanceOf();
        }
        else if ((string)parameters["callnumber"] == "4") {
            balanceOf = JsonUtility.FromJson<BalanceOf>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
        }
        else if ((string)parameters["callnumber"] == "5") {
            giveApprove = JsonUtility.FromJson<GiveApprove>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
        }
        else if ((string)parameters["callnumber"] == "6") {
            twoPointDeal = JsonUtility.FromJson<TwoPointDeal>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
        }
        else if ((string)parameters["callnumber"] == "7") {
            putExchange = JsonUtility.FromJson<PutExchange>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
        }
        else if ((string)parameters["callnumber"] == "8") {
            lookExchange_Self = JsonUtility.FromJson<LookExchange_Self>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
            CheckBalanceOf();
        }
        else if ((string)parameters["callnumber"] == "9") {
            lookExchange_Other = JsonUtility.FromJson<LookExchange_Other>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
            StartLookExchange_Self();
        }
        else if ((string)parameters["callnumber"] == "10") {
            exchangeStatus = JsonUtility.FromJson<ExchangeStatus>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
        }
        else if ((string)parameters["callnumber"] == "11") {
            cancelExchange = JsonUtility.FromJson<CancelExchange>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
        }
        else if ((string)parameters["callnumber"] == "12") {
            exchangeItem = JsonUtility.FromJson<ExchangeItem>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
        }
        else if ((string)parameters["callnumber"] == "13") {
            buyLottery = JsonUtility.FromJson<BuyLottery>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
        }
        else if ((string)parameters["callnumber"] == "14") {
            lookLottery = JsonUtility.FromJson<LookLottery>("{" + tempReturnValue + "}");
            transformStatus = "finished";
            waitProcess = "";
            listenStatus = "finished";
            ChangbuttonOnOrOff(true);
            CheckBalanceOf();
        }
        //清空暫存內容
        countObject = 0;
        tempReturnValue = "";
    }
    public void TimeCount() {
        if (timeStart == true) {
            timer_f += Time.deltaTime;
            timer_i = (int)timer_f;
            if (timer_i == 1) {
                if (controllExecute == true) {
                    controllExecute = false;
                    EverySecondExecute();
                }
                if (waitMessage == false) {
                    if (transformStatus == "finished" && listenStatus == "finished") {
                        if (controllStart == true) {
                            controllStart = false;
                            listenStatus = "processing";
                            ListenEvent(PhotonNetwork.playerName);
                        }
                    }
                }
                timer_f = 0;
            }
            if (timer_i == 0) {
                controllStart = true;
                controllExecute = true;
            }
        }
    }
    public void EverySecondExecute() {
        if (listenEvent_New.blockHash != listenEvent_Old.blockHash && listenEvent_New.blockHash != "") {
            UpdateData();//更新介面資料
            listenEvent_Old.blockHash = listenEvent_New.blockHash;
        }
        switch (waitProcess) {
            case "GiveApprove":
                StartGiveApprove();
                break;
            case "TwoPointDeal":
                StartTwoPointDeal();
                break;
            case "PutExchange":
                StartPutExchange();
                break;
            case "ExchangeStatus":
                StartExchangeStatus();
                break;
            case "CancelExchange":
                StartCancelExchange();
                break;
            case "ExchangeItem":
                StartExchangeItem();
                break;
            case "ChangeAddress":
                ChangeAddressPosition();
                break;
            case "BalanceOf":
                CheckBalanceOf();
                break;
            case "LookExchange_Self":
                StartLookExchange_Self();
                break;
            case "LookExchange_Other":
                StartLookExchange_Other();
                break;
            case "BuyLottery":
                StartBuyLottery();
                break;
            case "LookLottery":
                StartLookLottery();
                break;
            default:
                break;
        }
    }
    public IEnumerator TimeStart() {
        ContractStatus();
        yield return new WaitForSeconds(1);
        CheckBalanceOf();
        yield return new WaitForSeconds(1);
        ChangeAddressPosition();
        yield return new WaitForSeconds(1);
        StartLookLottery();
        timeStart = true;//全部執行一次後，在持續更新
    }
    public void UpdateData() {
        switch (ControllGameUI.Instance.UIStatus) {
            case "MoneyTransaction":
                CheckBalanceOf();
                break;
            case "GiveCredit":
                ChangeAddressPosition();
                break;
            case "ItemTransaction":
                StartLookExchange_Other();
                break;
            case "LotteryTransaction":
                StartLookLottery();
                break;
            default:
                break;
        }
        timeStart = true;//全部執行一次後，在持續更新
    }
    public void ChangbuttonOnOrOff(bool OnOrOff) {
        giveCreditBack_button.interactable = OnOrOff;
        giveCreditChange_button.interactable = OnOrOff;
        giveCreditValue_InputField.interactable = OnOrOff;
        giveCreditEnter_button.interactable = OnOrOff;
        giveCreditChange_InputField.interactable = OnOrOff;
        giveCreditChange_Toggle.interactable = OnOrOff;

        moneyTransactionBack_button.interactable = OnOrOff;
        moneyTransactionVale_InputField.interactable = OnOrOff;
        moneyTransactionEnter_button.interactable = OnOrOff;
        moneyTransactionCredit_InputField.interactable = OnOrOff;
        moneyTransactionCredit_Toggle.interactable = OnOrOff;

        ItemTransactionBack_button.interactable = OnOrOff;
        ItemTransactionGive_button.interactable = OnOrOff;
        ItemTransactionChangeStatus_button.interactable = OnOrOff;
        ItemTransactionCancel_button.interactable = OnOrOff;
        ItemTransactionEnter_button.interactable = OnOrOff;

        lotteryTransaction_Value.interactable = OnOrOff;
        lotteryTransactionBack_button.interactable = OnOrOff;
        lotteryTransactionLook_button.interactable = OnOrOff;
        lotteryTransactionBuy_button.interactable = OnOrOff;
        lotteryTransactionLookNext_button.interactable = OnOrOff;
        lotteryTransactionLookBack_button.interactable = OnOrOff;
        lotteryTransactionLookValue_InputField.interactable = OnOrOff;
    }
}
