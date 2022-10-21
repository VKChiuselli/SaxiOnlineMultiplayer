using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    //  GameBoard gameBoard;
    //   PlaceManager placeManager;
    public NetworkVariable<int> CurrentTurn = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerZeroMP = new NetworkVariable<int>(3); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerZeroDP = new NetworkVariable<int>(2); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerOneMP = new NetworkVariable<int>(3); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerOneDP = new NetworkVariable<int>(2); //0 giocatore destra, 1 giocatore sinistra

    void Start()
    {
        //    PlayerActions.current = FindObjectOfType<PlayerActions>();
        //  TriggerManager.current = FindObjectOfType<TriggerManager>();
        //    gameBoard = FindObjectOfType<GameBoard>();
        //      placeManager = FindObjectOfType<PlaceManager>();
        //TriggerManager.current.EnableDiscardManager = false;
    }

    public void EndTurn()
    {
        CurrentTurn.Value = CurrentTurn.Value == 1 ? 0 : 1;


        PlayerActions.current.HasMoved = true;
        PlayerActions.current.HasPlaced = true;
        TriggerManager.current.ResetTurn();
        //   gameBoard.ReadyTableCards();

        (PlayerActions.current.PlayerTop, PlayerActions.current.PlayerBot) = (PlayerActions.current.PlayerBot, PlayerActions.current.PlayerTop);
    }

    public void DeployPointSpent(int howMuchPoint, int whichPlayer)
    {
        DeployPointSpentServerRpc(howMuchPoint, whichPlayer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeployPointSpentServerRpc(int howMuchPoint, int whichPlayer)
    {
        if (whichPlayer == 0)
        {
            PlayerZeroDP.Value = PlayerZeroDP.Value - howMuchPoint;
        }
        else if (whichPlayer == 1)
        {
            PlayerOneDP.Value = PlayerOneDP.Value - howMuchPoint;
        }
        else
        {
            Debug.Log("ERROR!! Class gameManager, class DeployPointSpentServerRpc. whichplayer is wrong!!");
        }
    }
    
    public void MovePointSpent(int howMuchPoint, int whichPlayer)
    {
        MovePointSpentServerRpc(howMuchPoint, whichPlayer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void MovePointSpentServerRpc(int howMuchPoint, int whichPlayer)
    {
        if (whichPlayer == 0)
        {
            PlayerZeroMP.Value = PlayerZeroMP.Value - howMuchPoint;
        }
        else if (whichPlayer == 1)
        {
            PlayerOneMP.Value = PlayerOneMP.Value - howMuchPoint;
        }
        else
        {
            Debug.Log("ERROR!! Class gameManager, class DeployPointSpentServerRpc. whichplayer is wrong!!");
        }
    }
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