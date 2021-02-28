using UnityEngine;

public class LeaveCurrentMatch : MonoBehaviour {

    public void OnClick_LeaveMatach() {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("SignUI");
    }
}
