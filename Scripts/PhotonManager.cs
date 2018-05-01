using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviour
{
    public bool leaveCheck = false;
    public string playerPrefabName = "US_Marine";
    public GameObject menuPos = null;
    int teamNumView = 0;
    int ptNumView = 0;
    void Start()
    {
    }
    void OnJoinedRoom()
    {
        Debug.Log("룸접속");
        CreatePlayer();
        Camera.main.GetComponent<CameraControl>().enabled = true;
    }

    void CreatePlayer()
    {
        //캐릭 Spwan 관련
        float pos = Random.Range(700.0f, 800.0f);
        GameObject Player = PhotonNetwork.Instantiate(this.playerPrefabName, new Vector3(pos, 154.7f, pos), Quaternion.identity, 0);
        //메인카메라 관련
        Camera.main.farClipPlane = 1000.0f;
        Camera.main.transform.SetParent(Player.transform);
        GameObject camPivot = Player.transform.Find("CamPivot").gameObject;
        Camera.main.transform.position = new Vector3(camPivot.transform.position.x, camPivot.transform.position.y, camPivot.transform.position.z);
        Camera.main.transform.rotation = new Quaternion(camPivot.transform.rotation.x, camPivot.transform.rotation.y, camPivot.transform.rotation.z, camPivot.transform.rotation.w);

        //플레이어 스크립트 관련
        CharacterMove PlayerMove = Player.GetComponent<CharacterMove>();
        PlayerMove.enabled = true;
        OptionManager PlayerOptionMG = Player.GetComponent<OptionManager>();
        PlayerOptionMG.enabled = true;
        FireScript PlayerFs = Player.GetComponent<FireScript>();
        PlayerFs.enabled = true;
        CheckCollider PlayerCC = Player.GetComponent<CheckCollider>();
        PlayerCC.enabled = true;
        PlayerState playerState = Player.GetComponent<PlayerState>();
        playerState.enabled = true;
        PhotonView pv = Player.GetComponent<PhotonView>();
        Debug.Log("플레이어 ID : " + pv.viewID);
        ptNumView = pv.viewID;
        playerState.TeamNum = (pv.viewID / 1000) % 2 == 0 ? 2 : 1;
        teamNumView = playerState.TeamNum;
        Rader PlayerRader = Player.GetComponentInChildren<Rader>();
        PlayerRader.enabled = true;
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        GUILayout.Label("Team Num : " + teamNumView + " PhotonView ID : " + ptNumView);
        if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room

        if (GUILayout.Button("Leave Room"))
        {
            leaveCheck = true;
            Camera.main.transform.SetParent(menuPos.transform);
            Camera.main.GetComponent<CameraControl>().enabled = false;
            Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
            Camera.main.transform.position = Vector3.zero;
            Camera.main.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            PhotonNetwork.LeaveRoom();
        }
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }
}
