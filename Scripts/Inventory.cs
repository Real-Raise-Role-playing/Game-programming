using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    public Transform slot;
    public List<Slot> slotScripts = new List<Slot>();

    //드레그 스크립트
    public Transform draggingItem;
    public Slot enteredSlot;

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
                newSlot.SetParent(transform, true);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                slotRect.anchorMin = new Vector2(0.2f * j + 0.05f, 1 - (0.2f * (i + 1) - 0.05f));
                slotRect.anchorMax = new Vector2(0.2f * (j + 1) - 0.05f, 1 - (0.2f * i + 0.05f));
                slotRect.offsetMin = Vector2.zero;
                slotRect.offsetMax = Vector2.zero;

                slotScripts.Add(newSlot.GetComponent<Slot>());
                newSlot.GetComponent<Slot>().number = i * 5 + j;
            }
        }
        //아이템 데이터베이스 최대 갯수 Add
         AddItem(3);
    }

    public void ItemImageChange(Slot _slot)
    {
        if (_slot.item.itemValue == 1)
        {
            _slot.transform.GetChild(0).gameObject.SetActive(true);
            _slot.transform.GetChild(0).GetComponent<Image>().sprite = _slot.item.itemImage;
        }
        else
        {
            _slot.transform.GetChild(0).gameObject.SetActive(false);
            _slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        }
    }

    void AddItem(int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (slotScripts[i].item.itemValue == 0)
            {
                slotScripts[i].item = ItemDatabase.instance.items[i];
                ItemImageChange(slotScripts[i]);
            }
        }

        for (int i = number; i < slotScripts.Count; i++)
        {
            if (slotScripts[i].item.itemValue == 0)
            {
                slotScripts[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

}
