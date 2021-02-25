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

        if (PhotonNetwork.connected == true) {
            if (PhotonView.isMine == true) {
                photonView.RPC("RPC_GiveName", PhotonTargets.All);
            }
            else {
                userCamera.SetActive(false);
                closeOther = true;
            }
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
    private void GoStraight(bool reduceMoveState, Vector3 vector) {
        if (moveSpeed < 7) {
            moveSpeed += 0.5f;
        }
        reduceMoveState = false;
        reduceMoveStateQ = false;
        reduceMoveStateE = false;
        this.transform.Translate(vector * Time.deltaTime * moveSpeed);
        ControllWalkOrRun();
    }
    private void GoHorizontal(bool reduceMoveState, float speed) {
        if (turnSpeed < 70) {
            turnSpeed += 0.5f;
        }
        reduceMoveState = false;
        this.transform.Rotate(Vector3.up, Time.deltaTime * speed);
        ControllWalkOrRun();
    }
    private void GoCurve(bool reduceMoveState, float speed) {
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
        reduceMoveState = false;
        this.transform.Rotate(Vector3.up, Time.deltaTime * speed);
        ControllWalkOrRun();
    }
    private void StartMove() {
        if (Input.GetKey(KeyCode.W) || MobileControll.Instance.keyboard == "W") {
            GoStraight(reduceMoveStateS, Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S) || MobileControll.Instance.keyboard == "S") {
            GoStraight(reduceMoveStateW, Vector3.back);
        }
        if (Input.GetKey(KeyCode.A) || MobileControll.Instance.keyboard == "A") {
            GoHorizontal(reduceMoveStateD, -turnSpeed);
        }
        if (Input.GetKey(KeyCode.D) || MobileControll.Instance.keyboard == "D") {
            GoHorizontal(reduceMoveStateA, turnSpeed);
        }
        if (jumped == false) {
            if (Input.GetKey(KeyCode.Q) || MobileControll.Instance.keyboard == "Q") {
                GoCurve(reduceMoveStateD, -turnSpeed);
            }
            if (Input.GetKey(KeyCode.E) || MobileControll.Instance.keyboard == "E") {
                GoCurve(reduceMoveStateA, turnSpeed);
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
    private void StopMove(KeyCode keyCode, bool vector) {
        if (Input.GetKeyUp(keyCode)) {
            vector = true;
        }
    }
    private void StopMoveState() {
        StopMove(KeyCode.W, reduceMoveStateW);
        StopMove(KeyCode.S, reduceMoveStateS);
        StopMove(KeyCode.A, reduceMoveStateA);
        StopMove(KeyCode.D, reduceMoveStateD);
        StopMove(KeyCode.LeftShift, reduceMoveStateShift);
        StopMove(KeyCode.Q, reduceMoveStateE);

        if (jumped == false) {
            StopMove(KeyCode.Q, reduceMoveStateE);
            StopMove(KeyCode.E, reduceMoveStateQ);
        }
    }
    private void ReduceMoveStateControll(bool reduceMoveState, float moveSpeedvalue, Vector3 vector, float vectorSpeed) {
        if (reduceMoveState == true) {
            if (moveSpeed > 0) {
                moveSpeed -= moveSpeedvalue;
                this.transform.Translate(vector * Time.deltaTime * vectorSpeed);
                ControllWalkOrRun();
            }
            else {
                reduceMoveState = false;
                photonView.RPC("RPC_PerformStopWalk", PhotonTargets.All);
            }
        }
    }
    private void StopMove() {
        ReduceMoveStateControll(reduceMoveStateW, 1f, Vector3.forward, moveSpeed);
        ReduceMoveStateControll(reduceMoveStateS, 1f, Vector3.back, moveSpeed);
        ReduceMoveStateControll(reduceMoveStateA, 2f, Vector3.up, -turnSpeed);
        ReduceMoveStateControll(reduceMoveStateD, 2f, Vector3.up, turnSpeed);
        ReduceMoveStateControll(reduceMoveStateQ, 1f, Vector3.left, moveSpeed);
        ReduceMoveStateControll(reduceMoveStateE, 1f, Vector3.right, moveSpeed);

        if (reduceMoveStateShift == true) {
            if (moveSpeed > 7 || turnSpeed > 70) {
                if (moveSpeed > 7) {
                    moveSpeed -= 0.5f;
                }
                else if (turnSpeed > 70) {
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
        if (Input.GetKeyDown(KeyCode.Space) && jumped == false) {
            photonView.RPC("RPC_PerformJump", PhotonTargets.All);
        }
    }
    private void StartRun() {
        if (Input.GetKey(KeyCode.LeftShift) && (moveSpeed >= 7 || turnSpeed >= 70)) {
            if (moveSpeed < 28) {
                moveSpeed += 1f;
            }
            else if (turnSpeed < 200) {
                turnSpeed += 1f;
            }
            walkOrRunLock = 1;
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


