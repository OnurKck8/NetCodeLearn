using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GetLobby : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();//Yazmazsan çalýþmaz
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void GetLobbiesTest()
    {
        try
        {
            QueryLobbiesOptions options = new();
            Debug.LogWarning("QuarryLobiesTest");
            options.Count = 25;

            //Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter
                (
                    field:QueryFilter.FieldOptions.AvailableSlots,
                    op:QueryFilter.OpOptions.GT,
                    value:"0"
                )
            };

            //Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder
                (
                    asc:false,
                    field:QueryOrder.FieldOptions.AvailableSlots)
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            Debug.LogWarning("Get Lobbies Done Count:" + lobbies.Results.Count);

            foreach (Lobby bulunanLobby in lobbies.Results)
            {
                Debug.Log("Lobby Ýsmi:" + bulunanLobby.Name+"\n" + "Lobby Olusturlma Vakti:" + bulunanLobby.Created);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
