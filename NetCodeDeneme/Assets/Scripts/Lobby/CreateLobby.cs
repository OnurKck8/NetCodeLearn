using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Lobbies;
using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class CreateLobby : MonoBehaviour
{
    public TMP_InputField lobbyname;
    public TMP_InputField lobbycode;
    public TMP_Dropdown maxplayers;
    public TMP_Dropdown gamemode;
    public Toggle isLobbyPrivate;

    public async void CreateLobbyMethod()
    {
        string lobbyName = lobbyname.text;
        int maxPlayers = Convert.ToInt32(maxplayers.options[maxplayers.value].text);
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = isLobbyPrivate.isOn;

        //Player Creation
        options.Player = new Player(AuthenticationService.Instance.PlayerId);
        options.Player.Data = new Dictionary<string, PlayerDataObject>()
        {
            {"PLayerLevel",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,"5")}
        };

        //Lobby Data
        options.Data = new Dictionary<string, DataObject>()
        {
            {"GameMode",new DataObject(DataObject.VisibilityOptions.Public,gamemode.options[gamemode.value].text,DataObject.IndexOptions.S1)}
        };
        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        DontDestroyOnLoad(this);
        Debug.Log("Create Lobby done!");
        LobbyStatic.LogLobby(lobby);
        LobbyStatic.LogPlayersInLobby(lobby);
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
