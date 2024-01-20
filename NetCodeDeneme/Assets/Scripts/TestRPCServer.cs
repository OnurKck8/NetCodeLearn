using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestRPCServer : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            TextServerRpc(0);
    }

    [ClientRpc]
    void TextClientRpc(int value)
    {
        if(IsClient)
        {
            Debug.Log("Client Received the RPC" + value);
            TextServerRpc(value + 1);
        }
    }

    [ServerRpc]
    void TextServerRpc(int value)
    {
        Debug.Log("Server Received the RPC" + value);
        TextClientRpc(value);
    }
}
