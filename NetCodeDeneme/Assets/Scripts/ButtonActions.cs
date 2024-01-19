using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class ButtonActions : MonoBehaviour
{
    private NetworkManager networkManager;
    public TextMeshProUGUI text;
    void Start()
    {
        networkManager = GetComponentInParent<NetworkManager>();
    }

    public void StartClient()
    {
        networkManager.StartClient();
        In�tMovementText();
    }

    public void StartHost()
    {
        networkManager.StartHost();
        In�tMovementText();
    }

    public void SubmitNewPsoition()
    {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        var player = playerObject.GetComponent<PlayerMovement>();
        player.Move();
    }

    private void In�tMovementText()
    {
        if(NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)//Server m�? veya Kurucumuyuz
        {
            text.text = "Move";
        }
        else if(NetworkManager.Singleton.IsClient)
        {
            text.text = "Request Move";

        }
    }
}
