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

    public void ShowMovementTilesAroundCard(GameObject gameObject)
    {
        if (gridTiles != null)
        {

            int x = gameObject.transform.parent.GetComponent<CoordinateSystem>().x;
            int y = gameObject.transform.parent.GetComponent<CoordinateSystem>().y;

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
                    tile.GetComponent<Highlight>().ShowTileCanDeploy();
                }
                else
                if (tile.GetComponent<CoordinateSystem>().x == leftX && tile.GetComponent<CoordinateSystem>().y == leftY)
                {
                    tile.GetComponent<Highlight>().ShowTileCanDeploy();
                }
                else
                if (tile.GetComponent<CoordinateSystem>().x == downX && tile.GetComponent<CoordinateSystem>().y == downY)
                {
                    tile.GetComponent<Highlight>().ShowTileCanDeploy();
                }
                else
                if (tile.GetComponent<CoordinateSystem>().x == rightX && tile.GetComponent<CoordinateSystem>().y == rightY)
                {
                    tile.GetComponent<Highlight>().ShowTileCanDeploy();
                }
            }
        }
        else
        {
            Debug.Log("ShowMovementTilesAroundCard doens't found any grid tile");
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
                tile.transform.GetChild(tile.transform.childCount-1).gameObject.GetComponent<NetworkObject>().Despawn();
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
}
