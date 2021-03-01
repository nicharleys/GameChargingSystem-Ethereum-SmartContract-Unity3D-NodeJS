using UnityEngine;
using System.Collections;
public class CameraFlow : MonoBehaviour {
    public Transform target;
    public float distanceUp = 0f;
    public float distanceAway = 0f;
    public float smooth = 2f;
    public float camDepthSmooth = 5f;
    void Update() {
        if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) || Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80) {
            Camera.main.fieldOfView += Input.mouseScrollDelta.y * camDepthSmooth * Time.deltaTime;
        }
    }
    void LateUpdate() {
        Vector3 disPos = target.position + Vector3.up * distanceUp - target.forward * distanceAway;
        transform.position = Vector3.Lerp(transform.position, disPos, Time.deltaTime * smooth);
        transform.LookAt(target.position);
    }
}
