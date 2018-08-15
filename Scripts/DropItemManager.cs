using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropItemManager : Photon.MonoBehaviour
{
    PhotonView pv = null;
    public static DropItemManager instance;
    public List<GameObject> dropItemList = new List<GameObject>();

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
        dropItemList.AddRange(GameObject.FindGameObjectsWithTag("Item"));
    }

    public void Action(string dropItemName, bool state)
    {
        pv.RPC("acquireObject", PhotonTargets.AllBufferedViaServer, dropItemName, state);
    }

    [PunRPC]
    void acquireObject(string dropItemName, bool state)
    {
        foreach (GameObject item in dropItemList)
        {
            if (item.name == dropItemName)
            {
                item.SetActive(state);
            }
        }
    }
}
