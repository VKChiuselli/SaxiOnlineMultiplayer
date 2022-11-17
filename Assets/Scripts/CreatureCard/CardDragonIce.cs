using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardDragonIce : CardInterface
{
    GameObject gameManager;
    GameObject gridContainer;


    void Start()
    {
        gameManager = GameObject.Find("Managers/GameManager");
        keyword1 = CardKeyword.DEPLOYCONDITION;
        keyword2 = CardKeyword.ETB;
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");

    }

    public override void MyCardEffect()
    {
        Debug.Log("Add a the creature to my hand");

        MyCardEffectClientRpc();

    }

    public override void MyCardCostEffect()
    {
        Debug.Log("Bounce a creature to my hand");
    }

    public override bool MyCardDeploy(GameObject card)
    {
        if (!(gridContainer.GetComponent<GridContainer>().ExistHalfBoardCard(gameManager.GetComponent<GameManager>().CurrentTurn.Value)))
        {//if  I don't have any friendly creature, I return false
            return false;
        }
        else
        {
            //TODO sequence for the player to pay the cost of the card , put the card in hand and put the card in play
            MyCardEffect();
            return true;
        }
    }

    [ClientRpc] //TODO improve connection with client, I must understand which and how the client is called from the server, we need a specific client, not every client 
    void MyCardEffectClientRpc()
    {
        if (IsClient)
        {
            List<GameObject> list = gridContainer.GetComponent<GridContainer>().GetHalfBoardCard(gameManager.GetComponent<GameManager>().CurrentTurn.Value);
            gridContainer.GetComponent<GridContainer>().ShowTileToInteract(list);
            Debug.Log("PICK UR CARD");
        }
    }

}
