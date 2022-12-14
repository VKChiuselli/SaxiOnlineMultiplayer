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

    public List<GameObject> GetAllTileWithCardInColumn(int whichColumn)
    {
        List<GameObject> tileWithCards = new List<GameObject>();

        for(int i=1; i<7; i++)
        {
            if(GetTopCardOnTile(whichColumn, i) != null)
            {
                tileWithCards.Add(GetTile(whichColumn, i));
            }
        }

        return tileWithCards;
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
    public void ShowTileToInteractByTile(List<GameObject> tileToInteract)
    {
        foreach (GameObject tile in tileToInteract)
        {
            if (tile.GetComponent<Highlight>() != null)
            {
                tile.GetComponent<Highlight>().ShowTileCanInteract(7);
            }
        }
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
        GameObject tile2 = tile.transform.GetChild(tile.transform.childCount - 1).gameObject;
        tile2.transform.GetChild(2).gameObject.GetComponent<NetworkObject>().Despawn(); //despawn the child network and than the father
        tile2.GetComponent<NetworkObject>().Despawn();
    }

    public GameObject GetTopCardOnTile(int x, int y)//do per scontato che qui c'? una carta 
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

    public GameObject GetTopCardOnTile(GameObject tile)//do per scontato che qui c'? una carta 
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

    public GameObject GetNextTile(int x1, int y1, int x2, int y2)
    {

        int finalX = x1 - x2;
        int finalY = y1 - y2;

        GameObject nextTile = GetTile(x2 - finalX, y2 - finalY);

        if (nextTile == null)
        {
            Debug.Log("Method GetNextTile: no tile found on next");
            return null;
        }

        return nextTile;
    }

    public int GetTileType(int xPushed, int yPushed)
    {

        GameObject nextTile = GetTile(xPushed, yPushed);

        if (nextTile == null)
        {
            Debug.Log("Method GetTileType: no tile found on next");
            return 5;//it means no tile avaiable, the VOID
        }

        if (nextTile.transform.childCount >= 1)
        {
            Debug.Log("Method GetTileType: the next tile is filled by a card");
            return 2;
        }//the next tile is filled by a card

        return 1; //the next tile is empty
    } 
    
    public int GetNextTileType(int x1, int y1, int x2, int y2)
    {

        int finalX = x1 - x2;
        int finalY = y1 - y2;

        GameObject nextTile = GetTile(x2-finalX, y2-finalY);

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
    public List<GameObject> GetTilesFromPlayer(int player)
    {
        List<GameObject> playerCards = new List<GameObject>();

        foreach (GameObject tile in gridTiles)
        {
            if (GetTopCardOnTile(tile)!=null)
            {
                if (GetTopCardOnTile(tile).GetComponent<CardTable>().IdOwner.Value == player)
                {
                    playerCards.Add(tile);
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
            if (GetTopCardOnTile(x - 1, y).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x - 1, y).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    return true;
                }
            }
        }

        if (GetTopCardOnTile(x + 1, y) != null)
        {
            if (GetTopCardOnTile(x + 1, y).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x + 1, y).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    return true;
                }
            }
        }

        if (GetTopCardOnTile(x, y + 1) != null)
        {
            if (GetTopCardOnTile(x, y + 1).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x, y + 1).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    return true;
                }
            }
        }

        if (GetTopCardOnTile(x, y - 1) != null)
        {
            if (GetTopCardOnTile(x, y - 1).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x, y - 1).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public List<GameObject> GetAdjacentHeavierEnemyCard(int x, int y, int player)
    {
        List<GameObject> EnemyHeavierCard = new List<GameObject>();
        CardTable card = GetTopCardOnTile(x, y).GetComponent<CardTable>();
        int weightCard = card.MergedWeight.Value == 0 ? card.Weight.Value : card.MergedWeight.Value;

        if (GetHeavierEnemyCard(x - 1, y, player, weightCard))
        {
            EnemyHeavierCard.Add(GetTopCardOnTile(x - 1, y));
        }
        if (GetHeavierEnemyCard(x + 1, y, player, weightCard))
        {
            EnemyHeavierCard.Add(GetTopCardOnTile(x + 1, y));
        }
        if (GetHeavierEnemyCard(x, y - 1, player, weightCard))
        {
            EnemyHeavierCard.Add(GetTopCardOnTile(x, y - 1));
        }
        if (GetHeavierEnemyCard(x, y + 1, player, weightCard))
        {
            EnemyHeavierCard.Add(GetTopCardOnTile(x, y + 1));
        }


        return EnemyHeavierCard;
    }

    private bool GetHeavierEnemyCard(int x, int y, int player, int weightCard)
    {
        if (GetTopCardOnTile(x, y) != null)
        {
            if (GetTopCardOnTile(x, y).GetComponent<CardTable>() != null)
            {
                if (GetTopCardOnTile(x, y).GetComponent<CardTable>().IdOwner.Value != player)
                {
                    int enemyCardWeight = GetTopCardOnTile(x, y).GetComponent<CardTable>().MergedWeight.Value == 0 ? GetTopCardOnTile(x, y).GetComponent<CardTable>().Weight.Value : GetTopCardOnTile(x, y).GetComponent<CardTable>().MergedWeight.Value;
                    if (weightCard < enemyCardWeight)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
