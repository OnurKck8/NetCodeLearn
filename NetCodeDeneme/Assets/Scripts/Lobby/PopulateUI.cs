using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

public class PopulateUI : MonoBehaviour
{
    public TextMeshProUGUI lobbyCode;
    public TextMeshProUGUI lobbyName;
    public TextMeshProUGUI gameMode;

    public GameObject playerInfoContainer;
    public GameObject playerInfoPrefab;

    private CurrentLobby _currentLobby;

    public string lobbyId;
    void Start()
    {
        _currentLobby = GameObject.Find("LobbyManager").GetComponent<CurrentLobby>();
        PopulateUIElements();
        lobbyId = _currentLobby.currentLobby.Id;
        InvokeRepeating("PollForLobbyUpdate", 1.1f, 2f);
        LobbyStatic.LogPlayersInLobby(_currentLobby.currentLobby);
    }

    void PopulateUIElements()
    {
        lobbyCode.text = _currentLobby.currentLobby.LobbyCode;
        lobbyName.text = _currentLobby.currentLobby.Name;
        gameMode.text = _currentLobby.currentLobby.Data["GameMode"].Value;
        ClearContainer();
        foreach ( Player player in _currentLobby.currentLobby.Players)
        {
            CreatePlayerInfoCard(player);
        }
    }

    void CreatePlayerInfoCard(Player player)
    {
        var text = Instantiate(playerInfoPrefab, Vector3.zero, Quaternion.identity);
        text.name = player.Joined.ToShortTimeString();
        text.GetComponent<TextMeshProUGUI>().text = player.Id + "" + player.Data["PLayerLevel"].Value;
        var rectTransform = text.GetComponent<RectTransform>();
        rectTransform.SetParent(playerInfoContainer.transform);
    }

    private void ClearContainer()
    {
        if (playerInfoContainer is not null && playerInfoContainer.transform.childCount > 0)
        {
            foreach (Transform item in playerInfoContainer.transform)
            {
                Destroy(item.gameObject);
            }
        }

    }

    async void PollForLobbyUpdate()
    {
        _currentLobby.currentLobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
        PopulateUIElements();
    }
}
