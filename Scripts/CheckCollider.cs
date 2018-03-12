using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    public static CheckCollider instance;

    //아이템 줍기 가능 상태 여부 Flag
    public bool isGetItemflag = false;
    //public GameObject ItemObj = null;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isGetItemflag)
        {
            //특정 행위 
            //Debug.Log("전Inventory.itemCount : " + Inventory.itemCount);
            Inventory.itemCount++;
            //Debug.Log("후Inventory.itemCount : " + Inventory.itemCount);
            ItemDatabase.instance.Add("helmets", 1, 50, "Good helmets", Inventory.itemCount, ItemType.Consumption);
            Inventory.instance.AddItem(Inventory.itemCount);
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
            Debug.Log("진입 충돌 체 이름 : " + other.gameObject.name);
            //ItemObj.gameObject.name = other.gameObject.name;
            //Destroy(gameObject);
        }
     }
    private void OnTriggerExit(Collider other)
    {
        int layerIndex = other.gameObject.layer;
        if (LayerMask.LayerToName(layerIndex) == "Item")// && Input.GetKeyDown(KeyCode.F))
        {
            isGetItemflag = false;
            Debug.Log("탈출 충돌 체 이름 : " + other.gameObject.name);
        }

    }
}
