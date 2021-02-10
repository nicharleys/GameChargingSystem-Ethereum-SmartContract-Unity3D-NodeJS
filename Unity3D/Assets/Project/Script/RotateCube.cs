using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour {
    private int speed = 60;
	void Update () {
        //this.transform.Rotate(Vector3.up * Time.deltaTime * speed);
        this.transform.Rotate(Vector3.right * Time.deltaTime * speed);
        this.transform.Rotate(Vector3.back * Time.deltaTime * speed);
    }
}
