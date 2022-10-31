using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeckLoad : NetworkBehaviour
{
    private GameObject CardOne;
    private GameObject CardTwo;
    private GameObject CardThree;
    private GameObject CardFour;
    private GameObject CardFive;
    private GameObject CardSix;

    void Start()
    {
        if (gameObject.transform.GetChild(0).gameObject != null)
        {
            CardOne = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        }
        if (gameObject.transform.GetChild(1).gameObject != null)
        {
            CardTwo = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        }
        if (gameObject.transform.GetChild(2).gameObject != null)
        {
            CardThree = gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        }
        if (gameObject.transform.GetChild(3).gameObject != null)
        {
            CardFour = gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
        }
    }

    
    public GameObject GetCard(int index)
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
