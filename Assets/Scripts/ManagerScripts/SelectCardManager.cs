using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardManager : MonoBehaviour {
    GameObject selectCardFromTable;
    bool isCardSelected;

    void Start() {
        EventsManager.current.onSelectCardFromTable += SelectCard;
    }

    public void SelectCard(GameObject selectCard) { //TODO in future maybe we select more than one card
        selectCardFromTable = selectCard;
        Debug.Log("Passing selected card from table");
    }

    public GameObject GetSelectedCard() {
        return selectCardFromTable;
    }
    public void ResetSelectedCard() {
        selectCardFromTable = null;
    }

}
