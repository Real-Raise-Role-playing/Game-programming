using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : Photon.MonoBehaviour
{
    bool[] playerEquipState = new bool[Constants.equipmentMaxCount];
    enum EQUIP_STATE
    {
        HELMET = 0,
        //BAG,
        GRENADE,
        SHOVEL,
        BLANKET,
        CANTEEN
    }
    GameObject menuPos;

    //public GameObject bagState = null;
    public GameObject helmetsState = null;
    public GameObject canteen = null;
    public GameObject blanket = null;
    public GameObject shovel = null;
    public GameObject grenade = null;
    public List<GameObject> itemList = null;
    public bool pickUpAnimCheck = false;

    //DropItemManager dim = null;
    string dropItemName = string.Empty;

    private ItemDatabase idb = null;
    private Inventory iv = null;
    //아이템 줍기 가능 상태 여부 Flag
    public Transform WeaponTr;
    public bool isGetItemflag = false;
    private GameObject itemObj = null;
    private string itemName = null;
    private PhotonView pv = null;
    private PhotonManager pm = null;
    private FireScript fs = null;
    void Awake()
    {
        idb = GetComponentInChildren<ItemDatabase>();
        iv = GetComponentInChildren<Inventory>();
        pv = GetComponent<PhotonView>();
        pm = GetComponent<PhotonManager>();
        fs = GetComponent<FireScript>();
        menuPos = GameObject.Find("MenuPos");
    }

    //void Start()
    //{
        //dim = GameObject.Find("Items").GetComponent<DropItemManager>();
    //}

    void Update()
    {
        if (!pv.isMine) { return; }
        if (Input.GetKeyDown(KeyCode.F) && isGetItemflag && itemObj != null)
        {
            if (itemName == "helmet" && playerEquipState[(int)EQUIP_STATE.HELMET] ||
                itemName == "blanket" && playerEquipState[(int)EQUIP_STATE.BLANKET] || itemName == "shovel" && playerEquipState[(int)EQUIP_STATE.SHOVEL] ||
               itemName == "canteen" && playerEquipState[(int)EQUIP_STATE.CANTEEN] || itemName == "grenade" && playerEquipState[(int)EQUIP_STATE.GRENADE])
            {
                Debug.Log("두개 안돼");
                return;
            }
            idb.itemCount++;
            insertItemList(itemName);
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
                pv.RPC("EquipObject", PhotonTargets.AllBufferedViaServer, itemName, true);
                DropItemManager.instance.Action(dropItemName, false);
                Debug.Log("무기 먹음");
            }
            //먹은 아이템이 장식품이면
            else if (idb.selectItem.itemType == ItemType.Misc)
            {
                Debug.Log("장식품 먹음");
                DropItemManager.instance.Action(dropItemName, false);
            }
            //먹은 아이템이 소모품이면
            else
            {
                Debug.Log("소모품 먹음");
                DropItemManager.instance.Action(dropItemName, false);
            }
            isGetItemflag = false;
            pickUpAnimCheck = true;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            pickUpAnimCheck = false;
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
            playerEquipState[(int)EQUIP_STATE.CANTEEN] = true;
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
        else if (itemName == "bulletSet")
        {
            fs.havingBulletCount += 30;
        }
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

    void insertItemList(string itemName)
    {
        if (itemName == "helmet")
        {
            idb.Insert("helmet", 1, 50, "Good helmets", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "firstaid")
        {
            idb.Insert("firstaid", 1, 10, "Good firstaid", idb.itemCount, ItemType.Consumption, itemObj);
        }
        else if (itemName == "canteen")
        {
            idb.Insert("canteen", 1, 800, "Beautiful canteen", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "blanket")
        {
            idb.Insert("blanket", 1, 800, "Beautiful blanket", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "shovel")
        {
            idb.Insert("shovel", 1, 800, "Beautiful shovel", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "grenade")
        {
            idb.Insert("grenade", 1, 800, "Beautiful grenade", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "bulletSet")
        {
            idb.Insert("bulletSet", 1, 800, "Beautiful bulletSet", idb.itemCount, ItemType.Equipment, itemObj);
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
            Camera.main.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            int veiwId = transform.GetComponent<PhotonView>().viewID;
            Debug.Log("나간놈 ID : " + veiwId);
            //foreach (GameObject player in pm.playerList)
            //{
            //    if (player.name == veiwId.ToString())
            //    {
            //        pm.playerList.Remove(player);
            //    }
            //}
            PhotonNetwork.Destroy(PhotonView.Find(veiwId).gameObject);
            PhotonNetwork.LeaveRoom();
        }
    }
}
