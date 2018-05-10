using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCheckCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        //if (!playerState.isDead && other.CompareTag("Player"))
        int collisionLayer = other.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
            //PhotonNetwork.Destroy(gameObject);
            //추가적으로 케릭터를 부딪혔을때 파티클
        }
        else if (collisionLayer == LayerMask.NameToLayer("Ground") || collisionLayer == LayerMask.NameToLayer("Building"))
        {
            //바닥에 꽂힌 순간부터 충돌 불가(Bullet -> Default로 변경해 캐릭터와 충돌체크 불가)
            gameObject.layer = 0;
            Invoke("DestroyBullet",2.0f);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
        //PhotonNetwork.Destroy(gameObject);
    }
}
