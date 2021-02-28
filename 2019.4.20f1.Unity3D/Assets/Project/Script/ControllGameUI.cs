using UnityEngine;
using UnityEngine.UI;
public class ControllGameUI : MonoBehaviour {
    public static ControllGameUI Instance;
    public UpdateUI updateUI;
    //0 : userUI, 1 : sideUI
    //[SerializeField] public GameObject[] mainUI_GameObject;
    ////0 : giveCreditUI, 1 : moneyTransactionUI, 2 : ItemTransactionUI, 3 : lotteryTransactionUI
    //[SerializeField] private GameObject[] transactionUI_GameObject;

    public GameObject userUI;
    public GameObject sideUI;
    public GameObject giveCreditUI;
    public GameObject moneyTransactionUI;
    public GameObject itemTransactionUI;
    public GameObject lotteryTransactionUI;

    public GameObject lotteryStatusUI;

    [SerializeField] private GameObject otherUserAddress_GameObject;
    //0 : Toggle, 1 : UI, 2 : InputField, 3 : Text
    [SerializeField] private GameObject[] giveCreditInput_Gameobject;
    //0 : Toggle, 1 : UI, 2 : InputField
    [SerializeField] private GameObject[] moneyTransactionInput_GameObject;
    //0 : transformBothUI, 1 : transformGiveUI 
    [SerializeField] private GameObject[] itemTransactionUI_GameObject;
    [SerializeField] private InputField lotteryBuyValue_InputField;
    [SerializeField] private GameObject lotteryStatusUI_GameObject;
    [SerializeField] private GameObject PhoneContoll_GameObject;

    public string uiStatus_string { get; private set; }
    int playerNumber_int;
    bool transformBothUIControll_bool = false;
    bool transformGiveUIControll_bool = false;
    bool lotteryStatuscontroll_bool = false;
    bool uiModeControll_bool = true;

    void Awake() {
        Instance = this;
        uiStatus_string = "";
        if (!Application.isMobilePlatform) {
            PhoneContoll_GameObject.SetActive(false);
        }
    }
    void Update() {
        UIModeControll();
    }
    private void MainUIControll(string uiStatus, GameObject transactionUI) {
        uiStatus_string = uiStatus;
        sideUI.SetActive(false);
        transactionUI.SetActive(true);
    }
    public void GiveCredit() {
        MainUIControll("GiveCredit", giveCreditUI);
    }
    public void MoneyTransaction() {
        MainUIControll("MoneyTransaction", moneyTransactionUI);
    }
    public void ItemTransaction() {
        MainUIControll("ItemTransaction", giveCreditUI);
    }
    public void LotteryTransaction() {
        MainUIControll("LotteryTransaction", lotteryTransactionUI);
    }
    private void LotteryChangeNumber(int oneNumber) {
        playerNumber_int = int.Parse(updateUI.lotteryTransactionLookValue_InputField.GetComponent<InputField>().text) + oneNumber;
        updateUI.lotteryTransactionLookValue_InputField.text = "" + playerNumber_int;
    }
    public void LotteryNextNumber() {
        LotteryChangeNumber(1);
    }
    public void LotteryBackNumber() {
        if (int.Parse(updateUI.lotteryTransactionLookValue_InputField.text) != 0) {
            LotteryChangeNumber(-1);
        }
    }
    private void ToggleChange(GameObject toggle, GameObject address, GameObject ui, GameObject input) {
        if (toggle.GetComponent<Toggle>().isOn == true) {
            address.SetActive(false);
            ui.SetActive(true);
        }
        else {
            input.GetComponent<InputField>().text = "";
            ui.SetActive(false);
            address.SetActive(true);
        }
    }
    public void UseCredit() {
        ToggleChange(moneyTransactionInput_GameObject[0], otherUserAddress_GameObject, moneyTransactionInput_GameObject[1], moneyTransactionInput_GameObject[2]);
    }
    public void SearchOtherAddress() {
        ToggleChange(giveCreditInput_Gameobject[0], otherUserAddress_GameObject, giveCreditInput_Gameobject[1], giveCreditInput_Gameobject[2]);
    }
    public void ChangeAddressPosition() {
        if (giveCreditInput_Gameobject[3].GetComponent<Text>().text == "對方給予") {
            giveCreditInput_Gameobject[3].GetComponent<Text>().text = "自己給予";
        }
        else {
            giveCreditInput_Gameobject[3].GetComponent<Text>().text = "對方給予";
        }
    }
    private void ItemTransactionInnerlayerUI(bool uiControll, GameObject uiOption) {
        if (uiControll == false) {
            uiControll = true;
            uiOption.SetActive(true);
        }
        else {
            uiControll = false;
            uiOption.SetActive(false);
        }
    }
    public void ItemTransaction_showBothItem() {
        transformGiveUIControll_bool = false;
        itemTransactionUI.SetActive(false);
        ItemTransactionInnerlayerUI(transformBothUIControll_bool, itemTransactionUI);
    }
    public void ItemTransaction_showPutItem() {
        transformBothUIControll_bool = false;
        itemTransactionUI.SetActive(false);
        ItemTransactionInnerlayerUI(transformGiveUIControll_bool, itemTransactionUI);
    }

    public void LotteryTransaction_showStatus() {
        ItemTransactionInnerlayerUI(lotteryStatuscontroll_bool, lotteryStatusUI_GameObject);
    }

    private void InputUIBack(GameObject[] ui, GameObject transactionUI) {
        uiStatus_string = "";
        otherUserAddress_GameObject.SetActive(true);
        ui[0].GetComponent<Toggle>().isOn = false;
        ui[2].GetComponent<InputField>().text = "";
        ui[1].SetActive(false);
        transactionUI.SetActive(false);
        sideUI.SetActive(true);
    }
    public void GiveCreditBack() {
        InputUIBack(giveCreditInput_Gameobject, giveCreditUI);
    }
    public void MoneyTransactionBack() {
        InputUIBack(moneyTransactionInput_GameObject, moneyTransactionUI);
    }

    public void ItemTransactionBack() {
        uiStatus_string = "";
        transformBothUIControll_bool = false;
        itemTransactionUI_GameObject[0].SetActive(false);
        transformGiveUIControll_bool = false;
        itemTransactionUI_GameObject[1].SetActive(false);
        giveCreditUI.SetActive(false);
        itemTransactionUI.SetActive(true);
    }
    public void LotteryTransactionBack() {
        uiStatus_string = "";
        lotteryBuyValue_InputField.text = "";
        playerNumber_int = 0;
        lotteryStatusUI_GameObject.SetActive(false);
        lotteryTransactionUI.SetActive(false);
        sideUI.SetActive(true);
    }
    public void CloseUI() {
        ClickObject.Instance.ControllUI = false;
        otherUserAddress_GameObject.GetComponent<Text>().text = "";
        userUI.SetActive(false);
        uiModeControll_bool = true;
    }
    private void UIModeControll() {
        if (uiModeControll_bool == true) {
            if (GameObject.Find("Person(Clone)")) {
                if (ClickObject.Instance.ControllUI == true) {
                    otherUserAddress_GameObject.GetComponent<Text>().text = ClickObject.Instance.OtherUserAddress;
                    userUI.SetActive(true);
                }
                else if (ClickObject.Instance.ControllUI == false) {
                    userUI.SetActive(false);
                }
            }
        }
        if (Input.GetKey(KeyCode.Tab)) {
            if (ClickObject.Instance.ControllUI == false) {
                uiModeControll_bool = false;
                otherUserAddress_GameObject.GetComponent<Text>().text = "";
                userUI.SetActive(true);
            }
        }
    }
}
