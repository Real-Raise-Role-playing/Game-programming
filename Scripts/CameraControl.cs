using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public float sensitivity = 120.0f;
    float rotationX;
    float rotationY;
    float ZkeyPos = 0.78f;
    float CamDefaultPos = 3.08f;
    private bool keyZ = true;
    CharacterController PlayerCC = null;
    BoxCollider PlayerBC = null;
    public GameObject Player = null;
    
    void Start()
    {
        PlayerCC = Player.GetComponent<CharacterController>();
        PlayerBC = Player.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseMoveValueX = Input.GetAxis("Mouse X");
        float mouseMoveValueY = Input.GetAxis("Mouse Y");

        rotationY += mouseMoveValueX * sensitivity * Time.deltaTime;
        rotationX += mouseMoveValueY * sensitivity * Time.deltaTime;
        rotationX %= 360;
        rotationY %= 360;
        rotationX = Mathf.Clamp(rotationX, -80.0f, 80.0f);

        Player.transform.eulerAngles = new Vector3(-rotationX, rotationY, 0.0f);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (keyZ)
            {
                //Debug.Log("ZkeyPos :" + ZkeyPos);
                transform.position = new Vector3(Player.transform.position.x, ZkeyPos, Player.transform.position.z);
                //transform.SetParent(transform.ge,true);
                PlayerCC.height = 1.0f;
                PlayerBC.size = new Vector3(1.5f,1.0f,1.5f);
            }
            else
            {
                //Debug.Log("CamDefaultPos : " + CamDefaultPos);
                transform.position = new Vector3(Player.transform.position.x, CamDefaultPos, Player.transform.position.z);
                PlayerCC.height = 2.0f;
                PlayerBC.size = new Vector3(1.5f,2.0f,1.5f);
            }
            keyZ = !keyZ;
        }
        //Debug.Log((-rotationX));
    }
}
