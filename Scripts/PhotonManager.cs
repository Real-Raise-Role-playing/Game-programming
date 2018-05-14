using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : Photon.MonoBehaviour
{
    public bool leaveCheck = false;
    string redTeamPlayerPrefabName = "PlayerRed";
    string blueTeamPlayerPrefabName = "PlayerBlue";
    public GameObject menuPos = null;
    int ptNumView = 0;

    void OnJoinedRoom()
    {
        Debug.Log("룸접속");
        CreatePlayer();
        Camera.main.GetComponent<CameraControl>().enabled = true;
    }

    void CreatePlayer()
    {
        GameObject Player = null;
        //GameObject Player = PhotonNetwork.Instantiate(this.playerPrefabName, new Vector3(pos, 154.7f, pos), Quaternion.identity, 0);
        //캐릭 Spwan 관련
        float posX = Random.Range(1600.0f, 1700.0f);
        float posZ = Random.Range(2000.0f, 2100.0f);

        if (PunTeams.PlayersPerTeam[PunTeams.Team.red].Count <= PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count)
        {
            Player = PhotonNetwork.Instantiate(redTeamPlayerPrefabName, new Vector3(posX, 150.0f, posZ), Quaternion.identity, 0);
            //Player = PhotonNetwork.Instantiate(redTeamPlayerPrefabName, new Vector3(posX, 154.7f, posZ), Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        }
        else //if (PunTeams.PlayersPerTeam[PunTeams.Team.red].Count > PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count)
        {
            Player = PhotonNetwork.Instantiate(blueTeamPlayerPrefabName, new Vector3(posX, 150.0f, posZ), Quaternion.identity, 0);
            //Player = PhotonNetwork.Instantiate(blueTeamPlayerPrefabName, new Vector3(posX, 154.7f, posZ), Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
        }
        Player.name = PhotonNetwork.playerName;
        //메인카메라 관련
        Camera.main.farClipPlane = 1000.0f;
        Camera.main.transform.SetParent(Player.transform);
        GameObject camPivot = Player.transform.Find("CamPivot").gameObject;
        Camera.main.transform.position = new Vector3(camPivot.transform.position.x, camPivot.transform.position.y, camPivot.transform.position.z);
        Camera.main.transform.rotation = new Quaternion(camPivot.transform.rotation.x, camPivot.transform.rotation.y, camPivot.transform.rotation.z, camPivot.transform.rotation.w);

        //플레이어 스크립트 관련
        Player.GetComponent<CharacterMove>().enabled = true;
        Player.GetComponent<OptionManager>().enabled = true;
        Player.GetComponent<FireScript>().enabled = true;
        Player.GetComponent<CheckCollider>().enabled = true;
        Player.GetComponent<PlayerState>().enabled = true;
        Player.GetComponentInChildren<ItemDatabase>().enabled = true;
        Player.GetComponentInChildren<Inventory>().enabled = true;
        Player.GetComponentInChildren<SliderBarControl>().enabled = true;
        //Player.GetComponent<NetworkCharacterMove>().enabled = true;
        //Player.GetComponentInChildren<Rader>().enabled = true;
        PhotonView pv = Player.GetComponent<PhotonView>();
        //pv.RPC("TaggedPlayer",PhotonTargets.All, playerState.UserId);
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        GUILayout.Label("Name : " + PhotonNetwork.playerName);
        GUILayout.Label("Red : " + PunTeams.PlayersPerTeam[PunTeams.Team.red].Count + " Blue : "+ PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count);
        if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room

        if (GUILayout.Button("Leave Room"))
        {
            // Mouse Lock
            Cursor.lockState = CursorLockMode.None;
            // Cursor visible
            Cursor.visible = true;
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
