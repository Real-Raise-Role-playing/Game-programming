using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour
{
    //public static ItemDatabase instance;
    [HideInInspector]
    public List<ItemManager> items = new List<ItemManager>();
    public List<ItemManager> equipItems = new List<ItemManager>();

    public List<string> itemsName = new List<string>();
    public List<GameObject> itemObjs = new List<GameObject>();
    public List<GameObject> equipItemObjs = new List<GameObject>();

    [HideInInspector]
    public int itemCount = 0;
    //public int itemCount = Constants.startItemCount;
    public ItemManager selectItem = null;
    Inventory iv = null;

    public void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    void Start()
    {
        itemsName.Capacity = 25;
        iv = transform.root.GetComponentInChildren<Inventory>();
    }
    public void Remove(int _itemCount)
    {
        items.RemoveAt((_itemCount - 1));
        itemObjs.RemoveAt((_itemCount - 1));

        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemCount = (i + 1);
        }

        iv.slotScripts[_itemCount - 1].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemImages/f");

        for (int i = _itemCount - 1; i < itemCount; i++)
        {
            iv.slotScripts[i].item = iv.slotScripts[i + 1].item;
            iv.slotScripts[i].transform.GetChild(0).GetComponent<Image>().sprite = iv.slotScripts[i + 1].transform.GetChild(0).GetComponent<Image>().sprite;
        }
        //********마지막 슬롯의 item.itemValue를 0으로 해줘야 다음 이미지가 들어갈때 수정이됨************* 
        iv.slotScripts[itemCount - 1].item = new ItemManager("f", 0, "null", ItemType.NONE, itemCount - 1, Resources.Load<Sprite>("ItemImages/f"));
        //iv.slotScripts[itemCount - 1].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemImages/f");
        iv.slotScripts[itemCount-1].transform.GetChild(0).gameObject.SetActive(false);
        itemCount--;
    }
    public void Add(string itemName, int itemValue, string originalName, int _itemCount, ItemType itemType, GameObject itemObj)
    {
        if (itemValue == 1)
        {
            Debug.Log("if (itemValue == 1) 실행");
            items.Add(new ItemManager(itemName, itemValue, originalName, itemType, _itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
            itemObjs.Add(itemObj);
        }
    }

    public void Insert(string itemName, int itemValue, string originalName, int _itemCount, ItemType itemType, GameObject itemObj)
    {
        items.Insert((_itemCount - 1), new ItemManager(itemName, itemValue, originalName, itemType, _itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
        itemObjs.Insert((_itemCount - 1), itemObj);
    }

    public void GetItemInfo(int _itemCount)
    {
        selectItem = items.Find(x => x.itemCount == _itemCount);
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
