using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : Photon.MonoBehaviour
{
    
    public Animator anim; //Anim

    public GameObject scopeOverlay; //scope 
    public float scopedFOV = 79f;
    private float normalFOV;

    //Transform tr = null;
    Rigidbody rb = null;
    CharacterController characterController = null;
    PlayerState ps = null;
    OptionManager om = null;
    FireScript fs = null;
    StateBarManager sbm = null;
    StateUIControl suc = null;
    CheckCollider cc = null;
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

    //bool 값을 통해 run aim을 구별
    [HideInInspector]
    //이동 변수
    public float x, z;

    public bool run = false;
    public bool aim = false;
    public bool sit = false;
    public bool isWalk = false;
    public bool jump = false;
    public bool tilt_L = false;
    public bool tilt_R = false;
    public bool attack = false;
    public bool pickUp = false;

    //public bool crawl = false;
    void Awake()
    {
        //this.enabled = GetComponent<PhotonView>().isMine;
        ps = GetComponent<PlayerState>();
        suc = GetComponentInChildren<StateUIControl>();
        //sbm = GetComponentInChildren<StateBarManager>();
        //tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        om = GetComponent<OptionManager>();
        fs = GetComponent<FireScript>();
        cc = GetComponent<CheckCollider>();
        if (pv.isMine)
        {
            //rader.playerPos = tr;
            Camera.main.GetComponent<CameraControl>().Player = this.gameObject;
            Camera.main.GetComponent<CameraControl>().enabled = true;
        }
        else
        {
            rb.isKinematic = true;
            //this.enabled = false;
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.GetComponent<Transform>();
        normalFOV = Camera.main.fieldOfView;
    }

    void FixedUpdate()
    {
        //if (ps.playerStateNum == Constants.DEAD)
        //{
        //    return;
        //}
        if (!pv.isMine)
        {
            return;
        }
        else
        {
            //Debug.Log(characterController.isGrounded);
            if (!om.InventoryOn)
            {
                x = Input.GetAxis("Horizontal");
                z = Input.GetAxis("Vertical");
                //Camera.main.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                //rb.transform.eulerAngles = new Vector3(0,Camera.main.transform.rotation.eulerAngles);
                moveDirection = new Vector3(x, 0, z);
                moveDirection = cameraTransform.TransformDirection(moveDirection);
                
                moveDirection *= moveSpeed;
            }
            else
            {
                moveDirection = Vector3.zero;
            }
            yVelocity += (gravity * Time.deltaTime);
            moveDirection.y = yVelocity;
            characterController.Move(moveDirection * Time.deltaTime);
            runCheck();
            animCheck(x, z);
            //-----------------------------------------
        }
    } // End of Update

    public void AnimPlay(string animName, int layer, float time)
    {
        if (om.InventoryOn)
        {
            return;
        }
        anim.Play(animName, layer, time);
        pv.RPC("otherAnimPlay", PhotonTargets.Others, animName, layer, time);
    }
    [PunRPC]
    void otherAnimPlay(string animName, int layer, float time)
    {
        if (om.InventoryOn)
        {
            return;
        }
        anim.Play(animName, layer, time);
    }

    void AnimBool(string animName, bool check)
    {
        //인벤토리가 열리면 IDLE상태로 해주기위함.
        if (om.InventoryOn)
        {
            check = false;
        }
        anim.SetBool(animName, check);
        pv.RPC("otherAnimBool", PhotonTargets.Others, animName, check);
    }
    [PunRPC]
    void otherAnimBool(string animName, bool check)
    {
       //if (animName == "aim")
       // {
       //     aim = check;
       // }
       // //else if (animName == "crawl")
       // //{
       // //    crawl = check;
       // //}
       // else if (animName == "sit")
       // {
       //     sit = check;
       // }
       // else if (animName == "tilt_L")
       // {
       //     tilt = check;
       // }
       // else if (animName == "tilt_R")
       // {
       //     tilt = check;
       // }
        anim.SetBool(animName, check);
    }

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
            //rb.isKinematic = true;
            isWalk = false;
        }
        //-----------------------------------------
        //점프
        if (Input.GetButtonDown("Jump") && jumpCount < Constants.jumpCountMax)
        {
            //if (!jump)
            //{
            //    AnimPlay("JUMP01", -1, 0f);
            //}
            //점프 애니메이션 수정
            //Debug.Log("점프");
            yVelocity = jumpSpeed;
            jumpCount++;
            ps.isGrounded = false;
            jump = true;
            //AnimPlay("JUMP01", -1, 0f);
            AnimBool("jump", jump);
        }
        if (ps.isGrounded == true && jumpCount > 0.0f)
        {
            yVelocity = Constants.Default_yVelocity;
            jumpCount = 0.0f;
            jump = false;
            AnimBool("jump", jump);
        }
        //-----------------------------------------
        //근접 공격
        if (Input.GetKeyDown(KeyCode.V))
        {
            AnimPlay("MELEE_ATTACK", -1, 0f);
        }

        //-----------------------------------------                                                                 
        //에임 공격
        if (!aim && Input.GetMouseButtonDown(1) && !om.InventoryOn)
        {
            Camera.main.fieldOfView = normalFOV;
            AnimPlay("AIM", -1, 0f);
            AnimBool("aim", true);
            scopeOverlay.SetActive(true);
            aim = true;
            //aim = fs.shotState;
            //--------------------------
            //WeaponsCamera.SetActive(true);
            //-----------------------
        }
        else if (aim && Input.GetMouseButtonDown(1) && !om.InventoryOn)
        {
            Camera.main.fieldOfView = scopedFOV;
            AnimBool("aim", false);
            scopeOverlay.SetActive(false);
            aim = false;
            //aim = fs.shotState;
            //WeaponsCamera.SetActive(false);
        }
        //-----------------------------------------
        //논 에임 공격
        if (aim && Input.GetMouseButton(0))
        {
            if (fs.currentBulletCount > 0)
            {
                AnimPlay("AIM_SHOT", -1, 0f);
                attack = true;
            }
        }
        if (!aim && Input.GetMouseButton(0))
        /*aim == false && Input.GetMouseButtonDown(0)*/
        {
            if (fs.currentBulletCount > 0)
            {
                AnimPlay("NONE_AIM", -1, 0f);
            }
        }
        //-----------------------------------------

        //-----------------------------------------
        // 앉기
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            AnimPlay("sit_down", -1, 0f);
            moveSpeed = Constants.SitMoveSpeed;
            sit = true;
            AnimBool("sit", sit);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            AnimPlay("sit_up", -1, 0f);
            moveSpeed = Constants.DefaultMoveSpeed;
            sit = false;
            AnimBool("sit", sit);
        }
        //재장전
        if (!aim && fs.reloadAnimCheck)
        {
            AnimPlay("reload", -1, 0f);
        }
        if (!aim && cc.pickUpAnimCheck)
        {
            AnimPlay("pick_up", -1, 0f);
        }
        //왼쪽 내밀기
        if (!aim && !jump  && Input.GetKey(KeyCode.Q))
        {
            //anim.Play("tilt_L", -1, 0f);
            tilt_L = true;
            AnimBool("tilt_L", tilt_L);
        }
        else if (tilt_L && Input.GetKeyUp(KeyCode.Q))
        {
            tilt_L = false;
            AnimBool("tilt_L", tilt_L);
        }
        if (!aim && !jump && Input.GetKey(KeyCode.E))
        {
            //anim.Play("tilt_R", -1, 0f);
            tilt_R = true;
            AnimBool("tilt_R", tilt_R);
        }
        else
        {
            tilt_R = false;
            AnimBool("tilt_R", tilt_R);
        }

        //-----------------------------------------

    }

    void runCheck()
    {
        //달리기 관련 부분
        //-----------------------------------------
        if (Input.GetKey(KeyCode.LeftShift))
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            suc.HangerBarSlider.value -= 0.5f;

            //sbm.HangerBarSlider.value -= 0.005f;
            //if (sbm.HangerBarSlider.value <= 0.2f)
            if (suc.HangerBarSlider.value <= 2.0f)
            {
                moveSpeed = Constants.DefaultMoveSpeed;
                run = false;
            }
            else
            {
                moveSpeed += Constants.AddMoveSpeed;
                //달리기가 빨라지다가 최대속도를 넘을 시 최대 속도를 유지
                if (moveSpeed >= Constants.MaxMoveSpeed)
                {
                    moveSpeed = Constants.MaxMoveSpeed;
                }
                run = true;
            }
        }
        else
        {
            //달리기가 빨라지다가 최대속도를 넘을 시 최대 속도를 유지
            moveSpeed -= Constants.AddMoveSpeed;
            suc.HangerBarSlider.value += 0.3f;
            //sbm.HangerBarSlider.value += 0.003f;
            if (moveSpeed <= Constants.DefaultMoveSpeed)
            {
                moveSpeed = Constants.DefaultMoveSpeed;
            }
            run = false;
        }
    }
}