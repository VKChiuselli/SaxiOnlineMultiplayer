using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDragonIce : CardInterface
{
    GameObject gameManager;
    GameObject gridContainer;
    void Start()
    {
        gameManager = GameObject.Find("Managers/GameManager");
        keyword1 = CardKeyword.DEPLOYCONDITION;
        keyword2 = CardKeyword.COSTEFFECT;
        keyword3 = CardKeyword.EFFECT;
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");

    }

    public override void MyCardEffect()
    {
        Debug.Log("Gain 1 move point");
        gameManager.GetComponent<GameManager>().MovePointIncreaseServerRpc(1);
    }

    public override void MyCardCostEffect()
    {
        Debug.Log("Bounce a creature to my hand");
    }

    public override bool MyCardDeploy(GameObject card)
    {
       if(!(gridContainer.GetComponent<GridContainer>().ExistHalfBoardCard(gameManager.GetComponent<GameManager>().CurrentTurn.Value)))
        {//if  I don't have any friendly creature, I return false
            return false;
        }
        else
        {
            //TODO sequence for the player to pay the cost of the card , put the card in hand and put the card in play
            return true;
        }


    }
}
