using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScopeUiControl : Photon.MonoBehaviour
{
    public GameObject crossHairObj;
    PhotonView pv = null;

    // Use this for initialization
    void Start()
    {
        pv = transform.root.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            crossHairObj = Instantiate((GameObject)Resources.Load("Crosshair"), transform.position, transform.rotation) as GameObject;
            crossHairObj.transform.SetParent(transform);
            Transform crossHairTr = transform.Find("CrosshairPos").gameObject.transform;
            crossHairObj.transform.position = new Vector3(crossHairTr.position.x, crossHairTr.position.y, crossHairTr.position.z);
            crossHairObj.transform.rotation = new Quaternion(crossHairTr.rotation.x, crossHairTr.rotation.y, crossHairTr.rotation.z, crossHairTr.rotation.w);
        }
        else
        {
            this.enabled = false;
            crossHairObj.SetActive(false);
        }
    }
}
