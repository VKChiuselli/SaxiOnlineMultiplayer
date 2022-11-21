using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveCard : MonoBehaviour, IPointerClickHandler
{
    PlaceManager placeManager;
    GameObject gridContainer;
    GameObject gameManager;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");

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
                    gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
                    gameManager.GetComponent<GameManager>().SetIsPopupChoosing(0);
                    gameObject.transform.GetChild(2).GetComponent<CardInterface>().MyCardEffect();
                }
            }
        }

    }
}
