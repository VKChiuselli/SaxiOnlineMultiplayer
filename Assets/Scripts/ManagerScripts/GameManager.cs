using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject popupChoose;
     GameObject grid;
     GridContainer gridContainer;
    //  GameBoard gameBoard;
    //   PlaceManager placeManager;
    public NetworkVariable<int> IsUnmergeChoosing = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> IsPopupChoosing = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> CurrentTurn = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerZeroMP = new NetworkVariable<int>(3); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerZeroDP = new NetworkVariable<int>(2); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerOneMP = new NetworkVariable<int>(3); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerOneDP = new NetworkVariable<int>(2); //0 giocatore destra, 1 giocatore sinistra

    private void Start()
    {
        grid = GameObject.Find("CanvasHandPlayer/GridManager");
        gridContainer = grid.GetComponent<GridContainer>();
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
        IsPopupChoosing.Value = 0;

        ResetSpeedCards(CurrentTurn.Value);


        CurrentTurn.Value = CurrentTurn.Value == 1 ? 0 : 1; //inverto il turno
        //fare un trigger manager che guarda tutte le carte** e attiva i vari effetti (le carte dovranno avere un parametro TRIGGER che si eseguira una volta trovato e setacciato dal trigger manager
   
    }

    private void ResetSpeedCards(int player)
    {
        List<GameObject> playerCard = gridContainer.GetCardsFromPlayer(player);

        foreach (GameObject card in playerCard)
        {
            card.GetComponent<CardTable>().Speed.Value = 2 ; //TODO call a method that reset own speed
        }

        //TODO implement reset speed card, every card get his own card and reset it as default
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

    public int GetCurrentPlayerDeployPoint()
    {
        if (CurrentTurn.Value == 0)
        {
        return    PlayerZeroDP.Value  ;
        }
        else if (CurrentTurn.Value == 1)
        {
            return PlayerOneDP.Value ;
        }
        else
        {
            Debug.Log("ERROR!! Class gameManager, class GetCurrentPlayerDeployPoint. whichplayer is wrong!!");
            return 0;
        }
    }
    public int GetCurrentPlayerMovePoint()
    {
        if (CurrentTurn.Value == 0)
        {
        return    PlayerZeroMP.Value  ;
        }
        else if (CurrentTurn.Value == 1)
        {
            return PlayerOneMP.Value ;
        }
        else
        {
            Debug.Log("ERROR!! Class gameManager, class GetCurrentPlayerMovePoint. whichplayer is wrong!!");
            return 0;
        }
    }

    public void DeployPointSpent(int howMuchPoint)
    {
        if (CurrentTurn.Value == 0)
        {
            PlayerZeroDP.Value = PlayerZeroDP.Value - howMuchPoint;
        }
        else if (CurrentTurn.Value == 1)
        {
            PlayerOneDP.Value = PlayerOneDP.Value - howMuchPoint;
        }
        else
        {
            Debug.Log("ERROR!! Class gameManager, class DeployPointSpentServerRpc. whichplayer is wrong!!");
        }
    }

    public void DeployPointIncrease(int howMuchPoint)
    {
        if (CurrentTurn.Value == 0)
        {
            PlayerZeroDP.Value = PlayerZeroDP.Value + howMuchPoint;
        }
        else if (CurrentTurn.Value == 1)
        {
            PlayerOneDP.Value = PlayerOneDP.Value + howMuchPoint;
        }
        else
        {
            Debug.Log("ERROR!! Class gameManager, class DeployPointSpentServerRpc. whichplayer is wrong!!");
        }
    }

    public void MovePointSpent(int howMuchPoint)
    {

        if (CurrentTurn.Value == 0)
        {
            PlayerZeroMP.Value = PlayerZeroMP.Value - howMuchPoint;
        }else
        if (CurrentTurn.Value == 1)
        {
            PlayerOneMP.Value = PlayerOneMP.Value - howMuchPoint;
        }
        else
        {
            Debug.Log("ERROR!! Class gameManager, class DeployPointSpentServerRpc. whichplayer is wrong!!");
        }
    }

    public void MovePointIncrease(int howMuchPoint)
    {

        if (CurrentTurn.Value == 0)
        {
            PlayerZeroMP.Value = PlayerZeroMP.Value + howMuchPoint;
        }else
        if (CurrentTurn.Value == 1)
        {
            PlayerOneMP.Value = PlayerOneMP.Value + howMuchPoint;
        }
        else
        {
            Debug.Log("ERROR!! Class gameManager, class DeployPointSpentServerRpc. whichplayer is wrong!!");
        }
    }
 
}
