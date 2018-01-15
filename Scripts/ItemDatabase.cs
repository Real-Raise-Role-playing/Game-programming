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
        Add("axe", 1, 500, "Good Axe", ItemType.Equipment);
        Add("armor", 1, 500, "Best Armor", ItemType.Equipment);
        Add("apple", 1, 50, "Delicious Apple", ItemType.Consumption);
    }

void Add(string itemName, int itemValue, int itemPrice, string itemDesc, ItemType itemType)
    {
        items.Add(new ItemManager(itemName, itemValue, itemPrice, itemDesc, itemType, Resources.Load<Sprite>("ItemImages/" + itemName)));
        //Debug.Log("itemName : "+itemName);
    }
}
