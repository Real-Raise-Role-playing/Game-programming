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
                Debug.Log("원래 이름; : "+item.originalName);
                foreach (GameObject _item in DropItemManager.instance.dropItemList)
                {
                    string[] SpText = _item.name.Split('(');
                    if (SpText[0] == item.originalName)
                    {
                        Debug.Log("버린 아이템 이름 : " + SpText[0]);
                        DropItemManager.instance.Action(_item.name, true);
                        _item.transform.position = new Vector3(transform.root.position.x, _item.transform.position.y, transform.root.position.z);
                        itemDataBase.Remove(item.itemCount);
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("빈거 던지기 ㄴ");
                return;
            }
        }
    }
}
