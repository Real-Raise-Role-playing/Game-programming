using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : Photon.MonoBehaviour
{
    GameObject menuPos;

    public GameObject bagState = null;
    public GameObject helmetsState = null;
    public GameObject canteen = null;
    public GameObject blanket = null;
    public GameObject shovel = null;
    public List<GameObject> itemList = null;

    private ItemDatabase idb = null;
    private Inventory iv = null;
    //아이템 줍기 가능 상태 여부 Flag
    public Transform WeaponTr;
    public bool isGetItemflag = false;
    private GameObject itemObj = null;
    private string itemName = null;
    private PhotonView pv = null;
    private PhotonManager pm = null;
    void Awake()
    {
        idb = GetComponentInChildren<ItemDatabase>();
        iv = GetComponentInChildren<Inventory>();
        pv = GetComponent<PhotonView>();
        pm = GetComponent<PhotonManager>();
        menuPos = GameObject.Find("MenuPos");

    }

    void Update()
    {
        if (!pv.isMine) { return; }
        if (Input.GetKeyDown(KeyCode.F) && isGetItemflag)
        {
            idb.itemCount++;
            addItemList(itemName);
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
                Debug.Log("무기 먹음");
                pv.RPC("acquireObject", PhotonTargets.AllBufferedViaServer, false);
            }
            //먹은 아이템이 장식품이면
            else if (idb.selectItem.itemType == ItemType.Misc)
            {
                Debug.Log("장식품 먹음");
                pv.RPC("acquireObject", PhotonTargets.AllBufferedViaServer, false);
            }
            //먹은 아이템이 소모품이면
            else
            {
                Debug.Log("소모품 먹음");
                pv.RPC("acquireObject", PhotonTargets.AllBufferedViaServer, false);
            }
            isGetItemflag = false;
        }
    }
    
    [PunRPC]
    void EquipObject(string itemName, bool state)
    {
        if (itemName == "helmet")
        {
            helmetsState.SetActive(state);
            itemList.Add(helmetsState);
        }
        else if (itemName == "bag")
        {
            bagState.SetActive(state);
            itemList.Add(bagState);
        }
        else if (itemName == "canteen")
        {
            canteen.SetActive(state);
            itemList.Add(canteen);
        }
        else if (itemName == "blanket")
        {
            blanket.SetActive(state);
            itemList.Add(blanket);
        }
        else if (itemName == "shovel")
        {
            shovel.SetActive(state);
            itemList.Add(shovel);
        }
        else
        {
            Debug.Log("없음");
        }
    }

    [PunRPC]
    void acquireObject(bool State)
    {
        if (itemObj != null)
        {
            itemObj.SetActive(State);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pv.isMine)
        {
            isGetItemflag = false;
            itemName = string.Empty;
            itemObj = null;
            return;
        }
        else
        {

            int layerIndex = other.gameObject.layer;
            //if (other.CompareTag("Pig")) //태그 비교할땐 컴페어를 주로 사용할 것
            if (LayerMask.LayerToName(layerIndex) == "Item")
            {
                isGetItemflag = true;
                itemName = other.gameObject.tag;
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
            int layerIndex = other.gameObject.layer;
            if (LayerMask.LayerToName(layerIndex) == "Item")
            {
                isGetItemflag = false;
                itemName = string.Empty;
                itemObj = null;
            }
        }
    }

    void addItemList(string itemName)
    {
        if (itemName == "helmet")
        {

            idb.Add("helmet", 1, 50, "Good helmets", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "firstaid")
        {
            idb.Add("firstaid", 1, 10, "Good firstaid", idb.itemCount, ItemType.Consumption, itemObj);
        }
        else if (itemName == "bag")
        {
            idb.Add("bag", 1, 2000, "Beautiful Bag", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "canteen")
        {
            idb.Add("canteen", 1, 800, "Beautiful canteen", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "blanket")
        {
            idb.Add("blanket", 1, 800, "Beautiful blanket", idb.itemCount, ItemType.Equipment, itemObj);
        }
        else if (itemName == "shovel")
        {
            idb.Add("shovel", 1, 800, "Beautiful shovel", idb.itemCount, ItemType.Equipment, itemObj);
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
