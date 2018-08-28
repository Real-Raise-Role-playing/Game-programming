using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float sensitivity = Constants.defaultSensitivity;
    float rotationX;
    float rotationY;
    float ZkeyPos = 0.78f;
    float CamDefaultPos = 3.08f;
    private bool keyZ = true;

    public GameObject Player = null;
    public PhotonView pv = null;
    private OptionManager om = null;
    private PlayerState ps = null;
    public Camera thisCamera = null;

    void OnEnable()
    {
        pv = Player.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            //UICamTr = Player.transform.Find("UICamera").gameObject.GetComponent<Transform>();
            om = Player.GetComponent<OptionManager>();
            ps = Player.GetComponent<PlayerState>();
            thisCamera = this.gameObject.GetComponent<Camera>();
            thisCamera.fieldOfView = 100.0f;
        }
        else
        {
            thisCamera = null;
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.isMine || ps.playerStateNum == Constants.DEAD) { return; }

        else
        {
            if (!om.InventoryOn)
            {
                float mouseMoveValueX = Input.GetAxis("Mouse X");
                float mouseMoveValueY = Input.GetAxis("Mouse Y");
                rotationY += mouseMoveValueX * sensitivity * Time.deltaTime;
                rotationX += mouseMoveValueY * sensitivity * Time.deltaTime;
                rotationX %= 360;
                rotationY %= 360;
                rotationX = Mathf.Clamp(rotationX, -30.0f, 80.0f);

                transform.eulerAngles = new Vector3(-rotationX, rotationY, 0.0f);
            }
        }
    }
}