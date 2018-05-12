using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,  //장비
    Consumption,//소모
    Misc        //기타
}

[System.Serializable]
public class ItemManager  {
    public string itemName { get; set; }
    public int itemValue { get; set; }
    public int itemPrice { get; set; }
    public string itemDesc { get; set; }
    public ItemType itemType { get; set; }
    public int itemCount { get; set; }
    public Sprite itemImage { get; set; }

    public override int GetHashCode()
    {
        return itemCount;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        ItemManager objAsPart = obj as ItemManager;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }

    public bool Equals(ItemManager other)
    {
        if (other == null) return false;
        return (this.itemCount.Equals(other.itemCount));
    }

    public ItemManager(string _itemName, int _itemValue, int _itemPrice, string _itemDesc, ItemType _itemType, int _itemCount, Sprite _itemImage)
    {
        itemName = _itemName;
        itemValue = _itemValue;
        itemPrice = _itemPrice;
        itemDesc = _itemDesc;
        itemType = _itemType;
        itemCount = _itemCount;
        itemImage = _itemImage;
    }

    public ItemManager()
    {

    }
}
