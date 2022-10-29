using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnmergeCard : NetworkBehaviour, IPointerDownHandler
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (NetworkManager.Singleton.IsClient
           && placeManager.GetMergedCardSelectedFromTable() != null
           && gameManager.GetComponent<GameManager>().IsUnmergeChoosing.Value == 1) //bisogna mettere molte più condizioni per mettere la carta
        {
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value > 0) //instead of 0 put CARD.MOVEMENT_COST
                    {

                        bool isPlayed = MoveCardFromTable("RPCT");
                        if (isPlayed)
                        {
                            Debug.Log("punto sottratto PlayerZero move");
                            gameManager.GetComponent<GameManager>().MovePointSpent(1, 0);
                        }
                    }
                }

            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)//&& (NetworkManager.Singleton.LocalClientId % 2) == 0)
            {//check the max move of the card
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value > 0)
                    {

                        Debug.Log("punto sottratto PlayerOne move");

                        bool isPlayed = MoveCardFromTable("LPCT");
                        if (isPlayed)
                        {
                            gameManager.GetComponent<GameManager>().MovePointSpent(1, 1);
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

    private bool MoveCardFromTable(string cardTableTag)
    {//if it is gridManager, it means it is empty space on the grid otherwise is a card already existing
        if (gameObject.transform.parent.name == "GridManager")
        {
            if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                             //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
            {
                ChangeOwnerServerRpc();
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
                {
                    CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();
                    MoveCardFromTableOnEmptySpaceServerRpc(
              cardTable.IdCard.Value,
              cardTable.Weight.Value,
              cardTable.Speed.Value,
              cardTable.IdOwner.Value,
              cardTable.IdImageCard.Value.ToString(),
              cardTableTag, //RPT Right player Table
              true, //it means that we have to destroy the old game object when we move
                 gameObject.GetComponent<CoordinateSystem>().x,
              gameObject.GetComponent<CoordinateSystem>().y,
                   cardTable.CurrentPositionX.Value,
                   cardTable.CurrentPositionY.Value
              );
                    //     UpdateWeightTopCard(placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().Weight.Value, placeManager.GetMergedCardSelectedFromTable().transform.parent.GetComponent<CoordinateSystem>().x, placeManager.GetMergedCardSelectedFromTable().transform.parent.GetComponent<CoordinateSystem>().y);
                    UpdateWeightTopCardLeft(gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTileLessLastOne(cardTable.CurrentPositionX.Value, cardTable.CurrentPositionY.Value), cardTable.CurrentPositionX.Value, cardTable.CurrentPositionY.Value);
                }
                else
                {
                    Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");
                }

                placeManager.ResetMergedCardTable();
                return true;
            }
            else
                return false;
        }
        else
        {
            if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                                                         //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
            {
                ChangeOwnerServerRpc();
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>() != null)
                {
                    CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();
                    SpawnCardOnFilledSpaceFromServerRpc(
              cardTable.IdCard.Value,
              cardTable.Weight.Value,
              cardTable.Speed.Value,
              cardTable.IdOwner.Value,
              cardTable.IdImageCard.Value.ToString(),
              cardTableTag, //RPT Right player Table
              true, //it means that we have to destroy the old game object when we move
                 gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
                 gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
                   cardTable.CurrentPositionX.Value,
                   cardTable.CurrentPositionY.Value
              );
                    UpdateWeightTopCardLeft(gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTileLessLastOne(cardTable.CurrentPositionX.Value, cardTable.CurrentPositionY.Value), cardTable.CurrentPositionX.Value, cardTable.CurrentPositionY.Value);
                    UpdateWeightTopCard(gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTile(gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x, gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y), gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x, gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y);
                }
                else
                {
                    Debug.Log("Class PlaceCard, method OnPointerDown, Errore! CardHand vuota");
                }

                placeManager.ResetMergedCardTable();
                return true;
            }
            else
                return false;
        }
    }

    private int GetWeightCardBelowTop(GameObject cardOnTop)
    {
        int finalWeight = 0;
        GameObject cardBelow = gridContainer.GetComponent<GridContainer>().GetBelowCard(cardOnTop.transform.parent.GetComponent<CoordinateSystem>().x, cardOnTop.transform.parent.GetComponent<CoordinateSystem>().y);
        if (cardBelow != null)
        {
            finalWeight = cardBelow.GetComponent<CardTable>().Weight.Value;
        }
        //TODO prendere il padre della carta, iterare la carta fino a trovare il secondo figlio cardOnTop.transform.parent
        return finalWeight;
    }

    private void UpdateWeightTopCard(int cardWeight, int x, int y) //params: the coordinate (x,y) where the update of the top card should go and the weight of the top card i'm going to update
    {
        CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();
        cardWeight = cardTable.Weight.Value + cardWeight;
        UpdateWeightTopCardServerRpc(cardWeight, x, y);
    }

    private void UpdateWeightTopCardLeft(int cardWeight, int x, int y) //params: the coordinate (x,y) where the update of the top card should go and the weight of the top card i'm going to update
    {
        UpdateWeightTopCardLeftServerRpc(cardWeight, x, y);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateWeightTopCardServerRpc(int finalWeight, int x, int y)
    {
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateWeightTopCardLeftServerRpc(int finalWeight, int x, int y)
    {
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetBelowCard(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }


    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }


    [ServerRpc(RequireOwnership = false)]
    public void MoveCardFromTableOnEmptySpaceServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete) //MyCardStruct cartaDaSpawnare
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
        //  gameManager.GetComponent<GameManager>().CurrentTurn.Value = (gameManager.GetComponent<GameManager>().CurrentTurn.Value==1 ? 0 : 1);
        if (toDestroy)
        {
            gridContainer.GetComponent<GridContainer>().RemoveCardFromTable(xToDelete, yToDelete);
        }

    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnCardOnFilledSpaceFromServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete) //MyCardStruct cartaDaSpawnare
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
        //  gameManager.GetComponent<GameManager>().CurrentTurn.Value = (gameManager.GetComponent<GameManager>().CurrentTurn.Value==1 ? 0 : 1);
        if (toDestroy)
        {
            gridContainer.GetComponent<GridContainer>().RemoveCardFromTable(xToDelete, yToDelete);
        }

    }

}
