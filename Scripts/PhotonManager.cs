using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : Photon.MonoBehaviour
{
    const string redTeamPlayerPrefabName = "PlayerRed";
    const string blueTeamPlayerPrefabName = "PlayerBlue";
    //public List<string> PlayerListManager = new List<string>();
    public PhotonPlayer[] players = null;
    public GameObject[] playerObjs = null;
    public List<PlayersManager> playerListManager = new List<PlayersManager>();
    
    void OnJoinedRoom()
    {
        Debug.Log("룸접속");
        CreatePlayer();
    }
    GameObject Player = null;

    void CreatePlayer()
    {
        //GameObject Player = PhotonNetwork.Instantiate(this.playerPrefabName, new Vector3(pos, 154.7f, pos), Quaternion.identity, 0);
        //캐릭 Spwan 관련

        //players = PhotonNetwork.playerList;
        //GameObject Player = null;

        //플레이어 위치------------------------------
        float posX = Random.Range(1350.0f, 1400.0f);
        float posY = 65.0f;
        //float posY = 150.0f;
        float posZ = Random.Range(2300.0f, 2350.0f);
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
        //메인카메라 관련
        Camera.main.farClipPlane = 1000.0f;
        Camera.main.transform.SetParent(Player.transform);
        Transform camPivotTr = Player.transform.Find("CamPivot").gameObject.transform;
        Camera.main.transform.position = new Vector3(camPivotTr.position.x, camPivotTr.position.y, camPivotTr.position.z);
        Camera.main.transform.rotation = new Quaternion(camPivotTr.rotation.x, camPivotTr.rotation.y, camPivotTr.rotation.z, camPivotTr.rotation.w);

        //플레이어 스크립트 관련
        Player.GetComponent<CharacterMove>().enabled = true;
        Player.GetComponent<OptionManager>().enabled = true;
        Player.GetComponent<FireScript>().enabled = true;
        Player.GetComponent<CheckCollider>().enabled = true;
        Player.GetComponent<PlayerState>().enabled = true;
        //Player.GetComponent<NetworkCharacterMove>().enabled = true;
        Player.GetComponentInChildren<ItemDatabase>().enabled = true;
        //Player.GetComponentInChildren<Inventory>().enabled = true;
        Player.GetComponentInChildren<SliderBarControl>().enabled = true;
        Player.GetComponentInChildren<CameraControl>().enabled = true;
        Player.GetComponentInChildren<ScopeUiControl>().enabled = true;
        //Player.GetComponent<NetworkCharacterMove>().enabled = true;
        //Player.GetComponentInChildren<Rader>().enabled = true;
        //PlayerListManager.Add(PhotonNetwork.player);

        photonView.RPC("SetConnectPlayerList", PhotonTargets.AllBuffered, null);
    }
    [PunRPC]
    void SetConnectPlayerList()
    {

        //모든 Player 프리팹 저장
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playerObjs)
        {
            //Debug.Log(player.GetComponent<PhotonView>().viewID);
            player.name = player.GetComponent<PhotonView>().viewID.ToString();
        }
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        //GUILayout.Label("Red : " + PunTeams.PlayersPerTeam[PunTeams.Team.red].Count + " Blue : " + PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count + " 사람 수 : " + PhotonNetwork.countOfPlayersInRooms );
        if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.Log("누구 연결 끊어짐" + PhotonNetwork.player);
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }
}
