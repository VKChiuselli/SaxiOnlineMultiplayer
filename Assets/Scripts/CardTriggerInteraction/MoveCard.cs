using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCard : NetworkBehaviour, IDropHandler
{

    PlaceManager placeManager;
    GameObject gridContainer;
    GameObject gameManager;
    GameObject deckManager;
    GameObject SpawnManager;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CoreGame/CanvasHandPlayer/GridManager");
          gameManager = GameObject.Find("CoreGame/Managers/GameManager");
         SpawnManager = GameObject.Find("CoreGame/Managers/SpawnManager");
          deckManager = GameObject.Find("CoreGame/CanvasHandPlayer/PanelPlayerRight");
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (gameManager.GetComponent<GameManager>().IsRunningPlayer() && placeManager.GetMergedCardSelectedFromTable() != null) // && gameManager.GetComponent<GameManager>().IsPopupChoosing.Value == 0
        {

            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    Debug.Log("dropping merged card");
                    MoveSelectedCard(0, placeManager.GetMergedCardSelectedFromTable());
                }
            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
            {//check the max move of the card
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    Debug.Log("dropping merged card");
                    MoveSelectedCard(1, placeManager.GetMergedCardSelectedFromTable());
                }
            }
            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
            gameManager.GetComponent<GameManager>().SetIsPopupChoosing(0);
        }
        else if (gameManager.GetComponent<GameManager>().IsRunningPlayer() && placeManager.GetSingleCardSelectedFromTable() != null) // && gameManager.GetComponent<GameManager>().IsPopupChoosing.Value == 0
        {

            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    Debug.Log("dropping single card");
                    MoveSelectedCard(0, placeManager.GetSingleCardSelectedFromTable());
                }
            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
            {//check the max move of the card
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    Debug.Log("dropping single card");
                    MoveSelectedCard(1, placeManager.GetSingleCardSelectedFromTable());
                }
            }

            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
            gameManager.GetComponent<GameManager>().SetIsPopupChoosing(0);
        }

    }

    private void MoveSelectedCard(int player, GameObject cardTableToMove)
    {
        bool IsSingleCard = true;
        //check if the tile chosed is filled by a card or is an empty tile
        if (gameObject.GetComponent<CardTable>() != null)
        {
            IsSingleCard = false;
        }

        if (IsSingleCard)
        {
            if (player == 0)
            {
                MoveCardFromTableOnEmptySpace(cardTableToMove);
            }
            else if (player == 1)
            {
                MoveCardFromTableOnEmptySpace(cardTableToMove);
            }
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
        }
        else if (!IsSingleCard)
        {
            if (player == 0)
            {
                MoveCardFromTableOnFilledSpace(cardTableToMove);
            }
            else if (player == 1)
            {
                MoveCardFromTableOnFilledSpace(cardTableToMove);
            }
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
        }
    }


    private bool MoveCardFromTableOnFilledSpace(GameObject cardTableToMove)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                                                     //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (cardTableToMove.GetComponent<CardTable>() != null)
            {
                SpawnManager.GetComponent<SpawnCardServer>().MoveToFriendlyTileServerRpc(
                       cardTableToMove.GetComponent<CardTable>().CurrentPositionX.Value,
cardTableToMove.GetComponent<CardTable>().CurrentPositionY.Value,
gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y
);
            }
            else
            {
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");
            }

            placeManager.ResetCardHand();
            return true;
        }
        else if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)                                                     //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (cardTableToMove.GetComponent<CardTable>() != null)
            {
                SpawnManager.GetComponent<SpawnCardServer>().PushCardFromTable(
                        cardTableToMove.GetComponent<CardTable>().CurrentPositionX.Value,
cardTableToMove.GetComponent<CardTable>().CurrentPositionY.Value,
gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y
);
            }
            else
            {
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");
            }

            placeManager.ResetCardHand();
            return true;
        }
        else
        {
            Debug.Log("MoveCardFromTableOnFilledSpace type of tile not correct: ");
            return false;
        }
    }


    private bool MoveCardFromTableOnEmptySpace(GameObject cardTableToMove)
    {
        if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                         //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (cardTableToMove.GetComponent<CardTable>() != null)
            {
                SpawnManager.GetComponent<SpawnCardServer>().MoveAllCardsToEmptyTileServerRpc(
cardTableToMove.GetComponent<CardTable>().CurrentPositionX.Value,
cardTableToMove.GetComponent<CardTable>().CurrentPositionY.Value,
gameObject.GetComponent<CoordinateSystem>().x,
gameObject.GetComponent<CoordinateSystem>().y,
false
);
            }
            else
            {
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");
            }

            placeManager.ResetCardHand();
            return true;
        }
        else
            return false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        ChangeOwner();
    }

    public void ChangeOwner()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }
}
