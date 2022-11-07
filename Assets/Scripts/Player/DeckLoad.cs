using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeckLoad : NetworkBehaviour
{
    [SerializeField] private GameObject CardOne;
    [SerializeField] private GameObject CardTwo;
    [SerializeField] private GameObject CardThree;
    [SerializeField] private GameObject CardFour;
    [SerializeField] private GameObject CardFive;
    [SerializeField] private GameObject CardSix;

    void Start()
    {
        //if (gameObject.transform.GetChild(0).gameObject != null)
        //{
        //    CardOne = gameObject.transform.GetChild(0).gameObject.transform.GetChild(8).gameObject;
        //}
        //if (gameObject.transform.GetChild(1).gameObject != null)
        //{
        //    CardTwo = gameObject.transform.GetChild(1).gameObject.transform.GetChild(8).gameObject;
        //}
        //if (gameObject.transform.GetChild(2).gameObject != null)
        //{
        //    CardThree = gameObject.transform.GetChild(2).gameObject.transform.GetChild(8).gameObject;
        //}
        //if (gameObject.transform.GetChild(3).gameObject != null)
        //{
        //    CardFour = gameObject.transform.GetChild(3).gameObject.transform.GetChild(8).gameObject;
        //}
    }

    public GameObject GetCardGameObject(int IdCard)
    {

        GameObject cardHand = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<CardHand>() != null)
            {
                if (transform.GetChild(i).gameObject.GetComponent<CardHand>().IdCard.Value == IdCard)
                {
                    cardHand = transform.GetChild(i).gameObject;
                    return cardHand;
                }
            }
        }

        Debug.Log("ERRRRORRR no card found from hand");

        return cardHand;
    }
    public CardHand GetCardHand(int IdCard)
    {
        CardHand cardHand = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<CardHand>() != null)
            {
                if (transform.GetChild(i).gameObject.GetComponent<CardHand>().IdCard.Value == IdCard)
                {
                    cardHand = transform.GetChild(i).gameObject.GetComponent<CardHand>();
                    return cardHand;
                }
            }
        }

        Debug.Log("ERRRRORRR no card found from hand");

        return cardHand;
    }


    public GameObject GetIndexCard(int index)
    {
        if (index == 0)
        {
            return CardOne;
        }
        if (index == 1)
        {
            return CardTwo;
        }
        if (index == 2)
        {
            return CardThree;
        }
        if (index == 3)
        {
            return CardFour;
        }
        Debug.Log("DeckLoad, method GetCard: No card found! Error!!");
        return null;
    }

}
