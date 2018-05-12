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
    public const float forwardPower = 80.0f;
    public const float upPower = 5.0f;
    public const float m16FireSpeed = 0.2f;
    public const float kar98FireSpeed = 1.0f;

    //Inventory 관련 상수
    public const int startItemCount = 3;

    //PlayerState 상태 관련 상수
    public const int initHp = 100;
    public const int NONE = 0;
    public const int GROGGY = 1;
    public const int DEAD = 2;
}
