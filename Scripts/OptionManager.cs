using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionManager : MonoBehaviour
{
    GameObject menuPos;
    //public static bool gameOptionOn = false;
    public GameObject InventoryObj = null;
    PlayerState ps = null;
    FireScript fireScript = null;
    CharacterMove characterMoveScrpt = null;
    CameraControl cameraControlScript = null;
    PhotonView pv = null;
    Inventory Iv = null;
    //PlayerState playerState = null; //플레이어 죽음 처리
    public bool InventoryOn = false;

    void LockCursor()
    {
        // Mouse Lock
        Cursor.lockState = CursorLockMode.Locked;
        // Cursor visible
        Cursor.visible = false;
    }

    void NoneCursor()
    {
        // Mouse Lock
        Cursor.lockState = CursorLockMode.None;
        // Cursor visible
        Cursor.visible = true;
    }

    private void Awake()
    {
        LockCursor();
        menuPos = GameObject.Find("MenuPos");
        InventoryObj = GameObject.Find("Inventory");
        pv = GetComponent<PhotonView>();
        ps = GetComponent<PlayerState>();
        Iv = GetComponentInChildren<Inventory>();
        fireScript = GetComponent<FireScript>();
        characterMoveScrpt = GetComponent<CharacterMove>();
        cameraControlScript = Camera.main.GetComponent<CameraControl>();
        //cameraControlScript = GetComponentInChildren<CameraControl>();
    }

    private void OnGUI()
    {
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        if (GUILayout.Button("Leave Room"))
        {
            // Mouse Lock
            Cursor.lockState = CursorLockMode.None;
            // Cursor visible
            Cursor.visible = true;
            Camera.main.transform.SetParent(menuPos.transform);
            Camera.main.GetComponent<CameraControl>().enabled = false;
            Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
            Camera.main.transform.position = Vector3.zero;
            Camera.main.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            int veiwId = transform.GetComponent<PhotonView>().viewID;
            Debug.Log("나간놈 ID : " + veiwId);
            PhotonNetwork.Destroy(PhotonView.Find(veiwId).gameObject);
            PhotonNetwork.LeaveRoom();
        }
    }

    void Update()
    {
        if (!pv.isMine || ps.playerStateNum == Constants.DEAD) { return; }
        //과연 Update()에서 계속 체크를 하면서 false를 시키는게 속도에 지장이 없을까
        /*
        if (ps.playerStateNum == Constants.DEAD)
        {
            this.enabled = false;
        }
        */
        //캐릭터가 땅위에 있을때만 가능..
        //if (ps.isGrounded)
        //{
            /*ESC 따로
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gamePause = !gamePause;
                if (gamePause == true)
                {
                    Time.timeScale = 0.0f;
                    StopAllCoroutines();
                }
                else
                {
                    Time.timeScale = 1.0f;
                }
            }
            */

            //인벤토리 on/off
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
            {
                //Debug.Log("Screen.width : "+ Screen.width);
                InventoryOn = !InventoryOn;
                if (InventoryOn == true)
                {
                    NoneCursor();
                    //InventoryObj.transform.position = new Vector3((Screen.width - (Screen.width / 5)), InventoryObj.transform.position.y, InventoryObj.transform.position.z);

                    //----왜 이런 작업이 있어야하는가....(인벤토리 멀어짐 현상)
                    InventoryObj.transform.position = new Vector3((Screen.width - (Screen.width / 5)), InventoryObj.transform.position.y, 0.0f);
                    foreach (Slot item in Iv.slotScripts)
                    {
                        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 0.0f);
                    }
                    //--------------------------------------

                    InventoryObj.SetActive(true);
                    //옵션을 사용 중이라면 총알 발사 및 여러 행동 제한.
                    fireScript.enabled = false;
                    cameraControlScript.enabled = false;
                }
                else
                {
                    LockCursor();
                    InventoryObj.SetActive(false);
                    //옵션을 사용 중이라면 총알 발사 및 여러 행동 제한.
                    fireScript.enabled = true;
                    cameraControlScript.enabled = true;
                }
            }
        //}
    }
}
