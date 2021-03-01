using UnityEngine;

public class LobbyNetwork : MonoBehaviour {


    private void Start () {
        if (!PhotonNetwork.connected) {
            print("Connecting to server..");
            PhotonNetwork.ConnectUsingSettings("0.0.1");
        }
	}
    private void OnConnectedToMaster() {
        print("Connected to master.");
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    private void OnJoinedLobby() {
        print("Joined lobby.");
        if (!PhotonNetwork.inRoom) {
            MainCanvasManager.instance.StartGameCanvas.transform.SetAsLastSibling();
        }
    }

}
