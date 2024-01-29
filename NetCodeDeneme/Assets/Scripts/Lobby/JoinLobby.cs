using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies;
using System;
using Unity.Services.Lobbies.Models;

public class JoinLobby : MonoBehaviour
{
    public TMP_InputField inputField;

    public async void JoinLobbyWithLobbyCode(string lobbyCode)
    {
        var code = inputField.text;
        try
        {
            await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            Debug.Log("Joined Lobby With Code:" + code);
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
            Lobby lobby=await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyId);
            Debug.Log("Joined Lobby With Code:" + lobbyId);
            Debug.LogWarning("Lobby Code: " + lobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
