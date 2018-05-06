using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerState : Photon.PunBehaviour
{
    public enum PLAYERSTATE
    {
        GROGGY = 0,
        DEAD
    }
    //Player 스크립트
    private FireScript fireScript = null;
    private Rigidbody rb = null;
    private PhotonView pv = null;
    private PhotonManager pm = null;
    //플레이어 상태
    public bool isDead = false;
    public bool groggy = false;
    public string check = "비 접근";

    //플레이어 속성
    public string UserId = "Unknown";
    public bool isGrounded = false;
    [HideInInspector] public int currHp = 0;
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
            //int playerMax = PunTeams.PlayersPerTeam[PunTeams.Team.red].Count + PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count;
            //for (int i = 0; i < playerMax; i++)
            //{
            //    if (PhotonNetwork.playerList[i].name == other.gameObject.name) {
            //        state = "찾았다.";
            //    }
            //}
            PhotonView otPV = other.transform.GetComponent<PhotonView>();
            otherTeamNum = otPV.viewID;
            foreach (PhotonPlayer pl in PhotonNetwork.playerList)
            {
                if (pl.ID == otPV.viewID)
                {
                    state = "찾았다.";
                }
            }
            //pv.RPC("TaggedPlayer", PhotonTargets.Others, TeamNum);
            check = "일단 플레이어끼리 접근";
            //otherName = other.transform.gameObject.name;
            //other.transform.gameObject.GetPhotonView
            PlayerState ps = other.transform.gameObject.GetComponent<PlayerState>();
            otherName = ps.UserId;
            //PlayerState ps = other.gameObject.GetComponent<PlayerState>();
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            myTeamNum = this.TeamNum;
            //otherTeamNum = ps.TeamNum;
            //otherTeamNum = other.gameObject.GetComponent<PlayerState>().TeamNum;
            if (ps.TeamNum == this.TeamNum)
            {
                check = "우리팀 접근 성공";
                otherRb.isKinematic = false;
                rb.isKinematic = false;
                if (ps.groggy)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        ps.currHp = 40;
                        ps.SetPlayerVisible(true, PLAYERSTATE.GROGGY);
                        ps.groggy = false;
                        ps.fireScript.enabled = true;
                        ps.rb.isKinematic = true;
                    }
                }
            }
        }
        if (collisionLayer == LayerMask.NameToLayer("Ground") && isGrounded != true)
        {
            isGrounded = true;
        }
    }

    //void OnPhotonPlayerConnected(PhotonPlayer player)
    //{
    //    Debug.Log("OnPhotonPlayerConnected: " + player);

    //    // when new players join, we send "who's it" to let them know
    //    // only one player will do this: the "master"
    //    pv.RPC("TaggedPlayer", PhotonTargets.Others, PhotonNetwork.playerName);
    //}

    //[PunRPC]
    //void TaggedPlayer(int playerID)
    //{
    //    TeamNum = playerID;
    //}

    void OnGUI()
    {
        GUILayout.Label("myTeamNum : " + myTeamNum + " otherTeamNum : " + otherTeamNum + " PlayerName : "+ PhotonNetwork.playerName + " otherName : " + otherName );// + "otherPs : " + otherPs.TeamNum);
        GUILayout.Label("\n");
        GUILayout.Label("\n");
        GUILayout.Label("\n");
        GUILayout.Label("접촉? : "+ check + "상태 : " + state + " 내 ID : " + UserId);
    }

    //private void OnCollisionExit(Collision other)
    //{
    //    int collisionLayer = other.gameObject.layer;
    //    if (collisionLayer == LayerMask.NameToLayer("Player") && this.gameObject != other.gameObject)
    //    {
    //        check = "비 접근";
    //    }
    //}
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