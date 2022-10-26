using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnmergeCard :  NetworkBehaviour, IPointerDownHandler
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
            && placeManager.GetMergedCardSelectedFromTable().transform.parent.GetComponent<CoordinateSystem>().typeOfTile==7) //bisogna mettere molte più condizioni per mettere la carta
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

                            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
                            placeManager.ResetCardHand();
                            placeManager.ResetMergedCardTable();
                            placeManager.ResetSingleCardTable();
                        }
                        Debug.Log("provo a mettermi nella carta amica");
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

                            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
                            placeManager.ResetCardHand();
                            placeManager.ResetMergedCardTable();
                            placeManager.ResetSingleCardTable();
                        }
                    }
                }
            }
         

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
                    SpawnCardFromServerRpc(
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdCard.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().Weight.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().Speed.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdImageCard.Value.ToString(),
              cardTableTag, //RPT Right player Table
              true, //it means that we have to destroy the old game object when we move
                 gameObject.GetComponent<CoordinateSystem>().x,
              gameObject.GetComponent<CoordinateSystem>().y,
                   placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
              0
              );
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
                    SpawnCardFromServerRpc(
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdCard.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().Weight.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().Speed.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdImageCard.Value.ToString(),
              cardTableTag, //RPT Right player Table
              true, //it means that we have to destroy the old game object when we move
                 gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
              gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
                   placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
              placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
              1
              );
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
