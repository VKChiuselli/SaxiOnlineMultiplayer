using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardSnake : CardInterface
{
    GameObject gameManager;
    GameObject gridContainer;
    int x = 0;
    int y = 0;
    //int deployCostEffectCard =1;
    int moveCostEffectCard = 2;
    GameObject SpawnManager;
    void Start()
    {
        SpawnManager = GameObject.Find("Managers/SpawnManager");
        gameManager = GameObject.Find("Managers/GameManager");
        keyword1 = CardKeyword.ACTIVEEFFECT;
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
    }

    public override void MyCardEffect()
    {
    
        if (gameManager.GetComponent<GameManager>().GetCurrentPlayerMovePoint() < moveCostEffectCard)
        {
            return;
        }//IF pass this control, it means we have enough move points
        UpdateVariable();
        List<GameObject> heavyerCards = CheckHeavierCardAround();
        heavyerCards = CheckBehind(x, y, heavyerCards);
        if (heavyerCards.Count > 0)
        {
            gridContainer.GetComponent<GridContainer>().ShowTileToInteract(heavyerCards);
            gameManager.GetComponent<GameManager>().SetIsPickingChoosing(1);
            EventsManager.current.SelectCardFromTable(gameObject.transform.parent.gameObject);
            //now the player will choose the card to go behind
        }
        else
        {
            return; //DO NOTHING because no cards are there to active the effect
        }

    }

    private List<GameObject> CheckBehind(int x, int y, List<GameObject> heavyerCards)
    {
        List<GameObject> heavyerCardFreeBehind = new List<GameObject>();
        foreach (GameObject enemyCard in heavyerCards)
        {
            if (gridContainer.GetComponent<GridContainer>()
                .GetNextTileType(enemyCard.GetComponent<CardTable>().CurrentPositionX.Value, enemyCard.GetComponent<CardTable>().CurrentPositionY.Value) == 1)
            {
                heavyerCardFreeBehind.Add(enemyCard);
            }
        }


        return heavyerCardFreeBehind;
    }

    private List<GameObject> CheckHeavierCardAround()
    {
        List<GameObject> heavyerCards = gridContainer.GetComponent<GridContainer>().GetAdjacentHeavierEnemyCard(x, y, gameManager.GetComponent<GameManager>().CurrentTurn.Value);
        return heavyerCards;
    }

    public override void MyCardCostEffect(GameObject card)
    {
        //TODO you must implement here the card that goes after the other one
        GameObject newTileJumped = JumpTwoTile(x, y, card.GetComponent<CardTable>().CurrentPositionX.Value, card.GetComponent<CardTable>().CurrentPositionY.Value);
        int newX = newTileJumped.GetComponent<CoordinateSystem>().x;
        int newY = newTileJumped.GetComponent<CoordinateSystem>().y;


        SpawnManager.GetComponent<SpawnCardServer>().TeleportCardsToEmptyTileServerRpc(x, y, newX, newY, moveCostEffectCard, 1);

    }

    private GameObject JumpTwoTile(int oldX, int oldY, int newX, int newY)
    {
        return gridContainer.GetComponent<GridContainer>().GetNextTile(oldX, oldY, newX, newY);
    }

    private void UpdateVariable()
    {
        x = gameObject.transform.parent.gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
        y = gameObject.transform.parent.gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
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

}
