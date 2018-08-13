using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : Photon.MonoBehaviour
{
    enum PARTICLE_STATE
    {
        SHOTLIGHT,
        BLOOD
    }

    CharacterMove cm = null;

    public Transform cameraTransform = null;
    public float forwardPower = Constants.forwardPower;
    public float upPower = Constants.upPower;
    public Transform fireTransform;
    GameObject fireObject = null;
    GameObject emptyFireObject = null;
    Rigidbody fireObjectRb = null;
    Rigidbody emptyFireObjectRb = null;
    public bool shotState = false;
    private bool SingleShot = true;
    private float nextFire = 0.0f;
    float cameraDefaultZoom;
    bool toggle = false;
    private PhotonView pv = null;
    //오디오 컴포턴트 할당 변수
    private AudioSource sfx = null;
    //Kar98 사운드 파일
    private AudioClip kar98 = null;

    ParticleSystem[] particleLight;
    public GameObject particleBlood;
    //탄 관련
    StateUIControl suc = null;
    //현재 탄창에 있는 총알 수
    public int currentBulletCount;
    //가방에 가지고 있는 총알 수
    public int havingBulletCount;
    public bool reloadAnimCheck;
    public float ShootRate;//DPS
    public float ReloadTime;//재장전 시간


    void Awake()
    {
        cm = GetComponent<CharacterMove>();
        fireObject = (GameObject)Resources.Load("Bullet1");
        fireObjectRb = fireObject.GetComponent<Rigidbody>();
        emptyFireObject = (GameObject)Resources.Load("Bullet2");
        emptyFireObjectRb = emptyFireObject.GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        sfx = GetComponent<AudioSource>();
        suc = GetComponentInChildren<StateUIControl>();
        kar98 = Resources.Load<AudioClip>("Sounds\\Kar98");
        particleLight = transform.root.GetComponentsInChildren<ParticleSystem>();
    }

    // Use this for initialization
    void Start()
    {
        cameraTransform = Camera.main.GetComponent<Transform>();
        //cameraTransform = GameObject.Find("MainCamera").GetComponent<Transform>();
        //this.enabled = GetComponent<PhotonView>().isMine;
        cameraDefaultZoom = Camera.main.fieldOfView;

        currentBulletCount = 0;
        havingBulletCount = Constants.m16InitBulletCount;
        reloadAnimCheck = false;
    }
    [PunRPC]
    void UpadateBulletCount()
    {
        suc.currentBulletText.text = currentBulletCount.ToString();
        suc.havingBulletText.text = "/ " + havingBulletCount.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (!pv.isMine)
        {
            return;
        }
        UpadateBulletCount();
        Fire();
        /*
        //플레이어 죽음 처리, 일시정지 
        if (PauseManager.gamePause || playerState.isDead)
        {
            return;
        }
        */
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (havingBulletCount > 0 && currentBulletCount < 30)
            {
                reloadAnimCheck = true;
                //cm.AnimPlay("reload", -1, 0f);
                StartCoroutine(reloadGun());
            }
            else
            {
                Debug.Log("총알이 없다;");
            }
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            reloadAnimCheck = false;
        }
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
        if (currentBulletCount > 0)
        {
            //단발
            if (Input.GetMouseButtonDown(0) && SingleShot)
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + Constants.m16FireSpeed;
                    //photonView.RPC("otherFireOther", PhotonTargets.Others, cameraTransform.forward);
                    CreateBullet();
                    currentBulletCount--;
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
                //particleLight[(int)PARTICLE_STATE.SHOTLIGHT].Play();
                pv.RPC("WeaponLight", PhotonTargets.All, "kar98");
            }
            //연발
            if (Input.GetMouseButton(0) && !SingleShot)
            {

                if (Time.time > nextFire)
                {
                    nextFire = Time.time + Constants.m16FireSpeed;
                    CreateBullet();
                    currentBulletCount--;
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

    IEnumerator reloadGun()
    {
        Debug.Log("재장전 시작");
        yield return new WaitForSeconds(2.8f);
        if (havingBulletCount >= Constants.m16MaxBulletCount)
        {
            int addBulletCount = Constants.m16MaxBulletCount - currentBulletCount;
            havingBulletCount -= addBulletCount;
            currentBulletCount += addBulletCount;
        }
        else
        {
            currentBulletCount = havingBulletCount;
            havingBulletCount = 0;
        }
        Debug.Log("재장전 끝");
    }

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

    [PunRPC]
    void WeaponLight(string waepon)
    {
        if (waepon == "kar98")
        {
            particleLight[(int)PARTICLE_STATE.SHOTLIGHT].Play();
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
