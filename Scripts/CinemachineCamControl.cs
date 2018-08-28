using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCamControl : MonoBehaviour
{
    Cinemachine.CinemachineVirtualCamera cv;
    //float rotationX;
    //float rotationY;
    private void Start()
    {
        //cv.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain
    }
    //// Update is called once per frame
    //void Update()
    //{
    //    //transform.Rotate(Vector3.forward * Time.deltaTime * Constants.defaultSensitivity * Input.GetAxis("Mouse Y"));
    //    float mouseMoveValueX = Input.GetAxis("Mouse X");
    //    float mouseMoveValueY = Input.GetAxis("Mouse Y");
    //    rotationY += mouseMoveValueX * Constants.defaultSensitivity * Time.deltaTime;
    //    rotationX += mouseMoveValueY * Constants.defaultSensitivity * Time.deltaTime;
    //    rotationX %= 360;
    //    rotationY %= 360;
    //    rotationX = Mathf.Clamp(rotationX, -30.0f, 80.0f);

    //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationY, transform.eulerAngles.z);
    //}
}
