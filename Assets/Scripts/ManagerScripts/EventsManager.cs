using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EventsManager : MonoBehaviour {
    public static EventsManager current;
    void Awake() {
        int numGameSessions = FindObjectsOfType<EventsManager>().Length;
        if (numGameSessions > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
        current = this;
    }
     
    public event Action<GameObject> onPickCardFromHand;

    public void PickCardFromHand(GameObject cardPicked) {
        if (NetworkManager.Singleton.IsClient)
        {
            if (onPickCardFromHand != null)
            {
                onPickCardFromHand(cardPicked);
            }
        
        }
     
    }  
    public event Action<GameObject> onPickCardSpellFromHand;

    public void PickCardSpellFromHand(GameObject cardPicked) {
        if (onPickCardSpellFromHand != null)
            onPickCardSpellFromHand(cardPicked);
    }
    public event Action<GameObject> onPickSingleCardFromTable;

    public void PickSingleCardFromTable(GameObject PickSingleCardFromTable) {
        if (onPickSingleCardFromTable != null)
            onPickSingleCardFromTable(PickSingleCardFromTable);
    }

    public event Action<GameObject> onPickMergedCardFromTable;
    public void PickMergedCardFromTable(GameObject PickMergedCardFromTable) {
        if (onPickMergedCardFromTable != null)
            onPickMergedCardFromTable(PickMergedCardFromTable);
    }

    public event Action<GameObject> onActiveCardFromTable;
    public void ActiveCardFromTable(GameObject cardPicked) {
        if (onActiveCardFromTable != null)
            onActiveCardFromTable(cardPicked);
    }   
    
    public event Action<GameObject> onDiscardCardFromHand;
    public void DiscardCardFromHand(GameObject cardToDiscard) {
        if (onDiscardCardFromHand != null)
            onDiscardCardFromHand(cardToDiscard);
    }
    public event Action<GameObject> onSelectCardFromTable;
    public void SelectCardFromTable(GameObject cardSelected) {
        if (onSelectCardFromTable != null)
            onSelectCardFromTable(cardSelected);
    }
    public event Action<GameObject> onSelectCard;
    public void SelectCard(GameObject selectCard) {
        onSelectCard?.Invoke(selectCard);
    }
}
