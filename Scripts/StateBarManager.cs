using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBarManager : MonoBehaviour
{
    GameObject HpBarObj;
    GameObject HangerBarObj;
    public UISlider HpBarSlider;
    public UISlider HangerBarSlider;
    PhotonView pv = null;

    // Use this for initialization
    void Start()
    {
        pv = transform.root.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            HpBarObj = Instantiate((GameObject)Resources.Load("SliderHP"), transform.position, transform.rotation) as GameObject;
            HangerBarObj = Instantiate((GameObject)Resources.Load("SliderHunger"), transform.position, transform.rotation) as GameObject;
            HpBarObj.transform.SetParent(transform);
            HangerBarObj.transform.SetParent(transform);
            HpBarSlider = HpBarObj.GetComponentInChildren<UISlider>();
            HangerBarSlider = HangerBarObj.GetComponentInChildren<UISlider>();
            Transform SliderHpTr = transform.Find("SliderHpPos").gameObject.transform;
            HpBarObj.transform.position = new Vector3(SliderHpTr.position.x, SliderHpTr.position.y, SliderHpTr.position.z);
            HpBarObj.transform.rotation = new Quaternion(SliderHpTr.rotation.x, SliderHpTr.rotation.y, SliderHpTr.rotation.z, SliderHpTr.rotation.w);
            HpBarObj.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
            Transform SliderHungerTr = transform.Find("SliderHungerPos").gameObject.transform;
            HangerBarObj.transform.position = new Vector3(SliderHungerTr.position.x, SliderHungerTr.position.y, SliderHungerTr.position.z);
            HangerBarObj.transform.rotation = new Quaternion(SliderHungerTr.rotation.x, SliderHungerTr.rotation.y, SliderHungerTr.rotation.z, SliderHungerTr.rotation.w);
            HangerBarObj.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        }
        else
        {
            this.enabled = false;
        }
    }
}
