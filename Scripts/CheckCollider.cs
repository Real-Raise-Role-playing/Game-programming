using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        int layerIndex = other.gameObject.layer;
        //if (other.CompareTag("Pig")) //태그 비교할땐 컴페어를 주로 사용할 것
        if (LayerMask.LayerToName(layerIndex) == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("충돌 체 이름 : " + other.gameObject.name);
            Destroy(gameObject);
        }
     }
}
