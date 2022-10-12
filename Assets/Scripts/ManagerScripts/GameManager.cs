using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  //  GameBoard gameBoard;
 //   PlaceManager placeManager;

    void Start() {
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();
    //    gameBoard = FindObjectOfType<GameBoard>();
  //      placeManager = FindObjectOfType<PlaceManager>();
        TriggerManager.current.EnableDiscardManager = false;
    }

    public void EndTurn() {
        PlayerActions.current.HasMoved = true;
        PlayerActions.current.HasPlaced = true;
        TriggerManager.current.ResetTurn();
     //   gameBoard.ReadyTableCards();

        (PlayerActions.current.PlayerTop, PlayerActions.current.PlayerBot) = (PlayerActions.current.PlayerBot, PlayerActions.current.PlayerTop);
    }

   // void Update() {
      //  DisableEnablePlaceManagerKey();
 //   }

    //private void DisableEnablePlaceManagerKey() {
    //    if (Input.GetKeyDown(KeyCode.P)) {
    //        Debug.Log("Disattivo il place manager");
    //        placeManager.ResetCardTable();
    //        placeManager.ResetCardHand();
    //        placeManager.gameObject.SetActive(false);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.L)) {
    //        Debug.Log("Attivo il place manager");
    //        placeManager.ResetCardTable();
    //        placeManager.ResetCardHand();
    //        placeManager.gameObject.SetActive(true);
    //    }
    //}
}
