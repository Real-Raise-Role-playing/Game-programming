using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {
    public static ItemDatabase instance;
    public List<ItemManager> items = new List<ItemManager>();

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //시작 아이템 정보
        Add("axe", 1, 500, "Good Axe", 1, ItemType.Equipment);
        Add("armor", 1, 500, "Best Armor", 2, ItemType.Equipment);
        Add("apple", 1, 50, "Delicious Apple", 3, ItemType.Consumption);
    }

public void Add(string itemName, int itemValue, int itemPrice, string itemDesc, int itemCount, ItemType itemType)
    {
        items.Add(new ItemManager(itemName, itemValue, itemPrice, itemDesc, itemType, itemCount, Resources.Load<Sprite>("ItemImages/" + itemName)));
        //Debug.Log("itemName : "+itemName);
    }
}
