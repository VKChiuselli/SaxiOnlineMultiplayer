using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTraitor : CardInterface
{
    GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Managers/GameManager");
        keyword1 = CardKeyword.PUSH;

    }

    public override void MyCardEffect()
    {
        Debug.Log("Gain 1 move point");
        gameManager.GetComponent<GameManager>().MovePointIncreaseServerRpc(1);
    }

}
