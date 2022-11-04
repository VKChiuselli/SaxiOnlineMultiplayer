using System.Collections;
using System.Collections.Generic;
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
            //TODO push
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
            gameManager.GetComponent<GameManager>().SetIsPopupChoosing(0);
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
        } 
    }




    private void OnDisable()
    {
        _xOldTile = 0;
        _yOldTile = 0;
        _xNewTile = 0;
        _yNewTile = 0;
    }
}
