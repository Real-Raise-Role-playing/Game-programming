using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : Photon.MonoBehaviour
{
    public GameObject bagState = null;
    public GameObject gunState = null;
    public GameObject helmetsState = null;
    public GameObject armorState = null;
    public GameObject etcState = null;

    private ItemDatabase idb = null;
    private Inventory iv = null;
    //아이템 줍기 가능 상태 여부 Flag
    public Transform WeaponTr;
    public bool isGetItemflag = false;
    private GameObject itemObj = null;
    private string itemName = null;
    private PhotonView pv = null;
    void Awake()
    {
        idb = GetComponentInChildren<ItemDatabase>();
        iv = GetComponentInChildren<Inventory>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!pv.isMine) { return; }
        if (Input.GetKeyDown(KeyCode.F) && isGetItemflag)
        {
            //아이템 줍기 시 기존 아이템 갯수에서 ++해준다. 
            idb.itemCount++;
            //아이템 리스트에서 꺼내어 저장
            addItemList(itemName);
            //인벤토리 관련 인스턴스를 사용하려면 먼저 false상태에서 active상태로 해야함
            iv.AddItem(idb.itemCount);
            idb.GetItemInfo(idb.itemCount);
            if (idb.selectItem == null)
            {
                Debug.Log("idb.selectItem == null 오류");
                return;
            }
            //먹은 아이템이 무기면
            else if (idb.selectItem.itemType == ItemType.Equipment)
            {
                //transform.Find("Cylinder002").transform.Find(itemName).gameObject.SetActive(true);
                //pv.RPC("EquipObject", PhotonTargets.All, "Cylinder002", itemName, true);
                pv.RPC("EquipObject", PhotonTargets.All, itemName, true);
                //Debug.Log("item 찾은 이름 : "+ transform.Find("Cylinder002").transform.Find(itemName).gameObject.name);
                Debug.Log("무기 먹음");
                pv.RPC("acquireObject", PhotonTargets.All, false);
            }
            //먹은 아이템이 장식품이면
            else if (idb.selectItem.itemType == ItemType.Misc)
            {
                Debug.Log("장식품 먹음");
                //itemObj.SetActive(false);
                pv.RPC("acquireObject",PhotonTargets.All, false);
            }
            //먹은 아이템이 소모품이면
            else
            {
                Debug.Log("소모품 먹음");
                //itemObj.SetActive(false);
                pv.RPC("acquireObject",PhotonTargets.All,false);
            }
            //idb.itemObjs.Add(itemObj);
            isGetItemflag = false;
        }
    }

    //장비를 모두 입혀놓고 true, false 하는 것으로
    //[PunRPC]
    //void EquipObject(string parentName, string childName, bool state)
    //{
    //    Debug.Log("GetSiblingIndex : " + transform.Find(parentName).gameObject.name);
    //    transform.Find(parentName).transform.Find(childName).gameObject.SetActive(state);
    //}    
    [PunRPC]
    void EquipObject( string itemName, bool state)
    {
        if (itemName == "helmets")
        {
            helmetsState.SetActive(state);
        }
        else if (itemName == "bag")
        {
            bagState.SetActive(state);
        }
        else if (itemName == "rings")
        {
            gunState.SetActive(state);
        }
        else if (itemName == "b_t_01")
        {
            armorState.SetActive(state);
        }
    }

    [PunRPC]
    void acquireObject(bool State) {
        if (itemObj != null)
        {
            itemObj.SetActive(State);
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
            itemObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int layerIndex = other.gameObject.layer;
        if (LayerMask.LayerToName(layerIndex) == "Item")
        {
            isGetItemflag = false;
            itemName = string.Empty;
            itemObj = null;
        }
    }

    void addItemList(string itemName)
    {
        if (itemName == "helmets")
        {

            idb.Add("helmets", 1, 50, "Good helmets", idb.itemCount, ItemType.Consumption, itemObj);
        }
        else if (itemName == "b_t_01")
        {
          idb.Add("b_t_01", 1, 10, "b_t_01", idb.itemCount, ItemType.Misc, itemObj);
        }
        else if (itemName == "bag")
        {
            idb.Add("bag", 1, 2000, "Beautiful Bag", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "rings")
        {
            idb.Add("rings", 1, 800, "Beautiful rings", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "gem")
        {
            idb.Add("gem", 1, 800, "Beautiful gem", idb.itemCount, ItemType.Misc, itemObj);
        }
        else
        {
            Debug.Log("응 오류.");
        }
    }
}
