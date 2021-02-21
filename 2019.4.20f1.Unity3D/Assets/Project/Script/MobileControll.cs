using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControll : MonoBehaviour {
    public string keyboard { get; private set; }
    public static MobileControll Instance;

    void Awake() {
        Instance = this;
    }
    public void EnterW() {
        keyboard = "W";
    }
    public void EnterS() {
        keyboard = "S";
    }
    public void EnterA() {
        keyboard = "A";
    }
    public void EnterD() {
        keyboard = "D";
    }

    public void EnterQ() {
        keyboard = "Q";
    }
    public void EnterE() {
        keyboard = "E";
    }
    public void EnterStop() {
        keyboard = "P";
    }
}
