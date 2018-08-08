using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DropItemManager : Photon.MonoBehaviour
{
    PhotonView pv = null;
    public static DropItemManager instance;
    public List<GameObject> dropItemList = null;
    GameObject[] dropItems = null;

    // Use this for initialization
    void Awake() {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        pv = GetComponent<PhotonView>();

        //배열을 리스트에 넣기 using System.Linq;필요
        dropItems = GameObject.FindGameObjectsWithTag("Item");
        dropItemList = dropItems.OfType<GameObject>().ToList();
    }

    public void Action(string dropItemName, bool state)
    {
        pv.RPC("acquireObject", PhotonTargets.AllBufferedViaServer, dropItemName, state);
    }

    [PunRPC]
    void acquireObject(string dropItemName, bool state)
    {
        //Debug.Log("acquireObject(string itemName, bool State)" + "실행");
        foreach (GameObject item in dropItemList)
        {
            if (item.name == dropItemName)
            {
                item.SetActive(state);
            }
        }
    }
}
