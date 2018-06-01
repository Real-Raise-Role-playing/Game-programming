using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager
{
    public GameObject playerObj { get; set; }
    public string playerName { get; set; }
    //public int playerCount { get; set; }

    public PlayersManager(string _playerName)
    {
        playerName = _playerName;
        //playerCount++;
    }

    public PlayersManager()
    {
        playerObj = null;
        playerName = string.Empty;
        //playerCount = 0;
    }
}
