using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCheckCollider : MonoBehaviour
{
    //PlayerState playerState = null; //플레이어 죽음 처리

    //// Use this for initialization
    //void Awake () {
    //    playerState = GetComponent<PlayerState>();
    //}
    

    void OnCollisionEnter(Collision other)
    {
        //if (!playerState.isDead && other.CompareTag("Player"))
        int collisionLayer = other.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("총알 맞음");
            //추가적으로 케릭터를 부딪혔을때 파티클
            Destroy(gameObject);
        }
        else if (collisionLayer == LayerMask.NameToLayer("Ground") || collisionLayer == LayerMask.NameToLayer("Building"))
        {
            Debug.Log("바닥에 꽂힘");
            //Destroy(gameObject);
            Invoke("DestroyBullet",2.0f);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
