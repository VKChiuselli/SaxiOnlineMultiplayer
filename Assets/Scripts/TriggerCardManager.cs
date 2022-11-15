using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCardManager : MonoBehaviour
{
    GameObject gridContainer;

    void Start()
    {
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
    }

    public void TriggerPushEffect(int x, int y, CardKeyword push)
    {
        GameObject getTile = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        if (getTile.transform.GetChild(2).gameObject.GetComponent<CardInterface>().keyword1 == CardKeyword.PUSH)
            getTile.transform.GetChild(2).gameObject.GetComponent<CardInterface>().MyCardEffect();
    }

    public void TriggerETBEffect(GameObject card)
    {
        if (card.GetComponent<CardInterface>().keyword1 == CardKeyword.ETB)
            card.gameObject.GetComponent<CardInterface>().MyCardEffect();
    }
}
