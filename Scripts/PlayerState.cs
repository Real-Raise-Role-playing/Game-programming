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
    private CapsuleCollider CC = null;
    public GameObject hpBarObj = null;

    //플레이어 상태
    //public bool isDead = false;
    //public bool groggy = false;
    public string check = "비 접근";

    //플레이어 속성
    public bool isGrounded = false;
    //[SerializeField]
    [HideInInspector]
    public int currHp = 0;
    //[HideInInspector] public int initHp = 100;

    //플레이어 비활성화
    private MeshRenderer[] renderers;
    private SkinnedMeshRenderer[] skinRenderers;


    void Awake()
    {
        //pm = transform.Find("PhotonManager").gameObject.GetComponent<PhotonManager>();
        CC = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        fireScript = GetComponent<FireScript>();
        currHp = Constants.initHp;
        pv = GetComponent<PhotonView>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        skinRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
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
        if (collisionLayer == LayerMask.NameToLayer("Bullet"))
        {
            Destroy(other.gameObject);
            string myTeam = PhotonNetwork.player.GetTeam().ToString();
            if (currHp > 20)
            {
                //DamageByEnemy((currHp-20), Constants.NONE);
                //currHp -= 20;
                Debug.Log("1번 충돌");
                pv.RPC("DamageByEnemy", PhotonTargets.All, (currHp - 20), Constants.NONE);

            }
            else if (currHp <= 20 && playerStateNum != Constants.NONE && myTeam == "red" && PunTeams.PlayersPerTeam[PunTeams.Team.red].Count > 1)
            {
                //일정 체력을 준다.
                rb.isKinematic = false;
                fireScript.enabled = false;
                //playerStateNum = Constants.GROGGY;
                //SetPlayerVisible(false, Constants.GROGGY);
                //DamageByEnemy(40, Constants.GROGGY);

                Debug.Log("2번 충돌");
                pv.RPC("SetPlayerVisible", PhotonTargets.All, false, Constants.GROGGY);
                //currHp = 40;
                pv.RPC("DamageByEnemy", PhotonTargets.All, 40, Constants.GROGGY);
            }
            else if (currHp <= 20 && playerStateNum != Constants.NONE && myTeam == "blue" && PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count > 1)
            {
                //일정 체력을 준다.
                rb.isKinematic = false;
                fireScript.enabled = false;
                //playerStateNum = Constants.GROGGY;
                //SetPlayerVisible(false, Constants.GROGGY);
                //DamageByEnemy(40, Constants.GROGGY);

                Debug.Log("3번 충돌");
                pv.RPC("SetPlayerVisible", PhotonTargets.All, false, Constants.GROGGY);
                //currHp = 40;
                pv.RPC("DamageByEnemy", PhotonTargets.All, 40, Constants.GROGGY);
            }
            else if (currHp <= 20)
            {
                rb.isKinematic = false;
                fireScript.enabled = false;
                //playerStateNum = Constants.DEAD;
                //DamageByEnemy(0, Constants.DEAD);
                //SetPlayerVisible(false, Constants.DEAD);

                Debug.Log("4번 충돌");
                pv.RPC("SetPlayerVisible", PhotonTargets.All, false, Constants.DEAD);
                //currHp = 0;
                pv.RPC("DamageByEnemy", PhotonTargets.All, 0, Constants.DEAD);
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
                        otherPs.SetPlayerVisible(true, Constants.NONE);
                        //otherPs.playerStateNum = Constants.NONE;
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

    //RPC보단 자주 사용될때
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

    [PunRPC]
    void DamageByEnemy(int myHealth, int playerState)
    {
        currHp = myHealth;
        playerStateNum = playerState;
    }

    [PunRPC]
    void SetPlayerVisible(bool isVisible, int playerState)
    {
        //playerStateNum = playerState;
        if (playerState == Constants.GROGGY)
        {
            foreach (MeshRenderer _renderer in renderers)
            {
                _renderer.enabled = isVisible;
            }
        }
        else if (playerState == Constants.DEAD)
        {
            hpBarObj.SetActive(false);
            CC.enabled = false;
            foreach (MeshRenderer _renderer in renderers)
            {
                _renderer.enabled = isVisible;
            }
            foreach (SkinnedMeshRenderer _skinRenderers in skinRenderers)
            {
                _skinRenderers.enabled = isVisible;
            }
        }
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}