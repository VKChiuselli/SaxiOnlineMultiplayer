using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionCard : NetworkBehaviour, IPointerDownHandler
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (NetworkManager.Singleton.IsClient
           && placeManager.GetSingleCardSelectedFromTable() != null
           && gameManager.GetComponent<GameManager>().IsPopupChoosing.Value == 1) //bisogna mettere molte più condizioni per mettere la carta
        {
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 0)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerZeroMP.Value > 0) //instead of 0 put CARD.MOVEMENT_COST
                    {
                        if (gameObject.GetComponent<CoordinateSystem>() != null)//it means that is empty tile 
                        {
                            if(gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                                    placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                                    placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                                    gameObject.GetComponent<CoordinateSystem>().x,
                                    gameObject.GetComponent<CoordinateSystem>().y,
                                    gameObject.GetComponent<CoordinateSystem>().typeOfTile
                          );
                            }
                        }
                        else //so it is a filled tile
                        {
                            Debug.Log("OPEN POPUP player 0"); //manage point spent
                            if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                          placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                           placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
                           2
                       );
                            }
                            else if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                          placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                           placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
                           3
                       );
                            }
                        }
                    }
                }

            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)//&& (NetworkManager.Singleton.LocalClientId % 2) == 0)
            {//check the max move of the card
                if (placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value > 0) //instead of 0 put CARD.MOVEMENT_COST
                    {
                        if (gameObject.GetComponent<CoordinateSystem>() != null)//it means that is empty tile 
                        {
                            if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                                    placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                                    placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                                    gameObject.GetComponent<CoordinateSystem>().x,
                                    gameObject.GetComponent<CoordinateSystem>().y,
                                    gameObject.GetComponent<CoordinateSystem>().typeOfTile
                          );
                            }
                        }
                        else //so it is a filled tile
                        {
                            Debug.Log("OPEN POPUP player 1"); //manage point spent
                            if(gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2)
                                    {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                          placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                           placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
                           2
                       );
                            }  else if(gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)
                                    {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                          placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                           placeManager.GetSingleCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
                           3
                       );
                            }
                        }
                    }
                }
            }
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
            gameManager.GetComponent<GameManager>().SetIsPopupChoosing(0);
        }




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
                        if (gameObject.GetComponent<CoordinateSystem>() != null)//it means that is empty tile 
                        {
                            if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                                    placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                                    placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                                    gameObject.GetComponent<CoordinateSystem>().x,
                                    gameObject.GetComponent<CoordinateSystem>().y,
                                    gameObject.GetComponent<CoordinateSystem>().typeOfTile
                          );
                            }
                        }
                        else //so it is a filled tile
                        {
                            Debug.Log("OPEN POPUP player 0"); //manage point spent
                            if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                          placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                           placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
                           2
                       );
                            }
                            else if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                          placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                           placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
                           3
                       );
                            }
                        }
                    }
                }

            } else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
            {
                if (placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value == 1)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerOneMP.Value > 0) //instead of 0 put CARD.MOVEMENT_COST
                    {
                        if (gameObject.GetComponent<CoordinateSystem>() != null)//it means that is empty tile 
                        {
                            if (gameObject.GetComponent<CoordinateSystem>().typeOfTile == 1)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                                    placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                                    placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                                    gameObject.GetComponent<CoordinateSystem>().x,
                                    gameObject.GetComponent<CoordinateSystem>().y,
                                    gameObject.GetComponent<CoordinateSystem>().typeOfTile
                          );
                            }
                        }
                        else //so it is a filled tile
                        {
                            Debug.Log("OPEN POPUP player 1"); //manage point spent
                            if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 2)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                          placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                           placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
                           2
                       );
                            }
                            else if (gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().typeOfTile == 3)
                            {
                                gameManager.GetComponent<GameManager>().OpenPopupUI(
                          placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
                           placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionX.Value,
                           gameObject.GetComponent<CardTable>().CurrentPositionY.Value,
                           3
                       );
                            }
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
                    SpawnManager.GetComponent<SpawnCardServer>().MoveTopCardToAnotherTileServerRpc(
                   cardTable.CurrentPositionX.Value,
                   cardTable.CurrentPositionY.Value,
                      gameObject.GetComponent<CoordinateSystem>().x,
              gameObject.GetComponent<CoordinateSystem>().y
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
                    CardTable cardTable = placeManager.GetMergedCardSelectedFromTable().GetComponent<CardTable>();
                    SpawnManager.GetComponent<SpawnCardServer>().MoveTopCardToAnotherTileServerRpc(
                            cardTable.CurrentPositionX.Value,
                   cardTable.CurrentPositionY.Value,
                 gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
                 gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y
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
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }

}
