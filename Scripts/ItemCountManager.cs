using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCountManager : MonoBehaviour
{
    Text ItemCount;
    Slot ParentSlot;
    // Use this for initialization
    void Awake()
    {
        ParentSlot = GetComponentInParent<Slot>();
        ItemCount = GetComponent<Text>();
    }
    void Start()
    {
        ItemCount.text = ParentSlot.item.itemCount.ToString();
    }
}
