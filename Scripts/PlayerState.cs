using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerState : Photon.MonoBehaviour
{
    public int playerStateNum = 0;
    //public int teamNum = 0;
    public int killScore = 0;

    private PhotonView pv = null;
    public GameObject hpBarObj = null;
    public GameObject otherUIobj = null;
    public GameObject gameOverUIobj = null;
    public GameObject deathCamPivot = null;
    public bool isGrounded = false;
    //bool isDead = false;
    //플레이어 상태
    //public bool isDead = false;
    //public bool groggy = false;

    [HideInInspector]
    public int currHp = 0;

    //플레이어 비활성화
    private Collider[] colliders = null;
    public MeshRenderer[] renderers = null;
    private SkinnedMeshRenderer[] skinRenderers = null;
    private Canvas[] canvas = null;
    private CheckCollider checkColliderCs = null;
    private CameraControl camCon = null;
    private CharacterMove charMove = null;
    private OptionManager optionManager = null;
    private StateBarManager sbm = null;
    private FireScript fireScript = null;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        camCon = Camera.main.GetComponent<CameraControl>();
        optionManager = GetComponent<OptionManager>();
        charMove = GetComponent<CharacterMove>();
        fireScript = GetComponent<FireScript>();
        currHp = Constants.initHp;
        renderers = GetComponentsInChildren<MeshRenderer>();
        skinRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        colliders = GetComponentsInChildren<Collider>();
        canvas = GetComponentsInChildren<Canvas>();
        checkColliderCs = GetComponent<CheckCollider>();
        sbm = GetComponentInChildren<StateBarManager>();
        if (!pv.isMine)
        {
            otherUIobj.SetActive(false);
            gameOverUIobj.SetActive(false);
            deathCamPivot.SetActive(false);
        }
    }

    [PunRPC]
    void AddScore(int attackPlayerNum) {
        Debug.Log("attackPlayerNum : " + attackPlayerNum);
        foreach (GameObject player in PhotonManager.instance.playerObjList)
        {
            if (player.GetComponent<PhotonView>().viewID == attackPlayerNum)
            {
                player.GetComponent<PlayerState>().killScore++;
            }
        }

        //foreach (PhotonPlayer Player in PhotonNetwork.playerList)
        //{
        //    if (Player.ID == attackPlayerNum)
        //    {
        //        Debug.Log("Player.ID" + Player.ID);
        //        Player.AddScore(1);
        //        break;
        //    }
        //}
    }

    public void playerStateUpdate(int attackPlayerNum)
    {
        //if (!pv.isMine)
        //{
        //    return;
        //}
        //else
        //{
        sbm.HpBarSlider.value = (currHp * 0.01f);
        if (currHp <= 0 && playerStateNum != Constants.DEAD)
        {
            //foreach (GameObject Player in PhotonManager.instance.playerObjList)
            //{
            //    if (Player.GetComponent<PhotonView>().viewID == attackPlayerNum)
            //    {
            //        Player.GetComponent<PhotonView>().viewID.
            //        break;
            //    }
            //}
            playerStateNum = Constants.DEAD;

            //모든 루틴 멈춰야할지?
            StopAllCoroutines();
            CancelInvoke();
            pv.RPC("AddScore", PhotonTargets.AllBufferedViaServer, attackPlayerNum);
            
            //순서 중요
            hpBarObj.SetActive(false);
            //NGUI는 바로 Destroy..를 해야 에러가 안뜸..
            //otherUIobj.SetActive(false);
            Destroy(otherUIobj);

            optionManager.InventoryObj.SetActive(false);
            Camera.main.transform.position = new Vector3(deathCamPivot.transform.position.x, deathCamPivot.transform.position.y, deathCamPivot.transform.position.z);
            Camera.main.transform.eulerAngles = new Vector3(deathCamPivot.transform.eulerAngles.x, deathCamPivot.transform.eulerAngles.y, deathCamPivot.transform.eulerAngles.z);
            charMove.AnimBool("death", true);
            Invoke("DelayGameOverTime", 5.0f);
        }
        else if (currHp > 0 && playerStateNum != Constants.DEAD)
        {
            //플레이어 상태가 Constants.NONE 이면 Visble 작업 처리 하지 않음.
            pv.RPC("SetPlayerVisible", PhotonTargets.AllBufferedViaServer, true, currHp, Constants.NONE);
        }
        //}
    }

    [PunRPC]
    void SetPlayerVisible(bool isVisible, int myHealth, int playerState)
    {
        currHp = myHealth;
        playerStateNum = playerState;
        if (playerState == Constants.GROGGY)
        {
            foreach (MeshRenderer _renderer in renderers)
            {
                _renderer.enabled = isVisible;
            }
        }
        else if (playerState == Constants.DEAD)
        {
            foreach (Canvas _canvas in canvas)
            {
                _canvas.enabled = isVisible;
            }
            foreach (MeshRenderer _renderer in renderers)
            {
                _renderer.enabled = isVisible;
            }
            foreach (SkinnedMeshRenderer _skinRenderers in skinRenderers)
            {
                _skinRenderers.enabled = isVisible;
            }
            foreach (Collider _colliders in colliders)
            {
                _colliders.enabled = isVisible;
            }
            // Mouse Lock
            Cursor.lockState = CursorLockMode.None;
            // Cursor visible
            Cursor.visible = true;
        }
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }

    void DelayGameOverTime()
    {
        pv.RPC("SetPlayerVisible", PhotonTargets.AllBufferedViaServer, false, currHp, Constants.DEAD);
        gameOverUIobj.SetActive(true);
    }

}