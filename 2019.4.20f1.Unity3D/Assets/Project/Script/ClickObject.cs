using UnityEngine;
public class ClickObject : MonoBehaviour {
    Ray ray;
    RaycastHit hit;
    string player = "Person(Clone)";
    string clickBox = "Box";
    GameObject nowClick = null;//紀錄滑鼠點選目標
    GameObject lastClick = null;//紀錄上一個滑鼠點選目標

    public static ClickObject Instance;
    public bool ControllUI { get; set; }
    public string OtherUserAddress { get; private set; }

    void Awake() {
        ControllUI = false;
        Instance = this;
    }
    void Update() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit)) {
            if (hit.collider.name == player) {
                GameObject mouseClick = hit.collider.gameObject;
                nowClick = mouseClick;
                if (lastClick != null && nowClick != lastClick) {
                    GameObject finded = lastClick.transform.Find("ClickBox").gameObject;
                    finded.SetActive(false);
                }
                GameObject find = nowClick.transform.Find("ClickBox").gameObject;
                find.SetActive(true);
                lastClick = nowClick;
            }
            if (hit.collider.name == clickBox) {
                OtherUserAddress = nowClick.transform.Find("UserName").gameObject.GetComponent<TextMesh>().text;
                ControllUI = true;
            }
        }
    }
}