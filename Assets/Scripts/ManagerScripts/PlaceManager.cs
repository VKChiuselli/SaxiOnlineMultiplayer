using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour {
    GameObject handCard;
    GameObject tableSingleCard;
    GameObject tableMergedCard;
    bool isCardSelected;

    void OnEnable() {
        EventsManager.current.onPickCardFromHand += PickedHandCard;
        EventsManager.current.onPickMergedCardFromTable += PickedMergedTableCard;
        EventsManager.current.onPickSingleCardFromTable += PickedSingleTableCard;
    }
    void OnDisable() {
        EventsManager.current.onPickCardFromHand -= PickedHandCard;
        EventsManager.current.onPickMergedCardFromTable -= PickedMergedTableCard;
        EventsManager.current.onPickSingleCardFromTable -= PickedSingleTableCard;
    }

    public void PickedHandCard(GameObject cardToPlace) {
        if (tableSingleCard != null || tableMergedCard != null  )
        {
            ResetSingleCardTable();
            ResetMergedCardTable();
        }
        handCard = cardToPlace;
        Debug.Log("Passing card to pick it from hand");
    }


    public void PickedMergedTableCard(GameObject cardToPlace) {
        if (handCard != null)
        {
            ResetCardHand();
        }
        tableMergedCard = cardToPlace;
        Debug.Log("Passing merged card to pick it from table");
    }

    public void PickedSingleTableCard(GameObject cardToPlace) {
        if (handCard != null)
        {
            ResetCardHand();
        }
        tableSingleCard = cardToPlace;
        Debug.Log("Passing single card to pick it from table");
    }

    public GameObject GetCardSelectedFromHand() {
        return handCard;
    }
 
    public GameObject GetSingleCardSelectedFromTable() {
        return tableSingleCard;
    }
    public GameObject GetMergedCardSelectedFromTable() {
        return tableMergedCard;
    }

    public void ResetCardHand() {
        handCard = null;
    }

    public void ResetMergedCardTable() {
        tableMergedCard = null;
    }
    public void ResetSingleCardTable() {
        tableSingleCard = null;
    }

}
