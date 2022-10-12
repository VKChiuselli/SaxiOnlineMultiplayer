using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardManager : MonoBehaviour {
    GameObject handCard;
    bool isCardSelected;

    void Start() {
        EventsManager.current.onDiscardCardFromHand += DiscardCard;
    }

    public void DiscardCard(GameObject cardToDiscard) {
        handCard = cardToDiscard;
        Debug.Log("Passing card to discard from hand");
    }


    public GameObject GetCardSelectedFromHand() {
        return handCard;
    }

    public void ResetCardHand() {
        handCard = null;
    }

}
