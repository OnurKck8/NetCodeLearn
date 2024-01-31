using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetLobby : MonoBehaviour
{
    public GameObject buttonContainer;
    public GameObject buttonPrefab;

    async void Start()
    {
        await UnityServices.InitializeAsync();//Yazmazsan çalýþmaz
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void GetLobbiesTest()
    {
        ClearContainer();

        try
        {
            QueryLobbiesOptions options = new();
            Debug.LogWarning("QuarryLobiesTest");
            options.Count = 25;//Kaç adet lobi getirmek istiyorsun.

            //Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter
                (
                    field:QueryFilter.FieldOptions.AvailableSlots,
                    op:QueryFilter.OpOptions.GT,
                    value:"0"//0'dan büyük olan boþ lobileri getir
                )
            };

            /*
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter
                (
                    field:QueryFilter.FieldOptions.S1,
                    op:QueryFilter.OpOptions.EQ,
                    value:"Conquest"
                )
            };
            */

            //Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder
                (
                    asc:false,//Ýlk açýlan lobiler için "false", son açýlan lobiler için "true
                    field:QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            Debug.LogWarning("Get Lobbies Done Count:" + lobbies.Results.Count);

            foreach (Lobby bulunanLobby in lobbies.Results)
            {
                LobbyStatic.LogLobby(bulunanLobby);
                CreateLobbyButton(bulunanLobby);
            }
            //GetComponent<JoinLobby>().JoinLobbyWithLobbyId(lobbies.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void CreateLobbyButton(Lobby lobby)
    {
        var button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
        button.name = lobby.Name + "_Button";
        button.GetComponentInChildren<TextMeshProUGUI>().text = lobby.Name;
        var rectTransform = button.GetComponent<RectTransform>();
        rectTransform.SetParent(buttonContainer.transform);
        button.GetComponent<Button>().onClick.AddListener(delegate () { Lobby_OnClick(lobby); });
    }

    public void Lobby_OnClick(Lobby lobby)
    {
        Debug.Log("Clicked Lobby"+lobby.Name);
        GetComponent<JoinLobby>().JoinLobbyWithLobbyId(lobby.Id);
    }

    private void ClearContainer()
    {
        if(buttonContainer is not null && buttonContainer.transform.childCount>0)
        {
            foreach (Transform item in buttonContainer.transform)
            {
                Destroy(item.gameObject);
            }
        }
        
    }
}
