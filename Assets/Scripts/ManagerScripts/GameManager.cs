using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject popupChoose;
    [SerializeField] GameObject endTurnButton;
    GameObject grid;
    GridContainer gridContainer;
    //TestLobby testLobby;
    GameObject deckManagerRight;
    GameObject deckManagerLeft;

    //  GameBoard gameBoard;
    //   PlaceManager placeManager;
    public NetworkVariable<int> IsUnmergeChoosing = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> IsPopupChoosing = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra

    public bool IsRunningPlayer()
    {
        if(IsServer || IsHost)
        {
            if (CurrentTurn.Value == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if(IsClient)
        {
            if (CurrentTurn.Value == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
         
        return false;
    }

    public NetworkVariable<int> IsPickingChoosing = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> CurrentTurn = new NetworkVariable<int>(0); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerZeroMP = new NetworkVariable<int>(3); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerZeroDP = new NetworkVariable<int>(2); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerOneMP = new NetworkVariable<int>(3); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<int> PlayerOneDP = new NetworkVariable<int>(2); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<FixedString32Bytes> PlayerZero = new NetworkVariable<FixedString32Bytes>("ciao"); //0 giocatore destra, 1 giocatore sinistra
    public NetworkVariable<FixedString32Bytes> PlayerOne = new NetworkVariable<FixedString32Bytes>("ciao"); //0 giocatore destra, 1 giocatore sinistra

    private void Start()
    {
        endTurnButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            EndTurn();
        });

        endTurnButton.SetActive(false);
        grid = GameObject.Find("CanvasHandPlayer/GridManager");
        gridContainer = grid.GetComponent<GridContainer>();
   //    testLobby = grid.GetComponent<TestLobby>();
        deckManagerRight = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
        deckManagerLeft = GameObject.Find("CanvasHandPlayer/PanelPlayerLeft");
    }

    //server call this
    public void SetPlayerID()
    {
        if (IsServer || IsHost)
        {
            SetPlayerIDServer();
        }
        else  if(IsClient)
        {
            SetPlayerIDServerRpc();
        }
    }

    //client call this
    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerIDServerRpc()
    {
        SetPlayerIDClient();
    }

    public void SetPlayerIDServer()
    {
        PlayerZero.Value = AuthenticationService.Instance.PlayerId;
        Debug.Log("  PlayerZero.Value id:  " + AuthenticationService.Instance.PlayerId);
    }


    public void SetPlayerIDClient()
    {
        PlayerOne.Value = AuthenticationService.Instance.PlayerId;
        Debug.Log("  PlayerOne.Value id:  " + AuthenticationService.Instance.PlayerId);
    }




    private void Update()
    {
            if (IsRunningPlayer())
            {
                endTurnButton.SetActive(true);
            }
            else
            {
                endTurnButton.SetActive(false);
            }
    }


 public   void EndTurn()
    {
        if (IsClient)
        {
            EndTurnServerRpc();
        }
        else if (IsHost || IsServer)
        {
            EndTurnLocal();
        }
        else
        {
            Debug.Log("method wrong is: EndTun");
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void EndTurnServerRpc()
    {
        EndTurnLocal();
    }

    public void EndTurnLocal()
    {//TODO reset cards picked
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

        Debug.Log("endofturn");
        CurrentTurn.Value = CurrentTurn.Value == 1 ? 0 : 1; //inverto il turno
                                                            //fare un trigger manager che guarda tutte le carte** e attiva i vari effetti (le carte dovranno avere un parametro TRIGGER che si eseguira una volta trovato e setacciato dal trigger manager
    }

    public void AddCardCopyOnHand(int idCard, int quantity)
    {
        if (CurrentTurn.Value == 0)
        {
            CardHand card = deckManagerRight.GetComponent<DeckLoad>().GetCardHand(idCard);
            card.Copies.Value = card.Copies.Value + quantity;
        }
        else if (CurrentTurn.Value == 1)
        {
            CardHand card = deckManagerLeft.GetComponent<DeckLoad>().GetCardHand(idCard);
            card.Copies.Value = card.Copies.Value + quantity;
        }
    }


    private void ResetSpeedCards(int player)
    {
        List<GameObject> playerCard = gridContainer.GetCardsFromPlayer(player);

        foreach (GameObject card in playerCard)
        {
            card.GetComponent<CardTable>().CurrentSpeed.Value = card.GetComponent<CardTable>().BaseSpeed.Value; //TODO call a method that reset own speed
        }

        //TODO implement reset speed card, every card get his own card and reset it as default
    }



    #region SetIsPickingChoosing
    public void SetIsPickingChoosing(int isPopupChoosing)
    {
        if (IsClient)
        {
            SetIsPickingChoosingServerRpc(isPopupChoosing);
        }
        else if (IsHost || IsServer)
        {
            SetIsPickingChoosingLocal(isPopupChoosing);
        }
        else
        {
            Debug.Log("method wrong is: SetIsPopupChoosing");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetIsPickingChoosingServerRpc(int isPickingChoosing)
    {
        SetIsPickingChoosingLocal(isPickingChoosing);
    }

    public void SetIsPickingChoosingLocal(int isPickingChoosing)
    {
        IsPickingChoosing.Value = isPickingChoosing;
    }
    #endregion


    #region SetIsPopupChoosing
    public void SetIsPopupChoosing(int isPopupChoosing)
    {
        if (IsClient)
        {
            SetIsPopupChoosingServerRpc(isPopupChoosing);
        }
        else if(IsHost||IsServer)
        {
            SetIsPopupChoosingLocal(isPopupChoosing);
        }
        else
        {
            Debug.Log("method wrong is: SetIsPopupChoosing");   
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetIsPopupChoosingServerRpc(int isPopupChoosing)
    {
        SetIsPopupChoosingLocal(isPopupChoosing);
    }
    
    public void SetIsPopupChoosingLocal(int isPopupChoosing)
    {
        IsPopupChoosing.Value = isPopupChoosing;
    }
    #endregion


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


    #region SetUnmergeChoosing
    public void SetUnmergeChoosing(int isPopupChoosing)
    {
        if (IsClient)
        {
            SetUnmergeChoosingServerRpc(isPopupChoosing);
        }
        else if (IsHost || IsServer)
        {
            SetUnmergeChoosingLocal(isPopupChoosing);
        }
        else
        {
            Debug.Log("method wrong is: SetIsPopupChoosing");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetUnmergeChoosingServerRpc(int unmergeStatus)
    {
        SetUnmergeChoosingLocal(unmergeStatus);
    }

    public void SetUnmergeChoosingLocal(int unmergeStatus)
    {
        IsUnmergeChoosing.Value = unmergeStatus;
    }
#endregion

    public int GetCurrentPlayerDeployPoint()
    {
        if (CurrentTurn.Value == 0)
        {
            return PlayerZeroDP.Value;
        }
        else if (CurrentTurn.Value == 1)
        {
            return PlayerOneDP.Value;
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
            return PlayerZeroMP.Value;
        }
        else if (CurrentTurn.Value == 1)
        {
            return PlayerOneMP.Value;
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
        }
        else
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
      //  MovePointIncreaseServerRpc(howMuchPoint);
        if (CurrentTurn.Value == 0)
        {
            PlayerZeroMP.Value = PlayerZeroMP.Value + howMuchPoint;
        }
        else
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
