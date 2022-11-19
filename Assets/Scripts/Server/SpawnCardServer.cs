using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnCardServer : NetworkBehaviour
{

    GameObject gridContainer;
    GameObject gameManager;
    GameObject deckManagerRight;
    GameObject deckManagerLeft;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
        deckManagerRight = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
        deckManagerLeft = GameObject.Find("CanvasHandPlayer/PanelPlayerLeft");
    }


    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeployServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard,
        string tag, int x, int y, int deployCost, int Copies, int CardPosition)
    {


        DeckLoad deckLoad = null;

        if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
        {
            deckLoad = deckManagerRight.GetComponent<DeckLoad>();
        }
        else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
        {
            deckLoad = deckManagerLeft.GetComponent<DeckLoad>();
        }

        GameObject cardInterface = deckLoad.GetIndexCard(CardPosition);

        if (!(gameManager.GetComponent<TriggerCardManager>().TriggerDeployCondition(cardInterface)))
        {
            return;
        }

        if (CheckDeploy(deployCost))
        {
            return;
        }
        if (CheckEnoughCopies(Copies))
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

       

        NetworkObject cardInterfaceNetwork = Instantiate(cardInterface.GetComponent<NetworkObject>(),
          cardToSpawnNetwork.transform.position, Quaternion.identity);
        cardInterfaceNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        cardInterfaceNetwork.transform.SetParent(cardToSpawnNetwork.transform, false);
        CardHand cardToRemoveCopy = deckLoad.GetCardHand(IdCard);
        cardToRemoveCopy.PlayCard();
        gameManager.GetComponent<GameManager>().DeployPointSpent(deployCost);

        gameManager.GetComponent<TriggerCardManager>().TriggerETBEffect(cardInterface);
    }

    private bool CheckEnoughCopies(int copies)
    {
        if (copies == 0)
        {
            return true;
        }
        return false;
    }

    private bool CheckDeploy(int deployCost)
    {

        //if (!(gridContainer.GetComponent<GridContainer>().ExistHalfBoardCard(gameManager.GetComponent<GameManager>().CurrentTurn.Value)))
        //{
        //    return false;
        //}

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
    public void DeployMergeServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, int x, int y, int deployCost, int Copies, int CardPosition) //MyCardStruct cartaDaSpawnare
    {


        DeckLoad deckLoad = null;

        if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
        {
            deckLoad = deckManagerRight.GetComponent<DeckLoad>();
        }
        else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)
        {
            deckLoad = deckManagerLeft.GetComponent<DeckLoad>();
        }

        GameObject cardInterface = deckLoad.GetIndexCard(CardPosition);

        if (!(gameManager.GetComponent<TriggerCardManager>().TriggerDeployCondition(cardInterface)))
        {
            return;
        }


        if (CheckDeploy(deployCost))
        {
            return;
        }
        if (CheckEnoughCopies(Copies))
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

        //    GameObject cardInterface = deckManager.GetComponent<DeckLoad>().GetCardGameObject(IdCard).transform.GetChild(8).gameObject; //it is child 8 because the card is putted there
     

        NetworkObject cardInterfaceNetwork = Instantiate(cardInterface.GetComponent<NetworkObject>(),
          cardToSpawnNetwork.transform.position, Quaternion.identity);
        cardInterfaceNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        cardInterfaceNetwork.transform.SetParent(cardToSpawnNetwork.transform, false);


        CardHand cardToRemoveCopy = deckLoad.GetComponent<DeckLoad>().GetCardHand(IdCard);
        cardToRemoveCopy.PlayCard();
        gameManager.GetComponent<GameManager>().DeployPointSpent(deployCost);
        gameManager.GetComponent<TriggerCardManager>().TriggerMergeEffect(cardInterface);

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
        int totalSpeed = CheckSpeed(xOldTile, yOldTile);
        if (!isPushed)
        {
            if (totalMove > gameManager.GetComponent<GameManager>().GetCurrentPlayerMovePoint())
            {
                Debug.Log("Not enough Move Points");
                return;
            }
            else if (totalSpeed == 0)
            {
                Debug.Log("Not enough speed");
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
            RemoveSpeedCard(xNewTile, yNewTile);
            gameManager.GetComponent<GameManager>().MovePointSpent(totalMove);
        }
    }


    public int CheckMove(int xOldTile, int yOldTile)
    {
        return gridContainer.GetComponent<GridContainer>().GetTotalMoveCostOnTile(xOldTile, yOldTile);
    }
    private int CheckMoveTopCard(int xOldTile, int yOldTile)
    {
        return gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile).GetComponent<CardTable>().MoveCost.Value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveToFriendlyTileServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
        int totalMove = CheckMove(xOldTile, yOldTile);
        int totalSpeed = CheckSpeed(xOldTile, yOldTile);

        if (totalMove > gameManager.GetComponent<GameManager>().GetCurrentPlayerMovePoint())
        {
            Debug.Log("Not enough Move Points");
            return;
        }
        if (totalSpeed == 0)
        {
            Debug.Log("Not enough speed");
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
        Debug.Log("xNewTile, yNewTile" + xNewTile + " " + yNewTile);
        GameObject cardTable = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xNewTile, yNewTile);
        gameManager.GetComponent<TriggerCardManager>().TriggerMergeEffect(cardTable.transform.GetChild(2).gameObject);
        RemoveSpeedCard(xNewTile, yNewTile);
    }


    [ServerRpc(RequireOwnership = false)]
    public void MoveTopCardToAnotherTileServerRpc(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
        int totalMove = CheckMoveTopCard(xOldTile, yOldTile);
        int totalSpeed = CheckSpeed(xOldTile, yOldTile);
        if (totalMove > gameManager.GetComponent<GameManager>().GetCurrentPlayerMovePoint())
        {
            Debug.Log("Not enough Move Points");
            return;
        }
        if (totalSpeed == 0)
        {
            Debug.Log("Not enough speed");
            return;
        }
        //TODO improve code with cards here
        GameObject topCard = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile);
        GameObject newTile = gridContainer.GetComponent<GridContainer>().GetTile(xNewTile, yNewTile);
        topCard.transform.SetParent(newTile.transform, false);
        topCard.GetComponent<CardTable>().CurrentPositionX.Value = xNewTile;
        topCard.GetComponent<CardTable>().CurrentPositionY.Value = yNewTile;

        UpdateWeightTopCard(xOldTile, yOldTile);
        UpdateWeightTopCard(xNewTile, yNewTile);

        gameManager.GetComponent<GameManager>().MovePointSpent(1);
        RemoveSpeedCard(xNewTile, yNewTile);
    }

    private void UpdateWeightTopCard(int x, int y)
    {
        int finalWeight = gridContainer.GetComponent<GridContainer>().GetTotalWeightOnTile(x, y);
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }

    private int CheckSpeed(int xOldTile, int yOldTile)
    {
        GameObject topCard = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile);

        if (topCard.GetComponent<CardTable>() != null)
        {
            if (topCard.GetComponent<CardTable>().Speed.Value > 0)
            {
                return 1;
            }
        }
        return 0;
    }

    private void RemoveSpeedCard(int xNewTile, int yNewTile)
    {
        GameObject topCard = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xNewTile, yNewTile);
        if (topCard.GetComponent<CardTable>() != null)
        {
            topCard.GetComponent<CardTable>().Speed.Value = topCard.GetComponent<CardTable>().Speed.Value - 1;
        }
    }


    public int PushCardFromTable(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
        if (xOldTile == 0 && yOldTile == 0 && xNewTile == 0 && yNewTile == 0)
        {
            return 0;
        }

        CardTable cardPusher = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile).GetComponent<CardTable>();
        CardTable cardPushed = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xNewTile, yNewTile).GetComponent<CardTable>();

        if (cardPusher.Speed.Value == 0)
        {
            return 0;
        }

        int totalMove = CheckMove(xOldTile, yOldTile);

        if (totalMove > gameManager.GetComponent<GameManager>().GetCurrentPlayerMovePoint())
        {
            return 0;
        }

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
        List<GameObject> tilesToPush = FindAllCardsToPush(
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
        GameObject cardInterface = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile);
        gameManager.GetComponent<TriggerCardManager>().TriggerPushEffect(cardInterface);
        return gridContainer.GetComponent<GridContainer>().GetTile(xNewTile, yNewTile).transform.childCount;
    }

    private int CheckBehindCard(int xPusher, int yPusher, int xPushed, int yPushed, int weightFriendly, int weightEnemy)
    {
        int x = xPushed - xPusher;
        int y = yPushed - yPusher;
        if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 5)
        {
            return 400; //400 è VERO
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 1)
        {
            return 400; //400 è VERO
        }
        else if (gridContainer.GetComponent<GridContainer>().GetNextTileType(xPusher, yPusher, xPushed, yPushed) == 2)
        {
            int nextCardWeight = gridContainer.GetComponent<GridContainer>().GetNextTileWeight(xPusher + x, yPusher + y, xPushed + x, yPushed + y);
            int totalWeight = nextCardWeight + weightEnemy;
            if (totalWeight >= weightFriendly)
            {
                Debug.Log("too much weight, we can't push it");
                return 505; //505 è FALSO
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
