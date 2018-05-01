using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상수화 해놓을 수.
static class Constants  //캐릭터의 기본 정보
{
    //moveSpeed 관련 상수
    public const float DefaultMoveSpeed = 100.0f;
    public const float MaxMoveSpeed = 150.0f;
    public const float AddMoveSpeed = 2.0f;
    public const float SitMoveSpeed = 20.0f; // 앉았을시 캐릭터의 속도

    //Jump & 캐릭터 Y축 관련 상수
    public const float Default_yVelocity = 0.0f;
    public const float jumpCountMax = 1.0f;
    public const float Default_gravity = -25.0f;
    public const float jumpSpeed = 7.0f;

    //FireScript 관련 상수
    public const float forwardPower = 50.0f;
    public const float upPower = 20.0f;
    public const float m16FireSpeed = 0.2f;

    //Inventory 관련 상수
    public const int startItemCount = 3;
}

//public class CharacterMove : Photon.PunBehaviour
//{
//    public Animator anim; //Anim

//    Transform tr = null;
//    Rigidbody rb = null;
//    Rader rader = null;
//    CharacterController characterController = null;
//    public Transform cameraTransform;
//    public float moveSpeed = Constants.DefaultMoveSpeed;
//    public float jumpSpeed = Constants.jumpSpeed;
//    public float gravity = Constants.Default_gravity;
//    private float yVelocity = Constants.Default_yVelocity;
//    private float jumpCount = 0.0f;
//    Vector3 moveDirection = Vector3.zero;
//    //
//    public PhotonView pv = null;
//    public Transform camPivot;
//    private Vector3 currPos = Vector3.zero;
//    private Quaternion currRot = Quaternion.identity;

//    //Input H, V 값을 받는 H, V
//    private float inputH;
//    private float inputV;

//    // 마우스 입력

//    //  public Vector2 MouseInput; // 좌 우 입력을 통해 움직임 표현 

//    //bool 값을 통해 run aim을 구별
//    private bool run;
//    private bool aim;
//    private bool sit;
//    private bool crawl;
//    private bool isWalk;

//    void Awake()
//    {
//        //this.enabled = GetComponent<PhotonView>().isMine;
//        tr = GetComponent<Transform>();
//        rb = GetComponent<Rigidbody>();
//        pv = GetComponent<PhotonView>();
//        pv.synchronization = ViewSynchronization.UnreliableOnChange;
//        //pv.ObservedComponents[0] = this;

//        if (pv.isMine)
//        {
//            //rader.playerPos = tr;
//            Camera.main.GetComponent<CameraControl>().Player = this.gameObject;
//            Camera.main.GetComponent<CameraControl>().enabled = true;
//            //Camera.main.GetComponent<CameraControl>().transform.rotation = camPivot.transform.rotation;
//            //rb.centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);
//            //rb.isKinematic = true;
//        }
//        else
//        {
//            //rb.isKinematic = false;
//        }
//        currPos = tr.position;
//        currRot = tr.rotation;
//    }
//    // Use this for initialization
//    void Start()
//    {
//        characterController = GetComponent<CharacterController>();
//        anim = GetComponent<Animator>(); //Anim
//        //bool 값 초기화
//        run = false;
//        aim = false;
//        sit = false;
//        crawl = false;
//        isWalk = false;
//    }

//    void FixedUpdate()
//    {
//        if (!pv.isMine)
//        {
//            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 0.3f);
//            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 0.3f);
//        }
//        else
//        {
//            pv.RPC("move", PhotonTargets.Others, null);
//            //근접 공격
//            if (Input.GetKeyDown(KeyCode.V))
//            {
//                anim.Play("MELEE_ATTACK", -1, 0f);
//            }

//            //-----------------------------------------                                                                 
//            //에임 공격

//            if (aim == false && Input.GetMouseButtonDown(1))
//            {

//                anim.Play("AIM", -1, 0f);

//                aim = true;


//            }

//            else if (aim == true && Input.GetMouseButton(0))
//            {
//                anim.Play("AIM_SHOT", -1, 0f);
//            }

//            else if (aim == true && Input.GetMouseButtonDown(1))
//            {
//                aim = false;
//            }
//            //-----------------------------------------
//            //논 에임 공격
//            if (aim == false && Input.GetMouseButtonDown(0))
//            /*aim == false && Input.GetMouseButtonDown(0)*/
//            {
//                anim.Play("NONE_AIM", -1, 0f);
//            }

//            // 앉기
//            if (sit == false && Input.GetKeyDown(KeyCode.LeftControl))
//            {
//                anim.Play("sit_down", -1, 0f);
//                moveSpeed = Constants.SitMoveSpeed;
//                sit = true;

//            }
//            else if (sit == true && Input.GetKeyUp(KeyCode.LeftControl))
//            {
//                anim.Play("sit_up", -1, 0f);
//                moveSpeed = Constants.DefaultMoveSpeed;
//                sit = false;
//            }
//            //-----------------------------------------
//            inputH = Input.GetAxis("Horizontal");
//            inputV = Input.GetAxis("Vertical");

//            anim.SetFloat("inputH", inputH);
//            anim.SetFloat("inputV", inputV);
//            anim.SetBool("run", run);
//            anim.SetBool("aim", aim);
//            anim.SetBool("crawl", crawl);
//            anim.SetBool("sit", sit);
//            anim.SetBool("isWalk", isWalk);
//        }

//    } // End of Update

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.isWriting)
//        {
//            // We own this player: send the others our data
//            stream.SendNext(tr.position);
//            stream.SendNext(tr.rotation);
//        }
//        else
//        {
//            // Network player, receive data
//            currPos = (Vector3)stream.ReceiveNext();
//            currRot = (Quaternion)stream.ReceiveNext();
//        }
//    }
//    [PunRPC]
//    void move()
//    {
//        float x = Input.GetAxis("Horizontal");
//        float z = Input.GetAxis("Vertical");
//        moveDirection = new Vector3(x, 0, z);
//        moveDirection = cameraTransform.TransformDirection(moveDirection);

//        //달리기 관련 함수
//        runCheck();

//        //점프 관련 함수
//        jumpCheck();

//        yVelocity += (gravity * Time.deltaTime);
//        moveDirection.y = yVelocity;
//        characterController.Move(moveDirection * (Time.deltaTime * 0.3f));
//    }
//    void runCheck()
//    {
//        //-----------------------------------------
//        //그냥 걷기

//        if (inputH != 0 || inputV != 0)
//        {
//            isWalk = true;
//        }
//        else
//        {
//            isWalk = false;
//        }
//        //-----------------------------------------
//        //달리기 관련 부분
//        //-----------------------------------------
//        if (Input.GetKey(KeyCode.LeftShift))
//        {
//            moveSpeed += Constants.AddMoveSpeed;
//            //달리기가 빨라지다가 최대속도를 넘을 시 최대 속도를 유지
//            if (moveSpeed >= Constants.MaxMoveSpeed)
//            {
//                moveSpeed = Constants.MaxMoveSpeed;
//            }
//            run = true;
//        }
//        else if (Input.GetKeyUp(KeyCode.LeftShift))
//        {
//            moveSpeed = Constants.DefaultMoveSpeed;
//            run = false;
//        }
//        moveDirection *= moveSpeed;
//    }
//    void jumpCheck()
//    {
//        //-----------------------------------------
//        //점프
//        if (Input.GetButtonDown("Jump") && jumpCount < Constants.jumpCountMax)
//        {
//            anim.Play("JUMP01", -1, 0f);
//            //점프 애니메이션 수정
//            yVelocity = jumpSpeed;
//            jumpCount++;
//            //Debug.Log(jumpCount);
//        }

//        if (characterController.isGrounded == true)
//        {
//            yVelocity = Constants.Default_yVelocity;
//            jumpCount = 0.0f;
//            //Debug.Log(jumpCount);
//        }
//        //-----------------------------------------
//    }

//}


public class CharacterMove : Photon.PunBehaviour
{
    public Animator anim; //Anim

    Transform tr = null;
    Rigidbody rb = null;
    Rader rader = null;
    CharacterController characterController = null;
    public Transform cameraTransform;
    public float moveSpeed = Constants.DefaultMoveSpeed;
    public float jumpSpeed = Constants.jumpSpeed;
    public float gravity = Constants.Default_gravity;
    private float yVelocity = Constants.Default_yVelocity;
    private float jumpCount = 0.0f;
    Vector3 moveDirection = Vector3.zero;
    //
    public PhotonView pv = null;
    public Transform camPivot;
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    PlayerState ps = null;

    //bool 값을 통해 run aim을 구별
    private bool run;
    private bool aim;
    private bool sit;
    private bool crawl;
    private bool isWalk;

    void Awake()
    {
        //this.enabled = GetComponent<PhotonView>().isMine;
        ps = GetComponent<PlayerState>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        //pv.ObservedComponents[0] = this;

        if (pv.isMine)
        {
            //rader.playerPos = tr;
            Camera.main.GetComponent<CameraControl>().Player = this.gameObject;
            Camera.main.GetComponent<CameraControl>().enabled = true;
        }
        else
        {
            rb.isKinematic = false;
            this.enabled = false;
        }
        currPos = tr.position;
        currRot = tr.rotation;
    }
    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); //Anim

        //bool 값 초기화

        run = false;
        aim = false;
        sit = false;
        crawl = false;
        isWalk = false;
    }

    void FixedUpdate()
    {
        //if (ps.isDead)
        //{
        //    return;
        //}
        if (!pv.isMine || ps.isDead)
        {
            //return;
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 0.5f);
            tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 0.5f);
            return;
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            moveDirection = new Vector3(x, 0, z);
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            runCheck();
            animCheck(x, z);
            yVelocity += (gravity * Time.deltaTime);
            moveDirection.y = yVelocity;
            characterController.Move(moveDirection * Time.deltaTime);

            //-----------------------------------------
            anim.SetFloat("inputH", x);
            anim.SetFloat("inputV", z);

            anim.SetBool("run", run);
            anim.SetBool("aim", aim);
            anim.SetBool("crawl", crawl);
            anim.SetBool("sit", sit);
            anim.SetBool("isWalk", isWalk);
        }
    } // End of Update
    void animCheck(float x, float z)
    {
        //-----------------------------------------
        //그냥 걷기

        if (x != 0 || z != 0)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }
        //-----------------------------------------
        //점프
        if (Input.GetButtonDown("Jump") && jumpCount < Constants.jumpCountMax)
        {
            anim.Play("JUMP01", -1, 0f);
            //점프 애니메이션 수정
            yVelocity = jumpSpeed;
            jumpCount++;
            Debug.Log("jumpCount : " + jumpCount);
            rb.isKinematic = false;
            ps.isGrounded = false;
        }
        //if (characterController.isGrounded == true)
        if (ps.isGrounded == true)
        {
            yVelocity = Constants.Default_yVelocity;
            jumpCount = 0.0f;
            rb.isKinematic = true;
            Debug.Log("땅 착지 : "+jumpCount);
        }
        //-----------------------------------------
        //근접 공격
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.Play("MELEE_ATTACK", -1, 0f);
        }

        //-----------------------------------------                                                                 
        //에임 공격

        if (aim == false && Input.GetMouseButtonDown(1))
        {

            anim.Play("AIM", -1, 0f);

            aim = true;


        }

        else if (aim == true && Input.GetMouseButton(0))
        {
            anim.Play("AIM_SHOT", -1, 0f);
        }

        else if (aim == true && Input.GetMouseButtonDown(1))
        {
            aim = false;
        }
        //-----------------------------------------
        //논 에임 공격
        if (aim == false && Input.GetMouseButtonDown(0))
        /*aim == false && Input.GetMouseButtonDown(0)*/
        {
            anim.Play("NONE_AIM", -1, 0f);
        }
        //-----------------------------------------

        //-----------------------------------------
        // 앉기
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.Play("sit_down", -1, 0f);
            moveSpeed = Constants.SitMoveSpeed;
            sit = true;

        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.Play("sit_up", -1, 0f);
            moveSpeed = Constants.DefaultMoveSpeed;
            sit = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            // Network player, receive data
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void runCheck()
    {
        //달리기 관련 부분
        //-----------------------------------------
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed += Constants.AddMoveSpeed;
            //달리기가 빨라지다가 최대속도를 넘을 시 최대 속도를 유지
            if (moveSpeed >= Constants.MaxMoveSpeed)
            {
                moveSpeed = Constants.MaxMoveSpeed;
            }
            run = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = Constants.DefaultMoveSpeed;
            run = false;
        }
        moveDirection *= moveSpeed;
    }
    void jumpCheck()
    {
        
        if (characterController.isGrounded == true)
        {
            yVelocity = Constants.Default_yVelocity;
            jumpCount = 0.0f;
        }
        if (Input.GetButtonDown("Jump") && jumpCount < Constants.jumpCountMax)
        {
            yVelocity = jumpSpeed;
            jumpCount++;
        }
    }
}