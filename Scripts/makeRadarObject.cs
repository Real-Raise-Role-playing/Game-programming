using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class makeRadarObject : MonoBehaviour {
    public Image image;
	// Use this for initialization
	void Start () {
        Rader.RegisterRaderObject(this.gameObject, image);
	}
	
    void OnDestroy()
    {
        Rader.RemoveRaderObject(this.gameObject);
    }
}
