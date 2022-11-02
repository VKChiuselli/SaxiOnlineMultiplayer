using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnmergeCard : NetworkBehaviour, IPointerDownHandler
{
    PlaceManager placeManager;
    GameObject gridContainer;
    GameObject gameManager;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (NetworkManager.Singleton.IsClient
           && placeManager.GetMergedCardSelectedFromTable() != null
           && gameManager.GetComponent<GameManager>().IsUnmergeChoosing.Value == 1) //bisogna mettere molte più condizioni per mettere la carta
        {
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value > 0) //instead of 0 put CARD.MOVEMENT_COST
                    {

                        bool isPlayed = MoveCardFromTable("RPCT");
                        if (isPlayed)
                        {
                            Debug.Log("punto sottratto PlayerZero move");
                            gameManager.GetComponent<GameManager>().MovePointSpent(1, 0);
                        }
                    }
                }

            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)//&& (NetworkManager.Singleton.LocalClientId % 2) == 0)
            {//check the max move of the card
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value > 0)
                    {

                        Debug.Log("punto sottratto PlayerOne move");

                        bool isPlayed = MoveCardFromTable("LPCT");
                        if (isPlayed)
                        {
                            gameManager.GetComponent<GameManager>().MovePointSpent(1, 1);
                        }

                    }
                }
            }
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
        }

    }

    private bool MoveCardFromTable(string cardTableTag)
    {//if it is gridManager, it means it is empty space on the grid otherwise is a card already existing
        if (gameObject.transform.parent.name == "GridManager")
        {
            if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                             //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
            {
                ChangeOwnerServerRpc();
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
                {
                    CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();
                    MoveSingleCardToEmptyTileServerRpc(
                   cardTable.CurrentPositionX.Value,
                   cardTable.CurrentPositionY.Value,
                      gameObject.GetComponent<CoordinateSystem>().x,
              gameObject.GetComponent<CoordinateSystem>().y
              );
                }
                else
                {
                    Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");
                }

                placeManager.ResetMergedCardTable();
                return true;
            }
            else
                return false;
        }
        else
        {
            if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                                                         //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
            {
                ChangeOwnerServerRpc();
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
                {
                    CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();
                    SpawnCardOnFilledSpaceFromServerRpc(
                            cardTable.CurrentPositionX.Value,
                   cardTable.CurrentPositionY.Value,
                 gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
                 gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y
              );
                }
                else
                {
                    Debug.Log("Class PlaceCard, method OnPointerDown, Errore! CardHand vuota");
                }

                placeManager.ResetMergedCardTable();
                return true;
            }
            else
                return false;
        }
    }

    private int GetWeightCardBelowTop(GameObject cardOnTop)
    {
        int finalWeight = 0;
        GameObject cardBelow = gridContainer.GetComponent<GridContainer>().GetBelowCard(cardOnTop.transform.parent.GetComponent<CoordinateSystem>().x, cardOnTop.transform.parent.GetComponent<CoordinateSystem>().y);
        if (cardBelow != null)
        {
            finalWeight = cardBelow.GetComponent<CardTable>().Weight.Value;
        }
        //TODO prendere il padre della carta, iterare la carta fino a trovare il secondo figlio cardOnTop.transform.parent
        return finalWeight;
    }



    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveSingleCardToEmptyTileServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
        GameObject topCard = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile);
        topCard.transform.SetParent(transform, false);
        topCard.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
        topCard.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;
        UpdateWeightTopCard(xOldTile, yOldTile);
        UpdateWeightTopCard(xNewTile, yNewTile);
    }



    [ServerRpc(RequireOwnership = false)]
    public void SpawnCardOnFilledSpaceFromServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile) //MyCardStruct cartaDaSpawnare
    {
        GameObject topCard = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile);
        topCard.transform.SetParent(transform.parent, false);
        topCard.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
        topCard.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;
        UpdateWeightTopCard(xOldTile, yOldTile);
        UpdateWeightTopCard(xNewTile, yNewTile);
    }

    private void UpdateWeightTopCard(int x, int y)
    {
        int finalWeight = gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTile(x, y);
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }
}
