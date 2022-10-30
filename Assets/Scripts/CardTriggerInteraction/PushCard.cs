using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PushCard : NetworkBehaviour, IDropHandler
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
                    if (gameObject.GetComponent<CardTable>() != null)
                    {
                        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)
                        {
                            Debug.Log("player 0 push a merged card");
                            PushWithMergedCard(0);
                        }
                    }
                }
            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
            {//check the max move of the card
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    if (gameObject.GetComponent<CardTable>() != null)
                    {
                        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)
                        {
                            Debug.Log("player 1 push a merged card");
                            //  MoveMergedCard(0);
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
        else if (NetworkManager.Singleton.IsClient && placeManager.GetSingleCardSelectedFromTable() != null)
        {
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    if (gameObject.GetComponent<CardTable>() != null)
                    {
                        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)
                        {
                            Debug.Log("player 0 push a single card");
                            //  MoveMergedCard(0);
                        }
                    }
                }
            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
            {//check the max move of the card
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    if (gameObject.GetComponent<CardTable>() != null)
                    {
                        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)
                        {
                            Debug.Log("player 1 push a single card");
                            //  MoveMergedCard(0);
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

    private void PushWithMergedCard(int player)
    {
        int necessaryPoint = (placeManager.GetMergedCardSelectedFromTable().transform.parent.childCount - 1);


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

    private void MoveMergedCard(int player)
    {
        int necessaryPoint = (placeManager.GetMergedCardSelectedFromTable().transform.parent.childCount - 1);
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
        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                                                     //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo � maggiore uguale di uno il check
        {
            //CurrentTurn.Value = CurrentTurn.Value == 1 ? 0 : 1;
            int weightEnemyCard = gameObject.GetComponent<CardTable>().MergedWeight.Value == 0 ? gameObject.GetComponent<CardTable>().Weight.Value : gameObject.GetComponent<CardTable>().MergedWeight.Value;
            if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().MergedWeight.Value <= weightEnemyCard)
            {
                return false;
            }

            if (!CheckBehindCard(
                placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
                gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
                placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().MergedWeight.Value,
                weightEnemyCard))
            {
                return false;
            }

            ChangeOwnerServerRpc();
            int indexCard = 1;
            while (numberOfMergedCards != 0)
            {
                SpawnCardOnFilledSpaceFromServerRpc(
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().IdCard.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().Weight.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().Speed.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().IdOwner.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().IdImageCard.Value.ToString(),
cardTableTag, //RPT Right player Table
true, //it means that we have to destroy the old game object when we move
gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
indexCard
);
                numberOfMergedCards--;
                indexCard++;
            }
            UpdateWeightTopCard(placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard - 1).gameObject.GetComponent<CardTable>().MergedWeight.Value);

            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            return true;
        }
        else
        {
            Debug.Log("MoveCardFromTableOnFilledSpace type of tile not correct: ");
            return false;
        }
    }

    //se facessi 1=tileVuoto 2=tileOccupato 5=tileNonEsistente
    private bool CheckBehindCard(int xPusher, int yPusher, int xPushed, int yPushed, int weightFriendly, int weightEnemy)
    {
  //      string direction = CheckDirection(xPusher, yPusher, xPushed, yPushed);
        int x = xPushed - xPusher;
        int y = yPushed - yPusher;
        if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 5 || gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 1)
        {
            return true; //se c'� 5 o 1 (vuoto o baratro) allora faccio il check
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 2)
        {
            int nextCardWeight = gridContainer.GetComponent<GridContainer>().GetNextTileWeight(xPusher, yPusher, xPushed, yPushed);
            int totalWeight = nextCardWeight + weightEnemy;
            if (totalWeight >= weightFriendly)
            {
                return false;
            }

            return CheckBehindCard(xPusher+x, yPusher+y, xPushed+x, yPushed+y, weightFriendly, totalWeight);
        }

        return false;
    }

    private string CheckDirection(int xPusher, int yPusher, int xPushed, int yPushed)
    {
        string finalDirection = "";

        int x = xPushed - xPusher;
        int y = yPushed - yPusher;

        switch (x, y)
        {
            case (0, 1):
                finalDirection = "RIGHT";
                break;
            case (0, -1):
                finalDirection = "LEFT";
                break;
            case (1, 0):
                finalDirection = "DOWN";
                break;
            case (-1, 0):
                finalDirection = "UP";
                break;
        }

        return finalDirection;
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
                                                                         //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo � maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
            {
                int indexCard = 1;
                while (numberOfMergedCards != 0)
                {
                    MoveCardFromTableOnEmptySpaceServerRpc(
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().IdCard.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().Weight.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().Speed.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().IdOwner.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().IdImageCard.Value.ToString(),
cardTableTag, //RPT Right player Table
true, //it means that we have to destroy the old game object when we move
gameObject.GetComponent<CoordinateSystem>().x,
gameObject.GetComponent<CoordinateSystem>().y,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard).gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
indexCard
);
                    numberOfMergedCards--;
                    indexCard++;
                }
                UpdateWeight(placeManager.GetMergedCardSelectedFromTable().transform.parent.GetChild(indexCard - 1).gameObject.GetComponent<CardTable>().MergedWeight.Value, placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value, placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value);
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
    public void SpawnCardOnFilledSpaceFromServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete, int indexCard) //MyCardStruct cartaDaSpawnare
    {
        CardTableToSpawn.tag = tag;
        NetworkObject go = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
           transform.parent.position, Quaternion.identity);

        go.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        go.transform.SetParent(transform.parent, false);

        go.GetComponent<CardTable>().IdCard.Value = IdCard;
        go.GetComponent<CardTable>().Weight.Value = Weight;
        go.GetComponent<CardTable>().Speed.Value = Speed;
        go.GetComponent<CardTable>().IdOwner.Value = IdOwner;
        go.GetComponent<CardTable>().IdImageCard.Value = IdImageCard;
        go.GetComponent<CardTable>().CurrentPositionX.Value = x;
        go.GetComponent<CardTable>().CurrentPositionY.Value = y;
        go.GetComponent<NetworkObject>().tag = tag;
        go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        go.transform.localPosition = new Vector3(0.5f, 0.5f, 1f);
        if (toDestroy)
        {
            gridContainer.GetComponent<GridContainer>().RemoveFirstMergedCardFromTable(xToDelete, yToDelete, indexCard);
        }

    }
    [ServerRpc(RequireOwnership = false)]
    public void MoveCardFromTableOnEmptySpaceServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete, int indexCard) //MyCardStruct cartaDaSpawnare
    {
        CardTableToSpawn.tag = tag;
        NetworkObject go = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
           transform.position, Quaternion.identity);

        go.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        go.transform.SetParent(transform, false);

        go.GetComponent<CardTable>().IdCard.Value = IdCard;
        go.GetComponent<CardTable>().Weight.Value = Weight;
        go.GetComponent<CardTable>().Speed.Value = Speed;
        go.GetComponent<CardTable>().IdOwner.Value = IdOwner;
        go.GetComponent<CardTable>().IdImageCard.Value = IdImageCard;
        go.GetComponent<CardTable>().CurrentPositionX.Value = x;
        go.GetComponent<CardTable>().CurrentPositionY.Value = y;
        go.GetComponent<NetworkObject>().tag = tag;
        go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        go.transform.localPosition = new Vector3(0.5f, 0.5f, 1f);
        if (toDestroy)
        {
            gridContainer.GetComponent<GridContainer>().RemoveFirstMergedCardFromTable(xToDelete, yToDelete, indexCard);
        }

    }


}
