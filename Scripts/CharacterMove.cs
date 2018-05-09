using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : Photon.PunBehaviour
{
    public Animator anim; //Anim

    Transform tr = null;
    Rigidbody rb = null;
    Rader rader = null;
    CharacterController characterController = null;
    PlayerState ps = null;
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

    //삭제요망
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;


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
        //삭제요망
        currPos = tr.position;
        currRot = tr.rotation;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); //Anim
        cameraTransform = Camera.main.GetComponent<Transform>();
        //bool 값 초기화

        run = false;
        aim = false;
        sit = false;
        crawl = false;
        isWalk = false;
    }

    void FixedUpdate()
    {
        if (ps.playerStateNum == Constants.DEAD)
        {
            return;
        }
        if (!pv.isMine)
        {
            //tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 0.5f);
            //tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 0.5f);
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
            moveDirection *= moveSpeed;
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
            rb.isKinematic = true;
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
            rb.isKinematic = false;
            ps.isGrounded = false;
        }
        //if (characterController.isGrounded == true)
        if (ps.isGrounded == true && jumpCount > 0.0f)
        {
            yVelocity = Constants.Default_yVelocity;
            jumpCount = 0.0f;
            rb.isKinematic = true;
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