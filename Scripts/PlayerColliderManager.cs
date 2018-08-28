using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderManager : Photon.MonoBehaviour
{

    PlayerState ps = null;
    PhotonView pv = null;
    CharacterMove cm = null;
    ParticleManager pm = null;
    ParticleSystem[] Particles;
    //StateUIControl suc = null;
    StateBarManager sbm = null;
    // Use this for initialization
    void Start()
    {
        //suc = transform.root.GetComponentInChildren<StateUIControl>();
        pv = transform.root.GetComponent<PhotonView>();
        if (pv.isMine)
        {
            sbm = transform.root.GetComponentInChildren<StateBarManager>();
            pm = transform.root.GetComponent<ParticleManager>();
            ps = transform.root.GetComponent<PlayerState>();
            cm = transform.root.GetComponent<CharacterMove>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!pv.isMine || ps.playerStateNum == Constants.DEAD) { return; }

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
            ps.playerStateUpdate();
            pm.Action(this.gameObject.name);
            StartCoroutine(sbm.delayTime(2.0f));
        }
        else if (collisionLayer == LayerMask.NameToLayer("Knife"))// && !transform.root.gameObject) // cm.melee_attack)
        {
            sbm.beShotImg.color = sbm.beShotImgColor;
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
                    ps.currHp -= 20;
                }
            }
            else if (this.gameObject.name == "LCalfCollider" || this.gameObject.name == "RCalfCollider")
            {
                if (ps.currHp > 0)
                {
                    Debug.Log("허벅지 맞음");
                    ps.currHp -= 24;

                }
            }
            else if (this.gameObject.name == "BodyCollider")
            {
                if (ps.currHp > 0)
                {
                    Debug.Log("몸 맞음");
                    ps.currHp -= 30;
                }
            }
            ps.playerStateUpdate();
            pm.Action(this.gameObject.name);
            StartCoroutine(sbm.delayTime(2.0f));
        }
        else if (collisionLayer == LayerMask.NameToLayer("Ground2") && !ps.isGrounded)
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
