using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
    Inventory iv = null;
    public int number;
    public ItemManager item;
    public ItemDatabase itemDataBase;
    void Start()
    {
        iv = GetComponentInParent<Inventory>();
        itemDataBase = transform.root.GetComponentInChildren<ItemDatabase>();
    }

    public void OnDrag(PointerEventData data)
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).SetParent(iv.draggingItem, true);
        }
        iv.draggingItem.GetChild(0).position = data.position;
    }
    public void OnPointerEnter(PointerEventData data)
    {
        iv.enteredSlot = this;
    }

    public void OnPointerExit(PointerEventData data)
    {
        iv.enteredSlot = null;
    }

    public void OnEndDrag(PointerEventData data)
    {
        iv.draggingItem.GetChild(0).SetParent(transform, true);
        transform.GetChild(0).localPosition = Vector3.zero;
        //Debug.Log("transform.name : " + transform.gameObject.name);
        //**자식 오브젝트 아이템 이미지 레이케스트를 꺼야함;;;(마우스 Enter, Exit시 오류 발생가능)
        if (iv.enteredSlot == this)
        {
            return;
        }
        else if (iv.enteredSlot != null)
        {
            //임시 객체 선언 후 위치 교환
            ItemManager tempItem = item;
            item = iv.enteredSlot.item;
            iv.enteredSlot.item = tempItem;

            //아이템 위치 교환 시 위치 번호를 맞추기 위함
            item.itemCount = (number + 1);
            iv.enteredSlot.item.itemCount = (iv.enteredSlot.number + 1);
            //이미지 교환
            iv.ItemImageChange(this);
            iv.ItemImageChange(iv.enteredSlot);
            if (itemDataBase.itemObjs != null)
            {
                itemDataBase.Swap(itemDataBase.itemObjs, (item.itemCount - 1), (iv.enteredSlot.item.itemCount - 1));
            }
        }
        else
        {
            if (item.itemType != ItemType.NONE)
            {
                int tempitemCount = item.itemCount;
                Debug.Log(item.itemName + "  tempitemCount : " + tempitemCount);
                itemDataBase.Remove(item.itemCount);
            }
            else
            {
                Debug.Log("빈거 던지기 ㄴ");
                return;
            }
        }
    }
}
