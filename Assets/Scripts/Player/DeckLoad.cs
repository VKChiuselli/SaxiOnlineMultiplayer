using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class DeckLoad : NetworkBehaviour
{

    private GameObject CardOne;
    private GameObject CardTwo;
    private GameObject CardThree;
    private GameObject CardFour;
 private NetworkManager netti;

    public  override void OnNetworkSpawn()
    {//TODO improve network 
        transform.localScale = new Vector3(1f, 1f, 1f);
    
  //      GetComponent<SpawnCard>().LoadCards();
      

       
    

        //gameObject.transform.GetChild(0).gameObject.GetComponent<CardHand>().CardPosition.Value = 0;
        //gameObject.transform.GetChild(1).gameObject.GetComponent<CardHand>().CardPosition.Value = 1;
        //gameObject.transform.GetChild(2).gameObject.GetComponent<CardHand>().CardPosition.Value = 2;
        //gameObject.transform.GetChild(3).gameObject.GetComponent<CardHand>().CardPosition.Value = 3;

        //if (gameObject.transform.GetChild(1).gameObject != null)
        //{
        //    CardTwo = gameObject.transform.GetChild(1).gameObject.transform.GetChild(8).gameObject;
        //}
        //if (gameObject.transform.GetChild(2).gameObject != null)
        //{
        //    CardThree = gameObject.transform.GetChild(2).gameObject.transform.GetChild(8).gameObject;
        //}
        //if (gameObject.transform.GetChild(3).gameObject != null)
        //{
        //    CardFour = gameObject.transform.GetChild(3).gameObject.transform.GetChild(8).gameObject;
        //}
    }

    public   void LoadCards()
    {
        CardOne = gameObject.transform.GetChild(0).gameObject.transform.GetChild(8).gameObject;
        netti = FindObjectOfType<NetworkManager>();
        netti.AddNetworkPrefab(CardOne);

        //if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        //{
        //    LoadCardsLocals();

       
        //    //     CardTwo = gameObject.transform.GetChild(1).gameObject.transform.GetChild(8).gameObject;
        //    //     CardThree = gameObject.transform.GetChild(2).gameObject.transform.GetChild(8).gameObject;
        //    //     CardFour = gameObject.transform.GetChild(3).gameObject.transform.GetChild(8).gameObject;
        //    //   netti.AddNetworkPrefab(CardTwo);
        //    //   netti.AddNetworkPrefab(CardThree);
        //    //   netti.AddNetworkPrefab(CardFour);
        //    Debug.Log("LoadCards method is server loading111");
       
        //}
        //else if (NetworkManager.Singleton.IsClient)
        //{ 
        //}
        //else
        //{
        //    Debug.Log("LoadCards method is broken");
        //}
         
    } 

    private void LoadCardsLocals()
    {

        GameObject cardToSpawn =  Resources.Load("PrefabToLoad\\Cards\\Dog", typeof(GameObject)) as GameObject;

        NetworkObject cardToSpawnNetwork = Instantiate(cardToSpawn.GetComponent<NetworkObject>(),
       transform.position, Quaternion.identity);
        cardToSpawnNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        cardToSpawnNetwork.transform.SetParent(transform, false);
        Debug.Log("LoadCardsLocals");
    }

    public GameObject GetCardGameObject(int IdCard)
    {

        GameObject cardHand = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<CardHand>() != null)
            {
                if (transform.GetChild(i).gameObject.GetComponent<CardHand>().IdCard.Value == IdCard)
                {
                    cardHand = transform.GetChild(i).gameObject;
                    return cardHand;
                }
            }
        }

        Debug.Log("ERRRRORRR no card found from hand");

        return cardHand;
    }


    public CardHand GetCardHand(int IdCard)
    {
        CardHand cardHand = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<CardHand>() != null)
            {
                if (transform.GetChild(i).gameObject.GetComponent<CardHand>().IdCard.Value == IdCard)
                {
                    cardHand = transform.GetChild(i).gameObject.GetComponent<CardHand>();
                    return cardHand;
                }
            }
        }

        Debug.Log("ERRRRORRR no card found from hand");

        return cardHand;
    }


    public GameObject GetIndexCard(int index)
    {
        if (index == 0)
        {
            return CardOne;
        }
        if (index == 1)
        {
            return CardTwo;
        }
        if (index == 2)
        {
            return CardThree;
        }
        if (index == 3)
        {
            return CardFour;
        }
   
        Debug.Log("DeckLoad, method GetCard: No card found! Error!!");
        return null;
    }

}
