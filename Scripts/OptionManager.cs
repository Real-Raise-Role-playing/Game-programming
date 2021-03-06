﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionManager : MonoBehaviour
{
    //public static bool gameOptionOn = false;
    public GameObject InventoryObj = null;
    ScopeUiControl suc = null;
    PlayerState ps = null;
    FireScript fireScript = null;
    PhotonView pv = null;

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
        InventoryObj = GameObject.Find("Inventory");
        pv = GetComponent<PhotonView>();
        ps = GetComponent<PlayerState>();
        fireScript = GetComponent<FireScript>();
        suc = GetComponentInChildren<ScopeUiControl>();
        //cameraControlScript = GetComponentInChildren<CameraControl>();
    }

    void Update()
    {
        if (!pv.isMine || ps.playerStateNum == Constants.DEAD) { return; }

        //인벤토리 on/off
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Screen.width : "+ Screen.width + "   Screen.height : " + Screen.height);
            InventoryOn = !InventoryOn;
            if (InventoryOn == true)
            {
                NoneCursor();
                InventoryObj.transform.position = new Vector3((Screen.width - (Screen.width / 5)), InventoryObj.transform.position.y, 0.0f);
                InventoryObj.SetActive(true);
                //옵션을 사용 중이라면 총알 발사 및 여러 행동 제한.
                fireScript.enabled = false;
                suc.enabled = false;
            }
            else
            {
                LockCursor();
                InventoryObj.SetActive(false);
                //옵션을 사용 중이라면 총알 발사 및 여러 행동 제한.
                fireScript.enabled = true;
                suc.enabled = true;
            }
        }
        //}
    }
}
