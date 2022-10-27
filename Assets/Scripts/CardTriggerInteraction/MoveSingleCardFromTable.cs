using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSingleCardFromTable : NetworkBehaviour, IDropHandler
{
    bool isPlaceable;
    bool isCurrentPlayer;
    bool cardSelected;
    private Vector3 mOffset;
    private float mZCoord;
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
    {   if (NetworkManager.Singleton.IsClient && placeManager.GetSingleCardSelectedFromTable() != null) //bisogna mettere molte più condizioni per mettere la carta
        {
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0 && placeManager.GetSingleCardSelectedFromTable() != null)
            {
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value > 0)
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
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1 && placeManager.GetSingleCardSelectedFromTable() != null)//&& (NetworkManager.Singleton.LocalClientId % 2) == 0)
            {//check the max move of the card
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
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
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>() != null)
                {
                    SpawnCardFromServerRpc(
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdCard.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().Weight.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().Speed.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdImageCard.Value.ToString(),
              cardTableTag, //RPT Right player Table
              true, //it means that we have to destroy the old game object when we move
                 gameObject.GetComponent<CoordinateSystem>().x,
              gameObject.GetComponent<CoordinateSystem>().y,
                   placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
              0
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
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! typeOfTile errato");
                return false;
            }
        }
        else
        {
            if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                                                           //togliere ai move points  .GetComponent<CoordinateSystem>().typeOfTile, per questo è maggiore uguale di uno il check
            {
                ChangeOwnerServerRpc();
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>() != null)
                {
                    SpawnCardFromServerRpc(
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdCard.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().Weight.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().Speed.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdImageCard.Value.ToString(),
              cardTableTag, //RPT Right player Table
              true, //it means that we have to destroy the old game object when we move
                 gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
              gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
                   placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
              placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
              1
              );
                    UpdateWeightTopCard(placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().Weight.Value);
                }
                else
                {
                    Debug.Log("Class PlaceCard, method OnPointerDown, Errore! CardHand vuota");
                }

                placeManager.ResetCardHand();
                return true;
            }
            else
                return false;
        }
    }
    private void UpdateWeightTopCard(int cardWeight)
    {
        int finalWeight = cardWeight;
        CardTable cardTable = placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>();

        if (cardTable != null)
        {
            foreach (Transform singleCard in transform.parent)
            {
                if (singleCard.GetComponent<CardTable>() != null)
                {
                    finalWeight += singleCard.GetComponent<CardTable>().Weight.Value;
                }
            }
                Debug.Log("Final weight: " + finalWeight);
            UpdateWeightTopCardServerRpc(finalWeight, gameObject.GetComponent<CardTable>().CurrentPositionX.Value, gameObject.GetComponent<CardTable>().CurrentPositionY.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateWeightTopCardServerRpc(int finalWeight, int x, int y)
    {
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        Debug.Log("1OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("1NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnCardFromServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete, int checkTransform) //MyCardStruct cartaDaSpawnare
    {
        Debug.Log("2OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("2NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        CardTableToSpawn.tag = tag;
        NetworkObject go = null;
        if (checkTransform == 0)
        {
            go = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
          transform.position, Quaternion.identity);
            go.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            go.transform.SetParent(transform, false);
        }
        else if (checkTransform == 1)
        {
            go = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
       transform.parent.position, Quaternion.identity);
            go.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            go.transform.SetParent(transform.parent, false);
        }
        else
        {
            Debug.Log("checkTransform passed wrong!!!");
        }

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
