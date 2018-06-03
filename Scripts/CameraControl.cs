using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float sensitivity = 500.0f;
    float rotationX;
    float rotationY;
    float ZkeyPos = 0.78f;
    float CamDefaultPos = 3.08f;
    private bool keyZ = true;

    public CharacterController PlayerCC = null;
    public Rigidbody PlayerRb = null;
    public Transform PlayerTr = null;
    //public Transform UICamTr = null;
    public GameObject Player = null;
    public PhotonView pv = null;

    void OnEnable()
    {
        pv = Player.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            //UICamTr = Player.transform.Find("UICamera").gameObject.GetComponent<Transform>();
            PlayerCC = Player.GetComponent<CharacterController>();
            PlayerTr = Player.GetComponent<Transform>();
            PlayerRb = Player.GetComponent<Rigidbody>();
        }
        else
        {
            this.enabled = false;
        }
        
    }

    //void OnDisable() {
    //    PlayerCC = null;
    //    PlayerTr = null;
    //    PlayerRb = null;
    //    pv = null;
    //}

    // Update is called once per frame
    void Update()
    {
        if (!pv.isMine)
        {
            return;
        }
        else
        {
            float mouseMoveValueX = Input.GetAxis("Mouse X");
            float mouseMoveValueY = Input.GetAxis("Mouse Y");
            rotationY += mouseMoveValueX * sensitivity * Time.deltaTime;
            rotationX += mouseMoveValueY * sensitivity * Time.deltaTime;
            rotationX %= 360;
            rotationY %= 360;
            rotationX = Mathf.Clamp(rotationX, -40.0f, 80.0f);

            PlayerTr.eulerAngles = new Vector3(PlayerTr.eulerAngles.x, rotationY, PlayerTr.eulerAngles.z);
            //PlayerTr.eulerAngles = new Vector3(PlayerTr.rotation.eulerAngles.x, rotationY, PlayerTr.rotation.eulerAngles.z);
            transform.eulerAngles = new Vector3(-rotationX, rotationY, 0.0f);
            //UICamTr.eulerAngles = new Vector3(-rotationX, rotationY, 0.0f);
            //transform.root.eulerAngles = new Vector3(-rotationX, rotationY, 0.0f);
            //transform.root.Rotate(-rotationX, rotationY, 0.0f);
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (keyZ)
                {
                    //Debug.Log("ZkeyPos :" + ZkeyPos);
                    transform.position = new Vector3(Player.transform.position.x, ZkeyPos, Player.transform.position.z);
                    //transform.SetParent(transform.ge,true);
                    PlayerCC.height = 1.0f;
                    //PlayerBC.size = new Vector3(1.5f, 1.0f, 1.5f);
                }
                else
                {
                    //Debug.Log("CamDefaultPos : " + CamDefaultPos);
                    transform.position = new Vector3(Player.transform.position.x, CamDefaultPos, Player.transform.position.z);
                    PlayerCC.height = 2.0f;
                    //PlayerBC.size = new Vector3(1.5f, 2.0f, 1.5f);
                }
                keyZ = !keyZ;
            }
        }
    }
}