using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour {

    public static TriggerManager current;

    PlaceManager placeManager;
    DiscardManager discardManager;
    ActiveCardManager activeCardManager;
    SpellManager spellManager;
    SelectCardManager selectCardManager;

    public bool EnablePlaceManager { get; set; }
    public bool EnableDiscardManager { get; set; }
    public bool EnableActiveCardManager { get; set; }
    public bool EnableActiveSpellManager { get; set; }
    public bool EnableSelectCardManager { get; set; }


    void Start() {

        placeManager = FindObjectOfType<PlaceManager>();
        discardManager = FindObjectOfType<DiscardManager>();
        activeCardManager = FindObjectOfType<ActiveCardManager>();
        spellManager = FindObjectOfType<SpellManager>();
        selectCardManager = FindObjectOfType<SelectCardManager>();
        EnablePlaceManager = true;
        EnableDiscardManager = true;
        EnableActiveCardManager = true;
        EnableActiveSpellManager = true;
        EnableSelectCardManager = true;
    }

    public void ResetTurn() {
        EnablePlaceManager = true;
        EnableDiscardManager = false;
        EnableActiveCardManager = true;
        EnableActiveSpellManager = true;
        EnableSelectCardManager = true;
        placeManager.ResetCardHand();
        placeManager.ResetCardTable();
        discardManager.ResetCardHand();
        spellManager.ResetCardSpellHand();
        activeCardManager.ResetActiveCardTable();
        selectCardManager.ResetSelectedCard();
    }

}

//public void EnablePlaceManager() {
//    placeManager.gameObject.SetActive(true);
//}

//public void EnableDiscardManager() {
//    discardManager.gameObject.SetActive(true);
//}

//public void EnableActiveCardManager() {
//    activeCardManager.gameObject.SetActive(true);
//}

//placeManager = FindObjectOfType<PlaceManager>();
//discardManager = FindObjectOfType<DiscardManager>();
//activeCardManager = FindObjectOfType<ActiveCardManager>();
//PlayerActions.current = FindObjectOfType<PlayerActions>();