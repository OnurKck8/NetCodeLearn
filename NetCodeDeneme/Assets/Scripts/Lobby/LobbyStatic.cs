using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public static class LobbyStatic
{
    public static void LogPlayersInLobby(Lobby lobby)
    {
        foreach (Player player in lobby.Players)
        {
            Debug.Log("Plyers ID =" + player.Id);
            Debug.Log("Plyers Level =" + player.Data["PLayerLevel"].Value);
        }
    }
}
