using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnCard : NetworkBehaviour
{
    [SerializeField] GameObject whereLoadCardsRightPlayer;
    [SerializeField] GameObject whereLoadCardsLeftPlayer;


     void LoadCards()
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
        if (whereLoadCardsRightPlayer != null)
        {
            GameObject cardToSpawn = Instantiate(Resources.Load("PrefabToLoad\\Cards\\Dog", typeof(GameObject))) as GameObject;

            NetworkObject cardToSpawnNetwork = Instantiate(cardToSpawn.GetComponent<NetworkObject>(),
           transform.position, Quaternion.identity);
           cardToSpawnNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
         cardToSpawnNetwork.transform.SetParent(whereLoadCardsRightPlayer.transform, false);

            Debug.Log("ARRIVO");
        }
       
    }

    //public override void OnNetworkSpawn()
    //{
    //    base.OnNetworkSpawn();
    //    transform.localScale = new Vector3(1f, 1f, 1f);
    //    transform.localPosition = new Vector3(1f, 1f, 1f);
    //   // GetComponent < NetworkTransform > = new Vector3(1f, 1f, 1f);
    //}

}
