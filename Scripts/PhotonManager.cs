using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : Photon.MonoBehaviour
{
    const string redTeamPlayerPrefabName = "PlayerRed";
    const string blueTeamPlayerPrefabName = "PlayerBlue";
    public GameObject menuPos = null;
    //public List<string> PlayerListManager = new List<string>();
    public PhotonPlayer[] players = null;
    public GameObject[] playerObjs = null;
    public List<PlayersManager> playerListManager = new List<PlayersManager>();

    [PunRPC]
    void updatePlayerList() {
        players = PhotonNetwork.playerList;
    }

    [PunRPC]
    void updatePlayerObjs()
    {
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
    }
    
    void OnJoinedRoom()
    {
        Debug.Log("룸접속");
        CreatePlayer();
       
        //photonView.RPC("updatePlayerObjs", PhotonTargets.AllBuffered, null);
        //Camera.main.GetComponent<CameraControl>().enabled = true;
    }
    GameObject Player = null;

    void CreatePlayer()
    {
        //GameObject Player = PhotonNetwork.Instantiate(this.playerPrefabName, new Vector3(pos, 154.7f, pos), Quaternion.identity, 0);
        //캐릭 Spwan 관련

        //players = PhotonNetwork.playerList;
        //GameObject Player = null;

        //플레이어 위치------------------------------
        float posX = Random.Range(1500.0f, 1600.0f);
        float posY = 2.0f;
        //float posY = 150.0f;
        float posZ = Random.Range(2100.0f, 2200.0f);
        Vector3 playerPos = new Vector3(posX, posY, posZ);
        //---------------------------------------------

        if (PunTeams.PlayersPerTeam[PunTeams.Team.red].Count <= PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count)
        {
            Player = PhotonNetwork.Instantiate(redTeamPlayerPrefabName, playerPos, Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        }
        else //if (PunTeams.PlayersPerTeam[PunTeams.Team.red].Count > PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count)
        {
            Player = PhotonNetwork.Instantiate(blueTeamPlayerPrefabName, playerPos, Quaternion.identity, 0);
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
        //Player.GetComponent<NetworkCharacterMove>().enabled = true;
        Player.GetComponentInChildren<ItemDatabase>().enabled = true;
        Player.GetComponentInChildren<Inventory>().enabled = true;
        Player.GetComponentInChildren<SliderBarControl>().enabled = true;
        Player.GetComponentInChildren<CameraControl>().enabled = true;
        //Player.GetComponent<NetworkCharacterMove>().enabled = true;
        //Player.GetComponentInChildren<Rader>().enabled = true;
        //PlayerListManager.Add(PhotonNetwork.player);

        photonView.RPC("NoticePlayerInfo", PhotonTargets.AllBuffered, PhotonNetwork.player.UserId);
    }

    [PunRPC]
    void NoticePlayerInfo(string playerName)
    {
        //playerListManager.Add(new PlayersManager(playerName));
        Player.name = playerName;
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        //foreach (PlayersManager value in playerListManager)
        //{
        //    GUILayout.Label("playerName : " + value.playerName);
        //}
        if (players != null)
        {
            foreach (PhotonPlayer value in players)
            {
                GUILayout.Label("player UserId : " + value.UserId);
            }
        }
        
        GUILayout.Label("Red : " + PunTeams.PlayersPerTeam[PunTeams.Team.red].Count + " Blue : " + PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count + " 사람 수 : " + PhotonNetwork.countOfPlayersInRooms );
        if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room

        if (GUILayout.Button("Leave Room"))
        {
            // Mouse Lock
            Cursor.lockState = CursorLockMode.None;
            // Cursor visible
            Cursor.visible = true;
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
