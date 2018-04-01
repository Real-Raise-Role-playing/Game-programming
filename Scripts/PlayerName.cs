using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour {

	// Use this for initialization
	private void Start () {
        UILabel label = GetComponent<UILabel>();

        label.text = PlayerPrefs.GetString("PlayerName");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
