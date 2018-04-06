using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
//SmoothFollow 스크립트를 사용하기 위한 네임스페이스 추가
//상수화 해놓을 수.
static class Constants
{
    //moveSpeed 관련 상수
    public const float DefaultMoveSpeed = 7.0f;
    public const float MaxMoveSpeed = 12.0f;
    public const float AddMoveSpeed = 0.2f;

    //Jump & 캐릭터 Y축 관련 상수
    public const float Default_yVelocity = 0.0f;
    public const float jumpCountMax = 1.0f;
    public const float Default_gravity = -20.0f;
    public const float jumpSpeed = 10.0f;

    //FireScript 관련 상수
    public const float forwardPower = 20.0f;
    public const float upPower = 5.0f;
    public const float m16FireSpeed = 0.2f;

    //Inventory 관련 상수
    public const int startItemCount = 3;
}

public class CharacterMove : Photon.MonoBehaviour
{
    public static CharacterMove instance;


    CharacterController characterController = null;
    public Transform cameraTransform;
    public float moveSpeed = Constants.DefaultMoveSpeed;
    public float jumpSpeed = Constants.jumpSpeed;
    public float gravity = Constants.Default_gravity;
    private float yVelocity = Constants.Default_yVelocity;
    private float jumpCount = 0.0f;
    Vector3 moveDirection;
    //
    public PhotonView pv = null;
    public Transform camPivot;
    // Use this for initialization
    void Awake()
    {
        instance = this;
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        this.enabled = GetComponent<PhotonView>().isMine;

        pv = GetComponent<PhotonView>();
        if (pv.isMine)
        {
            Camera.main.GetComponent<CameraControl>().Player = this.gameObject;
            Camera.main.GetComponent<CameraControl>().enabled = true;
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
        }
    }


    void FixedUpdate()
    {
        //if (!pv.isMine) { return; }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveDirection = new Vector3(x, 0, z);
        moveDirection = cameraTransform.TransformDirection(moveDirection);

        //달리기 관련 함수
        runCheck();

        //점프 관련 함수
        jumpCheck();

        yVelocity += (gravity * Time.deltaTime);
        moveDirection.y = yVelocity;
        characterController.Move(moveDirection * Time.deltaTime);

    } // End of FixedUpdate

    public Vector3 _MoveDirection
    {
        get { return moveDirection; }
        set { moveDirection = value; }
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
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = Constants.DefaultMoveSpeed;
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