using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {
    public static PlayerNetwork Instance;
    private PhotonView PhotonView;
    private int PlayersInGame = 0;
    private ExitGames.Client.Photon.Hashtable m_playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private Coroutine m_pingCoroutine;

    private void Awake() {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.name == "GameRoomScene") {
            if (PhotonNetwork.isMasterClient) {
                MasterLoadedGame();
            }
            else {
                NonMasterLoadedGame();
            }
        }
    }
    private void MasterLoadedGame() {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }
    private void NonMasterLoadedGame() {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
    }
    [PunRPC]
    private void RPC_LoadGameOthers() {
        PhotonNetwork.LoadLevel("GameRoomScene");
    }
    [PunRPC]
    private void RPC_LoadedGameScene() {
        PlayersInGame++;
        if (PlayersInGame == PhotonNetwork.playerList.Length) {
            print("All players are in the game scene.");
            PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer() {
        float x = Random.Range(0.0f, 20.0f);
        float z = Random.Range(0.0f, 20.0f);
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Person"), new Vector3(x, 5f, z), Quaternion.identity, 0);
    }
    private IEnumerator C_SetPing() {
        while (PhotonNetwork.connected) {
            m_playerCustomProperties["Ping"] = PhotonNetwork.GetPing();
            PhotonNetwork.player.SetCustomProperties(m_playerCustomProperties);

            yield return new WaitForSeconds(5f);
        }
        yield break;
    }

    private void OnConnectedToMaster() {
        if (m_pingCoroutine != null) {
            StopCoroutine(m_pingCoroutine);
        }
        m_pingCoroutine = StartCoroutine(C_SetPing());
    }
}
