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
        await UnityServices.InitializeAsync();//Yazmazsan �al��maz
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void GetLobbiesTest()
    {
        try
        {
            QueryLobbiesOptions options = new();
            Debug.LogWarning("QuarryLobiesTest");
            options.Count = 25;//Ka� adet lobi getirmek istiyorsun.

            //Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter
                (
                    field:QueryFilter.FieldOptions.AvailableSlots,
                    op:QueryFilter.OpOptions.GT,
                    value:"0"//0'dan b�y�k olan bo� lobileri getir
                )
            };

            //Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder
                (
                    asc:false,//�lk a��lan lobiler i�in "false", son a��lan lobiler i�in "true
                    field:QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            Debug.LogWarning("Get Lobbies Done Count:" + lobbies.Results.Count);

            foreach (Lobby bulunanLobby in lobbies.Results)
            {
                Debug.Log("Lobby �smi:" + bulunanLobby.Name+"\n" + "Lobby Olusturlma Vakti:" + bulunanLobby.Created+
                    "Lobby Code:" + bulunanLobby.LobbyCode+
                    "Lobby ID:" + bulunanLobby.Id);
            }
            GetComponent<JoinLobby>().JoinLobbyWithLobbyId(lobbies.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
