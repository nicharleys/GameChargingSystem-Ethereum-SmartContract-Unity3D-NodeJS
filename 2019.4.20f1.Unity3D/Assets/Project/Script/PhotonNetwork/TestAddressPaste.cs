using UnityEngine;
using UnityEngine.UI;

public class TestAddressPaste : MonoBehaviour
{
    [SerializeField] public InputField address;
    private void Copy(string value) {
        GUIUtility.systemCopyBuffer = value;
    }
    private string Paste() {
        return GUIUtility.systemCopyBuffer;
    }
    public void CopyAddress1() {
        Copy("0x6E427fe90d42ba92f1f426c07d362113926eb3d1");
    }
    public void CopyAddress2() {
        Copy("0x65817a2bee8df38582f4124b00aa0d3ca5d87703");
    }
    public void CopyAddress3() {
        Copy("0x2a25676dE8fe8378e34571be55b7B3d689EA66BF");
    }
    public void PasteON() {
        address.text = Paste();
    }
}
