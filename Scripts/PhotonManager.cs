using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : Photon.MonoBehaviour
{
    public static PhotonManager instance;

    private const string redTeamPlayerPrefabName = "PlayerRed";
    private const string blueTeamPlayerPrefabName = "PlayerBlue";

    public List<GameObject> playerObjList = new List<GameObject>();
    public PhotonView pv = null;

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
    }

    void OnJoinedRoom()
    {
        //Debug.Log("룸접속");
        CreatePlayer();
    }

    void CreatePlayer()
    {
        //캐릭 Spwan 관련

        GameObject Player = null;

        //플레이어 위치------------------------------
        float posX = Random.Range(520.0f, 560.0f);
        //float posY = 65.0f;
        float posY = 160.0f;
        float posZ = Random.Range(540.0f, 600.0f);
        Vector3 playerPos = new Vector3(posX, posY, posZ);

        float RposX = Random.Range(555.0f, 600.0f);
        float RposY = 160.0f;
        float RposZ = Random.Range(580.0f, 600.0f);

        float BposX = Random.Range(850.0f, 900.0f);
        float BposY = 60.0f;
        float BposZ = Random.Range(1300.0f, 1400.0f);

        Vector3 redPlayerPos = new Vector3(RposX, RposY, RposZ);
        Vector3 bluePlayerPos = new Vector3(BposX, BposY, BposZ);
        //---------------------------------------------

        if (PunTeams.PlayersPerTeam[PunTeams.Team.red].Count <= PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count)
        {
            //Player = PhotonNetwork.Instantiate(redTeamPlayerPrefabName, playerPos, Quaternion.identity, 0);
            Player = PhotonNetwork.Instantiate(redTeamPlayerPrefabName, playerPos, Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        }
        else //if (PunTeams.PlayersPerTeam[PunTeams.Team.red].Count > PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count)
        {
            //Player = PhotonNetwork.Instantiate(blueTeamPlayerPrefabName, playerPos, Quaternion.identity, 0);
            Player = PhotonNetwork.Instantiate(blueTeamPlayerPrefabName, playerPos, Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
        }
        //메인카메라 관련
        Camera.main.farClipPlane = 100.0f;
        Camera.main.transform.SetParent(Player.transform);
        Transform camPivotTr = Player.transform.Find("CamPivot").gameObject.transform;
        Camera.main.transform.position = new Vector3(camPivotTr.position.x, camPivotTr.position.y, camPivotTr.position.z);
        Camera.main.transform.eulerAngles = new Vector3(camPivotTr.eulerAngles.x, camPivotTr.eulerAngles.y, camPivotTr.eulerAngles.z);
        //Camera.main.transform.rotation = new Quaternion(camPivotTr.rotation.x, camPivotTr.rotation.y, camPivotTr.rotation.z, camPivotTr.rotation.w);

        //플레이어 스크립트 관련
        //Player.transform.Find("Camera").gameObject.SetActive(true);
        Player.GetComponent<CharacterMove>().enabled = true;
        Player.GetComponent<OptionManager>().enabled = true;
        Player.GetComponent<FireScript>().enabled = true;
        Player.GetComponent<CheckCollider>().enabled = true;
        Player.GetComponent<PlayerState>().enabled = true;
        Player.GetComponentInChildren<ItemDatabase>().enabled = true;
        //Player.GetComponentInChildren<CameraControl>().enabled = true;
        //Player.GetComponentInChildren<ScopeUiControl>().enabled = true;
        //Player.GetComponentInChildren<Rader>().enabled = true;

        //photonView.RPC("SetConnectplayerObjList", PhotonTargets.AllBufferedViaServer, null);
        pv.RPC("SetConnectplayerObjList", PhotonTargets.AllBufferedViaServer, null);
    }

    [PunRPC]
    void SetConnectplayerObjList()
    {
        playerObjList.Clear();
        playerObjList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        foreach (GameObject player in playerObjList)
        {
            //Debug.Log(player.GetComponent<PhotonView>().owner);
            player.name = player.GetComponent<PhotonView>().viewID.ToString();
        }
        
    }

    public void LeavePlayer(int veiwId) {
        pv.RPC("SetDisConnectplayerObjList", PhotonTargets.AllBufferedViaServer, veiwId);
    }

    [PunRPC]
    void SetDisConnectplayerObjList(int veiwId)
    {
        //if (playerObjList.Count > 1)
        //{
        //    foreach (GameObject playerObj in playerObjList)
        //    {
        //        if (playerObj.GetComponent<PhotonView>().viewID == veiwId)
        //        {
        //            Debug.Log("플레이어 삭제 실행 : " + playerObj.GetComponent<PhotonView>().viewID);
        //            playerObjList.Remove(playerObj);
        //            //playerObjList.RemoveAt(playerObjList.IndexOf(playerObj));
        //            break;
        //        }
        //    }
        //}
        //else
        //{
        //    playerObjList.Clear();
        //}
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        //GUILayout.Label("Red : " + PunTeams.PlayersPerTeam[PunTeams.Team.red].Count + " Blue : " + PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count + " 사람 수 : " + PhotonNetwork.countOfPlayersInRooms );
        if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room
        GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 200, 1000));

        //foreach (GameObject playerObj in playerObjList)
        //{
        //    //GUILayout.Label("플레이어 이름 : " + playerObj.name);
        //    GUILayout.Label("플레이어 이름 : " + playerObj.GetComponent<PhotonView>().viewID);
        //}
        
        if (WorldTimerManager.instance.isGameStart)
        {
            GUILayout.Label("게임 시작!!");
        }
        else
        {
            GUILayout.Label("게임 대기중!!");
        }
        GUILayout.EndArea();
    }

    void OnDisconnectedFromPhoton()
    {
        //Leave Room 클릭 시 오브젝트리스트 삭제 및 포톤네트워크 상 캐릭 삭제
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }
}
