using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerState : MonoBehaviour
{
    public enum PLAYERSTATE
    {
        GROGGY = 0,
        DEAD
    }
    //Player 생존 확인
    FireScript fireScript = null;
    public bool isDead = false;
    public bool groggy = false;
    //[HideInInspector] public int TeamNum = 0;
    public int TeamNum = 1;
    public bool isGrounded = false;
    [HideInInspector] public int initHp = 100;
    [HideInInspector] public int currHp = 0;
    private MeshRenderer[] renderers;
    private SkinnedMeshRenderer[] skinRenderers;
    private Rigidbody rb = null;
    private PhotonView pv = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fireScript = GetComponent<FireScript>();
        currHp = initHp;
        pv = GetComponent<PhotonView>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        skinRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        int collisionLayer = other.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Bullet"))
        {
            if (currHp > 0)
            {
                currHp -= 20;
            }
            if (currHp <= 0 && !groggy)
            {
                //일정 체력을 준다.
                currHp = 40;
                rb.isKinematic = false;
                fireScript.enabled = false;
                groggy = true;
                SetPlayerVisible(false, PLAYERSTATE.GROGGY);
            }
            else if (currHp <= 0 && groggy)
            {
                currHp = 0;
                SetPlayerVisible(false, PLAYERSTATE.DEAD);
            }
        }
        else if (collisionLayer == LayerMask.NameToLayer("Player") && this.gameObject != other.gameObject)
        {
            PlayerState ps = other.gameObject.GetComponent<PlayerState>();
            if (ps.TeamNum == this.TeamNum)
            {
                if (ps.groggy)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        ps.currHp = 40;
                        ps.SetPlayerVisible(true,PLAYERSTATE.GROGGY);
                        ps.groggy = false;
                        ps.fireScript.enabled = true;
                        ps.rb.isKinematic = true;
                    }
                }
            }
        }
        else if (collisionLayer == LayerMask.NameToLayer("Ground")) {
            Debug.Log("계속찍나?");
            isGrounded = true;
        }

    }

    void SetPlayerVisible(bool isVisible, PLAYERSTATE state)
    {
        if (state == PLAYERSTATE.GROGGY)
        {
            foreach (MeshRenderer _renderer in renderers)
            {
                _renderer.enabled = isVisible;
            }
        }
        else if (state == PLAYERSTATE.DEAD)
        {
            foreach (SkinnedMeshRenderer _skinRenderers in skinRenderers)
            {
                _skinRenderers.enabled = isVisible;
            }
        }
    }
    //void Update()
    //{
    //    if (!pv.isMine)
    //    {
    //        return;
    //    }

    //}


    //public void DamageByEnemy()
    //{
    //    if (isDead)
    //    {
    //        return;
    //    }
    //    if (currHp <= 0)
    //    {
    //        isDead = true;
    //        Gun.SetActive(false);
    //    }
    //}

}
