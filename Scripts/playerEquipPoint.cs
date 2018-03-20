using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEquipPoint : MonoBehaviour {
    GameObject player;
    GameObject EquipPoint;
    bool isPlayerEnter;
    CharacterMove playerMove;
	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
        EquipPoint = GameObject.FindGameObjectWithTag("EquipPoint");
        playerMove = player.GetComponent<CharacterMove>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.L) && isPlayerEnter)
        {
            transform.SetParent(EquipPoint.transform);
            transform.localPosition = Vector3.zero;
            transform.rotation = new Quaternion(0,0,0,0);
            
            isPlayerEnter = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isPlayerEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isPlayerEnter = false;
        }
    }
    public void PickUp(GameObject item)
    {
        SetEquip(item, true);
    }

    void Drop()
    {
        GameObject item = EquipPoint.GetComponentInChildren<Rigidbody>().gameObject;
        SetEquip(item, false);

        EquipPoint.transform.DetachChildren();
    }

    void SetEquip(GameObject item, bool isEquip)
    {
        Collider[] itemColliders = item.GetComponents<Collider>();
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();

        foreach (Collider itemCollider in itemColliders)
        {
            itemCollider.enabled = !isEquip;
        }

        itemRigidbody.isKinematic = isEquip;
    }
}
