using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderManager : Photon.MonoBehaviour
{

    PlayerState ps = null;
    PhotonView pv = null;
    ParticleManager pm = null;
    ParticleSystem[] Particles;
    //StateUIControl suc = null;
    StateBarManager sbm = null;
    // Use this for initialization
    void Start()
    {
        //suc = transform.root.GetComponentInChildren<StateUIControl>();
        sbm = transform.root.GetComponentInChildren<StateBarManager>();
        pm = transform.root.GetComponent<ParticleManager>();
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
            sbm.beShotImg.color = sbm.beShotImgColor;
            //suc.beShotImg.color = suc.beShotImgColor;
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
            string spotName = this.gameObject.name;
            //pv.RPC("ParticleSystemControl", PhotonTargets.All, spotName);
            pm.Action(this.gameObject.name);
            ps.playerStateUpdate();
            StartCoroutine(sbm.delayTime());
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
