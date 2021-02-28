using System.Collections;
using UnityEngine;
public class MultiplayerMove : Photon.MonoBehaviour {
    [SerializeField] GameObject userCamera;
    [SerializeField] TextMesh userName;

    Animator anim;
    Rigidbody playerRigidbody;
    PhotonView PhotonView;
    Vector3 TargetPosition;
    Quaternion TargetRotation;

    int walkOrRunLock = 0;
    float moveSpeed;
    float turnSpeed;
    float jumpSpeed = 1f;
    bool jumped = false;
    bool reduceMoveStateW = true;
    bool reduceMoveStateA = true;
    bool reduceMoveStateS = true;
    bool reduceMoveStateD = true;
    bool reduceMoveStateQ = true;
    bool reduceMoveStateE = true;
    bool reduceMoveStateShift = true;

    bool useTranformView = true;
    bool closeOther = false;

    private void Awake() {
        PhotonView = GetComponent<PhotonView>();
        playerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (PhotonView.isMine == false && PhotonNetwork.connected == true) {
            userCamera.SetActive(false);
            closeOther = true;
        }
        if (PhotonView.isMine == true && PhotonNetwork.connected == true) {
            photonView.RPC("RPC_GiveName", PhotonTargets.All);
        }
    }
    private void FixedUpdate() {
        if (closeOther == false) {
            StartMove();
            StopMove();
            StopMoveState();
            StartJump();
            StartRun();
        }
        else {
            SmoothMove();
        }
    }
    private void SmoothMove() {
        if (useTranformView) {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500 * Time.deltaTime);
    }
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (useTranformView) {
            return;
        }
        if (stream.isWriting) {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
        }
    }
    private void StartMove() {
        if (Input.GetKey(KeyCode.W) || MobileControll.Instance.keyboard == "W") {
            if (moveSpeed < 7) {
                moveSpeed += 0.5f;
            }
            reduceMoveStateS = false;
            reduceMoveStateQ = false;
            reduceMoveStateE = false;
            this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            ControllWalkOrRun();
        }
        if (Input.GetKey(KeyCode.S) || MobileControll.Instance.keyboard == "S") {
            if (moveSpeed < 7) {
                moveSpeed += 0.5f;
            }
            reduceMoveStateW = false;
            reduceMoveStateQ = false;
            reduceMoveStateE = false;
            this.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
            ControllWalkOrRun();
        }
        if (Input.GetKey(KeyCode.A) || MobileControll.Instance.keyboard == "A") {
            if (turnSpeed < 70) {
                turnSpeed += 0.5f;
            }
            reduceMoveStateD = false;
            this.transform.Rotate(Vector3.up, Time.deltaTime * -turnSpeed);
            ControllWalkOrRun();
        }
        if (Input.GetKey(KeyCode.D) || MobileControll.Instance.keyboard == "D") {
            if (turnSpeed < 70) {
                turnSpeed += 0.5f;
            }
            reduceMoveStateA = false;
            this.transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed);
            ControllWalkOrRun();
        }
        if (jumped == false) {
            if (Input.GetKey(KeyCode.Q) || MobileControll.Instance.keyboard == "Q") {
                if (moveSpeed < 7) {
                    moveSpeed += 0.5f;
                }
                reduceMoveStateS = false;
                reduceMoveStateQ = false;
                reduceMoveStateE = false;
                this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                ControllWalkOrRun();
                if (turnSpeed < 70) {
                    turnSpeed += 0.5f;
                }
                reduceMoveStateD = false;
                this.transform.Rotate(Vector3.up, Time.deltaTime * -turnSpeed);
                ControllWalkOrRun();
            }
            if (Input.GetKey(KeyCode.E) || MobileControll.Instance.keyboard == "E") {
                if (moveSpeed < 7) {
                    moveSpeed += 0.5f;
                }
                reduceMoveStateS = false;
                reduceMoveStateQ = false;
                reduceMoveStateE = false;
                this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                ControllWalkOrRun();
                if (turnSpeed < 70) {
                    turnSpeed += 0.5f;
                }
                reduceMoveStateA = false;
                this.transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed);
                ControllWalkOrRun();
            }
        }
        if (MobileControll.Instance.keyboard == "P") {
            reduceMoveStateW = true;
            reduceMoveStateS = true;
            reduceMoveStateA = true;
            reduceMoveStateD = true;
            reduceMoveStateE = true;
            reduceMoveStateQ = true;
        }
    }
    private void StopMoveState() {
        if (Input.GetKeyUp(KeyCode.W)) {
            reduceMoveStateW = true;
        }
        if (Input.GetKeyUp(KeyCode.S)) {
            reduceMoveStateS = true;
        }
        if (Input.GetKeyUp(KeyCode.A)) {
            reduceMoveStateA = true;
        }
        if (Input.GetKeyUp(KeyCode.D)) {
            reduceMoveStateD = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            reduceMoveStateShift = true;
        }
        if (jumped == false) {
            if (Input.GetKeyUp(KeyCode.Q)) {
                reduceMoveStateE = true;
            }
            if (Input.GetKeyUp(KeyCode.E)) {
                reduceMoveStateQ = true;
            }
        }
    }
    private void StopMove() {
        if (reduceMoveStateW == true) {
            if (moveSpeed > 0) {
                moveSpeed -= 1f;
                this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                ControllWalkOrRun();
            }
            else {
                reduceMoveStateW = false;
                photonView.RPC("RPC_PerformStopWalk", PhotonTargets.All);
            }
        }
        if (reduceMoveStateS == true) {
            if (moveSpeed > 0) {
                moveSpeed -= 1f;
                this.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
                ControllWalkOrRun();
            }
            else {
                reduceMoveStateS = false;
                photonView.RPC("RPC_PerformStopWalk", PhotonTargets.All);
            }
        }
        if (reduceMoveStateA == true) {
            if (turnSpeed > 0) {
                turnSpeed -= 2f;
                this.transform.Rotate(Vector3.up, Time.deltaTime * -turnSpeed);
                ControllWalkOrRun();
            }
            else {
                reduceMoveStateA = false;
                photonView.RPC("RPC_PerformStopWalk", PhotonTargets.All);
            }
        }
        if (reduceMoveStateD == true) {
            if (turnSpeed > 0) {
                turnSpeed -= 2f;
                this.transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed);
                ControllWalkOrRun();
            }
            else {
                reduceMoveStateD = false;
                photonView.RPC("RPC_PerformStopWalk", PhotonTargets.All);
            }
        }
        if (reduceMoveStateQ == true) {
            if (moveSpeed > 0) {
                moveSpeed -= 1f;
                this.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
                ControllWalkOrRun();
            }
            else {
                reduceMoveStateQ = false;
                photonView.RPC("RPC_PerformStopWalk", PhotonTargets.All);
            }
        }
        if (reduceMoveStateE == true) {
            if (moveSpeed > 0) {
                moveSpeed -= 1f;
                this.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
                ControllWalkOrRun();
            }
            else {
                reduceMoveStateE = false;
                photonView.RPC("RPC_PerformStopWalk", PhotonTargets.All);
            }
        }
        if (reduceMoveStateShift == true) {
            if (moveSpeed > 7 || turnSpeed > 70) {
                if (moveSpeed > 7) {
                    moveSpeed -= 0.5f;
                }
                if (turnSpeed > 70) {
                    turnSpeed -= 1f;
                }
            }
            else {
                walkOrRunLock = 0;
            }
        }
    }
    private void ControllWalkOrRun() {
        if (walkOrRunLock == 0) {
            photonView.RPC("RPC_PerformStartWalk", PhotonTargets.All);
            photonView.RPC("RPC_PerformStopRun", PhotonTargets.All);
        }
        else if (walkOrRunLock == 1) {
            photonView.RPC("RPC_PerformStartRun", PhotonTargets.All);
        }
    }
    private void StartJump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (jumped == false) {
                photonView.RPC("RPC_PerformJump", PhotonTargets.All);
            }
        }
    }
    private void StartRun() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (moveSpeed >= 7 || turnSpeed >= 70) {
                if (moveSpeed < 28) {
                    moveSpeed += 1f;
                }
                if (turnSpeed < 200) {
                    turnSpeed += 1f;
                }
                walkOrRunLock = 1;
            }
        }
    }
    IEnumerator waitJump() {
        walkOrRunLock = 2;
        anim.SetBool("Run", false);
        jumped = true;
        anim.SetBool("Jump", true);
        yield return new WaitForSeconds(0.4f);
        playerRigidbody.velocity += new Vector3(0, 10, 0);
        playerRigidbody.AddForce(Vector3.up * jumpSpeed);
        yield return new WaitForSeconds(2.5f);
        anim.SetBool("Jump", false);
        jumped = false;
        walkOrRunLock = 0;
    }
    [PunRPC]
    private void RPC_PerformJump() {
        StartCoroutine("waitJump");
    }
    [PunRPC]
    private void RPC_PerformStartWalk() {
        anim.SetBool("Walk", true);
    }
    [PunRPC]
    private void RPC_PerformStopWalk() {
        anim.SetBool("Walk", false);
    }
    [PunRPC]
    private void RPC_PerformStartRun() {
        anim.SetBool("Run", true);
    }
    [PunRPC]
    private void RPC_PerformStopRun() {
        anim.SetBool("Run", false);
    }
    [PunRPC]
    private void RPC_GiveName() {
        userName.text = PhotonView.owner.NickName;
    }
}


