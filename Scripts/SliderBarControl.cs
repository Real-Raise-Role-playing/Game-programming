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
    void Start()
    {
        pv = transform.root.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            ps = transform.root.GetComponent<PlayerState>();
            cm = transform.root.GetComponent<CharacterMove>();
            HpBarObj = Instantiate((GameObject)Resources.Load("SliderHP"), transform.position, transform.rotation) as GameObject;
            HangerBarObj = Instantiate((GameObject)Resources.Load("SliderHunger"), transform.position, transform.rotation) as GameObject;
            HpBarObj.transform.SetParent(transform);
            HangerBarObj.transform.SetParent(transform);
            HpBarSlider = HpBarObj.GetComponent<Slider>();
            HangerBarSlider = HangerBarObj.GetComponent<Slider>();
            Transform SliderHpTr = transform.Find("SliderHpPos").gameObject.transform;
            HpBarObj.transform.position = new Vector3(SliderHpTr.position.x, SliderHpTr.position.y, SliderHpTr.position.z);
            HpBarObj.transform.rotation = new Quaternion(SliderHpTr.rotation.x, SliderHpTr.rotation.y, SliderHpTr.rotation.z, SliderHpTr.rotation.w);
            Transform SliderHungerTr = transform.Find("SliderHungerPos").gameObject.transform;
            HangerBarObj.transform.position = new Vector3(SliderHungerTr.position.x, SliderHungerTr.position.y, SliderHungerTr.position.z);
            HangerBarObj.transform.rotation = new Quaternion(SliderHungerTr.rotation.x, SliderHungerTr.rotation.y, SliderHungerTr.rotation.z, SliderHungerTr.rotation.w);
            RectTransform HpBarRect = HpBarObj.GetComponent<RectTransform>();
            HpBarRect.sizeDelta = new Vector2(Screen.width - 300.0f, 30);
            RectTransform HangerBarRect = HangerBarObj.GetComponent<RectTransform>();
            HangerBarRect.sizeDelta = new Vector2(Screen.width - 300.0f, 30);
        }
        else
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
