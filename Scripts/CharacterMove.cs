using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
//상수화 해놓을 수.
static class Constants  //캐릭터의 기본 정보
{
    //moveSpeed 관련 상수
    public const float DefaultMoveSpeed = 100.0f;
    public const float MaxMoveSpeed = 40.0f;
    public const float AddMoveSpeed = 2.0f;
    public const float SitMoveSpeed = 20.0f; // 앉았을시 캐릭터의 속도

    //Jump & 캐릭터 Y축 관련 상수
    public const float Default_yVelocity = 0.0f;
    public const float jumpCountMax = 1.0f;
    public const float Default_gravity = -25.0f;
    public const float jumpSpeed = 7.0f;

    //FireScript 관련 상수
    public const float forwardPower = 20.0f;
    public const float upPower = 5.0f;
    
}

public class CharacterMove : Singleton<CharacterMove>
=======
public class CharacterMove : Photon.MonoBehaviour
>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
{
    //이동 변수
    float x,z;
    public Animator anim; //Anim
    public GameObject scopeOverlay; //scope 
    public GameObject WeaponsCamera;
    public Camera mainCamera;

    public float scopedFOV = 79f;

    private float normalFOV;

    CharacterController characterController = null;
    PlayerState ps = null;
    OptionManager om = null;
    public Transform cameraTransform;

    public float moveSpeed = Constants.DefaultMoveSpeed;
    public float jumpSpeed = Constants.jumpSpeed;
    public float gravity = Constants.Default_gravity;
    private float yVelocity = Constants.Default_yVelocity;
<<<<<<< HEAD
    private float jumpCount = 0f;

    //Input H, V 값을 받는 H, V
    private float inputH;
    private float inputV;

    // 마우스 입력

  //  public Vector2 MouseInput; // 좌 우 입력을 통해 움직임 표현 
=======
    private float jumpCount = 0.0f;
    Vector3 moveDirection = Vector3.zero;
    //
    public PhotonView pv = null;
    public Transform camPivot;

    //삭제요망
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2

    //bool 값을 통해 run aim을 구별
    private bool run;
    private bool aim;
    private bool sit;
    private bool crawl;
    private bool isWalk;

<<<<<<< HEAD
    // Use this for initialization
=======
    void Awake()
    {
        //this.enabled = GetComponent<PhotonView>().isMine;
        ps = GetComponent<PlayerState>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        om = GetComponent<OptionManager>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        if (pv.isMine)
        {
            //rader.playerPos = tr;
            Camera.main.GetComponent<CameraControl>().Player = this.gameObject;
            Camera.main.GetComponent<CameraControl>().enabled = true;
        }
        else
        {
            rb.isKinematic = true;
            this.enabled = false;
        }
        //삭제요망
        currPos = tr.position;
        currRot = tr.rotation;
    }

>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
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

    void Update()
    {
<<<<<<< HEAD

        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        float x = inputH * 20f * Time.deltaTime;
        float z = inputV * 20f * Time.deltaTime;

      //  MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); // 마우스 입력
        Vector3 moveDirection = new Vector3(x, 0, z);


        moveDirection = cameraTransform.TransformDirection(moveDirection);


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
=======
        if (ps.playerStateNum == Constants.DEAD)
        {
            return;
        }
        if (!pv.isMine)
        {
            //tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 0.5f);
            //tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 0.5f);
            return;
>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
<<<<<<< HEAD
            run = false;
            //anim.Play("IDLE", -1, 0f); 이 코드가 살아나면 walk -> run 사이에 idle이 들어가게되서 부자연스럽다.
            moveSpeed = Constants.DefaultMoveSpeed;
        }

        moveDirection *= moveSpeed;

        //ScoreController.Instance.currentScore += 1000;

        
=======
            if (!om.InventoryOn)
            {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            moveDirection = new Vector3(x, 0, z);
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            runCheck();
            animCheck(x, z);
            moveDirection *= moveSpeed;
            yVelocity += (gravity * Time.deltaTime);
            moveDirection.y = yVelocity;
            characterController.Move(moveDirection * Time.deltaTime);
            }

            //-----------------------------------------
            //anim.SetFloat("inputH", x);
            //anim.SetFloat("inputV", z);
            //anim.SetBool("run", run);
            //anim.SetBool("aim", aim);
            //anim.SetBool("crawl", crawl);
            //anim.SetBool("sit", sit);
            //anim.SetBool("isWalk", isWalk);

            AnimFloat("inputH", x);
            AnimFloat("inputV", z);
            AnimBool("run", run);
            AnimBool("aim", aim);
            AnimBool("crawl", crawl);
            AnimBool("sit", sit);
            AnimBool("isWalk", isWalk);
        }
    } // End of Update

    void AnimFloat(string name, float value)
    {
        anim.SetFloat(name, value);
        pv.RPC("otherAnimFloat", PhotonTargets.Others, name, value);
    }
    [PunRPC]
    void otherAnimFloat(string name, float value)
    {
        //if (name == "inputH")
        //{
        //    x = value;
        //}
        //else
        //{
        //    z = value;
        //}
        anim.SetFloat(name, value);
    }

    void AnimPlay(string animName, int layer ,float time)
    {
        if (om.InventoryOn) {
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
        if (animName == "run")
        {
            run = check;
        }
        else if (animName == "aim")
        {
            aim = check;
        }
        else if (animName == "crawl")
        {
            crawl = check;
        }
        else if (animName == "sit")
        {
            sit = check;
        }
        else
        {
            isWalk = check;
        }
        anim.SetBool(animName, check);
    }

    

    void animCheck(float x, float z)
    {
>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
        //-----------------------------------------
        //그냥 걷기

        if (inputH != 0 || inputV != 0)
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
        if (Input.GetButtonDown("Jump") && jumpCount <  Constants.jumpCountMax)
        {
            AnimPlay("JUMP01", -1, 0f);
            //점프 애니메이션 수정
            yVelocity = jumpSpeed;
<<<<<<< HEAD
            jumpCount = 1.0f;
            //Debug.Log(jumpCount);
        }
        yVelocity += (gravity * Time.deltaTime);
        moveDirection.y = yVelocity;
        characterController.Move(moveDirection * Time.deltaTime);

        if (characterController.isGrounded == true)
        {
            yVelocity = Constants.Default_yVelocity;
            jumpCount = 0.0f;
            //Debug.Log(jumpCount);
=======
            jumpCount++;
            //rb.isKinematic = false;
            ps.isGrounded = false;
        }
        //if (characterController.isGrounded == true)
        if (ps.isGrounded == true && jumpCount > 0.0f)
        {
            yVelocity = Constants.Default_yVelocity;
            jumpCount = 0.0f;
            //rb.isKinematic = true;
>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
        }
        //-----------------------------------------
        //근접 공격
        if (Input.GetKeyDown(KeyCode.V))
        {
<<<<<<< HEAD
            anim.Play("MELEE_ATTACK", -1, 0f);

=======
            AnimPlay("MELEE_ATTACK", -1, 0f);
>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
        }

        //-----------------------------------------                                                                 
        //에임 공격
<<<<<<< HEAD
      
        if(aim==false && Input.GetMouseButtonDown(1))
        {
           
            anim.Play("AIM", -1, 0f);

            aim = true;

            scopeOverlay.SetActive(aim);
            WeaponsCamera.SetActive(true);

           // mainCamera.fieldOfView = normalFOV;

        }

        else if(aim==true && Input.GetMouseButton(0))
        {
            anim.Play("AIM_SHOT", -1, 0f);

        }

        else if(aim==true && Input.GetMouseButtonDown(1))
=======
        if (!aim && Input.GetMouseButtonDown(1))
        {
            AnimPlay("AIM", -1, 0f);
            aim = true;
        } else if (aim && Input.GetMouseButton(0))
        {
            AnimPlay("AIM_SHOT", -1, 0f);
        } else if (aim && Input.GetMouseButtonDown(1))
>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
        {
            aim = false;

            scopeOverlay.SetActive(aim);

            WeaponsCamera.SetActive(false);

            normalFOV = mainCamera.fieldOfView;
            mainCamera.fieldOfView = scopedFOV;
        }
        //-----------------------------------------
        //논 에임 공격
<<<<<<< HEAD
        if (aim==false && Input.GetMouseButtonDown(0))
            /*aim == false && Input.GetMouseButtonDown(0)*/
        { 
            anim.Play("NONE_AIM", -1, 0f);
=======
        if (!aim && Input.GetMouseButtonDown(0))
        /*aim == false && Input.GetMouseButtonDown(0)*/
        {
            AnimPlay("NONE_AIM", -1, 0f);
>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
        }
        //-----------------------------------------

        //-----------------------------------------
        // 앉기
        if (sit==false && Input.GetKeyDown(KeyCode.LeftControl))
        {
            AnimPlay("sit_down", -1, 0f);
            moveSpeed = Constants.SitMoveSpeed;
            sit = true;

        }
        else if(sit==true && Input.GetKeyUp(KeyCode.LeftControl))
        {
            AnimPlay("sit_up", -1, 0f);
            moveSpeed = Constants.DefaultMoveSpeed;
            sit = false;
        }
<<<<<<< HEAD
        //-----------------------------------------
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);

        anim.SetBool("run", run);
        anim.SetBool("aim", aim);
        anim.SetBool("crawl", crawl);
        anim.SetBool("sit", sit);
        anim.SetBool("isWalk", isWalk);
       
    } // End of Update
}
=======
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
>>>>>>> afeba32385cd9294e44f6fc93cebcc5a73e301c2
