using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
{
    public Animator anim; //Anim
    public GameObject scopeOverlay; //scope 
    public GameObject WeaponsCamera;
    public Camera mainCamera;

    public float scopedFOV = 79f;

    private float normalFOV;

    CharacterController characterController = null;
    public Transform cameraTransform;
    public float moveSpeed = Constants.DefaultMoveSpeed;
    public float jumpSpeed = Constants.jumpSpeed;
    public float gravity = Constants.Default_gravity;
    private float yVelocity = Constants.Default_yVelocity;
    private float jumpCount = 0f;

    //Input H, V 값을 받는 H, V
    private float inputH;
    private float inputV;

    // 마우스 입력

  //  public Vector2 MouseInput; // 좌 우 입력을 통해 움직임 표현 

    //bool 값을 통해 run aim을 구별
    private bool run;
    private bool aim;
    private bool sit;
    private bool crawl;
    private bool isWalk;

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

    void Update()
    {

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
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
            //anim.Play("IDLE", -1, 0f); 이 코드가 살아나면 walk -> run 사이에 idle이 들어가게되서 부자연스럽다.
            moveSpeed = Constants.DefaultMoveSpeed;
        }

        moveDirection *= moveSpeed;

        //ScoreController.Instance.currentScore += 1000;

        
        //-----------------------------------------
        //그냥 걷기

        if (inputH != 0 || inputV != 0)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }
        //-----------------------------------------
        //점프
        if (Input.GetButtonDown("Jump") && jumpCount <  Constants.jumpCountMax)
        {
            anim.Play("JUMP01", -1, 0f);
            //점프 애니메이션 수정
            yVelocity = jumpSpeed;
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
        }
        //-----------------------------------------
        //근접 공격
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.Play("MELEE_ATTACK", -1, 0f);

        }

        //-----------------------------------------                                                                 
        //에임 공격
      
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
        {
            aim = false;

            scopeOverlay.SetActive(aim);

            WeaponsCamera.SetActive(false);

            normalFOV = mainCamera.fieldOfView;
            mainCamera.fieldOfView = scopedFOV;
        }
        //-----------------------------------------
        //논 에임 공격
        if (aim==false && Input.GetMouseButtonDown(0))
            /*aim == false && Input.GetMouseButtonDown(0)*/
        { 
            anim.Play("NONE_AIM", -1, 0f);
        }
        //-----------------------------------------

        //-----------------------------------------
        // 앉기
        if (sit==false && Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.Play("sit_down", -1, 0f);
            moveSpeed = Constants.SitMoveSpeed;
            sit = true;

        }
        else if(sit==true && Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.Play("sit_up", -1, 0f);
            moveSpeed = Constants.DefaultMoveSpeed;
            sit = false;
        }
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
