﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUIControl : Photon.MonoBehaviour
{
    public Image beShotImg;
    public Color beShotImgColor;
    public Color beShotImgDisableColor;

    [HideInInspector]
    public GameObject HpBarObj;
    [HideInInspector]
    public GameObject HangerBarObj;
    [HideInInspector]
    public Slider HpBarSlider;
    [HideInInspector]
    public Slider HangerBarSlider;
    [HideInInspector]
    public GameObject currentBulletObj;
    [HideInInspector]
    public GameObject havingBulletObj;
    [HideInInspector]
    public Text currentBulletText;
    [HideInInspector]
    public Text havingBulletText;
    private PhotonView pv = null;
    // Use this for initialization
    void Awake()
    {
        pv = transform.root.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            beShotImgColor = beShotImg.color;
            beShotImgDisableColor = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            beShotImg.color = beShotImgDisableColor;

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

            currentBulletObj = Instantiate((GameObject)Resources.Load("CurrentBulletCount"), transform.position, transform.rotation) as GameObject;
            havingBulletObj = Instantiate((GameObject)Resources.Load("HavingBulletCount"), transform.position, transform.rotation) as GameObject;
            currentBulletObj.transform.SetParent(transform);
            havingBulletObj.transform.SetParent(transform);
            currentBulletText = currentBulletObj.GetComponent<Text>();
            havingBulletText = havingBulletObj.GetComponent<Text>();
            Transform currTextTr = transform.Find("CurrentBulletPos").gameObject.transform;
            currentBulletObj.transform.position = new Vector3(currTextTr.position.x, currTextTr.position.y, currTextTr.position.z);
            currentBulletObj.transform.rotation = new Quaternion(currTextTr.rotation.x, currTextTr.rotation.y, currTextTr.rotation.z, currTextTr.rotation.w);
            Transform havingTextTr = transform.Find("HavingBulletPos").gameObject.transform;
            havingBulletObj.transform.position = new Vector3(havingTextTr.position.x, havingTextTr.position.y, havingTextTr.position.z);
            havingBulletObj.transform.rotation = new Quaternion(havingTextTr.rotation.x, havingTextTr.rotation.y, havingTextTr.rotation.z, havingTextTr.rotation.w);
        }
        else
        {
            beShotImg.transform.gameObject.SetActive(false);
            this.enabled = false;
        }
    }

    void Update()
    {
        //if (Constants.Stage_1 <= WorldTimerManager.instance.worldTimer && WorldTimerManager.instance.worldTimer < Constants.Stage_2)
        //{
        //    HangerBarSlider.value -= 0.005f;
        //}
        //else if (Constants.Stage_2 <= WorldTimerManager.instance.worldTimer && WorldTimerManager.instance.worldTimer < Constants.Stage_3)
        //{
        //    HangerBarSlider.value -= 0.01f;
        //}
        //else if (Constants.Stage_3 <= WorldTimerManager.instance.worldTimer)
        //{
        //    HangerBarSlider.value -= 0.02f;
        //}
        HangerBarSlider.value -= 0.5f;
    }
    
    public IEnumerator delayTime()
    {
        yield return new WaitForSeconds(2);
        beShotImg.color = beShotImgDisableColor;
    }

}
