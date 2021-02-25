using UnityEngine;
using UnityEngine.UI;
public class FirstCheck : MonoBehaviour {
    [SerializeField] BlockChainFunction blockChainFunction;
    [SerializeField] public Text startUIAddress;
    [SerializeField] public Text startUIPassword;
    public string UserPassword { get; private set; }
    public static FirstCheck Instance;
    private void Awake() {
        Instance = this;
    }
    public void CheckAccount() {
        if (startUIAddress.text.Length != 0) {
            blockChainFunction.UnlockAccount(startUIAddress.text, startUIPassword.text);
            PhotonNetwork.playerName = startUIAddress.text;
            UserPassword = startUIPassword.text;
            MainCanvasManager.instance.LobbyCanvas.transform.SetAsLastSibling();
        }
    }
}
