using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderManager : MonoBehaviour {
    PlayerState ps = null;
    PhotonView pv = null;
    // Use this for initialization
    void Start()
    {
        pv = transform.root.GetComponent<PhotonView>();
        ps = transform.root.GetComponent<PlayerState>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!pv.isMine)
        {
            return;
        }
        int collisionLayer = other.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Empty"))
        {
            Destroy(other.gameObject);
        }
        else if (collisionLayer == LayerMask.NameToLayer("Bullet"))
        {
            Destroy(other.gameObject);
            if (this.gameObject.name == "HeadCollider")
            {
                if (ps.currHp > 0)
                {
                    Debug.Log("머리 맞음");
                    ps.currHp = 0;
                }
            }
            else if (this.gameObject.name == "RFootCollider" || this.gameObject.name == "LFootCollider")
            {
                if (ps.currHp > 0)
                {
                    Debug.Log("종아리 맞음");
                    ps.currHp -= 10;
                }
            }
            else if (this.gameObject.name == "LCalfCollider" || this.gameObject.name == "RCalfCollider")
            {
                if (ps.currHp > 0)
                {
                    Debug.Log("허벅지 맞음");
                    ps.currHp -= 12;
                }
            }
            else if (this.gameObject.name == "BodyCollider")
            {
                if (ps.currHp > 0)
                {
                    Debug.Log("몸 맞음");
                    ps.currHp -= 20;
                }
            }
            ps.playerStateUpdate();
        }
        else if (collisionLayer == LayerMask.NameToLayer("Ground") && !ps.isGrounded)
        {
            if (this.gameObject.name == "LFootCollider" || this.gameObject.name == "RFootCollider")
            {
                //Debug.Log("발 닿음");
                ps.isGrounded = true;
            }
        }
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
