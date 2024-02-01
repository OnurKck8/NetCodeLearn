using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopulateUI : MonoBehaviour
{
    public TextMeshProUGUI lobbyCode;
    public TextMeshProUGUI lobbyName;
    public TextMeshProUGUI gameMode;

    private CurrentLobby _currentLobby;

    void Start()
    {
        _currentLobby = GameObject.Find("LobbyManager").GetComponent<CurrentLobby>();
        PopulateUIElements();
        LobbyStatic.LogPlayersInLobby(_currentLobby.currentLobby);
    }

   void PopulateUIElements()
    {
        lobbyCode.text = _currentLobby.currentLobby.LobbyCode;
        lobbyName.text = _currentLobby.currentLobby.Name;
        gameMode.text = _currentLobby.currentLobby.Data["GameMode"].Value;
    }
}
