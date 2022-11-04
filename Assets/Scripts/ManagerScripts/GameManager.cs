using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject popupChoose;
    //  GameBoard gameBoard;
    //   PlaceManager placeManager;
    public NetworkVariable<int> IsUnmergeChoosing = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> IsPopupChoosing = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
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
    {//TODO reset cards picked
        EndTurnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void EndTurnServerRpc()
    {

        if (CurrentTurn.Value == 0)
        {
            PlayerZeroMP.Value = 3;
            PlayerZeroDP.Value = 2;
        }
        else if (CurrentTurn.Value == 1)
        {
            PlayerOneMP.Value = 3;
            PlayerOneDP.Value = 2;
        }
        IsUnmergeChoosing.Value = 0;

        CurrentTurn.Value = CurrentTurn.Value == 1 ? 0 : 1; //inverto il turno
        //fare un trigger manager che guarda tutte le carte** e attiva i vari effetti (le carte dovranno avere un parametro TRIGGER che si eseguira una volta trovato e setacciato dal trigger manager
    }

    public void SetIsPopupChoosing(int IsPopupChoosing)
    {
        SetIsPopupChoosingServerRpc(IsPopupChoosing);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetIsPopupChoosingServerRpc(int isPopupChoosing)
    {
        IsPopupChoosing.Value = isPopupChoosing;
    }


    public void OpenPopupUI(int xOldTile, int yOldTile, int xNewTile, int yNewTile, int typeOfTile)
    {
        if (xOldTile == xNewTile && yOldTile == yNewTile)
        {
            Debug.Log("Popup can't open, trying to open same tile");
            return;
        }
        popupChoose.SetActive(true);
        popupChoose.GetComponent<PopupUI>().InitializeVariables(xOldTile, yOldTile, xNewTile, yNewTile, typeOfTile);
    }

    public void SetUnmergeChoosing(int unmergeStatus)
    {
        SetUnmergeChoosingServerRpc(unmergeStatus);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetUnmergeChoosingServerRpc(int unmergeStatus)
    {
        IsUnmergeChoosing.Value = unmergeStatus;
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

    public void MovePointSpent(int howMuchPoint)
    {

        MovePointSpentServerRpc(howMuchPoint, CurrentTurn.Value);
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