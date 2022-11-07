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
    GameObject deckManager;
    GameObject SpawnManager;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
        SpawnManager = GameObject.Find("Managers/SpawnManager");
        deckManager = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
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
        bool IsSingleCard = true;
        //check if the tile chosed is filled by a card or is an empty tile
        if (gameObject.GetComponent<CardTable>() != null)
        {
            IsSingleCard = false;
        }

        if (IsSingleCard)
        {
            if (player == 0)
            {
                MoveCardFromTableOnEmptySpace();
            }
            else if (player == 1)
            {
                MoveCardFromTableOnEmptySpace();
            }
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
        }
        else if (!IsSingleCard)
        {
            if (player == 0)
            {
                MoveCardFromTableOnFilledSpace();
            }
            else if (player == 1)
            {
                MoveCardFromTableOnFilledSpace();
            }
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
        }
    }


    private bool MoveCardFromTableOnFilledSpace()
    {
        if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                                                     //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
            {
                SpawnManager.GetComponent<SpawnCardServer>().MoveToFriendlyTileServerRpc(
                        placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
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
        else if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)                                                     //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
            {
                SpawnManager.GetComponent<SpawnCardServer>().PushCardFromTable(
                        placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
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


    private bool MoveCardFromTableOnEmptySpace()
    {
        if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                         //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
            {
                SpawnManager.GetComponent<SpawnCardServer>().MoveAllCardsToEmptyTileServerRpc(
placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
gameObject.GetComponent<CoordinateSystem>().x,
gameObject.GetComponent<CoordinateSystem>().y,
false
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

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }

}
