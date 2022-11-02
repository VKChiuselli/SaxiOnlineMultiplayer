using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSingleCardFromTable : NetworkBehaviour, IDropHandler
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

    public void OnDrop(PointerEventData eventData)
    {
        if (NetworkManager.Singleton.IsClient && placeManager.GetSingleCardSelectedFromTable() != null) //bisogna mettere molte più condizioni per mettere la carta
        {
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value > 0)
                    {

                        bool isPlayed = MoveCardFromTable("RPCT");
                        if (isPlayed)
                        {
                            Debug.Log("punto sottratto PlayerZero move");
                            gameManager.GetComponent<GameManager>().MovePointSpent(1, 0);
                            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
                            placeManager.ResetCardHand();
                            placeManager.ResetMergedCardTable();
                            placeManager.ResetSingleCardTable();
                        }
                    }
                }

            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)//&& (NetworkManager.Singleton.LocalClientId % 2) == 0)
            {//check the max move of the card
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value > 0)
                    {

                        Debug.Log("punto sottratto PlayerOne move");

                        bool isPlayed = MoveCardFromTable("LPCT");
                        if (isPlayed)
                        {
                            gameManager.GetComponent<GameManager>().MovePointSpent(1, 1);
                            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
                            placeManager.ResetCardHand();
                            placeManager.ResetMergedCardTable();
                            placeManager.ResetSingleCardTable();
                        }
                    }
                }
            }

            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
        }
    }

    private bool MoveCardFromTable(string cardTableTag)
    {//if it is gridManager, it means it is empty space on the grid otherwise is a card already existing
        if (gameObject.transform.parent.name == "GridManager")
        {
            if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1)
            {
                ChangeOwnerServerRpc();
                CardTable cardTable = placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>();
                MoveSingleCardToEmptyTileServerRpc(
                    cardTable.CurrentPositionX.Value,
                    cardTable.CurrentPositionY.Value,
                    gameObject.GetComponent<CoordinateSystem>().x,
                    gameObject.GetComponent<CoordinateSystem>().y);

                placeManager.ResetCardHand();
                return true;
            }
            else
            {
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! typeOfTile errato");
                return false;
            }
        }
        else
        {
            if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2)
            {
                ChangeOwnerServerRpc();
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>() != null)
                {
                    CardTable cardTable = placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>();
                    int xNewTile = gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
                    int yNewTile = gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
                    MoveSingleCardToFilledTileServerRpc(
                        cardTable.CurrentPositionX.Value,
                        cardTable.CurrentPositionY.Value,
                        xNewTile,
                        yNewTile);
                    UpdateWeightTopCard(placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().Weight.Value, xNewTile, yNewTile);
                }
                else
                {
                    Debug.Log("Class PlaceCard, method OnPointerDown, Errore! CardHand vuota");
                }

                placeManager.ResetCardHand();
                return true;
            }
            else
                return false;
        }
    }



    private void UpdateWeightTopCard(int cardWeight, int x, int y)
    {
        int finalWeight = cardWeight;

        finalWeight = finalWeight + gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTile(x, y);
        Debug.Log("Final weight: " + finalWeight);
        UpdateWeightTopCardServerRpc(finalWeight, x, y);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateWeightTopCardServerRpc(int finalWeight, int x, int y)
    {
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveSingleCardToEmptyTileServerRpc(int x, int y, int xNewTile, int yNewTile)
    {
        List<GameObject> cardsFromTile = gridContainer.GetComponent<GridContainer>().GetAllCardsFromTile(x, y);
        foreach (GameObject card in cardsFromTile)
        {
            card.transform.SetParent(transform, false);
            card.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
            card.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveSingleCardToFilledTileServerRpc(int x, int y, int xNewTile, int yNewTile)
    {
        NetworkObject networkObjectCard = null;
        List<GameObject> cardsFromTile = gridContainer.GetComponent<GridContainer>().GetAllCardsFromTile(x, y);
        foreach (GameObject card in cardsFromTile)
        {
            networkObjectCard = Instantiate(card.GetComponent<NetworkObject>(),
    transform.parent.position, Quaternion.identity);
            networkObjectCard.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            networkObjectCard.transform.SetParent(transform.parent, false);
            gridContainer.GetComponent<GridContainer>().RemoveCardFromTable(x, y);
            networkObjectCard.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
            networkObjectCard.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;
        }
    }
}
