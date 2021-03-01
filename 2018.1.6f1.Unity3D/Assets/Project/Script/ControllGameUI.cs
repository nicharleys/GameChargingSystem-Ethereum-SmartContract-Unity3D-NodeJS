using UnityEngine;
using UnityEngine.UI;

public class ControllGameUI : MonoBehaviour {
    public BlockChainFunction blockChainFunction;

    public GameObject userUI;
    public GameObject transactionUI;
    public GameObject giveCreditUI;
    public GameObject moneyTransactionUI;
    public GameObject itemTransactionUI;
    public GameObject itemTransactionUI_BothItem;
    public GameObject itemTransactionUI_PutItem;
    public GameObject lotteryTransactionUI;
    public GameObject lotteryStatusUI;
    public Text OtherUserAddress;

    public GameObject nowClickAddress;

    public Toggle moneyTransaction_Toggle;
    public GameObject moneyTransaction_Address;
    public InputField moneyTransaction_AddressValue;

    public Toggle GiveCredit_Toggle;
    public GameObject GiveCredit_Address;
    public InputField GiveCredit_AddressValue;

    public Text changeAddressPosition_Text;

    public InputField lottery_BuyValue;

    public int playernumber;

    private bool bothItem_controll = false;
    private bool putItem_controll = false;

    private bool lotteryStatus_controll = false;

    private bool controllTabKey = false;
    private bool controllClickOrTab = true;

    public static ControllGameUI Instance;
    public string UIStatus { get; private set; }
    
    public GameObject PhoneContoll_W;
    public GameObject PhoneContoll_S;
    public GameObject PhoneContoll_A;
    public GameObject PhoneContoll_D;
    public GameObject PhoneContoll_Q;
    public GameObject PhoneContoll_E;
    public GameObject PhoneContoll_Stop;
    private int phoneStatus = 0;

    void Awake() {
        UIStatus = "";
        Instance = this;
        if (phoneStatus == 0) {
            PhoneContoll_W.SetActive(false);
            PhoneContoll_S.SetActive(false);
            PhoneContoll_A.SetActive(false);
            PhoneContoll_D.SetActive(false);
            PhoneContoll_Q.SetActive(false);
            PhoneContoll_E.SetActive(false);
            PhoneContoll_Stop.SetActive(false);
        }
    }
    void Update() {
        if (controllClickOrTab == true) { // true為控制click
            if (GameObject.Find("Person(Clone)")) {
                if (ClickObject.Instance.ControllUI == true) {
                    OtherUserAddress.text = ClickObject.Instance.OtherUserAddress;
                    userUI.SetActive(true);
                }
                else if (ClickObject.Instance.ControllUI == false) {
                    userUI.SetActive(false);
                }
            }
        }
        if (Input.GetKey(KeyCode.Tab)) {
            if (ClickObject.Instance.ControllUI == false) {
                controllClickOrTab = false; //false為控制tab
                OtherUserAddress.text = "";
                userUI.SetActive(true); 
            }
        }
    }
    public void GiveCredit() {
        UIStatus = "GiveCredit";
        transactionUI.SetActive(false);
        giveCreditUI.SetActive(true);
    }
    public void MoneyTransaction() {
        UIStatus = "MoneyTransaction";
        transactionUI.SetActive(false);
        moneyTransactionUI.SetActive(true);
    }
    public void ItemTransaction() {
        UIStatus = "ItemTransaction";
        transactionUI.SetActive(false);
        itemTransactionUI.SetActive(true);
    }
    public void LotteryTransaction() {
        UIStatus = "LotteryTransaction";
        transactionUI.SetActive(false);
        lotteryTransactionUI.SetActive(true);
    }
    public void LotteryNextNumber() {
        playernumber = int.Parse(blockChainFunction.lotteryTransactionLookValue_InputField.text) + 1;
        blockChainFunction.lotteryTransactionLookValue_InputField.text = "" + playernumber;
    }
    public void LotteryBackNumber() {
        if (int.Parse(blockChainFunction.lotteryTransactionLookValue_InputField.text) != 0) {
            playernumber = int.Parse(blockChainFunction.lotteryTransactionLookValue_InputField.text) - 1;
            blockChainFunction.lotteryTransactionLookValue_InputField.text = "" + playernumber;
        }
    }
    public void UseCredit() {
        if (moneyTransaction_Toggle.isOn == true) {
            nowClickAddress.SetActive(false);
            moneyTransaction_Address.SetActive(true);
        }
        else if(moneyTransaction_Toggle.isOn == false) {
            moneyTransaction_AddressValue.text = "";
            moneyTransaction_Address.SetActive(false);
            nowClickAddress.SetActive(true);
        }
    }
    public void SearchOtherAddress() {
        if (GiveCredit_Toggle.isOn == true) {
            nowClickAddress.SetActive(false);
            GiveCredit_Address.SetActive(true);
        }
        else if (GiveCredit_Toggle.isOn == false) {
            GiveCredit_AddressValue.text = "";
            GiveCredit_Address.SetActive(false);
            nowClickAddress.SetActive(true);
        }
    }
    public void ChangeAddressPosition() {
        if (changeAddressPosition_Text.text == "對方給予") {
            changeAddressPosition_Text.text = "自己給予";
        }
        else if (changeAddressPosition_Text.text == "自己給予") {
            changeAddressPosition_Text.text = "對方給予";
        }
    }
    public void ItemTransaction_showBothItem() {
        putItem_controll = false;
        itemTransactionUI_PutItem.SetActive(false);
        if (bothItem_controll == false) {
            bothItem_controll = true;
            itemTransactionUI_BothItem.SetActive(true);
        }
        else if (bothItem_controll == true) {
            bothItem_controll = false;
            itemTransactionUI_BothItem.SetActive(false);
        }
    }
    public void ItemTransaction_showPutItem() {
        bothItem_controll = false;
        itemTransactionUI_BothItem.SetActive(false);
        if (putItem_controll == false) {
            putItem_controll = true;
            itemTransactionUI_PutItem.SetActive(true);
        }
        else if (putItem_controll == true) {
            putItem_controll = false;
            itemTransactionUI_PutItem.SetActive(false);
        }
    }
    public void LotteryTransaction_showStatus() {
        if (lotteryStatus_controll == false) {
            lotteryStatus_controll = true;
            lotteryStatusUI.SetActive(true);
        }
        else if (lotteryStatus_controll == true) {
            lotteryStatus_controll = false;
            lotteryStatusUI.SetActive(false);
        }
    }
    public void GiveCreditBack() {
        UIStatus = "";
        nowClickAddress.SetActive(true);
        GiveCredit_Toggle.isOn = false;//返回重製UI
        GiveCredit_AddressValue.text = "";
        GiveCredit_Address.SetActive(false);

        giveCreditUI.SetActive(false);
        transactionUI.SetActive(true);
    }
    public void MoneyTransactionBack() {
        UIStatus = "";
        nowClickAddress.SetActive(true);
        moneyTransaction_Toggle.isOn = false;//返回重製UI
        moneyTransaction_AddressValue.text = "";
        moneyTransaction_Address.SetActive(false);

        moneyTransactionUI.SetActive(false);
        transactionUI.SetActive(true);
    }
    public void ItemTransactionBack() {
        UIStatus = "";
        bothItem_controll = false;
        itemTransactionUI_BothItem.SetActive(false);
        putItem_controll = false;
        itemTransactionUI_PutItem.SetActive(false);

        itemTransactionUI.SetActive(false);
        transactionUI.SetActive(true);
    }
    public void LotteryTransactionBack() {
        UIStatus = "";
        lottery_BuyValue.text = "";
        playernumber = 0;
        lotteryStatusUI.SetActive(false);
        lotteryTransactionUI.SetActive(false);
        transactionUI.SetActive(true);
    }
    public void closeUI() {
        ClickObject.Instance.ControllUI = false;
        OtherUserAddress.text = "";
        userUI.SetActive(false);
        controllClickOrTab = true;
    }

}
