using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies;
using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class JoinLobby : MonoBehaviour
{
    public TMP_InputField inputField;

    public async void JoinLobbyWithLobbyCode(string lobbyCode)
    {
        var code = inputField.text;
        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
            options.Player = new Player(AuthenticationService.Instance.PlayerId);
            options.Player.Data = new Dictionary<string, PlayerDataObject>()
            {
              {"PLayerLevel",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,"8")}
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code,options);
            Debug.Log("Joined Lobby With Code:" + code);

            DontDestroyOnLoad(this); 
            GetComponent<CurrentLobby>().currentLobby = lobby;

            LobbyStatic.LogPlayersInLobby(lobby);
            LobbyStatic.LoadLobbyRoom();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyWithLobbyId(string lobbyId)
    {
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions();
            options.Player = new Player(AuthenticationService.Instance.PlayerId);
            options.Player.Data = new Dictionary<string, PlayerDataObject>()
            {
              {"PLayerLevel",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,"8")}
            };

            Lobby lobby=await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId,options);

            DontDestroyOnLoad(this);
            GetComponent<CurrentLobby>().currentLobby = lobby;

            Debug.Log("Joined Lobby With ID:" + lobbyId);
            Debug.LogWarning("Lobby Code: " + lobby.LobbyCode);
            LobbyStatic.LogPlayersInLobby(lobby);
            LobbyStatic.LoadLobbyRoom();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoinMethod()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            DontDestroyOnLoad(this);
            GetComponent<CurrentLobby>().currentLobby = lobby;

            Debug.Log("Joined Lobby With Quick Join:" + lobby.Id);
            Debug.LogWarning("Lobby Code: " + lobby.LobbyCode);
            LobbyStatic.LoadLobbyRoom();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
