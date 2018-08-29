﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : Photon.MonoBehaviour
{
    bool[] playerEquipState = new bool[Constants.equipmentMaxCount];
    enum EQUIP_STATE
    {
        HELMET = 0,
        GRENADE,
        SHOVEL,
        BLANKET,
        //CANTEEN
    }
    GameObject menuPos;
    public GameObject helmetsState = null;
    public GameObject canteen = null;
    public GameObject blanket = null;
    public GameObject shovel = null;
    public GameObject grenade = null;
    public List<GameObject> itemList = new List<GameObject>();
    public bool pickUpAnimCheck = false;

    string dropItemName = string.Empty;

    private ItemDatabase idb = null;
    private Inventory iv = null;
    //아이템 줍기 가능 상태 여부 Flag
    public bool isGetItemflag = false;
    public Transform WeaponTr;
    private GameObject itemObj = null;
    private string itemName = null;
    private PhotonView pv = null;
    private FireScript fs = null;
    private OptionManager om = null;
    private PlayerState ps = null;
    private StateBarManager sbm = null;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (pv.isMine)
        {
            ps = GetComponent<PlayerState>();
            om = GetComponent<OptionManager>();
            idb = GetComponentInChildren<ItemDatabase>();
            iv = GetComponentInChildren<Inventory>();
            fs = GetComponent<FireScript>();
            menuPos = GameObject.Find("MenuPos");
            sbm = GetComponentInChildren<StateBarManager>();
        }
        pickUpAnimCheck = false;
    }

    void Update()
    {
        if (!pv.isMine || ps.playerStateNum == Constants.DEAD) { return; }
        if (Input.GetKeyDown(KeyCode.F) && isGetItemflag && itemObj != null)
        {
            //2개이상 템을 먹지 못하도록 제한
            if (itemName == "helmet" && playerEquipState[(int)EQUIP_STATE.HELMET] ||
                itemName == "blanket" && playerEquipState[(int)EQUIP_STATE.BLANKET] || itemName == "shovel" && playerEquipState[(int)EQUIP_STATE.SHOVEL] ||
               itemName == "grenade" && playerEquipState[(int)EQUIP_STATE.GRENADE]) //itemName == "canteen" && playerEquipState[(int)EQUIP_STATE.CANTEEN] || 
            {
                Debug.Log("두개 안돼");
                return;
            }
            idb.itemCount++;
            //InsertItemList(itemName);
            AddItemList(itemName);
            //문제발생부 함수내부 ItemImageChange()함수 오작동
            //iv.AddItem(idb.itemCount);
            iv.AddItem();

            idb.GetItemInfo(idb.itemCount);

            if (idb.selectItem == null)
            {
                Debug.Log("idb.selectItem == null 오류");
                return;
            }
            //먹은 아이템이 무기면
            else if (idb.selectItem.itemType == ItemType.Equipment)
            {
                //pv.RPC("EquipObject", PhotonTargets.AllBufferedViaServer, itemName, true);
                DropItemManager.instance.Action(dropItemName, false);
            }
            //먹은 아이템이 장식품이면
            else if (idb.selectItem.itemType == ItemType.Misc)
            {
                DropItemManager.instance.Action(dropItemName, false);
            }
            //먹은 아이템이 소모품이면
            else
            {
                DropItemManager.instance.Action(dropItemName, false);
            }
            isGetItemflag = false;
            pickUpAnimCheck = true;

            //확인용
            idb.itemsName.Clear();
            for (int i = 0; i < idb.items.Count; i++)
            {
                idb.itemsName.Add(idb.items[i].itemName);
            }

        }
        //&& !isGetItemflag 고려
        else if (Input.GetKeyUp(KeyCode.F) && !isGetItemflag)
        {
            pickUpAnimCheck = false;
        }

        if (iv.enteredSlot != null && Input.GetMouseButtonDown(1) && om.InventoryOn)
        {
            Debug.Log("사용 아이템 : " + iv.enteredSlot.item.itemName);

            if (iv.enteredSlot.item.itemType == ItemType.Equipment)
            {
                pv.RPC("EquipObject", PhotonTargets.AllBufferedViaServer, iv.enteredSlot.item.itemName, true);
                idb.equipItems.Add(iv.enteredSlot.item);
                foreach (GameObject item in DropItemManager.instance.dropItemList)
                {
                    if (item.name == iv.enteredSlot.item.originalName)
                    {
                        idb.equipItemObjs.Add(item);
                        break;
                    }
                }
                idb.Remove(iv.enteredSlot.item.itemCount);
            }
            else if (iv.enteredSlot.item.itemType == ItemType.Consumption)
            {
                if (iv.enteredSlot.item.itemName == "firstaid")
                {
                    if (ps.currHp < 100)
                    {
                        ps.currHp += 30;
                        if (ps.currHp > 100)
                        {
                            ps.currHp = 100;
                        }
                        //사용 하였으면 삭제
                        //idb.Remove(iv.enteredSlot.item.itemCount);
                        ps.playerStateUpdate(PhotonNetwork.player.ID);
                    }
                    else
                    {
                        Debug.Log("사용 불가");
                        return;
                    }
                }
                else if (iv.enteredSlot.item.itemName == "canteen")
                {
                    pv.RPC("EquipObject", PhotonTargets.AllBufferedViaServer, iv.enteredSlot.item.itemName, true);
                    if (sbm.HangerBarSlider.value < 100)
                    //if (suc.HangerBarSlider.value < 100)
                    {
                        //suc.HangerBarSlider.value += 10;
                        sbm.HangerBarSlider.value += 10;
                        //if (suc.HangerBarSlider.value > 100)
                        if (sbm.HangerBarSlider.value > 100)
                        {
                            sbm.HangerBarSlider.value = 100;
                            //suc.HangerBarSlider.value = 100;
                        }
                        //사용 하였으면 삭제
                        ps.playerStateUpdate(PhotonNetwork.player.ID);
                    }
                    else
                    {
                        Debug.Log("사용 불가");
                        return;
                    }
                }
                else if (itemName == "bulletSet")
                {
                    fs.havingBulletCount += 30;
                }
                idb.Remove(iv.enteredSlot.item.itemCount);
            }
        }
    }

    [PunRPC]
    void EquipObject(string itemName, bool state)
    {
        if (itemName == "helmet")
        {
            helmetsState.SetActive(state);
            itemList.Add(helmetsState);
            playerEquipState[(int)EQUIP_STATE.HELMET] = true;
        }
        else if (itemName == "canteen")
        {
            canteen.SetActive(state);
            itemList.Add(canteen);
            //playerEquipState[(int)EQUIP_STATE.CANTEEN] = true;
        }
        else if (itemName == "blanket")
        {
            blanket.SetActive(state);
            itemList.Add(blanket);
            playerEquipState[(int)EQUIP_STATE.BLANKET] = true;
        }
        else if (itemName == "shovel")
        {
            shovel.SetActive(state);
            itemList.Add(shovel);
            playerEquipState[(int)EQUIP_STATE.SHOVEL] = true;
        }
        else if (itemName == "grenade")
        {
            grenade.SetActive(state);
            itemList.Add(grenade);
            playerEquipState[(int)EQUIP_STATE.GRENADE] = true;
        }
        //else if (itemName == "bulletSet")
        //{
        //    fs.havingBulletCount += 30;
        //}
        else
        {
            Debug.Log("없음");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pv.isMine)
        {
            dropItemName = string.Empty;
            isGetItemflag = false;
            itemName = string.Empty;
            itemObj = null;
            return;
        }
        else
        {
            dropItemName = other.gameObject.name;
            if (other.CompareTag("Item")) //태그 비교할땐 컴페어를 주로 사용할 것
            //int layerIndex = other.gameObject.layer;
            //if (LayerMask.LayerToName(layerIndex) == "Item")
            {
                isGetItemflag = true;
                //itemName = other.gameObject.tag;
                //공백으로 띄어쓰기
                string[] _itempName = other.gameObject.name.Split('\x020');
                itemName = _itempName[0];
                itemObj = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!pv.isMine)
        {
            return;
        }
        else
        {
            dropItemName = string.Empty;
            isGetItemflag = false;
            itemName = string.Empty;
            itemObj = null;
        }
    }

    void AddItemList(string itemName)
    {
        Debug.Log("addItemList(string itemName) : " + itemName);
        if (itemName == "helmet")
        {
            idb.Add("helmet", 1, dropItemName, idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "firstaid")
        {
            idb.Add("firstaid", 1, dropItemName, idb.itemCount, ItemType.Consumption, itemObj);
        }
        else if (itemName == "canteen")
        {
            idb.Add("canteen", 1, dropItemName, idb.itemCount, ItemType.Consumption, itemObj);
        }
        else if (itemName == "blanket")
        {
            idb.Add("blanket", 1, dropItemName, idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "shovel")
        {
            idb.Add("shovel", 1, dropItemName, idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "grenade")
        {
            idb.Add("grenade", 1, dropItemName, idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "bulletSet")
        {
            idb.Add("bulletSet", 1, dropItemName, idb.itemCount, ItemType.Consumption, itemObj);
        }
        else
        {
            Debug.Log("응 오류.");
        }
    }

    void InsertItemList(string itemName)
    {
        if (itemName == "helmet")
        {
            idb.Insert("helmet", 1, dropItemName, idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "firstaid")
        {
            idb.Insert("firstaid", 1, dropItemName, idb.itemCount, ItemType.Consumption, itemObj);
        }
        else if (itemName == "canteen")
        {
            idb.Insert("canteen", 1, dropItemName, idb.itemCount, ItemType.Consumption, itemObj);
        }
        else if (itemName == "blanket")
        {
            idb.Insert("blanket", 1, dropItemName, idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "shovel")
        {
            idb.Insert("shovel", 1, dropItemName, idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "grenade")
        {
            idb.Insert("grenade", 1, dropItemName, idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "bulletSet")
        {
            idb.Insert("bulletSet", 1, dropItemName, idb.itemCount, ItemType.Consumption, itemObj);
        }
        else
        {
            Debug.Log("응 오류.");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label("Player Kill Score : " + ps.killScore);
        //GUILayout.Label("Player Kill Score : " + PhotonNetwork.player.GetScore());
        if (GUILayout.Button("Leave Room"))
        {
            // Mouse Lock
            Cursor.lockState = CursorLockMode.None;
            // Cursor visible
            Cursor.visible = true;
            Camera.main.transform.SetParent(menuPos.transform);
            Camera.main.GetComponent<CameraControl>().enabled = false;
            Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
            Camera.main.transform.position = Vector3.zero;
            Camera.main.transform.rotation = Quaternion.identity;
            int veiwId = transform.GetComponent<PhotonView>().viewID;
            ps.gameOverUIobj.SetActive(false);
            PhotonManager.instance.LeavePlayer(veiwId);

            //슬라임에게 플레이어 정보를 다시 전송 해야함.
            //Slime.instance.enabled = false;
            //pv.RPC("LeavePlayer", PhotonTargets.AllBufferedViaServer, veiwId);
            //Slime.instance.enabled = true;

            PhotonNetwork.Destroy(PhotonView.Find(veiwId).gameObject);
            PhotonNetwork.LeaveRoom();
        }
    }
}
