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
        //X축 ,Y축 갯수, X축간격
        SlotMake(5, 5, 0.04f);
        //아이템 데이터베이스 시작 갯수 Add
        AddItem(Constants.startItemCount);
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

                slotScripts.Add(newSlot.GetComponent<Slot>());
                newSlot.GetComponent<Slot>().number = y * xCount + x;

            }
        }
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

    public void AddItem(int number)
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


