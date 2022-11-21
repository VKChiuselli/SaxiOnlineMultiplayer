using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveCard : MonoBehaviour, IPointerClickHandler
{
    PlaceManager placeManager;
    GameObject gridContainer;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (NetworkManager.Singleton.IsClient)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (gameObject.transform.GetChild(2).GetComponent<CardInterface>().keyword1 == CardKeyword.ACTIVEEFFECT)
                {
                    placeManager.ResetCardHand();
                    placeManager.ResetMergedCardTable();
                    placeManager.ResetSingleCardTable();
                    gridContainer.GetComponent<GridContainer>().ResetShowTiles();
                    gameObject.transform.GetChild(2).GetComponent<CardInterface>().MyCardEffect();
                }
            }
        }

    }
}
