using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GridContainer : NetworkBehaviour
{
    public List<GameObject> gridTiles = new List<GameObject>();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            gridTiles.Add(child.gameObject);
        }
    }

    public List<GameObject> GetDeployTiles(string deployTagTile)
    {
        List<GameObject> DeployTile = new List<GameObject>();

        foreach (GameObject deployTile in gridTiles)
        {
            if (deployTile.tag == deployTagTile)
            {
                DeployTile.Add(deployTile);
            }
        }
        if (DeployTile == null)
        {
            Debug.Log("Class GridContainer, method GetDeployTilesRight, no deploy tiles found, null expection");
        }
        return DeployTile;
    }

    public void ResetShowTiles() //ClientRpc
    {
        if (gridTiles != null)
        {
            foreach (GameObject tile in gridTiles)
            {
                tile.GetComponent<Highlight>().ResetColorTile();
            }
        }
        else
        {
            Debug.Log("ResetShowTilesClientRpc doens't found any Highlight");
        }
    }

    public void ShowMovementTilesAroundCard(GameObject cardTablePassed)
    {
        if (gridTiles != null)
        {

            int x = cardTablePassed.transform.parent.GetComponent<CoordinateSystem>().x;
            int y = cardTablePassed.transform.parent.GetComponent<CoordinateSystem>().y;

            int upX = x;
            int upY = y - 1;

            int leftX = x - 1;
            int leftY = y;

            int downX = x;
            int downY = y + 1;

            int rightX = x + 1;
            int rightY = y;

            foreach (GameObject tile in gridTiles)
            {
                if (tile.GetComponent<CoordinateSystem>().x == upX && tile.GetComponent<CoordinateSystem>().y == upY)
                {
                    ShowTile(cardTablePassed, tile);
                }
                else
                if (tile.GetComponent<CoordinateSystem>().x == leftX && tile.GetComponent<CoordinateSystem>().y == leftY)
                {
                    ShowTile(cardTablePassed, tile);
                }
                else
                if (tile.GetComponent<CoordinateSystem>().x == downX && tile.GetComponent<CoordinateSystem>().y == downY)
                {
                    ShowTile(cardTablePassed, tile);
                }
                else
                if (tile.GetComponent<CoordinateSystem>().x == rightX && tile.GetComponent<CoordinateSystem>().y == rightY)
                {
                    ShowTile(cardTablePassed, tile);
                }
                else
                if (tile.GetComponent<CoordinateSystem>().x == x && tile.GetComponent<CoordinateSystem>().y == y)
                {
                    tile.GetComponent<Highlight>().ShowTileCanInteract(7);
                }
            }
        }
        else
        {
            Debug.Log("ShowMovementTilesAroundCard doens't found any grid tile");
        }
    }

    private static void ShowTile(GameObject cardTablePassed, GameObject tile)
    {
        if (tile.transform.childCount == 0)
        {//if has only one child it means that the tile is empty, so i put 1 in ShowTileCanDeploy
            tile.GetComponent<Highlight>().ShowTileCanInteract(1);
        }
        else
        {
            if (tile.transform.GetChild(0).gameObject.GetComponent<CardTable>().IdOwner.Value == cardTablePassed.GetComponent<CardTable>().IdOwner.Value)
            {
                tile.GetComponent<Highlight>().ShowTileCanInteract(2);
            }
            else
            {
                tile.GetComponent<Highlight>().ShowTileCanInteract(3);
            }
        }
    }

    public void ShowTileToInteract(List<GameObject> tileToInteract)
    {
        foreach (GameObject tile in tileToInteract)
        {
            if (tile.transform.parent.GetComponent<Highlight>() != null)
            {
                tile.transform.parent.GetComponent<Highlight>().ShowTileCanInteract(7);
            }
        }
    }

    public void RemoveCardFromTable(int x, int y)
    {
        GameObject cardToRemove = GetTile(x, y);
        cardToRemove.transform.GetChild(cardToRemove.transform.childCount - 1).gameObject.GetComponent<NetworkObject>().Despawn();
    }

    public bool ExistHalfBoardCard(int player) //it means if exist a card in the opposite part of the board , in "enemy terrain"
    {
        List<GameObject> listHalfBoard = GetHalfBoardCard(player);

        if (listHalfBoard.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public List<GameObject> GetHalfBoardCard(int player)
    {
        List<GameObject> listHalfBoard = new List<GameObject>();
        if (player == 0)
        {
            for (int x = 1; x < 6; x++)
            {
                for (int y = 1; y < 7; y++)
                {
                    GameObject topCard = GetTopCardOnTile(x, y);

                    if (topCard != null)
                    {
                        if (topCard.GetComponent<CardTable>() != null)
                        {
                            if (topCard.GetComponent<CardTable>().IdOwner.Value == player)
                            {
                                listHalfBoard.Add(topCard);
                            }
                        }

                    }
                }
            }
        }
        else if (player == 1)
        {
            for (int x = 6; x < 11; x++)
            {
                for (int y = 1; y < 7; y++)
                {
                    GameObject topCard = GetTopCardOnTile(x, y);

                    if (topCard != null)
                    {
                        if (topCard.GetComponent<CardTable>() != null)
                        {
                            if (topCard.GetComponent<CardTable>().IdOwner.Value == player)
                            {
                                listHalfBoard.Add(topCard);
                            }
                        }

                    }
                }
            }
        }

        return listHalfBoard;
    }

    public List<GameObject> GetAllCardsFromTile(int x, int y)
    {
        GameObject tile = GetTile(x, y);
        List<GameObject> cardsOnTile = new List<GameObject>();

        foreach (Transform cardToAdd in tile.transform)
        {
            GameObject card = cardToAdd.gameObject;
            cardsOnTile.Add(card);
        }

        return cardsOnTile;
    }
    public List<GameObject> GetAllCardsFromTile(GameObject tile)
    {
        List<GameObject> cardsOnTile = new List<GameObject>();

        foreach (Transform cardToAdd in tile.transform)
        {
            GameObject card = cardToAdd.gameObject;
            cardsOnTile.Add(card);
        }

        return cardsOnTile;
    }

    public Transform GetCardTransform(int x, int y)
    {
        GameObject tile = GetTile(x, y);
        //  tile.transform.GetChild(tile.transform.childCount - 1).gameObject.GetComponent<NetworkObject>().Spawn();
        return tile.transform;
    }

    public void RemoveFirstMergedCardFromTable(int x, int y)
    {

        GameObject tile = GetTile(x, y);
        tile.transform.GetChild(tile.transform.childCount - 1).gameObject.GetComponent<NetworkObject>().Despawn();
    }
    public void RemoveFirstMergedCardFromTable(GameObject card)
    {
        card.GetComponent<NetworkObject>().Despawn();
    }
    public void RemoveIndexMergedCardFromTable(int x, int y, int indexCard)
    {
        foreach (GameObject tile in gridTiles)
        {
            if (tile.GetComponent<CoordinateSystem>().x == x && tile.GetComponent<CoordinateSystem>().y == y)
            {
                Debug.Log("RemoveIndexMergedCardFromTable Found!!");
                //despawn the last children
                tile.transform.GetChild(indexCard).gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }
    }//TODO convert method using  GetTile

    public GameObject GetTopCardOnTile(int x, int y)//do per scontato che qui c'è una carta 
    {
        GameObject getTile = GetTile(x, y);
        if (getTile == null)
        {
            return null;
        }
        if (getTile.transform.childCount == 0)
        {
            return null;
        }

        GameObject topCard;

        topCard = getTile.transform.GetChild(getTile.transform.childCount - 1).gameObject;
        return topCard;
    }

    public GameObject GetTopCardOnTile(GameObject tile)//do per scontato che qui c'è una carta 
    {
        GameObject getTile = GetTile(tile.GetComponent<CoordinateSystem>().x, tile.GetComponent<CoordinateSystem>().y);
        if (getTile == null)
        {
            Debug.Log("GetTopCardOnTile: the card on top is null");
            return null;
        }
        if (getTile.transform.childCount == 0)
        {
            Debug.Log("I didnt find any card on top of this tile");
            return null;
        }

        GameObject topCard;

        topCard = getTile.transform.GetChild(getTile.transform.childCount - 1).gameObject;
        return topCard;
    }
    public int GetTotalWeightOnTile(int x, int y)
    {
        int weight = CalculateTotalWeight(GetTile(x, y));
        return weight;
    }

    public int GetTotalMoveCostOnTile(int x, int y)
    {
        int moveCost = CalculateTotalMoveCost(GetTile(x, y));
        return moveCost;
    }

    public int GetTotalWeightOnTileLessLastOne(int x, int y)
    {
        int weight = CalculateTotalWeightLessLastOne(GetTile(x, y));
        return weight;
    }

    public GameObject GetTile(int x, int y)
    {
        GameObject returnTile = null;
        foreach (GameObject tile in gridTiles)
        {
            if (tile.GetComponent<CoordinateSystem>().x == x && tile.GetComponent<CoordinateSystem>().y == y)
            {
                returnTile = tile;
                return returnTile;
            }
        }
        return returnTile;
    }

    private int CalculateTotalWeight(GameObject tile)
    {
        int weight = 0;
        foreach (Transform card in tile.transform)
        {
            if (card.GetComponent<CardTable>() != null)
            {
                weight += card.GetComponent<CardTable>().Weight.Value;
            }
        }
        return weight;
    }

    private int CalculateTotalMoveCost(GameObject tile)
    {
        int moveCost = 0;

        if (tile == null)
        {
            return 1;
        }

        foreach (Transform card in tile.transform)
        {
            if (card.GetComponent<CardTable>() != null)
            {
                moveCost += card.GetComponent<CardTable>().MoveCost.Value;
            }
        }
        if (moveCost == 0)
        {
            Debug.Log("ERROR!!! Zero move cost");
        }
        return moveCost;
    }

    private int CalculateTotalWeightLessLastOne(GameObject tile)
    {
        int weight = 0;
        foreach (Transform card in tile.transform)
        {
            if (card != tile.transform.GetChild(tile.transform.childCount - 1))//TODO correggere! non dovrebbe tornare il peso corretto
            {
                if (card.GetComponent<CardTable>() != null)
                {
                    weight += card.GetComponent<CardTable>().Weight.Value;
                }
            }


        }
        return weight;
    }

    public GameObject GetBelowCard(int x, int y)
    {
        GameObject finalCard = null;

        foreach (GameObject tile in gridTiles)
        {
            if (tile.GetComponent<CoordinateSystem>().x == x && tile.GetComponent<CoordinateSystem>().y == y)
            {
                finalCard = tile.transform.GetChild(tile.transform.childCount - 1).gameObject;
                return finalCard;
            }
        }
        Debug.Log("GetTopCardOnTile method, no CARD FOUND to return!!");
        return finalCard;
    }

    public GameObject GetNextTile(int xPusher, int yPusher, int xPushed, int yPushed)
    {

        int xNext = xPushed - xPusher;
        int yNext = yPushed - yPusher;

        GameObject nextTile = GetTile(xPusher + xNext, yPusher + yNext);

        if (nextTile == null)
        {
            Debug.Log("Method GetNextTile: no tile found on next");
            return null;
        }

        return nextTile;
    }

    public int GetNextTileType(int xPusher, int yPusher, int xPushed, int yPushed)
    {

        int xNext = xPushed - xPusher;
        int yNext = yPushed - yPusher;

        GameObject nextTile = GetTile(xPusher + xNext, yPusher + yNext);

        if (nextTile == null)
        {
            Debug.Log("Method GetNextTileType: no tile found on next");
            return 5;//it means no tile avaiable, the VOID
        }

        if (nextTile.transform.childCount >= 1)
        {
            return 2;
        }//the next tile is filled by a card

        return 1; //the next tile is empty
    }
    public int GetNextTileWeight(int xPusher, int yPusher, int xPushed, int yPushed)
    {

        int xNext = xPushed - xPusher;
        int yNext = yPushed - yPusher;

        GameObject nextTile = GetTile(xPusher + xNext, yPusher + yNext);

        if (nextTile == null)
        {
            Debug.Log("Method GetNextTileType: no tile found on next");
            return 0;
        }

        return CalculateTotalWeight(nextTile);
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

    public List<GameObject> GetCardsFromPlayer(int player)
    {
        List<GameObject> playerCards = new List<GameObject>();

        foreach (GameObject tile in gridTiles)
        {
            foreach (GameObject card in GetAllCardsFromTile(tile))
            {
                if (card.GetComponent<CardTable>() != null)
                {
                    if (card.GetComponent<CardTable>().IdOwner.Value == player)
                    {
                        playerCards.Add(card);
                    }
                }
            }
        }

        return playerCards;

    }

    public List<GameObject> GetAllTopCardsFromPlayer(int player)
    {
        List<GameObject> playerCards = new List<GameObject>();

        foreach (GameObject tile in gridTiles)
        {
            GameObject card = GetTopCardOnTile(tile);
            if (card != null)
            {
                if (card.GetComponent<CardTable>() != null)
                {
                    if (card.GetComponent<CardTable>().IdOwner.Value == player)
                    {
                        playerCards.Add(card);
                    }
                }
            }
        }

        return playerCards;

    }


    public bool HasAdjacentEnemyCard(int x, int y, int player)
    {
        if (GetTopCardOnTile(x - 1, y) != null)
        {
            if(GetTopCardOnTile(x - 1, y).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x - 1, y).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    return true;
                }
            }
        }

        if (GetTopCardOnTile(x + 1, y) != null)
        {
            if(GetTopCardOnTile(x + 1, y).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x + 1, y).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    return true;
                }
            }
        }

        if (GetTopCardOnTile(x , y + 1) != null)
        {
            if(GetTopCardOnTile(x , y + 1).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x , y + 1).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    return true;
                }
            }
        }

        if (GetTopCardOnTile(x , y - 1) != null)
        {
            if(GetTopCardOnTile(x , y - 1).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x , y - 1).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
