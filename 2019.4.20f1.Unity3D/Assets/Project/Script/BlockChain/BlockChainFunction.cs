using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockChainFunction : MonoBehaviour {
    public ControllGameUI controllGameUI;
 
    //0 : showname, 1 : showowner,  2 : showUserMoney_GiveCredit, 3 :　showUserMoney_MoneyTransaction, 4 :  showUserMoney_ItemTransaction, 5 : showUserMoney_LotteryTransaction, 6 : showUserAllowance_GiveCredit, 7 : showOtherItem1, 8 : showOtherItem2, 9 : showOtherBalance, 10 : showOtherStatus, 11 : showSelfItem1, 12 : showSelfItem2,13: showSelfBalance, 14 : showSelfStatus, 15 : lottery_Owner, 16 : lottery_Money, 17 : lottery_Player
    [SerializeField] private Text[] uiValue_text;
    private int nowPlayerNumber;
    private int oldPlayerNumber;

    private string transformStatus = "finished"; //processing, finished
    private string listenStatus = "finished"; //processing, finished, stop
    private string waitProcess = "";
    private string tempReturnValue;

    private int countObject = 0; //計算資料筆數
    private float timer_f = 0f;//時間計算
    private int timer_i = 0;
    private bool controllStart = true; //時間開關，用秒數區隔各函數啟用時間，避免WebRPC在單秒內重複傳送與接收資料造成錯誤
    private bool timeStart = false;
    private bool controllExecute = true; //時間開關，控制函式執行等待
    private bool waitMessage = true;


    public Text changeAddressPosition_Text;
    public Text nowClickAddress;

    public Button giveCreditBack_button;
    public Button giveCreditChange_button;
    public Button giveCreditEnter_button;
    public Button moneyTransactionBack_button;
    public Button moneyTransactionEnter_button;
    public Button ItemTransactionBack_button;
    public Button ItemTransactionGive_button;
    public Button ItemTransactionChangeStatus_button;
    public Button ItemTransactionCancel_button;
    public Button ItemTransactionEnter_button;
    public Button lotteryTransactionBack_button;
    public Button lotteryTransactionLook_button;
    public Button lotteryTransactionBuy_button;
    public Button lotteryTransactionLookNext_button;
    public Button lotteryTransactionLookBack_button;

    public InputField giveCreditValue_InputField;
    public InputField giveCreditChange_InputField;
    public InputField moneyTransactionVale_InputField;
    public InputField moneyTransactionCredit_InputField;
    public InputField itemTransactionBalances_Value;
    public InputField itemTransactionItem1_Value;
    public InputField itemTransactionItem2_Value;
    public InputField lotteryTransaction_Value;
    public InputField lotteryTransactionLookValue_InputField;


    public Toggle giveCreditChange_Toggle;
    public Toggle moneyTransactionCredit_Toggle;

    public ContractStatus contractStatus;
    public ListenEvent_New listenEvent_New;
    public ListenEvent_Old listenEvent_Old;
    public GiveApprove giveApprove;
    public TwoPointDeal twoPointDeal;
    public BalanceOf balanceOf;
    public Allowance allowance;
    public LookExchange_Self lookExchange_Self;
    public LookExchange_Other lookExchange_Other;
    public LookLottery lookLottery;
    public PutExchange putExchange;
    public ExchangeStatus exchangeStatus;
    public CancelExchange cancelExchange;
    public ExchangeItem exchangeItem;
    public BuyLottery buyLottery;

    void Awake() {
        AwakeSet();
    }
    void Update() {
        UpdateSet();
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

    private void SetWaitProcessStatus(string waitProcessStatus) {
        ChangbuttonOnOrOff(false);
        if (listenStatus == "processing") {
            listenStatus = "stop";
            waitMessage = true;
            waitProcess = waitProcessStatus;
        }
        else if (listenStatus == "finished") {
            listenStatus = "stop";
            waitMessage = false;
            waitProcess = waitProcessStatus;
        }
        if (listenStatus == "stop" && waitMessage == false && transformStatus == "finished") {
            transformStatus = "processing";
            switch (waitProcessStatus) {
                case "GiveApprove":
                    StartGiveApprove();
                    break;
                case "TwoPointDeal":
                    StartTwoPointDeal();
                    break;
                case "BalanceOf":
                    StartCheckBalanceOf();
                    break;
                case "ChangeAddress":
                    StartChangeAddressPosition();
                    break;
                case "PutExchange":
                    StartPutExchange();
                    break;
                case "LookExchange_self":
                    StartLookExchange_Self();
                    break;
                case "LookExchange_Other":
                    StartLookExchange_Other();
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
                case "BuyLottery":
                    StartBuyLottery();
                    break;
                case "LookLottery":
                    StartLookLottery1();
                    break;
                default:
                    break;
            }
        }
    }
    public void GiveApprove() {
        SetWaitProcessStatus("GiveApprove");
    }
    public void TwoPointDeal() {
        SetWaitProcessStatus("TwoPointDeal");
    }
    public void CheckBalanceOf() {
        SetWaitProcessStatus("BalanceOf");
    }
    public void ChangeAddressPosition() {
        SetWaitProcessStatus("ChangeAddress");
    }
    public void PutExchange() {
        SetWaitProcessStatus("PutExchange");
    }
    public void LookExchange_Self() {
        SetWaitProcessStatus("LookExchange_self");
    }
    public void LookExchange_Other() {
        SetWaitProcessStatus("LookExchange_Other");
    }
    public void ExchangeStatus() {
        SetWaitProcessStatus("ExchangeStatus");
    }
    public void CancelExchange() {
        SetWaitProcessStatus("CancelExchange");
    }
    public void ExchangeItem() {
        SetWaitProcessStatus("ExchangeItem");
    }
    public void BuyLottery() {
        SetWaitProcessStatus("BuyLottery");
    }
    public void LookLottery() {
        SetWaitProcessStatus("LookLottery");
    }
    private void StartGiveApprove() {
        GiveApprove(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, giveCreditValue_InputField.text);
    }
    private void StartTwoPointDeal() {
        if (moneyTransactionCredit_Toggle.isOn == true) {
            TwoPointDeal(moneyTransactionCredit_InputField.text, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, moneyTransactionVale_InputField.text);
        }
        else {
            TwoPointDeal(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, moneyTransactionVale_InputField.text);
        }
    }
    private void StartCheckBalanceOf() {
        BalanceOf(PhotonNetwork.playerName);
    }
    private void StartChangeAddressPosition() {
        if (giveCreditChange_Toggle.isOn == true) {
            if (changeAddressPosition_Text.text == "對方給予") {
                if (giveCreditChange_InputField.text.Length == 42) {
                    Allowance(giveCreditChange_InputField.text, PhotonNetwork.playerName);
                }
                else {
                    transformStatus = "finished";
                    waitProcess = "";
                    listenStatus = "finished";
                    ChangbuttonOnOrOff(true);
                }
            }
            else if (changeAddressPosition_Text.text == "自己給予") {
                if (giveCreditChange_InputField.text.Length == 42) {
                    Allowance(PhotonNetwork.playerName, giveCreditChange_InputField.text);
                }
                else {
                    transformStatus = "finished";
                    waitProcess = "";
                    listenStatus = "finished";
                    ChangbuttonOnOrOff(true);
                }
            }
        }
        else {
            if (changeAddressPosition_Text.text == "對方給予") {
                if (nowClickAddress.text.Length == 42) {
                    Allowance(nowClickAddress.text, PhotonNetwork.playerName);
                }
                else {
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
                else {
                    transformStatus = "finished";
                    waitProcess = "";
                    listenStatus = "finished";
                    ChangbuttonOnOrOff(true);
                }
            }
        }
    }
    private void StartPutExchange() {
        PutExchange(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, itemTransactionBalances_Value.text, itemTransactionItem1_Value.text, itemTransactionItem2_Value.text);
    }
    private void StartLookExchange_Self() {
        LookExchange_Self(PhotonNetwork.playerName, ClickObject.Instance.OtherUserAddress);
    }
    private void StartLookExchange_Other() {
        LookExchange_Other(PhotonNetwork.playerName, ClickObject.Instance.OtherUserAddress);
    }
    private void StartExchangeStatus() {
        ExchangeStatus(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
    }
    private void StartCancelExchange() {
        CancelExchange(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
    }
    private void StartExchangeItem() {
        ExchangeItem(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
    }
    private void StartBuyLottery() {
        BuyLottery(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, lotteryTransaction_Value.text);
    }
    private void StartLookLottery1() {
        LookLottery(int.Parse(lotteryTransactionLookValue_InputField.text));
    }

    private void SetLockReset() {
        transformStatus = "finished";
        waitProcess = "";
        listenStatus = "finished";
        ChangbuttonOnOrOff(true);
    }
    // 接收WebRPC回傳
    void OnWebRpcResponse(OperationResponse operationResponse) {
        if (operationResponse.ReturnCode != 0) {
            Debug.Log("WebRPC 操作失敗. Response: " + operationResponse.ToStringFull());
            SetLockReset();
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
        switch ((string)parameters["callnumber"]) {
            case "1":
                contractStatus = JsonUtility.FromJson<ContractStatus>("{" + tempReturnValue + "}");
                break;
            case "2":
                listenEvent_New = JsonUtility.FromJson<ListenEvent_New>("{" + tempReturnValue + "}");
                waitMessage = false;
                listenStatus = "finished";
                break;
            case "3":
                allowance = JsonUtility.FromJson<Allowance>("{" + tempReturnValue + "}");
                SetLockReset();
                CheckBalanceOf();
                break;
            case "4":
                balanceOf = JsonUtility.FromJson<BalanceOf>("{" + tempReturnValue + "}");
                SetLockReset();
                break;
            case "5":
                giveApprove = JsonUtility.FromJson<GiveApprove>("{" + tempReturnValue + "}");
                SetLockReset();
                break;
            case "6":
                twoPointDeal = JsonUtility.FromJson<TwoPointDeal>("{" + tempReturnValue + "}");
                SetLockReset();
                break;
            case "7":
                putExchange = JsonUtility.FromJson<PutExchange>("{" + tempReturnValue + "}");
                SetLockReset();
                break;
            case "8":
                lookExchange_Self = JsonUtility.FromJson<LookExchange_Self>("{" + tempReturnValue + "}");
                SetLockReset();
                CheckBalanceOf();
                break;
            case "9":
                lookExchange_Other = JsonUtility.FromJson<LookExchange_Other>("{" + tempReturnValue + "}");
                SetLockReset();
                LookExchange_Self();
                break;
            case "10":
                exchangeStatus = JsonUtility.FromJson<ExchangeStatus>("{" + tempReturnValue + "}");
                SetLockReset();
                break;
            case "11":
                cancelExchange = JsonUtility.FromJson<CancelExchange>("{" + tempReturnValue + "}");
                SetLockReset();
                break;
            case "12":
                exchangeItem = JsonUtility.FromJson<ExchangeItem>("{" + tempReturnValue + "}");
                SetLockReset();
                break;
            case "13":
                buyLottery = JsonUtility.FromJson<BuyLottery>("{" + tempReturnValue + "}");
                SetLockReset();
                break;
            case "14":
                lookLottery = JsonUtility.FromJson<LookLottery>("{" + tempReturnValue + "}");
                SetLockReset();
                CheckBalanceOf();
                break;
            default:
                break;
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
                if (waitMessage == false && transformStatus == "finished" && listenStatus == "finished" && controllStart == true) {
                    controllStart = false;
                    listenStatus = "processing";
                    ListenEvent(PhotonNetwork.playerName);
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
                GiveApprove();
                break;
            case "TwoPointDeal":
                TwoPointDeal();
                break;
            case "PutExchange":
                PutExchange();
                break;
            case "ExchangeStatus":
                ExchangeStatus();
                break;
            case "CancelExchange":
                CancelExchange();
                break;
            case "ExchangeItem":
                ExchangeItem();
                break;
            case "ChangeAddress":
                ChangeAddressPosition();
                break;
            case "BalanceOf":
                CheckBalanceOf();
                break;
            case "LookExchange_Self":
                LookExchange_Self();
                break;
            case "LookExchange_Other":
                LookExchange_Other();
                break;
            case "BuyLottery":
                BuyLottery();
                break;
            case "LookLottery":
                LookLottery();
                break;
            default:
                break;
        }
    }
    public void UpdateData() {
        switch (ControllGameUI.Instance.uiStatus_string) {
            case "MoneyTransaction":
                CheckBalanceOf();
                break;
            case "GiveCredit":
                ChangeAddressPosition();
                break;
            case "ItemTransaction":
                LookExchange_Other();
                break;
            case "LotteryTransaction":
                LookLottery();
                break;
            default:
                break;
        }
        timeStart = true;//全部執行一次後，在持續更新
    }
    public IEnumerator TimeStart() {
        ContractStatus();
        yield return new WaitForSeconds(1);
        CheckBalanceOf();
        yield return new WaitForSeconds(1);
        ChangeAddressPosition();
        yield return new WaitForSeconds(1);
        LookLottery();
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
    private void AwakeSet() {
        StartCoroutine("TimeStart");
        lotteryTransactionLookValue_InputField.text = "0";
        oldPlayerNumber = nowPlayerNumber;
    }
    private void UpdateSet() {
        TimeCount();
        uiValue_text[0].text = contractStatus.getnamereturn;
        uiValue_text[1].text = contractStatus.getownerreturn;
        uiValue_text[2].text = balanceOf.value;
        uiValue_text[3].text = balanceOf.value;
        uiValue_text[4].text = balanceOf.value;
        uiValue_text[5].text = balanceOf.value;
        uiValue_text[6].text = allowance.value;
        uiValue_text[7].text = lookExchange_Other.item1;
        uiValue_text[8].text = lookExchange_Other.item2;
        uiValue_text[9].text = lookExchange_Other.balances;
        uiValue_text[10].text = lookExchange_Other.status;
        uiValue_text[11].text = lookExchange_Self.item1;
        uiValue_text[12].text = lookExchange_Self.item2;
        uiValue_text[13].text = lookExchange_Self.balances;
        uiValue_text[14].text = lookExchange_Self.status;
        uiValue_text[15].text = lookLottery.getlotteryOwnerreturn;
        uiValue_text[16].text = lookLottery.getlookMoneyreturn;
        uiValue_text[17].text = lookLottery.getlookPlayersreturn;

        nowPlayerNumber = int.Parse(lotteryTransactionLookValue_InputField.text);
        if (oldPlayerNumber != nowPlayerNumber) {
            oldPlayerNumber = nowPlayerNumber;
            LookLottery();
        }
    }
}
