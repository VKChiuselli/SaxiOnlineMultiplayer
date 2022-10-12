using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{

    public List<GameObject> cards;
    public List<GameObject> decklist;
    public List<GameObject> cardPoolList;
    public List<GameObject> cardInHand;
    public List<GameObject> playArea;
    public GameObject handZone;

    public List<GameObject> GetCardInHand()
    {
        return cardInHand;
    }


    void Awake()
    {


        for (int i = 0; i < cards.Count; i++)
        {

            cardInHand.Add(GameObject.Instantiate(cards[i], handZone.transform));

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
