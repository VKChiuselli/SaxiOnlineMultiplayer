using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardSkeleton : CardInterface
{
    GameObject gameManager;
    GameObject gridContainer;
    int x = 0;
    int y = 0;
    int moveCostEffectCard = 3;
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

        List<GameObject> friendlyTiles = gridContainer.GetComponent<GridContainer>().
            GetTilesFromPlayer(gameManager.GetComponent<GameManager>().CurrentTurn.Value);

        if (friendlyTiles.Count <= 1)
        {
            return;
        }//if pass this control it means we have more than one creature

        List<GameObject> friendlyTilesLessThis = new List<GameObject>();

        foreach (GameObject tile in friendlyTiles)
        {
            if (tile.GetComponent<CoordinateSystem>().x == x && tile.GetComponent<CoordinateSystem>().y == y)
            {
               //nothing
            }
            else
            {
                friendlyTilesLessThis.Add(tile);
            }
        }

        gridContainer.GetComponent<GridContainer>().ShowTileToInteractByTile(friendlyTilesLessThis);
        gameManager.GetComponent<GameManager>().SetIsPickingChoosing(1);
        EventsManager.current.SelectCardFromTable(gameObject.transform.parent.gameObject);

    }

    private void UpdateVariable()
    {
        x = gameObject.transform.parent.gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
        y = gameObject.transform.parent.gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
    }

    public override void MyCardCostEffect(GameObject _cardTable)
    {
        CardTable cardTable = _cardTable.GetComponent<CardTable>();

        if (cardTable.IdOwner.Value == gameObject.transform.parent.gameObject.GetComponent<CardTable>().IdOwner.Value)
        {

            int weightFriendlyCardTableSacrificed = cardTable.MergedWeight.Value == 0 ? cardTable.Weight.Value : cardTable.MergedWeight.Value;

            //chiamo una funzione che  ritorna tutti i tile memici che pesano meno 
            int oppoTurn = gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0 ? 1 : 0;

            List<GameObject> enemyTiles = gridContainer.GetComponent<GridContainer>().GetTilesFromPlayer(oppoTurn);

            List<GameObject> enemyTilesLessHeavier = new List<GameObject>();


            foreach (GameObject tile in enemyTiles)
            {
                int tileWeight = gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTile(tile.GetComponent<CoordinateSystem>().x, tile.GetComponent<CoordinateSystem>().y);
                if (tileWeight < weightFriendlyCardTableSacrificed)
                {
                    enemyTilesLessHeavier.Add(tile);
                }
            }

            if (enemyTilesLessHeavier.Count <= 0)
            {
                return;
            }//se non ho trovato nessuna carta, despawno



            //se esiste almeno una, despawno la carta amica
            Debug.Log("Card sacrificed from skeleton");
            SpawnManager.GetComponent<SpawnCardServer>().DespawnAllCardsFromTileServerRpc
                         (cardTable.GetComponent<CardTable>().CurrentPositionX.Value,
                         cardTable.GetComponent<CardTable>().CurrentPositionY.Value);

            //attivo nuovamente il choosing 
            gameManager.GetComponent<GameManager>().SetIsPickingChoosing(1);
            EventsManager.current.SelectCardFromTable(gameObject.transform.parent.gameObject);
            //attivo tutti i tile delle carte nemiche che pesano meno  con showTile
            gridContainer.GetComponent<GridContainer>().ShowTileToInteractByTile(enemyTilesLessHeavier);
        }
        else
        {
            gameManager.GetComponent<GameManager>().MovePointSpent(moveCostEffectCard);
            Debug.Log("Card destroyed from skeleton");
            SpawnManager.GetComponent<SpawnCardServer>().DespawnAllCardsFromTileServerRpc
                       (cardTable.GetComponent<CardTable>().CurrentPositionX.Value,
                       cardTable.GetComponent<CardTable>().CurrentPositionY.Value);
        }

    }

}
