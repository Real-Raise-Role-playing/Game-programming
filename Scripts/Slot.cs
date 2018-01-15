using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
    public int number;
    public ItemManager item;

    public void OnDrag(PointerEventData data)
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).SetParent(Inventory.instance.draggingItem, true);
        }
        Inventory.instance.draggingItem.GetChild(0).position = data.position;
    }
    public void OnPointerEnter(PointerEventData data)
    {
        Inventory.instance.enteredSlot = this;
    }

    public void OnPointerExit(PointerEventData data)
    {
        Inventory.instance.enteredSlot = null;
    }

    public void OnEndDrag(PointerEventData data)
    {
        Inventory.instance.draggingItem.GetChild(0).SetParent(transform, true);
        transform.GetChild(0).localPosition = Vector3.zero;
        //**자식 오브젝트 아이템 이미지 레이케스트를 꺼야함;;;(마우스 Enter, Exit시 오류 발생가능)
        if (Inventory.instance.enteredSlot != null)
        {
            ItemManager tempItem = item;
            item = Inventory.instance.enteredSlot.item;
            Inventory.instance.enteredSlot.item = tempItem;
            Inventory.instance.ItemImageChange(this);
            Inventory.instance.ItemImageChange(Inventory.instance.enteredSlot);
        }
    }
}
