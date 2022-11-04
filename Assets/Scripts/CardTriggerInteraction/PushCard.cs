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
    GameObject SpawnManager;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
        SpawnManager = GameObject.Find("Managers/SpawnManager");
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
                            PushWithSingleCard(0);
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
                            PushWithSingleCard(1);
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

    private void PushWithSingleCard(int player)
    {
        int necessaryPoint = 1;


        if (player == 0)
        {//first check, if we have enough Move point to spend.
            if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value >= necessaryPoint)
            {
                bool cardCreated = PushCardFromTable( placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>());
                if (cardCreated)
                {
                    gameManager.GetComponent<GameManager>().MovePointSpent(necessaryPoint );
                    Debug.Log("Punti movimento spesi giocatore 0: " + necessaryPoint);
                }
            }
        }
        else if (player == 1)
        {//first check, if we have enough Move point to spend.
            if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value >= necessaryPoint)
            {
                bool cardCreated = PushCardFromTable( placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>());
                if (cardCreated)
                {
                    gameManager.GetComponent<GameManager>().MovePointSpent(necessaryPoint );
                    Debug.Log("Punti movimento spesi giocatore 1: " + necessaryPoint);
                }
            }
        }
    }
    private void PushWithMergedCard(int player)
    {
        int necessaryPoint = (placeManager.GetMergedCardSelectedFromTable().transform.parent.childCount);


        if (player == 0)
        {//first check, if we have enough Move point to spend.
            if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value >= necessaryPoint)
            {
                bool cardCreated = PushCardFromTable( placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>());
                if (cardCreated)
                {
                    gameManager.GetComponent<GameManager>().MovePointSpent(necessaryPoint );
                    Debug.Log("Punti movimento spesi giocatore 0: " + necessaryPoint);
                }
            }
        }
        else if (player == 1)
        {//first check, if we have enough Move point to spend.
            if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value >= necessaryPoint)
            {
                bool cardCreated = PushCardFromTable( placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>());
                if (cardCreated)
                {
                    gameManager.GetComponent<GameManager>().MovePointSpent(necessaryPoint );
                    Debug.Log("Punti movimento spesi giocatore 1: " + necessaryPoint);
                }
            }
        }
    }


    private bool PushCardFromTable( CardTable cardTable)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                                                     //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            CardTable currentCardSelected = cardTable;

            int weightFriendlyCard = cardTable.MergedWeight.Value == 0 ? cardTable.Weight.Value : cardTable.MergedWeight.Value;
            int weightEnemyCard = gameObject.GetComponent<CardTable>().MergedWeight.Value == 0 ? gameObject.GetComponent<CardTable>().Weight.Value : gameObject.GetComponent<CardTable>().MergedWeight.Value;
            if (weightFriendlyCard <= weightEnemyCard)
            {
                return false;
            }

            int check = CheckBehindCard(
                      currentCardSelected.CurrentPositionX.Value,
                      currentCardSelected.CurrentPositionY.Value,
                      gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
                      gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
                      weightFriendlyCard,
                      weightEnemyCard);

            if (check == 505)
            {
                return false;
            }



            ChangeOwnerServerRpc();


            List<GameObject> tilesToPushList = new List<GameObject>();
            List<GameObject> tilesToPush = new List<GameObject>();
            tilesToPush = FindAllCardsToPush(
                      currentCardSelected.CurrentPositionX.Value,
                      currentCardSelected.CurrentPositionY.Value,
                      gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
                      gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
                      weightFriendlyCard,
                      weightEnemyCard,
                      tilesToPushList);

            int x = gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x - currentCardSelected.CurrentPositionX.Value;
            int y = gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y - currentCardSelected.CurrentPositionY.Value;


            tilesToPush.Reverse();

            if (tilesToPush != null)
            {
                //I push the cards less the pusher
                foreach (GameObject tile in tilesToPush)
                {
                    SpawnManager.GetComponent<SpawnCardServer>().MoveAllCardsToEmptyTileServerRpc(
                        tile.GetComponent<CoordinateSystem>().x, tile.GetComponent<CoordinateSystem>().y,
                        tile.GetComponent<CoordinateSystem>().x + x, tile.GetComponent<CoordinateSystem>().y + y
                        );
                }
                //I move the card that pushed the other cards
                SpawnManager.GetComponent<SpawnCardServer>().MoveAllCardsToEmptyTileServerRpc(
                    currentCardSelected.CurrentPositionX.Value, currentCardSelected.CurrentPositionY.Value,
                    gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x, gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y
                    );
            }
            else
            {
                Debug.Log("ERROR! no card added in the list to be pushed!");
            }


            return true;
        }

        return false;
    }

    private List<GameObject> FindAllCardsToPush(int xPusher, int yPusher, int xPushed, int yPushed, int weightFriendly, int weightEnemy, List<GameObject> tilesToPush)
    {
        int x = xPushed - xPusher;
        int y = yPushed - yPusher;
        if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 5)
        {
            return tilesToPush;
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 1)
        {
            return tilesToPush;
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 2)
        {
            int nextCardWeight = gridContainer.GetComponent<GridContainer>().GetNextTileWeight(xPusher + x, yPusher + y, xPushed + x, yPushed + y);
            int totalWeight = nextCardWeight + weightEnemy;
            if (totalWeight >= weightFriendly)
            {
                Debug.Log("we shold not enter here becauise FindAllCardsToPush means that all cards will be pushed!");
                return tilesToPush;
            }

            GameObject tileToAdd = gridContainer.GetComponent<GridContainer>().GetTile(xPushed, yPushed);
            tilesToPush.Add(tileToAdd);

            return FindAllCardsToPush(xPusher + x, yPusher + y, xPushed + x, yPushed + y, weightFriendly, totalWeight, tilesToPush);
        }

        return tilesToPush;
    }


    //se facessi 1=tileVuoto 2=tileOccupato 5=tileNonEsistente
    private int CheckBehindCard(int xPusher, int yPusher, int xPushed, int yPushed, int weightFriendly, int weightEnemy)
    {
        int x = xPushed - xPusher;
        int y = yPushed - yPusher;
        if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 5)
        {
            return 400; //400 è VERO
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 1)
        {
            return 400; //400 è VERO
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 2)
        {
            int nextCardWeight = gridContainer.GetComponent<GridContainer>().GetNextTileWeight(xPusher + x, yPusher + y, xPushed + x, yPushed + y);
            int totalWeight = nextCardWeight + weightEnemy;
            if (totalWeight >= weightFriendly)
            {
                Debug.Log("too much weight, we can't push it");
                return 505; //505 è FALSO
            }

            return CheckBehindCard(xPusher + x, yPusher + y, xPushed + x, yPushed + y, weightFriendly, totalWeight);
        }

        return 505;
    }


    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        Debug.Log("1OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("1NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }


}
