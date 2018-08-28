using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBulletCheckCollider : MonoBehaviour {

    void OnCollisionEnter(Collision other)
    {
        int collisionLayer = other.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
        }
        else
        if (collisionLayer == LayerMask.NameToLayer("Ground2") || collisionLayer == LayerMask.NameToLayer("Building"))
        {
            //바닥에 꽂힌 순간부터 충돌 불가(Bullet -> Default로 변경해 캐릭터와 충돌체크 불가)
            gameObject.layer = 0;
            Invoke("DestroyBullet", 2.0f);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
