using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnCard : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition = new Vector3(1f, 1f, 1f);
       // GetComponent < NetworkTransform > = new Vector3(1f, 1f, 1f);
    }

}
