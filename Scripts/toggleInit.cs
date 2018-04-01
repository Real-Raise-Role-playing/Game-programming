using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIToggle toggle = GetComponent<UIToggle>();
        
        if(toggle!=null&&!toggle.value)
        {
            UIPlayTween tweenToPlay = GetComponent<UIPlayTween>();

            if(tweenToPlay!=null)
            {
                tweenToPlay.Play(true);
            }
        }
	}
}
