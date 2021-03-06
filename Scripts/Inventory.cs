﻿using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public static Inventory instance;
    ItemDatabase idb = null;
    public Transform slot;
    public List<Slot> slotScripts = new List<Slot>();
    //드레그 스크립트
    public Transform draggingItem;
    public Slot enteredSlot;
    private PhotonView pv = null;

    // Use this for initialization
    void Start()
    {
        pv = transform.root.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            idb = transform.root.GetComponentInChildren<ItemDatabase>();
            //X축 ,Y축 갯수, X축간격
            SlotMake(5, 5, 0.04f);
            foreach (Slot item in slotScripts)
            {
                ItemImageChange(item);
            }
        }
        this.gameObject.SetActive(false);
    }

    void SlotMake(int xCount, int yCount, float xInterval)
    {
        Vector2 panelSize = new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);
        float xWidthRate = (1 - xInterval * (xCount + 1)) / xCount;
        float yWidthRate = panelSize.x * xWidthRate / panelSize.y;
        float yInterval = (1 - yWidthRate * yCount) / (yCount + 1);
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                Transform newSlot = Instantiate(slot);
                newSlot.name = "Slot" + (y + 1) + "." + (x + 1);
                newSlot.SetParent(transform, true);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                slotRect.anchorMin = new Vector2(xWidthRate * x + xInterval * (x + 1), 1 - (yWidthRate * (y + 1) + yInterval * (y + 1)));
                slotRect.anchorMax = new Vector2(xWidthRate * (x + 1) + xInterval * (x + 1), 1 - (yWidthRate * y + yInterval * (y + 1)));
                slotRect.offsetMin = Vector2.zero;
                slotRect.offsetMax = Vector2.zero;
                //위에서 각 Slot의 x,y값을 조정하였으나 인벤토리의 z값을 받아와야 같은 선상에 위치하기 때문에 z축 작업은 따로 진행함.
                slotRect.transform.position = new Vector3(slotRect.transform.position.x, slotRect.transform.position.y, this.gameObject.transform.position.z);
                newSlot.position = slotRect.transform.position;
                slotScripts.Add(newSlot.GetComponent<Slot>());
                newSlot.GetComponent<Slot>().number = y * xCount + x;
            }
        }
        slot.gameObject.SetActive(false);
    }

    public void ItemImageChange(Slot _slot)
    {
        if (_slot.item.itemValue == 1)
        {
            _slot.transform.GetChild(0).gameObject.SetActive(true);
            _slot.transform.GetChild(0).GetComponent<Image>().sprite = _slot.item.itemImage;
        }
        else if (_slot.item.itemValue == -1)
        {
           _slot.transform.GetChild(0).gameObject.SetActive(true);
            _slot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemImages/f");
        }
        else
        {
            _slot.transform.GetChild(0).gameObject.SetActive(false);
            _slot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemImages/f");
        }
    }

    public void AddItem()
    {
        for (int i = 0; i < idb.items.Count; i++)
        {
            slotScripts[i].item = idb.items[i];
        }

        for (int i = 0; i < slotScripts.Count; i++)
        {
            if (slotScripts[i].item.itemValue == 1)
            {
                slotScripts[i].item = idb.items[i];
                ItemImageChange(slotScripts[i]);
            }
            else if (slotScripts[i].item.itemValue == 0)
            {
                slotScripts[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("AddItem() else 실행!!");
            }
        }
    }
}
