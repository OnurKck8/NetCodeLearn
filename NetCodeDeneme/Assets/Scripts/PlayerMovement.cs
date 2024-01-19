using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if(IsOwner)//Sahibi miyiz?
        {
            Move();
        }
    }

    void Update()
    {
        transform.position = Position.Value;
    }

    public void Move()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            var randompos=GetRandomPosOnPlane();
            transform.position = randompos;
            Position.Value = randompos;
        }
        else
        {
            SubmitPosRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitPosRequestServerRpc(ServerRpcParams serverRpcParams=default)//Rpc yazman lazým
    {
        Position.Value = GetRandomPosOnPlane();
    }

    static Vector3 GetRandomPosOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }
}
