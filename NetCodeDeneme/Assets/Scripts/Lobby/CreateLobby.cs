using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Lobbies;
using System;
using Unity.Services.Lobbies.Models;

public class CreateLobby : MonoBehaviour
{
    public TMP_InputField lobbyname;
    public TMP_InputField lobbycode;
    public TMP_Dropdown maxplayers;
    public Toggle isLobbyPrivate;

    public async void CreateLobbyMethod()
    {
        string lobbyName = lobbyname.text;
        int maxPlayers = Convert.ToInt32(maxplayers.options[maxplayers.value].text);
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = isLobbyPrivate.isOn;

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        DontDestroyOnLoad(this);
        Debug.Log("Create Lobby done!");
        lobbycode.text = lobby.LobbyCode;
        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15f));
    }

    //Kapanmamasý için lobiyi pingliyoruz.
    IEnumerator HeartbeatLobbyCoroutine(string lobbyId,float waitTimeSeconds)
    {
        var delay = new WaitForSeconds(waitTimeSeconds);
        while(true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}
