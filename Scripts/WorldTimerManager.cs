using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Timers;

public class WorldTimerManager : MonoBehaviour
{

    public static WorldTimerManager instance;
    private PhotonView pv = null;
    public int worldTimer;
    private int millisecondsElapsed;

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
        pv = GetComponent<PhotonView>();
        previousTime = DateTime.Now;
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
    }

    private void Update()
    {
        var now = DateTime.Now;
        var elapsed = now - previousTime;
        previousTime = now;
        var msec = (int)elapsed.TotalMilliseconds;
        updateFrame(msec);
        Debug.Log(worldTimer);
    }
}
