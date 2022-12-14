using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickCard : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{

    private Vector3 mOffset;
    private float mZCoord;
    GameObject gridContainer;
    GameObject gameManager;
    PlaceManager placeManager;
    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (gameManager.GetComponent<GameManager>().IsRunningPlayer())
            {

                if (gameManager.GetComponent<GameManager>().IsPickingChoosing.Value == 1)
                {
                    if (gameObject.transform.parent.GetComponent<CoordinateSystem>() != null)//it means that is empty tile 
                    {
                        if (gameObject.transform.parent.GetComponent<CoordinateSystem>().typeOfTile == 7)
                        {
                            TriggerCardSelected();
                        }
                    }
                    return;
                }
            } 
            
            if (gameManager.GetComponent<GameManager>().IsRunningPlayer())
            {

                if (gameManager.GetComponent<GameManager>().IsPopupChoosing.Value == 1)
                {
                    if (gameObject.transform.parent.GetComponent<CoordinateSystem>() != null)//it means that is empty tile 
                    {
                        if (gameObject.transform.parent.GetComponent<CoordinateSystem>().typeOfTile == 7)
                        {
                            gameManager.GetComponent<GameManager>().SetIsPopupChoosing(0);
                            PickCardFromTable();
                        }
                    }
                    return;
                }
            }


            if (gameManager.GetComponent<GameManager>().IsRunningPlayer())
            {

                if (gameManager.GetComponent<GameManager>().IsUnmergeChoosing.Value == 0 && gameManager.GetComponent<GameManager>().IsPopupChoosing.Value == 0)
                {
                    if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0 && gameObject.tag == "RPCH")// RPCH stands for left  player card hand
                    {
                        PickCardFromHand("DeployTileRight");
                    }
                    else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1 && gameObject.tag == "LPCH")// LPCH stands for left player card hand
                    {
                        PickCardFromHand("DeployTileLeft");
                    }
                    else if (gameObject.GetComponent<CardTable>() != null)
                    {
                        if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1 && gameObject.GetComponent<CardTable>().IdOwner.Value == 1)// LPCT stands for left player card table
                        {
                            PickCardFromTable();
                        }
                        else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0 && gameObject.GetComponent<CardTable>().IdOwner.Value == 0)// RPCT stands for right player card table
                        {//la questione ?: io clicco la carta, compare un popup in cui ho due possibilit?: il merge o select della carta 
                            PickCardFromTable();
                        }
                    }

                }
            }
        }
     
    }

    private Vector3 GetMouseAsWorldPoint()
    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen

        mousePoint.z = mZCoord;

        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    private void PickCardFromHand(string DeployTile)
    {
        ResetShowTilesClientRpc();
        if (placeManager.GetCardSelectedFromHand() != null)
        {
            if (gameObject.name == placeManager.GetCardSelectedFromHand().name)
            {
                placeManager.ResetCardHand();
                return;
            }
        }
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        EventsManager.current.PickCardFromHand(gameObject);
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        Debug.Log("Card selected from hand: " + gameObject.name);
        placeManager.ResetMergedCardTable();
        placeManager.ResetSingleCardTable();
        ShowTilesToDeployClientRpc(DeployTile);

    }

    private void TriggerCardSelected()
    {
        if (gameManager.GetComponent<GameManager>().IsPickingChoosing.Value == 1)
        {
            ResetShowTilesClientRpc();
            var cardInterface = placeManager.GetCardSelectedFromTable(); //il placemanager ha immagazzinato questa carta
            var eventManager = new EventsManager();
            gameManager.GetComponent<GameManager>().SetIsPickingChoosing(0);
            eventManager.onSelectCard += cardInterface.MyCardCostEffect; //mi metto in ascolto dell'effetto
            eventManager.SelectCard(gameObject); //triggero l'effetto
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
            placeManager.ResetCardHand();
            // eventManager.onSelectCard -= cardInterface.MyCardCostEffect;
            return;
        }
    }

    private void PickCardFromTable()
    {

        ResetShowTilesClientRpc();
        if (placeManager.GetMergedCardSelectedFromTable() != null)
        {
            if (gameObject.transform.parent.name == placeManager.GetMergedCardSelectedFromTable().transform.parent.name)
            {
                placeManager.ResetMergedCardTable();
                return;
            }
        }
        if (placeManager.GetSingleCardSelectedFromTable() != null)
        {
            if (gameObject.transform.parent.name == placeManager.GetSingleCardSelectedFromTable().transform.parent.name)
            {
                placeManager.ResetSingleCardTable();
                return;
            }
        }

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        if (gameObject.transform.parent.childCount == 1)
        {
            EventsManager.current.PickSingleCardFromTable(gameObject);
            gameManager.GetComponent<GameManager>().SetIsPopupChoosing(1);
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
        }
        else if (gameObject.transform.parent.childCount > 1)
        {
            EventsManager.current.PickMergedCardFromTable(gameObject);
            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(1);
            placeManager.ResetCardHand();
            placeManager.ResetSingleCardTable();
        }
        else
        {
            Debug.Log("Error! Class pick card, method  PickCardFromTable");
        }
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        Debug.Log("Card selected from table: " + gameObject.GetComponent<CardTable>().IdImageCard.Value);
        ShowTilesAround(false); //turn false to gameobject.component<TableCard>().isSpecial, to check if cards have special movements
    }

    private void ShowTilesAround(bool cardHasSpecialAbility)
    {
        if (cardHasSpecialAbility)
        {
            //TODO special movements card
        }
        else
        {
            gridContainer.GetComponent<GridContainer>().ShowMovementTilesAroundCard(gameObject);
        }
    }


    private void ResetShowTilesClientRpc()
    {
        if (gridContainer.GetComponent<GridContainer>().gridTiles != null)
        {
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
        }
        else
        {
            Debug.Log("ResetShowTilesClientRpc doens't found any Highlight");
        }

    }

    private void ShowTilesToDeployClientRpc(string DeployTile)
    {
        if (gridContainer.GetComponent<GridContainer>().gridTiles != null)
        {
            foreach (GameObject a in gridContainer.GetComponent<GridContainer>().GetDeployTiles(DeployTile))
            {
                a.GetComponent<Highlight>().ShowTileCanInteract(1);
            }
        }
        else
        {
            Debug.Log("ShowTilesToDeployClientRpc doens't found any Highlight");
        }

    }


    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //  Debug.Log("end drag");
    }


}
