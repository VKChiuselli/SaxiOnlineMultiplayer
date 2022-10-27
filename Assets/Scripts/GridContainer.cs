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
        if (tile.transform.childCount == 1)
        {//if has only one child it means that the tile is empty, so i put 1 in ShowTileCanDeploy
            tile.GetComponent<Highlight>().ShowTileCanInteract(1);
        }
        else
        {
            if (tile.transform.GetChild(1).gameObject.GetComponent<CardTable>().IdOwner.Value == cardTablePassed.GetComponent<CardTable>().IdOwner.Value)
            {
                tile.GetComponent<Highlight>().ShowTileCanInteract(2);
            }
            else
            {
                tile.GetComponent<Highlight>().ShowTileCanInteract(3);
            }
        }
    }

    public void RemoveCardFromTable(int x, int y)
    {
        foreach (GameObject tile in gridTiles)
        {
            if (tile.GetComponent<CoordinateSystem>().x == x && tile.GetComponent<CoordinateSystem>().y == y)
            {
                Debug.Log("RemoveCardFromTable Found!!");
                //despawn the last children
                tile.transform.GetChild(tile.transform.childCount - 1).gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }
    }
    public void RemoveFirstMergedCardFromTable(int x, int y, int indexCard)
    {
        foreach (GameObject tile in gridTiles)
        {
            if (tile.GetComponent<CoordinateSystem>().x == x && tile.GetComponent<CoordinateSystem>().y == y)
            {
                Debug.Log("RemoveFirstMergedCardFromTable Found!!");
                //despawn the last children
                tile.transform.GetChild(indexCard).gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    public GameObject GetTopCardOnTile(int x, int y)
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
}
