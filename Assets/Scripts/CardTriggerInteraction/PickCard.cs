using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickCard : MonoBehaviour, IPointerDownHandler
{

    private Vector3 mOffset;
    private float mZCoord;
    GameObject handZone;
    GameObject gridContainer;
    void Start()
    {

        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
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

    bool isSelected;

    public bool GetIsSelected()
    {
        return isSelected;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("piccato1");

        if (NetworkManager.Singleton.IsClient)
        {
            if ((NetworkManager.Singleton.LocalClientId % 2) == 1 && gameObject.tag == "RPCH")// RPCH stands for left  player card hand
            {
                PickCardFromHandRightPlayer();
            }
            else if ((NetworkManager.Singleton.LocalClientId % 2) == 1 && gameObject.tag == "RPCT")// RPCT stands for right player card table
            {
                PickCardFromTableRightPlayer();
            }
            else if ((NetworkManager.Singleton.LocalClientId % 2) == 0 && gameObject.tag == "LPCH")// LPCH stands for left player card hand
            {
                PickCardFromHandLeftPlayer();
            }
            else if ((NetworkManager.Singleton.LocalClientId % 2) == 0 && gameObject.tag == "LPCT")// LPCT stands for left player card table
            {
                PickCardFromTableLeftPlayer();
            }
        }

    }

    private void PickCardFromTableLeftPlayer()
    {
        ResetShowTilesClientRpc();
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        EventsManager.current.PickCardFromTable(gameObject);
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        Debug.Log("Card selected from table: " + gameObject.name);
    }



    private void PickCardFromTableRightPlayer()
    {
        ResetShowTilesClientRpc();
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        EventsManager.current.PickCardFromTable(gameObject);
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        Debug.Log("Card selected from table: " + gameObject.name);
        ShowTilesAround(false); //turn false to gameobject.component<card>().isSpecial, to check if cards have special movements

    }

    private void ShowTilesAround(bool v)
    {
        if (v)
        {
            //TODO special movements card
        }
        else
        {
            gridContainer.GetComponent<GridContainer>().ShowMovementTilesAroundCard(gameObject);
        }
    }

    private void PickCardFromHandLeftPlayer()
    {

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        EventsManager.current.PickCardFromHand(gameObject);
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        Debug.Log("Card selected from hand: " + gameObject.name);
        ShowTilesToDeployClientRpc();

    }

    private void PickCardFromHandRightPlayer()
    {
        ResetShowTilesClientRpc();
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        EventsManager.current.PickCardFromHand(gameObject);
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        Debug.Log("Card selected from hand: " + gameObject.name);
        ShowTilesToDeployClientRpc();

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

    private void ShowTilesToDeployClientRpc()
    {
        if (gridContainer.GetComponent<GridContainer>().gridTiles != null)
        {
            foreach (GameObject a in gridContainer.GetComponent<GridContainer>().GetDeployTilesRight())
            {
                a.GetComponent<Highlight>().ShowTileCanDeploy();
            }
        }
        else
        {
            Debug.Log("ShowTilesToDeployClientRpc doens't found any Highlight");
        }

    }


    // DisableAllHighlight();

    //if (gameObject.transform.GetChild(1) != null)
    //{
    //    gameObject.transform.GetChild(1).gameObject.SetActive(true);
    //}
    private void DisableAllHighlight()
    {
        for (int i = 0; i < handZone.transform.childCount; i++)
        {
            handZone.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
