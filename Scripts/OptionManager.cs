using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OptionManager : MonoBehaviour
{
    //public static bool gameOptionOn = false;
    public GameObject InventoryObj = null;
    CharacterController ParentCC = null;


    FireScript fireScript = null;
    CharaterMove characterMoveScrpt = null;
    CameraControl cameraControlScript = null;


    void Start()
    {
        ParentCC = GetComponent<CharacterController>();
        fireScript = GetComponent<FireScript>();
        characterMoveScrpt = GetComponent<CharaterMove>();
        cameraControlScript = GetComponentInChildren<CameraControl>();
    }
    private bool InventoryOn = false;
    void Update()
    {
        //캐릭터가 땅위에 있을때만 가능..
        if (ParentCC.isGrounded == true)
        {
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
            //아이템 줍기 시전
            //if (Input.GetKey(KeyCode.F))
            //{
            //    Inventory.instance.
            //}

            if (Input.GetKeyDown(KeyCode.I))
            {
                InventoryOn = !InventoryOn;
                if (InventoryOn == true)
                {
                    InventoryObj.SetActive(true);
                    //옵션을 사용 중이라면 총알 발사 및 여러 행동 제한.
                    fireScript.enabled = false;
                    characterMoveScrpt.enabled = false;
                    cameraControlScript.enabled = false;
                    //Time.timeScale = 0.0f;
                }
                else
                {
                    InventoryObj.SetActive(false);
                    //옵션을 사용 중이라면 총알 발사 및 여러 행동 제한.
                    fireScript.enabled = true;
                    characterMoveScrpt.enabled = true;
                    cameraControlScript.enabled = true;
                    //Time.timeScale = 1.0f;
                }
            }
        }
    }
}
