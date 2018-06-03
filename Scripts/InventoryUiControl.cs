using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiControl : MonoBehaviour {
    public GameObject inventoryObj;
    PhotonView pv = null;
    // Use this for initialization
    void Awake () {
        pv = transform.root.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            inventoryObj = Instantiate((GameObject)Resources.Load("Inventory"), transform.position, transform.rotation) as GameObject;
            inventoryObj.transform.SetParent(transform);
            Transform inventoryTr = transform.Find("InventoryPos").gameObject.transform;
            inventoryObj.transform.position = new Vector3(inventoryTr.position.x, inventoryTr.position.y, inventoryTr.position.z);
            inventoryObj.transform.rotation = new Quaternion(inventoryTr.rotation.x, inventoryTr.rotation.y, inventoryTr.rotation.z, inventoryTr.rotation.w);
            inventoryObj.GetComponentInChildren<Inventory>().enabled = true;
        }
        else
        {
            this.enabled = false;
        }
    }
}
