using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public static bool gameOptionOn = false;
    //private bool gamePause = false;
    public GameObject Inventory = null;
    CharacterController ParentCC = null;
    void Start()
    {
        ParentCC = GetComponentInParent<CharacterController>();
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
                    gameOptionOn = true;
                }
                else
                {
                    Time.timeScale = 1.0f;
                    gameOptionOn = false;
                }
            }
            */
            if (Input.GetKeyDown(KeyCode.I))
            {
                InventoryOn = !InventoryOn;
                if (InventoryOn == true)
                {
                    Inventory.SetActive(true);
                    Time.timeScale = 0.0f;
                    gameOptionOn = true;
                }
                else
                {
                    Inventory.SetActive(false);
                    Time.timeScale = 1.0f;
                    gameOptionOn = false;
                }
            }
        }
    }
}
