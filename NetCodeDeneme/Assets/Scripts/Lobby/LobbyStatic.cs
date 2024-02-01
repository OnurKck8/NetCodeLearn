using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public static void LogLobby(Lobby lobby)
    {
        Debug.Log("Lobby ID =" + lobby.Id + "\n" + "GameMode=" + lobby.Data["GameMode"].Value);
    }

    public static void LoadLobbyRoom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
