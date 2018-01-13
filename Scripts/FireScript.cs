using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    public Transform cameraTransform = null;
    public GameObject fireObject = null;
    public float forwardPower = Constants.forwardPower;
    public float upPower = Constants.upPower;
    public Transform fireTransform;
    
    private bool SingleShot = true;

    /*
    PlayerState playerState = null; //플레이어 죽음 처리
    // Use this for initialization
    void Start()
    {
        playerState = GetComponent<PlayerState>(); //죽음 처리 하기 위해 얻어온 컴포넌트
    }
    */

    // Update is called once per frame
    void Update()
    {

        /*
        //플레이어 죽음 처리, 일시정지 
        if (PauseManager.gamePause || playerState.isDead)
        {
            return;
        }
        */


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
                CreateBullet();
            }
        }
        else
        {
            //연발
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CreateBullet();
            }
        }
        //------------------------------------------------------------------------------------

    }

    //----------------------함수 정리----------------------------------------------------
    //총알 생성 함수.
    void CreateBullet()
    {
        GameObject obj = Instantiate(fireObject) as GameObject; // Instantiate 는 new와 같은 의미 
        obj.transform.position = fireTransform.position;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = (cameraTransform.forward * forwardPower) + (Vector3.up * upPower);
    }
    //------------------------------------------------------------------------------------
}
