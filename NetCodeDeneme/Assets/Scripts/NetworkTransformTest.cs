using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkTransformTest : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner && IsServer)
            transform.RotateAround(GetComponentInParent<Transform>().position, Vector3.up, 100f*Time.deltaTime);
    }
}
