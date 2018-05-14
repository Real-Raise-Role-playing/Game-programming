using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBarControl : Photon.MonoBehaviour
{
    public GameObject HpBarObj;
    public GameObject HangerBarObj;
    public Slider HpBarSlider;
    public Slider HangerBarSlider;
    PlayerState ps = null;
    CharacterMove cm = null;
    PhotonView pv = null;
    // Use this for initialization
    void Start () {
        ps = transform.root.GetComponent<PlayerState>();
        cm = transform.root.GetComponent<CharacterMove>();
        pv = transform.root.GetComponent<PhotonView>();
        HpBarObj = Instantiate((GameObject)Resources.Load("SliderHP"), transform.position, transform.rotation) as GameObject;
        HangerBarObj = Instantiate((GameObject)Resources.Load("SliderHunger"), transform.position, transform.rotation) as GameObject;
        HpBarObj.transform.SetParent(transform);
        HangerBarObj.transform.SetParent(transform);
        HpBarSlider = HpBarObj.GetComponent<Slider>();
        HangerBarSlider = HangerBarObj.GetComponent<Slider>();
        GameObject SliderHpPos = transform.Find("SliderHpPos").gameObject;
        HpBarObj.transform.position = new Vector3(SliderHpPos.transform.position.x, SliderHpPos.transform.position.y, SliderHpPos.transform.position.z);
        HpBarObj.transform.rotation = new Quaternion(SliderHpPos.transform.rotation.x, SliderHpPos.transform.rotation.y, SliderHpPos.transform.rotation.z, SliderHpPos.transform.rotation.w);
        GameObject SliderHungerPos = transform.Find("SliderHungerPos").gameObject;
        HangerBarObj.transform.position = new Vector3(SliderHungerPos.transform.position.x, SliderHungerPos.transform.position.y, SliderHungerPos.transform.position.z);
        HangerBarObj.transform.rotation = new Quaternion(SliderHungerPos.transform.rotation.x, SliderHungerPos.transform.rotation.y, SliderHungerPos.transform.rotation.z, SliderHungerPos.transform.rotation.w);

    }

    // Update is called once per frame
    void Update () {
        if (!pv.isMine) { return; }
        HpBarSlider.value = ps.currHp;
        if (cm.run)
        {
            HangerBarSlider.value -= 0.5f;
        }
        else
        {
            HangerBarSlider.value += 0.3f;
        }
    }
}
