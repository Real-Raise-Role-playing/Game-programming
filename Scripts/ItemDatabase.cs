using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {
    //public static ItemDatabase instance;
    public List<ItemManager> items = new List<ItemManager>();
    public List<GameObject> itemObjs = new List<GameObject>();
    public List<GameObject> startItemObjs = new List<GameObject>();
    //public Dictionary<int , GameObject> itemObjs = new Dictionary<int ,GameObject>();
    //public ArrayList itemObjs = new ArrayList();
    public int itemCount = Constants.startItemCount;
    public ItemManager selectItem = null;
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
        //시작 아이템 정보
        startItemObjs.Add(Instantiate(Resources.Load<GameObject>("Items/axe"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject);
        startItemObjs.Add(Instantiate(Resources.Load<GameObject>("Items/armor"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject);
        startItemObjs.Add(Instantiate(Resources.Load<GameObject>("Items/apple"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject);

        Add("axe", 1, 500, "Good Axe", 1, ItemType.Equipment, startItemObjs[0]);
        Add("armor", 1, 500, "Best Armor", 2, ItemType.Equipment, startItemObjs[1]);
        Add("apple", 1, 50, "Delicious Apple", 3, ItemType.Consumption, startItemObjs[2]);
        foreach (GameObject item in startItemObjs)
        {
            item.SetActive(false);
        }
    }

    public void Add(string itemName, int itemValue, int itemPrice, string itemDesc, int itemCount, ItemType itemType, GameObject itemObj)
    {
        items.Add(new ItemManager(itemName, itemValue, itemPrice, itemDesc, itemType, itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
        itemObjs.Add(itemObj);
        //Debug.Log("itemObj Name : " + itemObj.name);
    }

    public void GetItemInfo(int _itemCount) {
        selectItem = items.Find(x => x.itemCount == _itemCount);
        //Debug.Log("GetItemInfo : " + selectItem.itemName + " " + selectItem.itemCount);
    }
}
