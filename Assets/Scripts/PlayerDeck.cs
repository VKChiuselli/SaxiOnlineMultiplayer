using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDeck : NetworkBehaviour
{

    public List<GameObject> cards;
  //  public List<GameObject> decklist;
   // public List<GameObject> cardPoolList;
    public List<GameObject> cardInHand;
    public List<GameObject> playArea;
    public GameObject handZone; // da mettere privato



    public List<GameObject> GetCardInHand()
    {
        return cardInHand;
    }
    //creare funzione che quando viene loggato il giocatore, aggiunga le carte al gioco

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

            if (NetworkManager.Singleton.ConnectedClients.Count == 1)
            {
                handZone = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
                Debug.Log("handzone name: " + handZone.name);
              //  SpawnCardOnPanel();
            }
            else if (NetworkManager.Singleton.ConnectedClients.Count == 2)
            {
                handZone = GameObject.Find("CanvasHandPlayer/PanelPlayerLeft");
           //     SpawnCardOnPanel();
            }

         
        }


    }

    private void SpawnCardOnPanel()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject go = Instantiate(cards[i], handZone.transform);
            go.GetComponent<NetworkObject>().Spawn();
            cardInHand.Add(go);
        }
    }

    public void PlayCardFromHand(GameObject cardPlayed)
    {

        playArea.Add(cardPlayed);

        //   if (cardPlayed.GetComponent<Card>().hasEffect)
        //    cardPlayed.GetComponent<Card>().ActiveEffect(cardPlayed);

        cardInHand.Remove(cardPlayed);
    }

}
