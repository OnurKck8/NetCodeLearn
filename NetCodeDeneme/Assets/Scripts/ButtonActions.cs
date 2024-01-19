using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ButtonActions : MonoBehaviour
{
    private NetworkManager networkManager;

    void Start()
    {
        networkManager = GetComponentInParent<NetworkManager>();
    }

    public void StartClient()
    {
        networkManager.StartClient();
    }

    public void StartHost()
    {
        networkManager.StartHost();
    }
}
