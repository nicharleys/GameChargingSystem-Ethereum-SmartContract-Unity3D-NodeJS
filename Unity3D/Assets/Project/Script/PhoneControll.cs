using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneControll : MonoBehaviour {
    public string keyboard { get; private set; }
    public static PhoneControll Instance;

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
