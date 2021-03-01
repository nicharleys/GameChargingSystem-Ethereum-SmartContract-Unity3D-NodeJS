using UnityEngine;
using UnityEngine.UI;

public class MainCanvasManager : MonoBehaviour {

    public static MainCanvasManager instance;
    public InputField address;

    [SerializeField]
    private LobbyCanvas _lobbyCanvas;
    public LobbyCanvas LobbyCanvas {
        get { return _lobbyCanvas; }
    }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas {
        get { return _currentRoomCanvas;  }
    }

    [SerializeField]
    private StartGameCanvas _startGameCanvas;
    public StartGameCanvas StartGameCanvas {
        get { return _startGameCanvas; }
    }

    private void Awake() {
        instance = this;
    }
    public void Copy(string value) {
        GUIUtility.systemCopyBuffer = value;
    }
    public string Paste() {
        return GUIUtility.systemCopyBuffer;
    }
    public void Copyadd1() {
        Copy("0x6E427fe90d42ba92f1f426c07d362113926eb3d1");
    }
    public void Copyadd2() {
        Copy("0x65817a2bee8df38582f4124b00aa0d3ca5d87703");
    }
    public void Copyadd3() {
        Copy("0x2a25676dE8fe8378e34571be55b7B3d689EA66BF");
    }
    public void PasteON() {
        address.text = Paste();
    }
}
