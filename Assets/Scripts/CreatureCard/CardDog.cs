using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDog : CardInterface
{

    GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("CoreGame/Managers/GameManager");
        keyword1 = CardKeyword.ETB;

    }

    public override void MyCardEffect()
    {
        Debug.Log("Gain 1 move point");
        gameManager.GetComponent<GameManager>().MovePointIncrease(1);
    }

}
