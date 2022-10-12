using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {
    GameObject handCardSpell;
    bool isCardSelected;

    void Start() {
        EventsManager.current.onPickCardSpellFromHand += PickedSpellCard;
    }

    public void PickedSpellCard(GameObject cardSpellToPlace) {
        handCardSpell = cardSpellToPlace;
        Debug.Log("Passing cardSpell to pick it from hand");
    }

    public GameObject GetCardSpellSelectedFromHand() {
        return handCardSpell;
    }
    public void ResetCardSpellHand() {
        handCardSpell = null;
    }

}
