﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : Photon.MonoBehaviour
{
    
    public Animator anim; //Anim

    public GameObject scopeOverlay; //scope 
    //public GameObject WeaponsCamera;
    public float scopedFOV = 79f;
    private float normalFOV;

    //Transform tr = null;
    Rigidbody rb = null;
    //Rader rader = null;
    CharacterController characterController = null;
    PlayerState ps = null;
    OptionManager om = null;
    FireScript fs = null;
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
    //public bool crawl = false;
    public bool isWalk = false;

    void Awake()
    {
        //this.enabled = GetComponent<PhotonView>().isMine;
        ps = GetComponent<PlayerState>();
        //tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        om = GetComponent<OptionManager>();
        fs = GetComponent<FireScript>();
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
        //anim = GetComponent<Animator>(); //Anim
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
            runCheck();
            animCheck(x, z);
            yVelocity += (gravity * Time.deltaTime);
            moveDirection.y = yVelocity;
            characterController.Move(moveDirection * Time.deltaTime);
            //-----------------------------------------
        }
    } // End of Update

    void AnimPlay(string animName, int layer, float time)
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
       if (animName == "aim")
        {
            aim = check;
        }
        //else if (animName == "crawl")
        //{
        //    crawl = check;
        //}
        else if (animName == "sit")
        {
            sit = check;
        }
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
            //AnimPlay("JUMP01", -1, 0f);
            //점프 애니메이션 수정
            yVelocity = jumpSpeed;
            jumpCount++;
            ps.isGrounded = false;
        }
        if (ps.isGrounded == true && jumpCount > 0.0f)
        {
            yVelocity = Constants.Default_yVelocity;
            jumpCount = 0.0f;
        }
        //-----------------------------------------
        //근접 공격
        if (Input.GetKeyDown(KeyCode.V))
        {
            AnimPlay("MELEE_ATTACK", -1, 0f);
        }

        //-----------------------------------------                                                                 
        //에임 공격
        if (!aim && Input.GetMouseButtonDown(1))
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
        else if (aim && Input.GetMouseButtonDown(1))
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
            AnimPlay("AIM_SHOT", -1, 0f);
        }
        else if (!aim && Input.GetMouseButtonDown(0))
        /*aim == false && Input.GetMouseButtonDown(0)*/
        {
            AnimPlay("NONE_AIM", -1, 0f);
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
    }

    void runCheck()
    {
        //달리기 관련 부분
        //-----------------------------------------
        if (Input.GetKey(KeyCode.LeftShift))
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed += Constants.AddMoveSpeed;
            //달리기가 빨라지다가 최대속도를 넘을 시 최대 속도를 유지
            if (moveSpeed >= Constants.MaxMoveSpeed)
            {
                moveSpeed = Constants.MaxMoveSpeed;
            }
                run = true;
        }
        else
        {
            //달리기가 빨라지다가 최대속도를 넘을 시 최대 속도를 유지
            if (moveSpeed <= Constants.DefaultMoveSpeed)
            {
                moveSpeed = Constants.DefaultMoveSpeed;
                run = false;
            }
            moveSpeed -= Constants.AddMoveSpeed;
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