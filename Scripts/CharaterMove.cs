using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



}

public class CharaterMove : MonoBehaviour {
    CharacterController characterController = null;
    public Transform cameraTransform;
    public float moveSpeed = Constants.DefaultMoveSpeed;
    public float jumpSpeed = Constants.jumpSpeed;
    public float gravity = Constants.Default_gravity;
    private float yVelocity = Constants.Default_yVelocity;
    private float jumpCount = 0.0f;
    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
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
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = Constants.DefaultMoveSpeed;
        }
        moveDirection *= moveSpeed;
        //-----------------------------------------

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
        yVelocity += (gravity * Time.deltaTime);
        moveDirection.y = yVelocity;
        characterController.Move(moveDirection * Time.deltaTime);

    } // End of Update
}
