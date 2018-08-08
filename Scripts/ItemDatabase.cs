using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour
{
    //public static ItemDatabase instance;
    [HideInInspector]
    public List<ItemManager> items = new List<ItemManager>();

    public List<string> itemsName = new List<string>();
    public List<GameObject> itemObjs = new List<GameObject>();
    List<GameObject> startItemObjs = new List<GameObject>();
    public int itemCount = Constants.startItemCount;
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
        //Instantiate("items/axe", Vector3.zero, Quaternion.identity) as GameObject
        //아이템 관리할 리스트 초기화
        //for (int i = 0; i < Constants.maxInventoryCount; i++)
        //{
        //    itemObjs.Add(null);
        //}
        //시작 아이템 정보

        iv = transform.root.GetComponentInChildren<Inventory>();
        startItemObjs.Add(Instantiate(Resources.Load<GameObject>("Items/grenade"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject);
        startItemObjs.Add(Instantiate(Resources.Load<GameObject>("Items/canteen"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject);
        startItemObjs.Add(Instantiate(Resources.Load<GameObject>("Items/firstaid"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject);

        Add("grenade", 1, 500, "Good grenade", 1, ItemType.Equipment, startItemObjs[0]);
        Add("canteen", 1, 500, "Best canteen", 2, ItemType.Equipment, startItemObjs[1]);
        Add("firstaid", 1, 50, "Delicious firstaid", 3, ItemType.Consumption, startItemObjs[2]);
        foreach (GameObject item in startItemObjs)
        {
            item.SetActive(false);
        }
        for (int i = itemCount + 1; i < Constants.maxInventoryCount; i++) 
        {
            Add("f", 1, 0, "null", i, ItemType.NONE, null);
        }

        for (int i = 0; i < items.Count; i++)
        {
            itemsName.Add(items[i].itemName);
        }
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
        iv.slotScripts[itemCount - 1].item = new ItemManager("f", 0, 0, "null", ItemType.NONE, itemCount - 1, Resources.Load<Sprite>("ItemImages/f"));
        //iv.slotScripts[itemCount - 1].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemImages/f");
        iv.slotScripts[itemCount-1].transform.GetChild(0).gameObject.SetActive(false);
        itemCount--;
    }
    public void Add(string itemName, int itemValue, int itemPrice, string itemDesc, int _itemCount, ItemType itemType, GameObject itemObj)
    {
        items.Add(new ItemManager(itemName, itemValue, itemPrice, itemDesc, itemType, _itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
        itemObjs.Add(itemObj);
    }

    public void Insert(string itemName, int itemValue, int itemPrice, string itemDesc, int _itemCount, ItemType itemType, GameObject itemObj)
    {
        items.Insert((_itemCount - 1), new ItemManager(itemName, itemValue, itemPrice, itemDesc, itemType, _itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
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
