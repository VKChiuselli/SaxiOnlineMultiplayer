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

    public void TriggerPushEffect(GameObject card)
    {
        if (card.transform.GetChild(2).GetComponent<CardInterface>().keyword1 == CardKeyword.PUSH)
        {
            card.transform.GetChild(2).GetComponent<CardInterface>().MyCardEffect();
        }
    }

    public void TriggerETBEffect(GameObject card)
    {
        if (card.GetComponent<CardInterface>().keyword1 == CardKeyword.ETB)
        {
            card.GetComponent<CardInterface>().MyCardEffect();
        }
    }

    public void TriggerMergeEffect(GameObject card)
    {
        if (card.GetComponent<CardInterface>().keyword1 == CardKeyword.MERGE)
        {
            card.GetComponent<CardInterface>().MyCardEffect();
        }
    }
    public bool TriggerDeployCondition(GameObject card)
    {
        if (card.GetComponent<CardInterface>().keyword1 == CardKeyword.DEPLOYCONDITION)
        {
            return card.GetComponent<CardInterface>().MyCardDeploy(card);
        }

        return card.GetComponent<CardInterface>().MyCardDeploy(card);
    }
}
