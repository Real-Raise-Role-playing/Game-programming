using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Timers;

public class WorldTimerManager : MonoBehaviour
{

    public static WorldTimerManager instance;
    public int worldTimer;
    private int millisecondsElapsed;
    public bool isGameStart;
    DateTime now;
    DateTime previousTime;

    // Use this for initialization
    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        //previousTime = DateTime.Now;
        isGameStart = false;
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, 0, 200, 30));
        GUILayout.Label("서버 시간 흐름 : " + worldTimer);
        GUILayout.EndArea();
    }

    public void updateFrame(int msec)
    {
        millisecondsElapsed += msec;
        worldTimer = (int)(millisecondsElapsed / 1000);

        //Time.deltaTime을 이용하여 동기화 할때 그러나 값이 일정하지 않아 잠시 정지
        //worldTimer = (int)(millisecondsElapsed * (Time.deltaTime * 0.05));
    }

    void Update()
    {
        //게임 시작시 필요한 처리
        if (PhotonManager.instance.playerObjList.Count >= 2 && !isGameStart)
        {
            isGameStart = true;
            previousTime = DateTime.Now;
            //Slime.instance.GetComponent<Slime>().enabled = true;
        }
        else if (PhotonManager.instance.playerObjList.Count < 1 && isGameStart)
        {
            //Slime.instance.GetComponent<Slime>().enabled = false;
            isGameStart = false;
        }
        if (isGameStart)
        {
            var now = DateTime.Now;
            var elapsed = now - previousTime;
            previousTime = now;
            var msec = (int)elapsed.TotalMilliseconds;
            updateFrame(msec);
        }
        else
        {
            return;
        }

        //var now = DateTime.Now;
        //var elapsed = now - previousTime;
        //previousTime = now;
        //var msec = (int)elapsed.TotalMilliseconds;
        //updateFrame(msec);
    }
}
