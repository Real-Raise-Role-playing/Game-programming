using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMove : Photon.MonoBehaviour
{
    public Animator anim; //Anim
    public GameObject scopeOverlay; //scope 
    public float scopedFOV = 10.0f;
    private float normalFOV;
    //Transform tr = null;
    Rigidbody rb = null;
    CharacterController characterController = null;
    PlayerState ps = null;
    OptionManager om = null;
    FireScript fs = null;
    //StateUIControl suc = null;
    StateBarManager sbm = null;
    CheckCollider cc = null;
    public Transform cameraTransform;
    public float moveSpeed = Constants.DefaultMoveSpeed;
    public float jumpSpeed = Constants.jumpSpeed;
    public float gravity = Constants.Default_gravity;
    private float yVelocity = Constants.Default_yVelocity;
    private float jumpCount = 0.0f;
    Vector3 moveDirection = Vector3.zero;
    //
    private float nextAttack = 0.0f;
    private PhotonView pv = null;
    public Transform camPivot;

    //bool 값을 통해 run aim을 구별
    //이동 변수
    [HideInInspector]
    public float x, z;
    //float rotationX;
    float rotationY;

    [HideInInspector]
    public bool run = false;
    [HideInInspector]
    public bool aim = false;
    [HideInInspector]
    public bool sit = false;
    [HideInInspector]
    public bool isWalk = false;
    [HideInInspector]
    public bool jump = false;
    [HideInInspector]
    public bool tilt_L = false;
    [HideInInspector]
    public bool tilt_R = false;
    [HideInInspector]
    public bool attack = false;
    [HideInInspector]
    public bool pickUp = false;
    [HideInInspector]
    public bool death = false;
    [HideInInspector]
    public bool melee_attack = false;

    public CapsuleCollider knifeCollider;
    //public bool crawl = false;
    void Awake()
    {
        knifeCollider.gameObject.SetActive(false);
        pv = GetComponent<PhotonView>();
        if (pv.isMine)
        {
            //rader.playerPos = tr;
            ps = GetComponent<PlayerState>();
            sbm = GetComponentInChildren<StateBarManager>();
            rb = GetComponent<Rigidbody>();
            om = GetComponent<OptionManager>();
            fs = GetComponent<FireScript>();
            cc = GetComponent<CheckCollider>();
            Camera.main.GetComponent<CameraControl>().Player = this.gameObject;
            Camera.main.GetComponent<CameraControl>().enabled = true;
        }
        else
        {
            //anim = null;
            scopeOverlay = null;
            cameraTransform = null;
            camPivot = null;
            //rb.isKinematic = true;
            //this.enabled = false;
        }
    }

    void Start()
    {
        if (pv.isMine)
        {
            characterController = GetComponent<CharacterController>();
            cameraTransform = Camera.main.GetComponent<Transform>();
            normalFOV = Camera.main.fieldOfView;
        }
        //rotationX = 0.0f;
        rotationY = 0.0f;
    }

    void FixedUpdate()
    {
        if (!pv.isMine || ps.playerStateNum == Constants.DEAD)
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

                rotationY = Input.GetAxis("Mouse X") * Constants.defaultSensitivity * Time.deltaTime;
                //Debug.Log("rotationY : " + rotationY);
                //캐릭터 회전 관련 마우스 X축 (좌 <-> 우)
                transform.Rotate(Vector3.up * rotationY);
                //AnimFloat("RotationX", rotationX);
                AnimFloat("RotationY", rotationY);

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
            RunCheck();
            //수정해야함 Check에서 뺴야함
            AnimCheck(x, z);
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
        pv.RPC("OtherAnimPlay", PhotonTargets.Others, animName, layer, time);
    }

    [PunRPC]
    void OtherAnimPlay(string animName, int layer, float time)
    {
        //if (om.InventoryOn)
        //{
        //    return;
        //}
        anim.Play(animName, layer, time);
    }

    public void AnimFloat(string animName, float value)
    {
        if (!om.InventoryOn && animName != "death")
        {
            anim.SetFloat(animName, value);
            pv.RPC("OtherAnimFloat", PhotonTargets.Others, animName, value);
        }
    }
    [PunRPC]
    void OtherAnimFloat(string animName, float value)
    {
        anim.SetFloat(animName, value);
    }

    public void AnimBool(string animName, bool check)
    {
        //인벤토리가 열리면 IDLE상태로 해주기위함. 인벤이 열렸어도 죽음처리는 해야하기에
        if (om.InventoryOn && animName != "death")
        {
            check = false;
        }
        anim.SetBool(animName, check);
        pv.RPC("OtherAnimBool", PhotonTargets.Others, animName, check);
    }

    [PunRPC]
    void OtherAnimBool(string animName, bool check)
    {
        anim.SetBool(animName, check);
    }

    void AnimCheck(float x, float z)
    {
        if (!om.InventoryOn)
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
            if (Input.GetKeyDown(KeyCode.V) && knifeCollider.gameObject.GetActive() == false)
            {
                //StartCoroutine(delayAnimPlay(1.0f, "MELEE_ATTACK"));
                //if (Time.time > nextAttack)
                //{
                //    nextAttack = Time.time + Constants.meleeAttackSpeed;
                //    AnimPlay("MELEE_ATTACK", -1, 1.0f);
                //}
                //Invoke("meleeAttackDelay", Constants.meleeAttackSpeed-1.5f);
                //Debug.Log("if (Input.GetKeyDown(KeyCode.V) && knifeCollider.gameObject.GetActive() == false) ");
                AnimPlay("MELEE_ATTACK", -1, 1.0f);
                Invoke("meleeAttackDelay1", Constants.meleeAttackSpeed - 1.8f);
                Debug.Log("긁기 내 ID : " + PhotonNetwork.player.ID);
                //StartCoroutine(meleeAttackDelay(true, 0.5f));
            }
            else if (Input.GetKeyUp(KeyCode.V) && knifeCollider.gameObject.GetActive() == true)
            {
                //StartCoroutine(meleeAttackDelay(false, 2.0f));
                //Debug.Log("else if (Input.GetKeyUp(KeyCode.V) && knifeCollider.gameObject.GetActive() == true) 실행");
                Invoke("meleeAttackDelay2", Constants.meleeAttackSpeed - 1.0f);
            }
            else if (knifeCollider.gameObject.GetActive() == true)
            {
                //Debug.Log("else if (knifeCollider.gameObject.GetActive() == true) 실행");
                Invoke("meleeAttackDelay2", Constants.meleeAttackSpeed - 1.5f);
                //meleeAttackDelay2();
            }
            //else if (Input.GetKeyUp(KeyCode.V))
            //{
            //    //melee_attack = false;
            //    meleeAttackDelay(false);
            //}

            //-----------------------------------------                                                                 
            //에임 공격
            if (!aim && Input.GetMouseButtonDown(1))
            {
                Camera.main.fieldOfView = scopedFOV;
                //AnimPlay("AIM", -1, 0f);
                AnimBool("aim", true);
                scopeOverlay.SetActive(true);
                //Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
                ps.renderers[1].enabled = false;
                aim = true;
                //aim = fs.shotState;
                //--------------------------
                //WeaponsCamera.SetActive(true);
                //-----------------------
            }
            else if (aim && Input.GetMouseButtonDown(1))
            {
                AnimBool("aim", false);
                scopeOverlay.SetActive(false);
                Camera.main.fieldOfView = normalFOV;
                //Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Player");
                ps.renderers[1].enabled = true;
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
            else if (!aim && Input.GetMouseButton(0))
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
            if (Input.GetKeyDown(KeyCode.C))
            {
                sit = !sit;
                AnimBool("sit", sit);
                if (sit)
                {
                    moveSpeed = Constants.SitMoveSpeed;
                }
                else
                {
                    moveSpeed = Constants.DefaultMoveSpeed;
                }
            }
            //else if (Input.GetKeyUp(KeyCode.C) && sit)
            //{
            //    //AnimPlay("sit_up", -1, 0f);
            //    moveSpeed = Constants.DefaultMoveSpeed;
            //    sit = false;
            //    AnimBool("sit", false);
            //}
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
            if (!aim && !jump && Input.GetKey(KeyCode.Q))
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
    }

    void RunCheck()
    {
        //달리기 관련 부분
        //-----------------------------------------
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (x != 0 || z != 0)
            {
                moveSpeed += Constants.AddMoveSpeed;
                //달리기가 빨라지다가 최대속도를 넘을 시 최대 속도를 유지
                if (moveSpeed >= Constants.MaxMoveSpeed)
                {
                    moveSpeed = Constants.MaxMoveSpeed;
                }
                run = true;
                if (sbm.HangerBarSlider.value <= 0.001f)
                {
                    sbm.beShotImg.color = sbm.beShotImgColor;
                    ps.currHp -= 1;
                    ps.playerStateUpdate(PhotonNetwork.player.ID);
                }
            }
        }
        else
        {
            if (run)
            {
                StartCoroutine(sbm.delayTime(2.0f));
            }
            //달리기가 빨라지다가 최대속도를 넘을 시 최대 속도를 유지
            moveSpeed -= Constants.AddMoveSpeed;
            //suc.HangerBarSlider.value += 0.3f;
            if (moveSpeed <= Constants.DefaultMoveSpeed)
            {
                moveSpeed = Constants.DefaultMoveSpeed;
            }
            run = false;
        }
    }

    IEnumerator meleeAttackDelay(bool active, float time)
    {
        yield return new WaitForSeconds(time);
        pv.RPC("MelleBool", PhotonTargets.All, active);
    }

    void meleeAttackDelay1()
    {
        pv.RPC("MelleBool", PhotonTargets.All, true);
    }

    void meleeAttackDelay2()
    {
        pv.RPC("MelleBool", PhotonTargets.All, false);
    }

    [PunRPC]
    void MelleBool(bool _melee_attack)
    {
        knifeCollider.gameObject.SetActive(_melee_attack);
        knifeCollider.gameObject.GetComponent<KnifeCheckCollider>().masterViewNum = pv.viewID;
        //knifeCollider.gameObject.GetComponent<KnifeCheckCollider>().masterViewNum = PhotonNetwork.player.ID;
    }

    //IEnumerator delayAnimPlay(float sec,string animName)
    //{
    //    yield return new WaitForSeconds(sec);
    //    AnimPlay("MELEE_ATTACK", -1, 0f);
    //}
}