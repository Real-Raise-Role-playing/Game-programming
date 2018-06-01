using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : Photon.MonoBehaviour
{
    public Transform cameraTransform = null;
    public float forwardPower = Constants.forwardPower;
    public float upPower = Constants.upPower;

    public Transform fireTransform;
    private Vector3 fireVelocity = Vector3.zero;

    GameObject fireObject = null;
    GameObject emptyFireObject = null;
    Rigidbody fireObjectRb = null;
    Rigidbody emptyFireObjectRb = null;

    private bool SingleShot = true;
    private float nextFire = 0.0f;
    float cameraDefaultZoom;
    bool toggle = false;
    PlayerState playerState = null;
    private PhotonView pv = null;
    //오디오 컴포턴트 할당 변수
    private AudioSource sfx = null;
    //Kar98 사운드 파일
    private AudioClip kar98 = null;

    void Awake()
    {
        fireObject = (GameObject)Resources.Load("Bullet1");
        fireObjectRb = fireObject.GetComponent<Rigidbody>();
        emptyFireObject = (GameObject)Resources.Load("Bullet2");
        emptyFireObjectRb = emptyFireObject.GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        sfx = GetComponent<AudioSource>();
        kar98 = Resources.Load<AudioClip>("Sounds\\Kar98");
    }

    // Use this for initialization
    void Start()
    {
        cameraTransform = Camera.main.GetComponent<Transform>();
        //cameraTransform = GameObject.Find("MainCamera").GetComponent<Transform>();
        playerState = GetComponent<PlayerState>(); //죽음 처리 하기 위해 얻어온 컴포넌트
        //this.enabled = GetComponent<PhotonView>().isMine;
        cameraDefaultZoom = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.isMine)
        {
            return;
        }
        Fire();
        /*
        //플레이어 죽음 처리, 일시정지 
        if (PauseManager.gamePause || playerState.isDead)
        {
            return;
        }
        */
    }

    //----------------------함수 정리----------------------------------------------------
    //발사 로직
    void Fire()
    {
        //------------------------------------------------------------------------------------
        //단발, 연발 B키로 선택
        if (Input.GetKeyDown(KeyCode.B))
        {
            SingleShot = !SingleShot;
        }

        //단발
        if (Input.GetMouseButtonDown(0) && SingleShot)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + Constants.m16FireSpeed;
                //photonView.RPC("otherFireOther", PhotonTargets.Others, cameraTransform.forward);
                CreateBullet();
                //pv.RPC("FireOther", PhotonTargets.Others, cameraTransform.forward);
                if (PhotonNetwork.countOfPlayersInRooms <= 1)
                {
                    soloWeaponSound("kar98");
                }
                else
                {
                    pv.RPC("WeaponSound", PhotonTargets.Others, "kar98");
                }
                //WeaponSound("kar98");
                //sfx.PlayOneShot(kar98,1.0f);
                //kar98Sound();
            }
        }
        //연발
        if (Input.GetMouseButton(0) && !SingleShot)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + Constants.m16FireSpeed;
                CreateBullet();
                if (PhotonNetwork.countOfPlayersInRooms <= 1)
                {
                    soloWeaponSound("kar98");
                }
                else
                {
                    pv.RPC("WeaponSound", PhotonTargets.Others, "kar98");
                }
                //sfx.PlayOneShot(kar98,1.0f);
                //kar98Sound();
            }
        }
        //------------------------------------------------------------------------------------
        if (Input.GetMouseButtonDown(1))
        {
            if (toggle == true)
            {
                Camera.main.fieldOfView = cameraDefaultZoom;
            }
            else
            {
                Camera.main.fieldOfView = 10;
            }
            toggle = !toggle;
        }
    }



    //포톤 네트워크 방법
    //void CreateBullet()
    //{
    //    GameObject obj = PhotonNetwork.Instantiate("Bullet1", fireTransform.position, fireTransform.rotation, 0) as GameObject; // Instantiate 는 new와 같은 의미 
    //    Rigidbody rb = obj.GetComponent<Rigidbody>();
    //    rb.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    //}
    //------------------------------------------------------------------------------------

    //void CreateBullet()
    //{
    //    Rigidbody shellInstance = Instantiate(fireObjectRb, fireTransform.position, fireTransform.rotation) as Rigidbody;
    //    shellInstance.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    //    photonView.RPC("FireOther", PhotonTargets.Others, fireTransform.position, shellInstance.velocity);
    //}

    //[PunRPC]
    //void FireOther(Vector3 pos, Vector3 velocity)
    //{
    //    Rigidbody shellInstance = Instantiate(fireObjectRb, pos, fireTransform.rotation) as Rigidbody;
    //    shellInstance.velocity = velocity;
    //}

    //void CreateBullet()
    //{
    //    Rigidbody shellInstance = Instantiate(fireObjectRb, fireTransform.position, fireTransform.rotation) as Rigidbody;
    //    shellInstance.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    //}

    
    void soloWeaponSound(string waepon)
    {
        if (waepon == "kar98")
        {
            sfx.PlayOneShot(kar98, 1.0f);
        }
    }

    [PunRPC]
    void WeaponSound(string waepon)
    {
        if (waepon == "kar98")
        {
            sfx.PlayOneShot(kar98, 1.0f);
        }
    }

    void CreateBullet()
    {
        Rigidbody shellInstance = Instantiate(emptyFireObjectRb, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
        pv.RPC("FireOther", PhotonTargets.Others, cameraTransform.forward);
    }
    [PunRPC]
    void FireOther(Vector3 camForward)
    {
        Rigidbody shellInstance = Instantiate(fireObjectRb, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = (camForward * forwardPower) + (Vector3.up * upPower);
    }
}
