using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeCard : NetworkBehaviour, IDropHandler
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
        if (NetworkManager.Singleton.IsClient && placeManager.GetMergedCardSelectedFromTable() != null) // && gameManager.GetComponent<GameManager>().IsPopupChoosing.Value == 0
        {

            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    Debug.Log("dropping merged card");
                    MoveMergedCard(0);
                }
            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
            {//check the max move of the card
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    Debug.Log("dropping merged card");
                    MoveMergedCard(1);
                }
            }

            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
        }

    }

    private void MoveMergedCard(int player)
    {
        int necessaryPoint = (placeManager.GetMergedCardSelectedFromTable().transform.parent.childCount);
        bool IsSingleCard = true;
        //check if the tile chosed is filled by a card or is an empty tile
        if (gameObject.GetComponent<CardTable>() != null)
        {
            IsSingleCard = false;
        }

        if (IsSingleCard)
        {
            if (player == 0)
            {//first check, if we have enough Move point to spend.
                if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value >= necessaryPoint)
                {
                    bool cardCreated = MoveCardFromTableOnEmptySpace("RPCT", necessaryPoint);
                    if (cardCreated)
                    {
                        gameManager.GetComponent<GameManager>().MovePointSpent(necessaryPoint, 0);
                        Debug.Log("Punti movimento spesi giocatore 0: " + necessaryPoint);
                        gridContainer.GetComponent<GridContainer>().ResetShowTiles();
                        placeManager.ResetCardHand();
                        placeManager.ResetMergedCardTable();
                        placeManager.ResetSingleCardTable();
                    }
                }
            }
            else if (player == 1)
            {//first check, if we have enough Move point to spend.
                if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value >= necessaryPoint)
                {
                    bool cardCreated = MoveCardFromTableOnEmptySpace("LPCT", necessaryPoint);
                    if (cardCreated)
                    {
                        gameManager.GetComponent<GameManager>().MovePointSpent(necessaryPoint, 1);
                        Debug.Log("Punti movimento spesi giocatore 1: " + necessaryPoint);
                        gridContainer.GetComponent<GridContainer>().ResetShowTiles();
                        placeManager.ResetCardHand();
                        placeManager.ResetMergedCardTable();
                        placeManager.ResetSingleCardTable();
                    }
                }
            }
        }
        else if (!IsSingleCard)
        {
            if (player == 0)
            {//first check, if we have enough Move point to spend.
                if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value >= necessaryPoint)
                {
                    bool cardCreated = MoveCardFromTableOnFilledSpace("RPCT", necessaryPoint);
                    if (cardCreated)
                    {
                        gameManager.GetComponent<GameManager>().MovePointSpent(necessaryPoint, 0);
                        Debug.Log("Punti movimento spesi giocatore 0: " + necessaryPoint);
                    }
                }
            }
            else if (player == 1)
            {//first check, if we have enough Move point to spend.
                if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value >= necessaryPoint)
                {
                    bool cardCreated = MoveCardFromTableOnFilledSpace("LPCT", necessaryPoint);
                    if (cardCreated)
                    {
                        gameManager.GetComponent<GameManager>().MovePointSpent(necessaryPoint, 1);
                        Debug.Log("Punti movimento spesi giocatore 1: " + necessaryPoint);
                    }
                }
            }
        }
    }


    private bool MoveCardFromTableOnFilledSpace(string cardTableTag, int numberOfMergedCards)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                                                     //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
            {
                    SpawnCardOnFilledSpaceFromServerRpc(
                        placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(0).gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(0).gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
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

    private void UpdateWeightTopCard(int cardWeight)
    {
        int finalWeight = cardWeight;
        CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();

        if (cardTable != null)
        {
            foreach (Transform singleCard in transform.parent)
            {
                if (singleCard.GetComponent<CardTable>() != null)
                {
                    finalWeight += singleCard.GetComponent<CardTable>().Weight.Value;
                }
                Debug.Log("Final weight: " + finalWeight);
            }
            UpdateWeightTopCardServerRpc(finalWeight, gameObject.GetComponent<CardTable>().CurrentPositionX.Value, gameObject.GetComponent<CardTable>().CurrentPositionY.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateWeightTopCardServerRpc(int finalWeight, int x, int y)
    {
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }

    private bool MoveCardFromTableOnEmptySpace(string cardTableTag, int numberOfMergedCards)
    {
        if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                         //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
            {
                    MoveCardFromTableOnEmptySpaceServerRpc(
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(0).gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(0).gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
gameObject.GetComponent<CoordinateSystem>().x,
gameObject.GetComponent<CoordinateSystem>().y
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

    private void UpdateWeight(int finalWeight, int x, int y)
    {
        UpdateWeightTopCardServerRpc(finalWeight, x, y);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        Debug.Log("1OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("1NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }


    [ServerRpc(RequireOwnership = false)]
    public void SpawnCardOnFilledSpaceFromServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile) //MyCardStruct cartaDaSpawnare
    {
        List<GameObject> cardsFromTile = gridContainer.GetComponent<GridContainer>().GetAllCardsFromTile(xOldTile, yOldTile);
        foreach (GameObject card in cardsFromTile)
        {
            card.transform.SetParent(transform.parent, false);
            card.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
            card.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;
        }
        int weightNewTile = gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTile(xNewTile, yNewTile);
        UpdateWeightCard(weightNewTile, xNewTile, yNewTile);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveCardFromTableOnEmptySpaceServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile) //MyCardStruct cartaDaSpawnare
    {
        List<GameObject> cardsFromTile = gridContainer.GetComponent<GridContainer>().GetAllCardsFromTile(xOldTile, yOldTile);
        foreach (GameObject card in cardsFromTile)
        {
            card.transform.SetParent(transform, false);
            card.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
            card.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;
        }
        int weightNewTile = gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTile(xNewTile, yNewTile);
        UpdateWeightCard(weightNewTile, xNewTile, yNewTile);
    }

    private void UpdateWeightCard(int weightNewTile, int xNewTile, int yNewTile)
    {
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xNewTile, yNewTile);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = weightNewTile;
    }
}
