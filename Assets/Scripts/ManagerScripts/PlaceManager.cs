using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour {
    GameObject handCard;
    GameObject tableSingleCard;
    GameObject tableMergedCard;
    GameObject tableSelectCard;
    bool isCardSelected;

    void OnEnable() {
        EventsManager.current.onPickCardFromHand += PickedHandCard;
        EventsManager.current.onPickMergedCardFromTable += PickedMergedTableCard;
        EventsManager.current.onPickSingleCardFromTable += PickedSingleTableCard;
        EventsManager.current.onSelectCardFromTable += SelectCardFromTable;
    }
    void OnDisable() {
        EventsManager.current.onPickCardFromHand -= PickedHandCard;
        EventsManager.current.onPickMergedCardFromTable -= PickedMergedTableCard;
        EventsManager.current.onPickSingleCardFromTable -= PickedSingleTableCard;
        EventsManager.current.onSelectCardFromTable -= SelectCardFromTable;
    }

    public void PickedHandCard(GameObject cardToPlace) {
        if (tableSingleCard != null || tableMergedCard != null|| tableSelectCard != null  )
        {
            ResetSingleCardTable();
            ResetMergedCardTable();
            ResetTableSelectCard();
        }
        handCard = cardToPlace;
        Debug.Log("Passing card to pick it from hand");
    }


    public void PickedMergedTableCard(GameObject cardToPlace) {
        if (handCard != null)
        {
            ResetCardHand();
            ResetTableSelectCard();
        }
        tableMergedCard = cardToPlace;
        Debug.Log("Passing merged card to pick it from table");
    }

    public void PickedSingleTableCard(GameObject cardToPlace) {
        if (handCard != null)
        {
            ResetCardHand();
            ResetTableSelectCard();
        }
        tableSingleCard = cardToPlace;
        Debug.Log("Passing single card to pick it from table");
    }

    public void SelectCardFromTable(GameObject cardToSelect) {
        if (handCard != null)
        {
            ResetCardHand();
        }
        if (tableSingleCard != null || tableMergedCard != null)
        {
            ResetSingleCardTable();
            ResetMergedCardTable();
        }
        tableSelectCard = cardToSelect;
        Debug.Log("Passing selected card to use it from table");
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
    public CardInterface GetCardSelectedFromTable() {
        //placeManager.GetCardSelectedFromTable().transform.GetChild(8).GetComponent<CardInterface>();
        if (tableSelectCard.GetComponent<CardTable>()!=null)
        {
            return tableSelectCard.transform.GetChild(2).GetComponent<CardInterface>();
        }
        else
        {
            return tableSelectCard.transform.GetChild(8).GetComponent<CardInterface>(); ;
        }
    }

    public void ResetCardHand() {
        handCard = null;
    }

    public void ResetMergedCardTable() {
        tableMergedCard = null;
    }
    public void ResetTableSelectCard() {
        tableSelectCard = null;
    }
    public void ResetSingleCardTable() {
        tableSingleCard = null;
    }

}
