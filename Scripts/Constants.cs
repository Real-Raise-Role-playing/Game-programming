using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상수화 해놓을 수.
public static class Constants
{
    //moveSpeed 관련 상수
    public const float DefaultMoveSpeed = 15.0f;
    public const float MaxMoveSpeed = 20.0f;
    public const float AddMoveSpeed = 0.8f;
    public const float SitMoveSpeed = 6.0f; // 앉았을시 캐릭터의 속도

    //Jump & 캐릭터 Y축 관련 상수
    public const float Default_yVelocity = 0.0f;
    public const float jumpCountMax = 1.0f;
    public const float Default_gravity = -25.0f;
    public const float jumpSpeed = 7.0f;

    //FireScript 관련 상수
    public const float forwardPower = 300.0f;
    public const float upPower = 5.0f;
    public const float m16FireSpeed = 0.2f; 
    public const float meleeAttackSpeed = 2.0f;
    public const float kar98FireSpeed = 1.0f;
    public const int   m16MaxBulletCount = 30;
    public const int   m16InitBulletCount = 30;

    //Inventory 관련 상수
    public const int maxInventoryCount = 25;
    //public const int startItemCount = 3;
    public const int equipmentMaxCount = 6; //현재 모델링 된 장비 몇개인지
    
    //PlayerState 상태 관련 상수
    public const int initHp = 100;
    public const int NONE = 0;
    public const int GROGGY = 1;
    public const int DEAD = 2;

    //WorldTime 관련 상수
    public const int Stage_1 = 30;
    public const int Stage_2 = 60;
    public const int Stage_3 = 120;

    //CameraControler
    public const float defaultSensitivity = 350.0f;

}
