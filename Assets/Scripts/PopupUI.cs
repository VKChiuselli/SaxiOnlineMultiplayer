using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [SerializeField] GameObject pushButton;
    [SerializeField] GameObject mergeButton;
    [SerializeField] GameObject cancelButton;

    int _xOldTile;
    int _yOldTile;
    int _xNewTile;
    int _yNewTile;
    int _typeOfTile;
    PlaceManager placeManager;
    GameObject gridContainer;
    GameObject gameManager;
    GameObject deckManager;
    GameObject SpawnManager;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
        SpawnManager = GameObject.Find("Managers/SpawnManager");
        deckManager = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
    }


    private void OnEnable()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => button_pressed(0));
        gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => button_pressed(1));
        gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => button_pressed(2));
    }

    private void button_pressed(int numberAction)
    {
        if (numberAction == 0)
        {
            Debug.Log("TODO push");
            bool result = PushCardFromTable(_xOldTile, _yOldTile, _xNewTile, _yNewTile);
            if (result)
            {
                Debug.Log("Card pushed from UI!");
            }
            else
            {
                Debug.Log("Card not pushed from UI!");
            }
            gameObject.SetActive(false);
        }
        else
        if (numberAction == 1)
        {
            Debug.Log("TODO merge/move");
            SpawnManager.GetComponent<SpawnCardServer>().MoveToFriendlyTileServerRpc(_xOldTile, _yOldTile, _xNewTile, _yNewTile);
            gameObject.SetActive(false);
        }
        else
        if (numberAction == 2)
        {
            Debug.Log("TODO cancel");
            gameObject.SetActive(false);
        }
    }

    public void FunctionAvaiable()
    {

    }

    public void InitializeVariables(int xOldTile, int yOldTile, int xNewTile, int yNewTile, int typeOfTile)
    {
        _xOldTile = xOldTile;
        _yOldTile = yOldTile;
        _xNewTile = xNewTile;
        _yNewTile = yNewTile;
        _typeOfTile = typeOfTile;

        if (typeOfTile == 1)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (typeOfTile == 2)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (typeOfTile == 3)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _xOldTile = 0;
        _yOldTile = 0;
        _xNewTile = 0;
        _yNewTile = 0;
    }


    private bool PushCardFromTable(int xOldTile, int yOldTile, int xNewTile, int yNewTile)
    {
        if (xOldTile == 0 && yOldTile == 0 && xNewTile == 0 && yNewTile == 0)
        {
            return false;
        }
        CardTable cardPusher = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xOldTile, yOldTile).GetComponent<CardTable>();
        CardTable cardPushed = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(xNewTile, yNewTile).GetComponent<CardTable>();

        int weightFriendlyCard = cardPusher.MergedWeight.Value == 0 ? cardPusher.Weight.Value : cardPusher.MergedWeight.Value;
        int weightEnemyCard = cardPushed.MergedWeight.Value == 0 ? cardPushed.Weight.Value : cardPushed.MergedWeight.Value;
        if (weightFriendlyCard <= weightEnemyCard)
        {
            return false;
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
            return false;
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
                SpawnManager.GetComponent<SpawnCardServer>().MoveAllCardsToEmptyTileServerRpc(
                    tile.GetComponent<CoordinateSystem>().x, tile.GetComponent<CoordinateSystem>().y,
                    tile.GetComponent<CoordinateSystem>().x + x, tile.GetComponent<CoordinateSystem>().y + y
                    );
            }
            //I move the card that pushed the other cards
            SpawnManager.GetComponent<SpawnCardServer>().MoveAllCardsToEmptyTileServerRpc(
                  xOldTile,
                  yOldTile,
                  xNewTile,
                  yNewTile
                );
        }
        else
        {
            Debug.Log("ERROR! no card added in the list to be pushed!");
        }


        return true;

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


    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
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
