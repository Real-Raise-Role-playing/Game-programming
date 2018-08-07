using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour
{
    //public static ItemDatabase instance;
    public List<ItemManager> items = new List<ItemManager>();
    public List<GameObject> itemObjs = new List<GameObject>();
    List<GameObject> startItemObjs = new List<GameObject>();
    //public Dictionary<int , GameObject> itemObjs = new Dictionary<int ,GameObject>();
    //public ArrayList itemObjs = new ArrayList();
    public int itemCount = Constants.startItemCount;
    public ItemManager selectItem = null;
    Inventory iv = null;
    //public int currentItemCount = 0;

    //void Awake()
    //{
    //    instance = this;
    //}

    // Use this for initialization
    //public void Swap(ArrayList list, int indexA, int indexB)
    //{
    //    GameObject tmp = ((GameObject)list[indexA]);
    //    list[indexA] = list[indexB];
    //    list[indexB] = tmp;
    //    Debug.Log("A[" + indexA + "] : " + ((GameObject)list[indexA]).name + " B[" + indexB + "] : " + ((GameObject)list[indexB]).name);
    //}

    //public void Swap<T>(IDictionary<int, T> Dic, int indexA, int indexB)
    //{
    //    T tmp = Dic[indexA];
    //    Dic[indexA] = Dic[indexB];
    //    Dic[indexB] = tmp;
    //}

    public void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    void Start()
    {
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
            //Destroy(item);
        }
        for (int i = itemCount - 1; i < Constants.maxInventoryCount; i++) 
        {
            Add("f", -1, 0, "null", i + 1, ItemType.NONE, null);
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

        Image tempImg = iv.slotScripts[_itemCount - 1].transform.GetChild(0).GetComponent<Image>();
        tempImg.sprite = Resources.Load<Sprite>("ItemImages/f");
        for (int i = _itemCount - 1; i < itemCount; i++) 
        {
            Image A = iv.slotScripts[i].transform.GetChild(0).GetComponent<Image>();
            Image B = iv.slotScripts[i+1].transform.GetChild(0).GetComponent<Image>();
            Image temp = A;
            A.sprite = B.sprite;
            B.sprite = temp.sprite;
            //if ((i + 1) == itemCount)
            //{
            //    iv.slotScripts[i].transform.GetChild(0).gameObject.SetActive(false);
            //}
        }
        itemCount--;
        iv.slotScripts[itemCount].transform.GetChild(0).gameObject.SetActive(false);


        //Add("f", 0, 0, "null", Constants.maxInventoryCount, ItemType.NONE, null);
        //Debug.Log("지우기 후");
        //foreach (var item in items)
        //{
        //    Debug.Log(item.itemName + " : " + item.itemCount);
        //}
        //foreach (ItemManager index in items)
        //{
        //    if (index.itemCount == _itemCount)
        //    {
        //        items.Remove(index);
        //        Add("f", 0, 0, "Empty Item", 0, ItemType.NONE, null);
        //        items.RemoveAt(items.Count - 1);
        //        break;
        //    }
        //}

    }
    public void Add(string itemName, int itemValue, int itemPrice, string itemDesc, int _itemCount, ItemType itemType, GameObject itemObj)
    {
        items.Add(new ItemManager(itemName, itemValue, itemPrice, itemDesc, itemType, _itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
        //items.RemoveAt(items.Count - 1);
        itemObjs.Add(itemObj);
        //items.Add(new ItemManager(itemName, itemValue, itemPrice, itemDesc, itemType, _itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
        //itemObjs.Add(itemObj);
        //Debug.Log("itemObj Name : " + itemObj.name);
        //currentItemCount++;
    }

    public void Insert(string itemName, int itemValue, int itemPrice, string itemDesc, int _itemCount, ItemType itemType, GameObject itemObj)
    {
        items.Insert((_itemCount - 1), new ItemManager(itemName, itemValue, itemPrice, itemDesc, itemType, _itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
        itemObjs.Insert((_itemCount - 1), itemObj);
    }

    public void GetItemInfo(int _itemCount)
    {
        selectItem = items.Find(x => x.itemCount == _itemCount);
        //Debug.Log("GetItemInfo : " + selectItem.itemName + " " + selectItem.itemCount);
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
