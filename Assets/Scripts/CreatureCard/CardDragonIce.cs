using Assets.Scripts;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardDragonIce : CardInterface
{
    GameObject gameManager;
    GameObject gridContainer;
    PlaceManager placeManager;

    void Start()
    {
        gameManager = GameObject.Find("Managers/GameManager");
        keyword1 = CardKeyword.DEPLOYCONDITION;
        keyword2 = CardKeyword.ETB;
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        placeManager = FindObjectOfType<PlaceManager>();
    }

    public override void MyCardCostEffect(GameObject card)
    {
        //remove card and put in hand
        MyCardCostEffectServerRpc(card.GetComponent<CardTable>().CurrentPositionX.Value, card.GetComponent<CardTable>().CurrentPositionY.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MyCardCostEffectServerRpc(int x, int y)
    {
        GameObject card = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x,y);
     
        gridContainer.GetComponent<GridContainer>().RemoveFirstMergedCardFromTable(x,y);
        gameManager.GetComponent<GameManager>().AddCardCopyOnHand(card.GetComponent<CardTable>().IdCard.Value, 1);
        Debug.Log("Card added to hand--> " + card.GetComponent<CardTable>().IdCard.Value);
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

    public override void MyCardEffect()
    {
        Debug.Log("DragonIce MyCardEffect");

        MyCardEffectClientRpc();

    }

    [ClientRpc] //TODO improve connection with client, I must understand which and how the client is called from the server, we need a specific client, not every client 
    void MyCardEffectClientRpc()
    {
        if (IsClient)
        {
            List<GameObject> list = gridContainer.GetComponent<GridContainer>().GetHalfBoardCard(gameManager.GetComponent<GameManager>().CurrentTurn.Value);
            gridContainer.GetComponent<GridContainer>().ShowTileToInteract(list);
            gameManager.GetComponent<GameManager>().SetIsPickingChoosing(1);
            EventsManager.current.SelectCardFromTable(gameObject.transform.parent.gameObject);
            //          GameObject cardSelectFromTable = WaitingForSelectCard().GetAwaiter().GetResult();


            Debug.Log("PICK UR CARD: "); //+ cardSelectFromTable.name);
        }
    }

    //async Task<GameObject> WaitingForSelectCard()
    //{
    

    //    Debug.Log("TROVAT2");
    //    await Task.Delay(100);
    //    return placeManager.GetCardSelectedFromTable();
    //}
}
