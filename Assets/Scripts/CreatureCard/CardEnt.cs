using Assets.Scripts;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardEnt : CardInterface
{
    GameObject gameManager;
    GameObject gridContainer;
    PlaceManager placeManager;

    void Start()
    {
        gameManager = GameObject.Find("Managers/GameManager");
        keyword1 = CardKeyword.MERGE;
        keyword2 = CardKeyword.EFFECT;
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
        GameObject card = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        card.GetComponent<CardTable>().CurrentSpeed.Value = card.GetComponent<CardTable>().CurrentSpeed.Value + 1;
    }

    public override void MyCardEffect()
    {
        Debug.Log("Add a the creature to my hand");

        MyCardEffectClientRpc();

    }

    [ClientRpc] //TODO improve connection with client, I must understand which and how the client is called from the server, we need a specific client, not every client 
    void MyCardEffectClientRpc()
    {
        if (IsClient)
        {
            List<GameObject> list = gridContainer.GetComponent<GridContainer>().GetAllTopCardsFromPlayer(gameManager.GetComponent<GameManager>().CurrentTurn.Value);
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
