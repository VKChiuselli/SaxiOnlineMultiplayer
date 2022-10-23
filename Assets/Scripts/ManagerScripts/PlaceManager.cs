using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour {
    GameObject handCard;
    GameObject tabledCard;
    bool isCardSelected;

    void OnEnable() {
        EventsManager.current.onPickCardFromHand += PickedCard;
        EventsManager.current.onPickCardFromTable += PickedTableCard;
    }
    void OnDisable() {
        EventsManager.current.onPickCardFromHand -= PickedCard;
        EventsManager.current.onPickCardFromTable -= PickedTableCard;
    }

    public void PickedCard(GameObject cardToPlace) {
        if (tabledCard != null)
        {
            ResetCardTable();
        }
        handCard = cardToPlace;
        Debug.Log("Passing card to pick it from hand");
    }

    public void PickedTableCard(GameObject cardToPlace) {
        if (handCard != null)
        {
            ResetCardHand();
        }
        tabledCard = cardToPlace;
        Debug.Log("Passing card to pick it from table");
    }

    public GameObject GetCardSelectedFromHand() {
        return handCard;
    }
    public GameObject GetCardSelectedFromTable() {
        return tabledCard;
    }
    public void ResetCardHand() {
        handCard = null;
    }
    public void ResetCardTable() {
        tabledCard = null;
    }

}
