using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviour {
    public string playerPrefabName = "Player2";
    
    void OnJoinedRoom()
    {
        Debug.Log("룸접속");
        CreatePlayer();
    }

    void CreatePlayer()
    {
        //다시 카메라 원점으로 돌림
        Camera.main.farClipPlane = 1000;

        float pos = Random.Range(-100.0f, 100.0f);
        GameObject Player = PhotonNetwork.Instantiate(this.playerPrefabName, new Vector3(pos, 1, pos), Quaternion.identity, 0);
        CharacterMove PlayerMove = Player.GetComponent<CharacterMove>();
        PlayerMove.enabled = true;
        OptionManager PlayerOptionMG = Player.GetComponent<OptionManager>();
        PlayerOptionMG.enabled = true;
        FireScript PlayerFs = Player.GetComponent<FireScript>();
        PlayerFs.enabled = true;
        CheckCollider PlayerCC = Player.GetComponent<CheckCollider>();
        PlayerCC.enabled = true;
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room

        if (GUILayout.Button("Leave Room"))
        {
            Camera.main.GetComponent<CameraControl>().enabled = false;
            PhotonNetwork.LeaveRoom();
            Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
        }
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }
}
