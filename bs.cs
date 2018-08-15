using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bs : MonoBehaviour {

    public GameObject bloody;

	// Use this for initialization
	void Start () {

        bloody.SetActive(false);
    }


    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Slime")
        {
            bloody.SetActive(true);

        }
        else bloody.SetActive(false);
    }
}
