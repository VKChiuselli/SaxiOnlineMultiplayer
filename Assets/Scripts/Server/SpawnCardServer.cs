using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnCardServer : NetworkBehaviour
{

    PlaceManager placeManager;
    GameObject gridContainer;
    GameObject gameManager;
    GameObject deckManager;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
        deckManager = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
    }


    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeployServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, int x, int y, int deployCost) //MyCardStruct cartaDaSpawnare
    {
        if (CheckDeploy(deployCost))
        {
            return;
        }
        GameObject cardToSpawn = gridContainer.GetComponent<GridContainer>().GetTile(x, y);
        NetworkObject cardToSpawnNetwork = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
        transform.position, Quaternion.identity);
        cardToSpawnNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        cardToSpawnNetwork.transform.SetParent(cardToSpawn.transform, false);

        cardToSpawnNetwork.GetComponent<CardTable>().IdCard.Value = IdCard;
        cardToSpawnNetwork.GetComponent<CardTable>().Weight.Value = Weight;
        cardToSpawnNetwork.GetComponent<CardTable>().Speed.Value = Speed;
        cardToSpawnNetwork.GetComponent<CardTable>().IdOwner.Value = IdOwner;
        cardToSpawnNetwork.GetComponent<CardTable>().IdImageCard.Value = IdImageCard;
        cardToSpawnNetwork.GetComponent<CardTable>().CurrentPositionX.Value = x;
        cardToSpawnNetwork.GetComponent<CardTable>().CurrentPositionY.Value = y;
        cardToSpawnNetwork.GetComponent<NetworkObject>().tag = tag;
        cardToSpawnNetwork.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        cardToSpawnNetwork.transform.localPosition = new Vector3(0.5f, 0.5f, 1f);

        GameObject cardInterface = deckManager.GetComponent<DeckLoad>().GetCard(0);//TODO instead of put 0, I must put the number of card, the zero it will be card Dog(0), ent(1), dragon(2) etc

        NetworkObject cardInterfaceNetwork = Instantiate(cardInterface.GetComponent<NetworkObject>(),
          cardToSpawnNetwork.transform.position, Quaternion.identity);
        cardInterfaceNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        cardInterfaceNetwork.transform.SetParent(cardToSpawnNetwork.transform, false);

        gameManager.GetComponent<GameManager>().DeployPointSpent(deployCost);
    }

    private bool CheckDeploy(int deployCost)
    {
        if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
        {
            if (gameManager.GetComponent<GameManager>().PlayerZeroDP.Value >= deployCost)
            {
                return false;
            }
        }
        else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
        {
            if (gameManager.GetComponent<GameManager>().PlayerOneDP.Value >= deployCost)
            {
                return false;
            }
        }

        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeployMergeServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, int x, int y, int deployCost) //MyCardStruct cartaDaSpawnare
    {
        if (CheckDeploy(deployCost))
        {
            return;
        }
        GameObject cardToSpawn = gridContainer.GetComponent<GridContainer>().GetTile(x, y);
        NetworkObject cardToSpawnNetwork = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
        transform.position, Quaternion.identity);
        cardToSpawnNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        cardToSpawnNetwork.transform.SetParent(cardToSpawn.transform, false);

        cardToSpawnNetwork.GetComponent<CardTable>().IdCard.Value = IdCard;
        cardToSpawnNetwork.GetComponent<CardTable>().Weight.Value = Weight;
        cardToSpawnNetwork.GetComponent<CardTable>().Speed.Value = Speed;
        cardToSpawnNetwork.GetComponent<CardTable>().IdOwner.Value = IdOwner;
        cardToSpawnNetwork.GetComponent<CardTable>().IdImageCard.Value = IdImageCard;
        cardToSpawnNetwork.GetComponent<CardTable>().CurrentPositionX.Value = x;
        cardToSpawnNetwork.GetComponent<CardTable>().CurrentPositionY.Value = y;
        cardToSpawnNetwork.GetComponent<NetworkObject>().tag = tag;
        cardToSpawnNetwork.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        cardToSpawnNetwork.transform.localPosition = new Vector3(0.5f, 0.5f, 1f);

        GameObject cardInterface = deckManager.GetComponent<DeckLoad>().GetCard(0);//TODO instead of put 0, I must put the number of card, the zero it will be card Dog(0), ent(1), dragon(2) etc

        NetworkObject cardInterfaceNetwork = Instantiate(cardInterface.GetComponent<NetworkObject>(),
          cardToSpawnNetwork.transform.position, Quaternion.identity);
        cardInterfaceNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        cardInterfaceNetwork.transform.SetParent(cardToSpawnNetwork.transform, false);

        gameManager.GetComponent<GameManager>().DeployPointSpent(deployCost);
        UpdateWeightTopCard(x, y);
    }



    [ServerRpc(RequireOwnership = false)]
    public void DespawnAllCardsFromTileServerRpc(int x, int y)
    {

        List<GameObject> cardsFromTile = gridContainer.GetComponent<GridContainer>().GetAllCardsFromTile(x, y);

        foreach (GameObject card in cardsFromTile)
        {
            card.GetComponent<NetworkObject>().Despawn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveAllCardsToEmptyTileServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
        int totalMove = CheckMove(xOldTile, yOldTile);

        if (totalMove==0)
        {
            return;
        }
     

        GameObject tileWhereToSpawn = gridContainer.GetComponent<GridContainer>().GetTile(xNewTile, yNewTile);
        if (tileWhereToSpawn == null)
        {//this IF is made for PUSH 
            Debug.Log("card destroied because no tile found");
            DespawnAllCardsFromTileServerRpc(xOldTile, yOldTile);
            return;
        }
        List<GameObject> cardsFromTile = gridContainer.GetComponent<GridContainer>().GetAllCardsFromTile(xOldTile, yOldTile);
        foreach (GameObject card in cardsFromTile)
        {
            card.transform.SetParent(tileWhereToSpawn.transform, false);
            card.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
            card.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;
        }

        gameManager.GetComponent<GameManager>().MovePointSpent(totalMove);
    }

    private int CheckMove(int xOldTile, int yOldTile)
    {
 return       gridContainer.GetComponent<GridContainer>().GetTotalMoveCostOnTile(xOldTile, yOldTile);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveToFriendlyTileServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
        int totalMove = CheckMove(xOldTile, yOldTile);

        if (totalMove == 0)
        {
            return;
        }

        GameObject tileWhereToSpawn = gridContainer.GetComponent<GridContainer>().GetTile(xNewTile, yNewTile);
        if (tileWhereToSpawn == null)
        {
            Debug.Log("card destroied because no tile found");
            DespawnAllCardsFromTileServerRpc(xOldTile, yOldTile);
            return;
        }
        List<GameObject> cardsFromTile = gridContainer.GetComponent<GridContainer>().GetAllCardsFromTile(xOldTile, yOldTile);
        foreach (GameObject card in cardsFromTile)
        {
            card.transform.SetParent(tileWhereToSpawn.transform, false);
            card.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
            card.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;
        }

        UpdateWeightTopCard(xNewTile, yNewTile);
        gameManager.GetComponent<GameManager>().MovePointSpent(totalMove);
    }


    [ServerRpc(RequireOwnership = false)]
    public void MoveTopCardToAnotherTileServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
    //TODO improve code with cards here
        GameObject topCard = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile);
        GameObject newTile = gridContainer.GetComponent<GridContainer>().GetTile(xNewTile, yNewTile);
        topCard.transform.SetParent(newTile.transform, false);
        topCard.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
        topCard.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;

        UpdateWeightTopCard(xOldTile, yOldTile);
        UpdateWeightTopCard(xNewTile, yNewTile);

        gameManager.GetComponent<GameManager>().MovePointSpent(1);
    }

    private void UpdateWeightTopCard(int x, int y)
    {
        int finalWeight = gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTile(x, y);
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }

}
