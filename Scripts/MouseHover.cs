using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public static MouseHover instance = null;
    public bool isUIHover = false;
	// Use this for initialization
	void Awake () {
        instance = this;
	}

    public void OnPointerEnter(PointerEventData eventData) {
        isUIHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isUIHover = false;
    }
}
