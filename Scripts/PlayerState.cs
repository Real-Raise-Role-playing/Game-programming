using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {
    //Player 생존 확인
    public bool isDead = false;
    public GameObject Gun = null; //총도 숨기기위해서
    /*
    public void DamageByEnemy()
    {
        if (isDead)
        {
            return;
        }
        --healthPoint;
        if (healthPoint <= 0)
        {
            isDead = true;
            Gun.SetActive(false);
        }
    }
    */
}
