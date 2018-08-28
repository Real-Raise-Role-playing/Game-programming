using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBarManager : MonoBehaviour
{
    public UISprite beShotImg;

    [HideInInspector]
    public Color beShotImgColor;
    [HideInInspector]
    public Color beShotImgDisableColor;

    public UISlider HpBarSlider;
    public UISlider HangerBarSlider;

    //[HideInInspector]
    //public GameObject currentBulletObj;
    //[HideInInspector]
    //public GameObject havingBulletObj;
    //[HideInInspector]
    //public Text currentBulletText;
    //[HideInInspector]
    //public Text havingBulletText;
    
    public UILabel currentBulletText;
    public UILabel havingBulletText;
    PhotonView pv = null;

    // Use this for initialization
    void Start()
    {
        pv = transform.root.GetComponent<PhotonView>();
        if (!pv.isMine)
        {
            this.enabled = false;
            HpBarSlider.enabled = false;
            HangerBarSlider.enabled = false;
            beShotImg.transform.gameObject.SetActive(false);
        }
        else
        {
            beShotImgColor = beShotImg.color;
            beShotImgDisableColor = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            beShotImg.color = beShotImgDisableColor;
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
        HangerBarSlider.value -= 0.005f;
    }

    public IEnumerator delayTime(float sec)
    {
        yield return new WaitForSeconds(sec);
        beShotImg.color = beShotImgDisableColor;
    }
}
