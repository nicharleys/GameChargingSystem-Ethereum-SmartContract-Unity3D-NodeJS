using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FirstCheck : MonoBehaviour {
    public Text startUIAddress;
    public Text startUIPassword;

    public string UserPassword { get; private set; }

    public static FirstCheck Instance;

    private void Awake() {
        Instance = this;
    }
    
    public void CheckAccount() {
        if (startUIAddress.text.Length != 0) {
            UnlockAccount(startUIAddress.text, startUIPassword.text);
            PhotonNetwork.playerName = startUIAddress.text;
            UserPassword = startUIPassword.text;
            MainCanvasManager.instance.LobbyCanvas.transform.SetAsLastSibling();
        }
    }
    public void UnlockAccount(string useraddresstext, string userpasswordtext) {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("status", "UnlockAccount");
        parameters.Add("useraddress", useraddresstext);
        parameters.Add("userpassword", userpasswordtext);
        PhotonNetwork.WebRpc("Transform", parameters);
    }
}
