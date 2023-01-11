using Assets.Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionLoad : MonoBehaviour
{
    GameObject panelHand;
    List<Image> cardsImages;
   // List<GameObject> cardShowedList;
    void Start()
    {
        cardsImages = new List<Image>();
        panelHand = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
        RetriveImagedFromCards();

    }

    private void RetriveImagedFromCards()
    {

        foreach(Transform card in panelHand.transform)
        {
            if (card.GetComponent<Image>() != null)
            {
            cardsImages.Add(card.gameObject.GetComponent<Image>());
            } 
        }

        ShowCardsToUI();
    }

    private void ShowCardsToUI()
    {
        for(int i=0; i < cardsImages.Count; i++)
        {
            GameObject cardToShow = Instantiate(new GameObject(), transform);
            cardToShow.AddComponent<Image>();
            cardToShow.AddComponent<CollectionInteractionCard>();
            cardToShow.GetComponent<Image>().sprite = cardsImages[i].sprite;
           // cardShowedList.Add(cardToShow);
        }

        //if(cardShowedList.Count > 0)
        //{

        //}
    }
}
