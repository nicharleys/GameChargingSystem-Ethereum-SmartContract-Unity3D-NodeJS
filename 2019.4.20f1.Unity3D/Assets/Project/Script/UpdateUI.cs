using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour {
    [SerializeField] private BlockChainFunction blockChainFunction;
    //0 : ContractNameShow_Text, 1 : ContractOwnerShow_Text,  2 : GiveCreditUserMoney_Text, 3 :　MoneyTransactionUserMoney_Text, 4 :  ItemTransactionUserMoney_Text, 5 : LotteryTransactionUserMoney_Text, 6 : AllowanceValue_Text, 7 : OtherItem1Value_Text, 8 : OtherItem2Value_Text, 9 : OtherMoneyValue_Text, 10 : OtherStatusValue_Text, 11 : SelfItem1Value_Text, 12 : SelfItem2Value_Text,13: SelfMoneyValue_Text, 14 : SelfStatusValue_Text, 15 : LotteryOwnerName_Text, 16 : LotterySaveMoney_Text, 17 : LotteryPlayersName_Text
    [SerializeField] private Text[] uiValue_Text;
    [Space(10)]
    //0 : giveCreditBack_Button, 1 : giveCreditChange_Button, 2 : giveCreditEnter_Button, 3 : giveCreditValue_InputField, 4 : giveCreditChange_InputField, 5 : giveCreditChange_Toggle
    [SerializeField] public GameObject[] giveCredit_GameObject;
    [Space(10)]
    //0 : moneyTransactionBack_Button, 1 : moneyTransactionEnter_button, 2 : moneyTransactionValue_InputField, 3 : moneyTransactionCredit_InputField, 4 : moneyTransactionCredit_Toggle
    [SerializeField] public GameObject[] moneyTransaction_GameObject;
    [Space(10)]
    //0 : itemTransactionBack_Button, 1 : itemTransactionGive_Button, 2 : itemTransactionChangeStatus_Button, 3 : itemTransactionCancel_Button, 4 : itemTransactionEnter_Button, 5 : itemTransactionBalances_InputField, 6 : itemTransactionItem1_InputField, 7 : itemTransactionItem2_InputField
    [SerializeField] public GameObject[] itemTransaction_GameObject;
    [Space(10)]
    //0 : lotteryTransactionBack_Button, 1 : lotteryTransactionLook_Button, 2 : lotteryTransactionBuy_Button, 3 : lotteryTransactionLookNext_Button, 4 : lotteryTransactionLookBack_Button, 5 : lotteryTransaction_InputField
    [SerializeField] public GameObject[] lotteryTransaction_GameObject;
    public InputField lotteryTransactionLookValue_InputField;

    public Text nowClickAddress_Text;
    public Text changeAddressPosition_Text;

    int nowPlayerNumber;
    int oldPlayerNumber;
    string transformStatus = "finished"; //processing, finished
    string listenStatus = "finished"; //processing, finished, stop
    string waitProcess = "";
    string tempReturnValue;

    int countObject = 0; //計算資料筆數
    float timer_f = 0f;//時間計算
    int timer_i = 0;
    bool controllStart = true; //時間開關，用秒數區隔各函數啟用時間，避免WebRPC在單秒內重複傳送與接收資料造成錯誤
    bool timeStart = false;
    bool controllExecute = true; //時間開關，控制函式執行等待
    bool waitMessage = true;

    [HideInInspector] public ContractStatus contractStatus;
    [HideInInspector] public ListenEvent_New listenEvent_New;
    [HideInInspector] public ListenEvent_Old listenEvent_Old;
    [HideInInspector] public GiveApprove giveApprove;
    [HideInInspector] public TwoPointDeal twoPointDeal;
    [HideInInspector] public BalanceOf balanceOf;
    [HideInInspector] public Allowance allowance;
    [HideInInspector] public LookExchange_Self lookExchange_Self;
    [HideInInspector] public LookExchange_Other lookExchange_Other;
    [HideInInspector] public LookLottery lookLottery;
    [HideInInspector] public PutExchange putExchange;
    [HideInInspector] public ExchangeStatus exchangeStatus;
    [HideInInspector] public CancelExchange cancelExchange;
    [HideInInspector] public ExchangeItem exchangeItem;
    [HideInInspector] public BuyLottery buyLottery;

    void Awake() {
        AwakeSet();
    }
    void Update() {
        UpdateSet();
    }
    private void SetLockReset() {
        transformStatus = "finished";
        waitProcess = "";
        listenStatus = "finished";
        ChangbuttonOnOrOff(true);
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
        blockChainFunction.GiveApprove(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, giveCredit_GameObject[3].GetComponent<InputField>().text);
    }
    private void StartTwoPointDeal() {
        if (moneyTransaction_GameObject[4].GetComponent<Toggle>().isOn == true) {
            blockChainFunction.TwoPointDeal(moneyTransaction_GameObject[3].GetComponent<InputField>().text, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, moneyTransaction_GameObject[2].GetComponent<InputField>().text);
        }
        else {
            blockChainFunction.TwoPointDeal(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, moneyTransaction_GameObject[2].GetComponent<InputField>().text);
        }
    }
    private void StartCheckBalanceOf() {
        blockChainFunction.BalanceOf(PhotonNetwork.playerName);
    }
    private void AllowanceTarget(string targetSide, string address1, string address2) {
        if (changeAddressPosition_Text.text == targetSide) {
            if (address1.Length == 42) {
                blockChainFunction.Allowance(address1, address2);
            }
            else {
                transformStatus = "finished";
                waitProcess = "";
                listenStatus = "finished";
                ChangbuttonOnOrOff(true);
            }
        }
    }
    private void StartChangeAddressPosition() {
        if (giveCredit_GameObject[5].GetComponent<Toggle>().isOn == true) {
            AllowanceTarget("對方給予", giveCredit_GameObject[4].GetComponent<InputField>().text, PhotonNetwork.playerName);
            AllowanceTarget("自己給予", PhotonNetwork.playerName, giveCredit_GameObject[4].GetComponent<InputField>().text);
        }
        else {
            AllowanceTarget("對方給予", nowClickAddress_Text.text, PhotonNetwork.playerName);
            AllowanceTarget("自己給予", PhotonNetwork.playerName, nowClickAddress_Text.text);
        }
    }
    private void StartPutExchange() {
        blockChainFunction.PutExchange(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress, itemTransaction_GameObject[5].GetComponent<InputField>().text, itemTransaction_GameObject[6].GetComponent<InputField>().text, itemTransaction_GameObject[7].GetComponent<InputField>().text);
    }
    private void StartLookExchange_Self() {
        blockChainFunction.LookExchange_Self(PhotonNetwork.playerName, ClickObject.Instance.OtherUserAddress);
    }
    private void StartLookExchange_Other() {
        blockChainFunction.LookExchange_Other(PhotonNetwork.playerName, ClickObject.Instance.OtherUserAddress);
    }
    private void StartExchangeStatus() {
        blockChainFunction.ExchangeStatus(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
    }
    private void StartCancelExchange() {
        blockChainFunction.CancelExchange(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
    }
    private void StartExchangeItem() {
        blockChainFunction.ExchangeItem(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, ClickObject.Instance.OtherUserAddress);
    }
    private void StartBuyLottery() {
        blockChainFunction.BuyLottery(PhotonNetwork.playerName, FirstCheck.Instance.UserPassword, lotteryTransaction_GameObject[5].GetComponent<InputField>().text);
    }
    private void StartLookLottery1() {
        blockChainFunction.LookLottery(int.Parse(lotteryTransactionLookValue_InputField.text));
    }
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
                    blockChainFunction.ListenEvent(PhotonNetwork.playerName);
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
        blockChainFunction.ContractStatus();
        yield return new WaitForSeconds(1);
        CheckBalanceOf();
        yield return new WaitForSeconds(1);
        ChangeAddressPosition();
        yield return new WaitForSeconds(1);
        LookLottery();
        timeStart = true;//全部執行一次後，在持續更新
    }

    public void ChangbuttonOnOrOff(bool OnOrOff) {
        giveCredit_GameObject[0].GetComponent<Button>().interactable = OnOrOff;
        giveCredit_GameObject[1].GetComponent<Button>().interactable = OnOrOff;
        giveCredit_GameObject[2].GetComponent<Button>().interactable = OnOrOff;
        giveCredit_GameObject[3].GetComponent<InputField>().interactable = OnOrOff;
        giveCredit_GameObject[4].GetComponent<InputField>().interactable = OnOrOff;
        giveCredit_GameObject[5].GetComponent<Toggle>().interactable = OnOrOff;

        moneyTransaction_GameObject[0].GetComponent<Button>().interactable = OnOrOff;
        moneyTransaction_GameObject[1].GetComponent<Button>().interactable = OnOrOff;
        moneyTransaction_GameObject[2].GetComponent<InputField>().interactable = OnOrOff;
        moneyTransaction_GameObject[3].GetComponent<InputField>().interactable = OnOrOff;
        moneyTransaction_GameObject[4].GetComponent<Toggle>().interactable = OnOrOff;

        itemTransaction_GameObject[0].GetComponent<Button>().interactable = OnOrOff;
        itemTransaction_GameObject[1].GetComponent<Button>().interactable = OnOrOff;
        itemTransaction_GameObject[2].GetComponent<Button>().interactable = OnOrOff;
        itemTransaction_GameObject[3].GetComponent<Button>().interactable = OnOrOff;
        itemTransaction_GameObject[4].GetComponent<Button>().interactable = OnOrOff;

        lotteryTransaction_GameObject[0].GetComponent<Button>().interactable = OnOrOff;
        lotteryTransaction_GameObject[1].GetComponent<Button>().interactable = OnOrOff;
        lotteryTransaction_GameObject[2].GetComponent<Button>().interactable = OnOrOff;
        lotteryTransaction_GameObject[3].GetComponent<Button>().interactable = OnOrOff;
        lotteryTransaction_GameObject[4].GetComponent<Button>().interactable = OnOrOff;
        lotteryTransaction_GameObject[5].GetComponent<InputField>().interactable = OnOrOff;
        lotteryTransactionLookValue_InputField.interactable = OnOrOff;
    }
    private void AwakeSet() {
        StartCoroutine("TimeStart");
        lotteryTransactionLookValue_InputField.text = "0";
        oldPlayerNumber = nowPlayerNumber;
    }
    private void UpdateSet() {
        TimeCount();
        uiValue_Text[0].text = contractStatus.getnamereturn;
        uiValue_Text[1].text = contractStatus.getownerreturn;
        uiValue_Text[2].text = balanceOf.value;
        uiValue_Text[3].text = balanceOf.value;
        uiValue_Text[4].text = balanceOf.value;
        uiValue_Text[5].text = balanceOf.value;
        uiValue_Text[6].text = allowance.value;
        uiValue_Text[7].text = lookExchange_Other.item1;
        uiValue_Text[8].text = lookExchange_Other.item2;
        uiValue_Text[9].text = lookExchange_Other.balances;
        uiValue_Text[10].text = lookExchange_Other.status;
        uiValue_Text[11].text = lookExchange_Self.item1;
        uiValue_Text[12].text = lookExchange_Self.item2;
        uiValue_Text[13].text = lookExchange_Self.balances;
        uiValue_Text[14].text = lookExchange_Self.status;
        uiValue_Text[15].text = lookLottery.getlotteryOwnerreturn;
        uiValue_Text[16].text = lookLottery.getlookMoneyreturn;
        uiValue_Text[17].text = lookLottery.getlookPlayersreturn;

        nowPlayerNumber = int.Parse(lotteryTransactionLookValue_InputField.text);
        if (oldPlayerNumber != nowPlayerNumber) {
            oldPlayerNumber = nowPlayerNumber;
            LookLottery();
        }
    }
}
