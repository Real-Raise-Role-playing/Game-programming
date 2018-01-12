using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    public static bool putItem = false;
    private GameObject Item = null;
    private void OnTriggerEnter(Collider other)
    {
        int layerIndex = other.gameObject.layer;
        //if (other.CompareTag("Pig")) //태그 비교할땐 컴페어를 주로 사용할 것
        if (LayerMask.LayerToName(layerIndex) == "Item")
        {
            Debug.Log("충돌 체 이름 : " + other.gameObject.name);
            Item = other.gameObject;
            putItem = true;
        }
        
    }
   

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Bird" || other.tag == "Panguin")
        {
            //PlayerMove.instance.moveSpeed = 10.0f;
            putItem = false;
        }
    }
    */

    private void OnTriggerExit(Collider other)
    {
        putItem = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && putItem)
        {
            Destroy(Item);
        }
    }
}
