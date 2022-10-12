using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverCardManager : MonoBehaviour {

    GameObject overCard;
    bool isCardSelected;

    void Start() {
        EventsManager.current.onOverCard += OveredCard;
    }

    public void OveredCard(GameObject overedCard) { //TODO in future maybe we select more than one card
        overCard = overedCard;
        Debug.Log("Card overed");
    }

    public GameObject GetOveredCard() {
        return overCard;
    }

}
