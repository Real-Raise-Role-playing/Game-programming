using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    public Transform slot;
    public List<Slot> slotScripts = new List<Slot>();

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Transform newSlot = Instantiate(slot);
                newSlot.name = "Slot" + (i + 1) + "." + (j + 1);
                //newSlot.parent = transform;
                newSlot.SetParent(transform, false);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                //slotRect.anchorMin = new Vector2(0.2f * i + 0.05f, 1 - (0.2f * (j + 1) - 0.05f));
                //slotRect.anchorMax = new Vector2(0.2f * (i + 1) - 0.05f, 1 - (0.2f * j + 0.05f));
                slotRect.anchorMin = new Vector2((0.8f - (0.2f * i)) + 0.05f, 1 - (0.2f * (j + 1) - 0.05f));
                slotRect.anchorMax = new Vector2((0.8f - (0.2f * i)) + 0.15f, 1 - (0.2f * j + 0.05f));
                slotRect.offsetMin = Vector2.zero;
                slotRect.offsetMax = Vector2.zero;

                slotScripts.Add(newSlot.GetComponent<Slot>());
                newSlot.GetComponent<Slot>().number = i * 5 + j;
            }
        }
        AddItem(0);
        AddItem(1);
    }

    void ItemImageChange(Transform _slot)
    {
        if (_slot.GetComponent<Slot>().item.itemValue == 0)
        {
            //_slot.GetChild(0).gameObject.SetActive(false);
            _slot.gameObject.SetActive(false);
        }
        else
        {
            //_slot.GetChild(0).gameObject.SetActive(true);
            //_slot.GetChild(0).GetComponent<Image>().sprite = _slot.GetComponent<Slot>().item.itemImage;
            _slot.gameObject.SetActive(true);
            _slot.GetComponent<Image>().sprite = _slot.GetComponent<Slot>().item.itemImage;
        }
    }

    void AddItem(int number)
    {
        for (int i = 0; i < slotScripts.Count; i++)
        {
            if (slotScripts[i].item.itemValue == 0)
            {
                slotScripts[i].item = ItemDatabase.instance.items[number];
                ItemImageChange(slotScripts[i].transform);
                break;
            }
        }
    }

}
