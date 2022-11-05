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
                            CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();
                            int xNewTile = gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
                            int yNewTile = gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
                            SpawnManager.GetComponent<SpawnCardServer>().PushCardFromTable(
                                cardTable.CurrentPositionX.Value,
                                cardTable.CurrentPositionY.Value,
                                xNewTile,
                                yNewTile
                                );
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
                            CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();
                            int xNewTile = gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
                            int yNewTile = gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
                            SpawnManager.GetComponent<SpawnCardServer>().PushCardFromTable(
                                cardTable.CurrentPositionX.Value,
                                cardTable.CurrentPositionY.Value,
                                xNewTile,
                                yNewTile
                                );
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
                            CardTable cardTable = placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>();
                            int xNewTile = gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
                            int yNewTile = gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
                            SpawnManager.GetComponent<SpawnCardServer>().PushCardFromTable(
                                cardTable.CurrentPositionX.Value,
                                cardTable.CurrentPositionY.Value,
                                xNewTile,
                                yNewTile
                                );
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
                            CardTable cardTable = placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>();
                            int xNewTile = gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
                            int yNewTile = gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
                            SpawnManager.GetComponent<SpawnCardServer>().PushCardFromTable(
                                cardTable.CurrentPositionX.Value,
                                cardTable.CurrentPositionY.Value,
                                xNewTile,
                                yNewTile
                                );
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

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }


}
