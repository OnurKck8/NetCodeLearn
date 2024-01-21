using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using TMPro;
using System;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;

public class RelayManager : MonoBehaviour
{
    private string playerID;

    private RelayHostData hostData;
    private RelayJoinData joinData;

    public TMP_InputField InputField;
    public TextMeshProUGUI IdText;
    public TextMeshProUGUI joinCodeText;
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
        hostData = new RelayHostData()
        {
            Ipv4Address = allocation.RelayServer.IpV4,
            port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.ConnectionData,
            Key = allocation.Key,
        };
        hostData.JoinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(hostData.AllocationID);
        Debug.Log("Allocation Complete" + hostData.AllocationID);

        Debug.LogWarning("Join Code:" + hostData.JoinCode);
        joinCodeText.text = hostData.JoinCode;

        UnityTransport unityTransport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
        unityTransport.SetRelayServerData(hostData.Ipv4Address, hostData.port, hostData.AllocationIDBytes, hostData.Key, hostData.ConnectionData);
        NetworkManager.Singleton.StartHost();
    }

    public async void OnJoinClick()
    {
        JoinAllocation joinAllocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(InputField.text);

        joinData = new RelayJoinData()
        {
            Ipv4Address = joinAllocation.RelayServer.IpV4,
            port = (ushort)joinAllocation.RelayServer.Port,
            AllocationID = joinAllocation.AllocationId,
            AllocationIDBytes = joinAllocation.ConnectionData,
            ConnectionData=joinAllocation.ConnectionData,
            HostConnectionData=joinAllocation.HostConnectionData,
            Key = joinAllocation.Key,
        };
        Debug.Log("Join Success" + joinData.AllocationID);
        UnityTransport unityTransport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
        unityTransport.SetRelayServerData(joinData.Ipv4Address, joinData.port,joinData.AllocationIDBytes, joinData.Key, joinData.ConnectionData,joinData.HostConnectionData);
        NetworkManager.Singleton.StartClient();
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

public struct RelayJoinData
{  
    public string Ipv4Address;
    public ushort port;
    public Guid AllocationID;
    public byte[] AllocationIDBytes;
    public byte[] ConnectionData;
    public byte[] HostConnectionData;
    public byte[] Key;
}
