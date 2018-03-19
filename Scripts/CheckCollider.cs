using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    public static CheckCollider instance;
    //아이템 줍기 가능 상태 여부 Flag
    public bool isGetItemflag = false;

    public GameObject itemObj = null;
    public string itemName = null;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isGetItemflag)
        {
            //아이템 줍기 시 기존 아이템 갯수에서 ++해준다. 
            ItemDatabase.instance.itemCount++;
            //아이템 리스트에서 꺼내어 저장
            addItemList(itemName);
            //인벤토리 관련 인스턴스를 사용하려면 먼저 false상태에서 active상태로 해야함
            Inventory.instance.AddItem(ItemDatabase.instance.itemCount);
            itemObj = GameObject.Find(itemName);
            //Debug.Log("GameObjName : "+ itemObj.name);
            itemObj.SetActive(false);
            isGetItemflag = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int layerIndex = other.gameObject.layer;
        //if (other.CompareTag("Pig")) //태그 비교할땐 컴페어를 주로 사용할 것
        if (LayerMask.LayerToName(layerIndex) == "Item")
        {
            isGetItemflag = true;
            itemName = other.gameObject.name;
            //Debug.Log("진입 충돌 체 이름 : " + other.gameObject.name);
        }
     }
    private void OnTriggerExit(Collider other)
    {
        int layerIndex = other.gameObject.layer;
        if (LayerMask.LayerToName(layerIndex) == "Item")
        {
            isGetItemflag = false;
            //Debug.Log("탈출 충돌 체 이름 : " + other.gameObject.name);
        }

    }

    void addItemList(string itemName) {
        if (itemName == "helmets")
        {
            ItemDatabase.instance.Add("helmets", 1, 50, "Good helmets", ItemDatabase.instance.itemCount, ItemType.Consumption);
        }
        else if (itemName == "b_t_01")
        {
            ItemDatabase.instance.Add("b_t_01", 1, 10, "b_t_01", ItemDatabase.instance.itemCount, ItemType.Misc);
        }
        else if (itemName == "bag")
        {
            ItemDatabase.instance.Add("bag", 1, 2000, "Beautiful Bag", ItemDatabase.instance.itemCount, ItemType.Equipment);
        }
        else if (itemName == "rings")
        {
            ItemDatabase.instance.Add("rings", 1, 800, "Beautiful rings", ItemDatabase.instance.itemCount, ItemType.Equipment);
        }
        else if (itemName == "gem")
        {
            ItemDatabase.instance.Add("gem", 1, 800, "Beautiful gem", ItemDatabase.instance.itemCount, ItemType.Misc);
        }
        else
        {
            Debug.Log("응 오류.");
        }
    }
}
