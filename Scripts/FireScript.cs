using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : Photon.MonoBehaviour
{
    public Transform cameraTransform = null;
    public float forwardPower = Constants.forwardPower;
    public float upPower = Constants.upPower;
    public Transform fireTransform;
    GameObject fireObject = null;
    public Rigidbody fireObjectRb = null;
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

    private void Awake()
    {
        fireObject = (GameObject)Resources.Load("Bullet1");
        fireObjectRb = fireObject.GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        sfx = GetComponent<AudioSource>();
        kar98 = Resources.Load<AudioClip>("Sounds\\Kar98");
    }

    // Use this for initialization
    void Start()
    {
        cameraTransform = Camera.main.GetComponent<Transform>();
        playerState = GetComponent<PlayerState>(); //죽음 처리 하기 위해 얻어온 컴포넌트
        this.enabled = GetComponent<PhotonView>().isMine;
        cameraDefaultZoom = Camera.main.fieldOfView;
    }


    // Update is called once per frame
    void Update()
    {
        if (!pv.isMine) {
            return;
        }
        else
        {
            Fire();
        }
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
        if (SingleShot)
        {
            //단발
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + Constants.m16FireSpeed;
                    CreateBullet();
                    //sfx.PlayOneShot(kar98,1.0f);
                    kar98Sound();
                }
            }
        }
        else
        {
            //연발
            if (Input.GetMouseButton(0))
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + Constants.m16FireSpeed;
                    CreateBullet();
                    //sfx.PlayOneShot(kar98,1.0f);
                    kar98Sound();
                }
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

    void kar98Sound()
    {
        sfx.PlayOneShot(kar98, 1.0f);
        pv.RPC("otherkar98Sound", PhotonTargets.Others, null);
    }
    [PunRPC]
    void otherkar98Sound()
    {
        sfx.PlayOneShot(kar98, 1.0f);
    }

    //포톤 네트워크 방법
    //void CreateBullet()
    //{
    //    GameObject obj = PhotonNetwork.Instantiate("Bullet1", fireTransform.position, fireTransform.rotation, 0) as GameObject; // Instantiate 는 new와 같은 의미 
    //    Rigidbody rb = obj.GetComponent<Rigidbody>();
    //    rb.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    //}
    //------------------------------------------------------------------------------------
    void CreateBullet()
    {
        Rigidbody shellInstance = Instantiate(fireObjectRb, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
        photonView.RPC("FireOther", PhotonTargets.Others, fireTransform.position, shellInstance.velocity);
    }

    [PunRPC]
    void FireOther(Vector3 pos, Vector3 velocity)
    {
        Rigidbody shellInstance = Instantiate(fireObjectRb, pos, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = velocity;
    }

    //Phton View를 Bullet1에 넣어 구별 하기위해 시도
    //void CreateBullet()
    //{
    //    GameObject obj = PhotonNetwork.Instantiate("Bullet1", fireTransform.position, fireTransform.rotation, 0) as GameObject; // Instantiate 는 new와 같은 의미 
    //    Rigidbody rb = obj.GetComponent<Rigidbody>();
    //    rb.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    //    photonView.RPC("FireOther", PhotonTargets.Others, fireTransform.position, rb.velocity);
    //}

    //[PunRPC]
    //void FireOther(Vector3 pos, Vector3 velocity)
    //{
    //    GameObject obj = PhotonNetwork.Instantiate("Bullet1", fireTransform.position, fireTransform.rotation, 0) as GameObject; // Instantiate 는 new와 같은 의미 
    //    Rigidbody rb = obj.GetComponent<Rigidbody>();
    //    rb.velocity = velocity;
    //}
}
