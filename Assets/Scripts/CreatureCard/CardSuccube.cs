using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardSuccube : CardInterface
{
    GameObject gameManager;
    GameObject gridContainer;
    int x = 0;
    int y = 0;
    int deployCostEffectCard = 1;
    int moveCostEffectCard = 1;
    GameObject SpawnManager;
    void Start()
    {
        SpawnManager = GameObject.Find("CoreGame/Managers/SpawnManager");
        gameManager = GameObject.Find("CoreGame/Managers/GameManager");
        keyword1 = CardKeyword.ACTIVEEFFECT;
        gridContainer = GameObject.Find("CoreGame/CanvasHandPlayer/GridManager");
    }

    public override void MyCardEffect()
    {

        if (gameManager.GetComponent<GameManager>().GetCurrentPlayerMovePoint() < moveCostEffectCard)
        {
            return;
        }//IF pass this control, it means we have enough move points

        if (gameManager.GetComponent<GameManager>().GetCurrentPlayerDeployPoint() < deployCostEffectCard)
        {
            return;
        }//IF pass this control, it means we have enough deploy points

        UpdateVariable();

        //TODO destroy card in the column
        List<GameObject> tilesWithCardsToDestroy = gridContainer.GetComponent<GridContainer>().GetAllTileWithCardInColumn(x);
        if (tilesWithCardsToDestroy.Count < 4)
        {
            return;
        }//IF pass this control, it means there are enough cards

        //destroy the cards in the column
        foreach (GameObject tileToDestroy in tilesWithCardsToDestroy)
        {
            SpawnManager.GetComponent<SpawnCardServer>().
                    DespawnAllCardsFromTileServerRpc(tileToDestroy.GetComponent<CoordinateSystem>().x, tileToDestroy.GetComponent<CoordinateSystem>().y);
        }

    }

    private void UpdateVariable()
    {
        x = gameObject.transform.parent.gameObject.GetComponent<CardTable>().CurrentPositionX.Value;
        y = gameObject.transform.parent.gameObject.GetComponent<CardTable>().CurrentPositionY.Value;
    }

}
