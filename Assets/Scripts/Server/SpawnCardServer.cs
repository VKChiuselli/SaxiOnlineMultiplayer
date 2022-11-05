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
    public void MoveAllCardsToEmptyTileServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile, bool isPushed)
    {
        int totalMove = CheckMove(xOldTile, yOldTile);

        if (!isPushed)
        {
            if (totalMove == 0)
            {
                return;
            }
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

        if (!isPushed)
        {
            gameManager.GetComponent<GameManager>().MovePointSpent(totalMove);
        }
    }

    private int CheckMove(int xOldTile, int yOldTile)
    {
        return gridContainer.GetComponent<GridContainer>().GetTotalMoveCostOnTile(xOldTile, yOldTile);
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



    public int PushCardFromTable(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
        if (xOldTile == 0 && yOldTile == 0 && xNewTile == 0 && yNewTile == 0)
        {
            return 0;
        }
        CardTable cardPusher = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile).GetComponent<CardTable>();
        CardTable cardPushed = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xNewTile, yNewTile).GetComponent<CardTable>();

        int weightFriendlyCard = cardPusher.MergedWeight.Value == 0 ? cardPusher.Weight.Value : cardPusher.MergedWeight.Value;
        int weightEnemyCard = cardPushed.MergedWeight.Value == 0 ? cardPushed.Weight.Value : cardPushed.MergedWeight.Value;
        if (weightFriendlyCard <= weightEnemyCard)
        {
            return 0;
        }

        int check = CheckBehindCard(
                  xOldTile,
                  yOldTile,
                  xNewTile,
                  yNewTile,
                  weightFriendlyCard,
                  weightEnemyCard);

        if (check == 505)
        {
            return 0;
        }



        List<GameObject> tilesToPushList = new List<GameObject>();
        List<GameObject> tilesToPush = new List<GameObject>();
        tilesToPush = FindAllCardsToPush(
                  xOldTile,
                  yOldTile,
                  xNewTile,
                  yNewTile,
                  weightFriendlyCard,
                  weightEnemyCard,
                  tilesToPushList);

        int x = xNewTile - xOldTile;
        int y = yNewTile - yOldTile;

        tilesToPush.Reverse();

        if (tilesToPush != null)
        {
            //I push the cards less the pusher
            foreach (GameObject tile in tilesToPush)
            {
                 MoveAllCardsToEmptyTileServerRpc(
                    tile.GetComponent<CoordinateSystem>().x, tile.GetComponent<CoordinateSystem>().y,
                    tile.GetComponent<CoordinateSystem>().x + x, tile.GetComponent<CoordinateSystem>().y + y,
                    true
                    );
            }
            //I move the card that pushed the other cards
           MoveAllCardsToEmptyTileServerRpc(
                  xOldTile,
                  yOldTile,
                  xNewTile,
                  yNewTile,
                  false
                );
        }
        else
        {
            Debug.Log("ERROR! no card added in the list to be pushed!");
        }

        return gridContainer.GetComponent<GridContainer>().GetTile(xNewTile, yNewTile).transform.childCount;
    }

    private int CheckBehindCard(int xPusher, int yPusher, int xPushed, int yPushed, int weightFriendly, int weightEnemy)
    {
        int x = xPushed - xPusher;
        int y = yPushed - yPusher;
        if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 5)
        {
            return 400; //400 � VERO
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 1)
        {
            return 400; //400 � VERO
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 2)
        {
            int nextCardWeight = gridContainer.GetComponent<GridContainer>().GetNextTileWeight(xPusher + x, yPusher + y, xPushed + x, yPushed + y);
            int totalWeight = nextCardWeight + weightEnemy;
            if (totalWeight >= weightFriendly)
            {
                Debug.Log("too much weight, we can't push it");
                return 505; //505 � FALSO
            }

            return CheckBehindCard(xPusher + x, yPusher + y, xPushed + x, yPushed + y, weightFriendly, totalWeight);
        }

        return 505;
    }




    private List<GameObject> FindAllCardsToPush(int xPusher, int yPusher, int xPushed, int yPushed, int weightFriendly, int weightEnemy, List<GameObject> tilesToPush)
    {
        int x = xPushed - xPusher;
        int y = yPushed - yPusher;
        if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 5)
        {
            return tilesToPush;
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 1)
        {
            return tilesToPush;
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 2)
        {
            int nextCardWeight = gridContainer.GetComponent<GridContainer>().GetNextTileWeight(xPusher + x, yPusher + y, xPushed + x, yPushed + y);
            int totalWeight = nextCardWeight + weightEnemy;
            if (totalWeight >= weightFriendly)
            {
                Debug.Log("we shold not enter here becauise FindAllCardsToPush means that all cards will be pushed!");
                return tilesToPush;
            }

            GameObject tileToAdd = gridContainer.GetComponent<GridContainer>().GetTile(xPushed, yPushed);
            tilesToPush.Add(tileToAdd);

            return FindAllCardsToPush(xPusher + x, yPusher + y, xPushed + x, yPushed + y, weightFriendly, totalWeight, tilesToPush);
        }

        return tilesToPush;
    }

}
