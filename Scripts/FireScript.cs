using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : Photon.MonoBehaviour
{
    public Transform cameraTransform = null;
    public float forwardPower = Constants.forwardPower;
    public float upPower = Constants.upPower;
    public Transform fireTransform;
    //GameObject fireObject = null;
    //public Rigidbody fireObjectRb = null;
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
        //fireObject = (GameObject)Resources.Load("Bullet1");
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
                    //pv.RPC("FireOther", PhotonTargets.All, fireTransform.position, fireTransform.rotation);
                    sfx.PlayOneShot(kar98,1.0f);
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
                    //pv.RPC("FireOther", PhotonTargets.All, fireTransform.position, fireTransform.rotation);
                    sfx.PlayOneShot(kar98,1.0f);
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

    void CreateBullet()
    {
        GameObject obj = PhotonNetwork.Instantiate("Bullet1", fireTransform.position, fireTransform.rotation, 0) as GameObject; // Instantiate 는 new와 같은 의미 
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    }
    //------------------------------------------------------------------------------------
    //[PunRPC]
    //void fireOther(Vector3 pos,Quaternion rot)
    //{
    //    GameObject obj = PhotonNetwork.Instantiate("Bullet1", pos, rot, 0) as GameObject; // Instantiate 는 new와 같은 의미 
    //    //GameObject obj = Instantiate(fireObject) as GameObject; // Instantiate 는 new와 같은 의미 
    //    //obj.transform.position = pos;
    //    Rigidbody rb = obj.GetComponent<Rigidbody>();
    //    rb.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    //}
    
    //[PunRPC]
    //public void FireOther(Vector3 pos, Quaternion rot)//, PhotonMessageInfo info)
    //{
    //    //GameObject obj = PhotonNetwork.Instantiate("Bullet1", pos, rot, 0) as GameObject; // Instantiate 는 new와 같은 의미 
    //    //GameObject obj = Instantiate(fireObject, pos, rot); 
    //    //Rigidbody obj = Instantiate(fireObject,pos,rot) as Rigidbody; // Instantiate 는 new와 같은 의미 
    //    Rigidbody obj = Instantiate(fireObjectRb, pos, rot) as Rigidbody;
    //    obj.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    //}
}
