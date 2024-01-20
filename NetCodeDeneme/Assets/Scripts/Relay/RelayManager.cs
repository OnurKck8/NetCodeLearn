using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using TMPro;
using System;
using Unity.Services.Relay.Models;

public class RelayManager : MonoBehaviour
{
    private string playerID;
    private RelayHostData data;
    public TextMeshProUGUI IdText;
    public TMP_Dropdown playerCount;
    async void Start()
    {
        await UnityServices.InitializeAsync();//Yazmazsan çalýþmaz
        Debug.Log("Unity Service Inýt");
        SignIn();
    }

    async void SignIn()
    {
        Debug.Log("Signing In");
        await AuthenticationService.Instance.SignInAnonymouslyAsync();//Anonim Giriþ.
        playerID = AuthenticationService.Instance.PlayerId;
        Debug.Log("Sign In" + playerID);
        IdText.text = playerID;
    }

    public async void OnHostClick()
    {
        int maxPLayerCount = Convert.ToInt32(playerCount.options[playerCount.value].text);
        Allocation allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(maxPLayerCount);
        data = new RelayHostData()
        {
            Ipv4Address = allocation.RelayServer.IpV4,
            port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.ConnectionData,
            Key = allocation.Key,
        };

        Debug.Log("Allocation Complete" + data.AllocationID);
    }
}

public struct RelayHostData
{
    public string JoinCode;
    public string Ipv4Address;
    public ushort port;
    public Guid AllocationID;
    public byte[] AllocationIDBytes;
    public byte[] ConnectionData;
    public byte[] Key;
}
