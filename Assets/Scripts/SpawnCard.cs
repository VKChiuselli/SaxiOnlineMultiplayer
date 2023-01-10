using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnCard : NetworkBehaviour
{
    [SerializeField] GameObject whereLoadCardsRightPlayer;
    [SerializeField] GameObject whereLoadCardsLeftPlayer;


    public void LoadCards()
    {
        if (IsServer || IsHost)
        {
            LoadCardsLocals(); 
        }
        else if (IsClient)
        {
            LoadCardsServerRpc();
        }
        else
        {
            Debug.Log("LoadCards method is broken");
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void LoadCardsServerRpc()
    {
        LoadCardsLocals();
    }


    private void LoadCardsLocals()
    {
        GameObject instance = Instantiate(Resources.Load("Hatchet", typeof(GameObject))) as GameObject;
    }

    //public override void OnNetworkSpawn()
    //{
    //    base.OnNetworkSpawn();
    //    transform.localScale = new Vector3(1f, 1f, 1f);
    //    transform.localPosition = new Vector3(1f, 1f, 1f);
    //   // GetComponent < NetworkTransform > = new Vector3(1f, 1f, 1f);
    //}

}
