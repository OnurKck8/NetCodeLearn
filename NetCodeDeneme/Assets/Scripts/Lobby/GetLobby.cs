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

    public async void GetLobbiesTest()
    {
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

            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter
                (
                    field:QueryFilter.FieldOptions.S1,
                    op:QueryFilter.OpOptions.EQ,
                    value:"Conquest"
                )
            };

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
            }
            GetComponent<JoinLobby>().JoinLobbyWithLobbyId(lobbies.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
