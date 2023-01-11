using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnCard : NetworkBehaviour
{
    [SerializeField] GameObject whereLoadCardsRightPlayer;
    [SerializeField] GameObject whereLoadCardsLeftPlayer;


    public void StartGame()
    {
        //if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        //{
        //    LoadCards("PanelPlayerRight", "RPCH");
        //}
        LoadCards("PanelPlayerRight", "RPCH");
    }


    public void LoadCards(string panelName, string tagName)
    {
       
      //  GameObject serverHand = GameObject.Find($"CoreGame/CanvasHandPlayer/{panelName}/Dog(Clone)");
      //  if (serverHand == null)
       // {
            GameObject cardToSpawn = Resources.Load("PrefabToLoad\\Cards\\Dog", typeof(GameObject)) as GameObject;
            GameObject Traitor = Resources.Load("PrefabToLoad\\Cards\\Traitor", typeof(GameObject)) as GameObject;
            GameObject DragonIce = Resources.Load("PrefabToLoad\\Cards\\DragonIce", typeof(GameObject)) as GameObject;
            GameObject Ent = Resources.Load("PrefabToLoad\\Cards\\Ent", typeof(GameObject)) as GameObject;
            GameObject serverHand = GameObject.Find($"CoreGame/CanvasHandPlayer/{panelName}");
            GameObject a = Instantiate(cardToSpawn, serverHand.transform);
            GameObject b = Instantiate(Traitor, serverHand.transform);
            GameObject c = Instantiate(DragonIce, serverHand.transform);
            GameObject d = Instantiate(Ent, serverHand.transform);
            a.tag = tagName;
            b.tag = tagName;
            c.tag = tagName;
            d.tag = tagName;
        serverHand.GetComponent<DeckLoad>().LoadCards();
        Debug.Log("LoadCards");
            //  LoadCardsServerRpc(panelName, tagName);
     //   }
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoadCardsServerRpc(string panelName, string tagName)
    {

        GameObject cardToSpawn = Resources.Load("PrefabToLoad\\Cards\\Dog", typeof(GameObject)) as GameObject;
        GameObject serverHand = GameObject.Find($"CoreGame/CanvasHandPlayer/{panelName}");
        GameObject a = Instantiate(cardToSpawn, serverHand.transform);
        a.tag = tagName;
        Debug.Log("LoadCardsServerRpc");
        //   serverHand.GetComponent<DeckLoad>().LoadCards();
    }

}
