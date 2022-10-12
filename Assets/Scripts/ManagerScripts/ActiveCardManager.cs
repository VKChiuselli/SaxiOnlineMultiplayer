using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCardManager : MonoBehaviour {
    GameObject activeTabledCard;
    bool isCardSelected;

    void OnEnable() {
        EventsManager.current.onActiveCardFromTable += ActiveTableCard;
    }
    void OnDisable() {
        EventsManager.current.onActiveCardFromTable -= ActiveTableCard;
    }

    public void ActiveTableCard(GameObject cardToPlace) {
        activeTabledCard = cardToPlace;
        Debug.Log("card activated from table");
    }

    public GameObject GetActiveCardSelectedFromTable() {
        return activeTabledCard;
    }
  
    public void ResetActiveCardTable() {
        activeTabledCard = null;
    }

}
