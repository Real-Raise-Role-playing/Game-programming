using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerState : Photon.PunBehaviour
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
    public string UserId = "Unknown";
    public bool isGrounded = false;
    [SerializeField]
    private int currHp = 0;
    [HideInInspector] public int TeamNum = 0;
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
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
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
            PhotonView otPV = other.transform.GetComponent<PhotonView>();
            ////내총알 이라면 피해 없음
            if ((otPV.viewID / 1000) == (pv.viewID / 1000))
            {
                return;
            }
            if (currHp > 0)
            {
                currHp -= 20;
            }
            if (currHp <= 0 && playerStateNum != Constants.GROGGY)
            {
                //일정 체력을 준다.
                currHp = 40;
                rb.isKinematic = false;
                fireScript.enabled = false;
                playerStateNum = Constants.GROGGY;
                SetPlayerVisible(false, Constants.GROGGY);
            }
            else if (currHp <= 0 && playerStateNum == Constants.GROGGY)
            {
                currHp = 0;
                SetPlayerVisible(false, Constants.DEAD);
                playerStateNum = Constants.DEAD;
            }
            pv.RPC("HealthRPC", PhotonTargets.All, currHp);

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // 우리 캐릭터의 정보를 다른 모든 유저들에게 전송합니다. ^^
            stream.SendNext(this.currHp);
            stream.SendNext(this.playerStateNum);
        }
        else
        {
            // 다른 유저가 정보를 받습니다. ^^
            this.currHp = (int)stream.ReceiveNext();
            this.playerStateNum = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void HealthRPC(int myHealth) {
        currHp = myHealth;
        Debug.Log(myHealth);
        PhotonNetwork.SendOutgoingCommands();
    }
    void OnGUI()
    {
        GUILayout.Label("myTeamNum : " + myTeamNum + " otherTeamNum : " + otherTeamNum + " PlayerName : "+ PhotonNetwork.playerName );// + "otherPs : " + otherPs.TeamNum);
        GUILayout.Label("\n");
        GUILayout.Label("\n");
        GUILayout.Label("\n");
        GUILayout.Label("접촉 : "+ check + " 내 ID : " + UserId);
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