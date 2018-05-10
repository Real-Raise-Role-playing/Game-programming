using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerState : Photon.MonoBehaviour
{
    //public enum PLAYERSTATE
    //{
    //    NONE = 0,
    //    GROGGY = 1,
    //    DEAD
    //}
    //Player 스크립트
    //NONE = 0 , GROGGY = 1 , DEAD = 3
    public int playerStateNum = 0;
    private FireScript fireScript = null;
    private Rigidbody rb = null;
    private PhotonView pv = null;
    private PhotonManager pm = null;
    //플레이어 상태
    //public bool isDead = false;
    //public bool groggy = false;
    public string check = "비 접근";

    //플레이어 속성
    public bool isGrounded = false;
    [SerializeField]
    private int currHp = 0;
    //[HideInInspector] public int initHp = 100;

    //플레이어 비활성화
    private MeshRenderer[] renderers;
    private SkinnedMeshRenderer[] skinRenderers;


    void Awake()
    {
        //pm = transform.Find("PhotonManager").gameObject.GetComponent<PhotonManager>();
        rb = GetComponent<Rigidbody>();
        fireScript = GetComponent<FireScript>();
        currHp = Constants.initHp;
        pv = GetComponent<PhotonView>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        skinRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    int myTeamNum = 0;
    int otherTeamNum = 0;
    string otherName = "미상";
    string state = "미상";
    private void OnCollisionEnter(Collision other)
    {
        int collisionLayer = other.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Bullet"))
        {
            if (currHp > 0)
            {
                DamageByEnemy((currHp-20));
            }
            else if (currHp <= 0 && playerStateNum != Constants.GROGGY)
            {
                //일정 체력을 준다.
                DamageByEnemy(40);
                rb.isKinematic = false;
                fireScript.enabled = false;
                playerStateNum = Constants.GROGGY;
                SetPlayerVisible(false, Constants.GROGGY);
            }
            else if (currHp <= 0 && playerStateNum == Constants.GROGGY)
            {
                DamageByEnemy(0);
                SetPlayerVisible(false, Constants.DEAD);
                playerStateNum = Constants.DEAD;
            }
        }
        else if (collisionLayer == LayerMask.NameToLayer("Player") && this.gameObject != other.gameObject)
        {
            PhotonView otPV = other.transform.GetComponent<PhotonView>();
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            PhotonPlayer otherPhotonPlayer = PhotonPlayer.Find(otPV.ownerId);
            PhotonPlayer myPhotonPlayer = PhotonPlayer.Find(pv.ownerId);
            PlayerState otherPs = other.gameObject.GetComponent<PlayerState>();
            FireScript otherFs = other.gameObject.GetComponent<FireScript>();
            //충돌 플레이어의 팀과 나의 팀이 같다면
            if (otherPhotonPlayer.GetTeam() == myPhotonPlayer.GetTeam())
            {
                otherRb.isKinematic = false;
                rb.isKinematic = false;
                check = "우리팀 접근 성공";
                if (otherPs.playerStateNum == Constants.GROGGY)
                {
                    Debug.Log("F접근 전");
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        check = "살리기 성공";
                        otherPs.currHp = 40;
                        otherPs.SetPlayerVisible(true, Constants.GROGGY);
                        otherPs.playerStateNum = Constants.NONE;
                        otherFs.enabled = true;
                        otherPs.rb.isKinematic = true;
                    }
                }
            }
            else
            {
                check = "상대팀 접근 성공";
            }
        }
        if (collisionLayer == LayerMask.NameToLayer("Ground") && isGrounded != true)
        {
            isGrounded = true;
        }
    }

    //RPC보단 느리다.
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //    {
    //        stream.SendNext(this.currHp);
    //        stream.SendNext(this.playerStateNum);
    //    }
    //    else
    //    {
    //        this.currHp = (int)stream.ReceiveNext();
    //        this.playerStateNum = (int)stream.ReceiveNext();
    //    }
    //}

    void DamageByEnemy(int myHealth)
    {
        currHp = myHealth;
        pv.RPC("otherDamageByEnemy", PhotonTargets.Others, currHp);
    }
    [PunRPC]
    void otherDamageByEnemy(int myHealth) {
        currHp = myHealth;
    }
    
    void SetPlayerVisible(bool isVisible, int state)
    {
        if (state == Constants.GROGGY)
        {
            foreach (MeshRenderer _renderer in renderers)
            {
                _renderer.enabled = isVisible;
            }
        }
        else if (state == Constants.DEAD)
        {
            foreach (SkinnedMeshRenderer _skinRenderers in skinRenderers)
            {
                _skinRenderers.enabled = isVisible;
            }
        }
        pv.RPC("otherSetPlayerVisible", PhotonTargets.Others, isVisible, state);
    }
    [PunRPC]
    void otherSetPlayerVisible(bool isVisible, int state)
    {
        if (state == Constants.GROGGY)
        {
            foreach (MeshRenderer _renderer in renderers)
            {
                _renderer.enabled = isVisible;
            }
        }
        else if (state == Constants.DEAD)
        {
            foreach (SkinnedMeshRenderer _skinRenderers in skinRenderers)
            {
                _skinRenderers.enabled = isVisible;
            }
        }
    }
}