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
    private PhotonView pv = null;
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

    //플레이어 비활성화
    public Collider[] colliders = null;
    public MeshRenderer[] renderers;
    public SkinnedMeshRenderer[] skinRenderers;
    public Canvas[] canvas = null;
    public CheckCollider checkColliderCs = null;
    public CameraControl CamCon = null;
    public CharacterMove CharMove = null;
    public OptionManager optionManager = null;
    void Awake()
    {
        //player들이 동적할당이 되기전에 가져오는 것인가..?
        CamCon = Camera.main.GetComponent<CameraControl>();
        optionManager = GetComponent<OptionManager>();
        CharMove = GetComponent<CharacterMove>();
        fireScript = GetComponent<FireScript>();
        currHp = Constants.initHp;
        pv = GetComponent<PhotonView>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        skinRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        colliders = GetComponentsInChildren<Collider>();
        canvas = GetComponentsInChildren<Canvas>();
        checkColliderCs = GetComponent<CheckCollider>();
    }
    public void playerStateUpdate()
    {

        //void SetPlayerVisible(bool isVisible, int myHealth, int playerState)
        if (currHp <= 0)
        {
            pv.RPC("SetPlayerVisible", PhotonTargets.AllBufferedViaServer, false, currHp, Constants.DEAD);
        }
        //나의 팀원이 생존해있을때 기절 처리
        //else if (true)
        //{

        //}
        else
        {
            //플레이어 상태가 Constants.NONE 이면 Visble 작업 처리 하지 않음.
            pv.RPC("SetPlayerVisible", PhotonTargets.AllBufferedViaServer, true, currHp, Constants.NONE);
        }
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
            // Mouse Lock
            Cursor.lockState = CursorLockMode.None;
            // Cursor visible
            Cursor.visible = true;
            //optionManager.enabled = false;
            if (pv.isMine)
            {
                CamCon.enabled = false;
            }
            CharMove.enabled = false;
            fireScript.enabled = false;
            hpBarObj.SetActive(false);
            optionManager.enabled = false;
            foreach (GameObject _obj in checkColliderCs.itemList)
            {
                _obj.SetActive(isVisible);
            }
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
            
        }
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}