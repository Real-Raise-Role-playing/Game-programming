using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public float sensitivity = 300.0f;
    float rotationX;
    float rotationY;
    float ZkeyPos = 0.78F;
    float CamDefaultPos = 3.08f;
    private bool keyZ = true;
    CharacterController ParentCC = null;
    BoxCollider ParentBC = null;
    void Start()
    {
        ParentCC = GetComponentInParent<CharacterController>();
        ParentBC = GetComponentInParent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OptionManager.gameOptionOn)
        {
            transform.eulerAngles.Normalize();
            return;
        }
        float mouseMoveValueX = Input.GetAxis("Mouse X");
        float mouseMoveValueY = Input.GetAxis("Mouse Y");

        rotationY += mouseMoveValueX * sensitivity * Time.deltaTime;
        rotationX += mouseMoveValueY * sensitivity * Time.deltaTime;
        rotationX %= 360;
        rotationY %= 360;
        rotationX = Mathf.Clamp(rotationX, -80.0f, 80.0f);

        transform.eulerAngles = new Vector3(-rotationX, rotationY, 0.0f);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (keyZ)
            {
                //Debug.Log("ZkeyPos :" + ZkeyPos);
                transform.position = new Vector3(transform.position.x, ZkeyPos, transform.position.z);
                //transform.SetParent(transform.ge,true);
                ParentCC.height = 1.0f;
                ParentBC.size = new Vector3(1.5f,1.0f,1.5f);
            }
            else
            {
                //Debug.Log("CamDefaultPos : " + CamDefaultPos);
                transform.position = new Vector3(transform.position.x, CamDefaultPos, transform.position.z);
                ParentCC.height = 2.0f;
                ParentBC.size = new Vector3(1.5f,2.0f,1.5f);
            }
            keyZ = !keyZ;
        }
        //Debug.Log((-rotationX));
    }
}
