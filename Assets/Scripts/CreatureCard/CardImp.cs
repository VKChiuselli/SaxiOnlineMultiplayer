using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardImp : CardInterface
{
    GameObject gameManager;
    GameObject gridContainer;
    int x = 0;
    int y = 0;
    bool conditionVerify = false;
    void Start()
    {
        gameManager = GameObject.Find("Managers/GameManager");
        keyword1 = CardKeyword.CONDITION;
        keyword2 = CardKeyword.EFFECT;
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
    }

    public override void MyCardEffect()
    {
        //IF gridmanager cerca un tile nemico di fianco
        //THAN moveCost == 0

    }

    void Update()
    {
        if (gameObject.transform.parent.gameObject.GetComponent<CardTable>() != null)
        {
            if (gameObject.transform.parent.gameObject.GetComponent<CardTable>().IsOnTop.Value)
            {
                UpdateVariable();
                conditionVerify = CheckCondition();
                ResolveCondition(conditionVerify);
            }
            else
            {//in this way, if the card is not in the top, the move cost is 1
                gameObject.transform.parent.gameObject.GetComponent<CardTable>().ChangeMoveCostServerRpc(1);
            }
            
        }
    }

    private void ResolveCondition(bool condition)
    {
        if (condition)
        {
            gameObject.transform.parent.gameObject.GetComponent<CardTable>().ChangeMoveCostServerRpc(0);
        }
        else
        {
            gameObject.transform.parent.gameObject.GetComponent<CardTable>().ChangeMoveCostServerRpc(1);
        }
    }

    private void UpdateVariable()
    {
            x = gameObject.transform.parent.gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
            y = gameObject.transform.parent.gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
    }

    private bool CheckCondition()
    {
        if (gridContainer.GetComponent<GridContainer>().HasAdjacentEnemyCard(x, y, gameManager.GetComponent<GameManager>().CurrentTurn.Value))
        {
                return true;
        }
        return false;
    }
}
